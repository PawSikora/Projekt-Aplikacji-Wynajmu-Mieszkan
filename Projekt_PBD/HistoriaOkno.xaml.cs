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
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private int zaleglaOplataIDX;
        private List<ZalegleOplaty> listaZaleglych = new List<ZalegleOplaty>();

        private struct ZalegleOplaty
        {
            public decimal kwota;
            public DateTime data;
            public ZalegleOplaty(decimal kwota, DateTime data)
            {
                this.kwota = kwota;
                this.data = data.Date;
            }
        }
        public HistoriaOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        public HistoriaOkno(Klient klientM) : this()
        {
            klient = klientM;
            WyswietlTransakcje();
            WyswietlZalegle();
        }

        private void WyswietlTransakcje()
        {
            lbxTransakcje.Items.Clear();
            var mieszkanieKlienta = context.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();
            var transakcje = context.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();

            if (transakcje != null)
            {
                var bilans = context.Bilans.Where(b => b.idM == transakcje.idM).Where(bk => bk.dataTransakcji >= mieszkanieKlienta.poczatekWynajmu).ToList();
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

        private void WyswietlZalegle()
        {
            lbxZalegle.Items.Clear();
            var mieszkanieKlienta = context.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();
            var oferta = context.Ofertas.Where(o => o.idM == mieszkanieKlienta.idM).FirstOrDefault();
            var wplaty = context.Bilans.Where(b => b.idM == mieszkanieKlienta.idM).Where(bk => bk.dataTransakcji >= mieszkanieKlienta.poczatekWynajmu).ToList();

            DateTime dataWynajmu = ((DateTime)mieszkanieKlienta.poczatekWynajmu).Date;
            DateTime tmpData = dataWynajmu;


            //while (dataWynajmu < new DateTime(2022, 9, 25))
            while (dataWynajmu <= DateTime.Today)
            {
                dataWynajmu = dataWynajmu.AddMonths(1);

                //if (wplaty.Where(w => ((DateTime)w.dataTransakcji).Date  <= dataWynajmu.Date).FirstOrDefault() == null)
                if (wplaty.Where(w => w.dataTransakcji <= dataWynajmu && w.dataTransakcji > tmpData).FirstOrDefault() == null)
                {

                    lbxZalegle.Items.Add($"Zaległa opłata: {oferta.cenaZaMiesiac:C} za {dataWynajmu:d}");
                    listaZaleglych.Add(new ZalegleOplaty((decimal)oferta.cenaZaMiesiac, dataWynajmu));
                }
                tmpData = tmpData.AddMonths(1);
            }
        }
        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new KlientOkno(klient).Show();
            Close();
        }

        private void btnWplata_Click(object sender, RoutedEventArgs e)
        {
            if (zaleglaOplataIDX >= 0)
            {
                var zalegle = listaZaleglych[zaleglaOplataIDX];
                var mieszkanieKlienta = context.DaneMieszkanias.Where(m => m.idK == klient.idK).FirstOrDefault();
                Bilan oplata = new Bilan();
                oplata.dataTransakcji = DateTime.Today;
                oplata.idM = mieszkanieKlienta.idM;
                oplata.kategoria = $"Opłata miesięczna za {zalegle.data:d}";
                //oplata.kwota = mieszkanieKlienta.Ofertas.Where(o => o.idM == mieszkanieKlienta.idM).FirstOrDefault().cenaZaMiesiac;
                oplata.kwota = zalegle.kwota;
                MessageBox.Show($"Dokonano wpłaty {oplata.kwota:C} dnia {oplata.dataTransakcji}");

                context.Bilans.Add(oplata);
                context.SaveChanges();
            }
            WyswietlTransakcje();
            WyswietlZalegle();
        }

        private void lbxZalegle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            zaleglaOplataIDX = lbxZalegle.SelectedIndex;
        }
    }
}
