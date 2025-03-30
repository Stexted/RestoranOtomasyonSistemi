using System;
using System.Windows.Forms;

namespace RestoranOtomasyonSistemi
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if ( (username == "admin" && password == "1234") || (username == "personel1" && password == "1234")) 
            {
                this.Hide(); 
                ProductEditForm productEditForm = new ProductEditForm(); 
                productEditForm.Show(); 
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
