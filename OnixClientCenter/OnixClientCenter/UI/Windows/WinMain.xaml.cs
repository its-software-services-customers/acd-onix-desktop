using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.ClientCenter.UControls;
using Onix.ClientCenter.Windows;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Wis.WsClientAPI;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Reports;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.UI.Inventory.InventoryItem;

namespace Onix.ClientCenter
{
    public class UControlTupple
    {
        public UserControl uright;
        public UserControl uleft;
    }

    public partial class WinMain : Window
    {       
        private Hashtable ucontrolHash = new Hashtable();
        private String currentUserControlSelected = "";
        private Boolean isLogin = false;

        private GridLength showLength = new GridLength(100, GridUnitType.Star);
        private GridLength hideLength = new GridLength(0);

        public static readonly DependencyProperty ColLeftWidthProperty =
            DependencyProperty.Register("ColLeftWidth", typeof(GridLength), typeof(WinMain),
                new UIPropertyMetadata(new GridLength(100, GridUnitType.Star), 
                new PropertyChangedCallback(OnColLeftWidthChanged)));

        private CTable currentUser = null;

        private Hashtable menuItemConfigs = new Hashtable();
        private ArrayList menuOrders = new ArrayList();
        private Hashtable menuMaps = new Hashtable();

        public WinMain()
        {
            InitializeComponent();                        
        }

        private static void OnColLeftWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
        }

        public GridLength ColLeftWidth
        {
            get { return (GridLength)GetValue(ColLeftWidthProperty); }
            set { SetValue(ColLeftWidthProperty, value); }
        }

        private void cmdQuery_Click(object sender, RoutedEventArgs e)
        {
        }

        private MMenuItem addMenuItemConfig(String mname, String cname, String rname)
        {
            MMenuItem mnuItem = new MMenuItem(new CTable(""));
            mnuItem.MenuName = mname;
            mnuItem.ClassName = cname;
            mnuItem.AccessRightName = rname;

            menuItemConfigs.Add(mname, mnuItem);

            return (mnuItem);
        }

        private void criteriaInitFunc(String menu, CCriteriaBase cb)
        {
            cb.Initialize(menu);            
            cb.LoadScreenConfig();
        }

        private void addMenuItem(String groupKey, String menuName, String caption, String img, int grp, String criteria, String accessRight, RoutedEventHandler evt, String productHopper)
        {
            ArrayList tempArr = new ArrayList();
            if (menuMaps.Contains(groupKey))
            {
                tempArr = (ArrayList)menuMaps[groupKey];
            }
            else
            {
                menuMaps.Add(groupKey, tempArr);
            }

            MMenuItem mni = addMenuItemConfig(menuName, criteria, accessRight);
            mni.Group = grp.ToString();
            mni.Caption = caption;
            mni.Image = img;
            mni.MenuEvent = evt;
            mni.ProductHopper = productHopper;

            tempArr.Add(mni);
        }

        private void addMenuGroup(String caption, Boolean isOnlyAdmin, String productHopper)
        {
            MMenuItem mi = new MMenuItem(new CTable(""));
            mi.Caption = caption;
            mi.IsOnlyAdmin = isOnlyAdmin;
            mi.ProductHopper = productHopper;

            menuOrders.Add(mi);
        }

        private void constructMenuOrder()
        {
            //Hopper : 0=Onix, 1=Lotto, 2=Sass
            addMenuGroup("admin", true, "YYY");

            addMenuGroup("general", false, "YYY");
            addMenuGroup("sass", false, "NNY");
            addMenuGroup("inventory", false, "YNN");
            addMenuGroup("promotion_marketting", false, "NYY");
            addMenuGroup("sale_ar", false, "YYN");
            addMenuGroup("CashFlow", false, "YYN");
            addMenuGroup("purchase_ap", false, "YYN");
            addMenuGroup("hr", false, "YYY");
        }

        private void constructMenuItems()
        {
            //Start General
            addMenuItem("general", "mnuCompanyProfile", "company_profile", "bmpCompany", 1, "", "GENERAL_COMPANY_MENU", mnuMenu_Click, "YYY");
            addMenuItem("general", "mnuMasterRef", "master_ref", "bmpMasterRef", 2, "", "GENERAL_MASTER_MENU", mnuModule_Click, "YYY");
            addMenuItem("general", "mnuService", "service", "bmpService", 3, "CCriteriaService", "GENERAL_SERVICE_MENU", mnuModule_Click, "YYN");
            addMenuItem("general", "mnuServiceOther", "service_other", "bmpCoupon", 3, "CCriteriaService", "GENERAL_SERVICE_MENU", mnuModule_Click, "NNN");
            addMenuItem("general", "mnuCustomer", "sale_customer", "bmpCustomer", 4, "CCriteriaEntity", "GENERAL_ENTITY_MENU", mnuModule_Click, "YYY");
            addMenuItem("general", "mnuSupplier", "purchase_supplier", "bmpSupplier", 4, "CCriteriaEntity", "GENERAL_ENTITY_MENU", mnuModule_Click, "YYN");            
            addMenuItem("general", "mnuProject", "project", "bmpProject", 6, "CCriteriaProject", "GENERAL_PROJECT_MENU", mnuModule_Click, "YYN");
            //addMenuItem("general", "mnuCycle", "cycle", "bmpCycleCalendar", 7, "CCriteriaCycle", "GENERAL_CYCLE_MENU", mnuModule_Click, "YNN");            
            addMenuItem("general", "mnuGeneralReport", "general_report", "bmpReport", 8, "", "GENERAL_REPORT_MENU", mnuModule_Click, "YYY");
            //End General

            //Start Sass
            addMenuItem("sass", "mnuCloudProject", "cloud_project", "bmpProject", 1, "Sass.CCriteriaSassProject", "SASS_PROJECT_MENU", mnuModule_Click, "NNY");
            addMenuItem("sass", "mnuMicroService", "micro_service", "bmpMicroService", 2, "Sass.CCriteriaSassService", "SASS_SERVICE_MENU", mnuModule_Click, "NNY");
            addMenuItem("sass", "mnuDiskImage", "disk_image", "bmpDiskImage", 3, "Sass.CCriteriaSassDiskImage", "SASS_IMAGE_MENU", mnuModule_Click, "NNY");
            addMenuItem("sass", "mnuVM", "virtual_machine", "bmpServer", 3, "Sass.CCriteriaSassVM", "SASS_VM_MENU", mnuModule_Click, "NNY");
            //End Sass


            //Start Inventory
            addMenuItem("inventory", "mnuInventoryItem", "inventory_item_by_table", "bmpInventory", 1, "CCriteriaInventoryItem", "INVENTORY_ITEM_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuItemByCategory", "inventory_item_by_category", "bmpInventory", 1, "", "INVENTORY_ITEM_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuWareHouse", "inventory_location", "bmpWareHouse", 2, "CCriteriaLocation", "INVENTORY_LOCATION_MENU", mnuModule_Click, "YNN");

            addMenuItem("inventory", "mnuInventoryBorrow", "inventory_borrow", "bmpInventoryBorrow", 3, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuInventoryReturn", "inventory_return", "bmpInventoryReturn", 3, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");

            addMenuItem("inventory", "mnuInventoryImport", "inventory_import", "bmpInventoryImport", 4, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuInventoryExport", "inventory_export", "bmpInventoryExport", 4, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuInventoryXfer", "inventory_xfer", "bmpXfer", 4, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");
            addMenuItem("inventory", "mnuInventoryAdjust", "inventory_adjust", "bmpInventoryAdjust", 5, "CCriteriaInventoryDoc", "INVENTORY_DOCUMENT_MENU", mnuModule_Click, "YNN");            
            addMenuItem("inventory", "mnuInventoryReport", "inventory_report", "bmpReport", 6, "", "INVENTORY_REPORT_MENU", mnuModule_Click, "YNN");
            //End Inventory

            //Start Promotion
            addMenuItem("promotion_marketting", "mnuPackage", "Promotion", "bmpPricePlan", 1, "CCriteriaPromotion", "PROMOTION_PROMOTION_MENU", mnuModule_Click, "YYY");
            addMenuItem("promotion_marketting", "mnuStandardPackage", "sale_standard_package", "bmpStandardPackage", 2, "", "PROMOTION_PROMOTION_MENU", mnuModule_Click, "YYY");
            //End Promotion

            //Start Sale & AR
            addMenuItem("sale_ar", "mnuQuotation", "sale_quotation", "bmpQuotation", 1, "CCriteriaAuxilaryDocSale", "SALE_QUOTATION_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuSaleOrder", "sale_order", "bmpSaleOrders", 2, "CCriteriaAccountDocSale", "SALE_ORDER_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuCashSelling", "sale_cash_saling", "bmpCashSale", 3, "CCriteriaAccountDocSale", "SALE_BYCASH_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuDebtSelling", "sale_debt_saling", "bmpDebtSale", 3, "CCriteriaAccountDocSale", "SALE_BYDEBT_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuSaleMisc", "sale_misc", "bmpWalletOut", 4, "CCriteriaAccountDocSale", "SALE_MISC_MENU", mnuModule_Click, "YNY");
            addMenuItem("sale_ar", "mnuDebitNOte", "sale_debit_note", "bmpSaleDebitNote", 5, "CCriteriaAccountDocSale", "SALE_DRNOTE_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuCreditNote", "sale_credit_note", "bmpSaleCreditNote", 5, "CCriteriaAccountDocSale", "SALE_CRNOTE_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuBillSummary", "bill_summary", "bmpCashDepositAr", 6, "CCriteriaAccountDocSale", "SALE_BILLSUM_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuARReceipt", "ARRec", "bmpARRec", 7, "CCriteriaAccountDocSale", "SALE_RECEIPT_MENU", mnuModule_Click, "YYY");
            addMenuItem("sale_ar", "mnuSaleReport", "sale_report", "bmpReport", 5, "", "SALE_REPORT_MENU", mnuModule_Click, "YYY");
            //End Sale & AR

            //Start Cash
            addMenuItem("CashFlow", "mnuCashAcc", "CashAcc", "bmpMoney", 1, "CCriteriaCashAccount", "CASH_ACCOUNT_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuCashIn", "CashIn", "bmpMoney_Bag", 2, "CCriteriaCashInOut", "CASH_IN_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuCashOut", "CashOut", "bmpMoneyBag", 2, "CCriteriaCashInOut", "CASH_OUT_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuTrans", "Trans", "bmpTransfer", 2, "CCriteriaCashXfer", "CASH_XFER_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuReceivableCheque", "receivable_cheque", "bmpCheque1", 3, "CCriteriaCheque", "CASH_CHEQUE_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuPayableCheque", "payable_cheque", "bmpCheque2", 3, "CCriteriaCheque", "CASH_CHEQUE_MENU", mnuModule_Click, "YYY");
            addMenuItem("CashFlow", "mnuCashReport", "cash_report", "bmpReport", 5, "", "CASH_REPORT_MENU", mnuModule_Click, "YYY");
            //End Cash

            //Start Purchase & AP
            addMenuItem("purchase_ap", "mnuPO", "purchase_po", "bmpPurchasePO", 1, "CCriteriaAuxilaryDocPurchase", "PURCHASE_PO_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuPurchaseByCash", "purchase_cash", "bmpPurchaseCash", 2, "CCriteriaAccountDocPurchase", "PURCHASE_BYCASH_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuPurchaseByDebt", "purchase_debt", "bmpPurchaseDebt", 2, "CCriteriaAccountDocPurchase", "PURCHASE_BYDEBT_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuPurchaseMisc", "purchase_misc", "bmpWalletOut", 3, "CCriteriaAccountDocPurchase", "PURCHASE_MISC_MENU", mnuModule_Click, "YNY");
            addMenuItem("purchase_ap", "mnuDebitNOteP", "purchase_debit_note", "bmpSaleDebitNote", 4, "CCriteriaAccountDocPurchase", "PURCHASE_DRNOTE_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuCreditNoteP", "purchase_credit_note", "bmpSaleCreditNote", 4, "CCriteriaAccountDocPurchase", "PURCHASE_CRNOTE_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuAPReceipt", "APRec", "bmpARRec", 5, "CCriteriaAccountDocPurchase", "PURCHASE_RECEIPT_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuStockCost", "ap_stock_cost", "bmpCost", 6, "CCriteriaStockCostDocument", "PURCHASE_COST_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuTaxForm", "ap_revenue_department", "bmpTaxReport", 7, "CCriteriaTaxDocument", "PURCHASE_TAXDOC_MENU", mnuModule_Click, "YYY");
            addMenuItem("purchase_ap", "mnuPurchaseReport", "purchase_report", "bmpReport", 8, "", "PURCHASE_REPORT_MENU", mnuModule_Click, "YYY");
            //End Purchase & AP

            //Start HR
            addMenuItem("hr", "mnuEmployeeInfo", "employee", "bmpEmployee", 1, "CCriteriaEmployee", "HR_EMPLOYEE_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuOrganizeChart", "organize_chart", "bmpUser", 2, "", "HR_ORGCHART_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuLeave", "leave", "bmpLeave", 3, "CCriteriaEmployeeLeave", "HR_LEAVE_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuOTDocument", "hr_ot_form", "bmpCycleCalendar", 3, "CCriteriaOTDocument", "HR_OT_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuPayroll", "payroll", "bmpCashDepositAp", 3, "CCriteriaPayrollDocument", "HR_PAYROLL_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuHrTaxForm1", "hr_revenue_tax_1", "bmpTaxReport", 4, "CCriteriaHrTaxDocument", "HR_TAXFORM_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuHrTaxForm2", "hr_revenue_tax_1kor", "bmpTaxReport", 4, "CCriteriaHrTaxDocumentKor", "HR_TAXFORM_MENU", mnuModule_Click, "YYY");
            addMenuItem("hr", "mnuHr", "hr_report", "bmpReport", 5, "", "HR_REPORT_MENU", mnuModule_Click, "YYY");
            //End HR


            //Start Admin
            addMenuItem("admin", "mnuUser", "admin_user", "bmpUser", 1, "CCriteriaUser", "ADMIN_USER_MENU", mnuModule_Click, "YYY");
            addMenuItem("admin", "mnuGroup", "admin_group", "bmpUserGroup", 2, "CCriteriaUserGroup", "ADMIN_GROUP_MENU", mnuModule_Click, "YYY");
            addMenuItem("admin", "mnuLoginHistory", "login_history", "bmpLoginHistory", 3, "CCriteriaLoginHistory", "ADMIN_HISTORY_MENU", mnuModule_Click, "YYY");
            //End Admin
        }

        private Boolean isActivated(MMenuItem mi)
        {
            String hopper = mi.ProductHopper;
            String product = CConfig.GetProduct();

            Boolean required = CProductFilter.IsRequiredByProduct(hopper);

            return (required);
        }

        private void renderMenu()
        {
            foreach (MMenuItem mng in menuOrders)
            {
                String grp = mng.Caption;
                Boolean isForAdmin = mng.IsOnlyAdmin;
                Boolean isAdminLogin = OnixWebServiceAPI.IsAdmin();

                if (isForAdmin && !isAdminLogin)
                {
                    //Not show if not admin
                    continue;
                }
                else if (!isForAdmin && isAdminLogin)
                {
                    //Not show if not admin
                    continue;
                }
                else if (!isActivated(mng))
                {
                    continue;
                }

                ArrayList menuItems = (ArrayList) menuMaps[grp];

                MenuItem m = new MenuItem();

                Binding menuHeaderBinding = new Binding();
                menuHeaderBinding.Source = CTextLabel.Instance;
                menuHeaderBinding.Path = new PropertyPath(grp);
                BindingOperations.SetBinding(m, MenuItem.HeaderProperty, menuHeaderBinding);

                mnuMain.Items.Add(m);

                int cnt = 0;
                String prevGroup = "";

                foreach (MMenuItem mnu in menuItems)
                {
                    if (!isActivated(mnu))
                    {
                        continue;
                    }

                    if (cnt == 0)
                    {
                        prevGroup = mnu.Group;
                    }
                    else if ((cnt > 0) && (!prevGroup.Equals(mnu.Group)))
                    {
                        Separator sp = new Separator();
                        m.Items.Add(sp);

                        prevGroup = mnu.Group;
                    }

                    cnt++;
                    MenuItem mi = new MenuItem();

                    Binding menuItemHeaderBinding = new Binding();
                    menuItemHeaderBinding.Source = CTextLabel.Instance;
                    menuItemHeaderBinding.Path = new PropertyPath(mnu.Caption);
                    BindingOperations.SetBinding(mi, MenuItem.HeaderProperty, menuItemHeaderBinding);

                    BitmapImage bi = (BitmapImage) Application.Current.FindResource(mnu.Image);
                    Image img = new Image();
                    img.Source = bi;

                    mi.Icon = img;

                    mi.Click += new RoutedEventHandler(mnu.MenuEvent);
                    mi.Name = mnu.MenuName;

                    m.Items.Add(mi);
                }
            }
        }

        private void configureMenu()
        {
            constructMenuOrder();
            constructMenuItems();
            renderMenu();
        }

        private CCriteriaBase getMenuCriteria(String mnuName)
        {
            MMenuItem m = (MMenuItem) menuItemConfigs[mnuName];
            if (m == null)
            {
                return (null);
            }

            if (m.ClassName.Equals(""))
            {
                return (null);
            }

            CUtil.EnableForm(false, this);

            CCriteriaBase cr = null;
            if (mnuName.Equals("mnuCashIn"))
            {
                cr = FactoryCriteria.GetObject(m.ClassName, new MCashDocIn(new CTable("")));                
            }
            else if (mnuName.Equals("mnuCashOut"))
            {
                cr = FactoryCriteria.GetObject(m.ClassName, new MCashDocOut(new CTable("")));
            }
            else
            {
                cr = FactoryCriteria.GetObject(m.ClassName);
            }

            criteriaInitFunc(mnuName, cr);
            CUtil.EnableForm(true, this);

            return (cr);
        }

        private void WinMain_OnLoad(object sender, RoutedEventArgs e)
        {
            CConfig.ConfigRead();

            initStatusBar();

            WinLogin wLogin = new WinLogin();

            wLogin.ShowDialog();
            Boolean isOK = wLogin.LoginOK;

            if (isOK)
            {
                stbiUserValue.Content = OnixWebServiceAPI.GetLastUserLogin();
                isLogin = true;

                configureMenu();

                //Load user variables 
                if (!OnixWebServiceAPI.UserID().Equals(""))
                {
                    currentUser = new CTable("USER");
                    currentUser.SetFieldValue("USER_ID", OnixWebServiceAPI.UserID());
                    currentUser = OnixWebServiceAPI.GetUserInfo(currentUser);
                    CConfig.LoadLastValueSaved(currentUser);
                }

                CReportFactory.InitReports();
                CMasterReference.LoadCompanyProfile();
                CGlobalVariable.InitGlobalVariables();
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetAllMasterRefList);
                CUtil.ExportLoadingImage();
            }
        }

        private void initStatusBar()
        {
            double w1 = stbiUser.Width + stbiServer.Width + stbiVersion.Width;
            double w0 = this.ActualWidth;
            double left = w0 - w1;
            double r = left / 4;

            stbiUserValue.Width = 0.70 * r;            
            stbiServerValue.Width = 2.30 *r;
            stbiVersionValue.Width = 1 * r;

            stbiServerValue.Content = CConfig.GetUrl();
            stbiVersionValue.Content = CConfig.Version;
        }

        private void mnuMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (MenuItem)sender;

            if (mnu.Name.Equals("mnuExit"))
            {
                this.Close();
            }
            else if (mnu.Name.Equals("mnuPasswd"))
            {
                WinEditUser w = new WinEditUser();
                w.Caption = CLanguage.getValue("ADMIN_PASSWD");
                w.ShowDialog();
            }
            else if (mnu.Name.Equals("mnuServer"))
            {
                WinServerSetting w = new WinServerSetting();
                w.ShowDialog();
            }
            else if (mnu.Name.Equals("mnuCompanyProfile"))
            {
                if (CHelper.VerifyAccessRight("GENERAL_COMPANY_EDIT"))
                {
                    WinAddEditCompanyProf cp = new WinAddEditCompanyProf();
                    cp.Title = (String)mnu.Header;
                    cp.ShowDialog();

                    if (cp.DialogOK)
                    {
                        CMasterReference.LoadCompanyProfile();
                    }
                }
            }
            else if (mnu.Name.Equals("mnuFormatDoc"))
            {
                if (CHelper.VerifyAccessRight("GENERAL_FMTDOC_EDIT"))
                {
                    WinDocumentConfig fd = new WinDocumentConfig((String)mnu.Header);
                    fd.ShowDialog();
                }
            }
            else if (mnu.Name.Equals("mnuGlobalVariable"))
            {
                if (CHelper.VerifyAccessRight("GENERAL_GLOBAL_VARIABLE_EDIT"))
                {
                    WinGlobalVariable ga = new WinGlobalVariable((String)mnu.Header);
                    ga.ShowDialog();
                }
            }
        }

        private void mnuModule_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (MenuItem)sender;
            String name = mnu.Name;
            UControlTupple ua;
            UControlTupple old;
            UserControl uleft;
            UserControl uright;
            String header = (String) mnu.Header;

            this.Title = (String)mnu.Header;

            if (name.Equals("mnuLoginSession") || name.Equals("mnuInventoryReport") || 
                name.Equals("mnuSaleReport") || name.Equals("mnuStandardPackage") ||
                name.Equals("mnuCashReport") || name.Equals("mnuCompanyCommissionProfile") || name.Equals("mnuHr") ||
                name.Equals("mnuCommissionReport") || name.Equals("mnuPurchaseReport") || name.Equals("mnuGeneralReport"))
            {
                ColLeftWidth = hideLength;
            }
            else
            {
                ColLeftWidth = showLength;
            }

            if (menuItemConfigs.ContainsKey(name))
            {
                if (!name.Equals("mnuMasterRef") && !name.Equals("mnuItemByCategory"))
                {
                    ColLeftWidth = hideLength;
                }
            }

            if (currentUserControlSelected.Equals(name))
            {
                //Do nothing, same menu selected
                return;
            }

            old = (UControlTupple) ucontrolHash[currentUserControlSelected];

            if (ucontrolHash.ContainsKey(name))
            {
                ua = (UControlTupple) ucontrolHash[name];                
                currentUserControlSelected = name;
            }
            else
            {
                CCriteriaBase cr = getMenuCriteria(name);

                if (cr != null)
                {
                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UCriteriaGeneric(cr, header);
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    CUtil.EnableForm(true, this);

                    (uright as UCriteriaGeneric).SetFirstFocus();
                }
                else if (name.Equals("mnuMasterRef"))
				{
					if (!CHelper.VerifyAccessRight("GENERAL_MASTER_MENU"))
					{
						return;
					}

					CUtil.EnableForm(false, this);
					uleft = new UMasterRefSearch();

					Grid.SetRow(uleft, 0);
					Grid.SetColumn(uleft, 0);
					grdMain.Children.Add(uleft);

					uright = new UMasterRef();
					Grid.SetRow(uright, 0);
					Grid.SetColumn(uright, 1);
					grdMain.Children.Add(uright);
					(uright as UMasterRef).InitUserControl();

					(uleft as UMasterRefSearch).SetProvider(uright);
					(uleft as UMasterRefSearch).LoadData();
					CUtil.EnableForm(true, this);
				}
                else if (name.Equals("mnuItemByCategory"))
                {
                    if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UInventoryItemByCategorySearch();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UInventoryItem(2);
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UInventoryItem).InitUserControl();

                    (uleft as UInventoryItemByCategorySearch).SetProvider(uright);
                    (uleft as UInventoryItemByCategorySearch).LoadData();
                    CUtil.EnableForm(true, this);
                }                
				else if (name.Equals("mnuOrganizeChart"))
                {
                    if (!CHelper.VerifyAccessRight("HR_ORGCHART_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UI.HumanResource.OrgChart.UCompanyOrgChart();
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UI.HumanResource.OrgChart.UCompanyOrgChart).InitUserControl();

                    CUtil.EnableForm(true, this);
                }
                else if (name.Equals("mnuStandardPackage"))
				{
					if (!CHelper.VerifyAccessRight("SALE_STDPACKAGE_MENU"))
					{
						return;
					}

					CUtil.EnableForm(false, this);
					uleft = new UBlank();

					Grid.SetRow(uleft, 0);
					Grid.SetColumn(uleft, 0);
					grdMain.Children.Add(uleft);

					uright = new UStandardPackage();
					Grid.SetRow(uright, 0);
					Grid.SetColumn(uright, 1);
					grdMain.Children.Add(uright);
					(uright as UStandardPackage).InitUserControl();

					CUtil.EnableForm(true, this);
				}
				else if (name.Equals("mnuInventoryReport"))
				{
                    if (!CHelper.VerifyAccessRight("INVENTORY_REPORT_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
					uleft = new UBlank();

					Grid.SetRow(uleft, 0);
					Grid.SetColumn(uleft, 0);
					grdMain.Children.Add(uleft);

					uright = new UReportView();
					Grid.SetRow(uright, 0);
					Grid.SetColumn(uright, 1);
					grdMain.Children.Add(uright);
					(uright as UReportView).ReportType = ReportType.ReportInv;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
					CUtil.EnableForm(true, this);
				}
				else if (name.Equals("mnuSaleReport"))
				{
                    if (!CHelper.VerifyAccessRight("SALE_REPORT_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
					uleft = new UBlank();

					Grid.SetRow(uleft, 0);
					Grid.SetColumn(uleft, 0);
					grdMain.Children.Add(uleft);

					uright = new UReportView();
					Grid.SetRow(uright, 0);
					Grid.SetColumn(uright, 1);
					grdMain.Children.Add(uright);
					(uright as UReportView).ReportType = ReportType.ReportSale;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
					CUtil.EnableForm(true, this);
				}
                else if (name.Equals("mnuGeneralReport"))
                {
                    if (!CHelper.VerifyAccessRight("GENERAL_REPORT_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UReportView();
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UReportView).ReportType = ReportType.ReportGeneral;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
                    CUtil.EnableForm(true, this);
                }
                else if (name.Equals("mnuPurchaseReport"))
                {
                    if (!CHelper.VerifyAccessRight("PURCHASE_REPORT_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UReportView();
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UReportView).ReportType = ReportType.ReportPurchase;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
                    CUtil.EnableForm(true, this);
                }
                else if (name.Equals("mnuCashReport"))
                {                    
                    if (!CHelper.VerifyAccessRight("CASH_REPORT_MENU"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UReportView();
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UReportView).ReportType = ReportType.ReportCash;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
                    CUtil.EnableForm(true, this);
                }
                else if (name.Equals("mnuHr"))
                {
                    if (!CHelper.VerifyAccessRight("HR_PAYROLL_REPORT"))
                    {
                        return;
                    }

                    CUtil.EnableForm(false, this);
                    uleft = new UBlank();

                    Grid.SetRow(uleft, 0);
                    Grid.SetColumn(uleft, 0);
                    grdMain.Children.Add(uleft);

                    uright = new UReportView();
                    Grid.SetRow(uright, 0);
                    Grid.SetColumn(uright, 1);
                    grdMain.Children.Add(uright);
                    (uright as UReportView).ReportType = ReportType.ReportHr;
                    (uright as UReportView).typeReport = this.Title;
                    (uright as UReportView).LoadData();
                    CUtil.EnableForm(true, this);
                }
                else
                {
					uleft = new UBlank();
					uright = null;
					Grid.SetRow(uleft, 0);
					Grid.SetColumn(uleft, 1);
					grdMain.Children.Add(uleft);
				}

                currentUserControlSelected = name;
                ua = new UControlTupple();
                ua.uleft = uleft;
                ua.uright = uright;
                ucontrolHash.Add(name, ua);
            }

            if (old != null)
            {
                old.uleft.Visibility = Visibility.Hidden;
                if (old.uright != null)
                {
                    old.uright.Visibility = Visibility.Hidden;
                }
            }

            ua.uleft.Visibility = Visibility.Visible;
            if (ua.uright != null)
            {
                ua.uright.Visibility = Visibility.Visible;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isLogin)
            {
                Boolean result = CHelper.AskConfirmMessage("CONFIRM_EXIT");
                if (result)
                {
                    CUtil.EnableForm(false, this);
                    CConfig.ConfigWrite();
                    OnixWebServiceAPI.Logout(new CTable("DUMMY"));
                    CUtil.EnableForm(true, this);
//e.Cancel = true;
                    return;
                }

                e.Cancel = true;
            }

            CConfig.ConfigWrite();
        }

        private void mnuItemGroup_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mi = sender as MenuItem;

            //Win_AddEditItemGroup ig = new Win_AddEditItemGroup();
            //ig.Title = (String) mi.Header;
            //ig.ShowDialog();
        }

        private void mnuReportFilter_Click(object sender, RoutedEventArgs e)
        {
            WinFormPrinting w = new WinFormPrinting("", null);
            w.ShowDialog();
        }

        private void mnuImportExcel_Click(object sender, RoutedEventArgs e)
        {
            WinImportData w = new WinImportData();
            w.ShowDialog();
        }

        private void mainWindow_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
