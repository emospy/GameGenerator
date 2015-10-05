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
	/// Interaction logic for ChooseConnectionType.xaml
	/// </summary>
	public partial class ChooseConnectionType : Window
	{
		public ConnectionTypes SelectedIndex
		{
			get { return (ConnectionTypes)this.cmbType.SelectedIndex; }
		}

		public ChooseConnectionType()
		{
			InitializeComponent();
            this.cmbType.SelectedIndex = 0;
		}

		private void btnOk_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
	}
}
