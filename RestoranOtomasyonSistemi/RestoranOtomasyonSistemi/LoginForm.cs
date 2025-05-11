using System;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace RestoranOtomasyonSistemi
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;

            txtPassword.Text = "1234";
            txtUsername.Text = "personel1";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (username == "admin" && password == "1234")
            {
                AdminPanel adminPanel = new AdminPanel();
                adminPanel.FormClosed += (s, e) => { this.Show(); };
                adminPanel.Show();
                this.Hide();
                return;
            }

            DataBaseService dbService = ServiceLocator.GetService<DataBaseService>();
            if(dbService.TryPersonelLogin(username, password, out int personelId))
            {
                MasaTakipModule masaTakipModule = new MasaTakipModule(personelId);
                masaTakipModule.FormClosed += (s, e) => { this.Show(); };
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
