using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UProductFinalDiscountSelection : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
        DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UProductFinalDiscountSelection),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UProductFinalDiscountSelection()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UProductFinalDiscountSelection control = obj as UProductFinalDiscountSelection;
            MPackageFinalDiscount d = (MPackageFinalDiscount)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UProductFinalDiscountSelection control, MPackageFinalDiscount v)
        {
            if (v == null)
            {
                return;
            }

            MPackageFinalDiscount o = new MPackageFinalDiscount(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            int selectionType = CUtil.StringToInt(o.SelectionType);

            control.cboSelectionType.SelectedIndex = selectionType - 1;

            MMasterRef mr = CUtil.MasterIDToObject(CMasterReference.Instance.ProductSpecificSelectionTypes, o.SelectionType);
            if (mr != null)
            {
                //control.cboSelectionType.SelectedItem = mr;

                if (mr.MasterID.Equals("1"))
                {
                    control.lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                    control.lkupItem.SelectedObject = o.ServiceObj;
                }
                else if (mr.MasterID.Equals("2"))
                {
                    control.lkupItem.Lookup = LookupSearchType2.InventoryItemLookup;
                    control.lkupItem.SelectedObject = o.ItemObj;
                }
                else
                {
                    control.lkupItem.Lookup = LookupSearchType2.ServiceLookup;
                    control.lkupItem.SelectedObject = o.ServiceObj;
                }
            }

            control.txtQuantity.Text = o.Quantity;

            v.ExtFlag = "I";
            if (v.PackageFinalDiscountID.Equals(""))
            {
                v.ExtFlag = "A";
            }
        }

        private void updateObject()
        {
            MPackageFinalDiscount v = (MPackageFinalDiscount)ProductSelected;
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
            }

            v.Quantity = txtQuantity.Text;
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UProductFinalDiscountSelection control = this;
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
            UProductFinalDiscountSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageFinalDiscount v = (MPackageFinalDiscount)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductFinalDiscountSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MPackageFinalDiscount v = (MPackageFinalDiscount)ProductSelected;
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
            UProductFinalDiscountSelection control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UProductFinalDiscountSelection control = this;
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

            MMasterRef mr = (MMasterRef) cbo.SelectedItem;
            if (mr == null)
            {
                return;
            }

            if (mr.MasterID.Equals("1"))
            {
                lkupItem.Lookup = LookupSearchType2.ServiceLookup;
            }
            else if (mr.MasterID.Equals("2"))
            {
                lkupItem.Lookup = LookupSearchType2.InventoryItemLookup;
            }

            lkupItem.SelectedObject = null;
            updateObject();
        }
    }
}
