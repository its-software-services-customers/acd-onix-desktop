using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using Wis.WsClientAPI;
using System.Text.RegularExpressions;
using Onix.ClientCenter.Windows;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter
{
    public partial class WinAddEditPackage : Window
    {
        private MPackage vw = null;
        private MPackage actualView = null;

        private MPackagePrice currentViewObj = null;

        private ObservableCollection<MBaseModel> parentItemsSource = null;
        private PackageType pt = PackageType.PackageItem;
        private RoutedEventHandler itemAddedHandler = null;

        //private Boolean isActivated = false;
        private String pg = "";
        private Boolean isOK = false;
        public Boolean DialogOK = false;
        private Boolean isDoneRender = false;

        public String Caption = "";
        public String Mode = "";

        public WinAddEditPackage(String group)
        {
            pg = group;
            InitializeComponent();
        }

        public WinAddEditPackage(String group, String mode)
        {
            pg = group;
            Mode = mode;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public Boolean IsOK
        {
            get
            {
                return (isOK);
            }
        }

        public Boolean IsEditableMode
        {
            get
            {
                return (!Mode.Equals("V"));
            }
        }

        public RoutedEventHandler ItemAddedHandler
        {
            get
            {
                return (itemAddedHandler);
            }

            set
            {
                itemAddedHandler = value;
            }
        }

        public Visibility PricingVisibility
        {
            get
            {
                if (pg.Equals("1"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility BonusVisibility
        {
            get
            {
                if (pg.Equals("2"))
                {
                    return (Visibility.Visible);
                }
                else if (pg.Equals("9"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility BundleVisibility
        {
            get
            {
                if (pg.Equals("5"))
                {
                    return (Visibility.Visible);
                }
                else if (pg.Equals("10"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility FinalDiscountVisibility
        {
            get
            {
                if (pg.Equals("6"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility PostGiftVisibility
        {
            get
            {
                if (pg.Equals("7"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility DiscountVisibility
        {
            get
            {
                if (pg.Equals("3"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility VoucherVisibility
        {
            get
            {
                if (pg.Equals("4"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility TrayVisibility
        {
            get
            {
                if (pg.Equals("8"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public PackageType PackageType
        {
            get
            {
                return (pt);
            }

            set
            {
                pt = value;
            }
        }

        public MPackage ViewData
        {
            set
            {
                actualView = value;
            }
        }

        public ObservableCollection<MBaseModel> ParentItemSource
        {
            set
            {
                parentItemsSource = value;
            }
        }

        private Boolean validateCustomers(int tabIdx)
        {
            int idx = 0;
            if (vw.IsCustomerSpecific == false)
            {
                //No need to validate items
                return (true);
            }

            ObservableCollection<MPackageCustomer> custs = vw.PackageCustomers;
            foreach (MPackageCustomer c in custs)
            {
                idx++;

                if (c.EnabledFlag.Equals("N"))
                {
                    continue;
                }

                if (c.SelectionType.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    TabMain.SelectedIndex = tabIdx;
                    return (false);
                }

                if (c.IsItemEmpty)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_CUSTOMER_SELECT", null);
                    TabMain.SelectedIndex = tabIdx;
                    return (false);
                }
            }

            return (true);
        }

        private Boolean validatePackageItems<T>(ObservableCollection<T> collection, TabItem titem, Boolean chkCnt) where T : MBaseModel
        {
            Hashtable hash = new Hashtable();

            int idx = 0;
            int cnt = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;

                if (!c.IsEnabled)
                {
                    continue;
                }

                if (c.IsEmpty)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;
                    return (false);
                }

                cnt++;

                if (hash.ContainsKey(c.ID))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_DUPLICATED", null);
                    titem.IsSelected = true;
                    return (false);
                }

                hash.Add(c.ID, c);
            }

            if ((cnt <= 0) && chkCnt)
            {
                CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_COUNT", null);
                titem.IsSelected = true;
                return (false);
            }

            return (true);
        }

        private Boolean validateItems()
        {
            Boolean result = false;

            result = validateCustomers(2);
            if (!result)
            {
                return (result);
            }

            if (vw.PackageType.Equals("1"))
            {
                //Pricing
                result = validatePackageItems(vw.PackageItemPrices, tbiPriceOfItem, true);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("2"))
            {
                //Bonus
                result = validatePackageItems(vw.PackageBonusBuy, tbiBonus, true);
                if (!result)
                {
                    return (result);
                }

                result = validatePackageItems(vw.PackageBonusFree, tbiBonus, true);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("3"))
            {
                //Item Discount
                result = validatePackageItems(vw.PackageDiscount, tbiDiscount, true);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("4"))
            {
                //Voucher
                result = validatePackageItems(vw.PackageVoucherBuy, tbiVoucher, true);
                if (!result)
                {
                    return (result);
                }

                result = validatePackageItems(vw.PackageVoucherFree, tbiVoucher, true);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("5"))
            {
                //Bundle
                result = validatePackageItems(vw.PackageBundles, tbiBundle, true);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("6"))
            {
                //Final Discount
                result = validatePackageItems(vw.PackageFinalDiscounts, tbiFinalDiscount, (Boolean) vw.IsProductSpecific);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("7"))
            {
                //Post Gift
                result = validatePackageItems(vw.PackagePostGiftBuys, tbiPostGift, (Boolean) vw.IsProductSpecific);
                if (!result)
                {
                    return (result);
                }

                result = validatePackageItems(vw.PackagePostGiftFrees, tbiPostGift, (Boolean)vw.IsProductSpecific);
                if (!result)
                {
                    return (result);
                }
            }
            else if (vw.PackageType.Equals("8"))
            {
                //Tray Price&Discount
                result = validatePackageItems(vw.PackageTrayByItems, tbiTrayByItem, true);
                if (!result)
                {
                    return (result);
                }
            }

            return (true);
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateTextBox(lblPackageCode, txtPackageCode, false);
            if (!result)
            {
                return (result);
            }

            result = CHelper.ValidateTextBox(lblPackageName, txtPackageName, true);
            if (!result)
            {
                return (result);
            }

            result = validateItems();
            if (!result)
            {
                return (result);
            }

            CTable ug = new CTable("PACKAGE");
            MPackage uv = new MPackage(ug);
            uv.PackageCode = vw.PackageCode;
            uv.PackageID = vw.PackageID;

            CUtil.EnableForm(false, this);
            if (OnixWebServiceAPI.IsPackageExist(uv.GetDbObject()))
            {
                CUtil.EnableForm(true, this);
                CHelper.ShowKeyExist(lblPackageCode, txtPackageCode);
                return (false);
            }

            CUtil.EnableForm(true, this);
            return (result);
        }

        private Boolean SaveToView( )
        {
            if (!ValidateData())
            {
                return(false);
            }

            return (true);
        }

        private Boolean SaveData(String approveFlag)
        {
            if (!CHelper.VerifyAccessRight("SALE_PACKAGE_ADD"))
            {
                return (false);
            }

            if (Mode.Equals("A"))
            {
                if (SaveToView())
                {
                    CUtil.EnableForm(false, this);
                    CTable newobj = OnixWebServiceAPI.CreatePackage(vw.GetDbObject());
                    CUtil.EnableForm(true, this);
                    if (newobj != null)
                    {
                        vw.SetDbObject(newobj);
                        if (itemAddedHandler != null)
                        {
                            itemAddedHandler(vw, null);
                        }
                        else
                        {
                            //Will be obsoleted soon
                            parentItemsSource.Insert(0, vw);
                        }

                        return (true);
                    }

                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                    return (false);
                }
            }
            else if (Mode.Equals("E"))
            {
                if (vw.IsModified)
                {
                    Boolean result = SaveToView();
                    if (result)
                    {
                        CUtil.EnableForm(false, this);
                        CTable t = OnixWebServiceAPI.UpdatePackage(vw.GetDbObject());
                        CUtil.EnableForm(true, this);
                        if (t != null)
                        {
                            actualView.SetDbObject(t);
                            actualView.NotifyAllPropertiesChanged();

                            return (true);
                        }

                        CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_EDIT", null);
                    }

                    return (false);
                }
                else if (Mode.Equals("V"))
                {
                    return (true);
                }
            }

            return (false);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                this.Close();
                return;
            }

            Boolean r = SaveData("N");
            if (r)
            {
                vw.IsModified = false;
                DialogOK = true;
                CUtil.EnableForm(true, this);


                isOK = true;
                this.Close();
            }
        }

        private void txtText_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmdOK.Focus();
            if (vw.IsModified)
            {
                Boolean result = CHelper.AskConfirmSave();
                if (result)
                {
                    //Yes, save it
                    isOK = true;
                    Boolean r = SaveData("N");
                    e.Cancel = !r;
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void LoadData()
        {
            txtPackageCode.SetFocus();

            CTable t = new CTable("PACKAGE");
            vw = new MPackage(t);

            vw.PackageType = pg;
            DataContext = vw;

            CUtil.EnableForm(false, this);

            if (Mode.Equals("E") || Mode.Equals("V"))
            {
                CTable m = OnixWebServiceAPI.GetPackageInfo(actualView.GetDbObject());
                if (m != null)
                {
                    vw.SetDbObject(m);
                }

                //cboDiscountMappingType.SelectedIndex = CUtil.StringToInt(vw.DiscountMapType);
            }
            else if (Mode.Equals("A"))
            {
                vw.EffectiveDate = DateTime.Now;
                vw.ExpireDate = DateTime.Now.Add(new TimeSpan(30, 0, 0, 0));
                vw.IsEnabled = true;
                vw.IsTimeSpecific = false;
            }

            vw.InitPeriods();
            vw.InitItemsPrice();
            vw.InitPackageCustomers();
            vw.InitPackageDiscountFilters();
            vw.InitPackageBonusFilters();
            vw.InitPackageVoucherFilters();
            vw.InitPackageBundles();
            vw.InitPackageFinalDiscounts();
            vw.InitPackageBranches();
            vw.InitPackagePostFrees();
            vw.InitTrayPriceItem();

            vw.NotifyAllPropertiesChanged();

            vw.IsModified = false;
            CUtil.EnableForm(true, this);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            LoadData();
            isDoneRender = true;
        }

        private void cbxCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void mnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MPackagePrice)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void dtEffectiveDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void UTimeControl_OnChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private bool isTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void txtDiscount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            TextBox t = (sender as TextBox);
            String txt = e.Text;

            Boolean flag = isTextAllowed(txt);
            e.Handled = !flag;
        }

        private void tbiItem_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void tbiItem_Initialized(object sender, EventArgs e)
        {

        }

        private void UProductSelection_OnChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void OnCustomerSelectedChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdPriceAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackagePrice pp = new MPackagePrice(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddPriceItem(pp);

            vw.IsModified = true;
        }

        private void cmdCustomerAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageCustomer pp = new MPackageCustomer(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddCustomerItem(pp);

            vw.IsModified = true;
        }

        private void cmdPriceDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackagePrice pp = (MPackagePrice) (sender as Button).Tag;
            vw.RemovePriceItem(pp);

            vw.IsModified = true;
        }

        private void cmdCustomerDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageCustomer pp = (MPackageCustomer)(sender as Button).Tag;
            vw.RemoveCustomerItem(pp);

            vw.IsModified = true;
        }

        private void UProductPriceSelection_OnChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdBonusBuyDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageBonus pp = (MPackageBonus)(sender as Button).Tag;
            vw.RemoveBonusBuyItem(pp);

            vw.IsModified = true;
        }

        private void cmdBonusGetDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageBonus pp = (MPackageBonus)(sender as Button).Tag;
            vw.RemoveBonusFreeItem(pp);

            vw.IsModified = true;
        }

        private void cmdBonusBuyAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageBonus pp = new MPackageBonus(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddBonusItemBuy(pp);

            vw.IsModified = true;
        }

        private void cmdBonusFreeAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageBonus pp = new MPackageBonus(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddBonusItemFree(pp);

            vw.IsModified = true;
        }

        private void cmdDiscountAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageDiscount pp = new MPackageDiscount(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddDiscountItem(pp);

            vw.IsModified = true;
        }

        private void cmdDiscountDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageDiscount pp = (MPackageDiscount)(sender as Button).Tag;
            vw.RemoveDiscountItem(pp);

            vw.IsModified = true;
        }

        private void cboDiscountMappingType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vw.IsModified = true;
            if (isDoneRender)
            {
                //Need to reset  and reconfigure
                vw.DiscountBasketTypeConfig = "";
            }
        }

        private void cmdBundleAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageBundle pp = new MPackageBundle(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddBundleItem(pp);
            
            vw.IsModified = true;            
        }

        private void cmdVoucherBuyAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = new MPackageVoucher(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddVoucherBuyItem(pp);

            vw.IsModified = true;
        }

        private void UProductBundleSelection_OnChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdBundleDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageBundle pp = (MPackageBundle)(sender as Button).Tag;
            vw.RemoveBundleItem(pp);
            
            vw.IsModified = true;            
        }

        private void txtBundleAmount_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
            e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
        }

        private void cmdVoucherFreeAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = new MPackageVoucher(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddVoucherFreeItem(pp);

            vw.IsModified = true;
        }

        private void cmdVoucherFreeDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = (MPackageVoucher)(sender as Button).Tag;
            vw.RemoveVoucherFreeItem(pp);

            vw.IsModified = true;
        }

        private void cmdVoucherBuyDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = (MPackageVoucher)(sender as Button).Tag;
            vw.RemoveVoucherBuyItem(pp);

            vw.IsModified = true;
        }

        private void cmdFinalDiscAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageFinalDiscount pp = new MPackageFinalDiscount(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddFinalDiscountItem(pp);

            vw.IsModified = true;
        }

        private void cmdFinalDiscDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageFinalDiscount pp = (MPackageFinalDiscount)(sender as Button).Tag;
            vw.RemoveFinalDiscountItem(pp);

            vw.IsModified = true;
        }

        private void cmdFinalDiscountInterval_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditIntervalConfigEx w = new WinAddEditIntervalConfigEx(vw.DiscountDefinition, CLanguage.getValue("discount"), "DISCOUNT");
            w.ShowDialog();
            if (w.IsOK)
            {
                vw.DiscountDefinition = w.ConfigString;
                vw.IsModified = true;
            }
        }

        private void lsvBranch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvBranch.View as GridView;

            double w = (lsvBranch.ActualWidth * 1) - 35;
            double[] ratios = new double[3] { 0.05, 0.10, 0.85 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cbxBranch_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cbxBranch_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
        }

        private void cmdBasketType_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditBasketTypeSelection w = new WinAddEditBasketTypeSelection(vw.DiscountBasketTypeConfig, "", vw.BasketConfigType);
            w.ShowDialog();
            if (w.IsOK)
            {
                vw.DiscountBasketTypeConfig = w.ConfigString;
                vw.IsModified = true;
            }
        }

        private void cbxProductSpecefic_Checked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            if (isDoneRender)
            {
                //Need to reset  and reconfigure
                vw.DiscountBasketTypeConfig = "";
            }
        }

        private void cbxProductSpecefic_Unchecked(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            if (isDoneRender)
            {
                //Need to reset  and reconfigure
                vw.DiscountBasketTypeConfig = "";
            }
        }

        private void cmdPostGiftBuyAdd_Click(object sender, RoutedEventArgs e)
        {
            //Reuse MPackageVoucher as an item
            MPackageVoucher pp = new MPackageVoucher(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddPostGiftBuyItem(pp);

            vw.IsModified = true;
        }

        private void cmdPostFreeBuyDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = (MPackageVoucher)(sender as Button).Tag;
            vw.RemovePostGiftBuyItem(pp);

            vw.IsModified = true;
        }

        private void cmdPostGiftFreeDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageVoucher pp = (MPackageVoucher)(sender as Button).Tag;
            vw.RemovePostGiftFreeItem(pp);

            vw.IsModified = true;
        }

        private void cmdPostGiftFreeAdd_Click(object sender, RoutedEventArgs e)
        {
            //Reuse MPackageVoucher as an item
            MPackageVoucher pp = new MPackageVoucher(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddPostGiftFreeItem(pp);

            vw.IsModified = true;
        }

        private void cmdPGBasketType_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditBasketTypeSelection w = new WinAddEditBasketTypeSelection(vw.DiscountBasketTypeConfig, "", vw.PostGiftBasketConfigType);
            w.ShowDialog();
            if (w.IsOK)
            {
                vw.DiscountBasketTypeConfig = w.ConfigString;
                vw.IsModified = true;
            }
        }

        private void cmdPostGiftInterval_Click(object sender, RoutedEventArgs e)
        {
            //WinAddEditIntervalConfig w = new WinAddEditIntervalConfig(vw.DiscountDefinition, CLanguage.getValue("ratio"), "F");
            //w.ShowDialog();
            //if (w.IsOK)
            //{
            //    vw.DiscountDefinition = w.ConfigString;
            //    vw.IsModified = true;
            //}
        }

        private void cmdTrayByItemAdd_Click(object sender, RoutedEventArgs e)
        {
            MPackageTrayPriceDiscount pp = new MPackageTrayPriceDiscount(new CTable(""));
            pp.EnabledFlag = "Y";
            pp.SelectionType = "1";
            vw.AddTrayPriceItem(pp);

            vw.IsModified = true;
        }

        private void cmdTrayByItemDelete_Click(object sender, RoutedEventArgs e)
        {
            MPackageTrayPriceDiscount pp = (MPackageTrayPriceDiscount)(sender as Button).Tag;
            vw.RemoveTrayPriceItem(pp);

            vw.IsModified = true;
        }

        private void txtPackageCode_TextChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
