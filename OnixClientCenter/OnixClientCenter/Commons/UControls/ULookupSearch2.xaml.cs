using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.UI.General.Service;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.UI.HumanResource.EmployeeInfo;

namespace Onix.ClientCenter.Commons.UControls
{
    public enum LookupSearchType2
    {
        InventoryItemLookup = 1,
        CustomerLookup = 2,
        SupplierLookup = 3,
        EmployeeLookup = 4,
        ServiceLookup = 5, /* Both */
        ServiceSaleLookup = 6,
        ServicePurchaseLookup = 7,
        ProjectLookup = 8,
        CustomerTypeLookup = 9,
        CustomerGroupLookup = 10,
        ItemCategoryLookup = 11,
        ServiceRegularSaleLookup = 12,
        ServiceRegularPurchaseLookup = 13,
        ServiceOtherSaleLookup = 14,
        ServiceOtherPurchaseLookup = 15,
        ServiceTypeLookup = 16,
        InventoryBorrowReturnLookup = 17,

        EmployeeLookupMonthly = 18,
        EmployeeLookupDaily = 19,
    }

    public class CLookupConfig
    {
        public String CodeFieldName { get; set;}
        public String NameFieldName { get; set; }
        public String ExtraFieldName { get; set; }
        public String ExtraFieldValue { get; set; }
        public String ExtraFieldName2 { get; set; }
        public String ExtraFieldValue2 { get; set; }
        public GetListFunction ListFunction { get; set; }
        public Type ModelType { get; set; }
        public String ScreenCriteriaClassName { get; set; }
        public TextSearchNameSpace SearchNameSpace { get; set; }
        public String TableAlias { get; set; }
    }

    public partial class ULookupSearch2 : UserControl
    { 
        public static readonly DependencyProperty SelectedObjectProperty =
        DependencyProperty.Register("SelectedObject", typeof(MBaseModel), typeof(ULookupSearch2),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                new PropertyChangedCallback(OnSelectedObjectPropertyChanged)));

        public static readonly DependencyProperty LookupProperty =
        DependencyProperty.Register("Lookup", typeof(LookupSearchType2), typeof(ULookupSearch2),
            new FrameworkPropertyMetadata(LookupSearchType2.CustomerLookup, new PropertyChangedCallback(OnLookupPropertyChanged)));

        public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register("Caption", typeof(String), typeof(ULookupSearch2),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnCaptionPropertyChanged)));

        public static readonly DependencyProperty AssociateObjectProperty =
        DependencyProperty.Register("AssociateObject", typeof(MBaseModel), typeof(ULookupSearch2),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAssociateObjectPropertyChanged)));

        public event EventHandler SelectedObjectChanged;

        private Boolean isEmpty = true;
        private Hashtable lookupTypeConfigs = new Hashtable();
        private ArrayList buttonContextMenu = new ArrayList();

        public LookupSearchType2 Lookup
        {
            get { return (LookupSearchType2)GetValue(LookupProperty); }
            set { SetValue(LookupProperty, value); }
        }

        public String Caption
        {
            get { return (String)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public MBaseModel AssociateObject
        {
            get { return (MBaseModel)GetValue(AssociateObjectProperty); }
            set { SetValue(AssociateObjectProperty, value); }
        }

        private static String lookupTypeToCaption(LookupSearchType2 lt)
        {
            Hashtable temps = new Hashtable();

            temps[LookupSearchType2.InventoryItemLookup] = CLanguage.getValue("item");
            temps[LookupSearchType2.CustomerLookup] = CLanguage.getValue("sale_customer");
            temps[LookupSearchType2.SupplierLookup] = CLanguage.getValue("purchase_supplier");
            temps[LookupSearchType2.EmployeeLookup] = CLanguage.getValue("employee");
            temps[LookupSearchType2.EmployeeLookupDaily] = CLanguage.getValue("employee");
            temps[LookupSearchType2.EmployeeLookupMonthly] = CLanguage.getValue("employee");
            temps[LookupSearchType2.ServiceLookup] = CLanguage.getValue("service");
            temps[LookupSearchType2.ProjectLookup] = CLanguage.getValue("project");
            temps[LookupSearchType2.CustomerTypeLookup] = CLanguage.getValue("custom_type");
            temps[LookupSearchType2.CustomerGroupLookup] = CLanguage.getValue("customer_group");
            temps[LookupSearchType2.ServiceTypeLookup] = CLanguage.getValue("service_type");
            temps[LookupSearchType2.ItemCategoryLookup] = CLanguage.getValue("item_category");
            temps[LookupSearchType2.InventoryBorrowReturnLookup] = CLanguage.getValue("item");

            return ((String) temps[lt]);
        }

        private static MasterRefEnum lookupTypeToMasterRef(LookupSearchType2 lk)
        {
            Hashtable temp = new Hashtable();
            //Only for Master Ref
            temp[LookupSearchType2.CustomerTypeLookup] = MasterRefEnum.MASTER_CUSTOMER_TYPE;
            temp[LookupSearchType2.CustomerGroupLookup] = MasterRefEnum.MASTER_CUSTOMER_GROUP;
            temp[LookupSearchType2.ServiceTypeLookup] = MasterRefEnum.MASTER_SERVICE_TYPE;

            if (temp.ContainsKey(lk))
            {
                return ((MasterRefEnum)temp[lk]);
            }

            return (MasterRefEnum.MASTER_REF_UNDEF);
        }

        private static void OnAssociateObjectPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ULookupSearch2 control = sender as ULookupSearch2;
        }

        private static void OnLookupPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ULookupSearch2 control = sender as ULookupSearch2;
            control.Lookup = (LookupSearchType2)e.NewValue;

            CLookupConfig cfg = (CLookupConfig)control.lookupTypeConfigs[control.Lookup];

            control.txtCode.TextSearchNameSpace = cfg.SearchNameSpace;
            control.txtCode.MasterRefType = lookupTypeToMasterRef(control.Lookup);
        }

        private static void OnCaptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ULookupSearch2 control = sender as ULookupSearch2;
            control.Caption = (String)e.NewValue;
        }

        public MBaseModel SelectedObject
        {
            get { return (MBaseModel)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        private static void OnSelectedObjectPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ULookupSearch2 control = sender as ULookupSearch2;
            MBaseModel v = (MBaseModel)e.NewValue;

            if (v != null)
            {
                CTable obj = v.GetDbObject();

                CLookupConfig cfg = (CLookupConfig)control.lookupTypeConfigs[control.Lookup];
                control.txtCode.Text = obj.GetFieldValue(cfg.CodeFieldName);
                control.txtName.Text = obj.GetFieldValue(cfg.NameFieldName);
            }
            else
            {
                control.txtCode.Text = "";
                control.txtName.Text = "";
            }

            control.isEmpty = control.txtCode.Text.Equals("");

            if (control.SelectedObjectChanged != null)
            {
                control.SelectedObjectChanged(control, null);
            }
        }

        private void addLookupTypeConfig(LookupSearchType2 ltype, String codeFieldName, String nameFieldName, String extraFieldName, 
            String extraFieldValue, String extraFieldName2, String extraFieldValue2, GetListFunction func, Type mt, String crName, TextSearchNameSpace ns, String tbAlias)
        {
            CLookupConfig cfg = new CLookupConfig();

            cfg.CodeFieldName = codeFieldName;
            cfg.NameFieldName = nameFieldName;
            cfg.ExtraFieldName = extraFieldName;
            cfg.ExtraFieldValue = extraFieldValue;
            cfg.ExtraFieldName2 = extraFieldName2;
            cfg.ExtraFieldValue2 = extraFieldValue2;
            cfg.ListFunction = func;
            cfg.ModelType = mt;
            cfg.ScreenCriteriaClassName = FactoryCriteria.GetFqdnClassName(crName);
            cfg.SearchNameSpace = ns;
            cfg.TableAlias = tbAlias;

            lookupTypeConfigs.Add(ltype, cfg);
        }

        public Boolean IsEmpty()
        {
            return (isEmpty);
        }

        public ULookupSearch2()
        {
            addLookupTypeConfig(LookupSearchType2.CustomerLookup, "ENTITY_CODE", "ENTITY_NAME", "CATEGORY", "1", "", "",
                OnixWebServiceAPI.GetEntityList, typeof(MEntity), "CCriteriaEntity", TextSearchNameSpace.CustomerCodeNS, "EN.");

            addLookupTypeConfig(LookupSearchType2.SupplierLookup, "ENTITY_CODE", "ENTITY_NAME", "CATEGORY", "2", "", "",
                OnixWebServiceAPI.GetEntityList, typeof(MEntity), "CCriteriaEntity", TextSearchNameSpace.SupplierCodeNS, "EN.");

            addLookupTypeConfig(LookupSearchType2.InventoryItemLookup, "ITEM_CODE", "ITEM_NAME_THAI", "", "", "", "",
                OnixWebServiceAPI.GetInventoryItemList, typeof(MInventoryItem), "CCriteriaInventoryItem", TextSearchNameSpace.ItemCodeNS, "IT.");

            addLookupTypeConfig(LookupSearchType2.InventoryBorrowReturnLookup, "ITEM_CODE", "ITEM_NAME_THAI", "BORROW_FLAG", "Y", "", "",
                OnixWebServiceAPI.GetInventoryItemList, typeof(MInventoryItem), "CCriteriaInventoryItem", TextSearchNameSpace.ItemCodeBorrowReturnNS, "IT.");


            addLookupTypeConfig(LookupSearchType2.EmployeeLookup, "EMPLOYEE_CODE", "EMPLOYEE_NAME_LASTNAME", "", "", "", "",
                OnixWebServiceAPI.GetEmployeeList, typeof(MEmployee), "CCriteriaEmployee", TextSearchNameSpace.EmployeeCodeNS, "EP.");

            addLookupTypeConfig(LookupSearchType2.EmployeeLookupDaily, "EMPLOYEE_CODE", "EMPLOYEE_NAME_LASTNAME", "EMPLOYEE_TYPE", "1", "", "",
                OnixWebServiceAPI.GetEmployeeList, typeof(MEmployee), "CCriteriaEmployee", TextSearchNameSpace.EmployeeDailyCodeNS, "EP.");

            addLookupTypeConfig(LookupSearchType2.EmployeeLookupMonthly, "EMPLOYEE_CODE", "EMPLOYEE_NAME_LASTNAME", "EMPLOYEE_TYPE", "2", "", "",
                OnixWebServiceAPI.GetEmployeeList, typeof(MEmployee), "CCriteriaEmployee", TextSearchNameSpace.EmployeeMonthlyCodeNS, "EP.");


            addLookupTypeConfig(LookupSearchType2.ServiceLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "", "", "",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeNS, "SV.");

            addLookupTypeConfig(LookupSearchType2.ServiceSaleLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "1", "", "",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeSaleNS, "SV.");

            addLookupTypeConfig(LookupSearchType2.ServicePurchaseLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "2", "", "",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodePurchaseNS, "SV.");

            addLookupTypeConfig(LookupSearchType2.ProjectLookup, "PROJECT_CODE", "PROJECT_NAME", "", "", "", "",
                OnixWebServiceAPI.GetProjectList, typeof(MProject), "CCriteriaProject", TextSearchNameSpace.ProjectCodeNS, "PJ.");

            addLookupTypeConfig(LookupSearchType2.ItemCategoryLookup, "CATEGORY_NAME", "CATEGORY_NAME", "", "", "", "",
                OnixWebServiceAPI.GetItemCategoryList, typeof(MItemCategory), "CCriteriaItemCategory", TextSearchNameSpace.ItemCategoryPathNS, "");


            //== Start Maste Ref == 

            addLookupTypeConfig(LookupSearchType2.CustomerTypeLookup, "CODE", "DESCRIPTION", "REF_TYPE", ((int) MasterRefEnum.MASTER_CUSTOMER_TYPE).ToString(), "", "",
                OnixWebServiceAPI.GetMasterRefList, typeof(MMasterRef), "CCriteriaMasterRef", TextSearchNameSpace.MasterRefCodeNS, "");

            addLookupTypeConfig(LookupSearchType2.CustomerGroupLookup, "CODE", "DESCRIPTION", "REF_TYPE", ((int)MasterRefEnum.MASTER_CUSTOMER_GROUP).ToString(), "", "",
                OnixWebServiceAPI.GetMasterRefList, typeof(MMasterRef), "CCriteriaMasterRef", TextSearchNameSpace.MasterRefCodeNS, "");

            addLookupTypeConfig(LookupSearchType2.ServiceTypeLookup, "CODE", "DESCRIPTION", "REF_TYPE", ((int)MasterRefEnum.MASTER_SERVICE_TYPE).ToString(), "", "",
                OnixWebServiceAPI.GetMasterRefList, typeof(MMasterRef), "CCriteriaMasterRef", TextSearchNameSpace.MasterRefCodeNS, "");
            
            //== End Maste Ref == 

            addLookupTypeConfig(LookupSearchType2.ServiceRegularSaleLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "1", "SERVICE_LEVEL", "1",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeRegularSaleNS, "SV.");

            addLookupTypeConfig(LookupSearchType2.ServiceRegularPurchaseLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "2", "SERVICE_LEVEL", "1",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeRegularPurchaseNS, "SV.");


            addLookupTypeConfig(LookupSearchType2.ServiceOtherSaleLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "1", "SERVICE_LEVEL", "2",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeOtherSaleNS, "SV.");

            addLookupTypeConfig(LookupSearchType2.ServiceOtherPurchaseLookup, "SERVICE_CODE", "SERVICE_NAME", "CATEGORY", "2", "SERVICE_LEVEL", "2",
                OnixWebServiceAPI.GetServiceList, typeof(MService), "CCriteriaService", TextSearchNameSpace.ServiceCodeOtherPurchaseNS, "SV.");


            InitializeComponent();
        }

        private void GetObjectInfo(String code)
        {
            CLookupConfig cfg = (CLookupConfig)lookupTypeConfigs[Lookup];

            ArrayList arr = null;
            CTable tb = new CTable("");

            tb.SetFieldValue(cfg.CodeFieldName, code);
            tb.SetFieldValue("!EXT_EQUAL_STRING_COMPARE_FIELDS", cfg.TableAlias + cfg.CodeFieldName);

            if (!cfg.ExtraFieldValue.Equals(""))
            {
                tb.SetFieldValue(cfg.ExtraFieldName, cfg.ExtraFieldValue);
            }

            if (!cfg.ExtraFieldValue2.Equals(""))
            {
                tb.SetFieldValue(cfg.ExtraFieldName2, cfg.ExtraFieldValue2);
            }

            CUtil.EnableForm(false, this);

            arr = cfg.ListFunction(tb);

            if ((arr == null) || (arr.Count <= 0))
            {
                SelectedObject = null;
                isEmpty = true;
            }
            else
            {
                CTable o = (CTable)arr[0];
                MBaseModel instance = (MBaseModel) Activator.CreateInstance(cfg.ModelType, new object[] { o });
                SelectedObject = instance;

                txtCode.Text = o.GetFieldValue(cfg.CodeFieldName);
                txtName.Text = o.GetFieldValue(cfg.NameFieldName);

                isEmpty = false;
            }

            CUtil.EnableForm(true, this);
        }

        private void mnuSearch_Click(object sender, RoutedEventArgs e)
        {
            CLookupConfig cfg = (CLookupConfig)lookupTypeConfigs[Lookup];
            CCriteriaBase cr = (CCriteriaBase)Activator.CreateInstance(Type.GetType(cfg.ScreenCriteriaClassName));
            cr.SetActionEnable(false);
            criteriaInitFunc(cr);

            String caption = Caption;
            if (Caption.Equals(""))
            {
                caption = lookupTypeToCaption(Lookup);
            }

            WinLookupSearch2 w = new WinLookupSearch2(cr, caption);
            w.ShowDialog();

            if (w.IsOK)
            {
                SelectedObject = w.ReturnedObj;
            }
        }

        #region Custom Add-Menu

        private void mnuCustomerAdd_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = lookupTypeToCaption(Lookup);
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = arr;
            FactoryWindow.ShowWindow("WinAddEditCustomer", param);
        }

        protected static void showWindow(CWinLoadParam param, String className)
        {
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
        }

        private void mnuServiceAdd_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("service");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int) ServiceLevel.ServiceLevelRegular).ToString();
            param.Mode = "A";
            param.ParentItemSources = arr;
            FactoryWindow.ShowWindow("WinAddEditService", param);
        }

        private void mnuServiceAddOther_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("service");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int)ServiceLevel.ServiceLevelOther).ToString();
            param.Mode = "A";
            param.ParentItemSources = arr;
            FactoryWindow.ShowWindow("WinAddEditService", param);
        }

        private void mnuInventoryAdd_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("inventory_item");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = "";
            param.Mode = "A";
            param.ParentItemSources = arr;
            FactoryWindow.ShowWindow("WinAddEditInventoryItem", param);
        }

        private void showSalePurchaseHistoryWindow(String category, String selectType, String langKey)
        {
            MSalePurchaseHistory h = new MSalePurchaseHistory(new CTable(""));
            if (selectType.Equals("2"))
            {
                h.ItemCode = this.txtCode.Text;
            }
            else
            {
                //1=Service
                h.ServiceCode = this.txtCode.Text;
            }
            h.Category = category;
            h.SelectionType = selectType;

            //ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            Criteria.CCriteriaSalePurchaseHistory cr = new Criteria.CCriteriaSalePurchaseHistory();
            cr.SetActionEnable(false);
            cr.SetDefaultData(h);
            cr.Init(category);

            String caption = CLanguage.getValue(langKey);

            WinLookupSearch1 w = new WinLookupSearch1(cr, caption);
            w.ShowDialog();
        }

        private void showItemCostHistoryWindow(String langKey)
        {
            String itemID = "";

            MInventoryCurrentBalance h = new MInventoryCurrentBalance(new CTable(""));
            MInventoryItem t = (MInventoryItem) SelectedObject;

            if (t != null)
            {
                itemID = t.ItemID;
            }
            
            h.ItemID = itemID;

            Criteria.CCriteriaItemCostHistory cr = new Criteria.CCriteriaItemCostHistory();
            cr.SetActionEnable(false);
            cr.SetDefaultData(h);
            cr.Init("");

            String caption = CLanguage.getValue(langKey);

            WinLookupSearch1 w = new WinLookupSearch1(cr, caption);
            w.ShowDialog();
        }

        private void mnuInventoryHistory_Click1(object sender, RoutedEventArgs e)
        {
            showSalePurchaseHistoryWindow("1", "2", "sale_history");
        }

        private void mnuInventoryHistory_Click2(object sender, RoutedEventArgs e)
        {
            showSalePurchaseHistoryWindow("2", "2", "purchase_history");
        }

        private void mnuInventoryHistory_Click3(object sender, RoutedEventArgs e)
        {
            showItemCostHistoryWindow("item_cost_balance");
        }

        private void mnuServiceHistory_Click1(object sender, RoutedEventArgs e)
        {
            showSalePurchaseHistoryWindow("1", "1", "sale_history");
        }

        private void mnuServiceHistory_Click2(object sender, RoutedEventArgs e)
        {
            showSalePurchaseHistoryWindow("2", "1", "purchase_history");
        }

        private void mnuSupplierAdd_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = lookupTypeToCaption(Lookup);
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = arr;
            FactoryWindow.ShowWindow("WinAddEditSupplier", param);
        }

        private void mnuEmployeeAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_EMPLOYEE_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("employee");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = new ObservableCollection<MBaseModel>();
            FactoryWindow.ShowWindow("WinAddEditEmployeeInfo", param);
        }

        private void mnuProjectAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_PROJECT_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + lookupTypeToCaption(Lookup);
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = new ObservableCollection<MBaseModel>();
            FactoryWindow.ShowWindow("WinAddEditProject", param);
        }
        #endregion

        #region Depend on LookupType here
        private void criteriaInitFunc(CCriteriaBase cb)
        {
            if (Lookup == LookupSearchType2.CustomerLookup)
            {
                cb.Init("1");
            }
            else if (Lookup == LookupSearchType2.SupplierLookup)
            {
                cb.Init("2");
            }
            else if ((Lookup == LookupSearchType2.CustomerGroupLookup) || 
                (Lookup == LookupSearchType2.CustomerTypeLookup) ||
                (Lookup == LookupSearchType2.ServiceTypeLookup))
            {
                cb.Init(((int) lookupTypeToMasterRef(Lookup)).ToString());
            }
            else if (Lookup == LookupSearchType2.ServiceLookup)
            {
                cb.Init("0");
            }
            else if (Lookup == LookupSearchType2.ServiceSaleLookup)
            {
                cb.Init("1");
            }
            else if (Lookup == LookupSearchType2.ServicePurchaseLookup)
            {
                cb.Init("2");
            }
            else if (Lookup == LookupSearchType2.ServiceRegularSaleLookup)
            {
                (cb as CCriteriaService).Init("1", ((int) ServiceLevel.ServiceLevelRegular).ToString());
            }
            else if (Lookup == LookupSearchType2.ServiceOtherSaleLookup)
            {
                (cb as CCriteriaService).Init("1", ((int)ServiceLevel.ServiceLevelOther).ToString());
            }
            else if (Lookup == LookupSearchType2.ServiceRegularPurchaseLookup)
            {
                (cb as CCriteriaService).Init("2", ((int)ServiceLevel.ServiceLevelRegular).ToString());
            }
            else if (Lookup == LookupSearchType2.ServiceOtherPurchaseLookup)
            {
                (cb as CCriteriaService).Init("2", ((int)ServiceLevel.ServiceLevelOther).ToString());
            }
            else if (Lookup == LookupSearchType2.EmployeeLookupDaily)
            {
                (cb as CCriteriaEmployee).Init("1");
            }
            else if (Lookup == LookupSearchType2.EmployeeLookupMonthly)
            {
                (cb as CCriteriaEmployee).Init("2");
            }
            else
            {
                cb.Init("");
            }
        }

        private void addDefaultContextMenu(Button btn)
        {
            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuSearch", "search", new RoutedEventHandler(mnuSearch_Click), 1);
            buttonContextMenu.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuClear", "clear", new RoutedEventHandler(mnuClear_Click), 2);
            buttonContextMenu.Add(ct2);
        }

        private void addContextMenu(Button btn)
        {
            if (Lookup == LookupSearchType2.CustomerLookup)
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuCustomerAdd_Click), 3);
                buttonContextMenu.Add(ct1);
            }
            else if (Lookup == LookupSearchType2.SupplierLookup)
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuSupplierAdd_Click), 3);
                buttonContextMenu.Add(ct1);
            }
            else if ((Lookup == LookupSearchType2.ServiceLookup) ||
                    (Lookup == LookupSearchType2.ServiceSaleLookup) ||
                    (Lookup == LookupSearchType2.ServiceRegularSaleLookup) ||
                    (Lookup == LookupSearchType2.ServiceOtherSaleLookup) ||
                    (Lookup == LookupSearchType2.ServicePurchaseLookup) ||
                    (Lookup == LookupSearchType2.ServiceRegularPurchaseLookup) ||
                    (Lookup == LookupSearchType2.ServiceOtherPurchaseLookup))
            {
                if ((Lookup == LookupSearchType2.ServiceRegularSaleLookup) || (Lookup == LookupSearchType2.ServiceRegularPurchaseLookup))
                {
                    CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuServiceAdd_Click), 3);
                    buttonContextMenu.Add(ct1);
                }
                else if ((Lookup == LookupSearchType2.ServiceOtherSaleLookup) || (Lookup == LookupSearchType2.ServiceOtherPurchaseLookup))
                {
                    CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuServiceAddOther_Click), 3);
                    buttonContextMenu.Add(ct1);
                }

                CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuSaleHistory", "sale_history", new RoutedEventHandler(mnuServiceHistory_Click1), 4);
                buttonContextMenu.Add(ct2);

                CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuSalePurchase", "purchase_history", new RoutedEventHandler(mnuServiceHistory_Click2), 4);
                buttonContextMenu.Add(ct3);
            }
            else if ((Lookup == LookupSearchType2.InventoryItemLookup) ||
                (Lookup == LookupSearchType2.InventoryBorrowReturnLookup))
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuInventoryAdd_Click), 3);
                buttonContextMenu.Add(ct1);

                CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuSaleHistory", "sale_history", new RoutedEventHandler(mnuInventoryHistory_Click1), 4);
                buttonContextMenu.Add(ct2);

                CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuSalePurchase", "purchase_history", new RoutedEventHandler(mnuInventoryHistory_Click2), 4);
                buttonContextMenu.Add(ct3);

                CCriteriaContextMenu ct4 = new CCriteriaContextMenu("mnuItemCost", "item_cost_balance", new RoutedEventHandler(mnuInventoryHistory_Click3), 5);
                buttonContextMenu.Add(ct4);
            }
            else if ((Lookup == LookupSearchType2.EmployeeLookup) || 
                     (Lookup == LookupSearchType2.EmployeeLookupDaily) ||
                     (Lookup == LookupSearchType2.EmployeeLookupMonthly))
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuEmployeeAdd_Click), 3);
                buttonContextMenu.Add(ct1);
            }
            else if (Lookup == LookupSearchType2.ProjectLookup)
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuProjectAdd_Click), 3);
                buttonContextMenu.Add(ct1);
            }

            createContextMenu(btn, buttonContextMenu);
        }
        #endregion

        private void createContextMenu(Button btn, ArrayList menu)
        {
            btn.ContextMenu.Items.Clear();

            int cnt = 0;
            int prevGroup = -1;
            foreach (CCriteriaContextMenu ct in buttonContextMenu)
            {
                if (cnt == 0)
                {
                    prevGroup = ct.Group;
                }
                else if ((cnt > 0) && (prevGroup != ct.Group))
                {
                    Separator sp = new Separator();
                    btn.ContextMenu.Items.Add(sp);

                    prevGroup = ct.Group;
                }

                cnt++;

                MenuItem mi = new MenuItem();
                mi.Name = ct.Name;
                mi.Click += (RoutedEventHandler)ct.ClickHandler;

                Binding menuItemHeaderBinding = new Binding();
                menuItemHeaderBinding.Source = CTextLabel.Instance;
                menuItemHeaderBinding.Path = new PropertyPath(ct.Caption);
                BindingOperations.SetBinding(mi, MenuItem.HeaderProperty, menuItemHeaderBinding);

                btn.ContextMenu.Items.Add(mi);
            }
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            buttonContextMenu.Clear();
            addDefaultContextMenu(btn);
            addContextMenu(btn);
            createContextMenu(btn, buttonContextMenu);

            btn.ContextMenu.IsOpen = true;
        }

        private void mnuClear_Click(object sender, RoutedEventArgs e)
        {
            SelectedObject = null;
            isEmpty = true;
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            //addContextMenu(cmdSearch);
        }

        private void txtCode_TextSelected(object sender, EventArgs e)
        {
            String code = txtCode.Text.Trim();
            if (!code.Equals(""))
            {
                GetObjectInfo(code);
            }
            else
            {
                isEmpty = true;
            }
        }

        public void SetFocus()
        {
            txtCode.Focus();
        }

        private void rootElement_GotFocus(object sender, RoutedEventArgs e)
        {
            //txtCode.Focus();
        }
    }
}
