using Investment.Framework.Biz;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OptionTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOptionCalculate_Click(object sender, EventArgs e)
        {
          
            DateTime now = DateTime.Now;
            string err = BzOption.CalculateOption();

            MessageBox.Show(err == string.Empty ? "完成" + ((DateTime.Now - now).TotalMilliseconds) : err);
        }
    }
}
