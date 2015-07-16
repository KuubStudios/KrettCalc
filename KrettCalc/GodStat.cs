using System.Xml.Serialization;

namespace KrettCalc {
    public enum AbilityType {
        Field,
        Instant,
        Channel,
    }

    [XmlType("Ability")]
    public class GodAbility {
        [XmlAttribute]
        public string Name;

        [XmlAttribute("Dmg")]
        public double BaseDamage;

        [XmlAttribute]
        public double Rank;

        [XmlAttribute]
        public double Scaling;

        [XmlAttribute("Type")]
        public AbilityType AbilityType;

        [XmlAttribute]
        public double Refire;

        [XmlAttribute]
        public double Precast;

        [XmlAttribute]
        public double Postcast;

        [XmlAttribute]
        public double Duration;
    }

    public enum GodType {
        Physical,
        Magical
    }

    [XmlType("God")]
    public class GodStat {
        [XmlAttribute]
        public string Name;

        [XmlAttribute("Type")]
        public GodType PowerType;

        [XmlAttribute("HP")]
        public double BaseHealth;

        [XmlAttribute("HPScaling")]
        public double HealthScaling;

        [XmlAttribute("PP")]
        public double BasePhysProtection;

        [XmlAttribute("PPScaling")]
        public double PhysProtectionScaling;

        [XmlAttribute("MP")]
        public double BaseMagicalProtection;

        [XmlAttribute("MPScaling")]
        public double MagicalProtectionScaling;

        [XmlAttribute("AS")]
        public double BaseAttackSpeed;

        [XmlAttribute("ASScaling")]
        public double AttackSpeedScaling;

        [XmlAttribute("Atk")]
        public double BaseAttack;

        [XmlAttribute("AtkScaling")]
        public double AttackScaling;

        [XmlAttribute("Mana")]
        public double BaseMana;

        [XmlAttribute]
        public double ManaScaling;

        [XmlElement("Ability1")]
        public GodAbility FirstAbility;

        [XmlElement("Ability2")]
        public GodAbility SecondAbility;

        [XmlElement("Ability3")]
        public GodAbility ThirdAbility;

        [XmlElement("Ability4")]
        public GodAbility UltimateAbility;

        [XmlAttribute]
        public string Passive;

        [XmlAttribute("Hp5")]
        public double BaseHp5;

        [XmlAttribute]
        public double Hp5Scaling;

        public override string ToString() {
            return Name;
        }
    }
}