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
    /// Logika interakcji dla klasy DodajOferteOkno.xaml
    /// </summary>
    public partial class DodajOferteOkno : Window
    {
        private Wlasciciel wlasciciel;
        private DaneMieszkania mieszkanie;
        private Baza_wynajmuEntities context ;
        public DodajOferteOkno()
        {
            InitializeComponent();
            context = new Baza_wynajmuEntities();
        }

        public DodajOferteOkno(Wlasciciel Wlasciciel, DaneMieszkania Mieszkanie) : this()
        {
            this.mieszkanie=Mieszkanie;
            this.wlasciciel=Wlasciciel;
            


        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (tbxCenaZaMiesiac.Text.Length > 0 && tbxMetraz.Text.Length > 0 && tbxOpis.Text.Length > 0 &&
                tbxWyposazenie.Text.Length > 0)
            {
                if (context.Ofertas.Where(i => i.idM == mieszkanie.idM).FirstOrDefault()==null)
                {
                    Oferta oferta = new Oferta();
                    oferta.idM = mieszkanie.idM;
                    oferta.aktualne = true;
                    oferta.cenaZaMiesiac = Convert.ToDecimal(tbxCenaZaMiesiac.Text);
                    oferta.opis = tbxOpis.Text;
                    oferta.wyposazenie=tbxWyposazenie.Text;
                    oferta.dataWystawienia=DateTime.Now;
                    oferta.metraz =Convert.ToDouble(tbxMetraz.Text) ;
                    context.Ofertas.Add(oferta);
                    context.SaveChanges();
                }
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MessageBox.Show("N");
        }
    }
}
