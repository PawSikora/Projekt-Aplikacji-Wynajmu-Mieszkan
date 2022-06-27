﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects.DataClasses;
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
    /// Logika interakcji dla klasy PowiadomieniaOkno.xaml
    /// </summary>
    public partial class PowiadomieniaOkno : Window
    {
        private Wlasciciel wlasciciel;
        private Zainteresowani zainteresowany;
        private DaneMieszkania mieszkanie;
        private Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        public PowiadomieniaOkno()
        {
            InitializeComponent();
            lblData.Content += $" {DateTime.Today:d}";
            btnWynajmij.IsEnabled = false;
        }

        public PowiadomieniaOkno(Wlasciciel wlasciciel, DaneMieszkania mieszkanie) : this()
        {
            this.wlasciciel = wlasciciel;
            this.mieszkanie = mieszkanie;
            for (int i = 1; i <= 120; i++) { cbxOkresWynajmu.Items.Add($"{i}"); } 

            //foreach (var z in context.Zainteresowanis.Where(z => z.idO == mieszkanie.Ofertas.Where(o => o.idM == mieszkanie.idM).Select(s => s.idO).FirstOrDefault())) 
            Wyswietl();
        }

        private void Wyswietl()
        {
            lbxZainteresowani.Items.Clear();

            var powiadomienia = context.Zainteresowanis.Where(p => p.idO == context.Ofertas.Where(o => o.idM == mieszkanie.idM).FirstOrDefault().idO).ToList();

            //foreach (var z in context.Zainteresowanis.Where(z => z.idO == context.Ofertas.OrderByDescending(o => o.dataWystawienia).Select(s => s.idO).FirstOrDefault()))
            foreach (var z in powiadomienia)
            {
                if (z.Klient.DaneMieszkanias.Where(k => k.idK == z.Klient.idK).Count() == 0)
                    lbxZainteresowani.Items.Add(z);
            }
        }

        private void lbxZainteresowani_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbxPowiadomienie.Clear();
            zainteresowany = (Zainteresowani)lbxZainteresowani.SelectedItem;
            if (zainteresowany != null)
            {
                tbxPowiadomienie.Text = $"E-mail: {zainteresowany.Klient.email} \n{zainteresowany.daneKontaktowe}";
                if (context.DaneMieszkanias.Where(dm => dm.idK == zainteresowany.Klient.idK).Count() > 0)
                {
                    btnWynajmij.IsEnabled = false;
                    tbxPowiadomienie.Text += $"\n \n \n - Klient wynajmuje obecnie mieszkanie!";
                }
                else
                    btnWynajmij.IsEnabled = true;
            }
        }
        private void btnWyjdz_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnWynajmij_Click(object sender, RoutedEventArgs e) //Sprawdzić czemu SCRASHOWAŁO!!!!!!!
        {
            DaneMieszkania mieszkanie = context.DaneMieszkanias.Where(m => m.idM == this.mieszkanie.idM).FirstOrDefault();
            Oferta of = context.Ofertas.Where(o => o.idO == context.Ofertas.OrderByDescending(offer => offer.dataWystawienia).Select(s => s.idO).FirstOrDefault()).FirstOrDefault();
            Zainteresowani z = context.Zainteresowanis.Where(za => za.idZ == zainteresowany.idZ).FirstOrDefault();
            if (mieszkanie != null)
            {
                if (clrWynajemOd.SelectedDate != null && clrWynajemDo.SelectedDate != null)
                {
                    mieszkanie.idK = zainteresowany.idK;
                    //mieszkanie.poczatekWynajmu = DateTime.Today;
                    mieszkanie.poczatekWynajmu = clrWynajemOd.SelectedDate;
                    //mieszkanie.koniecWynajmu = DateTime.Today.AddMonths(Convert.ToInt32(cbxOkresWynajmu.SelectedValue));
                    mieszkanie.koniecWynajmu = clrWynajemDo.SelectedDate;
                    mieszkanie.doWynajecia = false;
                    of.aktualne = false;
                    MessageBox.Show($"{mieszkanie.poczatekWynajmu} - {mieszkanie.koniecWynajmu}");
                    context.Zainteresowanis.Remove(zainteresowany);
                    int x = context.SaveChanges();
                    MessageBox.Show($"Wynajęto mieszkanie! SaveChanges({x})");
                    btnWynajmij.IsEnabled = false;
                    Wyswietl();
                    tbxPowiadomienie.Clear();
                }
                else
                {
                    MessageBox.Show("Nie wybrano daty!");
                }
            }
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"{clrWynajemOd.SelectedDate:d}-{clrWynajemDo.SelectedDate:d}");
            if (clrWynajemOd.SelectedDate == null)
                MessageBox.Show("Lewy null");
            if (clrWynajemDo.SelectedDate == null)
                MessageBox.Show("Prawy null");
        }

        private void chkNieokreslony_Checked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Checked");
            clrWynajemDo.IsEnabled = false;
        }

        private void chkNieokreslony_Unchecked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Unchecked");
            clrWynajemDo.IsEnabled = true;
        }
    }
}