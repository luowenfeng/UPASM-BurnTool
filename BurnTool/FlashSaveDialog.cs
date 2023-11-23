using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace BurnTool
{
    public partial class FlashSaveDialog : Form
    {
        private Boolean clickedOk=false;
        public FlashSaveDialog()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            clickedOk = true;
            this.Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public UInt32 getBeginAddress() {
            UInt32 addr =0;
            addr = Convert.ToUInt32(this.beginAddrTB.Text);
            return addr;
        }

        public UInt32 getReadSize()
        {
            UInt32 rdsize = 0;
            rdsize = Convert.ToUInt32(this.rdSizeTB.Text);
            return rdsize;
        }

        public Boolean selectedOk()
        {
            return clickedOk;
        }
    }
}
