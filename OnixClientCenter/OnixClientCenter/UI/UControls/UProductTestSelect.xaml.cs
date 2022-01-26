using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.UControls
{
    public partial class UProductTestSelect : UserControl
    {
        public static readonly DependencyProperty ProductSelectedProperty =
       DependencyProperty.Register("ProductSelected", typeof(MBaseModel), typeof(UProductTestSelect),
           new UIPropertyMetadata(null, new PropertyChangedCallback(OnProductSelectedChanged)));

        public event EventHandler OnChanged;

        public UProductTestSelect()
        {
            InitializeComponent();
        }

        private static void OnProductSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UProductTestSelect control = obj as UProductTestSelect;
            MSelectedItem d = (MSelectedItem)e.NewValue;
            updateGui(control, d);
        }

        public MBaseModel ProductSelected
        {
            get { return (MBaseModel)GetValue(ProductSelectedProperty); }
            set { SetValue(ProductSelectedProperty, value); }
        }

        private static void updateGui(UProductTestSelect control, MSelectedItem v)
        {
            if (v == null)
            {
                return;
            }

            MSelectedItem o = new MSelectedItem(v.GetDbObject().Clone());

            control.cbxEnable.IsChecked = o.EnabledFlag.Equals("Y");
            control.cbxTray.IsChecked = o.TrayFlag.Equals("Y");
            int selectionType = CUtil.StringToInt(o.SelectionType);

            control.cboSelectionType.SelectedIndex = selectionType - 1;

            if (o.SelectionType.Equals("1"))
            {
                control.lkupItem.SelectedObject = o.ServiceObj;
            }
            else if (o.SelectionType.Equals("2"))
            {
                control.lkupItem.SelectedObject = o.ItemObj;
            }
            else
            {
                control.lkupItem.SelectedObject = o.ServiceObj;
            }

            control.txtValue.Text = o.ItemQuantity;
        }

        private void updateObject()
        {
            MSelectedItem v = (MSelectedItem)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.ServiceObj = null;
            v.ItemObj = null;

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

            v.ItemQuantity = txtValue.Text;
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {
            UProductTestSelect control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MSelectedItem v = (MSelectedItem)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "Y";
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductTestSelect control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MSelectedItem v = (MSelectedItem)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.EnabledFlag = "N";
        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            UProductTestSelect control = this;
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

        private void txtValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            UProductTestSelect control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            updateObject();
        }

        private void cboSelectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UProductTestSelect control = this;
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

            lkupItem.SelectedObject = null;

            updateObject();
        }

        private void cbxTray_Checked(object sender, RoutedEventArgs e)
        {
            UProductTestSelect control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MSelectedItem v = (MSelectedItem)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.TrayFlag = "Y";
        }

        private void cbxTray_Unchecked(object sender, RoutedEventArgs e)
        {
            UProductTestSelect control = this;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }

            MSelectedItem v = (MSelectedItem)ProductSelected;
            if (v == null)
            {
                return;
            }

            v.TrayFlag = "N";
        }
    }
}
