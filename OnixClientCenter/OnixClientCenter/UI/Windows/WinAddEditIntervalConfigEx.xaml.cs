using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditIntervalConfigEx : Window
    {
        private ObservableCollection<MInterval> intervals = new ObservableCollection<MInterval>();
        private MIntervalConfig cfg = new MIntervalConfig(new CTable(""));
        private Boolean isOK = false;
        private String t = "IP";

        private ObservableCollection<MMasterRef> outputsStep = new ObservableCollection<MMasterRef>();
        private ObservableCollection<MMasterRef> outputsTier = new ObservableCollection<MMasterRef>();
        private ObservableCollection<MMasterRef> mappingType = new ObservableCollection<MMasterRef>();

        public WinAddEditIntervalConfigEx(String cfgString, String itemName, String type)
        {
            t = type;
            initComboBox();

            cfg.StepSelectionMap = outputsStep;
            cfg.TierSelectionMap = outputsTier;
            cfg.MappingTypeMap = mappingType;

            cfg.Name = itemName;
            cfg.DeserializeConfig(cfgString);
            cfg.IsModified = false;

            this.DataContext = cfg;
            InitializeComponent();
        }

        private void addItem(ObservableCollection<MMasterRef> coll, String id, String caption)
        {
            MMasterRef m = new MMasterRef(new CTable(""));
            m.MasterID = id;
            m.Description = caption;

            coll.Add(m);
        }

        private void initComboBox()
        {
            if (t.Equals("IP"))
            {
                //IP - Item & Service Price
                addItem(mappingType, "1", CLanguage.getValue("mapping_by_quantity"));
            }
            else if (t.Equals("DISCOUNT"))
            {
                addItem(mappingType, "0", CLanguage.getValue("mapping_by_quantity"));
                addItem(mappingType, "1", CLanguage.getValue("mapping_by_amount"));
            }
        }

        private void initPriceByQuantityCombo()
        {
            outputsStep.Clear();
            addItem(outputsStep, "0", CLanguage.getValue("for_unit_price"));
            addItem(outputsStep, "1", CLanguage.getValue("for_total_price"));

            outputsTier.Clear();
            addItem(outputsTier, "0", CLanguage.getValue("for_unit_price"));
            addItem(outputsTier, "1", CLanguage.getValue("for_total_price"));
        }

        private void initDiscountByQuantityCombo()
        {
            //Discount by Quantity
            outputsStep.Clear();
            addItem(outputsStep, "0", CLanguage.getValue("discount_output_fix"));
            addItem(outputsStep, "1", CLanguage.getValue("discount_output_per_unit"));

            outputsTier.Clear();
            addItem(outputsTier, "0", CLanguage.getValue("discount_output_fix"));
            addItem(outputsTier, "1", CLanguage.getValue("discount_output_per_unit"));
            addItem(outputsTier, "2", CLanguage.getValue("discount_output_percent"));
        }

        private void initDiscountByAmountCombo()
        {
            //Discount by Amount
            outputsStep.Clear();
            addItem(outputsStep, "0", CLanguage.getValue("discount_output_fix"));
            addItem(outputsStep, "2", CLanguage.getValue("discount_output_percent"));

            outputsTier.Clear();
            addItem(outputsTier, "0", CLanguage.getValue("discount_output_fix"));
            addItem(outputsTier, "2", CLanguage.getValue("discount_output_percent"));
        }

        public ObservableCollection<MMasterRef> OutputTypesStep
        {
            get
            {
                return (outputsStep);
            }
        }

        public ObservableCollection<MMasterRef> OutputTypesTier
        {
            get
            {
                return (outputsTier);
            }
        }

        public ObservableCollection<MMasterRef> MappingTypes
        {
            get
            {
                return (mappingType);
            }
        }

        public Visibility StepTypeVisibility
        {
            get
            {
                return (Visibility.Visible);
            }
        }

        public Visibility TierTypeVisibility
        {
            get
            {
                return (Visibility.Visible);
            }
        }

        public Boolean EnableStepType
        {
            get
            {
                return (true);
            }
        }

        public Boolean EnableTierType
        {
            get
            {
                return (true);
            }
        }

        public String ConfigString
        {
            get
            {
                return (cfg.ConfigString);
            }
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblDesc, txtDesc, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBoxEscape(lblDesc, txtDesc, "|");
            if (!result)
            {
                return (result);
            }
            
            result = CHelper.ValidateComboBox(CLanguage.getValue("price") + CLanguage.getValue("tier"), cboTierUnitType, false);
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private void RootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cfg.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = ValidateData();
                    isOK = r;
                    e.Cancel = !r;
                }
                else
                {
                    isOK = false;
                }
            }
        }

        private void RootElement_ContentRendered(object sender, EventArgs e)
        {
            if (cfg.SelectionType == 2)
            {
                radTier.IsChecked = true;
            }
            else if (cfg.SelectionType == 1)
            {
                radStep.IsChecked = true;
            }
            else
            {
                radTier.IsChecked = true;
            }

            cfg.IsModified = false;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            cfg.AddInterval();
            cfg.IsModified = true;
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = ValidateData();
            if (r)
            {
                cfg.IsModified = false;
                isOK = true;
                this.Close();
            }
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            isOK = false;
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            MInterval v = (MInterval) (sender as Button).Tag;
            cfg.RemoveInterval(v);
            cfg.IsModified = true;
        }

        private void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            String value = tb.Text;
            MInterval vi = (MInterval) tb.Tag; 

            if (value.Trim().Equals(""))
            {
                vi.ToValue = vi.OldToValue;
                return;
            }

            double toQty = CUtil.StringToDouble(value);
            double fromQty = CUtil.StringToDouble(vi.FromValue);

            if (toQty <= fromQty)
            {
                vi.ToValue = vi.OldToValue;
                return;
            }

            cfg.ArrangeInterval();
        }

        private void radStep_Checked(object sender, RoutedEventArgs e)
        {
            cfg.SelectionType = 1;
            cfg.IsModified = true;
        }

        private void radTier_Checked(object sender, RoutedEventArgs e)
        {
            cfg.SelectionType = 2;
            cfg.IsModified = true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            cfg.IsModified = true;
        }

        private void cboStepUnitType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cfg.IsModified = true;
        }

        private void cboMappingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MMasterRef m = (MMasterRef) cboMappingType.SelectedItem;
            if (m == null)
            {
                return;
            }

            if (t.Equals("IP"))
            {
                if (m.MasterID.Equals("1"))
                {
                    initPriceByQuantityCombo();
                }
            }
            else if (t.Equals("DISCOUNT"))
            {
                if (m.MasterID.Equals("0"))
                {
                    initDiscountByQuantityCombo();
                }
                else
                {
                    //1 Amount
                    initDiscountByAmountCombo();
                }
            }
        }

        private void RootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
