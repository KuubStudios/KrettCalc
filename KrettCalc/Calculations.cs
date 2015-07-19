using System;
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

        public double SelfPhysicalPower {
            get {
                //=(Calculations!K5+(Calculator!B38*Calculations!K7)+IF(Calculator!A36="Magical",Calculations!A16*0.2,Calculations!A16))*IF(Calculator!C47="Enable Initiative",1.3,1)
                return 0;
            }
        }

        public double SelfInHandDamage {
            get {
                //=Calculations!A17*IF(Calculator!$C$47="Enable Fastest God Alive",1.5,1)*IF(Calculator!C47="Enable Frostbite",2,1)*IF(Calculator!C47="Enable Book of Demons",1.5,1)*IF(Calculator!C47="Enable Overcharge",1.9,1)*IF(Calculator!C47="Enable Shifting Sands",1.2,1)*IF(Calculator!D47="Predator is Automated",1.15,1)+IF(Calculator!C47="Auto Attack Wheel of Time",0.35*A16,0)+IF(Calculator!C47="Magic Power Wheel of Time",0.2*(K5+K7*Calculator!B38),0)+IF(Calculator!$C$47="Enable Irradiate",25+15*Calculator!$B$62+0.25*$A$16,0)+IF(Calculator!$D$47="Enable Pulse",10+10*Calculator!$B$63+0.15*$A$16,0)+IF(Calculator!C47="Enable Trident",(20+Calculator!B63*20+0.4*A16),0)+IF(Calculator!F42="Yes",IF(Calculator!A36="magical",15,12),0)+IF(H27="Yes",H26,0)+IF(Calculator!C47="Enable Dragon Call",15+Calculator!B63*15+0.3*A16,0)
                return SelfPhysicalPower;
            }
        }

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