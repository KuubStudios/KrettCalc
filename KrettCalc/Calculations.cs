using System.Linq;

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

        public bool SelfRedBuff { get; set; }
        public bool SelfPurpleBuff { get; set; }
        public bool SelfPythagAura { get; set; }
        public bool SelfVoidstone { get; set; }
        public bool SelfAchillesSpear { get; set; }

        public string SelfSteroid1 { get; set; }
        public string SelfSteroid2 { get; set; }

        private double A3 { // todo
            get {
                /*=
                 * (Filters!D11*30)+
                 * IF(Calculator!D47="Enable Gravity Surge",60,0)+
                 * IF(Calculator!C47="You're smashed!",30,IF(Calculator!C47="You're tipsy",10,0))+
                 * IF(Calculator!D47="Enable Intoxicate Steroid",60,0)+
                 * IF(Calculator!A36="Bakasura",5+5*Calculator!B64,0)+
                 * IF(Calculator!C47="Rune Howl",105,0)+
                 * IF(Calculator!C47="Seething Howl",70,0)+
                 * IF(Calculator!B36="Hercules",(30+1*Calculator!B38)*(Calculator!A38/100))+
                 * (IF(A28="Yes", A26, 0))+
                 * (IF(A35="Yes", A34*6, 0))+
                 * (IF(A32="Yes", A31, 0))+
                 * (IF(G27="Yes", 30, 0))+
                 * (IF(G32="Yes", 20, 0))+
                 * (IF(B28="Yes",B26,0))+
                 * IF(Calculator!$B$36="Scylla",E61,0)+
                 * IF(Calculator!$B$36="Mercury",Steroids!BA40,0)+
                 * IF(Calculator!$B$36="Odin",Calculator!C49*5,0)+
                 * IF(Calculator!$B$36="Thor",Calculator!C49*15,0)+
                 * IF(Calculator!C47="Assault Stance",Calculator!B64*10,0)+
                 * IF(Calculator!C47="Enable Expose Weakness",Calculator!B63*10,0)+
                 * IF(Calculator!B36="Vamana",0.2*B85,0)+
                 * IF(Calculator!C47="Enable Colossal Fury",30+Calculator!B65*15,0)+
                 * IF(Calculator!$B$36="Xbalanque",Calculator!C49*5,0)+
                 * IF(Calculator!C47="Enable Branching Bola",Calculator!B62*10,0)+
                 * IF(Calculator!F40="Yes",IF(Calculator!A36="magical",10,5),0)+
                 * IF(Calculator!F43="Is a teammate giving you Pythag's aura?",IF(Calculator!F44="Yes",20,0),0)+
                 * IF(Calculator!F43="Is a teammate giving you Soul Eater?",IF(Calculator!F44="Yes",10,0),0)
                 */
                return 0;
            }
        }

        private double A19 {
            get { return SelfItems.Sum(i => i != null ? i.Power : 0) + A3; }
        }

        private double A16 { // todo
            /*=
             * A19*
             * (
             *  1+
             *  IF(Calculator!F40="Yes",0.2,0)+
             *  IF(countif(Filters!$C$7:$C$12,"Rod of Tahuti")>0,0.25,0)+
             *  IF(Calculator!C47="Magic Power Wheel of Time",0.2,0)+
             *  IF(Calculator!$B$36="He Bo",0.05*Calculator!C49,0)+
             *  IF(Calculator!$B$36="Nox",0.03*Calculator!C49,0)+
             *  IF(Calculator!C47="Enabled Mark of the Golden Crow",0.3,0)
             * )+
             * IF(Calculator!$B$36="Kukulkan",Steroids!BA36,0)
             */
            get {
                return A19 *
                       (1);
            }
        }

        private double A17 { // todo
            get {
                /*=
                 * (Calculations!K5+
                 *  (Calculator!B38*Calculations!K7)+
                 *  IF(Calculator!A36="Magical",Calculations!A16*0.2,Calculations!A16)
                 * )*
                 * IF(Calculator!C47="Enable Initiative",1.3,1)
                 */
                return 0;
            }
        }

        public double SelfInHandDamage { // todo
            get {
                /*=
                 * Calculations!A17*
                 * IF(Calculator!$C$47="Enable Fastest God Alive",1.5,1)*
                 * IF(Calculator!C47="Enable Frostbite",2,1)*
                 * IF(Calculator!C47="Enable Book of Demons",1.5,1)*
                 * IF(Calculator!C47="Enable Overcharge",1.9,1)*
                 * IF(Calculator!C47="Enable Shifting Sands",1.2,1)*
                 * IF(Calculator!D47="Predator is Automated",1.15,1)+
                 * IF(Calculator!C47="Auto Attack Wheel of Time",0.35*A16,0)+
                 * IF(Calculator!C47="Magic Power Wheel of Time",0.2*(K5+K7*Calculator!B38),0)+
                 * IF(Calculator!$C$47="Enable Irradiate",25+15*Calculator!$B$62+0.25*$A$16,0)+
                 * IF(Calculator!$D$47="Enable Pulse",10+10*Calculator!$B$63+0.15*$A$16,0)+
                 * IF(Calculator!C47="Enable Trident",(20+Calculator!B63*20+0.4*A16),0)+
                 * IF(Calculator!F42="Yes",IF(Calculator!A36="magical",15,12),0)+
                 * IF(H27="Yes",H26,0)+
                 * IF(Calculator!C47="Enable Dragon Call",15+Calculator!B63*15+0.3*A16,0)
                 */
                return A17 *
                       (SelfSteroid1 == "Enable Fastest God Alive" ? 1.5 : 1) *
                       (SelfSteroid1 == "Enable Frostbite" ? 2 : 1) *
                       (SelfSteroid1 == "Enable Book of Demons" ? 1.5 : 1) *
                       (SelfSteroid1 == "Enable Overcharge" ? 1.9 : 1) *
                       (SelfSteroid1 == "Enable Shifting Sands" ? 1.2 : 1) *
                       (SelfSteroid2 == "Predator is Automated" ? 1.15 : 1) +
                       (SelfSteroid1 == "Auto Attack Wheel of Time" ? 0.35 * A16 : 0) +
                       (SelfSteroid1 == "Magic Power Wheel of Time" ? 0.2 * (SelfGod.BaseAttack + SelfGod.AttackScaling * SelfLevel) : 0) +
                       (SelfSteroid1 == "Enable Irradiate" ? 25 + 15 * SelfGod.FirstAbility.CurrentLevel + 0.25 * A16 : 0) +
                       (SelfSteroid2 == "Enable Pulse" ? 10 + 10 * SelfGod.SecondAbility.CurrentLevel + 0.15 * A16 : 0) +
                       (SelfSteroid1 == "Enable Trident" ? 20 + SelfGod.SecondAbility.CurrentLevel * 20 + 0.4 * A16 : 0) +
                       (SelfPurpleBuff ? (SelfGod.PowerType == GodType.Magical ? 15 : 12) : 0) +
                       //(SelfItems.Any(i => i != null && i.Name == "Telkhines Ring") ? PUT SOMETHING HERE : 0) +
                       (SelfSteroid1 == "Enable Dragon Call" ? 15 + SelfGod.SecondAbility.CurrentLevel * 15 + 0.3 * A16 : 0);
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