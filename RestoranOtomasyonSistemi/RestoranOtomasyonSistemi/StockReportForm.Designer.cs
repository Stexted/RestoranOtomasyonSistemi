namespace RestoranOtomasyonSistemi
{
    partial class StockReportForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // StockReportForm
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "StockReportForm";
            this.Load += new System.EventHandler(this.StockReportForm_Load);
            this.ResumeLayout(false);
        }

        private void StockReportForm_Load(object sender, EventArgs e)
        {

        }
    }
}
