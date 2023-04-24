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
    /// Interaction logic for IzmeniZaposlenog.xaml
    /// </summary>
    public partial class IzmeniZaposlenog : Window
    {
        private Zaposleni zaposleni;
        private int i;
        internal IzmeniZaposlenog(Zaposleni z, int index)
        {
            zaposleni = z;
            i = index;
            InitializeComponent();
     
        }

        private void btnIzmeniZaposlenog_Click(object sender, RoutedEventArgs e)
        {

            if (tbIme.Text == "" || tbPrezime.Text == "" || tbPol.Text == "" || tbProfilnaSlika.Text == "")
                MessageBox.Show("Niste popunili sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else if (zaposleni.IzmeniZaposlenog(new Zaposleni() { Ime = tbIme.Text, Prezime = tbPrezime.Text, Pol = tbPol.Text, ProfilnaSlika = tbProfilnaSlika.Text }, i))
            {
                MessageBox.Show("Uspešno izmena!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                zaposleni.SnimiTextZaposleni("Zaposleni.txt");
                this.Close();
            }
            else
                MessageBox.Show("Neuspešno izmena!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

            

        }
    }
}
