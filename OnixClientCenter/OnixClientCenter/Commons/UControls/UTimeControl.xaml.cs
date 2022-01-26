using System;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.UControls
{
    public enum IntervalTypeEnum
    {
        DAY_ENTIRE = 1,
        DAY_INTERVAL,
    }

    public partial class UTimeControl : UserControl
    {
        public static readonly DependencyProperty DayProperty =
        DependencyProperty.Register("Day", typeof(DayEnum), typeof(UTimeControl),
            new UIPropertyMetadata(DayEnum.MONDAY_NONE, new PropertyChangedCallback(OnDayChanged)));

        public static readonly DependencyProperty DayIntervalProperty =
        DependencyProperty.Register("DayInterval", typeof(IntervalTypeEnum), typeof(UTimeControl),
            new UIPropertyMetadata(IntervalTypeEnum.DAY_ENTIRE, new PropertyChangedCallback(OnDayIntervalChanged)));

        public static readonly DependencyProperty FromHourProperty =
        DependencyProperty.Register("FromHour", typeof(String), typeof(UTimeControl),
            new UIPropertyMetadata("00", new PropertyChangedCallback(OnFromHourChanged)), ValidateHourValue);

        public static readonly DependencyProperty ToHourProperty =
        DependencyProperty.Register("ToHour", typeof(String), typeof(UTimeControl),
            new UIPropertyMetadata("00", new PropertyChangedCallback(OnToHourChanged)), ValidateHourValue);

        public static readonly DependencyProperty FromMinuteProperty =
        DependencyProperty.Register("FromMinute", typeof(String), typeof(UTimeControl),
            new UIPropertyMetadata("01", new PropertyChangedCallback(OnFromMinuteChanged)));

        public static readonly DependencyProperty ToMinuteProperty =
        DependencyProperty.Register("ToMinute", typeof(String), typeof(UTimeControl),
            new UIPropertyMetadata("01", new PropertyChangedCallback(OnToMinuteChanged)));

        public static readonly DependencyProperty SelectedProperty =
        DependencyProperty.Register("Selected", typeof(Boolean), typeof(UTimeControl),
            new UIPropertyMetadata(true, new PropertyChangedCallback(OnSelectedChanged)));

        public event EventHandler OnChanged;

        public UTimeControl()
        {
            //DataContext = this;
            InitializeComponent();            
        }

        private static bool ValidateHourValue(object value)
        {
            //String hh = (String) value;

            //int hour = 0;
            //if (int.TryParse(hh, out hour))
            //{
            //    if ((hour >= 0) && (hour <= 23))
            //    {
            //        return (true);
            //    }
            //}

            return (true);
        }

        private static void OnSelectedChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        public Boolean Selected
        {
            get { return (Boolean)GetValue(SelectedProperty); }
            set { SetValue(SelectedProperty, value); }
        }

        private static void OnToMinuteChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        public String ToMinute
        {
            get { return (String)GetValue(ToMinuteProperty); }
            set { SetValue(ToMinuteProperty, value); }
        }

        private static void OnFromMinuteChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        public String FromMinute
        {
            get { return (String)GetValue(FromMinuteProperty); }
            set { SetValue(FromMinuteProperty, value); }
        }

        private static void OnToHourChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        public String ToHour
        {
            get { return (String)GetValue(ToHourProperty); }
            set { SetValue(ToHourProperty, value); }
        }

        private static void OnFromHourChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        public String FromHour
        {
            get { return (String)GetValue(FromHourProperty); }

            set
            {
                SetValue(FromHourProperty, value);
            }
        }

        public DayEnum Day
        {
            get { return (DayEnum) GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        private static void OnDayChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            DayEnum d = (DayEnum)e.NewValue;
            control.cbxDay.Content = CUtil.DayEnumToString(d);
            control.grbBox.Header = CUtil.DayEnumToString(d);
        }

        public IntervalTypeEnum DayInterval
        {
            get { return (IntervalTypeEnum)GetValue(DayIntervalProperty); }
            set { SetValue(DayIntervalProperty, value); }
        }

        private static void OnDayIntervalChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UTimeControl control = obj as UTimeControl;
            IntervalTypeEnum di = (IntervalTypeEnum)e.NewValue;

            if (di == IntervalTypeEnum.DAY_ENTIRE)
            {
                control.radAll.IsChecked = true;
            }
            else
            {
                control.radInterval.IsChecked = true;
            }

            if (control.OnChanged != null)
            {
                control.OnChanged(control, null);
            }
        }

        private void radAll_Checked(object sender, RoutedEventArgs e)
        {
            DayInterval = IntervalTypeEnum.DAY_ENTIRE;
        }

        private void radInterval_Checked(object sender, RoutedEventArgs e)
        {
            DayInterval = IntervalTypeEnum.DAY_INTERVAL;
        }

        private bool isTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void txtText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox t = (sender as TextBox);
            String nm = t.Name;
            String txt = e.Text;

            Boolean flag = isTextAllowed(txt);
            e.Handled = !flag;
        }
    }
}
