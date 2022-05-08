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
    /// Logika interakcji dla klasy RejestracjaOkno.xaml
    /// </summary>
    ///
    public partial class RejestracjaOkno : Window
    {
        public RejestracjaOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        void Zamknij()
        {
            new MainWindow().Show();
            this.Close();
        }

        Baza_wynajmuEntities context=new Baza_wynajmuEntities();
        private void btnUwtorzKonto_Click(object sender, RoutedEventArgs e)
        {

            if (pwbHaslo.Password == pwbPowtorzHaslo.Password && tbxEmail.Text.Length > 0 && tbxEmail.Text.Contains("@") &&
                tbxImie.Text.Length > 0 && tbxNazwisko.Text.Length>0 && tbxNrKonta.Text.Length>0) 
            {
                if (context.Logs.Where(em => em.email == tbxEmail.Text).FirstOrDefault()==null)
                {
                    if (rdbCzyKlient.IsChecked == true)
                    {
                        Klient klient = new Klient();
                        klient.email=   tbxEmail.Text;
                        klient.imie = tbxImie.Text;
                        klient.nazwisko = tbxNazwisko.Text;
                        klient.nrKonta = Convert.ToInt32(tbxNrKonta.Text);
                        context.Klients.Add(klient);
                        context.SaveChanges();
                        Log log = new Log();
                        log.haslo = pwbHaslo.Password;
                        log.email = tbxEmail.Text;
                        log.idK = context.Klients.Where(k => k.email == tbxEmail.Text).Select(i=>i.idK).First();
                        context.Logs.Add(log);
                        context.SaveChanges();
                    }                    
                    if (rdbCzyWlasciciel.IsChecked == true)
                    {
                        Wlasciciel wlasciciel = new Wlasciciel();
                        wlasciciel.email=   tbxEmail.Text;
                        wlasciciel.imie = tbxImie.Text;
                        wlasciciel.nazwisko = tbxNazwisko.Text;
                        wlasciciel.nrKonta = Convert.ToInt32(tbxNrKonta.Text);
                        context.Wlasciciels.Add(wlasciciel);
                        context.SaveChanges();
                        Log log = new Log();
                        log.haslo = pwbHaslo.Password;
                        log.email = tbxEmail.Text;
                        log.idW = context.Wlasciciels.Where(w => w.email == tbxEmail.Text).Select(i=>i.idW).First();
                        context.Logs.Add(log);
                        context.SaveChanges();

                    }
                }
                Zamknij();
            }
        }
        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            Zamknij();
        }
    }
}
