using asprise_ocr_api;
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
            DateTime now = DateTime.Now;
            string err = BzStock.CalculateProfit();

            MessageBox.Show(err == string.Empty ? "完成" +((DateTime.Now-now).TotalMilliseconds) : err);
        }

        private void btnSZIndexImport_Click(object sender, EventArgs e)
        {
            string err = BzStock.ImportSZPrice();

            MessageBox.Show(err == string.Empty ? "完成" : err);
        }

        private void btnExportProfit_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string err = BzStock.ExportProfit();

            MessageBox.Show(err == string.Empty ? "完成" + ((DateTime.Now - now).TotalMilliseconds) : err);
        }

        private void btnExportJgd_Click(object sender, EventArgs e)
        {
            AspriseOCR.SetUp();
            AspriseOCR ocr = new AspriseOCR();
            ocr.StartEngine("eng", AspriseOCR.SPEED_FASTEST);
            string file = "e:\\2017-03-17.jpg"; // ☜ jpg, gif, tif, pdf, etc.
            string s = ocr.Recognize(file, -1, -1, -1, -1, -1, AspriseOCR.RECOGNIZE_TYPE_ALL, AspriseOCR.OUTPUT_FORMAT_PLAINTEXT);
            
            ocr.StopEngine();


            //DateTime now = DateTime.Now;
            //string err = BzStock.ExportJgd();

            //MessageBox.Show(err == string.Empty ? "完成" + ((DateTime.Now - now).TotalMilliseconds) : err);
        }
    }
}
