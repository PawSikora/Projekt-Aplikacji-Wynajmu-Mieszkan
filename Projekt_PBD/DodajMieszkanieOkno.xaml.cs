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
    /// Logika interakcji dla klasy DodajMieszkanieOkno.xaml
    /// </summary>
    public partial class DodajMieszkanieOkno : Window
    {
        private Wlasciciel wlasciciel;
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();

        public DodajMieszkanieOkno()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        public DodajMieszkanieOkno(Wlasciciel wlasciciel) : this()
        {
            this.wlasciciel = wlasciciel;
        }

        private void btnDodajMieszkanie_Click(object sender, RoutedEventArgs e)
        {
            
            if (tbxAdres.Text.Length > 0 && tbxKodPocztowy.Text.Length > 0 && tbxMiasto.Text.Length > 0 && tbxNumerMIeszkania.Text.Length>0)
            {
                string[] tab = tbxAdres.Text.Split(' ');

                if (tab.Length > 1)
                {
                    string adres = tab[0];
                    int nrBudynku = Convert.ToInt32(tab[1]);
                    int nrMieszkania = Convert.ToInt32(tbxNumerMIeszkania.Text);

                    if (context.DaneMieszkanias.Where(a => a.Ulica == adres)
                            .Where(nb => nb.nrBudynku == nrBudynku)
                            .Where(nm => nm.nrMieszkania == nrMieszkania).FirstOrDefault() == null)
                    {
                        DaneMieszkania mieszkanie = new DaneMieszkania
                        {
                            Ulica = adres,
                            nrBudynku = nrBudynku,
                            Miasto = tbxMiasto.Text,
                            kodPocztowy = tbxKodPocztowy.Text,
                            nrMieszkania = nrMieszkania,
                            idW = this.wlasciciel.idW
                        };

                        context.DaneMieszkanias.Add(mieszkanie);

                        mieszkanie.doRemontu = ckbDoRemontu.IsChecked;
                        mieszkanie.doWynajecia = ckbDoWynajmu.IsChecked;

                        context.SaveChanges();
                        new WlascicielOkno(wlasciciel).Show();
                        this.Close();
                    }

                }
                else MessageBox.Show("Podaj nr budynku po spacji w polu \"Ulica\"!");
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new WlascicielOkno(wlasciciel).Show();
            this.Close();
        }
    }
}
