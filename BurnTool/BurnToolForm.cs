//#define U31_DBG

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace BurnTool
{
    public partial class BurnToolForm : Form
    {        
        private ChipConfig[] cfgs;
        public FileStream fdata;
        public PartitionConfig partitionConfig;
        private UIConfig uiConfig;
    
        public BurnToolForm(ChipConfig[] _cfgs)
        {
            this.cfgs = _cfgs;
            partitionConfig = new PartitionConfig();
            uiConfig = new UIConfig();
            uiConfig.load();            
            InitializeComponent();
            foreach(ChipConfig cfg in this.cfgs)
            {
                this.productCombo.Items.Add(cfg.Name);
            }
            if (_cfgs.Length > 0)
            {
                this.productCombo.SelectedIndex = 0;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.textFile.Text = uiConfig.uiJson.partitionFile;            
            if (true == File.Exists(partitionConfig.wholebinFname)) {
                this.btnBurn.Enabled = true;
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = System.Environment.CurrentDirectory;
            dialog.Filter = "固件文件(*.json)|*.json";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textFile.Text = dialog.FileName;
                uiConfig.uiJson.chipType = "u31";
                uiConfig.uiJson.partitionFile = dialog.FileName;
                uiConfig.save();                
                if (cfgs.Length > 0)
                    this.btnBurn.Enabled = true;
            }
        }

        private static IICConnection waitingIIC;
        private static UInt32 waitingAddr;
        private static int remainBytes;
        private static bool quit;
        private static void waitingProc()
		{
            quit = false;
            while(!quit)
			{
                remainBytes = waitingIIC.ReadValue(waitingAddr);
                if (remainBytes == 0)
				{
                    break;
                }
                Thread.Sleep(100);
			}
        }

        private void appendLogText(string text)
		{
            this.textLog.Text += text;
            this.textLog.SelectionStart = this.textLog.Text.Length;
            this.textLog.ScrollToCaret();
            this.Update();
        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        private void btnBurn_Click(object sender, EventArgs e)
        {
            this.btnView.Enabled = false;
            this.btnBurn.Enabled = false;
            this.productCombo.Enabled = false;
            var cfg = this.cfgs[this.productCombo.SelectedIndex];            
            this.progressBar1.Value = 0;

            UInt32 reg_pcflag = 0x0901002c;

            UInt32 dev_flash_mode = 0x09010030;
            UInt32 dev_flash_time = 0x09010034;
            UInt32 dev_flash_addr = 0x09010038;
            UInt32 dev_flash_buf  = 0x0901003c;
            UInt32 dev_flash_cmd  = 0x09010040;

            UInt32 val_flash_mode = 0x00f105b0;
            UInt32 val_flash_time = 0x0000020e;
            UInt32 val_flash_addr = 0x00000000;//flash偏移地址
            UInt32 val_flash_buf  = 0x00001000;//U31缓存地址
            UInt32 val_flash_cmd  = 0;//擦除256B

            UInt32 reg_cpu_halt = 0x0901ff04;//=1 cpu hlt
            UInt32 reg_boot_select = 0x0901ff08;//=2 设置 bootflag=1 然后从rom启动,=1 从内存启动

            try
            {
                fdata = File.OpenRead(partitionConfig.wholebinFname);
                fdata.Position = 0;
                this.appendLogText(fdata.Length.ToString() + "字节待烧录\r\n");
            }
            catch (Exception ex)
            {
                this.appendLogText("wholebin打开失败\r\n");
                return;
            }

            try
			{
                this.appendLogText("IIC连接中...\r\n");
                IICConnection iic = new IICConnection(cfg.Endian == ChipConfig.EndianType.Big, cfg.I2CAddr);

#if U31_DBG
#else
                iic.WriteValue(reg_cpu_halt, (int)1);//cpu hlt
                iic.WriteValue(reg_boot_select, (int)2);//set bootflag then boot from rom
#endif
                Int32 bootflag = iic.ReadValue(0x0900000c);//get bootflag

                //Delay(50);//wait cpu reset

                //Int32 pcflag1 = iic.ReadValue(0x0901ff00);

                Int32 pcflag = iic.ReadValue(reg_pcflag);

                if (pcflag != 0)
                {
                    //------------------擦除flash
                    Stopwatch sw = new Stopwatch();
                    UInt32 fdataLen = (UInt32)fdata.Length;
                    UInt32[] eraseUnits = { 65536, 32768, 4096, 256 };
                    UInt32[] eraseCmds = { 0xd8800000, 0x52800000, 0x20800000, 0x81800000 };//擦62K，32K，4K，256
                    UInt32 eraseCnt = 0;
                    UInt32 wrAvailableLen = fdataLen- fdataLen % 256;
                    sw.Start();

                    this.appendLogText("正在擦除flash,大小="+ wrAvailableLen + "\r\n");
                    for (int n = 0; n < 4; n++)
                    {
                        eraseCnt = fdataLen / eraseUnits[n];
                        for (int i = 0; i < eraseCnt; i++)
                        {
                            iic.WriteValue(dev_flash_mode, (int)val_flash_mode);
                            iic.WriteValue(dev_flash_time, (int)val_flash_time);
                            iic.WriteValue(dev_flash_addr, (int)val_flash_addr);
                            iic.WriteValue(dev_flash_cmd, (int)eraseCmds[n]);
                            iic.WriteValue(reg_pcflag, (int)0);
                            val_flash_addr += eraseUnits[n];
                            while (0 == pcflag)
                            {
                                pcflag = iic.ReadValue(reg_pcflag);
                                Delay(10);
                            }
                        }
                        fdataLen = fdataLen % eraseUnits[n];
                    }

                    //------------------擦除flash
                    this.appendLogText("正在写入flash,大小=" + wrAvailableLen + "\r\n");
                    
                    int wrlen = 0;
                    fdataLen = (UInt32)wrAvailableLen;
                    val_flash_addr = 0;

                    UInt32 chipbuf = val_flash_buf;
                    this.progressBar1.Value = 0;
                    while (wrlen < (int)fdata.Length)
                    {
                        byte[] wrbuf = new byte[256];
                        int len = fdata.Read(wrbuf, 0, 256);
                        val_flash_cmd = 0x02800100;
                        iic.Write(chipbuf, wrbuf, len);

                        iic.WriteValue(dev_flash_mode, (int)val_flash_mode);
                        iic.WriteValue(dev_flash_time, (int)val_flash_time);
                        iic.WriteValue(dev_flash_addr, (int)val_flash_addr);
                        iic.WriteValue(dev_flash_buf, (int)chipbuf);
                        iic.WriteValue(dev_flash_cmd, (int)val_flash_cmd);
                        iic.WriteValue(reg_pcflag, (int)0);

                        chipbuf += (UInt32)len;
                        val_flash_addr+= (UInt32)len;
                        wrlen += len;                        
                        while (0 == pcflag)
                        {
                            pcflag = iic.ReadValue(reg_pcflag);
                            Delay(10);
                        }
                        this.progressBar1.Value = wrlen*100/(int)fdata.Length;
                        //this.appendLogText("总大小="+ fdata.Length+",已烧录="+ wrlen+"\r\n");
                    }
                    sw.Stop();
                    //------------------重启CPU
                    this.appendLogText("烧录完成,"+"耗时:"+ sw.Elapsed+"ms,CPU重启\r\n");
#if U31_DBG
#else
                    iic.WriteValue(reg_cpu_halt, (int)1);//cpu hlt
                    iic.WriteValue(reg_boot_select, (int)1);//cpu reset
#endif
                }
                else {
                    this.appendLogText("\r\nCPU未进烧录模式\r\n");
                }                    
            }

            catch (Exception ex)
			{
                this.appendLogText(ex.Message);
                this.appendLogText("\r\n======烧录失败!======\r\n");
            }
            fdata.Close();

            this.btnView.Enabled = true;
            this.btnBurn.Enabled = true;
            this.productCombo.Enabled = true;
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            var cfg = this.cfgs[this.productCombo.SelectedIndex];

            UInt32 reg_pcflag = 0x0901002c;

            UInt32 dev_flash_mode = 0x09010030;
            UInt32 dev_flash_time = 0x09010034;
            UInt32 dev_flash_addr = 0x09010038;
            UInt32 dev_flash_buf = 0x0901003c;
            UInt32 dev_flash_cmd = 0x09010040;

            UInt32 val_flash_mode = 0x00f105b0;
            UInt32 val_flash_time = 0x0000020e;
            UInt32 val_flash_addr = 0x00000000;//flash偏移地址
            UInt32 val_flash_buf = 0x00001000;//U31缓存地址
            UInt32 val_flash_cmd = 0x03008000;//读32KB

            UInt32 reg_cpu_halt = 0x0901ff04;//=1 cpu hlt
            UInt32 reg_boot_select = 0x0901ff08;//=2 设置 bootflag=1 然后从rom启动,=1 从内存启动

            UInt32 offsetBegin = 0;
            UInt32 offsetEnd =65536;
            UInt32 sumlen = offsetEnd - offsetBegin;
            UInt32 rdLen = 0;
            UInt32 wrSize = 32768;//每次读32K
            //byte[] wrbuf = new byte[32768];

            this.btnRead.Enabled = false;
            this.progressBar1.Value = 0;

            FlashSaveDialog dialog= new FlashSaveDialog();
            dialog.ShowDialog();

            if (false == dialog.selectedOk())
            {
                this.btnRead.Enabled = true;
                return;
            }
            offsetBegin = dialog.getBeginAddress();
            offsetEnd = offsetBegin+dialog.getReadSize();
            sumlen = offsetEnd - offsetBegin;

            try
            {
                this.appendLogText("IIC连接中...\r\n");
                IICConnection iic = new IICConnection(cfg.Endian == ChipConfig.EndianType.Big, cfg.I2CAddr);

#if U31_DBG
#else
                iic.WriteValue(reg_cpu_halt, (int)1);//cpu hlt
                iic.WriteValue(reg_boot_select, (int)2);//set bootflag then boot from rom
#endif
                Int32 bootflag = iic.ReadValue(0x0900000c);//get bootflag

                //Delay(50);//wait cpu reset

                Int32 pcflag = iic.ReadValue(reg_pcflag);                

                if (pcflag != 0)
                {
                    FileStream wholebinFile = new FileStream("flash_readout.bin", FileMode.Create, FileAccess.Write);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    val_flash_addr = offsetBegin;
                    while (val_flash_addr < offsetEnd) {
                        if (val_flash_addr + wrSize > offsetEnd)
                        {
                            wrSize = offsetEnd - val_flash_addr;
                            val_flash_cmd = 0x03000000 | wrSize;
                        }
                        this.appendLogText("\r\n已读取:"+ this.progressBar1.Value + "%,正在读取:"+ val_flash_addr+"~"+ (val_flash_addr + wrSize)+"\r\n");
                        iic.WriteValue(dev_flash_mode, (int)val_flash_mode);
                        iic.WriteValue(dev_flash_time, (int)val_flash_time);
                        iic.WriteValue(dev_flash_addr, (int)val_flash_addr);
                        iic.WriteValue(dev_flash_buf, (int)val_flash_buf);
                        iic.WriteValue(dev_flash_cmd, (int)val_flash_cmd);
                        iic.WriteValue(reg_pcflag, (int)0);

                        this.appendLogText("芯片正在读取flash\r\n");
                        while (0 == pcflag)//等待读完
                        {
                            pcflag = iic.ReadValue(reg_pcflag);
                            Delay(10);
                        }
                        this.appendLogText("芯片读取flash完成，读取芯片内存\r\n");
                        //read rambuf
                        byte[] wrbuf = iic.Read(val_flash_buf, (int)wrSize);
                        this.appendLogText("芯片内存读取完成，写入文件\r\n");

                        wholebinFile.Write(wrbuf, 0, (int)wrSize);
                        val_flash_addr += wrSize;
                        rdLen += wrSize;
                        this.progressBar1.Value = (int)(rdLen * 100 / sumlen);
                    }
                    wholebinFile.Close();
                    sw.Stop();
                    this.appendLogText("\r\n全部读取完成，总共读取字节:"+ rdLen+",耗时:"+ sw.Elapsed+"\r\n");
                }
                else {
                    this.appendLogText("\r\nCPU未进烧录模式\r\n");
                }
            }
            catch (Exception ex)
            {
                this.appendLogText(ex.Message);
                this.appendLogText("\r\n======读取失败!======\r\n");
            }
            this.btnRead.Enabled = true;
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            if (0 != partitionConfig.loadFile(this.textFile.Text))
            {
                this.appendLogText("分区文件：" + this.textFile.Text + "格式错误\r\n");
                return;
            }
            string retstr = partitionConfig.checkContent();
            if ("ok" != retstr)
            {
                this.appendLogText(retstr + "\r\n");
                return;
            }
            retstr = partitionConfig.wholebinCreate();
            if ("ok" != retstr)
            {
                this.appendLogText(retstr+"\r\n");
                return;
            }
            this.appendLogText("wholebin生成完成，大小="+ partitionConfig.flashSize+"\r\n");
            this.btnBurn.Enabled = true;
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            this.textLog.Text = "";
        }
    }
}
