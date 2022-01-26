using System;
using System.Windows;
using System.Windows.Controls;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UDatePicker : UserControl
    {
        public static readonly DependencyProperty SelectedDateProperty =
        DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(UDatePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedDateChanged));

        public event SelectionChangedEventHandler SelectedDateChanged;
        public event EventHandler OnChanged;

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        private static void OnSelectedDateChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UDatePicker ctrl = obj as UDatePicker;

            if (e.NewValue == null)
            {
                ctrl.dtDate.SelectedDate = null;
            }
            else
            {
                DateTime dt = (DateTime)e.NewValue;
                ctrl.dtDate.SelectedDate = dt;
            }
        }        

        public UDatePicker()
        {
            //DataContext = this;
            InitializeComponent();            
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            dtDate.SelectedDate = null;

            if (OnChanged != null)
            {
                OnChanged(sender, null);
            }
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker dp = sender as DatePicker;
            SelectedDate = dp.SelectedDate;

            if (OnChanged != null)
            {
                OnChanged(sender, null);
            }

            if (SelectedDateChanged != null)
            {
                SelectedDateChanged(this, null);
            }
        }   
    }
}
