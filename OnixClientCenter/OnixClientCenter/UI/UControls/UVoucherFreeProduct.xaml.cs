using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UVoucherFreeProduct : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
        DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UVoucherFreeProduct),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UVoucherFreeProduct()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UVoucherFreeProduct control = obj as UVoucherFreeProduct;
            MPackageVoucher d = (MPackageVoucher)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UVoucherFreeProduct control, MPackageVoucher v)
        {
            if (v == null)
            {
                return;
            }


            MPackageVoucher o = new MPackageVoucher(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            int selectionType = CUtil.StringToInt(o.SelectionType);

            int i = 1;
            if (selectionType >= 4)
            {
                i = 2;
            }
            control.cboSelectionType.SelectedIndex = selectionType - i;

            control.stackValue.Visibility = Visibility.Visible;
            control.stackOther.Visibility = Visibility.Collapsed;

            if (selectionType == 1)
            {
                control.lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                control.lkupItem.SelectedObject = o.ServiceObj;   
            }
            else if (selectionType == 2)
            {
                control.lkupItem.Lookup = LookupSearchType2.InventoryItemLookup;
                control.lkupItem.SelectedObject = o.ItemObj;
            }
            else if (selectionType == 3)
            {
                control.lkupItem.Lookup = LookupSearchType2.ItemCategoryLookup;
                control.lkupItem.SelectedObject = o.ItemCategoryObj; 
            }
            //else if (selectionType == 4)
            //{
            //    control.lkupItem.Lookup = LookupSearchType2.VoucherLookup;
            //    control.lkupItem.SelectedObject = o.VoucherObj;
            //}
            else if (selectionType == 5)
            {
                control.stackValue.Visibility = Visibility.Collapsed;
                control.stackOther.Visibility = Visibility.Visible;
            }
            else
            {
                control.lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                control.lkupItem.SelectedObject = o.ServiceObj;
            }

            control.txtQuantity.Text = o.Quantity;
            control.txtOther.Text = o.FreeText;

            v.ExtFlag = "I";
            if (v.PackageVoucherID.Equals(""))
            {
                v.ExtFlag = "A";
            }
        }

        private void updateObject()
        {
            MPackageVoucher v = (MPackageVoucher)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.ServiceObj = null;
            v.ItemObj = null;
            v.ItemCategoryObj = null;
            v.VoucherObj = null;

            if (lkupItem.SelectedObject != null)
            {
                if (lkupItem.Lookup == LookupSearchType2.ServiceLookup)
                {
                    v.ServiceObj = lkupItem.SelectedObject;
                    v.SelectionType = "1";
                }
                else if (lkupItem.Lookup == LookupSearchType2.InventoryItemLookup)
                {
                    v.ItemObj = lkupItem.SelectedObject;
                    v.SelectionType = "2";
                }
                else if (lkupItem.Lookup == LookupSearchType2.ItemCategoryLookup)
                {
                    v.ItemCategoryObj = lkupItem.SelectedObject;
                    v.SelectionType = "3";
                }
                //else if (lkupItem.Lookup == LookupSearchType2.VoucherLookup)
                //{
                //    v.VoucherObj = lkupItem.SelectedObject;
                //    v.SelectionType = "4";
                //}
            }
            else
            {
                v.SelectionType = "5";
            }

            v.FreeText = txtOther.Text;
            v.Quantity = txtQuantity.Text;
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UVoucherFreeProduct control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            if (lkupItem != null)
            {
                lblUnit.Content = "";

                if (lkupItem.Lookup == LookupSearchType2.ServiceLookup)
                {
                    MService sv = (MService)lkupItem.SelectedObject;
                    if (sv != null)
                    {
                        lblUnit.Content = sv.ServiceUOMName;
                    }
                }
                else if (lkupItem.Lookup == LookupSearchType2.InventoryItemLookup)
                {
                    MInventoryItem sv = (MInventoryItem)lkupItem.SelectedObject;
                    if (sv != null)
                    {
                        lblUnit.Content = sv.ItemUOMName;
                    }
                }

                updateObject();
            }
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {
            UVoucherFreeProduct control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageVoucher v = (MPackageVoucher)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UVoucherFreeProduct control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageVoucher v = (MPackageVoucher)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "N";
        }

        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[0-9]+$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            UVoucherFreeProduct control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UVoucherFreeProduct control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            ComboBox cbo = sender as ComboBox;

            if (lkupItem != null)
            {
                lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                lkupItem.SelectedObject = null;
                lkupItem.IsEnabled = true;
            }

            control.stackValue.Visibility = Visibility.Visible;
            control.stackOther.Visibility = Visibility.Collapsed;
            if (cbo.SelectedIndex == 0)
            {
                lkupItem.Lookup = LookupSearchType2.ServiceLookup;
            }
            else if (cbo.SelectedIndex == 1)
            {
                lkupItem.Lookup = LookupSearchType2.InventoryItemLookup;
            }
            else if (cbo.SelectedIndex == 2)
            {
                //lkupItem.Lookup = LookupSearchType2.VoucherLookup;
                lkupItem.IsEnabled = false;
            }
            else if (cbo.SelectedIndex == 3)
            {
                control.stackValue.Visibility = Visibility.Collapsed;
                control.stackOther.Visibility = Visibility.Visible;
            }

            lkupItem.SelectedObject = null;

            updateObject();
        }
    }
}
