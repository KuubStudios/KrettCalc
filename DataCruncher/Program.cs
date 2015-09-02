using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Serialization;

using KrettCalc;

using Microsoft.VisualBasic.FileIO;

namespace DataCruncher {
    internal class Program {
        private static readonly WebClient client = new WebClient();

        private static string CheckEnum(string field) {
            return field != "" ? field : "None";
        }

        public static void Main(string[] args) {
            Console.WriteLine("parsing gods");
            List<GodStat> gods = ParseGods("https://docs.google.com/spreadsheets/d/1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk/export?format=csv&id=1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk&gid=456866848");

            XmlSerializer godSerializer = new XmlSerializer(typeof(List<GodStat>));
            using(FileStream fs = new FileStream("gods.xml", FileMode.Create)) {
                godSerializer.Serialize(fs, gods);
            }

            Console.WriteLine("parsing items");
            List<ItemStat> items = ParseItems("https://docs.google.com/spreadsheets/d/1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk/export?format=csv&id=1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk&gid=66771184");

            XmlSerializer itemSerializer = new XmlSerializer(typeof(List<ItemStat>));
            using(FileStream fs = new FileStream("items.xml", FileMode.Create)) {
                itemSerializer.Serialize(fs, items);
            }

            Console.WriteLine("done");
            Console.ReadKey();
        }

        public static List<GodStat> ParseGods(string file) {
            List<GodStat> gods = new List<GodStat>();

            using(StringReader reader = new StringReader(client.DownloadString(file))) {
                using(TextFieldParser parser = new TextFieldParser(reader)) {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.ReadLine();

                    while(!parser.EndOfData) {
                        int i = 0;
                        string[] fields = parser.ReadFields();
                        if(fields == null) throw new ArgumentException("not a valid file", "file");

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
                        god.Hp5Scaling = Double.Parse(fields[i], CultureInfo.InvariantCulture);

                        gods.Add(god);
                    }
                }
            }

            using(StringReader reader = new StringReader(client.DownloadString("https://docs.google.com/spreadsheets/d/1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk/export?format=csv&id=1a6LlTs8BNXHIwGicqM0TFXEvGwk77VXAxGxi0Xqd8Dk&gid=257000868"))) {
                using(TextFieldParser parser = new TextFieldParser(reader)) {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.ReadLine();

                    while(!parser.EndOfData) {
                        int i = 0;
                        string[] fields = parser.ReadFields();
                        if(fields == null) throw new ArgumentException("not a valid file", "file");

                        string name = fields[i++];
                        int god = gods.FindIndex(g => g.Name == name);

                        gods[god].FirstSteroid = new GodSteroid() {
                            Enabled = fields[i++],
                            Disabled = fields[i++]
                        };

                        gods[god].SecondSteroid = new GodSteroid() {
                            Enabled = fields[i++],
                            Disabled = fields[i++]
                        };

                        gods[god].FirstSpecial = new GodSpecial() {
                            Name = fields[i++],
                            BaseDamage = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            CastType = (AbilityType)Enum.Parse(typeof(AbilityType), CheckEnum(fields[i++]), true),
                            Precast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Postcast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Duration = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture)
                        };

                        gods[god].SecondSpecial = new GodSpecial() {
                            Name = fields[i++],
                            BaseDamage = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            CastType = (AbilityType)Enum.Parse(typeof(AbilityType), CheckEnum(fields[i++]), true),
                            Precast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Postcast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Duration = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture)
                        };

                        gods[god].ThirdSpecial = new GodSpecial() {
                            Name = fields[i++],
                            BaseDamage = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            CastType = (AbilityType)Enum.Parse(typeof(AbilityType), CheckEnum(fields[i++]), true),
                            Precast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Postcast = Double.Parse("0" + fields[i++], CultureInfo.InvariantCulture),
                            Duration = Double.Parse("0" + fields[i], CultureInfo.InvariantCulture)
                        };

                        string last = fields[fields.Length - 1];
                        double v;
                        if(!Double.TryParse(last, out v)) gods[god].Extra = last;
                    }
                }
            }

            return gods;
        }

        public static List<ItemStat> ParseItems(string file) {
            List<ItemStat> items = new List<ItemStat>();

            using(StringReader reader = new StringReader(client.DownloadString(file))) {
                using(TextFieldParser parser = new TextFieldParser(reader)) {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.ReadLine();

                    while(!parser.EndOfData) {
                        int i = 0;
                        string[] fields = parser.ReadFields();
                        if(fields == null) throw new ArgumentException("not a valid file", "file");

                        ItemStat item = new ItemStat();
                        item.Name = fields[i++];

                        if(item.Name == "") continue;

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
                        item.MoveSpeed = Double.Parse("0" + fields[i], CultureInfo.InvariantCulture);

                        items.Add(item);
                    }
                }
            }

            return items;
        }
    }
}