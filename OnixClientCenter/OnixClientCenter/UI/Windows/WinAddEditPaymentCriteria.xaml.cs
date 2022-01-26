using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinAddEditPaymentCriteria : Window
    {
        private MPaymentCriteria vw = null;
        private MPaymentCriteria actualView = null;
        private MAuxilaryDoc parentView = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;
        //private Boolean isInLoad = false;

        public MPaymentCriteria ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public String TotalPOAmount
        {
            get
            {
                return (parentView.RevenueExpenseAmtFmt);
            }

            set
            {
            }
        }

        public WinAddEditPaymentCriteria(MAuxilaryDoc pv)
        {
            parentView = pv;
            InitializeComponent();
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadData()
        {
            //isInLoad = true;
            this.Title = Caption;

            CTable t = new CTable("");
            vw = new MPaymentCriteria(t);
            vw.CreateDefaultValue();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);                
            }
            else
            {
                vw.VatPercent = CGlobalVariable.GetGlobalVariableValue("VAT_PERCENTAGE");
                vw.WhPercent = "3";
            }

            vw.NotifyAllPropertiesChanged();

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
            //isInLoad = false;
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblVatPct, txtVatPct, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblWhPct, txtWhPct, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblPercent, txtPercent, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblDesc, txtDescription, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblExpense, txtExpense, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblVatAmt, txtVatAmt, true);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblWhAmt, txtWhAmt, true);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateComboBox(lblWhGroup, cboWhGroup, !cboWhGroup.IsEnabled);
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
            vw.CalculateExtraFields();

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    parentView.AddPaymentCriteriaItem(vw);
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

        private void cbxAllow_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxAllow_Unchecked(object sender, RoutedEventArgs e)
        {
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

        private void cbxAuto_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            vw.CalculateAuto(parentView.RevenueExpenseAmt);
        }

        private void cboWhGroup_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void rootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
