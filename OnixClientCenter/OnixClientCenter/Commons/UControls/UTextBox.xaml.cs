using System;
using System.Windows;
using System.Windows.Controls;
using Onix.ClientCenter.Windows;
using System.Windows.Input;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.Commons.UControls
{
    public enum TextSearchNameSpace
    {
        CustomerCodeNS = 1,
        CustomerNameNS = 2,
        SupplierCodeNS = 3,
        SupplierNameNS = 4,
        MasterRefCodeNS = 5,
        MasterRefDescNS = 6,
        ServiceCodeNS = 7,
        ServiceNameNS = 8,
        ItemCodeNS = 9,
        ItemNameThaiNS = 10,
        ItemNameEngNS = 11,
        ProjectCodeNS = 12,
        ProjectNameNS = 13,
        LocationCodeNS = 14,
        LocationNameNS = 15,
        EmployeeCodeNS = 16,
        EmployeeNameNS = 17,
        CashAccountCodeNS = 18,
        CashAccountNameNS = 19,
        CashAccountBranchNS = 20,
        ItemCategoryPathNS = 21,
        PromotionCodeNS = 22,
        PromotionNameNS = 23,
        ServiceCodeSaleNS = 24,
        ServiceCodePurchaseNS = 25,
        ServiceCodeRegularSaleNS = 26,
        ServiceCodeRegularPurchaseNS = 27,
        ServiceCodeOtherSaleNS = 28,
        ServiceCodeOtherPurchaseNS = 29,
        ArChequeNS = 30,
        ApChequeNS = 31,
        MicroServiceCodeNS = 32,
        ItemCodeBorrowReturnNS = 33,
        EmployeeDailyCodeNS = 34,
        EmployeeMonthlyCodeNS = 35,
    }

    public partial class UTextBox : UserControl
    {
        public static readonly DependencyProperty ShowDetailProperty =
        DependencyProperty.Register("ShowDetail", typeof(Boolean), typeof(UTextBox),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnShowDetailPropertyChanged)));

        public static readonly DependencyProperty TextSearchNameSpaceProperty =
        DependencyProperty.Register("TextSearchNameSpace", typeof(TextSearchNameSpace), typeof(UTextBox),
            new FrameworkPropertyMetadata(TextSearchNameSpace.CustomerCodeNS, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnTextSearchNameSpacePropertyChanged)));

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(String), typeof(UTextBox),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnTextPropertyChanged)));

        public static readonly DependencyProperty SideProperty =
        DependencyProperty.Register("Side", typeof(String), typeof(UTextBox),
            new FrameworkPropertyMetadata("B", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSidePropertyChanged)));

        public static readonly DependencyProperty MasterRefTypeProperty =
        DependencyProperty.Register("MasterRefType", typeof(MasterRefEnum), typeof(UTextBox),
            new FrameworkPropertyMetadata(MasterRefEnum.MASTER_REF_UNDEF, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnMasterRefTypePropertyChanged)));

        public static readonly DependencyProperty ExtraParamProperty =
        DependencyProperty.Register("ExtraParam", typeof(MBaseModel), typeof(UTextBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnExtraParamPropertyChanged)));

        private WinTextIntellisense win = null;
        private Boolean internalCall = false;
        private String side = "B";
        private MasterRefEnum mrType = MasterRefEnum.MASTER_REF_UNDEF;
        private Boolean isShowDetail = false;
        private MBaseModel extraParam = null;

        public event EventHandler TextChanged;
        public event EventHandler TextSelected;

        #region Dependency Property

        public TextSearchNameSpace TextSearchNameSpace
        {
            get { return (TextSearchNameSpace) GetValue(TextSearchNameSpaceProperty); }
            set { SetValue(TextSearchNameSpaceProperty, value); }
        }

        public MBaseModel ExtraParam
        {
            get { return (MBaseModel)GetValue(ExtraParamProperty); }
            set { SetValue(ExtraParamProperty, value); }
        }

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public String Side
        {
            get { return (String)GetValue(SideProperty); }
            set { SetValue(SideProperty, value); }
        }

        public MasterRefEnum MasterRefType
        {
            get { return (MasterRefEnum)GetValue(MasterRefTypeProperty); }
            set { SetValue(MasterRefTypeProperty, value); }
        }

        public Boolean ShowDetail
        {
            get { return (Boolean)GetValue(ShowDetailProperty); }
            set { SetValue(ShowDetailProperty, value); }
        }

        #endregion

        #region Event Handler

        private static void OnTextSearchNameSpacePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            control.TextSearchNameSpace = (TextSearchNameSpace)e.NewValue;
        }

        private static void OnShowDetailPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            control.isShowDetail = (Boolean)e.NewValue;
        }

        private static void OnSidePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            control.side = (String)e.NewValue;
        }

        private static void OnMasterRefTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            control.mrType = (MasterRefEnum)e.NewValue;
        }

        private static void OnExtraParamPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            control.extraParam = (MBaseModel) e.NewValue;
        }

        private static void OnTextPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UTextBox control = sender as UTextBox;
            String s = (String)e.NewValue;

            control.internalCall = true;
            control.txtGeneric.Text = s;
            control.internalCall = false;
        }

        #endregion


        public UTextBox()
        {
            InitializeComponent();
        }

        private void showIntellisense()
        {
        }

        public void CloseIntellisense()
        {
            if (win == null)
            {
                return;
            }

            String txt = win.GetCurrentText();

            win.Close();
            win = null;

            if (txt.Equals(""))
            {
                return;
            }

            internalCall = true;
            txtGeneric.Text = txt;
            txtGeneric.CaretIndex = txt.Length;

            Text = txt;
            if (TextChanged != null)
            {
                TextChanged(null, null);
            }

            if (TextSelected != null)
            {
                TextSelected(this, null);
            }

            internalCall = false;
        }

        private void txtGeneric_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (internalCall)
            {
                return;
            }

            String txt = txtGeneric.Text;
            if (!txt.Equals("") && (win == null))
            {
                win = new WinTextIntellisense(side, txtGeneric, Window.GetWindow(this), TextSearchNameSpace, mrType, isShowDetail, this, extraParam);
                win.Show();
                txtGeneric.Focus();
            }
            
            if (win != null)
            {
                win.RegisterText(txt);
            }

            Text = txt;

            if (TextChanged != null)
            {
                TextChanged(sender, e);
            }            
        }

        private void txtGeneric_LostFocus(object sender, RoutedEventArgs e)
        {
            if (win != null)
            {
                win.Close();
                win = null;
            }
        }

        private void txtGeneric_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (win != null)
                {
                    e.Handled = true;
                    String txt = win.GetCurrentText();

                    win.Close();
                    win = null;

                    if (!txt.Equals(""))
                    {
                        internalCall = true;
                        txtGeneric.Text = txt;
                        txtGeneric.CaretIndex = txt.Length;

                        Text = txt;
                        if (TextChanged != null)
                        {
                            TextChanged(sender, e);
                        }

                        if (TextSelected != null)
                        {
                            TextSelected(this, e);
                        }

                        internalCall = false;
                    }
                }
                else
                {
                    txtGeneric.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
            else if (e.Key == Key.Down)
            {                
                if (win != null)
                {
                    e.Handled = true;
                    win.MoveDown();
                }
            }
            else if (e.Key == Key.Up)
            {                
                if (win != null)
                {
                    e.Handled = true;
                    win.MoveUp();
                }
            }
            else if (e.Key == Key.Escape)
            {
                if (win != null)
                {
                    e.Handled = true;
                    win.Close();
                    win = null;
                }
            }
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            //txtGeneric.Focus();
        }

        private void rootElement_GotFocus(object sender, RoutedEventArgs e)
        {
            txtGeneric.Focus();
        }

        public void SetFocus()
        {
            txtGeneric.Focus();
        }
    }
}
