﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Logika interakcji dla klasy BilansOkno.xaml
    /// </summary>
    public partial class BilansOkno : Window
    {
        Baza_wynajmuEntities context = new Baza_wynajmuEntities();
        private Wlasciciel wlasciciel;
        private List<DaneMieszkania> mieszkania;
        public BilansOkno()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            Ukryj();

            //Miesiące i lata
            int rok = 0;
            int pusty = 0;
            foreach (var m in DateTimeFormatInfo.CurrentInfo.MonthNames)
                pusty = cbxMiesiac.Items.Add(m);

            cbxMiesiac.Items.RemoveAt(pusty);
            foreach (var d in Enumerable.Range(1930, DateTime.Now.Year - 1930+1).ToList())
                rok = cbxLata.Items.Add(d);

            cbxMiesiac.SelectedIndex = DateTime.Now.Month-1;
            cbxLata.SelectedIndex = rok;

            //Okresy
            cbxOkres.Items.Add("Miesiąc");
            cbxOkres.Items.Add("Kwartał");
            cbxOkres.Items.Add("Pół roku");
            cbxOkres.Items.Add("Rok");

            //Kwartały
            cbxKwartał.Items.Add("Kwartał I");
            cbxKwartał.Items.Add("Kwartał II");
            cbxKwartał.Items.Add("Kwartał III");
            cbxKwartał.Items.Add("Kwartał IV");
            cbxKwartał.Visibility = Visibility.Hidden;

            //Półroku
            cbxPolRoku.Items.Add("I połowa");
            cbxPolRoku.Items.Add("II połowa");
            cbxPolRoku.Visibility = Visibility.Hidden;
        }

        public BilansOkno(Wlasciciel wlascicielM) : this()
        {
            wlasciciel = wlascicielM;
            WyswietlCBX();
        }

        private void Ukryj()
        {
            lblWynajem.Content = "Nie wynajmowane";
            lblEmail.Visibility = Visibility.Hidden;
            lblImieNazwisko.Visibility = Visibility.Hidden;
            lblNrKonta.Visibility = Visibility.Hidden;
            tbxEmail.Visibility = Visibility.Hidden;
            tbxImieNazwisko.Visibility = Visibility.Hidden;
            tbxNrKonta.Visibility = Visibility.Hidden;
        }

        private void Odkryj()
        {
            lblWynajem.Content = "Wynajmowane dla:";
            lblEmail.Visibility = Visibility.Visible;
            lblImieNazwisko.Visibility = Visibility.Visible;
            lblNrKonta.Visibility = Visibility.Visible;
            tbxEmail.Visibility = Visibility.Visible;
            tbxImieNazwisko.Visibility = Visibility.Visible;
            tbxNrKonta.Visibility = Visibility.Visible;
        }

        private void WyswietlCBX()
        {
            mieszkania = context.DaneMieszkanias.Where(m => m.idW == wlasciciel.idW).Where(o => o.poczatekWynajmu != null).ToList();
            foreach (var x in mieszkania)
                cbxMieszkania.Items.Add(x);
        }

        private void WyswietlOplaty()
        {
            lbxMieszkania.Items.Clear();

            Ukryj();

            decimal? cenaZaMiesiac = null;
            decimal? kwota = null;
            DateTime data = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), cbxMiesiac.SelectedIndex+1, 1);
            var mieszkanie = (DaneMieszkania)cbxMieszkania.SelectedItem;
            

            if (mieszkanie != null)
            {
                var oferty = context.Ofertas.Where(o => o.idM == mieszkanie.idM).ToList();

                foreach (var o in oferty) { if (o.dataWystawienia <= data) cenaZaMiesiac = (Decimal)o.cenaZaMiesiac; }

                var bilans = context.Bilans.Where(b => b.idM == mieszkanie.idM).Where(b => b.dataTransakcji.Value.Year == data.Year).Where(b => b.dataTransakcji.Value.Month == data.Month).FirstOrDefault();

                if (bilans != null)
                {
                    kwota = (decimal)bilans.kwota;

                    Odkryj();
                    tbxImieNazwisko.Text = $"{bilans.DaneMieszkania.Klient.imie} {bilans.DaneMieszkania.Klient.nazwisko}";
                    tbxEmail.Text = $"{bilans.DaneMieszkania.Klient.email}";
                    tbxNrKonta.Text = $"{bilans.DaneMieszkania.Klient.nrKonta}";
                }

                var dataTransakcji = context.Bilans.Where(b => b.idM == mieszkanie.idM).Where(b => b.dataTransakcji.Value.Year == data.Year).Where(b => b.dataTransakcji.Value.Month == data.Month).Select(x => x.dataTransakcji)
                    .FirstOrDefault();

                if(dataTransakcji != null) lbxMieszkania.Items.Add(dataTransakcji);
                if(cenaZaMiesiac != null) lbxMieszkania.Items.Add("Cena za miesiąc: " + cenaZaMiesiac);
                if(mieszkanie.kosztaRemontow != null) lbxMieszkania.Items.Add("Koszta remontu: " + mieszkanie.kosztaRemontow);
                if(kwota != null) lbxMieszkania.Items.Add("Kwota: " + kwota);
                if (kwota != null && mieszkanie.kosztaRemontow != null) lbxMieszkania.Items.Add("Saldo: " + (kwota - mieszkanie.kosztaRemontow));
            }
        }

        private void btnZakoncz_Click(object sender, RoutedEventArgs e)
        {
            new WlascicielOkno(wlasciciel).Show();
            Close();
        }

        private void cbxMieszkania_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null) 
                WyswietlOplaty();
        }

        private void cbxMiesiac_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null)
                WyswietlOplaty();
            
        }

        private void ObliczRok()
        {
            DateTime pocz = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 1, 1);
            DateTime kon = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 12, 31);

            if (wlasciciel != null)
            {
                decimal? wynik = context.Bilans.Where(b => b.DaneMieszkania.idW == wlasciciel.idW).Where(b => b.dataTransakcji >= pocz && b.dataTransakcji < kon).Sum(s => s.kwota);

                lbxMieszkania.Items.Clear();
                lbxMieszkania.Items.Add($"Saldo za rok: {wynik}");
            }
        }

        private void cbxLata_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxMiesiac.Visibility == Visibility.Visible)
            {
                if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null && cbxMiesiac.SelectedValue != null)
                    WyswietlOplaty();
            }
            else 
            {
                switch (cbxOkres.SelectedIndex)
                {
                    case 0:
                        if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null &&
                            cbxMiesiac.SelectedValue != null)
                        {
                            WyswietlOplaty();
                            Odkryj();
                            lblWynajem.Visibility = Visibility.Visible;
                        } 
                        break;

                    case 1:
                        ObliczKwartal();
                        Ukryj();
                        lblWynajem.Visibility = Visibility.Hidden;
                        break;

                    case 2:
                        ObliczPolRoku();
                        Ukryj();
                        lblWynajem.Visibility = Visibility.Hidden;
                        break;

                    case 3:
                        ObliczRok();
                        Ukryj();
                        lblWynajem.Visibility = Visibility.Hidden;
                        break;
                }
            }
        }

        private void cbxOkres_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbxOkres.SelectedIndex)
            {
                case 0:
                    cbxMiesiac.Visibility = Visibility.Visible;
                    cbxKwartał.Visibility = Visibility.Hidden;
                    cbxPolRoku.Visibility = Visibility.Hidden;

                    if (cbxMieszkania.SelectedValue != null && cbxLata.SelectedValue != null &&
                        cbxMiesiac.SelectedValue != null)
                    {
                        WyswietlOplaty();
                        Odkryj();
                        lblWynajem.Visibility = Visibility.Visible;
                    } 
                    break;

                case 1:
                    cbxMiesiac.Visibility = Visibility.Hidden;
                    cbxKwartał.Visibility = Visibility.Visible;
                    cbxPolRoku.Visibility = Visibility.Hidden;

                    ObliczKwartal();
                    Ukryj();

                    lblWynajem.Visibility = Visibility.Hidden;
                    break;

                case 2:
                    cbxMiesiac.Visibility = Visibility.Hidden;
                    cbxKwartał.Visibility = Visibility.Hidden;
                    cbxPolRoku.Visibility = Visibility.Visible;

                    ObliczPolRoku();
                    Ukryj();

                    lblWynajem.Visibility = Visibility.Hidden;
                    break;

                case 3:
                    cbxMiesiac.Visibility = Visibility.Hidden;
                    cbxKwartał.Visibility = Visibility.Hidden;
                    cbxPolRoku.Visibility = Visibility.Hidden;

                    ObliczRok();
                    Ukryj();

                    lblWynajem.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void ObliczKwartal()
        {
            DateTime pocz = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 1, 1);
            DateTime kon = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 12, 31);

            switch (cbxKwartał.SelectedIndex)
            {
                case 0:
                    kon = pocz.AddMonths(3);
                    break;

                case 1:
                    pocz = pocz.AddMonths(3);
                    kon = pocz.AddMonths(3);
                    break;

                case 2:
                    pocz = pocz.AddMonths(6);
                    kon = pocz.AddMonths(3);
                    break;

                case 3:
                    pocz = pocz.AddMonths(9);
                    kon = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 12, 31);
                    break;
            }

            if (wlasciciel != null)
            {
                decimal? wynik = context.Bilans.Where(b => b.DaneMieszkania.idW == wlasciciel.idW).Where(b => b.dataTransakcji >= pocz && b.dataTransakcji < kon).Sum(s => s.kwota);
                lbxMieszkania.Items.Clear();
                lbxMieszkania.Items.Add($"Saldo za {cbxKwartał.SelectedValue.ToString()}: {wynik}");
            }
        }

        private void cbxKwartał_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObliczKwartal();
        }

        private void ObliczPolRoku()
        {
            DateTime pocz = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 1, 1);
            DateTime kon = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 12, 31);
            switch (cbxPolRoku.SelectedIndex)
            {
                case 0:
                    kon = pocz.AddMonths(6);
                    break;

                case 1:
                    pocz = pocz.AddMonths(6);
                    kon = new DateTime(Convert.ToInt32(cbxLata.SelectedValue), 12, 31);
                    break;
            }
            
            if (wlasciciel != null)
            {
                decimal? wynik = context.Bilans.Where(b => b.DaneMieszkania.idW == wlasciciel.idW).Where(b => b.dataTransakcji >= pocz && b.dataTransakcji < kon).Sum(s => s.kwota);

                lbxMieszkania.Items.Clear();
                lbxMieszkania.Items.Add($"Saldo za {cbxPolRoku.SelectedValue.ToString()} pół roku: {wynik}");
            }
        }
        private void cbxPolRoku_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ObliczPolRoku();
        }
    }
}
