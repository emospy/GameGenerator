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
	/// Interaction logic for BattleSettings.xaml
	/// </summary>
	public partial class BattleSettings : Window
	{
		ConnectionXML connection;
		public BattleSettings(ConnectionXML conn)
		{
			InitializeComponent();
			this.connection = conn;
			if (this.connection.Battle == null)
			{
				this.connection.Battle = new Battle();
				this.connection.Battle.Text = "Бий се!";
			}
			else
			{
				this.txtHealth.Text = this.connection.Battle.EnemyHealth.ToString();
				this.txtStrength.Text = this.connection.Battle.EnemyStrength.ToString();
				this.txtLose.Text = this.connection.Battle.Lose.ToString();
			}
			this.txtText.Text = this.connection.Battle.Text;
		}

		private void btnOk_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.connection.Battle.Text = this.txtText.Text;
			int.TryParse(this.txtStrength.Text, out this.connection.Battle.EnemyStrength);
			int.TryParse(this.txtHealth.Text, out this.connection.Battle.EnemyHealth);
			int.TryParse(this.txtLose.Text, out this.connection.Battle.Lose);
			this.Close();
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
}
