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
	/// Interaction logic for StringNomenklature.xaml
	/// </summary>
	public partial class StringNomenklature : Window
	{
		List<ItemStrings> lstItems;
		public StringNomenklature(List<ItemStrings> Items)
		{
			this.lstItems = Items;
			InitializeComponent();
			this.dgItems.ItemsSource = this.lstItems;
		}

		private void btnAdd_Click_1(object sender, RoutedEventArgs e)
		{
			this.lstItems.Add(new ItemStrings());
			this.dgItems.ItemsSource = null;
			this.dgItems.ItemsSource = lstItems;
			this.dgItems.Items.Refresh();
		}

		private void btnDelete_Click_1(object sender, RoutedEventArgs e)
		{
			if (this.dgItems.SelectedItem != null)
			{
				this.lstItems.Remove((ItemStrings)this.dgItems.SelectedItem);
				this.dgItems.ItemsSource = null;
				this.dgItems.ItemsSource = lstItems;
				this.dgItems.Items.Refresh();
			}
		}
	}
}
