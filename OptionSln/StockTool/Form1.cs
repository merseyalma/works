using Investment.Framework.Biz;
using Investment.Framework.DB;
using Investment.Framework.Entity;
using Investment.Framework.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StockTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnJGD_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            string err = BzStock.ImportExchangeList(ref result);

            MessageBox.Show(err == string.Empty ? ("成功" + result + "条") : err);
        }

        private void btnStockPriceImport_Click(object sender, EventArgs e)
        {
            string err = BzStock.ImportStockPrice();
            MessageBox.Show(err == string.Empty ? "完成" : err);
        }

        private void btnProfit_Click(object sender, EventArgs e)
        {

            string err = BzStock.CalculateProfit();

            MessageBox.Show(err == string.Empty ? "完成" : err);
        }

        private void btnSZIndexImport_Click(object sender, EventArgs e)
        {
            string err = BzStock.ImportSZPrice();

            MessageBox.Show(err == string.Empty ? "完成" : err);
        }
    }
}
