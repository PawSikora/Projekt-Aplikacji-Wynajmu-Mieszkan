using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Projekt_PBD
{
    /// <summary>
    /// Logika interakcji dla klasy KlientOkno.xaml
    /// </summary>
    public partial class KlientOkno : Window
    {
        private Klient klient;
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        public KlientOkno(Klient klient)
        {
            InitializeComponent();
            this.klient = klient;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbxImie.Text = klient.imie;
            tbxNazwisko.Text = klient.nazwisko;
            var mieszkanieKlienta = klient.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();
            if(mieszkanieKlienta != null)
            { 
                tbxDaneMieszkania.AppendText($"ul. {mieszkanieKlienta.Ulica} {mieszkanieKlienta.nrBudynku}/{mieszkanieKlienta.nrMieszkania}\n");
                tbxDaneMieszkania.AppendText($"{mieszkanieKlienta.kodPocztowy} {mieszkanieKlienta.Miasto}");
            }
            if (klient.DaneMieszkanias.Where(k => k.idK == klient.idK).FirstOrDefault() == null) btnWypowiedzUmowe.IsEnabled = false;
        }


        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void btnOfertyWynajmu_Click(object sender, RoutedEventArgs e)
        {
            new OfertyWynajmuOkno(klient).ShowDialog();
        }

        private void btnWypowiedzUmowe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult m = MessageBox.Show("Czy na pewno chcesz wypowiedzieć umowę?",
                "Wypowiedzenie umowy", MessageBoxButton.YesNo);
            if (m == MessageBoxResult.Yes)
            {
                var mieszkanieDoWypowiedzenia = context.DaneMieszkanias
                    .Where(mdw => mdw.idK == klient.idK).FirstOrDefault();
                if (mieszkanieDoWypowiedzenia != null)
                {
                    mieszkanieDoWypowiedzenia.koniecWynajmu = DateTime.Today.AddMonths(1);
                    context.SaveChanges();
                    MessageBox.Show("WYPOWIEDZIANO UMOWĘ!");
                }
            }
        }

        private void btnHistoriaWpłat_Click(object sender, RoutedEventArgs e)
        {
            HistoriaOkno okno = new HistoriaOkno(klient);
            this.Close();
            okno.ShowDialog();
        }
    }
}
