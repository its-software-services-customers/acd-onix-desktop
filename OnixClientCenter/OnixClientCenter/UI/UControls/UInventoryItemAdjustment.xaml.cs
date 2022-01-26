using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Model;

namespace Onix.ClientCenter.UControls
{
    public partial class UInventoryItemAdjustment : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
        DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UInventoryItemAdjustment),
            new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UInventoryItemAdjustment()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UInventoryItemAdjustment control = obj as UInventoryItemAdjustment;
            MInventoryAdjustment d = (MInventoryAdjustment)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UInventoryItemAdjustment control, MInventoryAdjustment v)
        {
            if ((v == null) || (control == null))
            {
                return;
            }

            MInventoryAdjustment o = new MInventoryAdjustment(v.GetDbObject().Clone());

            control.lkupItem.SelectedObject = o.ItemObj;
            control.txtQuantity.Text = o.AdjQuantity;
            control.txtAmount.Text = o.AdjAmount;
        }

        private void updateObject()
        {
            MInventoryAdjustment v = (MInventoryAdjustment) ProductSelected;
            if (v == null)
            {
                return;
            }

            v.ItemObj = null;

            if (lkupItem.SelectedObject != null)
            {
                //if (lkupItem.Lookup == LookupSearchType.InventoryItemLookup)
                //{
                //    v.ItemObj = lkupItem.SelectedObject;
                //}
            }

            v.AdjQuantity = txtQuantity.Text;
            v.AdjAmount = txtAmount.Text;
            v.ItemObj = lkupItem.SelectedObject;
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UInventoryItemAdjustment control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            if (lkupItem != null)
            {
                updateObject();
            }
        }

        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void txtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            UInventoryItemAdjustment control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }

        private void txtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UInventoryItemAdjustment control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }
    }
}
