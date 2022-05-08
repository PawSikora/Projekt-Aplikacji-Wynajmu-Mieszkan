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
        public WlascicielOkno(Wlasciciel wlasciciel)
        {
            InitializeComponent();
            this.wlasciciel = wlasciciel;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            tbxImie.Text = wlasciciel.imie;
            tbxNazwisko.Text = wlasciciel.nazwisko;
            var mieszkania = wlasciciel.DaneMieszkanias.Where(m => m.idW==wlasciciel.idW);
            foreach (var x in mieszkania)
            {
                cbxMieszkania.Items.Add($"{x.Miasto} {x.Ulica} {x.nrBudynku}/{x.nrMieszkania}");
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            Close();
            new MainWindow().ShowDialog();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Wyswietl();
        }
        public void Wyswietl()
        {
            tbxDaneMieszkania.Clear();
            var selected = context.DaneMieszkanias.ToList()[cbxMieszkania.SelectedIndex];
            tbxDaneMieszkania.AppendText($"ul. {selected.Ulica.ToString()} {selected.nrBudynku.ToString()}/{selected.nrMieszkania.ToString()}\n");
            tbxDaneMieszkania.AppendText($"{selected.kodPocztowy.ToString()} {selected.Miasto.ToString()}");
        }
    }
}
