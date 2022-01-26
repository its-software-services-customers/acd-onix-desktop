using System;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UCustomerSelection : UserControl
    {
        public static readonly DependencyProperty CustomerSelectedProperty =
        DependencyProperty.Register("CustomerSelected", typeof(MBaseModel), typeof(UCustomerSelection),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnCustomerSelectedChanged)));

        public event EventHandler OnChanged;

        public UCustomerSelection()
        {
            InitializeComponent();
        }

        private static void OnCustomerSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UCustomerSelection control = obj as UCustomerSelection;
            MPackageCustomer d = (MPackageCustomer) e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel CustomerSelected
        {
            get { return (MBaseModel)GetValue(CustomerSelectedProperty); }
            set { SetValue(CustomerSelectedProperty, value); }
        }

        private static void updateGui(UCustomerSelection control, MPackageCustomer v)
        {
            if (v == null)
            {
                //Reach when deleted
                return;
            }

            MPackageCustomer o = new MPackageCustomer(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            int selectionType = CUtil.StringToInt(o.SelectionType);

            control.cboSelectionType.SelectedIndex = selectionType - 1;

            if (selectionType == 1)
            {
                control.lkupItem.SelectedObject = o.CustomerTypeObj;
            }
            else if (selectionType == 2)
            {
                control.lkupItem.SelectedObject = o.CustomerGroupObj;
            }
            else if (selectionType == 3)
            {
                control.lkupItem.SelectedObject = o.CustomerObj;
            }
            else
            {
                control.lkupItem.SelectedObject = o.CustomerGroupObj;
            }

            //This function call only one at the beginning
            v.ExtFlag = "I";
            if (v.PackageCustomerID.Equals(""))
            {
                v.ExtFlag = "A";
            }
        }

        private void updateObject()
        {
            MPackageCustomer v = (MPackageCustomer)CustomerSelected;
            if (v == null)
            {
                return;
            }

            v.CustomerGroupObj = null;
            v.CustomerTypeObj = null;
            v.CustomerObj = null;

            if (lkupItem.SelectedObject != null)
            {
                if (lkupItem.Lookup == LookupSearchType2.CustomerGroupLookup)
                {
                    v.CustomerGroupObj = lkupItem.SelectedObject;
                    v.SelectionType = "2";
                }
                else if (lkupItem.Lookup == LookupSearchType2.CustomerTypeLookup)
                {
                    v.CustomerTypeObj = lkupItem.SelectedObject;
                    v.SelectionType = "1";
                }
                else if (lkupItem.Lookup == LookupSearchType2.CustomerLookup)
                {
                    v.CustomerObj = lkupItem.SelectedObject;
                    v.SelectionType = "3";
                }
            }
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UCustomerSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            if (lkupItem != null)
            {
                updateObject();
            }
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {
            UCustomerSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageCustomer v = (MPackageCustomer)CustomerSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UCustomerSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageCustomer v = (MPackageCustomer)CustomerSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "N";
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UCustomerSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            ComboBox cbo = sender as ComboBox;

            if (lkupItem != null)
            {
                lkupItem.Lookup = LookupSearchType2.CustomerGroupLookup;
                lkupItem.SelectedObject = null;
            }

            if (cbo.SelectedIndex == 0)
            {
                lkupItem.Lookup = LookupSearchType2.CustomerTypeLookup;
                lkupItem.Caption = CLanguage.getValue("customer_type");
            }
            else if (cbo.SelectedIndex == 1)
            {
                lkupItem.Lookup = LookupSearchType2.CustomerGroupLookup;
                lkupItem.Caption = CLanguage.getValue("customer_group");
            }
            else if (cbo.SelectedIndex == 2)
            {
                lkupItem.Lookup = LookupSearchType2.CustomerLookup;
                lkupItem.Caption = CLanguage.getValue("customer");
            }

            lkupItem.SelectedObject = null;

            updateObject();
        }
    }
}
