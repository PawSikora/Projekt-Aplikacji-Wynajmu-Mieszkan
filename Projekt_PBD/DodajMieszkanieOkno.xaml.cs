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

        public DodajMieszkanieOkno(Wlasciciel Wlasciciel) : this()
        {
            this.wlasciciel = Wlasciciel;
        }

        public DodajMieszkanieOkno()
        {
            InitializeComponent();
        }

        private void btnDodajMieszkanie_Click(object sender, RoutedEventArgs e)
        {
            
            if (tbxAdres.Text.Length > 0 && tbxKodPocztowy.Text.Length > 0 && tbxMiasto.Text.Length > 0 && tbxNumerMIeszkania.Text.Length>0)
            {
                
                String[] tab = tbxAdres.Text.Split(' ');/// 0 to Adres 1 to numer domu
                string adres= tab[0];
                int nrBudynku=Convert.ToInt32(tab[1]) ;
                int nrMieszkania= Convert.ToInt32(tbxNumerMIeszkania.Text) ;
                if (context.DaneMieszkanias.Where(a => a.Ulica == adres)
                        .Where(nb=>nb.nrBudynku==nrBudynku)
                        .Where(nm => nm.nrMieszkania == nrMieszkania).FirstOrDefault() == null)
                {
                    DaneMieszkania mieszkanie = new DaneMieszkania();
                    mieszkanie.Ulica = adres;
                    mieszkanie.nrBudynku = nrBudynku;
                    mieszkanie.Miasto = tbxMiasto.Text;
                    mieszkanie.kodPocztowy = tbxKodPocztowy.Text;
                    mieszkanie.nrMieszkania= nrMieszkania;
                    mieszkanie.idW = this.wlasciciel.idW;//B I L A N S  nie jest podpiety bo nie istnieje
                    context.DaneMieszkanias.Add(mieszkanie);
                    mieszkanie.doRemontu = ckbDoRemontu.IsChecked;
                    mieszkanie.doWynajecia=ckbDoWynajmu.IsChecked;
                    context.SaveChanges();




                }

            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}
