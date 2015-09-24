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
using GameClasses;
using Microsoft.Win32;
using System.IO;
using System.Drawing;
using System.Windows.Markup;

namespace RadControlsDiagram
{
    /// <summary>
    /// Interaction logic for EpizodeProperties.xaml
    /// </summary>
    public partial class EpizodeProperties : Window
    {
        Epizode Epizode;
        public EpizodeProperties(Epizode E)
        {
            InitializeComponent();
            if (E == null)
            {
                this.Epizode = new Epizode();
            }
            else
            {
                this.Epizode = E;
                this.txtEpizodeText.Text = this.Epizode.EpizodeText;
                this.txtEpizodeNumber.Text = this.Epizode.EpizodeNumber.ToString();
            }
            this.dgcmbStats.ItemsSource = Globals.GameElements.Stats;
            this.dgcmbItems.ItemsSource = Globals.GameElements.Items;
            this.dgInventories.ItemsSource = this.Epizode.lstInventories;
            this.dgStats.ItemsSource = this.Epizode.lstStats;

            if (this.Epizode.LargeIcon != null)
            {
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = new MemoryStream(this.Epizode.LargeIconSerialized);
                bitmapimage.EndInit();
                this.imgBox.Source = bitmapimage;

            }
        }

        private void btnEditInventoryList_Click_1(object sender, RoutedEventArgs e)
        {
            StringNomenklature win = new StringNomenklature(Globals.GameElements.Items);
            win.ShowDialog();
        }

        private void btnEditStatList_Click_1(object sender, RoutedEventArgs e)
        {
            StringNomenklature win = new StringNomenklature(Globals.GameElements.Stats);
            win.ShowDialog();
        }

        private void btnAddItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Epizode.lstInventories.Add(new Inventory());
            this.dgInventories.ItemsSource = null;
            this.dgInventories.ItemsSource = this.Epizode.lstInventories;
        }

        private void btnDeleteInventoryItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.dgInventories.SelectedItem != null)
            {
                this.Epizode.lstInventories.Remove((Inventory)this.dgInventories.SelectedItem);
                this.dgInventories.ItemsSource = null;
                this.dgInventories.ItemsSource = this.Epizode.lstInventories;
            }
        }

        private void btnAddStatItem_Click_1(object sender, RoutedEventArgs e)
        {
            this.Epizode.lstStats.Add(new Stat());
            this.dgStats.ItemsSource = null;
            this.dgStats.ItemsSource = this.Epizode.lstStats;
        }

        private void btnDeleteStatItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (this.dgStats.SelectedItem != null)
            {
                this.Epizode.lstStats.Remove((Stat)this.dgStats.SelectedItem);
                this.dgStats.ItemsSource = null;
                this.dgStats.ItemsSource = this.Epizode.lstStats;
            }
        }

        private void btnSave_Click_1(object sender, RoutedEventArgs e)
        {
            int.TryParse(this.txtEpizodeNumber.Text, out this.Epizode.EpizodeNumber);
            this.Epizode.EpizodeText = this.txtEpizodeText.Text;
            this.DialogResult = true;

            //Bitmap src = (Bitmap)this.imgBox.Source;

            //MemoryStream stream = new MemoryStream();
            //BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(src.UriSource));
            //encoder.Save(stream);
            //stream.Flush();
            //this.Epizode.LargeIcon = src.;

            this.Close();
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnChoosePicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == false)
            {
                return;
            }

            //BitmapImage myBitmapImage = new BitmapImage();

            //myBitmapImage.BeginInit();
            //myBitmapImage.UriSource = new Uri(ofd.FileName);
            //myBitmapImage.EndInit();

            FileStream fs = new FileStream(ofd.FileName, FileMode.Open);

            var a = new Bitmap(fs);
            this.Epizode.LargeIcon = a;

            using (MemoryStream memory = new MemoryStream())
            {
                a.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                this.imgBox.Source = bitmapimage;
            }
        }
    }
}
