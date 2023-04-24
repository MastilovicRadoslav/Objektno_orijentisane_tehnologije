using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projekat
{

    public partial class MainWindow : Window
    {
        
      
        Point startPoint = new Point();
        Point _startPoint = new Point();


        Kompanija k = new Kompanija();
        Zaposleni z = new Zaposleni();
        ObservableCollection<Kompanija> kmp = new ObservableCollection<Kompanija>();
       

        public MainWindow()
        {

            InitializeComponent();

            k.Import("Kompanije.txt");
            z.Import("Zaposleni.txt");
            
            MyListView.ItemsSource = k.Kompanije;

            Kompanija[] komp = new Kompanija[k.Kompanije.Count];

            for (int s = 0; s < k.Kompanije.Count; s++)
                komp[s] = new Kompanija() { Naziv = k.Kompanije[s].Naziv };

            int j = 0;
            int i = 0;
            while (i < k.Kompanije.Count)
            {
                while (j < z.ZaposleniRadnici.Count)
                {
                    if (!komp[i].RadniciKompanije.Contains(z.ZaposleniRadnici[j]) )
                    {
                            komp[i].RadniciKompanije.Add(new Zaposleni() {Jmbg = z.ZaposleniRadnici[j].Jmbg, Ime = z.ZaposleniRadnici[j].Ime, Prezime = z.ZaposleniRadnici[j].Prezime, Pol = z.ZaposleniRadnici[j].Pol, DatumRodjenja = z.ZaposleniRadnici[j].DatumRodjenja, ProfilnaSlika = z.ZaposleniRadnici[j].ProfilnaSlika});
                            j++;   
                    }
                    break;
                }
                kmp.Add(komp[i]);

                i++;
                if (i == k.Kompanije.Count)
                    i = 0;
               
                if (j == z.ZaposleniRadnici.Count)
                    break;
            }

            for (int n = 0; n < kmp.Count - 1; n++)
                for (int m = n + 1; m < kmp.Count; m++)
                    if (kmp[n].Naziv == kmp[m].Naziv)
                        kmp.RemoveAt(m);

            trvZaposleni.ItemsSource = kmp;
            trvZaposleniPrikaz.ItemsSource = kmp;
              
        }

        #region Tab 1

            #region Drag&drop 
		//Slika svijeta na nju prevlacimo
        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)  //ako nije dobar formati i ako nisu dobri podaci
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))		  //ako je format isti
            {
                Kompanija kompanija = e.Data.GetData("myFormat") as Kompanija;			 //prebaci ga tu		    objekat Data metode GetData

                double x = Math.Floor(e.GetPosition(slika).X);
                double y = Math.Floor(e.GetPosition(slika).Y);


                logoKompanije.Visibility = Visibility.Visible;

                Canvas.SetLeft(logoKompanije, x);
                Canvas.SetTop(logoKompanije, y);

                int j = 0;
                for (int i = 0; i < k.Kompanije.Count; i++)
                    if (MyListView.SelectedIndex == i)
                    {
                        BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        bi.UriSource = new Uri(k.Kompanije[i].Logo, UriKind.Relative);
                        bi.EndInit();
                        logoKompanije.Stretch = Stretch.Fill;
                        logoKompanije.Source = bi;
                        
						//onemogucavam onaj koji je prebacen 
                        ListViewItem item = MyListView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                        item.IsEnabled = false;

						//prolazimo kroz sve ostale kompanije i sve omogucavamo osim onog koji je selektovan
                        while (j < k.Kompanije.Count)
                        {
                            ListViewItem item2 = MyListView.ItemContainerGenerator.ContainerFromIndex(j) as ListViewItem;
                            if (item != item2)
                                item2.IsEnabled = true;

                            j++;
                        }
                    }
            }
        }
		 //listView   ----> iz njega prevlacimo
        private void MyListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			// Pohraniti poziciju miša 
			startPoint = e.GetPosition(null);	   //Point je koordinata, GetPosition vraca relativnu poziciju misa kada uradimo klik i to nam je startPoint odakle pocinjemo Drag and Drop
        }

        private void MyListView_MouseMove(object sender, MouseEventArgs e)
        {
			// Dobivamo trenutnu poziciju miša 
			Point mousePos = e.GetPosition(null);			//globalna pozicija u odnosu na cijeli ekran, x i  y koridnate
            Vector diff = startPoint - mousePos;		  //od trenutka kada smo kliknuli do trenutka(mouseMove se izvrsava dok se pomjeramo) i onda u svakom trenutku imamo diff vektor koji odredjuje koliko su ove dvije tacke udaljene

            if (e.LeftButton == MouseButtonState.Pressed &&		  //ako je kliknut lijevi button i ako je						USLOV DA NISMO OTISLI VAN EKRANA
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||   //aposlutna vr X veca od MinimumHorizontalDragDistance
				Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
				// Dobijte prevučeni ListViewItem 
				ListView listView = sender as ListView;	  //prolazimo kroz listView i gledmao koji je selektovan ,                   TO STO PREVLACIMO KASTUJEMO U listView
                ListViewItem listViewItem =	  //da bi smo dobili listViewItem koji se pravlaci onaj koji prevlacimo -- dobijamo podatak koji prevlacimo
                    FindAncestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
					// Pronađite podatke iza kontakta ListViewItem 
					Kompanija kompanija = (Kompanija)listView.ItemContainerGenerator.	//onda taj selektovani stavimo u kontenjer i kastujemo u nasu klasiu Kompanija da bi smo uzeli format za dalje
                    ItemFromContainer(listViewItem);

					// Inicijaliziraj drag & drop operaciju 
					DataObject dragData = new DataObject("myFormat", kompanija);  //povezujemo sa odredjenim formatom - string
                    DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);	// podatak, format i dozvoljeni efekat
                }
            }
        }
		// Pomoćnik za pretraživanje 
		private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }


        private void logoKompanije_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double x = Math.Floor(e.GetPosition(slika).X);
                double y = Math.Floor(e.GetPosition(slika).Y);

                Canvas.SetLeft(logoKompanije, x);
                Canvas.SetTop(logoKompanije, y);
            }
        }

        #endregion

             #region Menu
        private void MenuItemDodajKompaniju_Click(object sender, RoutedEventArgs e)
        {
            DodajKompaniju ik = new DodajKompaniju(k);
            ik.Show();
        }

        private void MenuItemUkloniLogo_Click(object sender, RoutedEventArgs e)
        {
            logoKompanije.Visibility = Visibility.Hidden;
            for (int i = 0; i < k.Kompanije.Count; i++)
            {
                ListViewItem item = MyListView.ItemContainerGenerator.ContainerFromIndex(i) as ListViewItem;
                item.IsEnabled = true;
            }
        }

        private void MenuItemObrisiKompaniju_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Da li ste sigurni da zelite da obriste kompaniju?",
                    "Obrisi kompaniju!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                for (int i = 0; i < k.Kompanije.Count; i++)
                {
                        if (MyListView.SelectedIndex == i)
                        {
                            k.Kompanije.RemoveAt(i);
                            kmp.RemoveAt(i);
						//z.ZaposleniRadnici.RemoveAt(i);
						//k.RadniciKompanije.RemoveAt(i);
                            k.SnimiTextKompanija("Kompanije.txt");

                            logoKompanije.Visibility = Visibility.Hidden;
                        }
                }
            }
        }

        private void MenuItemIzmeniKompaniju_Click(object sender, RoutedEventArgs e)
        {
            int poz = 0;

            for (int i = 0; i < k.Kompanije.Count; i++)
            {
                if (MyListView.SelectedIndex == i)
                {
                    poz = i;

                }
            }

            IzmeniKompaniju ik = new IzmeniKompaniju(k, poz);

            ik.Show();

            ik.pibIzmena.Text = k.Kompanije[poz].Pib.ToString();
            ik.nazivIzmena.Text = k.Kompanije[poz].Naziv;
            ik.sedisteIzmena.Text = k.Kompanije[poz].Sediste;
            ik.datumIzmena.Text = k.Kompanije[poz].DatumOsnivanja;
            ik.logoIzmena.Text = k.Kompanije[poz].Logo;
        }
        #endregion

        #endregion

        #region Tab 2

            #region Drag&Drop
        private void trvZaposleni_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
        }
      
        private void trvZaposleni_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = _startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                TreeView treeView = sender as TreeView;
                TreeViewItem treeViewItem =
                    FindAncestor<TreeViewItem>((DependencyObject)e.OriginalSource);

                if (treeViewItem != null)
                {
                    Object temp = (Object)treeView.ItemContainerGenerator.ItemFromContainer(treeViewItem);
                DataObject dragData = new DataObject("myFormat", temp);

                DragDrop.DoDragDrop(treeViewItem, dragData, DragDropEffects.Move);
                }
            }

        }

        private void slikaTab2_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void slikaTab2_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {

                double x = Math.Floor(e.GetPosition(slikaTab2).X);
                double y = Math.Floor(e.GetPosition(slikaTab2).Y);



                prSlika.Visibility = Visibility.Visible;

                Canvas.SetLeft(prSlika, x);
                Canvas.SetTop(prSlika, y);

                int j = 0;
                for (int i = 0; i < z.ZaposleniRadnici.Count; i++)
                    if (trvZaposleni.Items.IndexOf(trvZaposleni.SelectedItem) == i)
                    {
                        BitmapImage bi3 = new BitmapImage();
                        bi3.BeginInit();
                        bi3.UriSource = new Uri(z.ZaposleniRadnici[i].ProfilnaSlika, UriKind.Relative);
                        bi3.EndInit();
                        prSlika.Stretch = Stretch.Fill;
                        prSlika.Source = bi3;

                        TreeViewItem item = trvZaposleni.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                        item.IsEnabled = false;

                        while (j < k.Kompanije.Count)
                        {
                            TreeViewItem item2 = trvZaposleni.ItemContainerGenerator.ContainerFromIndex(j) as TreeViewItem;
                            if (item != item2)
                                item2.IsEnabled = true;
                            
                            j++;
                        }
                    }
            }
        }

        private void prifilnaSlika_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) 
            {
                double x = Math.Floor(e.GetPosition(slikaTab2).X);
                double y = Math.Floor(e.GetPosition(slikaTab2).Y);

                Canvas.SetLeft(prSlika, x);
                Canvas.SetTop(prSlika, y);
            }
        }
        #endregion

            #region Menu
        private void MenuItemDodajZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            DodajZaposlenog iz = new DodajZaposlenog(z);
            iz.Show();
        }

        private void MenuItemDodajUkloniProfilnuSliku_Click(object sender, RoutedEventArgs e)
        {
            prSlika.Visibility = Visibility.Hidden;
            for (int i = 0; i < k.Kompanije.Count; i++)
            {
                if (trvZaposleni.Items.IndexOf(trvZaposleni.SelectedItem) == i)
                {
                    TreeViewItem item = trvZaposleni.ItemContainerGenerator.ContainerFromIndex(i) as TreeViewItem;
                    item.IsEnabled = true;
                }
            }
        }

        private void MenuItemIzmeniZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = 0;

            for (int i = 0; i < z.ZaposleniRadnici.Count; i++)
            {
                if (trvZaposleni.Items.IndexOf(trvZaposleni.SelectedItem) == i)
                {
                    selectedIndex = i;

                }
            }

            IzmeniZaposlenog iz = new IzmeniZaposlenog(z, selectedIndex);

            iz.Show();

            iz.tbIme.Text = z.ZaposleniRadnici[selectedIndex].Ime;
            iz.tbPrezime.Text = z.ZaposleniRadnici[selectedIndex].Prezime;
            iz.tbJmbg.Text = z.ZaposleniRadnici[selectedIndex].Jmbg.ToString();
            iz.tbPol.Text = z.ZaposleniRadnici[selectedIndex].Pol;
            iz.tbDatumRodjenja.Text = z.ZaposleniRadnici[selectedIndex].DatumRodjenja;
            iz.tbProfilnaSlika.Text = z.ZaposleniRadnici[selectedIndex].ProfilnaSlika;

        }

        private void MenuItemObrisiZaposlenog_Click(object sender, RoutedEventArgs e)
        {
            

            if (MessageBox.Show("Da li ste sigurni da zelite da obriste zaposlenog?",
                    "Obrisi zaposlenog!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                for (int i = 0; i < z.ZaposleniRadnici.Count; i++)
                {
                    if (trvZaposleni.Items.IndexOf(trvZaposleni.SelectedItem) == i)
                    {
                        z.ZaposleniRadnici.RemoveAt(i);
                        z.SnimiTextZaposleni("Zaposleni.txt");

                        prSlika.Visibility = Visibility.Hidden;
                    }
                }
            }
            
        }

        #endregion

        #endregion

       
    }
}
