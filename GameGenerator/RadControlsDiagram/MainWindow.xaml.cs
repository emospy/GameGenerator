using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using GameClasses;
using Microsoft.Win32;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace RadControlsDiagram
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool IgnoreSelection = false;

		static MainWindow()
		{
			var saveBinding = new CommandBinding(DiagramCommands.Delete, ExecuteDelete, CanExecuteDelete);

			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), saveBinding);
			
		}


		public MainWindow()
		{
			InitializeComponent();
			Globals.GameElements = new GameElements();
			Globals.GameElements.Stats = new List<ItemStrings>();
			Globals.GameElements.Items = new List<ItemStrings>();
			Globals.GameElements.Skills = new List<ItemStrings>();
			Globals.GameElements.StartupStats = new List<StartupStats>();
			//diagram.InputBindings.Remove(new KeyBinding(DiagramCommands.Delete, Key.Delete, ModifierKeys.None));
		
		}

		private void btnSelect_Click_1(object sender, RoutedEventArgs e)
		{
			this.diagram.ActiveTool = Telerik.Windows.Diagrams.Core.MouseTool.PointerTool;
		}

		private void btnPan_Click_1(object sender, RoutedEventArgs e)
		{
			this.diagram.ActiveTool = Telerik.Windows.Diagrams.Core.MouseTool.PanTool;
		}

		private void btnConnector_Click_1(object sender, RoutedEventArgs e)
		{
			this.diagram.ActiveTool = Telerik.Windows.Diagrams.Core.MouseTool.ConnectorTool;
		}

		private void diagram_ShapeDoubleClicked_1(object sender, ShapeRoutedEventArgs e)
		{
			RadDiagramShape shape = (RadDiagramShape)e.Shape;
			Epizode Ep = (Epizode)shape.Tag;
			EpizodeProperties win = new EpizodeProperties(Ep);
			var result = win.ShowDialog();
			if (result == true)
			{
				shape.Tag = Ep;
				e.Shape.Content = ((Epizode)shape.Tag).EpizodeNumber;
			}
		}

		private void diagram_ConnectionManipulationCompleted_1(object sender, ManipulationRoutedEventArgs e)
		{
			ChooseConnectionType win = new ChooseConnectionType();
			if (win.ShowDialog() == true)
			{
				var ConnectionContents = new ConnectionXML();
				Window wind = null;
				switch (win.SelectedIndex)
				{
					case ConnectionTypes.eDecision:
						wind = new DecisionSettings(ConnectionContents);
						break;
					case ConnectionTypes.eChance:
						wind = new ChanceSettings(ConnectionContents);
						break;
					case ConnectionTypes.eBattle:
						wind = new BattleSettings(ConnectionContents);
						break;
					case ConnectionTypes.eCondition:
						wind = new ConditionSettings(ConnectionContents);
						break;
					case ConnectionTypes.eInventoryCondition:
						throw new ArgumentOutOfRangeException();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
				ConnectionContents.Type = win.SelectedIndex;
				this.IgnoreSelection = true;

				if (wind != null && wind.ShowDialog() == true)
				{
					e.Connection.TargetCapType = CapType.Arrow1Filled;
					e.Connection.Content = ConnectionContents;
					return;
				}
			}
			this.diagram.RemoveConnection(e.Connection);
		}

		private void btnNew_Click_1(object sender, RoutedEventArgs e)
		{
			RadDiagramShape shape = new RadDiagramShape();
			shape.Tag = new Epizode();
			shape.Position = new Point { X = this.diagram.Viewport.Left, Y = this.diagram.Viewport.Top };
			//shape.
			diagram.AddShape(shape);
		}

		private void btnSave_Click_1(object sender, RoutedEventArgs e)
		{
			foreach (RadDiagramShape sh in this.diagram.Shapes)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(Epizode));

				using (StringWriter writer = new StringWriter())
				{
					serializer.Serialize(writer, sh.Tag);

					sh.Content = writer.ToString();
				}
			}

			foreach (RadDiagramConnection co in this.diagram.Connections)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(ConnectionXML));

				using (StringWriter writer = new StringWriter())
				{
					serializer.Serialize(writer, co.Content);

					co.Content = writer.ToString();
				}
			}

			Stream fileStream = null;
			try
			{
				var dialog = new SaveFileDialog();
				if (dialog.ShowDialog() == true)
				{
					var nam = dialog.FileName;
					XmlSerializer serializer = new XmlSerializer(typeof(GameElements));
					TextWriter writ = new StreamWriter(nam + ".xml");

					serializer.Serialize(writ, Globals.GameElements);
					writ.Close();

					fileStream = dialog.OpenFile();
					using (fileStream)
					{
						var serializationString = this.diagram.Save();
						var writer = new StreamWriter(fileStream);
						writer.Write(serializationString);
						writer.Flush();
					}
				}
			}
			finally
			{
				if (fileStream != null)
					fileStream.Close();
			}
			foreach (RadDiagramShape sh in this.diagram.Shapes)
			{
				sh.Content = ((Epizode)sh.Tag).EpizodeNumber;
			}
			foreach (RadDiagramConnection con in this.diagram.Connections)
			{
				XmlSerializer serializer = new XmlSerializer(typeof(ConnectionXML));

				StringReader sr = new StringReader(con.Content.ToString());
				con.Content = (ConnectionXML)serializer.Deserialize(sr);
			}
		}

		private void btnLoad_Click_1(object sender, RoutedEventArgs e)
		{
			Stream fileStream = null;
			try
			{
				var dialog = new OpenFileDialog();
				dialog.ShowDialog();
				var nam = dialog.FileName;
				XmlSerializer serializer = new XmlSerializer(typeof(GameElements));
				if (System.IO.File.Exists(nam + ".xml") == true)
				{
					FileStream fs = new FileStream(nam + ".xml", FileMode.Open);
					Globals.GameElements = (GameElements)serializer.Deserialize(fs);
					fs.Close();
				}
				using (fileStream = dialog.OpenFile())
				{
					StreamReader reader = new StreamReader(fileStream);
					using (reader)
					{
						string serializedString = reader.ReadToEnd();
						this.diagram.Load(serializedString);
					}
				}
			}
			finally
			{
				if (fileStream != null)
					fileStream.Close();
			}
		}

		void diagram_ShapeDeserialized(object sender, ShapeSerializationRoutedEventArgs e)
		{
			// load the saved property
			XmlSerializer serializer = new XmlSerializer(typeof(Epizode));

			StringReader sr = new StringReader((e.Shape as RadDiagramShape).Content.ToString());
			(e.Shape as RadDiagramShape).Tag = (Epizode)serializer.Deserialize(sr);



			//(e.Shape as RadDiagramShape).Tag = (e.Shape as RadDiagramShape).Content;
			(e.Shape as RadDiagramShape).Content = ((Epizode)(e.Shape as RadDiagramShape).Tag).EpizodeNumber;
		}

		private void btnGameSettings_Click_1(object sender, RoutedEventArgs e)
		{
			GameSettings win = new GameSettings(Globals.GameElements.StartupStats);
			win.ShowDialog();
		}

		private void diagram_ConnectionDeserialized_1(object sender, ConnectionSerializationRoutedEventArgs e)
		{
			// load the saved property
			XmlSerializer serializer = new XmlSerializer(typeof(ConnectionXML));
			//RadDiagramConnection con = ;
			string XML = e.SerializationInfo["Content"].ToString();
			StringReader sr = new StringReader(XML);
			(e.OriginalSource as RadDiagramConnection).Content = (ConnectionXML)serializer.Deserialize(sr);

			//(e.Shape as RadDiagramShape).Tag = (e.Shape as RadDiagramShape).Content;
			//(e.Source as RadDiagramConnection).Content = ((ConnectionXML)(e.Source as RadDiagramConnection).Tag).Type;
		}

		private void diagram_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
		{
			if (this.IgnoreSelection == true)
			{
				this.IgnoreSelection = false;
				return;
			}
			if (e.AddedItems.Count > 0 && e.AddedItems[0] is RadDiagramConnection)
			{
				var Item = e.AddedItems[0] as RadDiagramConnection;
				var ConXML = Item.Content as ConnectionXML;
				Window wind = null;
				switch (ConXML.Type)
				{
					case ConnectionTypes.eDecision:
						wind = new DecisionSettings(ConXML);
						break;
					case ConnectionTypes.eChance:
						wind = new ChanceSettings(ConXML);
						break;
					case ConnectionTypes.eBattle:
						wind = new BattleSettings(ConXML);
						break;
					case ConnectionTypes.eCondition:
						wind = new ConditionSettings(ConXML);
						break;
					case ConnectionTypes.eInventoryCondition:
						throw new ArgumentOutOfRangeException();
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				if (wind != null && wind.ShowDialog() == true)
				{
					return;
				}
			}
		}

		private void btnGenerateGame_Click_1(object sender, RoutedEventArgs e)
		{
			Game Game = new Game();
			Game.lstStats = Globals.GameElements.StartupStats;
			Game.lstEpizodes = new List<EpizodeXML>();
			foreach (RadDiagramShape sh in this.diagram.Shapes)
			{
				if (sh.Tag != null)
				{
					EpizodeXML Ep = new EpizodeXML();
					Epizode Source = sh.Tag as Epizode;

					Ep.ID = Source.EpizodeNumber;
					Ep.Inventories = Source.lstInventories;
					Ep.Skills = Source.lstSkills;
					Ep.Stats = Source.lstStats;
					Ep.Text = Source.EpizodeText;
					Ep.Choices = new Choices();

					Game.lstEpizodes.Add(Ep);
				}
			}

			foreach (RadDiagramConnection con in this.diagram.Connections)
			{
				var source = con.Source as RadDiagramShape;
				var Epizode = source.Tag as Epizode;
				var EpizodeXML = Game.lstEpizodes.FirstOrDefault(ep => ep.ID == Epizode.EpizodeNumber);
				var Choice = con.Content as ConnectionXML;
				if (EpizodeXML.Choices == null)
				{
					EpizodeXML.Choices = new Choices();
				}
				switch (Choice.Type)
				{
					case ConnectionTypes.eDecision:
						Choice.Decision.GoTo = ((Epizode)((RadDiagramShape)con.Target).Tag).EpizodeNumber;
						EpizodeXML.Choices.Decisions.Add(Choice.Decision);
						break;
					case ConnectionTypes.eChance:
						Choice.Chance.GoTo = ((Epizode)((RadDiagramShape)con.Target).Tag).EpizodeNumber;
						EpizodeXML.Choices.Chances.Add(Choice.Chance);
						break;
					case ConnectionTypes.eBattle:
						Choice.Battle.GoTo = ((Epizode)((RadDiagramShape)con.Target).Tag).EpizodeNumber;
						EpizodeXML.Choices.Battles.Add(Choice.Battle);
						break;
					case ConnectionTypes.eCondition:
						Choice.Condition.GoTo = ((Epizode)((RadDiagramShape)con.Target).Tag).EpizodeNumber;
						EpizodeXML.Choices.Conditions.Add(Choice.Condition);
						break;
					case ConnectionTypes.eInventoryCondition:
						EpizodeXML.Choices.InventoryConditions.Add(Choice.InventoryCondition);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}

			var dialog = new SaveFileDialog();
			if (dialog.ShowDialog() == true)
			{
				var nam = dialog.FileName;
				XmlSerializer serializer = new XmlSerializer(typeof(Game));
				TextWriter writ = new StreamWriter(nam);

				serializer.Serialize(writ, Game);
				writ.Close();
			}
		}

		private void btnGenerateContainer_Click_1(object sender, RoutedEventArgs e)
		{
			RadDiagramContainerShape Container = new RadDiagramContainerShape();
			Container.Position = new Point { X = this.diagram.Viewport.Left, Y = this.diagram.Viewport.Top };
			Container.AllowDrop = false;
			Container.Width = 100;
			Container.Height = 100;
			Container.IsResizingEnabled = false;

			List<IConnector> cs = new List<IConnector>();
			foreach (var connen in Container.Connectors)
			{
				cs.Add(connen);
			}

			foreach (var connen in cs)
			{
				Container.Connectors.Remove(connen);
			}

			diagram.AddShape(Container);
		}

		void RemoveShapeConnectors(RadDiagramShapeBase shape)
		{
			List<IConnector> cs = new List<IConnector>();
			foreach (var connen in shape.Connectors)
			{
				if (connen.Name != "Right")
				{
					cs.Add(connen);
				}
			}

			foreach (var connen in cs)
			{
				shape.Connectors.Remove(connen);
			}
		}

		private void btnAddInput_Click_1(object sender, RoutedEventArgs e)
		{
			if (this.diagram.SelectedItem is RadDiagramContainerShape)
			{
				var cont = (RadDiagramContainerShape)this.diagram.SelectedItem;
				cont.IsResizingEnabled = false;

				RadDiagramShape shape = new RadDiagramShape();
				shape.Tag = new Epizode();
				shape.Position = new Point { X = 0, Y = 10 * cont.Children.Count() };
				shape.IsDraggingEnabled = false;
				shape.IsResizingEnabled = false;
				
				shape.Content = "Input " + cont.Children.Count();
				
				
				cont.Items.Add(shape);
				shape.BorderBrush = this.diagram.Background;
				shape.Background = this.diagram.Background;
				this.RemoveShapeConnectors(shape);

				List<RadDiagramShapeBase> lstItems = new List<RadDiagramShapeBase>();

				lstItems.Add(cont);

				foreach (var i in cont.Items)
				{
					if (i is RadDiagramShape)
					{
						lstItems.Add((RadDiagramShape)i);
					}
				}

				var lstShapes = lstItems.ToArray<IShape>();
				this.diagram.Group("1", lstShapes);
				//lstShapes.Add((RadDiagramShape)cont);

				//this.diagram.Group("1", lstShapes);
			}
		}
		
		private void diagram_CommandExecuted_1(object sender, CommandRoutedEventArgs e)
		{
			var comm = e.Command.Name;
			if (comm == "Delete Items")
			{
				if (this.diagram.SelectedItem is RadDiagramContainerShape)
				{
					var cont = (RadDiagramContainerShape)this.diagram.SelectedItem;
					foreach(var item in cont.Items)
					{
						cont.Items.Remove(item);
					}
				}
			}
		}
		
		private void diagram_KeyDown_1(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Delete)
			{
				var Items = this.diagram.SelectedItems;
				foreach (var i in Items)
				{
					this.diagram.Items.Remove(i);
				}
			}
		}

		private static void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
		{
			if (sender == null)
				throw new ArgumentNullException("sender");
			var owner = sender as MainWindow;
			e.CanExecute = owner != null && owner.diagram != null && owner.diagram.Items.Count > 0;
		}

		private static void ExecuteDelete(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender == null)
				throw new ArgumentNullException("sender");
			var owner = sender as MainWindow;
			if (owner != null)
			{
				var item = owner.diagram.SelectedItem;
				if (item is RadDiagramContainerShape)
				{
					var container = item as RadDiagramContainerShape;
					var lstContainerItems = new List<RadDiagramItem>();

					foreach (var i in container.Items)
					{
						if (i is RadDiagramItem)
						{
							lstContainerItems.Add((RadDiagramItem)i);
						}
					}

					foreach (var i in lstContainerItems)
					{
						owner.diagram.Items.Remove(i);
					}
					owner.diagram.Items.Remove(container);
				}
			}
		}
	}

	//internal class MyRadDiagram : RadDiagram
	//{
	//	Telerik.Windows.Diagrams.Core.ICommand DeleteCommand
	//}
}
