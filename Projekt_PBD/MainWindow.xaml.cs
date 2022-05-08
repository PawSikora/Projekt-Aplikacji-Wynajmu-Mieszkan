using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_PBD
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            //DaneMieszkania x = context.DaneMieszkanias.Where(d => d.idM == 2 ).First();
            //var klient = new Klient() { email = "klient4.com", imie = "Mati", nazwisko = "Bachi", DaneMieszkanias = new List<DaneMieszkania>(){x}, nrKonta = 246810};
            if (txtLogin.Text == "W")
            {
                var owner = context.Wlasciciels.First();
                WlascicielOkno wlasciciel = new WlascicielOkno(owner);
                wlasciciel.ShowDialog();
                Close();
            }
            else if (txtLogin.Text == "K")
            {
                var client = context.Klients.First();
                KlientOkno klient = new KlientOkno(client);
                klient.ShowDialog();
                Close();
            }
        }
    }
}
