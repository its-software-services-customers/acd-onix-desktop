using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UProductBundleSelection : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
        DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UProductBundleSelection),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UProductBundleSelection()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UProductBundleSelection control = obj as UProductBundleSelection;
            MPackageBundle d = (MPackageBundle)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UProductBundleSelection control, MPackageBundle v)
        {
            if (v == null)
            {
                return;
            }

            MPackageBundle o = new MPackageBundle(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            int selectionType = CUtil.StringToInt(o.SelectionType);

            control.cboSelectionType.SelectedIndex = selectionType - 1;

            if (selectionType == 1)
            {
                control.lkupItem.SelectedObject = o.ServiceObj;
            }
            else if (selectionType == 2)
            {
                control.lkupItem.SelectedObject = o.ItemObj;
            }
            else if (selectionType == 3)
            {
                control.lkupItem.SelectedObject = o.ItemCategoryObj;
            }
            else
            {
                control.lkupItem.SelectedObject = o.ServiceObj;
            }

            control.txtQuantity.Text = o.Quantity;

            v.ExtFlag = "I";
            if (v.PackageBundleID.Equals(""))
            {
                v.ExtFlag = "A";
            }
        }

        private void updateObject()
        {
            MPackageBundle v = (MPackageBundle)ProductSelected;
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
            }

            v.Quantity = txtQuantity.Text;
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UProductBundleSelection control = this;
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
            UProductBundleSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageBundle v = (MPackageBundle)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductBundleSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageBundle v = (MPackageBundle)ProductSelected;
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
            UProductBundleSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UProductBundleSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            ComboBox cbo = sender as ComboBox;

            if (lkupItem != null)
            {
                lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                lkupItem.SelectedObject = null;
            }

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

            updateObject();
        }
    }
}
