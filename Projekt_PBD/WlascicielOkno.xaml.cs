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
        public WlascicielOkno()
        {
            InitializeComponent();
            var mieszkania = context.DaneMieszkanias;
            foreach (var x in mieszkania)
            {
                cbxMieszkania.Items.Add(x.kodPocztowy);
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var mieszkania = context.DaneMieszkanias;
            //List<DaneMieszkania> tmp = new List<DaneMieszkania>();
            //tmp = mieszkania.ToList();
            //txtDaneMieszkania.Text = tmp[cbxMieszkania.SelectedIndex].Miasto.ToString();
            Wyswietl();
        }
        public void Wyswietl()
        {
            txtDaneMieszkania.Clear();
            var selected = context.DaneMieszkanias.ToList()[cbxMieszkania.SelectedIndex];
            txtDaneMieszkania.AppendText($"ul. {selected.Ulica.ToString()} {selected.nrBudynku.ToString()}/{selected.nrMieszkania.ToString()}\n");
            txtDaneMieszkania.AppendText($"{selected.kodPocztowy.ToString()} {selected.Miasto.ToString()}");
            
        }
        
    }
}
