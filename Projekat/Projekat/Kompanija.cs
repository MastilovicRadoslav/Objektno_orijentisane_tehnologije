using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace Projekat
{

	class Kompanija : INotifyPropertyChanged
	{
		private int pib;
		private string naziv;
		private string sediste;
		private string datumOsnivanja;
		private string logo;
		private ObservableCollection<Kompanija> kompanije;
		private ObservableCollection<Zaposleni> radniciKompanije;

		public event PropertyChangedEventHandler PropertyChanged; //okida se kad got se pozove neki propert

		public virtual void OnPropertyChanged(string name)			//metoda za mijenjanje  polja propertija klase preko Binding
		{
			if (this.PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		#region Konstruktori
		public Kompanija()
		{
			radniciKompanije = new ObservableCollection<Zaposleni>();
			kompanije = new ObservableCollection<Kompanija>();
		}
		public Kompanija(int pib, string naziv, string sediste, string datumOsnivanja, string logo)
		{
			this.pib = pib;
			this.naziv = naziv;
			this.sediste = sediste;
			this.datumOsnivanja = datumOsnivanja;
			this.logo = logo;

		}

		#endregion

		#region Propetiji
		public int Pib
		{
			get
			{
				return pib;
			}
			set
			{
				if (value != pib)
				{
					pib = value;
					OnPropertyChanged("Pib");
				}
			}
		}
		public string Naziv
		{
			get
			{
				return naziv;
			}
			set
			{
				if (value != naziv)
				{
					naziv = value;
					OnPropertyChanged("Naziv");
				}
			}
		}
		public string Sediste
		{
			get
			{
				return sediste;
			}
			set
			{
				if (value != sediste)
				{
					sediste = value;
					OnPropertyChanged("Sediste");
				}
			}
		}
		public string DatumOsnivanja
		{
			get
			{
				return datumOsnivanja;
			}
			set
			{
				if (value != datumOsnivanja)
				{
					datumOsnivanja = value;
					OnPropertyChanged("DatumOsnivanja");
				}
			}
		}

		public string Logo
		{
			get
			{
				return logo;
			}
			set
			{
				if (value != logo)
				{
					logo = value;
					OnPropertyChanged("Logo");
				}
			}
		}

		public ObservableCollection<Kompanija> Kompanije
        {
			get
			{
				return kompanije;
			}
			set
			{
				kompanije = value;
			}
		}

		public ObservableCollection<Zaposleni> RadniciKompanije
		{
			get
			{
				return radniciKompanije;
			}
			set
			{
				radniciKompanije = value;
			}
		}
        #endregion

        #region Ucitaj/Snimi fajl
        public void SnimiTextKompanija(string file)
		{
			StreamWriter sw = null;

			try
			{
				sw = new StreamWriter(file);
				foreach(var item in kompanije)
                {
					sw.WriteLine(item.ToString());
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
			str += pib + " " + naziv + " " + sediste + " " + datumOsnivanja + " " + logo;
			return str;
        }


        public void Import(string file)
		{
			StreamReader sr = null;


			try
			{
				sr = new StreamReader(file);
				string linija;

				while ((linija = sr.ReadLine()) != null)
				{
					string[] lineParts = linija.Split(' ');

					int pib;
					string naziv;
					string sediste;
					string datumOsnivanja;
					string logo;

					pib = Int32.Parse(lineParts[0]);
					naziv = lineParts[1];
					sediste = lineParts[2];
					datumOsnivanja = lineParts[3];
					logo = lineParts[4];

					if (!pibPostoji(pib))
					{
						kompanije.Add(new Kompanija(pib, naziv, sediste, datumOsnivanja,logo));
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

		private bool pibPostoji(int pib)
		{
			foreach (Kompanija s in kompanije)
			{
				if (s.pib == pib)
				{
					return true;
				}
			}
			return false;
		}
        #endregion

        #region Dodaj/Izmeni
        public bool Dodaj(Kompanija k)
		{
			foreach (Kompanija kat in Kompanije)
			{
				if (kat.Pib.Equals(k.Pib))
					return false;
			}

			Kompanije.Add(k);
			return true;
		}

		public bool Izmeni(Kompanija k, int index)
		{
			int r = 0;
			foreach (Kompanija kat in Kompanije)
			{
				if (r == index)
				{
					kat.Naziv = k.Naziv;
					kat.Sediste = k.sediste;
					kat.Logo = k.logo;

					return true;
				}
				r++;
			}
			return false;
		}
        #endregion
    }
}
