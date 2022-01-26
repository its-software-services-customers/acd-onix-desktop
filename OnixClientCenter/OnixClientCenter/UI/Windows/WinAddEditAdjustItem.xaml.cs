using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinAddEditAdjustItem : Window
    {
        private MInventoryTransaction vw = null;
        private MInventoryTransaction actualView = null;
        private MInventoryDoc parentView = null;

        private ObservableCollection<MInventoryTransaction> parentItemsSource = null;        
        private MLocation locationObj = null;
        private DateTime documentDate = DateTime.Now;

        public String Caption = "";
        public String Mode = "";
        public Boolean HasModified = false;

        private InventoryDocumentType Idt;

        public WinAddEditAdjustItem(InventoryDocumentType idt)
        {            
            InitializeComponent();
            lkupItem.Focus();
            Idt = idt;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public MLocation LocationObj
        {
            set
            {
                locationObj = value;
            }

            get
            {
                return (locationObj);
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                return (documentDate);
            }

            set
            {
                documentDate = value;
            }
        }

        public MInventoryTransaction ViewData
        {
            set
            {
                actualView = (MInventoryTransaction) value;
            }
        }

        public MInventoryDoc ParentView
        {
            set
            {
                parentView = (MInventoryDoc)value;
            }
        }

        public ObservableCollection<MInventoryTransaction> ParentItemSource
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

            result = CHelper.ValidateTextBox(lblTotalPrice, txtItemAmount, false, InputDataType.InputTypeZeroPossitiveDecimal);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblUnitPrice, txtUnitPrice, false);
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

                return (true);
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

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void LoadData()
        {
            this.Title = Caption;

            CTable t = new CTable("INVENTORY_TX");
            vw = new MInventoryTransaction(t);
            //vw.LocationObj = this.locationObj;
            vw.CreateDefaultValue();

            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E"))
            {
                CTable newDB = actualView.GetDbObject().Clone();
                vw.SetDbObject(newDB);
                vw.NotifyAllPropertiesChanged();
                radUp.IsChecked = vw.UpSelected;
                radDown.IsChecked = vw.DownSelected;
            }
            else
            {
                vw.DownSelected = true;
                radDown.IsChecked = true;
            }

            vw.IsModified = false;

            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            lkupItem.Focus();
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

        private void lkupItem_SelectedItemChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void radUp_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            vw.UpSelected = true;
        }

        private void radDown_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            vw.DownSelected = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void cbxLotSerial_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void txtLotSerial1_TextChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
