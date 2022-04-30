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

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {
            var context = new Baza_wynajmuEntities();
            DaneMieszkania tmp = new DaneMieszkania();
            DaneMieszkania x = context.DaneMieszkanias.Where(d => d.idM == 2 ).First();
            var klient = new Klient() { email = "klient4.com", imie = "Mati", nazwisko = "Bachi", DaneMieszkanias = new List<DaneMieszkania>(){x}, nrKonta = 246810};
            context.Klients.Add(klient);
            context.SaveChanges();

        }
    }
}
