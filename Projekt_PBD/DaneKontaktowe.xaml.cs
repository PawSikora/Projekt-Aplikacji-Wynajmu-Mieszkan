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
    /// Logika interakcji dla klasy DaneKontaktowe.xaml
    /// </summary>
    public partial class DaneKontaktowe : Window
    {
        private Oferta oferta;
        private Klient klient;
        public bool czyAnulowano = false;
        public DaneKontaktowe()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        public DaneKontaktowe(Oferta oferta, Klient klient) : this()
        {
            this.oferta = oferta;
            this.klient = klient;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (tbxDane.Text.Length != 0)
            {
                Baza_wynajmuEntities context = new Baza_wynajmuEntities();
                Zainteresowani zainteresowany = new Zainteresowani();

                zainteresowany.idK = this.klient.idK;
                zainteresowany.daneKontaktowe = tbxDane.Text;
                zainteresowany.idO = this.oferta.idO;

                context.Zainteresowanis.Add(zainteresowany);
                context.SaveChanges();
                Close();
            }
            else
            {
                MessageBox.Show("Brak podanych danych!");
            }
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            czyAnulowano = true;
            Close();
        }
    }
}
