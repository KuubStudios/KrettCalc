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

        public CalculatorForm() {
            InitializeComponent();
        }

        private void CalculatorForm_Load(object sender, EventArgs e) {
            tabControl.Appearance = TabAppearance.FlatButtons;
            tabControl.ItemSize = new Size(0, 1);
            tabControl.SizeMode = TabSizeMode.Fixed;

            XmlSerializer godSerializer = new XmlSerializer(typeof(List<GodStat>));
            using(FileStream fs = new FileStream("gods.xml", FileMode.Open)) {
                gods = (List<GodStat>)godSerializer.Deserialize(fs);
            }

            XmlSerializer itemSerializer = new XmlSerializer(typeof(List<ItemStat>));
            using(FileStream fs = new FileStream("items.xml", FileMode.Open)) {
                items = (List<ItemStat>)itemSerializer.Deserialize(fs);
            }

            foreach(GodStat god in gods) {
                targetGod.Items.Add(god);
            }

            targetGod.SelectedIndex = 0;
        }

        private void sliderTargetGodLvl_ValueChanged(object sender, EventArgs e) {
            targetGodLvlLabel.Text = targetGodLvl.Value.ToString();
            calculations.TargetLevel = targetGodLvl.Value;
            CalculateTarget();
        }

        private void btnTab1_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage1;
        }

        private void btnTab2_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage2;
        }

        private void btnTab3_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage3;
        }

        private void btnTab4_Click(object sender, EventArgs e) {
            tabControl.SelectedTab = tabPage4;
        }

        private void targetGod_SelectedIndexChanged(object sender, EventArgs e) {
            GodStat god = (GodStat)targetGod.SelectedItem;
            calculations.TargetGod = god;
            if(god.PowerType == GodType.Physical) {
                targetPhysical.Checked = true;
            } else {
                targetMagical.Checked = true;
            }

            ItemStat emptyItem = null;
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

                if(item.ItemType == ItemType.Both || item.ItemType.ToString() == god.PowerType.ToString()) {
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
            }

            if(targetItem1.SelectedItem == null) targetItem1.SelectedItem = emptyItem;
            if(targetItem2.SelectedItem == null) targetItem2.SelectedItem = emptyItem;
            if(targetItem3.SelectedItem == null) targetItem3.SelectedItem = emptyItem;
            if(targetItem4.SelectedItem == null) targetItem4.SelectedItem = emptyItem;
            if(targetItem5.SelectedItem == null) targetItem5.SelectedItem = emptyItem;
            if(targetItem6.SelectedItem == null) targetItem6.SelectedItem = emptyItem;

            CalculateTarget();
        }

        private void SetItem(ComboBox combo, MetroTextBox cost, PictureBox pic) {
            ItemStat item = (ItemStat)combo.SelectedItem;
            cost.Text = item.Cost.ToString();
            string name = item.Name.Replace(" ", "-").Replace("\'", "").ToLower();
            pic.ImageLocation = "images/" + name + ".png";

            targetWarlockStacks.Enabled = calculations.TargetItems.Any(i => i != null && i.Name == "Warlock's Sash");
            targetUrchinStacks.Enabled = calculations.TargetItems.Any(i => i != null && i.Name == "Hide of the Urchin");

            CalculateTarget();
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

        private void CalculateTarget() {
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
            if (Double.TryParse(targetAdditionalPhysProt.Text, out phys)) {
                calculations.TargetAdditionalPhysProtection = phys;
                CalculateTarget();
                targetAdditionalPhysProt.ForeColor = SystemColors.ControlText;
            } else {
                targetAdditionalPhysProt.ForeColor = Color.Red;
            }
        }

        private void targetAdditionalMagiProt_TextChanged(object sender, EventArgs e) {
            double magi;
            if (Double.TryParse(targetAdditionalMagiProt.Text, out magi)) {
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
    }
}