using System;
using System.Collections.Generic;
using System.Globalization;
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
	/// Interaction logic for ChanceSettings.xaml
	/// </summary>
	public partial class ChanceSettings : Window
	{
		ConnectionXML connection;
		public ChanceSettings(ConnectionXML conn)
		{
			InitializeComponent();
			this.connection = conn;
			if (this.connection.Chance == null)
			{
				this.connection.Chance = new Chance();
				this.connection.Chance.Text = "Опитай шанса си";
			}
			
			this.txtProbability.Text = this.connection.Chance.Probability.ToString();
			
			this.txtText.Text = this.connection.Chance.Text;
			this.connection.Type = ConnectionTypes.eChance;
		}

		private void btnOk_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.connection.Chance.Text = this.txtText.Text;
			var style = NumberStyles.Number;
			var culture = CultureInfo.CreateSpecificCulture("en-GB");
			Double.TryParse(this.txtProbability.Text, style, culture, out this.connection.Chance.Probability);
			//double.TryParse(this.txtProbability.Text, out this.connection.Chance.Probability);
			this.Close();
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
}
