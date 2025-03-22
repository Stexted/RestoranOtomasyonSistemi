namespace RestoranOtomasyonSistemi
{
    partial class ProductEditForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private TextBox txtYemekAdi;
        private TextBox txtFiyat;
        private TextBox txtStok;
        private Button btnEkle;
        private Button btnDuzenle;
        private Button btnSil;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }






        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(894, 638);
            button1.Name = "button1";
            button1.Size = new Size(98, 23);
            button1.TabIndex = 0;
            button1.Text = "DB Çağır";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 128, 128);
            button2.Location = new Point(1012, 638);
            button2.Name = "button2";
            button2.Size = new Size(98, 23);
            button2.TabIndex = 1;
            button2.Text = "DB Sil";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // ProductEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1139, 686);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "ProductEditForm";
            Text = "ProductEditForm";
            Load += ProductEditForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Button button1;
        private Button button2;
    }
}