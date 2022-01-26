using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinAddEditImportItem : Window
    {
        private MInventoryTransactionImport vw = null;
        private MInventoryTransactionImport actualView = null;
        private MInventoryDoc parentView = null;

        private ObservableCollection<MInventoryTransactionImport> parentItemsSource = null;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;
        public Boolean DialogOK = false;

        private InventoryDocumentType Idt;

        public WinAddEditImportItem(InventoryDocumentType idt)
        {
            vw = new MInventoryTransactionImport(new CTable(""));
            vw.CreateDefaultValue();
            vw.TxType = "I";

            Idt = idt;
            DataContext = vw;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public MInventoryTransactionImport ViewData
        {
            set
            {
                actualView = (MInventoryTransactionImport) value;
            }
        }

        public MInventoryDoc ParentView
        {
            set
            {
                parentView = (MInventoryDoc)value;
            }
        }

        public ObservableCollection<MInventoryTransactionImport> ParentItemSource
        {
            set
            {
                parentItemsSource = value;
            }
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateLookup(lblCode, lkupItem, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblQuantity, txtQuantity, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblUnitPrice, txtUnitPrice, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private Boolean SaveData()
        {
            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    parentView.AddTxItem(vw, Idt);
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

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = SaveData();
            if (r)
            {
                HasModified = true;

                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);

                this.Close();
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOK.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    Boolean r = SaveData();
                    e.Cancel = !r;

                    if (r)
                    {
                        HasModified = true;
                    }
                }
            }
        }

        private void LoadData()
        {
            this.Title = Caption;            

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);
                vw.NotifyAllPropertiesChanged();
            }

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {            
            LoadData();
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void txtLotSerial1_TextChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cboReason_SelectedObjectChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
