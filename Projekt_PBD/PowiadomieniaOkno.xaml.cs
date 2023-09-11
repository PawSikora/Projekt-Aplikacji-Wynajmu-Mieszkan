using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
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
    /// Logika interakcji dla klasy PowiadomieniaOkno.xaml
    /// </summary>
    public partial class PowiadomieniaOkno : Window
    {
        private Wlasciciel wlasciciel;
        private Zainteresowani zainteresowany;
        private DaneMieszkania mieszkanie;
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        public PowiadomieniaOkno()
        {
            InitializeComponent();
            lblData.Content += $" {DateTime.Today:d}";
            btnWynajmij.IsEnabled = false;
            chkNieokreslony.IsChecked = false;
        }

        public PowiadomieniaOkno(Wlasciciel wlasciciel, DaneMieszkania mieszkanie) : this()
        {
            this.wlasciciel = wlasciciel;
            this.mieszkanie = mieszkanie;
            for (int i = 1; i <= 120; i++) { cbxOkresWynajmu.Items.Add($"{i}"); } 

            Wyswietl();
        }

        private void Wyswietl()
        {
            lbxZainteresowani.Items.Clear();

            var powiadomienia = context.Zainteresowanis.Where(p => p.idO == context.Ofertas.Where(o => o.idM == mieszkanie.idM).FirstOrDefault().idO).ToList();

            foreach (var z in powiadomienia)
            {
                if (z.Klient.DaneMieszkanias.Where(k => k.idK == z.Klient.idK).Count() == 0)
                    lbxZainteresowani.Items.Add(z);
            }
        }

        private void lbxZainteresowani_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxPowiadomienie.Clear();

            zainteresowany = (Zainteresowani)lbxZainteresowani.SelectedItem;

            if (zainteresowany != null)
            {
                tbxPowiadomienie.Text = $"E-mail: {zainteresowany.Klient.email} \n{zainteresowany.daneKontaktowe}";

                if (context.DaneMieszkanias.Where(dm => dm.idK == zainteresowany.Klient.idK).Count() > 0)
                {
                    btnWynajmij.IsEnabled = false;
                    tbxPowiadomienie.Text += $"\n \n \n - Klient wynajmuje obecnie mieszkanie!";
                }
                else
                    btnWynajmij.IsEnabled = true;
            }
        }
        private void btnWyjdz_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnWynajmij_Click(object sender, RoutedEventArgs e)
        {
            DaneMieszkania mieszkanie = context.DaneMieszkanias.Where(m => m.idM == this.mieszkanie.idM).FirstOrDefault();
            Oferta of = context.Ofertas.Where(o => o.idO == context.Ofertas.OrderByDescending(offer => offer.dataWystawienia).Select(s => s.idO).FirstOrDefault()).FirstOrDefault();
            Zainteresowani z = context.Zainteresowanis.Where(za => za.idZ == zainteresowany.idZ).FirstOrDefault();

            if (mieszkanie != null)
            {
                if (clrWynajemOd.SelectedDate != null)
                { 
                    mieszkanie.idK = zainteresowany.idK;
                    mieszkanie.poczatekWynajmu = clrWynajemOd.SelectedDate.Value.Date;
                    mieszkanie.koniecWynajmu = DateTime.Today.AddMonths(Convert.ToInt32(cbxOkresWynajmu.SelectedValue));

                    if(chkNieokreslony.IsChecked == false) 
                        mieszkanie.koniecWynajmu = clrWynajemOd.SelectedDate.Value.Date.AddMonths(Convert.ToInt32(cbxOkresWynajmu.SelectedValue));
                    else if(chkNieokreslony.IsChecked == true)
                        mieszkanie.koniecWynajmu = null;

                    mieszkanie.doWynajecia = false;
                    of.aktualne = false;

                    context.Zainteresowanis.Remove(zainteresowany);
                    context.SaveChanges();

                    MessageBox.Show($"Wynajęto mieszkanie!");

                    btnWynajmij.IsEnabled = false;
                    Wyswietl();
                    tbxPowiadomienie.Clear();
                }
                else
                {
                    MessageBox.Show("Nie wybrano daty!");
                }
            }
        }

        private void chkNieokreslony_Checked(object sender, RoutedEventArgs e)
        {
            lblMiesiace.IsEnabled = false;
            cbxOkresWynajmu.IsEnabled = false;
            lblOkres.IsEnabled = false;
        }

        private void chkNieokreslony_Unchecked(object sender, RoutedEventArgs e)
        {
            lblMiesiace.IsEnabled = true;
            cbxOkresWynajmu.IsEnabled = true;
            lblOkres.IsEnabled = true;
        }
    }
}
