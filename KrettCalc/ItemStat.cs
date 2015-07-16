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

        public bool ShouldSerializeCost() {
            return Cost > 0;
        }

        public bool ShouldSerializeHealth() {
            return Health > 0;
        }

        public bool ShouldSerializePhysicalProtection() {
            return PhysicalProtection > 0;
        }

        public bool ShouldSerializeMagicalProtection() {
            return MagicalProtection > 0;
        }

        public bool ShouldSerializePower() {
            return Power > 0;
        }

        public bool ShouldSerializeAttackSpeed() {
            return AttackSpeed > 0;
        }

        public bool ShouldSerializePercentPenetration() {
            return PercentPenetration > 0;
        }

        public bool ShouldSerializeFlatPenetration() {
            return FlatPenetration > 0;
        }

        public bool ShouldSerializeCritChance() {
            return CritChance > 0;
        }

        public bool ShouldSerializeCooldownReduction() {
            return CooldownReduction > 0;
        }

        public bool ShouldSerializeLifesteal() {
            return Lifesteal > 0;
        }

        public bool ShouldSerializeMana() {
            return Mana > 0;
        }

        public bool ShouldSerializeHp5() {
            return Hp5 > 0;
        }

        public bool ShouldSerializeMoveSpeed() {
            return MoveSpeed > 0;
        }
    }
}