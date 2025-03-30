namespace RestoranOtomasyonSistemi
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;

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
            this.txtUsername = new TextBox();
            this.txtPassword = new TextBox();
            this.btnLogin = new Button();

            this.SuspendLayout();

            // 
            // txtUsername
            // 
            this.txtUsername.Location = new Point(100, 50);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new Size(200, 20);

            // 
            // txtPassword
            // 
            this.txtPassword.Location = new Point(100, 100);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*'; // Şifreyi gizlemek için
            this.txtPassword.Size = new Size(200, 20);

            // 
            // btnLogin
            // 
            this.btnLogin.Location = new Point(100, 150);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new Size(200, 40);
            this.btnLogin.Text = "Giriş Yap";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new EventHandler(this.btnLogin_Click);

            // 
            // LoginForm
            // 
            this.ClientSize = new Size(400, 250);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Name = "LoginForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Personel Girişi";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
