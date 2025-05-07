namespace RestoranOtomasyonSistemi
{
    partial class PersonelEkleForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSave;

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
            label1 = new Label();
            label2 = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnSave = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 30);
            label1.Name = "label1";
            label1.Size = new Size(76, 15);
            label1.TabIndex = 0;
            label1.Text = "Kullanıcı Adı:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 80);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 1;
            label2.Text = "Şifre:";
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(120, 30);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(150, 23);
            txtUsername.TabIndex = 2;
            txtUsername.TextChanged += txtUsername_TextChanged;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(120, 80);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(150, 23);
            txtPassword.TabIndex = 3;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(120, 130);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 30);
            btnSave.TabIndex = 4;
            btnSave.Text = "Kaydet";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // PersonelEkleForm
            // 
            ClientSize = new Size(300, 180);
            Controls.Add(btnSave);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "PersonelEkleForm";
            Text = "Personel Ekle";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
