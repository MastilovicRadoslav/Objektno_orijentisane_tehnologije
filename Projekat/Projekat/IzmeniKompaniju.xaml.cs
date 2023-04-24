using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class IzmeniKompaniju : Window
    {
        private Kompanija kompanija;
        int r;

        internal IzmeniKompaniju(Kompanija k, int i)
        {
            kompanija = k;
            r = i;

            InitializeComponent();
        }
        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (nazivIzmena.Text == "" || sedisteIzmena.Text == "" || logoIzmena.Text == "")
                MessageBox.Show("Niste popunili sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (kompanija.Izmeni(new Kompanija() { Naziv = nazivIzmena.Text, Sediste = sedisteIzmena.Text, Logo = logoIzmena.Text }, r))
            {
                MessageBox.Show("Uspešno izmena!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                kompanija.SnimiTextKompanija("Kompanije.txt");
            }

            else
                MessageBox.Show("Neuspešno izmena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            this.Close();

        }
    }
}
