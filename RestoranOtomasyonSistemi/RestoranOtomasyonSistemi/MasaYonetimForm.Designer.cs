namespace RestoranOtomasyonSistemi
{
    partial class MasaYonetimForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lstMasalar = new ListBox();
            btnMasaEkle = new Button();
            btnMasaSil = new Button();
            SuspendLayout();
            // 
            // lstMasalar
            // 
            lstMasalar.FormattingEnabled = true;
            lstMasalar.ItemHeight = 15;
            lstMasalar.Location = new Point(12, 12);
            lstMasalar.Name = "lstMasalar";
            lstMasalar.Size = new Size(300, 244);
            lstMasalar.TabIndex = 0;
            lstMasalar.SelectedIndexChanged += lstMasalar_SelectedIndexChanged;
            // 
            // btnMasaEkle
            // 
            btnMasaEkle.Location = new Point(330, 12);
            btnMasaEkle.Name = "btnMasaEkle";
            btnMasaEkle.Size = new Size(120, 40);
            btnMasaEkle.TabIndex = 1;
            btnMasaEkle.Text = "Yeni Masa Ekle";
            btnMasaEkle.UseVisualStyleBackColor = true;
            btnMasaEkle.Click += btnMasaEkle_Click;
            // 
            // btnMasaSil
            // 
            btnMasaSil.Location = new Point(330, 70);
            btnMasaSil.Name = "btnMasaSil";
            btnMasaSil.Size = new Size(120, 40);
            btnMasaSil.TabIndex = 2;
            btnMasaSil.Text = "Seçili Masayı Sil";
            btnMasaSil.UseVisualStyleBackColor = true;
            btnMasaSil.Click += btnMasaSil_Click;
            // 
            // MasaYonetimForm
            // 
            ClientSize = new Size(470, 270);
            Controls.Add(btnMasaSil);
            Controls.Add(btnMasaEkle);
            Controls.Add(lstMasalar);
            Name = "MasaYonetimForm";
            Text = "Masa Yönetimi";
            Load += MasaYonetimForm_Load;
            ResumeLayout(false);
        }

        private System.Windows.Forms.ListBox lstMasalar;
        private System.Windows.Forms.Button btnMasaEkle;
        private System.Windows.Forms.Button btnMasaSil;
    }
}
