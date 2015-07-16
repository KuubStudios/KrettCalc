using System.Xml.Serialization;

namespace KrettCalc {
    public enum ItemType {
        Physical,
        Magical,
        Both
    }

    [XmlType("Item")]
    public class ItemStat {
        [XmlAttribute]
        public string Name;

        [XmlAttribute]
        public double Cost;

        [XmlAttribute("Type")]
        public ItemType ItemType;

        [XmlAttribute]
        public double Health;

        [XmlAttribute("PP")]
        public double PhysicalProtection;

        [XmlAttribute("MP")]
        public double MagicalProtection;

        [XmlAttribute]
        public double Power;

        [XmlAttribute("AS")]
        public double AttackSpeed;

        [XmlAttribute("PercentPen")]
        public double PercentPenetration;

        [XmlAttribute("FlatPen")]
        public double FlatPenetration;

        [XmlAttribute("Crit")]
        public double CritChance;

        [XmlAttribute("CDR")]
        public double CooldownReduction;

        [XmlAttribute]
        public double Lifesteal;

        [XmlAttribute]
        public double Mana;

        [XmlAttribute]
        public string Passive;

        [XmlAttribute]
        public double Hp5;

        [XmlAttribute("MS")]
        public double MoveSpeed;

        public override string ToString() {
            return Name;
        }
    }
}