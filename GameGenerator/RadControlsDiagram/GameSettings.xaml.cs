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
	/// Interaction logic for GameSettings.xaml
	/// </summary>
	public partial class GameSettings : Window
	{
		List<StartupStats> lstStats; 
		public GameSettings(List<StartupStats> lst)
		{
			InitializeComponent();
			this.lstStats = lst;
			this.dgItems.ItemsSource = this.lstStats;
		}

		private void btnAdd_Click_1(object sender, RoutedEventArgs e)
		{
			this.lstStats.Add(new StartupStats());
			this.dgItems.ItemsSource = null;
			this.dgItems.ItemsSource = this.lstStats;
			this.dgItems.Items.Refresh();
		}

		private void btnDelete_Click_1(object sender, RoutedEventArgs e)
		{
			if (this.dgItems.SelectedItem != null)
			{
				this.lstStats.Remove((StartupStats)this.dgItems.SelectedItem);
				this.dgItems.ItemsSource = null;
				this.dgItems.ItemsSource = this.lstStats;
				this.dgItems.Items.Refresh();
			}
		}
	}
}
