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
    /// Interaction logic for IzmenaZaposlenog.xaml
    /// </summary>
    public partial class DodajZaposlenog : Window
    {
        private Zaposleni zap;
        internal DodajZaposlenog(Zaposleni z)
        {
            zap = z;
            InitializeComponent();
        }

        private void btnDodajZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            if (tbIme.Text == "" || tbPrezime.Text == "" || tbPol.Text == "" || tbJmbg.Text == "" || tbDatumRodjenja.Text == "" || tbProfilnaSlika.Text == "")
                MessageBox.Show("Niste popunili sva polja!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (!Int32.TryParse(tbJmbg.Text, out int jmbg))
                {
                    MessageBox.Show("Jmbg mora biti broj!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (zap.DodajZaposlenog(new Zaposleni()
                {
                    Jmbg = jmbg,
                    Ime = tbIme.Text,
                    Prezime = tbPrezime.Text,
                    Pol = tbPol.Text,
                    DatumRodjenja = tbDatumRodjenja.Text,
                    ProfilnaSlika = tbProfilnaSlika.Text
                }))
                {
                    MessageBox.Show("Uspešno dodavanje!", "Obaveštenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    zap.SnimiTextZaposleni("Zaposleni.txt");
                }
                else
                    MessageBox.Show("Neuspešno dodavanje!", "Greška", MessageBoxButton.OK, MessageBoxImage.Error);

                this.Close();
            }

           
        }
    }
}
