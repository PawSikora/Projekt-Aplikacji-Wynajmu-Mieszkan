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
    /// Logika interakcji dla klasy WlascicielOkno.xaml
    /// </summary>
    public partial class WlascicielOkno : Window
    {
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private Wlasciciel wlasciciel;
        private List<DaneMieszkania> mieszkania;

        //private int index = 0;

        public WlascicielOkno(Wlasciciel wlasciciel)
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            this.wlasciciel = wlasciciel;
            tbxImie.Text = wlasciciel.imie;
            tbxNazwisko.Text = wlasciciel.nazwisko;
            WyswietlMieszkania();
            if(mieszkania.Count == 0) btnWypowiedzUmowe.IsEnabled = false;
        }

        private void WyswietlMieszkania()
        {
            cbxMieszkania.Items.Clear();
            mieszkania = context.DaneMieszkanias.Where(m => m.idW == wlasciciel.idW).ToList();
            var listaMieszkań = context.DaneMieszkanias.ToList();
            foreach (var x in mieszkania)
            {
                if (x != null)
                {
                    int interesanci = 0;

                    var zainteresowani = context.Zainteresowanis.Where(z => z.Oferta.idM == x.idM).ToList();
                    foreach (var z in zainteresowani)
                    {
                        if (context.DaneMieszkanias.Where(m => m.idK == z.idK).FirstOrDefault() == null)
                            interesanci++;
                    }

                    string stan;
                    if (x.doWynajecia == true)
                        stan = "(Dostępne)";
                    else
                        stan = "(Niedostępne)";

                    cbxMieszkania.Items.Add($"{x} {stan} [{interesanci}] ");
                }
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e) => Wyswietl();
        public void Wyswietl()
        {
            //index = cbxMieszkania.SelectedIndex;
            //if (index < 0) { index = mieszkania.Count() - 1; }
            tbxDaneMieszkania.Clear();
            //MessageBox.Show(cbxMieszkania.SelectedIndex.ToString());
            var selected = mieszkania[cbxMieszkania.SelectedIndex];
            if (selected.idK == null) { btnWypowiedzUmowe.IsEnabled = false; }
            else { btnWypowiedzUmowe.IsEnabled = true; }
            if (selected != null)
            {
                tbxDaneMieszkania.AppendText($"ul. {selected.Ulica.ToString()} {selected.nrBudynku.ToString()}/{selected.nrMieszkania.ToString()}\n");
                tbxDaneMieszkania.AppendText($"{selected.kodPocztowy.ToString()} {selected.Miasto.ToString()}");
                if(selected.doRemontu == true) tbxDaneMieszkania.AppendText($"\nMieszkanie w remoncie");
                if (selected.doWynajecia == true) tbxDaneMieszkania.AppendText($"\nMieszkanie do wynajęcia");
                if(cbxMieszkania.SelectedItem.ToString().Contains("[0]") || selected.doWynajecia == false) 
                    btnPowiadomienia.IsEnabled = false;
                else 
                    btnPowiadomienia.IsEnabled = true;
            }
        }
        private void btnDodajeMieszkanie_Click(object sender, RoutedEventArgs e)
        {
            new DodajMieszkanieOkno(wlasciciel).Show();
            this.Close();
        }
        private void btnWystawOferte_Click(object sender, RoutedEventArgs e)
        {
            if (cbxMieszkania.Items.Count>0)
            {
                DaneMieszkania mieszkanie = mieszkania[cbxMieszkania.SelectedIndex];
                if (mieszkanie.idK == null)
                {
                    if (mieszkanie.doWynajecia == true)
                    {
                        if (mieszkanie.doRemontu == true)
                        {
                            MessageBoxResult m = MessageBox.Show("Czy chcesz zmienić mieszkanie na do remontu?",
                                "Zmiana ustawień", MessageBoxButton.YesNo);
                            if (m == MessageBoxResult.Yes) { mieszkanie.doRemontu = false; }
                        }
                        else
                        {
                            new DodajOferteOkno(wlasciciel, mieszkanie).Show();
                            Close();
                        }
                    }
                    else
                    {
                        MessageBoxResult n = MessageBox.Show("Czy chcesz zmienić mieszkanie na do Wynajecia?",
                            "Zmiana ustawień", MessageBoxButton.YesNo);
                        if (n == MessageBoxResult.Yes) { mieszkanie.doWynajecia = true; }

                        if (mieszkanie.doRemontu == true)
                        {
                            MessageBoxResult m = MessageBox.Show("Czy chcesz zmienić mieszkanie na do Remontu?",
                                "Zmiana ustawien", MessageBoxButton.YesNo);
                            if (m == MessageBoxResult.Yes) { mieszkanie.doRemontu = false; }
                        }
                        if (mieszkanie.doRemontu == false && mieszkanie.doWynajecia == true)
                        {
                            new DodajOferteOkno(wlasciciel, mieszkanie).Show();
                            Close();
                        }
                    }
                    context.SaveChanges();
                    //WyswietlMieszkania();
                }
                else { MessageBox.Show("Mieszkanie jest już wynajmowane!"); }
            }
        }

        private void btnUstawRemont_Click(object sender, RoutedEventArgs e)
        {
            if (cbxMieszkania.Items.Count > 0)
            {
                DaneMieszkania mieszkanie = mieszkania[cbxMieszkania.SelectedIndex];
                if (mieszkanie.idK == null)
                {
                    if (mieszkanie.doRemontu == false) mieszkanie.doRemontu = true;
                    else mieszkanie.doRemontu = false;
                    Wyswietl();
                }
                else
                {
                    MessageBox.Show("Mieszkanie jest wynajęte!");
                }
            }
        }

        private void btnUstawWynajęcie_Click(object sender, RoutedEventArgs e)
        {
            if (cbxMieszkania.Items.Count > 0)
            {
                DaneMieszkania mieszkanie = mieszkania[cbxMieszkania.SelectedIndex];
                if (mieszkanie.idK == null)
                {
                    if (mieszkanie.doWynajecia == false) mieszkanie.doWynajecia = true;
                    else mieszkanie.doWynajecia = false;
                    Wyswietl();
                }
                else
                {
                    MessageBox.Show("Mieszkanie jest wynajęte!");
                }
            }
        }

        private void btnWypowiedzUmowe_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult message = MessageBox.Show("Czy na pewno chcesz wypowiedzieć umowę?",
                "Wypowiedzenie umowy", MessageBoxButton.YesNo);
            if (message == MessageBoxResult.Yes)
            {
                var m = mieszkania[cbxMieszkania.SelectedIndex];
                var mieszkanieDoWypowiedzenia = context.DaneMieszkanias
                    .Where(mdw => mdw.idM == m.idM).FirstOrDefault();
                if (mieszkanieDoWypowiedzenia != null)
                {
                    mieszkanieDoWypowiedzenia.koniecWynajmu = DateTime.Today.AddMonths(1);
                    context.SaveChanges();
                    MessageBox.Show("WYPOWIEDZIANO UMOWĘ!");
                }
            }
        }

        private void btnBilans_Click(object sender, RoutedEventArgs e)
        {
            BilansOkno okno = new BilansOkno(wlasciciel);
            this.Close();
            okno.ShowDialog();
        }

        private void btnPowiadomienia_Click(object sender, RoutedEventArgs e)
        {
            PowiadomieniaOkno powiadomienia = new PowiadomieniaOkno(wlasciciel, mieszkania[cbxMieszkania.SelectedIndex]);
            powiadomienia.ShowDialog();
        }
    }
}
