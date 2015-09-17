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
			this.Close();
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

        private void btnChoosePicture_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
