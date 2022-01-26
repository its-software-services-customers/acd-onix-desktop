using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.ClientCenter.Windows;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UProductPriceDiscountTray : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
        DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UProductPriceDiscountTray),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UProductPriceDiscountTray()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UProductPriceDiscountTray control = obj as UProductPriceDiscountTray;
            MPackageTrayPriceDiscount d = (MPackageTrayPriceDiscount)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UProductPriceDiscountTray control, MPackageTrayPriceDiscount v)
        {
            if (v == null)
            {
                return;
            }

            MPackageTrayPriceDiscount o = new MPackageTrayPriceDiscount(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            control.cboSelectionType.SelectedIndex = CUtil.StringToInt(o.SelectionType)-1;
            //control.cbxStdPrice.IsChecked = o.StdPriceFlag.Equals("Y");

            if (o.SelectionType.Equals("1"))
            {
                control.lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                control.lkupItem.SelectedObject = o.ServiceObj;
            }
            else if (o.SelectionType.Equals("2"))
            {
                control.lkupItem.Lookup = LookupSearchType2.InventoryItemLookup;
                control.lkupItem.SelectedObject = o.ItemObj;
            }
            else
            {
                control.lkupItem.Lookup = LookupSearchType2.ItemCategoryLookup;
                control.lkupItem.SelectedObject = o.ItemCategoryObj;
            }

            //This function call only one at the beginning
            v.ExtFlag = "I";
            if (v.PackageTrayPriceID.Equals(""))
            {
                v.ExtFlag = "A";
            }
        }

        private void updateObject()
        {
            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount) ProductSelected;
            if (v == null)
            {
                return;
            }

            v.ServiceObj = null;
            v.ItemObj = null;
            v.ItemCategoryObj = null;

            if (lkupItem.SelectedObject != null)
            {
                if (lkupItem.Lookup == LookupSearchType2.ServiceLookup)
                {
                    MService svc = (MService) lkupItem.SelectedObject;

                    v.ServiceObj = svc;
                    v.SelectionType = "1";
                    v.Code = svc.ServiceCode;
                    v.Name = svc.ServiceName;
                }
                else if (lkupItem.Lookup == LookupSearchType2.InventoryItemLookup)
                {
                    MInventoryItem itm = (MInventoryItem)lkupItem.SelectedObject;

                    v.ItemObj = itm;
                    v.SelectionType = "2";
                    v.Code = itm.ItemCode;
                    v.Name = itm.ItemNameThai;
                }
                else if (lkupItem.Lookup == LookupSearchType2.ItemCategoryLookup)
                {
                    MItemCategory cat = (MItemCategory)lkupItem.SelectedObject;

                    v.ItemCategoryObj = cat;
                    v.SelectionType = "3";
                    v.Code = cat.ItemCategoryID;
                    v.Name = cat.CategoryName;
                }
            }
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UProductPriceDiscountTray control = this;
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
            UProductPriceDiscountTray control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductPriceDiscountTray control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "N";
        }

        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void cmdInterval_Click(object sender, RoutedEventArgs e)
        {
            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            UProductPriceDiscountTray control = this;

            WinAddEditIntervalConfigEx w = new WinAddEditIntervalConfigEx(v.PricingDefination, v.Name, "IP");
            w.ShowDialog();
            if (w.IsOK)
            {
                v.PricingDefination = w.ConfigString;

                if (control.OnChanged != null)
                {
                    control.OnChanged(control, null);
                }
            }
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UProductPriceDiscountTray control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            ComboBox cbo = sender as ComboBox;

            if (lkupItem != null)
            {
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
                    lkupItem.Lookup = LookupSearchType2.ItemCategoryLookup;
                }

                lkupItem.SelectedObject = null;
            }

            updateObject();
        }

        private void cmdDiscInterval_Click(object sender, RoutedEventArgs e)
        {
            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            UProductPriceDiscountTray control = this;

            WinAddEditIntervalConfigEx w = new WinAddEditIntervalConfigEx(v.DiscountDefination, v.Name, "DISCOUNT");
            w.ShowDialog();
            if (w.IsOK)
            {
                v.DiscountDefination = w.ConfigString;

                if (control.OnChanged != null)
                {
                    control.OnChanged(control, null);
                }
            }
        }

        private void cbxStdPrice_Checked(object sender, RoutedEventArgs e)
        {
            UProductPriceDiscountTray control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.StdPriceFlag = "Y";
        }

        private void cbxStdPrice_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductPriceDiscountTray control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageTrayPriceDiscount v = (MPackageTrayPriceDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.StdPriceFlag = "N";
        }
    }
}
