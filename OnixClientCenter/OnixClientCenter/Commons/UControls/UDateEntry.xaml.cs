using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UDateEntry : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(UDateEntry),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedDateEvtChanged));

        public static readonly DependencyProperty OnlyMonthProperty =
        DependencyProperty.Register("OnlyMonth", typeof(Boolean), typeof(UDateEntry),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnOnlyMonthEvtChanged));

        public static readonly DependencyProperty ButtonVisibleProperty =
        DependencyProperty.Register("ButtonVisible", typeof(Boolean), typeof(UDateEntry),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnButtonVisibleEvtChanged));

        private Boolean internalCall = false;
        private Boolean needEventProcess = true;

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public Boolean ButtonVisible
        {
            get { return (Boolean)GetValue(ButtonVisibleProperty); }
            set { SetValue(ButtonVisibleProperty, value); }
        }

        public Boolean OnlyMonth
        {
            get { return (Boolean)GetValue(OnlyMonthProperty); }
            set { SetValue(OnlyMonthProperty, value); }
        }

        private static void OnButtonVisibleEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UDateEntry ctrl = obj as UDateEntry;
            Boolean buttonVisible = (Boolean)e.NewValue;

            if (!buttonVisible)
            {
                ctrl.cmdClear.Visibility = Visibility.Collapsed;
            }
            else
            {
                ctrl.cmdClear.Visibility = Visibility.Visible;
            }
        }

        private static void OnOnlyMonthEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UDateEntry ctrl = obj as UDateEntry;
            Boolean onlyMonth = (Boolean) e.NewValue;

            if (onlyMonth)
            {
                ctrl.txtDD.Visibility = Visibility.Collapsed;
                ctrl.lblSlash1.Visibility = Visibility.Collapsed;
            }
        }

        private static void dateToText(UDateEntry ctrl, DateTime? dt)
        {
            if (ctrl.internalCall)
            {
                return;
            }

            if (dt == null)
            {
                ctrl.needEventProcess = false;
                ctrl.txtDD.Text = "";
                ctrl.txtMM.Text = "";
                ctrl.txtYY2.Text = "";
                ctrl.needEventProcess = true;

                return;
            }

            int d = ((DateTime) dt).Day;
            int m = ((DateTime)dt).Month;
            int y = ((DateTime)dt).Year;

            ctrl.needEventProcess = false;

            ctrl.txtDD.Text = d.ToString("D2");
            ctrl.txtMM.Text = m.ToString("D2");
            ctrl.txtYY2.Text = y.ToString("D4").Substring(2, 2);

            ctrl.needEventProcess = true;
        }

        private static void OnSelectedDateEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {            
            UDateEntry ctrl = obj as UDateEntry;
            dateToText(ctrl, (DateTime?) e.NewValue);
        }

        public event SelectionChangedEventHandler SelectedDateChanged;

        public UDateEntry()
        {
            InitializeComponent();            
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private DateTime? parseDate()
        {
            DateTime? myDate = null;

            if (txtDD.Text.Equals("") || txtMM.Text.Equals("") || txtYY2.Text.Equals(""))
            {
                return (null);
            }

            String dd = CUtil.StringToInt(txtDD.Text).ToString("D2");
            String mm = CUtil.StringToInt(txtMM.Text).ToString("D2");
            String yyyy = "20" + CUtil.StringToInt(txtYY2.Text).ToString("D2");

            String intDate = String.Format("{0}/{1}/{2}", dd, mm, yyyy);

            try
            {
                myDate = DateTime.ParseExact(intDate.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
            }

            return (myDate);
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!needEventProcess)
            {
                return;
            }

            DateTime? dt = parseDate();

            if (dt == null)
            {
                bdrBorder.BorderBrush = new SolidColorBrush(Colors.Red);
            }
            else
            {
                bdrBorder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);

                internalCall = true;
                SelectedDate = dt;
                internalCall = false;

                if (SelectedDateChanged != null)
                {
                    SelectedDateChanged(this, null);
                }
            }
        }

        private void RootElement_LostFocus(object sender, RoutedEventArgs e)
        {
            DateTime? dt = parseDate();
            if ((dt == null) && !ButtonVisible)
            {
                SelectedDate = DateTime.Now;
                bdrBorder.BorderBrush = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = null;
        }
    }
}
