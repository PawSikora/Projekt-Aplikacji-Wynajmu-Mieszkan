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
    /// Logika interakcji dla klasy HistoriaOkno.xaml
    /// </summary>
    public partial class HistoriaOkno : Window
    {
        private Klient klient;
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        public HistoriaOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void WyswietlTransakcje()
        {
            var transakcje = context.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();
           //MessageBox.Show(klient.idK.ToString());
           //MessageBox.Show(transakcje.idK.ToString());
            if (transakcje != null)
            {
                var bilans = context.Bilans.Where(b => b.idM == transakcje.idM).ToList();
                var mieszkanie = context.DaneMieszkanias.Where(m => m.idK == klient.idK).Where(o => o.poczatekWynajmu != null).FirstOrDefault();

                tbxPoczatekWynajmu.Text = $"{transakcje.poczatekWynajmu:d}";
                tbxKoniecWynajmu.Text = $"{transakcje.koniecWynajmu:d}";

                tbxDaneMieszkania.Text = $"Wynajmowane mieszkanie: {mieszkanie}";

                tbxImieNazwisko.Text = $"{transakcje.Wlasciciel.imie} {transakcje.Wlasciciel.nazwisko}";
                tbxEmail.Text = $"{transakcje.Wlasciciel.email}";
                tbxNrKonta.Text = $"{transakcje.Wlasciciel.nrKonta}";
                if (bilans != null)
                {
                    foreach (var b in bilans)
                    {
                        lbxTransakcje.Items.Add($"Opłata dnia: {b.dataTransakcji:d} - {b.kwota:C}");
                    }
                }
            }
        }

        public HistoriaOkno(Klient klientM) : this()
        {
            klient = klientM;
            WyswietlTransakcje();
        }
        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new KlientOkno(klient).Show();
            Close();
        }
    }
}
