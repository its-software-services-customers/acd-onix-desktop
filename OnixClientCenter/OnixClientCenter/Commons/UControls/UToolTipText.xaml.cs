using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UToolTipText : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register("ItemsSources", typeof(ObservableCollection<CToolTipItem>), typeof(UToolTipText),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits,
                OnItemsSourceChanged));

        public UToolTipText()
        {
            InitializeComponent();            
        }

        private static void InitGrid(Grid grd, ObservableCollection<CToolTipItem> items)
        {
            grd.Children.Clear();

            foreach (CToolTipItem it in items)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = GridLength.Auto;
                grd.RowDefinitions.Add(rd);
            }

            ColumnDefinition cd1 = new ColumnDefinition();
            cd1.Width = GridLength.Auto;
            grd.ColumnDefinitions.Add(cd1);

            ColumnDefinition cd2 = new ColumnDefinition();
            cd2.Width = GridLength.Auto;
            grd.ColumnDefinitions.Add(cd2);

            int r = 0;
            foreach (CToolTipItem it in items)
            {
                TextBlock label = new TextBlock();
                label.HorizontalAlignment = HorizontalAlignment.Left;
                label.VerticalAlignment = VerticalAlignment.Center;
                label.Text = it.Label;
                label.FontWeight = FontWeights.Bold;

                Grid.SetColumn(label, 0);
                Grid.SetRow(label, r);
                grd.Children.Add(label);


                TextBlock desc = new TextBlock();
                desc.HorizontalAlignment = HorizontalAlignment.Left;
                desc.VerticalAlignment = VerticalAlignment.Center;
                desc.Text = it.Description;
                desc.Margin = new Thickness(10, 0, 0, 0);

                Grid.SetColumn(desc, 1);
                Grid.SetRow(desc, r);
                grd.Children.Add(desc);

                r++;
            }
        }

        private static void OnItemsSourceChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UToolTipText ctrl = obj as UToolTipText;

            if (e.NewValue == null)
            {
                return;
            }

            ObservableCollection<CToolTipItem> items = (ObservableCollection<CToolTipItem>) e.NewValue;
            InitGrid(ctrl.grdMain, items);
        }

        public ObservableCollection<CToolTipItem> ItemsSources
        {
            get { return (ObservableCollection<CToolTipItem>) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}
