using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;

namespace GameClasses
{
	[Serializable]
	[XmlRoot("GameElements")]
	public class GameElements
	{
		[XmlArray("StartupStats"), XmlArrayItem(typeof(StartupStats), ElementName = "StartupStat")]
		public List<StartupStats> StartupStats;
		[XmlArray("Items"), XmlArrayItem(typeof(ItemStrings), ElementName = "Item")]
		public List<ItemStrings> Items;
		[XmlArray("Skills"), XmlArrayItem(typeof(ItemStrings), ElementName = "Skill")]
		public List<ItemStrings> Skills;
		[XmlArray("Stats"), XmlArrayItem(typeof(ItemStrings), ElementName = "Stat")]
		public List<ItemStrings> Stats;
	}

	public static class Globals
	{
		public static GameElements GameElements;
	}

	public class EpizodeXML
	{
		public int ID;
		public string Text;

		public List<Inventory> Inventories;
		public List<Skill> Skills;
		public List<Stat> Stats;
		public Choices Choices;
        public Byte[] image;
    }

	[Serializable]
	[XmlRoot("Epizode")]
	public class Epizode
	{
		[XmlElement("EpizodeNumber")]
		public int EpizodeNumber;
		[XmlElement("EpizodeText")]
		public string EpizodeText;
		[XmlArray("Inventories"), XmlArrayItem(typeof(Inventory), ElementName = "Inventory")]
		public List<Inventory> lstInventories;
		[XmlArray("Skills"), XmlArrayItem(typeof(Skill), ElementName = "Skill")]
		public List<Skill> lstSkills;
		[XmlArray("Stats"), XmlArrayItem(typeof(Stat), ElementName = "Stat")]
		public List<Stat> lstStats;

        [XmlIgnore]
        public Bitmap LargeIcon { get; set; }

        [XmlIgnore]
        public MemoryStream ms;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("LargeIcon")]
        public byte[] LargeIconSerialized
        {
            get
            { // serialize
                if (LargeIcon == null) return null;
                ms = new MemoryStream();
                {
                    LargeIcon.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    LargeIcon = null;
                }
                else
                {
                    ms = new MemoryStream(value);
                    {
                        LargeIcon = new Bitmap(ms);
                    }
                }
            }
        }

        public Epizode()
		{
			this.lstInventories = new List<Inventory>();
			this.lstSkills = new List<Skill>();
			this.lstStats = new List<Stat>();
			this.EpizodeNumber = 0;
		}
	}

	public class StartupStats
	{
		public string Name { get; set; }
		public int Min { get; set; }
		public int Max { get; set; }
	}

	public class Inventory
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public bool Action { get; set; }
	}

	public class Skill
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public bool Action { get; set; }
	}

	public class Stat
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public bool Action { get; set; }
		public bool Reset { get; set; }
	}

	public class Decision
	{
		public int GoTo;
		public string Text;
	}

    public class ChanceRollBack : Condition
    {
    }

    public class InventoryCondition : Decision
	{
		public string Name;
		public int Quantity;
		public bool IsAvailable;
	}

	public class Chance : Decision
	{
		public double Probability;
	}

	public class Battle : Decision
	{
		public int Lose;
		public int EnemyStrength;
		public int EnemyHealth;
	}

	public class Predicate
	{
		public PredicateTypes Type { get; set; }
		public string Name { get; set; }
		public bool IsAvailable { get; set; }
		public int Quantity { get; set; }
	}

	public class Condition : Decision
	{
		public List<Predicate> Predicates;
	}

	public class ItemStrings
	{
		public string Name { get; set; }
	}

	public enum ConnectionTypes
	{
		eDecision,
		eChance,
		eBattle,
		eCondition,
        eChanceRollback,
		eInventoryCondition,
	}

	public enum PredicateTypes
	{
		eInventory,
		eStat,
		eSkill,
	}

	[Serializable]
	[XmlRoot("Connection")]
	public class ConnectionXML
	{
		[XmlElement("Type")]
		public ConnectionTypes Type;
		[XmlElement("Decision")]
		public Decision Decision;
		[XmlElement("Chance")]
		public Chance Chance;
		[XmlElement("Battle")]
		public Battle Battle;
		[XmlElement("Condition")]
		public Condition Condition;
		[XmlElement("InventoryCondition")]
		public InventoryCondition InventoryCondition;
        [XmlElement("ChanceRollback")]
        public ChanceRollBack ChanceRollback;

        public override string ToString()
        {
            string ret = "";
            switch(Type)
            {
                case ConnectionTypes.eDecision:
                    ret = Decision.Text;
                    break;
                case ConnectionTypes.eBattle:
                    ret =  Battle.Text;
                    break;
                case ConnectionTypes.eChance:
                    ret = Chance.Text;
                    break;
                case ConnectionTypes.eChanceRollback:
                    ret = ChanceRollback.Text;
                    break;
                case ConnectionTypes.eCondition:
                    ret = Condition.Text;
                    break;
                case ConnectionTypes.eInventoryCondition:
                    ret = InventoryCondition.Text;
                    break;
            }
            if (ret.Length > 10)
            {
                ret = ret.Substring(0, 10);
            }
            return ret;
        }
    }

	public class Choices
	{
		public List<Decision> Decisions;
		public List<Chance> Chances;
		public List<Battle> Battles;
		public List<Condition> Conditions;
		public List<InventoryCondition> InventoryConditions;
        public List<ChanceRollBack> ChanceRollBack;

        public Choices()
		{
			this.Decisions = new List<Decision>();
			this.Chances = new List<Chance>();
			this.Battles = new List<Battle>();
			this.Conditions = new List<Condition>();
			this.InventoryConditions = new List<InventoryCondition>();
            this.ChanceRollBack = new List<ChanceRollBack>();
		}
	}

	[Serializable]
	[XmlRoot("Game")]
	public class Game
	{
		//[XmlArray("Skills"), XmlArrayItem(typeof(Skills), ElementName = "Skill")]
		//public List<Skills> lstSkills{ get; set; }

		[XmlArray("Stats"), XmlArrayItem(typeof(StartupStats), ElementName = "Stat")]
		public List<StartupStats> lstStats { get; set; }

		[XmlArray("Epizodes"), XmlArrayItem(typeof(EpizodeXML), ElementName = "Epizode")]
		public List<EpizodeXML> lstEpizodes { get; set; }
	}
}
