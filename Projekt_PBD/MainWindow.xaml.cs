using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekt_PBD
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();

        private void btnZaloguj_Click(object sender, RoutedEventArgs e)
        {

            Log user = Validate(txtLogin.Text,txtHaslo.Password);
            if (user != null)
            {
                if (user.idA != null)
                {
                    // context.Administrators.Where(a => a.idA == user.idA).First();
                    MessageBox.Show("AdminLog");
                }
                else if (user.idK != null)
                {
                    var client = context.Klients.Where(k => k.idK == user.idK).First();
                    KlientOkno klient = new KlientOkno(client);
                    Close();
                    klient.ShowDialog();
                }
                else if (user.idW != null)
                {
                    var owner = context.Wlasciciels.Where(w => w.idW == user.idW).First();
                    WlascicielOkno wlasciciel = new WlascicielOkno(owner);
                    Close();
                    wlasciciel.ShowDialog();
                }
            }
            else MessageBox.Show("Błąd nazwy lub hasła");
        }

        public Log Validate(string email, string haslo)
        {
            return context.Logs.Where(m => m.email == email).Where(n => n.haslo == haslo).FirstOrDefault();
        }

        private void btnZarejestruj_Click(object sender, RoutedEventArgs e)
        {
            RejestracjaOkno okno = new RejestracjaOkno();
            this.Close();
            okno.ShowDialog();
            
        }
    }
}
