using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.UControls;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditAccountSaleDocItem : Window
    {
        private MAccountDocItem vw = null;
        private MAccountDocItem actualView = null;
        private MAccountDoc parentView = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;

        private Boolean isInLoad = false;
        private Boolean isPromotionMode = false;
        private Boolean isSaleOrderMode = false;

        public MAccountDocItem ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public MAccountDoc ParentView
        {
            set
            {
                parentView = value;
            }
        }

        public Boolean IsPromotionMode
        {
            get
            {
                return (isPromotionMode);
            }
        }

        public Boolean IsManualMode
        {
            get
            {
                return (!isPromotionMode);
            }
        }

        public Boolean IsSaleOrder
        {
            get
            {
                return (isSaleOrderMode);
            }
        }

        public WinAddEditAccountSaleDocItem(Boolean isPromotion, Boolean isSaleOrder)
        {
            isPromotionMode = isPromotion;
            isSaleOrderMode = isSaleOrder;
            InitializeComponent();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private LookupSearchType2 selectionTypeToLookup(String selType)
        {
            LookupSearchType2 lkup;

            if (selType.Equals("1"))
            {
                lkup = LookupSearchType2.ServiceRegularSaleLookup;
            }
            else
            {
                //2
                lkup = LookupSearchType2.InventoryItemLookup;
            }

            return (lkup);
        }

        private void LoadData()
        {
            isInLoad = true;
            this.Title = Caption;
            lkup.Focus();

            CTable t = new CTable("");
            vw = new MAccountDocItem(t);
            vw.CreateDefaultValue();

            DataContext = vw;
            //lkup.AssociateObject = parentView.CustomerObj;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);

                lkup.Lookup = selectionTypeToLookup(vw.SelectType);

                vw.NotifyAllPropertiesChanged();
            }
            else
            {
                vw.SelectType = CGlobalVariable.GetGlobalVariableValue("DEFAULT_SELECTION_TYPE_SALE");
                lkup.Lookup = selectionTypeToLookup(vw.SelectType);
            }

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
            isInLoad = false;
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            if (vw.SelectType.Equals("3"))
            {
                CMessageBox.Show(CLanguage.getValue("ERROR_TYPE_NOT_SUPPORT"), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return (false);
            }

            result = CHelper.ValidateLookup(lb_itemcode, lkup, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lb_quantity, txt_quantity, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lb_unitprice, txt_unitprice, true);
            if (!result)
            {
                return (result);
            }

            if ((vw.IsWhTax) && (CUtil.StringToDouble(vw.WHTaxPct) <= 0.00)) 
            {
                CHelper.ShowErorMessage("", "ERROR_WH_MUST_GREATER_THAN_ZERO", null);
                return (false);
            }

            result = CHelper.ValidateLookup(lblProject, uProject, IsSaleOrder);
            if (!result)
            {
                return (result);
            }


            return (result);
        }

        private void btn_Ok_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                HasModified = true;

                vw.IsModified = false;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private Boolean SaveData()
        {
            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    parentView.AddAccountDocItem(vw);
                    return (true);
                }

                return (false);
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CTable o = actualView.GetDbObject();
                        o.CopyFrom(vw.GetDbObject());

                        actualView.NotifyAllPropertiesChanged();

                        return (true);
                    }

                    return (false);
                }
            }

            return (true);
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private void lkup_SelectedItemChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;


            if (lkup.SelectedObject == null)
            {
                return;
            }

            if (lkup.Lookup == LookupSearchType2.InventoryItemLookup)
            {
                vw.ItemUnitName = (lkup.SelectedObject as MInventoryItem).ItemUOMName;
            }
            else
            {
                vw.ServiceUnitName = (lkup.SelectedObject as MService).ServiceUOMName;
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void NumberPercentageValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            string expression = @"^(0*100{1,1}\.?((?<=\.)0*)?%?$)|(^0*\d{0,2}\.?((?<=\.)\d*)?%?)$|(^[.][0-9]+$)|(^[0-9]*[.]{0,1}[0-9]*$)";
            Regex regex = new Regex(expression);
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void cbxAllow_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllow_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isInLoad)
            {
                return;
            }

            ComboBox cbo = sender as ComboBox;

            if (lkup != null)
            {
                lkup.Lookup = selectionTypeToLookup(vw.SelectType);
                lkup.SelectedObject = null;                
            }

            vw.IsModified = true;
        }

        private void rootElement_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
        }

        private void rootElement_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    Boolean r = SaveData();
                    if (r)
                    {
                        HasModified = true;
                        CUtil.EnableForm(true, this);
                    }
                }
            }
        }

        private void radDiscountByPct_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void uProject_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
