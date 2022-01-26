using Onix.Client.Helper;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Onix.ClientCenter.UI.AccountPayable.StockCost
{
    public partial class UCostDistribution : UserControl
    {
        public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register("Caption", typeof(String), typeof(UCostDistribution),
            new FrameworkPropertyMetadata("", OnSelectedCaptionEvtChanged));

        public static readonly DependencyProperty EnabledProperty =
        DependencyProperty.Register("Enabled", typeof(bool), typeof(UCostDistribution),
            new FrameworkPropertyMetadata(true, OnSelectedEnabledEvtChanged));

        public static readonly DependencyProperty IsForLabelProperty =
        DependencyProperty.Register("IsForLabel", typeof(bool), typeof(UCostDistribution),
            new FrameworkPropertyMetadata(false, OnSelectedIsForLabelEvtChanged));

        public static readonly DependencyProperty ItemObjProperty =
        DependencyProperty.Register("ItemObj", typeof(MVStockCostDocumentItem), typeof(UCostDistribution),
            new FrameworkPropertyMetadata(null, OnSelectedItemObjEvtChanged));

        public event EventHandler TextChanged;

        #region ItemObj
        public MVStockCostDocumentItem ItemObj
        {
            get { return (MVStockCostDocumentItem)GetValue(ItemObjProperty); }
            set { SetValue(ItemObjProperty, value); }
        }

        private static String format(UCostDistribution ctrl, String number)
        {
            if (ctrl.Enabled)
            {
                return (number);
            }

            return (CUtil.FormatNumber(number));
        }

        private static void updateGui(UCostDistribution ctrl)
        {
            ctrl.txtJan.Text = format(ctrl, ctrl.ItemObj.JanAmount);
            ctrl.txtFeb.Text = format(ctrl, ctrl.ItemObj.FebAmount);
            ctrl.txtMar.Text = format(ctrl, ctrl.ItemObj.MarAmount);
            ctrl.txtApr.Text = format(ctrl, ctrl.ItemObj.AprAmount);
            ctrl.txtMay.Text = format(ctrl, ctrl.ItemObj.MayAmount);
            ctrl.txtJun.Text = format(ctrl, ctrl.ItemObj.JunAmount);
            ctrl.txtJul.Text = format(ctrl, ctrl.ItemObj.JulAmount);
            ctrl.txtAug.Text = format(ctrl, ctrl.ItemObj.AugAmount);
            ctrl.txtSep.Text = format(ctrl, ctrl.ItemObj.SepAmount);
            ctrl.txtOct.Text = format(ctrl, ctrl.ItemObj.OctAmount);
            ctrl.txtNov.Text = format(ctrl, ctrl.ItemObj.NovAmount);
            ctrl.txtDec.Text = format(ctrl, ctrl.ItemObj.DecAmount);
            ctrl.txtTot.Text = format(ctrl, ctrl.ItemObj.TotAmount);
        }

        private static void OnSelectedItemObjEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UCostDistribution ctrl = obj as UCostDistribution;
            updateGui(ctrl);
        }
        #endregion

        #region IsForLabel
        public Boolean IsForLabel
        {
            get { return (Boolean)GetValue(IsForLabelProperty); }
            set { SetValue(IsForLabelProperty, value); }
        }

        private static void updateMothLabel(UCostDistribution ctrl)
        {
            ctrl.txtJan.Text = "มกราคม";
            ctrl.txtJan.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtFeb.Text = "กุมภาพันธ์";
            ctrl.txtFeb.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtMar.Text = "มีนาคม";
            ctrl.txtMar.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtApr.Text = "เมษายน";
            ctrl.txtApr.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtMay.Text = "พฤษภาคม";
            ctrl.txtMay.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtJun.Text = "มิถุนายน";
            ctrl.txtJun.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtJul.Text = "กรกฎาคม";
            ctrl.txtJul.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtAug.Text = "สิงหาคม";
            ctrl.txtAug.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtSep.Text = "กันยายน";
            ctrl.txtSep.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtOct.Text = "ตุลาคม";
            ctrl.txtOct.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtNov.Text = "พฤศจิกายน";
            ctrl.txtNov.HorizontalContentAlignment = HorizontalAlignment.Center;

            ctrl.txtDec.Text = "ธันวาคม";
            ctrl.txtDec.HorizontalContentAlignment = HorizontalAlignment.Center;
        }

        private static void OnSelectedIsForLabelEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UCostDistribution ctrl = obj as UCostDistribution;
            updateMothLabel(ctrl);
        }
        #endregion

        #region IsEnabled
        public Boolean Enabled
        {
            get { return (Boolean)GetValue(EnabledProperty); }
            set { SetValue(EnabledProperty, value); }
        }

        private static void disableTextBoxes(UCostDistribution ctrl)
        {
            ctrl.txtJan.IsEnabled = ctrl.Enabled;
            ctrl.txtFeb.IsEnabled = ctrl.Enabled;
            ctrl.txtMar.IsEnabled = ctrl.Enabled;
            ctrl.txtApr.IsEnabled = ctrl.Enabled;
            ctrl.txtMay.IsEnabled = ctrl.Enabled;
            ctrl.txtJun.IsEnabled = ctrl.Enabled;
            ctrl.txtJul.IsEnabled = ctrl.Enabled;
            ctrl.txtAug.IsEnabled = ctrl.Enabled;
            ctrl.txtSep.IsEnabled = ctrl.Enabled;
            ctrl.txtOct.IsEnabled = ctrl.Enabled;
            ctrl.txtNov.IsEnabled = ctrl.Enabled;
            ctrl.txtDec.IsEnabled = ctrl.Enabled;
        }

        private static void OnSelectedEnabledEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UCostDistribution ctrl = obj as UCostDistribution;
            disableTextBoxes(ctrl);
        }
        #endregion

        #region Caption
        public String Caption
        {
            get { return (String)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        private static void OnSelectedCaptionEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UCostDistribution ctrl = obj as UCostDistribution;
            ctrl.txtCaption.Text = ctrl.Caption;
        }
        #endregion

        public UCostDistribution()
        {
            InitializeComponent();
        }

        private void TxtJan_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox) sender;

            if (item == null)
            {
                return;
            }

            item.JanAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtFeb_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.FebAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtMar_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.MarAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtApr_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.AprAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtMay_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.MayAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtJun_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.JunAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtJul_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.JulAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtAug_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.AugAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtSep_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.SepAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtOct_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.OctAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtNov_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.NovAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void TxtDec_TextChanged(object sender, TextChangedEventArgs e)
        {
            MVStockCostDocumentItem item = ItemObj;
            TextBox tb = (TextBox)sender;

            if (item == null)
            {
                return;
            }

            item.DecAmount = tb.Text;
            updateTotal();
            TextChanged?.Invoke(sender, e);
        }

        private void updateTotal()
        {
            double jan = CUtil.StringToDouble(txtJan.Text);
            double feb = CUtil.StringToDouble(txtFeb.Text);
            double mar = CUtil.StringToDouble(txtMar.Text);
            double apr = CUtil.StringToDouble(txtApr.Text);
            double may = CUtil.StringToDouble(txtMay.Text);
            double jun = CUtil.StringToDouble(txtJun.Text);
            double jul = CUtil.StringToDouble(txtJul.Text);
            double aug = CUtil.StringToDouble(txtAug.Text);
            double sep = CUtil.StringToDouble(txtSep.Text);
            double oct = CUtil.StringToDouble(txtOct.Text);
            double nov = CUtil.StringToDouble(txtNov.Text);
            double dec = CUtil.StringToDouble(txtDec.Text);

            double tot = jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + dec;
            ItemObj.TotAmount = tot.ToString();

            txtTot.Text = CUtil.FormatNumber(tot.ToString());
        }
    }
}
