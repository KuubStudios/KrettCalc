using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

using MetroFramework.Controls;

namespace KrettCalc {
    public partial class CalculatorForm : FancyForm {
        private List<GodStat> gods;
        private List<ItemStat> items;

        private readonly Calculations calculations = new Calculations();

        private bool dontCalculate;
        private ItemStat emptyItem;

        public CalculatorForm() {
            InitializeComponent();
        }

        private void CalculatorForm_Load(object sender, EventArgs e) {
            dontCalculate = true;

            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;

            XmlSerializer godSerializer = new XmlSerializer(typeof(List<GodStat>));
            using(FileStream fs = new FileStream("gods.xml", FileMode.Open)) {
                gods = (List<GodStat>)godSerializer.Deserialize(fs);
            }
            gods.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));

            XmlSerializer itemSerializer = new XmlSerializer(typeof(List<ItemStat>));
            using(FileStream fs = new FileStream("items.xml", FileMode.Open)) {
                items = (List<ItemStat>)itemSerializer.Deserialize(fs);
            }
            items.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.Ordinal));

            foreach(GodStat god in gods) {
                targetGod.Items.Add(god);
                selfGod.Items.Add(god);
            }

            targetGod.SelectedIndex = 0;
            selfGod.SelectedIndex = 0;

            selfBellonaStance.SelectedIndex = 0;

            selfHealthPerc.Value = 100;

            dontCalculate = false;
            CalculateTarget();
            CalculateSelf();
        }

        private void sliderTargetGodLvl_ValueChanged(object sender, EventArgs e) {
            targetGodLvlLabel.Text = targetGodLvl.Value.ToString();
            calculations.TargetLevel = targetGodLvl.Value;
            CalculateTarget();
        }

        private void btnTab1_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage1;
            Text = btnTab1.Text;
        }

        private void btnTab2_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage2;
            Text = btnTab2.Text;
        }

        private void btnTab3_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage3;
            Text = btnTab3.Text;
        }

        private void btnTab4_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage4;
            Text = btnTab4.Text;
        }

        private void targetGod_SelectedIndexChanged(object sender, EventArgs e) {
            GodStat god = (GodStat)targetGod.SelectedItem;
            calculations.TargetGod = god;
            if(god.PowerType == GodType.Physical) {
                targetPhysical.Checked = true;
            } else {
                targetMagical.Checked = true;
            }

            ItemStat item1 = (ItemStat)targetItem1.SelectedItem;
            ItemStat item2 = (ItemStat)targetItem2.SelectedItem;
            ItemStat item3 = (ItemStat)targetItem3.SelectedItem;
            ItemStat item4 = (ItemStat)targetItem4.SelectedItem;
            ItemStat item5 = (ItemStat)targetItem5.SelectedItem;
            ItemStat item6 = (ItemStat)targetItem6.SelectedItem;

            targetItem1.Items.Clear();
            targetItem2.Items.Clear();
            targetItem3.Items.Clear();
            targetItem4.Items.Clear();
            targetItem5.Items.Clear();
            targetItem6.Items.Clear();

            foreach(ItemStat item in items) {
                if(item.Name == "Empty") emptyItem = item;

                if(item.ItemType != ItemType.Both && item.ItemType.ToString() != god.PowerType.ToString()) continue;
                if(god.Name != "Ratatoskr" && (item.Name == "Opal Acorn" || item.Name == "Sapphire Acorn")) continue;

                targetItem1.Items.Add(item);
                targetItem2.Items.Add(item);
                targetItem3.Items.Add(item);
                targetItem4.Items.Add(item);
                targetItem5.Items.Add(item);
                targetItem6.Items.Add(item);

                if(item == item1) targetItem1.SelectedItem = item;
                if(item == item2) targetItem2.SelectedItem = item;
                if(item == item3) targetItem3.SelectedItem = item;
                if(item == item4) targetItem4.SelectedItem = item;
                if(item == item5) targetItem5.SelectedItem = item;
                if(item == item6) targetItem6.SelectedItem = item;
            }

            if(targetItem1.SelectedItem == null) targetItem1.SelectedItem = emptyItem;
            if(targetItem2.SelectedItem == null) targetItem2.SelectedItem = emptyItem;
            if(targetItem3.SelectedItem == null) targetItem3.SelectedItem = emptyItem;
            if(targetItem4.SelectedItem == null) targetItem4.SelectedItem = emptyItem;
            if(targetItem5.SelectedItem == null) targetItem5.SelectedItem = emptyItem;
            if(targetItem6.SelectedItem == null) targetItem6.SelectedItem = emptyItem;

            CalculateTarget();
        }

        private void SetItem(ComboBox combo, MetroTextBox cost, PictureBox pic, bool target = true) {
            ItemStat item = (ItemStat)combo.SelectedItem;
            cost.Text = item.Cost.ToString(CultureInfo.InvariantCulture);
            string name = item.Name.Replace(" ", "-").Replace("\'", "").ToLower();
            pic.ImageLocation = "images/" + name + ".png";

            if(target) {
                targetWarlockStacks.Enabled = calculations.TargetItems.Any(i => i != null && i.Name == "Warlock's Sash");
                if(!targetWarlockStacks.Enabled) calculations.TargetWarlockStacks = 0;

                targetUrchinStacks.Enabled = calculations.TargetItems.Any(i => i != null && i.Name == "Hide of the Urchin");
                if(!targetUrchinStacks.Enabled) calculations.TargetUrchinStacks = 0;

                CalculateTarget();
            } else {
                selfDevoStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Devourer's Gauntlet");
                if(!selfDevoStacks.Enabled) calculations.SelfDevourerStacks = 0;

                selfTransStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Transcendence");
                if(!selfTransStacks.Enabled) calculations.SelfTranscendenceStacks = 0;

                selfHeartStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Heartseeker");
                if(!selfHeartStacks.Enabled) calculations.SelfHeartseekerStacks = 0;

                selfDoomStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Doom Orb");
                if(!selfDoomStacks.Enabled) calculations.SelfDoomOrbStacks = 0;

                selfThothStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Book of Thoth");
                if(!selfThothStacks.Enabled) calculations.SelfThothStacks = 0;

                selfWarlockStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Warlock's Sash");
                if(!selfWarlockStacks.Enabled) calculations.SelfWarlockStacks = 0;

                selfAncileStacks.Enabled = calculations.SelfItems.Any(i => i != null && i.Name == "Ancile");
                if(!selfAncileStacks.Enabled) calculations.SelfAncileStacks = 0;

                CalculateSelf();
            }
        }

        private void targetItem1_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.TargetItems.First = (ItemStat)targetItem1.SelectedItem;
            SetItem(targetItem1, targetItem1Cost, targetItem1Pic);
        }

        private void targetItem2_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.TargetItems.Second = (ItemStat)targetItem2.SelectedItem;
            SetItem(targetItem2, targetItem2Cost, targetItem2Pic);
        }

        private void targetItem3_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.TargetItems.Third = (ItemStat)targetItem3.SelectedItem;
            SetItem(targetItem3, targetItem3Cost, targetItem3Pic);
        }

        private void targetItem4_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.TargetItems.Fourth = (ItemStat)targetItem4.SelectedItem;
            SetItem(targetItem4, targetItem4Cost, targetItem4Pic);
        }

        private void targetItem5_SelectedValueChanged(object sender, EventArgs e) {
            calculations.TargetItems.Fifth = (ItemStat)targetItem5.SelectedItem;
            SetItem(targetItem5, targetItem5Cost, targetItem5Pic);
        }

        private void targetItem6_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.TargetItems.Sixth = (ItemStat)targetItem6.SelectedItem;
            SetItem(targetItem6, targetItem6Cost, targetItem6Pic);
        }

        public void CalculateTarget() {
            if(dontCalculate) return;

            targetTotalHealth.Text = calculations.TargetHealth.ToString(CultureInfo.InvariantCulture);
            targetBonusHealth.Text = String.Format("(+{0})", calculations.TargetBonusHealth);

            targetPhysProtection.Text = calculations.TargetPhysProtection.ToString(CultureInfo.InvariantCulture);
            targetBonusPhysProt.Text = String.Format("(+{0})", calculations.TargetBonusPhysProtection);

            targetMagiProtection.Text = calculations.TargetMagiProtection.ToString(CultureInfo.InvariantCulture);
            targetBonusMagiProt.Text = String.Format("(+{0})", calculations.TargetBonusMagiProtection);

            targetPhysHealth.Text = calculations.TargetPhysHealth.ToString(CultureInfo.InvariantCulture);
            targetMagiHealth.Text = calculations.TargetMagiHealth.ToString(CultureInfo.InvariantCulture);
            targetHp5.Text = calculations.TargetHp5.ToString(CultureInfo.InvariantCulture);

            double cost = calculations.TargetItems.Sum(i => i == null ? 0 : i.Cost);

            targetPhysCost.Text = String.Format(CultureInfo.InvariantCulture, "({0:0.000} gold)", cost > 0 ? calculations.TargetPhysHealth / cost : 0);
            targetMagiCost.Text = String.Format(CultureInfo.InvariantCulture, "({0:0.000} gold)", cost > 0 ? calculations.TargetMagiHealth / cost : 0);
            targetItemsTotalCost.Text = cost.ToString(CultureInfo.InvariantCulture);
        }

        private void targetSovAura_CheckedChanged(object sender, EventArgs e) {
            calculations.TargetSovAura = targetSovAura.Checked;
            CalculateTarget();
        }

        private void targetHeartward_CheckedChanged(object sender, EventArgs e) {
            calculations.TargetHeartward = targetHeartward.Checked;
            CalculateTarget();
        }

        private void targetLotusPassive_CheckedChanged(object sender, EventArgs e) {
            calculations.TargetLotusCrown = targetLotusPassive.Checked;
            CalculateTarget();
        }

        private void targetShiftersPassive_CheckedChanged(object sender, EventArgs e) {
            calculations.TargetShifterShield = targetShiftersPassive.Checked;
            CalculateTarget();
        }

        private void targetAdditionalHealth_TextChanged(object sender, EventArgs e) {
            double health;
            if(Double.TryParse(targetAdditionalHealth.Text, out health)) {
                calculations.TargetAdditionalHealth = health;
                CalculateTarget();
                targetAdditionalHealth.ForeColor = SystemColors.ControlText;
            } else {
                targetAdditionalHealth.ForeColor = Color.Red;
            }
        }

        private void targetAdditionalPhysProt_TextChanged(object sender, EventArgs e) {
            double phys;
            if(Double.TryParse(targetAdditionalPhysProt.Text, out phys)) {
                calculations.TargetAdditionalPhysProtection = phys;
                CalculateTarget();
                targetAdditionalPhysProt.ForeColor = SystemColors.ControlText;
            } else {
                targetAdditionalPhysProt.ForeColor = Color.Red;
            }
        }

        private void targetAdditionalMagiProt_TextChanged(object sender, EventArgs e) {
            double magi;
            if(Double.TryParse(targetAdditionalMagiProt.Text, out magi)) {
                calculations.TargetAdditionalMagicalProtection = magi;
                CalculateTarget();
                targetAdditionalMagiProt.ForeColor = SystemColors.ControlText;
            } else {
                targetAdditionalMagiProt.ForeColor = Color.Red;
            }
        }

        private void targetWarlockStacks_ValueChanged(object sender, EventArgs e) {
            targetWarlockStacksLabel.Text = targetWarlockStacks.Value.ToString();
            calculations.TargetWarlockStacks = targetWarlockStacks.Value;
            CalculateTarget();
        }

        private void targetUrchinStacks_ValueChanged(object sender, EventArgs e) {
            targetUrchinStacksLabel.Text = targetUrchinStacks.Value.ToString();
            calculations.TargetUrchinStacks = targetUrchinStacks.Value;
            CalculateTarget();
        }

        private void targetReset_Click(object sender, EventArgs e) {
            dontCalculate = true;

            targetGod.SelectedIndex = 0;
            targetGodLvl.Value = 1;

            targetItem1.SelectedItem = emptyItem;
            targetItem2.SelectedItem = emptyItem;
            targetItem3.SelectedItem = emptyItem;
            targetItem4.SelectedItem = emptyItem;
            targetItem5.SelectedItem = emptyItem;
            targetItem6.SelectedItem = emptyItem;

            targetAdditionalHealth.Text = "0";
            targetAdditionalPhysProt.Text = "0";
            targetAdditionalMagiProt.Text = "0";
            targetLifesteal.Text = "0";

            targetWarlockStacks.Value = 0;
            targetUrchinStacks.Value = 0;

            targetSovAura.Checked = false;
            targetHeartward.Checked = false;
            targetLotusPassive.Checked = false;
            targetShiftersPassive.Checked = false;

            dontCalculate = false;
            CalculateTarget();
        }

        public void CalculateSelf() {
            if(dontCalculate) return;

            double cost = calculations.SelfItems.Sum(i => i == null ? 0 : i.Cost);
            selfItemsTotalCost.Text = cost.ToString(CultureInfo.InvariantCulture);

            selfInhandDamage.Text = calculations.SelfInHandDamage.ToString(CultureInfo.InvariantCulture);
        }

        private void selfGod_SelectedIndexChanged(object sender, EventArgs e) {
            GodStat god = (GodStat)selfGod.SelectedItem;
            calculations.SelfGod = god;
            if(god.PowerType == GodType.Physical) {
                selfPhysical.Checked = true;
            } else {
                selfMagical.Checked = true;
            }

            ItemStat item1 = (ItemStat)selfItem1.SelectedItem;
            ItemStat item2 = (ItemStat)selfItem2.SelectedItem;
            ItemStat item3 = (ItemStat)selfItem3.SelectedItem;
            ItemStat item4 = (ItemStat)selfItem4.SelectedItem;
            ItemStat item5 = (ItemStat)selfItem5.SelectedItem;
            ItemStat item6 = (ItemStat)selfItem6.SelectedItem;

            selfItem1.Items.Clear();
            selfItem2.Items.Clear();
            selfItem3.Items.Clear();
            selfItem4.Items.Clear();
            selfItem5.Items.Clear();
            selfItem6.Items.Clear();

            foreach(ItemStat item in items) {
                if(item.Name == "Empty") emptyItem = item;

                if(item.ItemType != ItemType.Both && item.ItemType.ToString() != god.PowerType.ToString()) continue;
                if(god.Name != "Ratatoskr" && (item.Name == "Opal Acorn" || item.Name == "Sapphire Acorn")) continue;

                selfItem1.Items.Add(item);
                selfItem2.Items.Add(item);
                selfItem3.Items.Add(item);
                selfItem4.Items.Add(item);
                selfItem5.Items.Add(item);
                selfItem6.Items.Add(item);

                if(item == item1) selfItem1.SelectedItem = item;
                if(item == item2) selfItem2.SelectedItem = item;
                if(item == item3) selfItem3.SelectedItem = item;
                if(item == item4) selfItem4.SelectedItem = item;
                if(item == item5) selfItem5.SelectedItem = item;
                if(item == item6) selfItem6.SelectedItem = item;
            }

            if(selfItem1.SelectedItem == null) selfItem1.SelectedItem = emptyItem;
            if(selfItem2.SelectedItem == null) selfItem2.SelectedItem = emptyItem;
            if(selfItem3.SelectedItem == null) selfItem3.SelectedItem = emptyItem;
            if(selfItem4.SelectedItem == null) selfItem4.SelectedItem = emptyItem;
            if(selfItem5.SelectedItem == null) selfItem5.SelectedItem = emptyItem;
            if(selfItem6.SelectedItem == null) selfItem6.SelectedItem = emptyItem;

            selfSteroid1Drop.Items.Clear();
            selfSteroid1Drop.Invalidate();

            selfSteroid2Drop.Items.Clear();
            selfSteroid2Drop.Invalidate();

            selfSteroid1Drop.Enabled = god.FirstSteroid != null;
            if(selfSteroid1Drop.Enabled) {
                if(!String.IsNullOrEmpty(god.FirstSteroid.Enabled)) selfSteroid1Drop.Items.Add(god.FirstSteroid.Enabled);
                if(!String.IsNullOrEmpty(god.FirstSteroid.Disabled)) selfSteroid1Drop.Items.Add(god.FirstSteroid.Disabled);

                selfSteroid1Drop.SelectedIndex = 0;
            } else {
                calculations.SelfSteroid1 = "";
            }

            selfSteroid2Drop.Enabled = god.SecondSteroid != null;
            if(selfSteroid2Drop.Enabled) {
                if(!String.IsNullOrEmpty(god.SecondSteroid.Enabled)) selfSteroid2Drop.Items.Add(god.SecondSteroid.Enabled);
                if(!String.IsNullOrEmpty(god.SecondSteroid.Disabled)) selfSteroid2Drop.Items.Add(god.SecondSteroid.Disabled);

                selfSteroid2Drop.SelectedIndex = 0;
            } else {
                calculations.SelfSteroid2 = "";
            }

            selfSteroid1Stacks.Enabled = !String.IsNullOrEmpty(god.Extra);
            selfSteroid1StacksLabel.Text = selfSteroid1Stacks.Enabled ? god.Extra : "Unused";

            selfSteroid2Stacks.Enabled = god.Name == "Bakasura" || god.Name == "Ah Puch";
            if(selfSteroid2Stacks.Enabled) {
                selfSteroid2StacksLabel.Text = god.Name == "Bakasura" ? "Insatiable Hunger Stacks? 0-3" : "Antiheal Stacks? 0-10";
            } else {
                selfSteroid2StacksLabel.Text = "Unused";
            }

            selfBellonaStance.Enabled = god.Name == "Bellona";

            CalculateSelf();
        }

        private void selfGodLvl_ValueChanged(object sender, EventArgs e) {
            selfGodLvlLabel.Text = selfGodLvl.Value.ToString();
            calculations.SelfLevel = selfGodLvl.Value;
            CalculateSelf();
        }

        private void selfHealthPerc_ValueChanged(object sender, EventArgs e) {
            selfHealthPercLabel.Text = selfHealthPerc.Value.ToString();
            calculations.SelfHealth = selfHealthPerc.Value;
            CalculateSelf();
        }

        private void selfItem1_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.First = (ItemStat)selfItem1.SelectedItem;
            SetItem(selfItem1, selfItem1Cost, selfItem1Pic, false);
        }

        private void selfItem2_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.Second = (ItemStat)selfItem2.SelectedItem;
            SetItem(selfItem2, selfItem2Cost, selfItem2Pic, false);
        }

        private void selfItem3_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.Third = (ItemStat)selfItem3.SelectedItem;
            SetItem(selfItem3, selfItem3Cost, selfItem3Pic, false);
        }

        private void selfItem4_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.Fourth = (ItemStat)selfItem4.SelectedItem;
            SetItem(selfItem4, selfItem4Cost, selfItem4Pic, false);
        }

        private void selfItem5_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.Fifth = (ItemStat)selfItem5.SelectedItem;
            SetItem(selfItem5, selfItem5Cost, selfItem5Pic, false);
        }

        private void selfItem6_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfItems.Sixth = (ItemStat)selfItem6.SelectedItem;
            SetItem(selfItem6, selfItem6Cost, selfItem6Pic, false);
        }

        private void selfDevoStacks_ValueChanged(object sender, EventArgs e) {
            selfDevoStacksLabel.Text = selfDevoStacks.Value.ToString();
            calculations.SelfDevourerStacks = selfDevoStacks.Value;
            CalculateSelf();
        }

        private void selfTransStacks_ValueChanged(object sender, EventArgs e) {
            selfTransStacksLabel.Text = selfTransStacks.Value.ToString();
            calculations.SelfTranscendenceStacks = selfTransStacks.Value;
            CalculateSelf();
        }

        private void selfHeartStacks_ValueChanged(object sender, EventArgs e) {
            selfHeartStacksLabel.Text = selfHeartStacks.Value.ToString();
            calculations.SelfHeartseekerStacks = selfHeartStacks.Value;
            CalculateSelf();
        }

        private void selfDoomStacks_ValueChanged(object sender, EventArgs e) {
            selfDoomStacksLabel.Text = selfDoomStacks.Value.ToString();
            calculations.SelfDoomOrbStacks = selfDoomStacks.Value;
            CalculateSelf();
        }

        private void selfThothStacks_ValueChanged(object sender, EventArgs e) {
            selfThothStacksLabel.Text = selfThothStacks.Value.ToString();
            calculations.SelfThothStacks = selfThothStacks.Value;
            CalculateSelf();
        }

        private void selfWarlockStacks_ValueChanged(object sender, EventArgs e) {
            selfWarlockStacksLabel.Text = selfWarlockStacks.Value.ToString();
            calculations.SelfWarlockStacks = selfWarlockStacks.Value;
            CalculateSelf();
        }

        private void selfAncileStacks_ValueChanged(object sender, EventArgs e) {
            selfAncileStacksLabel.Text = selfAncileStacks.Value.ToString();
            calculations.SelfAncileStacks = selfAncileStacks.Value;
            CalculateSelf();
        }

        private void selfSteroid1Drop_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfSteroid1 = selfSteroid1Drop.SelectedItem.ToString();
        }

        private void selfSteroid2Drop_SelectedIndexChanged(object sender, EventArgs e) {
            calculations.SelfSteroid2 = selfSteroid2Drop.SelectedItem.ToString();
        }

        private void selfRedBuffToggle_CheckedChanged(object sender, EventArgs e) {
            calculations.SelfRedBuff = selfRedBuffToggle.Checked;
        }

        private void selfPurpleBuffToggle_CheckedChanged(object sender, EventArgs e) {
            calculations.SelfPurpleBuff = selfPurpleBuffToggle.Checked;
        }

        private void selfPythagToggle_CheckedChanged(object sender, EventArgs e) {
            calculations.SelfPythagAura = selfPythagToggle.Checked;
        }

        private void selfVoidstoneToggle_CheckedChanged(object sender, EventArgs e) {
            calculations.SelfVoidstone = selfVoidstoneToggle.Checked;
        }

        private void selfAchillesToggle_CheckedChanged(object sender, EventArgs e) {
            calculations.SelfAchillesSpear = selfAchillesToggle.Checked;
        }

        private void selfBellonaStance_SelectedIndexChanged(object sender, EventArgs e) {}
    }
}