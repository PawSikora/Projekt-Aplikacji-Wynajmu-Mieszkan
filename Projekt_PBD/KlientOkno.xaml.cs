using System;
using System.Collections.Generic;
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
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private Klient klient;
        public KlientOkno(Klient klient)
        {
            InitializeComponent();
            this.klient = klient;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbxImie.Text = klient.imie;
            tbxNazwisko.Text = klient.nazwisko;
            var mieszkanieKlienta = klient.DaneMieszkanias.Where(m => m.idK == klient.idK).First();
            tbxDaneMieszkania.AppendText($"ul. {mieszkanieKlienta.Ulica} {mieszkanieKlienta.nrBudynku}/{mieszkanieKlienta.nrMieszkania}\n");
            tbxDaneMieszkania.AppendText($"{mieszkanieKlienta.kodPocztowy} {mieszkanieKlienta.Miasto}");
        }


        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new MainWindow().ShowDialog();
        }
    }
}
