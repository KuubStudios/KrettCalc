using System.Linq;
using System.Security.Policy;

namespace KrettCalc {
    public class Calculations {
        public GodStat TargetGod { get; set; }
        public PlayerItems TargetItems { get; set; }
        public int TargetLevel { get; set; }

        public double TargetAdditionalHealth { get; set; }
        public double TargetAdditionalPhysProtection { get; set; }
        public double TargetAdditionalMagicalProtection { get; set; }

        public int TargetWarlockStacks { get; set; }
        public int TargetUrchinStacks { get; set; }

        public bool TargetSovAura { get; set; }
        public bool TargetHeartward { get; set; }
        public bool TargetLotusCrown { get; set; }
        public bool TargetShifterShield { get; set; }

        public GodStat SelfGod { get; set; }
        public PlayerItems SelfItems { get; set; }
        public int SelfLevel { get; set; }
        public int SelfHealth { get; set; }

        public int SelfDevourerStacks { get; set; }
        public int SelfTranscendenceStacks { get; set; }
        public int SelfHeartseekerStacks { get; set; }
        public int SelfAncileStacks { get; set; }
        public int SelfDoomOrbStacks { get; set; }
        public int SelfThothStacks { get; set; }
        public int SelfWarlockStacks { get; set; }

        public double TargetBonusHealth {
            get { return TargetAdditionalHealth + (TargetItems.Any(i => i != null && i.Name == "Warlock's Sash") ? TargetWarlockStacks * 3 : 0); }
        }

        public double TargetHealth {
            get {
                return TargetGod.BaseHealth +
                       (TargetGod.HealthScaling * TargetLevel) +
                       TargetBonusHealth +
                       TargetItems.Sum(i => i == null ? 0 : i.Health);
            }
        }

        public double TargetBonusPhysProtection {
            get {
                return TargetAdditionalPhysProtection +
                       (TargetItems.Any(i => i != null && i.Name == "Hide of the Urchin") ? TargetUrchinStacks * 2 : 0) +
                       (TargetHeartward ? 20 : 0) +
                       (TargetLotusCrown ? 20 : 0) +
                       (TargetShifterShield ? 20 : 0);
            }
        }

        public double TargetPhysProtection {
            get {
                return TargetGod.BasePhysProtection +
                       (TargetGod.PhysProtectionScaling * TargetLevel) +
                       TargetBonusPhysProtection +
                       TargetItems.Sum(i => i == null ? 0 : i.PhysicalProtection);
            }
        }

        public double TargetPhysHealth {
            get { return TargetHealth / (100 / (100 + TargetPhysProtection)); }
        }

        public double TargetBonusMagiProtection {
            get {
                return TargetAdditionalMagicalProtection +
                       (TargetItems.Any(i => i != null && i.Name == "Hide of the Urchin") ? TargetUrchinStacks * 2 : 0) +
                       (TargetSovAura ? 20 : 0) +
                       (TargetLotusCrown ? 20 : 0) +
                       (TargetShifterShield ? 20 : 0);
            }
        }

        public double TargetMagiProtection {
            get {
                return TargetGod.BaseMagicalProtection +
                       (TargetGod.MagicalProtectionScaling * TargetLevel) +
                       TargetBonusMagiProtection +
                       TargetItems.Sum(i => i == null ? 0 : i.MagicalProtection);
            }
        }

        public double TargetMagiHealth {
            get { return TargetHealth / (100 / (100 + TargetMagiProtection)); }
        }

        public double TargetHp5 {
            get {
                return TargetGod.BaseHp5 +
                       (TargetGod.Hp5Scaling * TargetLevel) +
                       TargetItems.Sum(i => i == null ? 0 : i.Hp5) +
                       (TargetSovAura ? 25 : 0);
            }
        }

        public Calculations() {
            TargetItems = new PlayerItems();
            TargetLevel = 1;

            SelfItems = new PlayerItems();
            SelfLevel = 1;
        }
    }
}