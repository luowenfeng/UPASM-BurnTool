using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace BurnTool
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!IICConnection.InitDLL())
			{                
                return;
			}

            List<ChipConfig> cfgs = new List<ChipConfig>();
            DirectoryInfo dir = new DirectoryInfo("./configs");
            if (dir.Exists)
            {
                foreach(FileInfo finfo in dir.EnumerateFiles())
                {
                    try
                    {
                        ChipConfig cfg = ChipConfig.loadConfig(finfo);
                        cfgs.Add(cfg);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BurnToolForm(cfgs.ToArray()));

            IICConnection.DeinitDLL();
        }
    }
}
