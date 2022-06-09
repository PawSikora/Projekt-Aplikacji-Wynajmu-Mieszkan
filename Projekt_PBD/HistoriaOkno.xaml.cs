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
            if (transakcje != null)
            {
                var bilans = context.Bilans.Where(b => b.idM == transakcje.idM).ToList();
                lbxTransakcje.Items.Add($"Początek wynajmu: {transakcje.poczatekWynajmu}");
                lbxTransakcje.Items.Add($"Koniec wynajmu: {transakcje.koniecWynajmu}");
                if (bilans != null)
                {
                    foreach (var b in bilans)
                    {
                        lbxTransakcje.Items.Add($"{b.dataTransakcji} {b.kwota}");
                    }
                }
            }
        }

        public HistoriaOkno(Klient klientM) : this()
        {
            klient = klientM;
        }
        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new KlientOkno(klient).Show();
            Close();
        }
    }
}
