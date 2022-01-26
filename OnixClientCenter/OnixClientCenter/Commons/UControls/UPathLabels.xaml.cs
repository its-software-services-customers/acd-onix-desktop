using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Wis.WsClientAPI;
using Onix.ClientCenter.UI.HumanResource.OrgChart;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UPathLabels : UserControl
    {
        //private Boolean internalUpdate = false;

        public static readonly DependencyProperty ItemSourcesProperty =
        DependencyProperty.Register("ItemSources", typeof(ObservableCollection<MVOrgChart>), typeof(UPathLabels),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemSourcesEvtChanged));

        public static readonly DependencyProperty IsClickAbleProperty =
        DependencyProperty.Register("IsClickAble", typeof(Boolean), typeof(UPathLabels),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsClickAbleEvtChanged));

        //public event SelectionChangedEventHandler DirectoryClicked;
        public event RoutedEventHandler DirectoryClicked;

        public UPathLabels()
        {
            InitializeComponent();            
        }

        #region ClickAble
        public Boolean IsClickAble
        {
            get { return (Boolean)GetValue(IsClickAbleProperty); }
            set { SetValue(IsClickAbleProperty, value); }
        }

        private static void OnIsClickAbleEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }
        #endregion

        #region ItemSources
        public ObservableCollection<MVOrgChart> ItemSources
        {
            get { return (ObservableCollection<MVOrgChart>)GetValue(ItemSourcesProperty); }
            set { SetValue(ItemSourcesProperty, value); }
        }

        private static void OnSelectedItemSourcesEvtChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            UPathLabels ctrl = obj as UPathLabels;
            populateItems(ctrl);
        }
        #endregion

        private static void populateItems(UPathLabels ctrl)
        {
            ctrl.pnlMain.Children.Clear();

            ObservableCollection<MVOrgChart> arr = new ObservableCollection<MVOrgChart>(ctrl.ItemSources);

            MVOrgChart root = new MVOrgChart(new CTable(""));
            root.DirectoryID = "";
            root.ParentDirectoryID = "";
            root.DirectoryName = "/";
            arr.Insert(0, root);

            int i = 0;
            foreach (MVOrgChart obj in arr)
            {
                i++;

                Border bdr = new Border();
                bdr.BorderThickness = new Thickness(1);
                bdr.BorderBrush = Brushes.Blue;
                bdr.CornerRadius = new CornerRadius(2);
                bdr.Padding = new Thickness(10, 5, 10, 5);

                if (i == 1)
                {
                    bdr.Margin = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    bdr.Margin = new Thickness(5, 0, 0, 0);
                }

                TextBlock tb = new TextBlock();
                tb.Foreground = Brushes.Black;
                tb.Text = obj.DirectoryName;
                if (i != arr.Count)
                {
                    if (ctrl.IsClickAble)
                    {
                        tb.Foreground = Brushes.Blue;
                        tb.MouseDown += new MouseButtonEventHandler(ctrl.TextBlock_MouseDown);
                        tb.MouseEnter += new MouseEventHandler(ctrl.TextBlock_MouseEnter);
                        tb.MouseLeave += new MouseEventHandler(ctrl.TextBlock_MouseLeave);
                    }
                }

                tb.Tag = obj;

                bdr.Child = tb;

                ctrl.pnlMain.Children.Add(bdr);               
            }
        }

        private void RootElement_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        private void TextBlock_MouseEnter(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }

        private void TextBlock_MouseLeave(object sender, MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DirectoryClicked != null)
            {
                DirectoryClicked(sender, e);
            }
        }
    }
}
