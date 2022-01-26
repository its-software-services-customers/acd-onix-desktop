using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UTimePicker : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register("SelectedTime", typeof(DateTime?), typeof(UTimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedTimeChanged));

        private Boolean txtChanged = false;

        public DateTime? SelectedTime
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public event EventHandler OnChanged;

        public UTimePicker()
        {
            //DataContext = this;
            InitializeComponent();            
        }

        private static void updateGui(DateTime t, UTimePicker ctrl)
        {
            if (t == null)
            {
                ctrl.txtHH.Text = "";
                ctrl.txtMM.Text = "";
            }
            else
            {
                String dtm = CUtil.DateTimeToDateStringInternal(t);
                //YYYY/MM/DD HH:MM:SS

                ctrl.txtHH.Text = dtm.Substring(11, 2);
                ctrl.txtMM.Text = dtm.Substring(14, 2);
            }
        }

        private static void OnSelectedTimeChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimePicker ctrl = obj as UTimePicker;

            updateGui((DateTime) e.NewValue, ctrl);
            ctrl.txtChanged = false;
        }

        private void txtHH_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private DateTime constructTime()
        {
            int hh = CUtil.StringToInt(txtHH.Text);
            int mm = CUtil.StringToInt(txtMM.Text);
            int ss = 0;

            if (hh > 23)
            {
                hh = 23;
            }

            if (mm > 59)
            {
                mm = 59;
            }

            String curr = CUtil.DateTimeToDateStringInternal(DateTime.Now);

            String intDate = String.Format("{0} {1:00}:{2:00}:{3:00}", curr.Substring(0, 10), hh, mm, ss);
            DateTime dtm = CUtil.InternalDateToDate(intDate);

            return (dtm);
        }

        private void txtHH_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtChanged = true;

            if (OnChanged != null)
            {
                OnChanged(this, null);
            }
        }

        private void RootElement_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!txtChanged)
            {
                return;
            }
            
            if (OnChanged != null)
            {
                OnChanged(this, null);
            }

            DateTime dtm = constructTime();
            SelectedTime = dtm;

            txtChanged = false;
        }
    }
}
