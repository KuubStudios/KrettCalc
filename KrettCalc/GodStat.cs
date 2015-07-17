using System.Xml.Serialization;

namespace KrettCalc {
    public enum AbilityType {
        None,
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

        public bool ShouldSerializeBaseDamage() {
            return BaseDamage > 0;
        }

        public bool ShouldSerializeRank() {
            return Rank > 0;
        }

        public bool ShouldSerializeScaling() {
            return Scaling > 0;
        }

        public bool ShouldSerializeRefire() {
            return Refire > 0;
        }

        public bool ShouldSerializePrecast() {
            return Precast > 0;
        }

        public bool ShouldSerializePostcast() {
            return Postcast > 0;
        }

        public bool ShouldSerializeDuration() {
            return Duration > 0;
        }
    }

    [XmlType("Steroid")]
    public class GodSteroid {
        [XmlAttribute("Enabled")]
        public string NameEnable;

        [XmlAttribute("Disabled")]
        public string NameDisable;
    }

    [XmlType("Special")]
    public class GodSpecial {
        [XmlAttribute]
        public string Name;

        [XmlAttribute("Dmg")]
        public double BaseDamage;

        [XmlAttribute("Type")]
        public AbilityType CastType;

        [XmlAttribute]
        public double Precast;

        [XmlAttribute]
        public double Postcast;

        [XmlAttribute]
        public double Duration;

        public bool ShouldSerializeBaseDamage() {
            return BaseDamage > 0;
        }

        public bool ShouldSerializePrecast() {
            return Precast > 0;
        }

        public bool ShouldSerializePostcast() {
            return Postcast > 0;
        }

        public bool ShouldSerializeDuration() {
            return Duration > 0;
        }
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

        [XmlAttribute("HPScale")]
        public double HealthScaling;

        [XmlAttribute("PP")]
        public double BasePhysProtection;

        [XmlAttribute("PPScale")]
        public double PhysProtectionScaling;

        [XmlAttribute("MP")]
        public double BaseMagicalProtection;

        [XmlAttribute("MPScale")]
        public double MagicalProtectionScaling;

        [XmlAttribute("AS")]
        public double BaseAttackSpeed;

        [XmlAttribute("ASScale")]
        public double AttackSpeedScaling;

        [XmlAttribute("Atk")]
        public double BaseAttack;

        [XmlAttribute("AtkScale")]
        public double AttackScaling;

        [XmlAttribute("Mana")]
        public double BaseMana;

        [XmlAttribute("ManaScale")]
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

        [XmlAttribute("Hp5Scale")]
        public double Hp5Scaling;

        [XmlElement("Steroid1")]
        public GodSteroid FirstSteroid;

        [XmlElement("Steroid2")]
        public GodSteroid SecondSteroid;

        [XmlElement("Special1")]
        public GodSpecial FirstSpecial;

        [XmlElement("Special2")]
        public GodSpecial SecondSpecial;

        [XmlElement("Special3")]
        public GodSpecial ThirdSpecial;

        public override string ToString() {
            return Name;
        }

        public bool ShouldSerializeFirstSteroid() {
            return FirstSteroid != null && FirstSteroid.NameEnable != "";
        }

        public bool ShouldSerializeSecondSteroid() {
            return SecondSteroid != null && SecondSteroid.NameEnable != "";
        }

        public bool ShouldSerializeFirstSpecial() {
            return FirstSpecial != null && FirstSpecial.Name != "";
        }

        public bool ShouldSerializeSecondSpecial() {
            return SecondSpecial != null && SecondSpecial.Name != "";
        }

        public bool ShouldSerializeThirdSpecial() {
            return ThirdSpecial != null && ThirdSpecial.Name != "";
        }

        public bool ShouldSerializeMagicalProtectionScaling() {
            return MagicalProtectionScaling > 0;
        }
    }
}