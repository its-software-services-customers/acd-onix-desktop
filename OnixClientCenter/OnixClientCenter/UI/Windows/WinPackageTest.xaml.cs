using System;
using System.Windows.Controls;
using System.Windows;
using System.Collections;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Pricing;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Models;
using Onix.ClientCenter.Criteria;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Windows
{
    public partial class WinPackageTest : Window
    {
        private String caption = "";
        private MBillSimulate billSim = null;
        private MCompanyPackage companyPackage = null;
        private MCompanyCommissionProfile companyCommission = null;
        private Boolean isInit = false;
        private Boolean originalState = false;

        private String TypeTest = "";

        public WinPackageTest(MCompanyPackage cp, MBillSimulate sources, String cpt)
        {
            if (sources == null)
            {
                billSim = new MBillSimulate(new CTable(""));

            }
            else
            {
                billSim = sources;
                isInit = true;
            }

            originalState = billSim.IsModified;
            caption = cpt;
            companyPackage = cp;

            DataContext = billSim;
            InitializeComponent();
        }

        public WinPackageTest(MCompanyCommissionProfile cp, MBillSimulate sources, String cpt, String typeTest)
        {
            if (sources == null)
            {
                billSim = new MBillSimulate(new CTable(""));

            }
            else
            {
                billSim = sources;
                isInit = true;
            }

            originalState = billSim.IsModified;
            caption = cpt;
            companyCommission = cp;
            TypeTest = typeTest;

            DataContext = billSim;
            InitializeComponent();
        }

        public Visibility CommissionVisibility
        {
            get
            {
                if (TypeTest.Equals("Commission"))
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public MBillSimulate BillSimulate
        {
            get
            {
                return (billSim);
            }
        }

        public ObservableCollection<CBasketItemDisplay> VoucherItems
        {
            get
            {
                return (billSim.VoucherItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> FreeItems
        {
            get
            {
                return (billSim.FreeItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> PostGiftItems
        {
            get
            {
                return (billSim.PostGiftItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> ResultItems
        {
            get
            {
                return (billSim.ResultItems);
            }
        }

        public ObservableCollection<CProcessingResultGroup> ProcessingTree
        {
            get
            {
                return (billSim.ProcessingTree);
            }
        }


        public ObservableCollection<MCommissionProfileDisplay> CommissionItems
        {
            get
            {
                return (billSim.CommissionItems);
            }
        }

        public String Caption
        {
            get
            {
                return (caption);
            }
        }

        private void addItem()
        {
            MSelectedItem si = new MSelectedItem(new CTable(""));
            si.SelectionType = "2";
            si.ItemObj = null;
            si.EnabledFlag = "Y";
            si.ItemQuantity = "1";

            billSim.AddSelectedItem(si);
            billSim.IsModified = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            billSim.IsModified = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private Boolean loadLastBill()
        {
            String bsimID = CConfig.GetParamValue("LAST_BILL_SIMULATE_ID");
            if (bsimID.Equals(""))
            {
                CConfig.AddParam("LAST_BILL_SIMULATE_ID", "");
                return (false);
            }

            billSim.BillSimulateID = bsimID;
            CUtil.EnableForm(false, this);
            CTable rtn = OnixWebServiceAPI.GetBillSimulateInfo(billSim.GetDbObject());
            CUtil.EnableForm(true, this);
            if (rtn != null)
            {                
                billSim.SetDbObject(rtn);
                billSim.InitSelectedItems();
                billSim.NotifyAllPropertiesChanged();

                return (true);
            }

            billSim.BillSimulateID = "";
            return (false);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            if (!isInit)
            {
                dtmSimulateDate.SelectedDate = DateTime.Now;
                billSim.SimulateTime = DateTime.Now;

                Boolean loaded = loadLastBill();
                if (!loaded)
                {
                    addItem();
                }
            }
            
            billSim.IsModified = originalState;
        }

        private void cbxEnable_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbxEnable_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void txtTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            billSim.IsModified = true;
        }


        private void txtText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void lkupItem_SelectedObjectChanged(object sender, EventArgs e)
        {
            billSim.IsModified = true;
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            addItem();
        }

        private void cmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (TypeTest.Equals("Commission"))
            {
                CalculateCompanyCommissionProfile();
            }
            else
            {
                CalculateCompanyPackage();
            }
        }

        private void CalculateCompanyCommissionProfile()
        {
            CPriceProcessor.SetGetCompanyPackageAllCallback(OnixWebServiceAPI.GetCompanyPackageAll);

            CUtil.EnableForm(false, this);
            CTable t = new CTable("COMPANY_PACKAGE");
            companyPackage = new MCompanyPackage(t);
            companyPackage.CompanyID = "1";
            CTable m = OnixWebServiceAPI.GetCompanyPackageInfo(companyPackage.GetDbObject());
            if (m != null)
            {
                companyPackage.SetDbObject(m);
                companyPackage.InitChildItems();
                companyPackage.NotifyAllPropertiesChanged();
            }
            CPriceProcessor.LoadStandardPackages(companyPackage);
            CUtil.EnableForm(true, this);

            CBasketSet bks = CPriceProcessor.CreateInitialBasketSet(billSim.SelectedItems);

            CUtil.EnableForm(false, this);
            Boolean tmp = billSim.IsModified;

            CBasketSet output = CPriceProcessor.PromotionProcessing(companyPackage, bks, billSim);
            CPriceProcessor.CreateDisplayProcessingTreeView(billSim);
     
            CPriceProcessor.CreateDisplayView(output, billSim, null);
            billSim.NotifyAllPropertiesChanged();

            ArrayList arrBill = ConvertResultItems(billSim.ResultItems);
            ArrayList arrCommission = companyCommission.GetDbObject().GetChildArray("COMPANY_COMM_PROFILE_ITEM");

            CTable oCal = new CTable("");
            oCal.AddChildArray("BILL_LIST", arrBill);
            oCal.AddChildArray("COMMISSION_LIST", arrCommission);
            ArrayList CalComm = OnixWebServiceAPI.CalculateBillCommission(oCal);

            billSim.InitCommissionItems(CalComm);

            //CUtil.LoadMasterRefCombo(cboBranch, true, MasterRefEnum.MASTER_BRANCH, billSim.BranchId);
            billSim.IsModified = tmp;

            CUtil.EnableForm(true, this);

            TabMain.SelectedIndex = 3;
        }

        private ArrayList ConvertResultItems(ObservableCollection<CBasketItemDisplay> results)
        {
            ArrayList headItem = new ArrayList();
            ArrayList resultItem = new ArrayList();
            CTable m = new CTable("");

            foreach (CBasketItemDisplay id in results)
            {
                CTable t = Convert2CTable(id);
                resultItem.Add(t);
            }

            m.AddChildArray("BILL_ITEM", resultItem);
            headItem.Add(m);
            return (headItem);
        }

        private CTable Convert2CTable(CBasketItemDisplay di)
        {
            CTable t = new CTable("");
            t.SetFieldValue("ITEM_ID", di.ItemID);
            t.SetFieldValue("ITEM_CODE", di.ItemCode);
            t.SetFieldValue("ITEM_NAME_THAI", di.ItemName);

            t.SetFieldValue("ITEM_CATERGORY_ID", di.ItemCategoryID);

            t.SetFieldValue("SERVICE_ID", di.ServiceID);
            t.SetFieldValue("SERVICE_CODE", di.ServiceCode);
            t.SetFieldValue("SERVICE_NAME", di.ServiceName);

            t.SetFieldValue("SELECTION_TYPE", di.SelectionType);

            t.SetFieldValue("QUANTITY", di.Quantity.ToString());
            t.SetFieldValue("UNIT_PRICE", CUtil.FormatNumber((di.TotalAmount/di.Quantity).ToString(),"0"));
            t.SetFieldValue("TOTAL_AMOUNT", di.TotalAmount.ToString());
            t.SetFieldValue("DISCOUNT", di.Discount.ToString());
            t.SetFieldValue("PROMOTION_CODE", di.PromotionCode);
            t.SetFieldValue("PROMOTION_NAME", di.PromotionName);
            t.SetFieldValue("SEQUENCE", di.Sequence.ToString());

            return (t);
        }

        private void CalculateCompanyPackage()
        {
            CPriceProcessor.SetGetCompanyPackageAllCallback(OnixWebServiceAPI.GetCompanyPackageAll);

            CUtil.EnableForm(false, this);
            CPriceProcessor.LoadStandardPackages(companyPackage);
            CUtil.EnableForm(true, this);

            CBasketSet bks = CPriceProcessor.CreateInitialBasketSet(billSim.SelectedItems);

            CUtil.EnableForm(false, this);
            Boolean tmp = billSim.IsModified;

            CBasketSet output = CPriceProcessor.PromotionProcessing(companyPackage, bks, billSim);
            CPriceProcessor.CreateDisplayProcessingTreeView(billSim);
            CPriceProcessor.CreateDisplayView(output, billSim, null);
            billSim.NotifyAllPropertiesChanged();


            //CUtil.LoadMasterRefCombo(cboBranch, true, MasterRefEnum.MASTER_BRANCH, billSim.BranchId);
            billSim.IsModified = tmp;

            CUtil.EnableForm(true, this);

            TabMain.SelectedIndex = 2;
        }

        private void UProductSelection_OnChanged(object sender, EventArgs e)
        {
            billSim.IsModified = true;
        }

        private void cmdClear_Click(object sender, RoutedEventArgs e)
        {
            MSelectedItem v = (MSelectedItem)(sender as Button).Tag;
            billSim.DeleteSelectedItem(v);
            billSim.IsModified = true;
        }

        private void trvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
        }

        private void cmdReload_Click(object sender, RoutedEventArgs e)
        {
            Client.Model.MCompanyPackage mpk = new Client.Model.MCompanyPackage(companyPackage.GetDbObject());
            mpk.InitChildItems();

            CUtil.EnableForm(false, this);
            CPriceProcessor.ReloadStandardPackages(mpk);
            CUtil.EnableForm(true, this);
        }

        private Boolean ValidateData()
        {
            Boolean result = false;

            result = CHelper.ValidateLookup(lbCustomerCode, lkupItem, false);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateTextBox(lblDesc, txtDesc, true);
            if (!result)
            {
                return (result);
            }
            result = CHelper.ValidateComboBox(lblBranch, cboBranch, false);
            if (!result)
            {
                return (result);
            }
          
            return (result);
        }

        private Boolean SaveToView()
        {
            if (!ValidateData())
            {
                return (false);
            }

            return (true);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            Boolean result = SaveToView();
            if (!result)
            {
                return;
            }

            String errCd = "";

            CUtil.EnableForm(false, this);
            if (billSim.BillSimulateID.Equals(""))
            {
                //Add mode
                billSim.DocumentType = "1";
                billSim.DocumentStatus = "1";
                billSim.DocumentNo = CUtil.DateTimeToDateStringInternal(DateTime.Now);
                billSim.SimulationFlag = "Y";
                CTable rtn = OnixWebServiceAPI.CreateBillSimulate(billSim.GetDbObject());
                if (rtn != null)
                {
                    billSim.SetDbObject(rtn);
                    billSim.InitSelectedItems();
                    billSim.IsModified = false;
                    CUtil.EnableForm(true, this);

                    CConfig.AddParam("LAST_BILL_SIMULATE_ID", billSim.BillSimulateID);
                    return;
                }

                errCd = "ERROR_USER_ADD";
            }
            else
            {
                //Edit mode
                CTable rtn = OnixWebServiceAPI.UpdateBillSimulate(billSim.GetDbObject());
                if (rtn != null)
                {
                    billSim.IsModified = false;
                    CUtil.EnableForm(true, this);

                    return;
                }

                errCd = "ERROR_USER_EDIT";
            }

            CUtil.EnableForm(true, this);

            //Error here
            CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), errCd, null);
            return;

        }

        private void txtDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            billSim.IsModified = true;
        }

        private void cmdLoad_Click(object sender, RoutedEventArgs e)
        {
            loadLastBill();
            billSim.IsModified = false;
        }

        private void dtmSimulateDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            billSim.IsModified = true;
        }

        private void UTimePicker_OnChanged(object sender, EventArgs e)
        {
            billSim.IsModified = true;
        }

        private void lsvMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void lsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvMain.View as GridView;
            
            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvFree_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvFree.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvVoucher_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvVoucher.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[8] { 0.05, 0.15, 0.35, 0.10, 0.10, 0.10, 0.10, 0.05 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cmdUp_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdDown_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdNew_Click(object sender, RoutedEventArgs e)
        {
            billSim.ClearAll();
            billSim.NotifyAllPropertiesChanged();

            lkupItem.SelectedObject = null;
        }

        private void cmdSearch_Click(object sender, RoutedEventArgs e)
        {
            CCriteriaBillSimulate cr = new CCriteriaBillSimulate();
            cr.SetActionEnable(false);
            cr.Init("");

            WinLookupSearch2 w = new WinLookupSearch2(cr, Caption);
            w.ShowDialog();
            if (w.IsOK)
            {
                MBillSimulate bs = (MBillSimulate) w.ReturnedObj;
                CConfig.AddParam("LAST_BILL_SIMULATE_ID", bs.BillSimulateID);
                loadLastBill();

                billSim.IsModified = false;
            }
        }

        private void lsvPostGift_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvPostGift.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[7] { 0.05, 0.15, 0.40, 0.10, 0.10, 0.10, 0.10 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void lsvCalCommission_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = lsvCalCommission.View as GridView;

            double w = (e.NewSize.Width * 1) - 35;
            double[] ratios = new double[5] { 0.05, 0.25,0.35, 0.15, 0.2};
            //double[] ratios = new double[4] { 0.30, 0.40, 0.15, 0.15 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cboBranch_SelectedObjectChanged(object sender, EventArgs e)
        {
            billSim.IsModified = true;
        }

        private void lkupItem_SelectedObjectChanged_1(object sender, EventArgs e)
        {
            billSim.IsModified = true;
        }

        private void RootElement_Activated(object sender, EventArgs e)
        {
            CUtil.RegisterScreen(this.GetType().ToString());
        }
    }
}
