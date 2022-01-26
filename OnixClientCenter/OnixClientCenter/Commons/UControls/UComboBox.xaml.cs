using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.ClientCenter.Windows;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.UI.Inventory.Location;

namespace Onix.ClientCenter.Commons.UControls
{
    public enum ComboLoadType
    {
        CustomerTypeCombo = 1,
        CustomerGroupCombo = 2,
        NamePrefixCombo = 3,
        AddressTypeCombo = 4,
        BranchCombo = 5,
        SupplierTypeCombo = 6,
        SupplierGroupCombo = 7,
        ServiceTypeCombo = 8,
        ItemTypeCombo = 9,
        UomCombo = 10,
        EmployeeGroupCombo = 11,
        EmployeeTypeCombo = 12,
        CycleCombo = 13,
        BrandCombo = 14,
        LocationTypeCombo = 15,
        BarcodeTypeCombo = 16,
        LocationCombo = 17,
        ProjectGroupCombo = 18,
        VoidReasonCombo = 19,
        BankCombo = 20,
        BankAccountTypeCombo = 21,
        CurrencyCombo = 22,
        PaymentTermCombo = 23,
        WhGroupCombo = 24,
        ReasonTypeCombo = 25,
        CreditTermCombo = 26,
    }

    public class CComboConfig
    {
        public String ScreenCriteriaClassName { get; set; }
        public String Type { get; set; }
    }

    public partial class UComboBox : UserControl
    {
        private ArrayList buttonContextMenu = new ArrayList();
        private Hashtable comboTypeConfigs = new Hashtable();

        public static readonly DependencyProperty ComboLoadTypeProperty =
        DependencyProperty.Register("ComboLoadType", typeof(ComboLoadType), typeof(UComboBox),
            new FrameworkPropertyMetadata(ComboLoadType.CustomerTypeCombo, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnComboLoadTypePropertyChanged)));

        public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register("Caption", typeof(String), typeof(UComboBox),
            new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnCaptionPropertyChanged)));

        public static readonly DependencyProperty IDFieldNameProperty =
        DependencyProperty.Register("IDFieldName", typeof(String), typeof(UComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                new PropertyChangedCallback(OnIDFieldNamePropertyChanged)));

        //Use IList is better than using ObservableCollection
        public static readonly DependencyProperty ItemSourcesProperty =
        DependencyProperty.Register("ItemSources", typeof(IList), typeof(UComboBox),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemSourcesPropertyChanged)));

        public static readonly DependencyProperty DisplayMemberPathProperty =
        DependencyProperty.Register("DisplayMemberPath", typeof(String), typeof(UComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, 
                new PropertyChangedCallback(OnDisplayMemberPathPropertyChanged)));

        public static readonly DependencyProperty SelectedObjectProperty =
        DependencyProperty.Register("SelectedObject", typeof(MBaseModel), typeof(UComboBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                new PropertyChangedCallback(OnSelectedObjectPropertyChanged)));

        public event EventHandler SelectedObjectChanged;

        private MBaseModel selectedObject = null;
        private Boolean internalFlag = false;

#region Dependency Property
        public ComboLoadType ComboLoadType
        {
            get { return (ComboLoadType) GetValue(ComboLoadTypeProperty); }
            set { SetValue(ComboLoadTypeProperty, value); }
        }

        public String Caption
        {
            get { return (String) GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        public MBaseModel SelectedObject
        {
            get { return (MBaseModel)GetValue(SelectedObjectProperty); }
            set { SetValue(SelectedObjectProperty, value); }
        }

        public IList ItemSources
        {
            get { return (IList)GetValue(ItemSourcesProperty); }
            set { SetValue(ItemSourcesProperty, value); }
        }

        public String IDFieldName
        {
            get { return (String) GetValue(IDFieldNameProperty); }
            set { SetValue(IDFieldNameProperty, value); }
        }

        public String DisplayMemberPath
        {
            get { return (String)GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        #endregion

        public Boolean IsEmpty()
        {
            if (cboGeneric.SelectedValue == null)
            {
                return (true);
            }

            if (cboGeneric.Text == "")
            {
                return (true);
            }

            return (false);
        }

        #region Event Handler
        private static void OnComboLoadTypePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            control.ComboLoadType = (ComboLoadType)e.NewValue;
        }

        private static MBaseModel findObject(MBaseModel m, IList colls, String fldName)
        {
            if (m == null)
            {
                return(null);
            }

            if (fldName == null)
            {
                return (null);
            }

            if (colls == null)
            {
                return (null);
            }

            String id = (String) m.GetType().GetProperty(fldName).GetValue(m, null);
            foreach (MBaseModel o in colls)
            {
                String searchID = (String)o.GetType().GetProperty(fldName).GetValue(o, null);

                if (id.Equals(searchID))
                {
                    return (o);
                }
            }

            return (null);
        }

        private static void OnSelectedObjectPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            MBaseModel m = (MBaseModel)e.NewValue;

            if (control.internalFlag)
            {
                return;
            }

            control.selectedObject = m;
            updateGui(control);
        }

        private static void OnDisplayMemberPathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            control.cboGeneric.DisplayMemberPath = (String)e.NewValue;
        }

        private static void OnItemSourcesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            control.ItemSources = (IList)e.NewValue;
            control.cboGeneric.ItemsSource = control.ItemSources;

            updateGui(control);
        }

        private static void updateGui(UComboBox control)
        {
            if (control.ItemSources == null)
            {
                return;
            }

            if (control.selectedObject == null)
            {
                return;
            }

            if ((control.IDFieldName == null) || (control.IDFieldName.Equals("")))
            {
                return;
            }

            control.internalFlag = true;
            MBaseModel n = findObject(control.selectedObject, control.ItemSources, control.IDFieldName);
            if (n == null)
            {
                return;
            }

            if (!n.Equals(control.SelectedObject))
            {
                //WinAddEditInventoryItem , please regression test with this Window
                control.SelectedObjectChanged(control, null);
            }

            control.SelectedObject = n;
            control.cboGeneric.SelectedItem = n;
            control.internalFlag = false;            
        }

        private static void OnIDFieldNamePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            control.IDFieldName = (String)e.NewValue;

            updateGui(control);
        }

        private static void OnCaptionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            UComboBox control = sender as UComboBox;
            control.Caption = (String)e.NewValue;
        }
#endregion

        public UComboBox()
        {
            addComboTypeConfig(ComboLoadType.CustomerTypeCombo, "CCriteriaMasterRef", ((int) MasterRefEnum.MASTER_CUSTOMER_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.CustomerGroupCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_CUSTOMER_GROUP).ToString());

            addComboTypeConfig(ComboLoadType.AddressTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_ADDRESS_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.BranchCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_BRANCH).ToString());
            addComboTypeConfig(ComboLoadType.NamePrefixCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_NAME_PREFIX).ToString());
            addComboTypeConfig(ComboLoadType.ServiceTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_SERVICE_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.ItemTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_ITEM_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.UomCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_UOM).ToString());
            addComboTypeConfig(ComboLoadType.EmployeeTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_EMPLOYEE_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.EmployeeGroupCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_EMPLOYEE_GROUP).ToString());
            addComboTypeConfig(ComboLoadType.CycleCombo, "CCriteriaCycle", "");
            addComboTypeConfig(ComboLoadType.LocationCombo, "CCriteriaLocation", "");
            addComboTypeConfig(ComboLoadType.BrandCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_BRAND).ToString());
            addComboTypeConfig(ComboLoadType.LocationTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_LOCATION_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.BarcodeTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_BARCODE_TYPE).ToString());

            addComboTypeConfig(ComboLoadType.SupplierTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_SUPPLIER_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.SupplierGroupCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_SUPPLIER_GROUP).ToString());
            addComboTypeConfig(ComboLoadType.ProjectGroupCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_PROJECT_GROUP).ToString());
            addComboTypeConfig(ComboLoadType.VoidReasonCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_VOID_REASON).ToString());
            addComboTypeConfig(ComboLoadType.BankCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_BANK).ToString());
            addComboTypeConfig(ComboLoadType.BankAccountTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_BANK_ACCOUNT_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.CurrencyCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_CURRENCY).ToString());
            addComboTypeConfig(ComboLoadType.PaymentTermCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_TERM_OF_PAYMENT).ToString());
            addComboTypeConfig(ComboLoadType.WhGroupCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_WH_GROUP).ToString());
            addComboTypeConfig(ComboLoadType.ReasonTypeCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_REASON_TYPE).ToString());
            addComboTypeConfig(ComboLoadType.CreditTermCombo, "CCriteriaMasterRef", ((int)MasterRefEnum.MASTER_CREDIT_TERM).ToString());

            InitializeComponent();
        }

        public ComboBox GenericComboBox
        {
            get
            {
                return (cboGeneric);
            }
        }

        private void cboGeneric_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = (sender as ComboBox);
            this.selectedObject = (MBaseModel) cbo.SelectedItem;
            updateGui(this);
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.ContextMenu.IsOpen = true;
        }

        private void createContextMenu(Button btn, ArrayList menu)
        {
            int cnt = 0;
            int prevGroup = -1;
            foreach (CCriteriaContextMenu ct in menu)
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

        protected static Boolean showWindow(CWinLoadParam param, String className)
        {
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
            return (cr.IsOKClick);
        }

        private void mnuMasterRefAdd_Click(object sender, RoutedEventArgs e)
        {
            CComboConfig cfg = (CComboConfig)comboTypeConfigs[ComboLoadType];
            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();
            MasterRefEnum rt = (MasterRefEnum)int.Parse(cfg.Type);

            //WinAddEditMasterRef w = new WinAddEditMasterRef(rt);
            //w.Caption = (String)(sender as MenuItem).Header + " " + Caption;
            //w.SetMasterRefType(rt);
            //w.Mode = "A";
            //w.ParentItemSource = arr;
            //w.ShowDialog();

            String caption = CLanguage.getValue("add") + " " + Caption;
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = cfg.Type;
            param.Mode = "A";
            param.ParentItemSources = arr;
            Boolean isOK = FactoryWindow.ShowWindow("WinAddEditMasterRef", param);

            if (isOK)
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetAllMasterRefList, rt);
            }
        }

        private void mnuCycleAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_CYCLE_ADD"))
            {
                return;
            }

            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            WinAddEditCycle w = new WinAddEditCycle("A", null, arr);
            w.Title = (String)(sender as MenuItem).Header + " " + CLanguage.getValue("cycle");
            w.ShowDialog();

            if (w.DialogOK.Equals(true))
            {
                CMasterReference.LoadCycle(true, new ArrayList());
            }
        }

        private void mnuLocationAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_LOCATION_ADD"))
            {
                return;
            }

            ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("location");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.Mode = "A";
            param.ParentItemSources = arr;
            Boolean refresh = FactoryWindow.ShowWindow("WinAddEditLocation", param);
            if (refresh)
            {
                CMasterReference.LoadLocation(true, null);
            }                        
        }

        private void addContextMenu(Button btn)
        {
            btn.ContextMenu.Items.Clear();
            buttonContextMenu.Clear();

            CCriteriaContextMenu ct0 = new CCriteriaContextMenu("mnuSearch", "search", new RoutedEventHandler(mnuSearch_Click), 0);
            buttonContextMenu.Add(ct0);

            if (ComboLoadType == ComboLoadType.CycleCombo)
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuCycleAdd_Click), 1);
                buttonContextMenu.Add(ct1);
            }
            else if (ComboLoadType == ComboLoadType.LocationCombo)
            {
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuLocationAdd_Click), 1);
                buttonContextMenu.Add(ct1);
            }
            else
            { 
                //Master ref
                CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "add", new RoutedEventHandler(mnuMasterRefAdd_Click), 1);
                buttonContextMenu.Add(ct1);
            }

            createContextMenu(btn, buttonContextMenu);
        }

        private void rootElement_Loaded(object sender, RoutedEventArgs e)
        {
            addContextMenu(cmdSearch);
        }

        private void addComboTypeConfig(ComboLoadType ltype, String crName, String type)
        {
            CComboConfig cfg = new CComboConfig();

            cfg.ScreenCriteriaClassName = FactoryCriteria.GetFqdnClassName(crName); //"Onix.ClientCenter.Criteria." + crName;
            cfg.Type = type;

            comboTypeConfigs.Add(ltype, cfg);
        }

        private void criteriaInitFunc(CCriteriaBase cb)
        {
            CComboConfig cfg = (CComboConfig)comboTypeConfigs[ComboLoadType];
            CCriteriaBase cr = (CCriteriaBase)Activator.CreateInstance(Type.GetType(cfg.ScreenCriteriaClassName));

            cb.Init(cfg.Type);
        }

        private void mnuSearch_Click(object sender, RoutedEventArgs e)
        {
            CComboConfig cfg = (CComboConfig)comboTypeConfigs[ComboLoadType];
            CCriteriaBase cr = (CCriteriaBase)Activator.CreateInstance(Type.GetType(cfg.ScreenCriteriaClassName));
            cr.SetActionEnable(false);
            criteriaInitFunc(cr);

            WinLookupSearch2 w = new WinLookupSearch2(cr, Caption);
            w.ShowDialog();

            if (w.IsOK)
            {
                this.selectedObject = w.ReturnedObj;
                updateGui(this);
            }
        }

        private void mnuClear_Click(object sender, RoutedEventArgs e)
        {
            SelectedObject = null;
        }
    }
}
