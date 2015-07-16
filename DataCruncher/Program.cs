using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;

using KrettCalc;

using Microsoft.VisualBasic.FileIO;

namespace DataCruncher {
    internal class Program {
        public static void Main(string[] args) {
            List<GodStat> gods = new List<GodStat>();
            using(TextFieldParser parser = new TextFieldParser(@"God Stats.csv")) {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();

                while(!parser.EndOfData) {
                    int i = 0;
                    string[] fields = parser.ReadFields();

                    GodStat god = new GodStat();
                    god.Name = fields[i++];
                    god.PowerType = (GodType)Enum.Parse(typeof(GodType), fields[i++], true);

                    god.BaseHealth = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.HealthScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    god.BasePhysProtection = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.PhysProtectionScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    god.BaseMagicalProtection = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.MagicalProtectionScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    god.BaseAttackSpeed = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.AttackSpeedScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    god.BaseAttack = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.AttackScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    god.BaseMana = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.ManaScaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    GodAbility[] abilities = new GodAbility[4];
                    for(int a = 0; a < 4; a++) {
                        abilities[a] = new GodAbility();

                        abilities[a].Name = fields[i++];
                        abilities[a].BaseDamage = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                        abilities[a].Rank = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                        abilities[a].Scaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                        abilities[a].AbilityType = (AbilityType)Enum.Parse(typeof(AbilityType), fields[i++], true);
                        abilities[a].Refire = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                        abilities[a].Precast = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                        abilities[a].Postcast = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                        string duration = fields[i++];
                        abilities[a].Duration = duration == "" ? 0 : duration == "Infinite" ? Double.PositiveInfinity : Double.Parse(duration, CultureInfo.InvariantCulture);
                    }

                    god.FirstAbility = abilities[0];
                    god.SecondAbility = abilities[1];
                    god.ThirdAbility = abilities[2];
                    god.UltimateAbility = abilities[3];

                    god.Passive = fields[i++];
                    god.BaseHp5 = Double.Parse(fields[i++], CultureInfo.InvariantCulture);
                    god.Hp5Scaling = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    gods.Add(god);
                }
            }

            List<ItemStat> items = new List<ItemStat>();
            using(TextFieldParser parser = new TextFieldParser(@"Item Stats.csv")) {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                parser.ReadLine();

                while(!parser.EndOfData) {
                    int i = 0;
                    string[] fields = parser.ReadFields();

                    ItemStat item = new ItemStat();
                    item.Name = fields[i++];

                    if(item.Name == "") {
                        continue;
                    }

                    item.Cost = Double.Parse(fields[i++], CultureInfo.InvariantCulture);

                    string itemtype = fields[i++];
                    item.ItemType = (ItemType)Enum.Parse(typeof(ItemType), itemtype == "" ? "Both" : itemtype, true);
                    item.Health = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.PhysicalProtection = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.MagicalProtection = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.Power = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.AttackSpeed = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.PercentPenetration = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.FlatPenetration = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.CritChance = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.CooldownReduction = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.Lifesteal = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.Mana = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.Passive = fields[i++];
                    item.Hp5 = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);
                    item.MoveSpeed = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture);

                    items.Add(item);
                }
            }

            XmlSerializer godSerializer = new XmlSerializer(typeof(List<GodStat>));
            using(FileStream fs = new FileStream("gods.xml", FileMode.Create)) {
                godSerializer.Serialize(fs, gods);
            }

            XmlSerializer itemSerializer = new XmlSerializer(typeof(List<ItemStat>));
            using(FileStream fs = new FileStream("items.xml", FileMode.Create)) {
                itemSerializer.Serialize(fs, items);
            }

            //Console.ReadKey();
        }
    }
}