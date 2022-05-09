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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_PBD
{
    /// <summary>
    /// Logika interakcji dla klasy OfertyWynajmuOkno.xaml
    /// </summary>
    public partial class OfertyWynajmuOkno : Window
    {
        private Klient klient;
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private List<Oferta> oferty;
        public OfertyWynajmuOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            oferty = context.Ofertas.OrderByDescending(c => c.cenaZaMiesiac).ToList();
            WyświetlOferty();
        }

        void WyświetlOferty()
        {
            lbxOfertyWynajmu.Items.Clear();
            foreach (var o in oferty) { lbxOfertyWynajmu.Items.Add(o); }
        }
        public OfertyWynajmuOkno(Klient klient) : this()
        {
            this.klient = klient;
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lbxOfertyWynajmu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxDaneMieszkania.Clear();
            Oferta oferta = (Oferta)lbxOfertyWynajmu.SelectedItem;
            if (oferta != null)
            {
                tbxDaneMieszkania.Text += $"Miasto: {oferta.DaneMieszkania.Miasto}\tUlica: {oferta.DaneMieszkania.Ulica} {oferta.DaneMieszkania.nrBudynku}/{oferta.DaneMieszkania.nrMieszkania} \n";
                tbxDaneMieszkania.Text += $"Data wystawienia: {oferta.dataWystawienia:d} \nMetraż: {oferta.metraz}m2 \nCena za wynajęcie: {oferta.cenaZaMiesiac:C} \nWyposażenie: {oferta.wyposazenie}\nOpis: {oferta.opis}";
            }
            
        }

        private void btnFiltruj_Click(object sender, RoutedEventArgs e)
        {
            if (tbxMiasto.Text.Length > 0) oferty = context.Ofertas.Where(m => m.DaneMieszkania.Miasto.StartsWith(tbxMiasto.Text)).ToList();
            if (tbxUlica.Text.Length > 0) oferty = oferty.Where(u => u.DaneMieszkania.Ulica.StartsWith(tbxUlica.Text)).ToList();
            if (tbxCenaMinimalna.Text.Length > 0)
            {
                var cenaMinimalna = Convert.ToDecimal(tbxCenaMinimalna.Text);
                oferty = oferty.Where(cm => cm.cenaZaMiesiac >= cenaMinimalna).ToList();
            }
            if (tbxCenaMaksymalna.Text.Length > 0)
            {
                var cenaMaksymalna = Convert.ToDecimal(tbxCenaMaksymalna.Text);
                oferty = oferty.Where(cm => cm.cenaZaMiesiac <= cenaMaksymalna).ToList();
            }

            if (rdbMalejaco.IsChecked == true) oferty = oferty.OrderByDescending(c => c.cenaZaMiesiac).ToList();
            if (rdbRosnaco.IsChecked == true) oferty = oferty.OrderBy(c => c.cenaZaMiesiac).ToList();
            WyświetlOferty();
        }

        private void btnResetuj_Click(object sender, RoutedEventArgs e)
        {
            oferty = context.Ofertas.OrderByDescending(c => c.cenaZaMiesiac).ToList();
            WyświetlOferty();
            tbxDaneMieszkania.Clear();
            tbxMiasto.Clear();
            tbxUlica.Clear();
            tbxCenaMinimalna.Clear();
            tbxCenaMaksymalna.Clear();
        }
    }
}
