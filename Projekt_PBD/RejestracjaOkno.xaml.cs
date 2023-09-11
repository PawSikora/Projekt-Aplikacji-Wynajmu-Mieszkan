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
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
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

        public static String ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }
            return hex.ToString();
        }
        public static String CreateSalt(int size)
        {
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var buff = new byte[size];

            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        public static String GenerateSHA256Hash(String input, String salt)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input + salt);
            System.Security.Cryptography.SHA256Managed sha256hashstring = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha256hashstring.ComputeHash(bytes);

            return ByteArrayToHexString(hash);
        }

        private void btnUwtorzKonto_Click(object sender, RoutedEventArgs e)
        {
            if (pwbHaslo.Password == pwbPowtorzHaslo.Password && tbxEmail.Text.Length > 0 && tbxEmail.Text.Contains("@") &&
                tbxImie.Text.Length > 0 && tbxNazwisko.Text.Length>0 && tbxNrKonta.Text.Length>0)
            {
                if (context.Logs.Where(em => em.email == tbxEmail.Text).FirstOrDefault()==null)
                {
                    String salt = CreateSalt(10);

                    if (rdbCzyKlient.IsChecked == true)
                    {
                        Klient klient = new Klient()
                        {
                            email = tbxEmail.Text,
                            imie = tbxImie.Text,
                            nazwisko = tbxNazwisko.Text,
                            nrKonta = Convert.ToInt32(tbxNrKonta.Text)
                        };

                        context.Klients.Add(klient);
                        context.SaveChanges();

                        Log log = new Log()
                        {
                            haslo = GenerateSHA256Hash(pwbHaslo.Password, salt),
                            email = tbxEmail.Text,
                            salt = salt,
                            idK = context.Klients.Where(k => k.email == tbxEmail.Text).Select(i => i.idK).First()
                        };

                        context.Logs.Add(log);
                        context.SaveChanges();
                    }
                    
                    if (rdbCzyWlasciciel.IsChecked == true)
                    {
                        Wlasciciel wlasciciel = new Wlasciciel()
                        {
                            email = tbxEmail.Text,
                            imie = tbxImie.Text,
                            nazwisko = tbxNazwisko.Text,
                            nrKonta = Convert.ToInt32(tbxNrKonta.Text)
                        };

                        context.Wlasciciels.Add(wlasciciel);
                        context.SaveChanges();

                        Log log = new Log()
                        {
                            haslo = GenerateSHA256Hash(pwbHaslo.Password, salt),
                            email = tbxEmail.Text,
                            salt = salt,
                            idW = context.Wlasciciels.Where(w => w.email == tbxEmail.Text).Select(i => i.idW).First()
                        };

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
