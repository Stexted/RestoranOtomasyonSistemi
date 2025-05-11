using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RestoranOtomasyonSistemi
{
    public enum MasaDurumu
    {
        Bos = 0,
        Dolu = 1
    }

    public partial class MasaTakipModule : Form
    {
        private int personelId = 0;
        private DataBaseService databaseService;

        public MasaTakipModule(int personelId)
        {
            InitializeComponent();
            databaseService = ServiceLocator.GetService<DataBaseService>();
            this.personelId = personelId;
            LoadMasalar();
        }


        private void MasaTakipModule_Load(object sender, EventArgs e)
        {

        }

        private void Masa_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int masaID = (int)btn.Tag;

            new FoodOrderModule(masaID, personelId, this).Show();
        }

        public void UpdateMasalar()
        {
            List<(int MasaID, MasaDurumu Durum)> masalar = databaseService.GetAllTables();
            GetMasaButtons().ForEach(button =>
            {
                int masaID = (int)button.Tag;
                var masa = masalar.FirstOrDefault(m => m.MasaID == masaID);
                if (masa != default)
                {
                    button.BackColor = masa.Durum == MasaDurumu.Bos ? Color.GreenYellow : Color.Red;
                }
            });
        }

        private List<Button> GetMasaButtons()
        {
            List<Button> buttons = new List<Button>();
            foreach (Control control in this.Controls)
            {
                if (control is Button && control.Name.StartsWith("masa"))
                {
                    buttons.Add((Button)control);
                }
            }
            return buttons;
        }

        private void LoadMasalar()
        {
            List<(int MasaID, MasaDurumu Durum)> masalar = databaseService.GetAllTables(); 

            int x = 20, y = 20;
            int buttonWidth = 150, buttonHeight = 80;
            int margin = 20;
            int columnCount = 3;

            int counter = 0;

            foreach (var masa in masalar)
            {
                Button btn = new Button();
                btn.Name = $"masa{masa.MasaID}";
                btn.Text = $"Masa {masa.MasaID}";
                btn.Size = new Size(buttonWidth, buttonHeight);
                btn.Font = new Font("Segoe UI", 12);
                btn.Tag = masa.MasaID;

                switch (masa.Durum)
                {
                    case MasaDurumu.Bos:
                        btn.BackColor = Color.GreenYellow;
                        break;
                    case MasaDurumu.Dolu:
                        btn.BackColor = Color.Red;
                        break;
                    default:
                        btn.BackColor = Color.GreenYellow;
                        break;
                }

                btn.Location = new Point(x + (counter % columnCount) * (buttonWidth + margin),
                                         y + (counter / columnCount) * (buttonHeight + margin));

                btn.Click += Masa_Click;

                this.Controls.Add(btn);

                counter++;
            }
        }
    }
}
