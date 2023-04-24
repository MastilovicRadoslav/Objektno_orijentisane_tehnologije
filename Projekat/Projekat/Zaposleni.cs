using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Projekat
{
	class Zaposleni :INotifyPropertyChanged
	{
		private int jmbg;
		private string ime;
		private string prezime;
		private string pol;
		private string datumRodjenja;
		private string profilnaSlika;
		private ObservableCollection<Zaposleni> zaposleniRadnici;

		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void OnPropertyChanged(string name)
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

        #region Constructors
        public Zaposleni(int jmbg, string ime, string prezime, string pol, string datumRodjenja, string profilnaSlika)
		{
			this.jmbg = jmbg;
			this.ime = ime;
			this.prezime = prezime;
			this.pol = pol;
			this.datumRodjenja = datumRodjenja;
			this.profilnaSlika = profilnaSlika;
		}

		public Zaposleni()
        {
			zaposleniRadnici = new ObservableCollection<Zaposleni>();
		}

        #endregion

        #region Properties

        public int Jmbg
		{
			get
			{
				return jmbg;
			}
			set
			{
				if (value != jmbg)
				{
					jmbg = value;
					OnPropertyChanged("JMBG");
				}
			}
		}
		public string Ime
		{
			get
			{
				return ime;
			}
			set
			{
				if (value != ime)
				{
					ime = value;
					OnPropertyChanged("Ime");
				}
			}
		}
		public string Prezime
		{
			get
			{
				return prezime;
			}
			set
			{
				if (value != prezime)
				{
					prezime = value;
					OnPropertyChanged("Sediste");
				}
			}
		}
		public string Pol
		{
			get
			{
				return pol;
			}
			set
			{
				if (value != pol)
				{
					pol = value;
					OnPropertyChanged("DatumOsnivanja");
				}
			}
		}

		public string DatumRodjenja
		{
			get
			{
				return datumRodjenja;
			}
			set
			{
				if (value != datumRodjenja)
				{
					datumRodjenja = value;
					OnPropertyChanged("DatumRodjenja");
				}
			}
		}

		public string ProfilnaSlika
		{
			get
			{
				return profilnaSlika;
			}
			set
			{
				if (value != profilnaSlika)
				{
					profilnaSlika = value;
					OnPropertyChanged("Slika");
				}
			}
		}

		public ObservableCollection<Zaposleni> ZaposleniRadnici
		{
			get
			{
				return zaposleniRadnici;
			}
			set
			{
				zaposleniRadnici = value;
			}
		}

		#endregion

		#region Load/Save File

		public void SnimiTextZaposleni(string file)
		{
			StreamWriter sw = null;

			try
			{
				sw = new StreamWriter(file);
				foreach (var item in ZaposleniRadnici)
				{
					sw.WriteLine(item);
				}

				sw.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.StackTrace);
			}
		}

        public override string ToString()
        {
            string str = "";
			str += jmbg + " " + ime + " " + prezime + " " + pol + " " + datumRodjenja + " "+profilnaSlika;
			return str;
		}

        public void Import(string file)
		{
			StreamReader sr = null;
			string linija;

			try
			{
				sr = new StreamReader(file);
				
				while ((linija = sr.ReadLine()) != null)
				{
					string[] lineParts = linija.Split(' ');

					int jmbg;
					string ime;
					string prezime;
					string pol;
					string datumRodjenja;
					string profilnaSlika;

					jmbg = Int32.Parse(lineParts[0]);
					ime = lineParts[1];
					prezime = lineParts[2];
					pol = lineParts[3];
					datumRodjenja = lineParts[4];
					profilnaSlika = lineParts[5];

                    if (!jmbgPostoji(jmbg)) {
						zaposleniRadnici.Add(new Zaposleni(jmbg, ime, prezime, pol, datumRodjenja, profilnaSlika));
					}

				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine(e.StackTrace);
			}
			finally
			{
				if (sr != null)
				{
					sr.Close();
				}
			}
		}

		private bool jmbgPostoji(int jmbg)
		{
			foreach (Zaposleni s in zaposleniRadnici)
			{
				if (s.jmbg == jmbg)
				{
					return true;
				}
			}
			return false;
		}

        #endregion

        #region Change_Add

        public bool IzmeniZaposlenog(Zaposleni z, int index)
		{
			int i= 0;
			foreach (Zaposleni zap in ZaposleniRadnici)
			{
				if (i == index)
				{
					zap.Ime = z.Ime;
					zap.Prezime = z.Prezime;
					zap.Pol = z.Pol;
					zap.ProfilnaSlika = z.ProfilnaSlika;

					return true;
				}
				i++;
			}
			return false;
		}

		public bool DodajZaposlenog(Zaposleni z)
		{
			foreach (Zaposleni kat in ZaposleniRadnici)
			{
				if (kat.Jmbg.Equals(z.Jmbg))
					return false;
			}

			ZaposleniRadnici.Add(z);

			return true;
		}

        #endregion
    }
}
