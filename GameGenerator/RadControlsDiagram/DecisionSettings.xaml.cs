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
	/// Interaction logic for DecisionSettings.xaml
	/// </summary>
	public partial class DecisionSettings : Window
	{
		ConnectionXML connection;
		public DecisionSettings(ConnectionXML conn)
		{
			InitializeComponent();
			this.connection = conn;
			if (this.connection.Decision == null)
			{
				this.connection.Decision = new Decision();
				this.connection.Decision.Text = "Continue";
			}
			
			this.txtText.Text = this.connection.Decision.Text;
			
			this.connection.Type = ConnectionTypes.eDecision;
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void btnOk_Click_1(object sender, RoutedEventArgs e)
		{
			this.connection.Decision.Text = this.txtText.Text;
			this.DialogResult = true;
			this.Close();
		}
	}
}
