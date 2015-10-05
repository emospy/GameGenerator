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
	/// Interaction logic for ConditionSettings.xaml
	/// </summary>
	public partial class RollBackSettings : Window
	{
		ConnectionXML connection;
		public RollBackSettings(ConnectionXML conn)
		{
			InitializeComponent();
			this.connection = conn;
			//this.connection.Type = ConnectionTypes.eCondition;
			if (this.connection.ChanceRollback == null)
			{
				this.connection.ChanceRollback = new GameClasses.ChanceRollBack();
				this.connection.ChanceRollback.Predicates = new List<Predicate>();
			}
			else
			{
				this.txtText.Text = this.connection.ChanceRollback.Text;
			}

			this.dgPredicates.ItemsSource = this.connection.ChanceRollback.Predicates;
			var lstItems = new List<ItemStrings>();
			lstItems.AddRange(Globals.GameElements.Items);
			lstItems.AddRange(Globals.GameElements.Stats);
			lstItems.AddRange(Globals.GameElements.Skills);
			this.dgcmbPredicateName.ItemsSource = lstItems;

			List<PredicateTypeList> Dict = new List<PredicateTypeList>();
			Dict.Add(new PredicateTypeList{ Key = PredicateTypes.eInventory, Value = "Inventory"});
			Dict.Add(new PredicateTypeList { Key = PredicateTypes.eStat, Value = "Stat"});
			Dict.Add(new PredicateTypeList{ Key = PredicateTypes.eSkill, Value = "Skill"});
			this.dgcmbPredicateType.ItemsSource = Dict;
		}

		private void btnOk_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.connection.ChanceRollback.Text = this.txtText.Text;
			this.Close();
		}

		private void btnCancel_Click_1(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void btnAdd_Click_1(object sender, RoutedEventArgs e)
		{
			var predicate = new Predicate();
			this.connection.ChanceRollback.Predicates.Add(predicate);
			this.dgPredicates.ItemsSource = null;
			this.dgPredicates.ItemsSource = this.connection.ChanceRollback.Predicates;
			this.dgPredicates.Items.Refresh();
		}

		private void btnDelete_Click_1(object sender, RoutedEventArgs e)
		{
			if (this.dgPredicates.SelectedItem != null)
			{
				this.connection.ChanceRollback.Predicates.Remove((Predicate)this.dgPredicates.SelectedItem);
				this.dgPredicates.ItemsSource = null;
				this.dgPredicates.ItemsSource = this.connection.ChanceRollback.Predicates;
				this.dgPredicates.Items.Refresh();
			}
		}

		private class PredicateTypeList
		{
			public PredicateTypes Key { get; set; }
			public string Value { get; set; }
		}
	}
}
