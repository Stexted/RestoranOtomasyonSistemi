using System;
using System.Windows.Forms;

namespace RestoranOtomasyonSistemi
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            /*
            MasaTakipModule a = new MasaTakipModule(5);
            a.Show();
            this.Hide();
            */

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;


            if (username == "admin" && password == "1234")
            {
                ProductEditForm productEditForm = new ProductEditForm();
                productEditForm.Show();
                this.Hide();
            }
            else if (username == "personel1" && password == "1234")
            {
                MasaTakipModule masaTakipModule = new MasaTakipModule(1);
                masaTakipModule.Show();
                this.Hide();
            }
            else if (username == "personel2" && password == "1234")
            {
                MasaTakipModule masaTakipModule = new MasaTakipModule(2);
                masaTakipModule.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı kullanıcı adı veya şifre!", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
