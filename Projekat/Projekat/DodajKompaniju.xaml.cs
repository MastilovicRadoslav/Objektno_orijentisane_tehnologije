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

namespace Projekat
{
    /// <summary>
    /// Interaction logic for IzmeniKompaniju.xaml
    /// </summary>
    public partial class DodajKompaniju : Window
    {
        internal Kompanija kompanija;

        internal DodajKompaniju(Kompanija k)
        {
            kompanija = k;
            InitializeComponent();
        }

       

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (nazivDodaj.Text == "" || sedisteDodaj.Text == "" || pibDodaj.Text == "" || datumDodaj.Text == "" || logoDodaj.Text == "")
                MessageBox.Show("Niste popunili sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (!Int32.TryParse(pibDodaj.Text, out int pib))
                {
                    MessageBox.Show("Pib mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                else if (kompanija.Dodaj(new Kompanija()
                {
                    Pib = pib,
                    Naziv = nazivDodaj.Text,
                    Sediste = sedisteDodaj.Text,
                    DatumOsnivanja = datumDodaj.Text,
                    Logo = logoDodaj.Text

                }))
                {
                    MessageBox.Show("Uspešno dodavanje!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    kompanija.SnimiTextKompanija("Kompanije.txt");
                }

                else
                    MessageBox.Show("Neuspešno dodavanje!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                this.Close();
            }
        }

    }
}
