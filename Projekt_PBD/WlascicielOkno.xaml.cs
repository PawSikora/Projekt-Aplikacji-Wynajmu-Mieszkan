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
        private int index = 0;
  
        public WlascicielOkno(Wlasciciel wlasciciel)
        {
            InitializeComponent();
            this.wlasciciel = wlasciciel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbxImie.Text = wlasciciel.imie;
            tbxNazwisko.Text = wlasciciel.nazwisko;
            WyswietlMieszkania();

        }

        private void WyswietlMieszkania()
        {
            cbxMieszkania.Items.Clear();
            mieszkania = context.DaneMieszkanias.Where(m => m.idW == wlasciciel.idW).ToList();
            foreach (var x in mieszkania)
            {
                if (x != null) cbxMieszkania.Items.Add($"{x.Miasto} {x.Ulica} {x.nrBudynku}/{x.nrMieszkania}");


            }
            cbxMieszkania.SelectedIndex = index;

        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Wyswietl();
        }
        public void Wyswietl()
        {
            index = cbxMieszkania.SelectedIndex;
            tbxDaneMieszkania.Clear();
            if (index <0)
            {
                index =mieszkania.Count() - 1;
            }
            var selected = mieszkania[index];
            if (selected != null)
            {
                tbxDaneMieszkania.AppendText($"ul. {selected.Ulica.ToString()} {selected.nrBudynku.ToString()}/{selected.nrMieszkania.ToString()}\n");
                tbxDaneMieszkania.AppendText($"{selected.kodPocztowy.ToString()} {selected.Miasto.ToString()}");
            

            }

        }

        private void btnDodajeMieszkanie_Click(object sender, RoutedEventArgs e)
        {
            new DodajMieszkanieOkno(wlasciciel).ShowDialog();
            WyswietlMieszkania();

        }
        private void btnWystawOferte_Click(object sender, RoutedEventArgs e)
        {
            if (cbxMieszkania.Items.Count>0)
            {
                DaneMieszkania mieszkanie = mieszkania[cbxMieszkania.SelectedIndex];
                if (mieszkanie.doWynajecia == true)
                {


                    if (mieszkanie.doRemontu == true)
                    {
                        MessageBoxResult m = MessageBox.Show("Czy chcesz zmienic mieszkanie na do Remontu ? "
                            , "Zmiana ustawien", MessageBoxButton.YesNo);
                        if (m == MessageBoxResult.Yes)
                        {
                            mieszkanie.doRemontu = false;

                        }
                    }
                    else
                    {
                        new DodajOferteOkno(wlasciciel, mieszkanie).ShowDialog();
                    }
                }
                else
                {
                    MessageBoxResult n = MessageBox.Show("Czy chcesz zmienic mieszkanie na do Wynajecia ? "
                        , "Zmiana ustawien", MessageBoxButton.YesNo);
                    if (n == MessageBoxResult.Yes)
                    {
                        mieszkanie.doWynajecia = true;

                    }


                    if (mieszkanie.doRemontu == true)
                    {
                        MessageBoxResult m = MessageBox.Show("Czy chcesz zmienic mieszkanie na do Remontu ? "
                            , "Zmiana ustawien", MessageBoxButton.YesNo);
                        if (m == MessageBoxResult.Yes)
                        {
                            mieszkanie.doRemontu = false;

                        }
                    }
                    if(mieszkanie.doRemontu == false && mieszkanie.doWynajecia==true) new DodajOferteOkno(wlasciciel, mieszkanie).ShowDialog();







                }
                context.SaveChanges();
                WyswietlMieszkania();
            }

        }
    }
}
