using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logika interakcji dla klasy BilansOkno.xaml
    /// </summary>
    public partial class BilansOkno : Window
    {
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private Wlasciciel wlasciciel;
        private List<DaneMieszkania> mieszkania;
        public BilansOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Ukryj();

            int rok = 0;
            int pusty = 0;
            foreach (var m in DateTimeFormatInfo.CurrentInfo.MonthNames)
                pusty = cbxMiesiac.Items.Add(m);

            cbxMiesiac.Items.RemoveAt(pusty);
            foreach (var d in Enumerable.Range(1930, DateTime.Now.Year - 1930+1).ToList())
                rok = cbxLata.Items.Add(d);

            cbxMiesiac.SelectedIndex = DateTime.Now.Month-1;
            cbxLata.SelectedIndex = rok;
        }

        private void Ukryj()
        {
            lblWynajem.Content = "Nie wynajmowane";
            lblEmail.Visibility = Visibility.Hidden;
            lblImieNazwisko.Visibility = Visibility.Hidden;
            lblNrKonta.Visibility = Visibility.Hidden;
            tbxEmail.Visibility = Visibility.Hidden;
            tbxImieNazwisko.Visibility = Visibility.Hidden;
            tbxNrKonta.Visibility = Visibility.Hidden;
        }

        private void Odkryj()
        {
            lblWynajem.Content = "Wynajmowane dla:";
            lblEmail.Visibility = Visibility.Visible;
            lblImieNazwisko.Visibility = Visibility.Visible;
            lblNrKonta.Visibility = Visibility.Visible;
            tbxEmail.Visibility = Visibility.Visible;
            tbxImieNazwisko.Visibility = Visibility.Visible;
            tbxNrKonta.Visibility = Visibility.Visible;
        }

        private void WyswietlCBX()
        {
            mieszkania = context.DaneMieszkanias.Where(m => m.idW == wlasciciel.idW).Where(o => o.poczatekWynajmu != null).ToList();
            foreach (var x in mieszkania)
                cbxMieszkania.Items.Add(x);
        }

        private void WyswietlOplaty()
        {
            lbxMieszkania.Items.Clear();
            Ukryj();
            decimal? cenaZaMiesiac = null;
            decimal? kwota = null;
            DateTime data = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), cbxMiesiac.SelectedIndex+1, 1);
            var mieszkanie = (DaneMieszkania)cbxMieszkania.SelectedItem;
            

            if (mieszkanie != null)
            {
                var oferty = context.Ofertas.Where(o => o.idM == mieszkanie.idM).ToList();

                foreach (var o in oferty) { if (o.dataWystawienia <= data) cenaZaMiesiac = (Decimal)o.cenaZaMiesiac; }
                    
                var bilanse = context.Bilans.Where(b => b.idM == mieszkanie.idM).Where(b => b.dataTransakcji.Value.Year == data.Year).Where(b => b.dataTransakcji.Value.Month == data.Month).ToList();


                foreach (var b in bilanse)
                {
                    if (b.dataTransakcji.Value.Year <= data.Year && b.dataTransakcji.Value.Month <= data.Month)
                    {
                        kwota = (decimal)b.kwota;

                        Odkryj();

                        tbxImieNazwisko.Text = $"{b.DaneMieszkania.Klient.imie} {b.DaneMieszkania.Klient.nazwisko}";
                        tbxEmail.Text = $"{b.DaneMieszkania.Klient.email}";
                        tbxNrKonta.Text = $"{b.DaneMieszkania.Klient.nrKonta}";
                    }
                }


                var dataTransakcji = context.Bilans.Where(b => b.idM == mieszkanie.idM).Where(b => b.dataTransakcji.Value.Year == data.Year).Where(b => b.dataTransakcji.Value.Month == data.Month).Select(x => x.dataTransakcji)
                    .FirstOrDefault();
                if(dataTransakcji != null) lbxMieszkania.Items.Add(dataTransakcji);
                if(cenaZaMiesiac != null) lbxMieszkania.Items.Add("Cena za miesiąc: " + cenaZaMiesiac);
                if(mieszkanie.kosztaRemontow != null) lbxMieszkania.Items.Add("Koszta remontu: " + mieszkanie.kosztaRemontow);
                if(kwota != null) lbxMieszkania.Items.Add("Kwota: " + kwota);
                if (kwota != null && mieszkanie.kosztaRemontow != null) lbxMieszkania.Items.Add("Saldo: " + (kwota - mieszkanie.kosztaRemontow));
            }
        }

        public BilansOkno(Wlasciciel wlascicielM) : this()
        {
            wlasciciel = wlascicielM;
            WyswietlCBX();
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new WlascicielOkno(wlasciciel).Show();
            Close();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null) 
                WyswietlOplaty();
        }

        private void cbxMiesiac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null)
                WyswietlOplaty();
            
        }

        private void cbxLata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null)
                WyswietlOplaty();
        }
    }
}
