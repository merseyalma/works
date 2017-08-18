namespace StockTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnJGD = new System.Windows.Forms.Button();
            this.btnStockPriceImport = new System.Windows.Forms.Button();
            this.btnProfit = new System.Windows.Forms.Button();
            this.btnSZIndexImport = new System.Windows.Forms.Button();
            this.btnExportProfit = new System.Windows.Forms.Button();
            this.btnExportJgd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnJGD
            // 
            this.btnJGD.Location = new System.Drawing.Point(22, 12);
            this.btnJGD.Name = "btnJGD";
            this.btnJGD.Size = new System.Drawing.Size(118, 23);
            this.btnJGD.TabIndex = 1;
            this.btnJGD.Text = "股票交割单导入";
            this.btnJGD.UseVisualStyleBackColor = true;
            this.btnJGD.Click += new System.EventHandler(this.btnJGD_Click);
            // 
            // btnStockPriceImport
            // 
            this.btnStockPriceImport.Location = new System.Drawing.Point(158, 12);
            this.btnStockPriceImport.Name = "btnStockPriceImport";
            this.btnStockPriceImport.Size = new System.Drawing.Size(118, 23);
            this.btnStockPriceImport.TabIndex = 2;
            this.btnStockPriceImport.Text = "股票历史价格导入";
            this.btnStockPriceImport.UseVisualStyleBackColor = true;
            this.btnStockPriceImport.Click += new System.EventHandler(this.btnStockPriceImport_Click);
            // 
            // btnProfit
            // 
            this.btnProfit.Location = new System.Drawing.Point(22, 53);
            this.btnProfit.Name = "btnProfit";
            this.btnProfit.Size = new System.Drawing.Size(118, 23);
            this.btnProfit.TabIndex = 3;
            this.btnProfit.Text = "计算股票每日收益";
            this.btnProfit.UseVisualStyleBackColor = true;
            this.btnProfit.Click += new System.EventHandler(this.btnProfit_Click);
            // 
            // btnSZIndexImport
            // 
            this.btnSZIndexImport.Location = new System.Drawing.Point(158, 53);
            this.btnSZIndexImport.Name = "btnSZIndexImport";
            this.btnSZIndexImport.Size = new System.Drawing.Size(118, 23);
            this.btnSZIndexImport.TabIndex = 4;
            this.btnSZIndexImport.Text = "导入上证指数";
            this.btnSZIndexImport.UseVisualStyleBackColor = true;
            this.btnSZIndexImport.Click += new System.EventHandler(this.btnSZIndexImport_Click);
            // 
            // btnExportProfit
            // 
            this.btnExportProfit.Location = new System.Drawing.Point(22, 91);
            this.btnExportProfit.Name = "btnExportProfit";
            this.btnExportProfit.Size = new System.Drawing.Size(118, 23);
            this.btnExportProfit.TabIndex = 5;
            this.btnExportProfit.Text = "导出每日收益";
            this.btnExportProfit.UseVisualStyleBackColor = true;
            this.btnExportProfit.Click += new System.EventHandler(this.btnExportProfit_Click);
            // 
            // btnExportJgd
            // 
            this.btnExportJgd.Location = new System.Drawing.Point(158, 91);
            this.btnExportJgd.Name = "btnExportJgd";
            this.btnExportJgd.Size = new System.Drawing.Size(118, 23);
            this.btnExportJgd.TabIndex = 6;
            this.btnExportJgd.Text = "导出交割单概要";
            this.btnExportJgd.UseVisualStyleBackColor = true;
            this.btnExportJgd.Click += new System.EventHandler(this.btnExportJgd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 168);
            this.Controls.Add(this.btnExportJgd);
            this.Controls.Add(this.btnExportProfit);
            this.Controls.Add(this.btnSZIndexImport);
            this.Controls.Add(this.btnProfit);
            this.Controls.Add(this.btnStockPriceImport);
            this.Controls.Add(this.btnJGD);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "股票统计工具";
            this.ResumeLayout(false);

        }

        #endregion

    
        private System.Windows.Forms.Button btnJGD;
        private System.Windows.Forms.Button btnStockPriceImport;
        private System.Windows.Forms.Button btnProfit;
        private System.Windows.Forms.Button btnSZIndexImport;
        private System.Windows.Forms.Button btnExportProfit;
        private System.Windows.Forms.Button btnExportJgd;
    }
}

