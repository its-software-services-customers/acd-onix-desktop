using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Controller;
using Wis.WsClientAPI;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Globalization;

namespace Onix.Client.Helper
{
    public delegate Boolean DeleteFunction(CTable o);
    public delegate Boolean DeleteAPIFunction(String apiName, CTable o);
    public delegate Boolean AddFunction(CTable o);
    public delegate Boolean UpdateFunction(CTable o);
    public delegate ArrayList GetListFunction(CTable o);
    public delegate CTable GetInfoFunction(CTable o);
    public delegate void GenericCallback();
    public delegate void GenericAccountDocCallback(MAccountDoc doc, String id, AccountDocumentType dt);
    public delegate Boolean GenericRefTypeFilterCallback(MasterRefEnum rt);
    public delegate Boolean GenericStringTypeFilterCallback(String dt);

    public delegate void LoadReportComboDelegate(ComboBox cbo, String id);
    public delegate void SetupReportComboDelegate(ComboBox cbo);
    public delegate String ObjectToIDDelegate(MBaseModel obj);

    public enum AccountSalePayType
    {
        Cash = 1,
        CashXf = 2,
        CreditCard = 3,
    }

    public enum InventoryDocumentType
    {
        InvDocImport = 1,
        InvDocExport = 2,
        InvDocXfer = 3,
        InvDocAdjust = 4,
        InvDocBorrow = 5,
        InvDocReturn = 6,
    }

    public enum CashDocumentType
    {
        CashDocImport = 1,
        CashDocExport = 2,
        CashDocXfer = 3
    }

    public enum AuxilaryDocumentType
    {
        AuxDocPO = 1,
        AuxDocQuotation = 2,
    }

    public enum PackageType
    {
        PackageItem = 1,
        PackageItemType = 2,
        PackageService = 3,
        PackageAll = 999,
    }

    public enum CommissionType
    {
        ByItem = 1,
        ByGroup = 2,
    }

    public enum InventoryDocumentStatus
    {
        InvDocPending = 1,
        InvDocApproved = 2,
        InvDocCancelApproved = 3
    }

    public enum TaxDocumentType
    {
        TaxDocPP30 = 1,
        TaxDocRev3 = 2,
        TaxDocRev53 = 3,
        TaxDocRev1 = 4,
        TaxDocRev1Kor = 5,
    }

    public enum EmployeeType
    {
        EmployeeDaily = 1,
        EmployeeMonthly = 2,
    }

    public enum PayrollDocType
    {
        PayrollDaily = 1,
        PayrollMonthly = 2,
        PayrollBalanceForward = 3,
    }

    public enum PoDocumentStatus
    {
        PoPending = 1,
        PoApproved = 2,
        PoCancelApproved = 3
    }

    public enum VariableType
    {
        VarNumber = 1,
        VarString = 2,
        VarLangKey = 3,
        VarBinding = 4
    }

    public enum ServiceLevel
    {
        ServiceLevelRegular = 1,
        ServiceLevelOther = 2,
    }

    public enum AccountDocumentType
    {
        /* 1=Sell by cash, 2=Sell by debt, 4=Dr, 3=Cr */
        AcctDocCashSale = 1,
        AcctDocDebtSale = 2,
        AcctDocCrNote = 3,
        AcctDocDrNote = 4,

        AcctDocCashPurchase = 5,
        AcctDocDebtPurchase = 6,
        AcctDocCrNotePurchase = 7,
        AcctDocDrNotePurchase = 8,

        AcctDocArReceipt = 9,
        AcctDocApReceipt = 10,

        AcctDocMiscRevenue = 11,
        AcctDocMiscExpense = 12,

        AcctDocCashDepositAr = 13,
        AcctDocCashDepositAp = 14,

        AcctDocSaleOrder = 15,
        AcctDocBillSummary = 16,
    }

    public enum InputDataType
    {
        InputTypeZeroPossitiveDecimal = 0,
        InputTypeZeroPossitiveInt = 1,
        InputTypeGreaterThanZeroInt = 2
    }

    public enum BasketTypeEnum
    {
        //Keep this order, actual value might be kept in the database

        Available, /* Available : for sorting purpose, put this first */
        FreeVoucher, /* Free not already included */
        FreeAnnonymous, /* Self picking up */
        Used, /* Already used */
        Bundled, /* เหมา */
        Priced, /* คิดราคาแล้ว */
        Discounted, /* คิดราคาแล้ว */
        PricedAndDiscounted, /* Temp */
        PostFree, /* ของแถมท้ายบิล */
        FinalDiscounted, /* ของแถมท้ายบิล */
        AvailableTray, /* Available สำหรับกระบะ */
        PricedTray, /* คิดราคาแล้ว สำหรับกระบะ */
        DiscountedTray, /* คิดราคาแล้ว สำหรับกระบะ */
        FreeAnnonymousTray, /* Self picking up สำหรับกระบะ */
        UsedTray, /* Already used สำหรับกระบะ */
        BundledTray, /* เหมา สำหรับกระบะ */

    }

    public enum DayEnum
    {
        MONDAY_NONE = 0,
        MONDAY_ENUM = 1,
        TUESDAY_ENUM,
        WEDNESDAY_ENUM,
        THURSDAY_ENUM,
        FRIDAY_ENUM,
        SATHURDAY_ENUM,
        SUNDAY_ENUM,
    }

    public enum IntervalTypeEnum
    {
        DAY_ENTIRE = 1,
        DAY_INTERVAL,
    }

    public enum PaperTypeEnum
    {
        PAPER_TYPE_START = PAPER_TYPE_A4,

        PAPER_TYPE_A4 = 1,
        PAPER_TYPE_LETTER,
        PAPER_TYPE_A3,

        PAPER_TYPE_END = PAPER_TYPE_A3,

        PAPER_TYPE_UNDEF = 99999
    }

    public enum MasterRefEnum
    {
        MASTER_REF_START = MASTER_ITEM_TYPE,

        MASTER_ITEM_TYPE = 1,
        MASTER_UOM,
        MASTER_BRAND,
        MASTER_LOCATION_TYPE,
        MASTER_CUSTOMER_TYPE,
        MASTER_CUSTOMER_GROUP,
        MASTER_SERVICE_TYPE,
        MASTER_MEMBER_TYPE,
        MASTER_BANK,
        MASTER_BRANCH,
        MASTER_EMPLOYEE_TYPE,
        MASTER_EMPLOYEE_GROUP,
        MASTER_BARCODE_TYPE,
        MASTER_ADDRESS_TYPE,
        MASTER_SUPPLIER_TYPE,
        MASTER_SUPPLIER_GROUP,
        MASTER_NAME_PREFIX,
        MASTER_PROJECT_GROUP,
        MASTER_VOID_REASON,
        MASTER_BANK_ACCOUNT_TYPE,
        MASTER_CURRENCY,
        MASTER_TERM_OF_PAYMENT,
        MASTER_WH_GROUP,
        MASTER_DISCOUNT_TYPE,
        MASTER_REASON_TYPE,
        MASTER_CREDIT_TERM,

        //Don't forget to check CProductFilter also

        MASTER_REF_END = MASTER_CREDIT_TERM,
        MASTER_REF_UNDEF = 99999
    }

    public enum CashDocumentStatus
    {
        CashDocPending = 1,
        CashDocApproved = 2
    }

    public enum ReportType
    {
        ReportInv = 1,
        ReportSale = 2,
        ReportCash = 3,
        ReportCommission = 4,
        ReportPurchase = 5,
        ReportGeneral = 6,
        ReportHr = 7,
    }

    public enum SelectItemType
    {
        Select_Serveice = 1,
        Select_Item = 2,
    }

    public static class CUtil
    {
        private static ArrayList screens = new ArrayList();
        private static String currentScr = "";

        public static void RegisterScreen(String screenName)
        {
            //screens.Add(screenName);
            currentScr = screenName;
        }

        public static void UnRegisterScreen()
        {
            int last = screens.Count - 1;

            if (last >= 0)
            {
                screens.RemoveAt(last);
            }
        }

        public static String GetCurrentScreen()
        {
            //int last = screens.Count - 1;

            //String scr = "";
            //if (last >= 0)
            //{
            //    scr = (String) screens[last];
            //}

            //return (scr);
            return (currentScr);
        }

        public static DateTime InternalDateToDate(String intDate)
        {
            DateTime myDate = DateTime.MinValue;

            if (intDate.Equals(""))
            {
                return (DateTime.Now);
            }

            try
            {
                myDate = DateTime.ParseExact(intDate.Trim(), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                myDate = DateTime.MinValue;
            }
            return (myDate);
        }

        public static String DateTimeToDateString(DateTime? dt)
        {
            if (dt == null)
            {
                return ("");
            }

            DateTime dt2 = (DateTime)dt;
            string text = dt2.ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return (text);
        }

        public static String DateTimeToDateStringTime(DateTime dt)
        {
            string text = dt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            return (text);
        }

        public static String DateTimeToBEDateString(DateTime? dt)
        {
            if (dt == null)
            {
                return ("");
            }

            DateTime dt2 = (DateTime)dt;
            string text = dt2.ToString("dd/MM/yyyy", new CultureInfo("th-TH"));
            return (text);
        }

        public static String DateTimeToBEDateStringTime(DateTime dt)
        {
            string text = dt.ToString("dd/MM/yyyy HH:mm", new CultureInfo("th-TH"));
            return (text);
        }

        public static String DateTimeToDateStringInternalMin(DateTime? dt)
        {
            if (dt == null)
            {
                return ("");
            }

            DateTime dt2 = (DateTime)dt;

            string text = dt2.ToString("yyyy/MM/dd ", System.Globalization.CultureInfo.InvariantCulture);
            return (text + "00:00:00");
        }

        public static String DateTimeToDateStringInternalMax(DateTime? dt)
        {
            if (dt == null)
            {
                return ("");
            }

            DateTime dt2 = (DateTime)dt;

            string text = dt2.ToString("yyyy/MM/dd ", System.Globalization.CultureInfo.InvariantCulture);
            return (text + "23:59:59");
        }

        public static String DateTimeToDateStringInternal(DateTime? dt)
        {
            string text = string.Empty;
            if (dt != null)
                text = dt?.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
            return (text);
        }

        public static String DayOfWeekToText(int dow)
        {
            String[] days =
            {
                "sunday",
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "sathurday",                
            };

            String dowStr = "N/A";
            if (dow < 7)
            {
                dowStr = CLanguage.getValue(days[dow]);
            }
            
            return (dowStr);
        }

        public static double StringToDouble(String str)
        {
            double d = 0.00;
            if (str == null)
            {
                return (d);
            }
            
            Boolean result = Double.TryParse(str, out d);
            return (d);
        }

        public static int StringToInt(String str)
        {
            int i = 0;
            Boolean result = int.TryParse(str, out i);

            return i;
        }

        public static String CurrencyChequeAmtFmt(String str)
        {
            String tmp1 = FormatNumber(str);
            String tmp2 = tmp1.Replace(",", "");
            int len = tmp2.Length;
            int remain = 12 - len;

            String padStr = "";
            padStr = padStr.PadLeft(remain, '*');

            String nstr = padStr + tmp1;
            return (nstr);
        }

        public static String FormatNumber(String str)
        {
            if (str.Equals(""))
            {
                return ("0.00");
            }

            double d = StringToDouble(str);
            String s = String.Format("{0:n}", d);

            return (s);
        }

        public static String FormatNumberDash(String str)
        {
            if (str.Equals(""))
            {
                return ("-");
            }

            double d = StringToDouble(str);

            if (d == 0.00)
            {
                return ("-");
            }

            String s = String.Format("{0:n}", d);

            return (s);
        }

        public static String FormatNumber(String str, String ifZero)
        {
            double d = StringToDouble(str);
            String s = ifZero;
            if (d != 0.00)
            {
                s = String.Format("{0:n}", d);
            }

            return (s);
        }

        public static String FormatInt(String str)
        {
            if (str.Equals(""))
            {
                return ("0");
            }
           
            int d = int.Parse(str);
            String s = String.Format("{0:d}", d);

            return (s);
        }

        public static String FormatInt32(String str)
        {
            if (str.Equals(""))
            {
                return ("0");
            }
            
            Int32 d = Convert.ToInt32(Math.Round(Convert.ToDouble(str)));
            String s = String.Format("{0:d}", d);

            return (s);
        }

        public static String MasterRefTypeToString(MasterRefEnum dt)
        {
            String[] keys =
                {
                    "NOT USED",
                    "item_type",
                    "item_uom",
                    "item_brand",
                    "location_type",
                    "customer_type",
                    "customer_group",
                    "service_type",
                    "member_type",
                    "Bank",
                    "Branch",
                    "employee_type",
                    "employee_group",
                    "barcode_type",
                    "address_type",
                    "supplier_type",
                    "supplier_group",
                    "name_prefix",
                    "project_group",
                    "void_reason",
                    "bank_account_type",
                    "currency",
                    "payment_term",
                    "master_ref_group",
                    "discount_type",
                    "reason_type",
                    "credit_term",
                };

            String key = keys[(int)dt];
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static void LoadMasterRefType(ComboBox cbo, Boolean allowEmpty, String id, GenericRefTypeFilterCallback required)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = (int)MasterRefEnum.MASTER_REF_START; i <= (int)MasterRefEnum.MASTER_REF_END; i++)
            {
                if (!required((MasterRefEnum) i))
                {
                    continue;
                }

                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = MasterRefTypeToString((MasterRefEnum)i);

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadMasterRefCombo(ComboBox cbo, Boolean allowEmpty, MasterRefEnum type, String id)
        {
            if (!CMasterReference.IsMasterRefLoad(type))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(type);
            int idx = 0;
            cbo.ItemsSource = items;

            foreach (MMasterRef v in items)
            {
                if (v.MasterID.Equals(id))
                {
                    cbo.SelectedIndex = idx;
                    return;
                }

                idx++;
            }
        }

        public static int MasterIDToIndex(ObservableCollection<MMasterRef> items, String id)
        {
            int idx = 0;
            foreach (MMasterRef m in items)
            {
                if (m.MasterID.Equals(id))
                {
                    return (idx);
                }

                idx++;
            }

            return (idx);
        }

        public static int MasterIDToIndex(ObservableCollection<MLocation> items, String id)
        {
            int idx = 0;
            foreach (MLocation m in items)
            {
                if (m.LocationID.Equals(id))
                {
                    return (idx);
                }

                idx++;
            }

            return (idx);
        }

        public static String GetLoadingImagePath()
        {
            String tempPath = System.IO.Path.GetTempPath();
            String imagePath = String.Format(@"{0}\loading.gif", tempPath);
            return (imagePath);
        }

        public static void ExportLoadingImage()
        {
            Uri uri = new Uri("pack://application:,,,/OnixClient;component/Images/loading.gif");
            Stream stream = Application.GetResourceStream(uri).Stream;

            FileStream fileStream = File.Create(GetLoadingImagePath());           
            stream.CopyTo(fileStream);
            fileStream.Close();

            stream.Close();
        }

        public static MCashAccount CashAccountIDToObject(ObservableCollection<MCashAccount> items, String id)
        {
            foreach (MCashAccount m in items)
            {
                if (m.CashAccountID.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MMasterRef MasterIDToObject(ObservableCollection<MMasterRef> items, String id)
        {
            foreach (MMasterRef m in items)
            {
                if (m.MasterID.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MBaseModel IDToObject(IList items, String fldName, String id)
        {
            foreach (MBaseModel m in items)
            {
                String oid = (String)m.GetType().GetProperty(fldName).GetValue(m, null);

                if (oid.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MEntityAddress MasterIDToObject(ObservableCollection<MEntityAddress> items, String id)
        {
            foreach (MEntityAddress m in items)
            {
                if (m.EntityAddressID.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MLocation MasterIDToObject(ObservableCollection<MLocation> items, String id)
        {
            foreach (MLocation m in items)
            {
                if (m.LocationID.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MCashAccount MasterIDToObject(ObservableCollection<MCashAccount> items, String id)
        {
            foreach (MCashAccount m in items)
            {
                if (m.CashAccountID.Equals(id))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static MMasterRef MasterDescToObject(ObservableCollection<MMasterRef> items, String desc)
        {
            foreach (MMasterRef m in items)
            {
                if (m.Description.Equals(desc))
                {
                    return (m);
                }
            }

            return (null);
        }

        public static void InitMasterRef(MasterRefEnum type)
        {
            if (!CMasterReference.IsMasterRefLoad(type))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }
        }

        public static String CashDocStatusToString(CashDocumentStatus dt)
        {
            String key = "";
            if (dt == CashDocumentStatus.CashDocPending)
            {
                key = "INV_DOC_STATUS_PENDING";
            }
            else if (dt == CashDocumentStatus.CashDocApproved)
            {
                key = "INV_DOC_STATUS_APPROVED";
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static void LoadCashDocStatus(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 2; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CashDocStatusToString((CashDocumentStatus)i);

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadLocation(ComboBox cbo, Boolean allowEmpty, String id)
        {
            if (!CMasterReference.IsLocationLoad())
            {
                CMasterReference.LoadLocation(true, null);
            }

            ObservableCollection<MLocation> items = CMasterReference.Instance.Locations;
            int idx = 0;
            foreach (MLocation v in items)
            {
                if (v.LocationID.Equals(id))
                {
                    cbo.SelectedIndex = idx;
                    return;
                }

                idx++;
            }
        }

        public static void LoadCycle(ComboBox cbo, Boolean allowEmpty, String id, String typeFilter)
        {
            if (!CMasterReference.IsCycleLoad())
            {
                CMasterReference.LoadCycle(true, null);
            }
            if (!CMasterReference.IsCycleWeeklyLoad())
            {
                  CMasterReference.LoadCycleWeekly(true, null);
            }
            if (!CMasterReference.IsCycleMonthlyLoad())
            {
                CMasterReference.LoadCycleMonthly(true, null);
            }

            ObservableCollection<MCycle> items;
            if (typeFilter.Equals("1"))
            {
                items = CMasterReference.Instance.Cycle_Weekly;
            }
            else if (typeFilter.Equals("2"))
            {
                items = CMasterReference.Instance.Cycle_Monthly;
            }
            else
            {
                items = CMasterReference.Instance.Cycle;
            }

            cbo.ItemsSource = items;
            int idx = 0;
            foreach (MCycle v in items)
            {
                if (v.CycleID.Equals(id))
                {
                    cbo.SelectedIndex = idx;
                    return;
                }

                idx++;
            }
        }

        public static void LoadCashAccount(ComboBox cbo, Boolean allowEmpty, String id)
        {
            if (!CMasterReference.IsCashAccountLoad())
            {
                CMasterReference.LoadCashAccount(OnixWebServiceAPI.GetCashAccountList);
            }

            ObservableCollection<MCashAccount> items = CMasterReference.Instance.CashAccounts;
            cbo.ItemsSource = items;

            int idx = 0;
            foreach (MCashAccount v in items)
            {
                if (v.CashAccountID.Equals(id))
                {
                    cbo.SelectedIndex = idx;
                    return;
                }

                idx++;
            }            
        }

        public static void LoadItemCategoryPath(ComboBox cbo, Boolean allowEmpty, String id)
        {
            if (!CMasterReference.IsCategoryItemLoaded())
            {
                CMasterReference.LoadItemCategoryPathList(true, null);
            }

            ObservableCollection<MItemCategory> items = CMasterReference.Instance.ItemCategoryPaths;
            int idx = 0;
            foreach (MItemCategory v in items)
            {
                if (v.ItemCategoryID.Equals(id))
                {
                    cbo.SelectedIndex = idx;
                    return;
                }

                idx++;
            }
        }

        public static void EnableForm(Boolean flag, UIElement w)
        {
            w.IsEnabled = flag;
            if (!flag)
            {
                Mouse.OverrideCursor = Cursors.Wait;
            }
            else
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public static String PoStatusToString(PoDocumentStatus dt)
        {
            String str = CLanguage.getValue(dt.ToString());
            return (str);
        }

        public static String GenderToString(String gender)
        {
            String key = "male";
            if (gender.Equals("2"))
            {
                key = "female";
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String EmployeeTypeToString(String type)
        {
            String key = "daily";
            if (type.Equals("2"))
            {
                key = "monthly";
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String InvDocStatusToString(InventoryDocumentStatus dt)
        {
            String key = "";
            if (dt == InventoryDocumentStatus.InvDocPending)
            {
                key = "INV_DOC_STATUS_PENDING";
            }
            else if (dt == InventoryDocumentStatus.InvDocApproved)
            {
                key = "INV_DOC_STATUS_APPROVED";
            }
            else if (dt == InventoryDocumentStatus.InvDocCancelApproved)
            {
                key = "INV_DOC_STATUS_CANCEL_APPROVED";
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String AccountDocTypeToString(AccountDocumentType dt)
        {
            String key = dt.ToString();
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static DateTime? InternalDateToDateOrIsNull(String intDate)
        {
            DateTime myDate = DateTime.MinValue;
            if (intDate.Equals(""))
            {
                return (null);
            }
            else
            {
                try
                {
                    myDate = DateTime.ParseExact(intDate.Trim(), "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    myDate = DateTime.MinValue;
                }
            }
            return (myDate);
        }

        public static void LoadInventoryDocStatus(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            //ArrayList arr = OnixWebServiceAPI.GetLocationList(obj);
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            //foreach (CTable o in arr)
            for (int i = 1; i <= 3; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = InvDocStatusToString((InventoryDocumentStatus)i);

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadComboFromCollection(ComboBox cbo, Boolean allowEmpty, String id, ObservableCollection<MMasterRef> coll)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            foreach (MMasterRef v in coll)
            {
                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadCustomerGroup(ComboBox cbo, Boolean allowEmpty, String id)
        {
            MasterRefEnum type = MasterRefEnum.MASTER_CUSTOMER_GROUP;

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_CUSTOMER_GROUP))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(type);
            cbo.ItemsSource = items;

            //For WinReportParam
            if (!id.Equals(""))
            {
                MMasterRef m = MasterIDToObject(items, id);
                cbo.SelectedItem = m;
            }
        }

        public static void LoadSupplierGroup(ComboBox cbo, Boolean allowEmpty, String id)
        {
            MasterRefEnum type = MasterRefEnum.MASTER_SUPPLIER_GROUP;

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_SUPPLIER_GROUP))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(type);
            cbo.ItemsSource = items;

            //For WinReportParam
            if (!id.Equals(""))
            {
                MMasterRef m = MasterIDToObject(items, id);
                cbo.SelectedItem = m;
            }
        }

        public static void LoadCustomerType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            MasterRefEnum type = MasterRefEnum.MASTER_CUSTOMER_TYPE;

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_CUSTOMER_TYPE))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(type);
            cbo.ItemsSource = items;

            //For WinReportParam
            if (!id.Equals(""))
            {
                MMasterRef m = MasterIDToObject(items, id);
                cbo.SelectedItem = m;
            }
        }

        public static void LoadSupplierType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            MasterRefEnum type = MasterRefEnum.MASTER_SUPPLIER_TYPE;

            if (!CMasterReference.IsMasterRefLoad(MasterRefEnum.MASTER_SUPPLIER_TYPE))
            {
                CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
            }

            ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(type);
            cbo.ItemsSource = items;

            //For WinReportParam
            if (!id.Equals(""))
            {
                MMasterRef m = MasterIDToObject(items, id);
                cbo.SelectedItem = m;
            }
        }

        public static String CommissionProfileToString(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "comm_by_item";
            }
            else if (type.Equals("2"))
            {
                key = "comm_by_group";
            }

            return (CLanguage.getValue(key));
        }

        public static String PosImportStatusToString(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "import_error_open";
            }
            else if (type.Equals("2"))
            {
                key = "import_error_fixed";
            }

            return (CLanguage.getValue(key));
        }

        public static String PackageTypeToString(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "pricing_package";
            }
            else if (type.Equals("2"))
            {
                key = "bonus_package";
            }
            else if (type.Equals("3"))
            {
                key = "discount_package";
            }
            else if (type.Equals("4"))
            {
                key = "voucher_package";
            }
            else if (type.Equals("5"))
            {
                key = "bundle_package";
            }
            else if (type.Equals("6"))
            {
                key = "final_discount_package";
            }
            else if (type.Equals("7"))
            {
                key = "post_gift";
            }
            else if (type.Equals("8"))
            {
                key = "tray_package_price";
            }
            else if (type.Equals("9"))
            {
                key = "tray_package_bonus";
            }
            else if (type.Equals("10"))
            {
                key = "tray_package_bundle"; //แถมในกอง
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String PackageTypeToGroup(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "4";
            }
            else if (type.Equals("2"))
            {
                key = "3";
            }
            else if (type.Equals("3"))
            {
                key = "5";
            }
            else if (type.Equals("4"))
            {
                key = "3";
            }
            else if (type.Equals("5"))
            {
                key = "3";
            }
            else if (type.Equals("6"))
            {
                key = "6";
            }
            else if (type.Equals("7"))
            {
                key = "7";
            }
            else if (type.Equals("8"))
            {
                key = "2";
            }
            else if (type.Equals("9"))
            {
                key = "1";
            }
            else if (type.Equals("10"))
            {
                key = "1"; //แถมในกอง
            }

            return (key);
        }

        public static String CycleTypeToString(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "cycle_weekly";
            }
            else if (type.Equals("2"))
            {
                key = "cycle_monthly";
            }
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String CycleDayWeeklyToString(String type)
        {
            String key = "";
            if (type.Equals("0"))
            {
                key = "sunday";
            }
            else if (type.Equals("1"))
            {
                key = "monday";
            }
            else if (type.Equals("2"))
            {
                key = "tuesday";
            }
            else if (type.Equals("3"))
            {
                key = "wednesday";
            }
            else if (type.Equals("4"))
            {
                key = "thursday";
            }
            else if (type.Equals("5"))
            {
                key = "friday";
            }
            else if (type.Equals("6"))
            {
                key = "sathurday";
            }
          
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String EmployeeCategoryToString(String type)
        {
            String key = "";
            if (type.Equals("1"))
            {
                key = "employee_general";
            }
            else if (type.Equals("2"))
            {
                key = "employee_saleman";
            }
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static String IDToMonth(int id)
        {
            ArrayList temps = new ArrayList { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            String code = (String)temps[id-1];

            return (CLanguage.getValue(code));
        }

        public static String PaymentTypeToString(String type, String Lang)
        {
            String paymentTypeDesc = "";

            if (type.Equals("1"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "Cash");
            }
            else if (type.Equals("2"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "Transfer");
            }
            else if (type.Equals("3"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "CreditCard");
            }
            else if (type.Equals("4"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "Cheque");
            }
            else if (type.Equals("4"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "Cheque");
            }
            else if (type.Equals("5"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "personal_account");
            }
            else if (type.Equals("6"))
            {
                paymentTypeDesc = CLanguage.getValue(Lang, "company_saving_account");
            }

            return (paymentTypeDesc);
        }

        public static String DayEnumToString(DayEnum dt)
        {
            String[] keys =
                {
                    "NOT USED",
                    "monday",
                    "tuesday",
                    "wednesday",
                    "thursday",
                    "friday",
                    "sathurday",
                    "sunday",
                };

            String key = keys[(int)dt];
            String str = CLanguage.getValue(key);

            return (str);
        }


        public static String PaperTypeToString(PaperTypeEnum pt)
        {
            String[] keys =
                {
                    "NOT USED",
                    "a4",
                    "letter",
                    "a3",
                };

            String key = keys[(int)pt];
            String str = CLanguage.getValue(key);

            return (str);
        }

        public static void LoadPaperType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = (int)PaperTypeEnum.PAPER_TYPE_START; i <= (int)PaperTypeEnum.PAPER_TYPE_END; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = PaperTypeToString((PaperTypeEnum)i);

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadPackageType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            //ArrayList arr = OnixWebServiceAPI.GetLocationList(obj);
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 10; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = PackageTypeToString(i.ToString());

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadCommissionProfileType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            //ArrayList arr = OnixWebServiceAPI.GetLocationList(obj);
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 2; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CommissionProfileToString(i.ToString());

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadCycleType(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 2; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CycleTypeToString(i.ToString());

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadCycleDayWeekly(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 0; i <= 6; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CycleDayWeeklyToString(i.ToString());

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadCycleDayMonthly(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 28; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CLanguage.getValue("date") + " " + i.ToString();

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadEmployeeCategory(ComboBox cbo, Boolean allowEmpty, String id)
        {
            CTable obj = new CTable("MASTER_REF");
            List<MMasterRef> items = new List<MMasterRef>();
            int idx = 0;
            int selectedIndex = 0;

            if (allowEmpty)
            {
                MMasterRef v = new MMasterRef(null);
                v.RowIndex = idx;
                items.Add(v);

                idx++;
            }

            for (int i = 1; i <= 2; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = EmployeeCategoryToString(i.ToString());

                v.RowIndex = idx;
                items.Add(v);

                if (v.MasterID.Equals(id))
                {
                    selectedIndex = idx;
                }

                idx++;
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadChunkNavigateCombo(ComboBox cbo, CTable t, String chunkNo)
        {
            int cnt = 0;
            Boolean isOK = int.TryParse(t.GetFieldValue("EXT_CHUNK_COUNT"), out cnt);

            int i;
            int selectedIndex = 0;

            List<MChunkNavigate> items = new List<MChunkNavigate>();
            for (i = 0; i < cnt; i++)
            {
                CTable o = new CTable("CHUNK");
                MChunkNavigate v = new MChunkNavigate(o);
                v.ChunkNo = (i + 1).ToString();

                if (v.ChunkNo.Equals(chunkNo))
                {
                    selectedIndex = i;
                }

                items.Add(v);
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadPageCombo(ComboBox cbo, int pageCount)
        {
            int i;
            int selectedIndex = 0;

            List<MChunkNavigate> items = new List<MChunkNavigate>();
            for (i = 0; i < pageCount; i++)
            {
                CTable o = new CTable("DUMMY");
                MChunkNavigate v = new MChunkNavigate(o);

                v.PageNo = (i + 1).ToString();
                v.ChunkNo = String.Format("{0}", v.PageNo);

                items.Add(v);
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void LoadPageNavigateCombo(ComboBox cbo, int pageCount)
        {
            int i;
            int selectedIndex = 0;

            List<MChunkNavigate> items = new List<MChunkNavigate>();
            for (i = 0; i < pageCount; i++)
            {
                CTable o = new CTable("DUMMY");
                MChunkNavigate v = new MChunkNavigate(o);

                v.PageNo = (i + 1).ToString();
                v.ChunkNo = String.Format("{0}/{1}", v.PageNo, pageCount);

                items.Add(v);
            }

            cbo.ItemsSource = items;
            cbo.SelectedIndex = selectedIndex;
        }

        public static void ResizeGridViewColumns(GridView grdv, double[] ratios, double w)
        {
            for (int i = 0; i < ratios.Length; i++)
            {
                GridViewColumn gvc = grdv.Columns[i];
                gvc.Width = ratios[i] * w;
            }
        }

        public static String SelectTypeToString(SelectItemType dt)
        {
            String key = "";
            if (dt == SelectItemType.Select_Serveice)
            {
                key = "service";
            }
            else if (dt == SelectItemType.Select_Item)
            {
                key = "item";
            }

            String str = CLanguage.getValue(key);

            return (str);
        }

        public static Hashtable CTableArrayToHash(ArrayList arr, String keyField)
        {
            Hashtable hs = new Hashtable();

            foreach (CTable o in arr)
            {
                String key = o.GetFieldValue(keyField);
                hs[key] = o;
            }

            return (hs);
        }

        public static Hashtable ObserableCollectionToHash<T>(ObservableCollection<T> arr, String fieldName)
        {
            Hashtable hs = new Hashtable();

            foreach (T m in arr)
            {
                String key = (String)m.GetType().GetProperty(fieldName).GetValue(m, null);
                hs[key] = m;
            }

            return (hs);
        }

        public static double RoundUp25(double amt)
        {
            Double calMath = amt % 1.00;
            Double calValue = amt;

            if (calMath >= 0.01 && calMath <= 0.25)
            { calValue = Math.Floor(amt) + 0.25; }
            else if (calMath > 0.25 && calMath <= 0.50)
            { calValue = Math.Floor(amt) + 0.50; }
            else if (calMath > 0.50 && calMath <= 0.75)
            { calValue = Math.Floor(amt) + 0.75; }
            else if (calMath > 0.75 && calMath < 1)
            { calValue = Math.Floor(amt) + 1.00; }

            return (calValue);
        }

        public static void AutoUpdateProgram(String caller, String zip)
        {
            //https://103.58.151.73:444/onix/dev/wis/framework/cgi-bin/dispatcher.php

            String updater = "WisAutoUpdate.exe";
            String newUpdater = "Updater.exe";

            if (File.Exists(newUpdater))
            {
                File.Delete(newUpdater);
            }
            File.Copy(updater, newUpdater);

            String url = OnixWebServiceAPI.GetUrl();

            if (url.IndexOf("cgi-bin") > 0)
            {
                url = url.Replace("cgi-bin/dispatcher.php", "install/" + zip);
            }
            else
            {
                url = url.Replace("dispatcher.php", "install/" + zip);
            }
            
            String args = String.Format("{0} {1}", url, caller);

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = newUpdater;
            startInfo.Arguments = args;
            Process.Start(startInfo);
        }

        public static Boolean LockFile(String suffix)
        {
            //String cwd = Directory.GetCurrentDirectory();
            //String fname = String.Format("{0}\\Onix.{1}.lock", cwd, suffix);

            //Mutex m_Mutex = new Mutex(false, @"Global\MyMutex");

            //try
            //{
            //    m_Mutex.WaitOne();
            //}
            //finally
            //{
            //    //m_Mutex.ReleaseMutex();
            //}

            //FileStream fileStream = new FileStream(fname, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            //fileStream.Seek(0, SeekOrigin.Begin);

            //UnicodeEncoding uniEncoding = new UnicodeEncoding();
            //fileStream.Write(uniEncoding.GetBytes("HELLO"), 0, 5);
            //fileStream.Flush();

            //Boolean locked = false;

            //try
            //{
            //    fileStream.Lock(0, 1);

            //    //FileStream s2 = new FileStream(fname, FileMode.Open, FileAccess.Read, FileShare.None);
            //    locked = true;
            //}
            //catch
            //{
            //    locked = false;
            //}

            return (true);
        }

        //Copy from http://programmerdevelop.blogspot.com/2015/03/c.html
        public static String NumberToThaiText(String num)
        {
            string[][] th_num = new string[3][];
            string[] th_digit = { "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };

            th_num[0] = new string[10] { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };
            th_num[1] = new string[2] { "หนึ่ง", "เอ็ด" };
            th_num[2] = new string[2] { "สอง", "ยี่" };

            int ln = num.Length;

            string t = "";
            for (int i = ln; i > 0; i--)
            {
                var x = i - 1;
                var n = Convert.ToInt32(num.Substring(ln - i, 1));
                var digit = x % 6;
                if (n != 0)
                {
                    if (n == 1)
                    {
                        t += (digit == 1) ? "" : th_num[1][digit == 0 ? 1 : 0];
                    }
                    else if (n == 2)
                    {
                        t += th_num[2][digit == 1 ? 1 : 0];
                    }
                    else
                    {
                        t += th_num[0][n];
                    }
                    t += th_digit[(digit == 0 && x > 0 ? 6 : digit)];
                }
                else
                {
                    t += th_digit[digit == 0 && x > 0 ? 6 : 0];
                }
            }

            return t;
        }

        public static String changeNumericToWords(double numb)
        {
            String num = numb.ToString();
            return changeToWords(num, false, "", "");
        }
        public static String changeCurrencyToWords(String numb, String CurrencyStr, String CurrencySubStr)
        {
            return changeToWords(numb, true, CurrencyStr, CurrencySubStr);
        }
        public static String changeCurrencyToWords(String numb)
        {
            return changeToWords(numb, true, "", "");
        }
        public static String changeNumerictoWords(String numb)
        {
            return "(" + changeToWords(numb, false, "", "") + ")";
        }
        public static String changeCurrencyToWords(double numb)
        {
            return changeToWords(numb.ToString(), true, "", "");
        }
        private static String changeToWords(String numb, bool isCurrency, String CurrencyStr, String CurrencySubStr)
        {
            if (CurrencyStr.Equals(""))
            {
                CurrencyStr = "Baht";
            }
            if (CurrencySubStr.Equals(""))
            {
                CurrencySubStr = "Satang";
            }
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = (isCurrency) ? ("") : ("");
            try
            {
                numb = numb.Replace(",", "");
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = (isCurrency) ? (CurrencyStr + " and ") : ("");//just to separate whole numbers from points/cents
                        endStr = (isCurrency) ? (" " + CurrencySubStr) : ("");
                        pointStr = translateCents(points);
                    }
                    else
                    {
                        andStr = (isCurrency) ? (CurrencyStr + " ") : ("");
                    }
                }
                val = String.Format("{0} {1}{2}{3}", translateWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch
            {
                ;
            }
            return val;
        }
        private static String translateWholeNumber(String number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0xx
                bool isDone = false;//test if already translated
                double dblAmt = (Convert.ToDouble(number));
                //if ((dblAmt > 0) && number.StartsWith("0"))
                if (dblAmt > 0)
                {
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0; //store digit grouping
                    String place = "";//digit grouping name:hundreds,thousand, etc...
                    switch (numDigits)
                    {
                        case 1://ones' range
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2://tens' range
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3://hundreds' range
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            if (beginsZero)
                            {
                                place = " ";
                            }
                            break;
                        case 4://thousands' range
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions' range
                            pos = (numDigits % 10) + 1;
                            break;
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        //if translation is not done, continue...(recursion comes in now!!)
                        word = translateWholeNumber(number.Substring(0, pos)) + place + translateWholeNumber(number.Substring(pos));
                        // check for trailing zeros
                        //if (beginsZero) word = " and " + word.Trim();
                    }
                }
            }
            catch
            {
                ;
            }
            return word.Trim();
        }
        private static String tens(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = null;
            switch (digt)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Forty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (digt > 0)
                    {
                        name = tens(digit.Substring(0, 1) + "0") + ones(digit.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ones(String digit)
        {
            int digt = Convert.ToInt32(digit);
            String name = "";
            switch (digt)
            {
                case 1:
                    name = " One";
                    break;
                case 2:
                    name = " Two";
                    break;
                case 3:
                    name = " Three";
                    break;
                case 4:
                    name = " Four";
                    break;
                case 5:
                    name = " Five";
                    break;
                case 6:
                    name = " Six";
                    break;
                case 7:
                    name = " Seven";
                    break;
                case 8:
                    name = " Eight";
                    break;
                case 9:
                    name = " Nine";
                    break;
            }
            return name;
        }
        private static String translateCents(String cents)
        {
            String cts = "";
            /*for (int i = 0; i < cents.Length; i++)
            {
            digit = cents[i].ToString();
            if (digit == "0")
            {
            engOne = "Zero";
            }
            else
            {
            engOne = ones(digit);
            //engOne = tens(digit);
            }
            cts = cts + " " + engOne;
            }/**/
            //MessageBox.Show(cents[0].ToString() + " " + cents[1].ToString());
            if (cents[0].ToString().Equals("0"))
            {
                cts = "" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("2"))
            {
                cts = "Twenty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("3"))
            {
                cts = "Thirty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("4"))
            {
                cts = "Forty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("5"))
            {
                cts = "Fifty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("6"))
            {
                cts = "Sixty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("7"))
            {
                cts = "Seventy" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("8"))
            {
                cts = "Eighty" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents[0].ToString().Equals("9"))
            {
                cts = "Ninety" + ones(cents[1].ToString());
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents.Equals("11"))
            {
                cts = "Eleven";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents.Equals("12"))
            {
                cts = "Twelve";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents.Equals("13"))
            {
                cts = "Thirteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "14")
            {
                cts = "Fourteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "15")
            {
                cts = "Fifteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "16")
            {
                cts = "Sixteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "17")
            {
                cts = "Seventeen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "18")
            {
                cts = "Eighteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            if (cents == "19")
            {
                cts = "Nineteen";
                //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            }
            //MessageBox.Show(" cents " + cts + " " + cents[0].ToString() + cents[1].ToString());
            return cts;
        }

        public static String CurrencyToThaiText(String num)
        {
            String tmp = CBahtText.ToBahtText(CUtil.StringToDouble(num));
            if (tmp.Equals(""))
            {
                return (tmp);
            }

            return "(" + tmp + ")";
        }

        public static String CurrencyToThaiText(String num, String CurrencyStr, String CurrencySubStr)
        {
            if (CurrencyStr.Equals(""))
            {
                CurrencyStr = "บาท";
            }

            if (CurrencySubStr.Equals(""))
            {
                CurrencySubStr = "สตางค์";
            }

            if (num.Equals(""))
            {
                return ("");
            }

            String tmp = CBahtText.ToBahtText(CUtil.StringToDouble(num));
            tmp = tmp.Replace("บาท", CurrencyStr);
            tmp = tmp.Replace("สตางค์", CurrencySubStr);

            return "(" + tmp + ")";
        }

        public static double Cm2Dot(String cm)
        {
            double cm2inch = 0.393701;
            double inch2dot = 96;

            double dot = CUtil.StringToDouble(cm) * cm2inch * inch2dot;
            return (dot);
        }

        public static IList GetCollectionByID(IList sources, String fieldName, String id)
        {
            IList temp = new ObservableCollection<MBaseModel>();

            foreach (MBaseModel m in sources)
            {
                String oid = (String)m.GetType().GetProperty(fieldName).GetValue(m, null);

                if (oid.Equals(id))
                {
                    temp.Add(m);
                }
            }

            return (temp);
        }

        public static SolidColorBrush DocStatusToColor(String status)
        {
            if (status.Equals("3"))
            {
                return (new SolidColorBrush(Colors.Red));
            }
            else if (status.Equals("2"))
            {
                return (new SolidColorBrush(Colors.Black));
            }

            return (new SolidColorBrush(Colors.Blue));
        }

        public static BitmapImage GetBitmapFromUrl(String url)
        {
            String urlImg = url;
            Uri uriResult;

            Uri.TryCreate(urlImg, UriKind.Absolute, out uriResult);
            bool result = (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if (result)
            {
                BitmapImage b = new BitmapImage();
                b.BeginInit();
                b.UriSource = new Uri(urlImg);
                b.EndInit();
                return (b);
            }

            return (null);
        }

        public static void GenerateWhDocNumber(MAccountDoc doc)
        {
            if (!doc.RefWhDocNo.Equals(""))
            {
                return;
            }

            if (!doc.DocumentStatus.Equals("1"))
            {
                return;
            }

            String docDate = doc.GetDbObject().GetFieldValue("DOCUMENT_DATE");

            CTable t = new CTable("");
            t.SetFieldValue("SEQ_DOC_TYPE", "WH_DOC_NUMBER");
            t.SetFieldValue("DOCUMENT_DATE", docDate);
            
            CTable d = OnixWebServiceAPI.GenerateCustomDocumentNumber(t);
            String docNo = d.GetFieldValue("LAST_DOCUMENT_NO");

            doc.RefWhDocNo = docNo;
        }


        public static String TaxDocTypeToString(TaxDocumentType dt)
        {
            String tmp = "";
            if (dt == TaxDocumentType.TaxDocPP30)
            {
                tmp = CLanguage.getValue("tax_pp30");
            }
            else if (dt == TaxDocumentType.TaxDocRev3)
            {
                tmp = CLanguage.getValue("rv_tax_3");
            }
            else if (dt == TaxDocumentType.TaxDocRev53)
            {
                tmp = CLanguage.getValue("rv_tax_53");
            }
            else if (dt == TaxDocumentType.TaxDocRev1)
            {
                tmp = CLanguage.getValue("rv_tax_1");
            }
            else if (dt == TaxDocumentType.TaxDocRev1Kor)
            {
                tmp = CLanguage.getValue("rv_tax_1_kor");
            }

            return (tmp);
        }


        public static String EmployeeTypeToString(EmployeeType et)
        {
            String tmp = "";
            if (et == EmployeeType.EmployeeDaily)
            {
                tmp = CLanguage.getValue("daily");
            }
            else if (et == EmployeeType.EmployeeMonthly)
            {
                tmp = CLanguage.getValue("monthly");
            }

            return (tmp);
        }

        public static double CmToDot(String cm)
        {
            double width = CUtil.StringToDouble(cm);
            double dot = 0.393 * 96 * width;
            return (dot);
        }

        public static String PayrollDocTypeToString(PayrollDocType et)
        {
            String tmp = "";
            if (et == PayrollDocType.PayrollDaily)
            {
                tmp = CLanguage.getValue("daily");
            }
            else if (et == PayrollDocType.PayrollMonthly)
            {
                tmp = CLanguage.getValue("monthly");
            }
            else if (et == PayrollDocType.PayrollBalanceForward)
            {
                tmp = CLanguage.getValue("balance_document");
            }

            return (tmp);
        }

        public static String DocumentTypeToText(string dt)
        {
            string DocTypeStr = "";
            //Please implement this function
            if (dt == "1")
                DocTypeStr = "ขายสด";
            else if (dt == "2")
                DocTypeStr = "ขายเชื่อ";
            else if (dt == "3")
                DocTypeStr = "ลดหนี้ขาย";
            else if (dt == "4")
                DocTypeStr = "เพิ่มหนี้ขาย";
            else if (dt == "5")
                DocTypeStr = "ซื้อสด";
            else if (dt == "6")
                DocTypeStr = "ซื้อเชื่อ";
            else if (dt == "7")
                DocTypeStr = "ลดหนี้ซื้อ";
            else if (dt == "8")
                DocTypeStr = "เพิ่มหนี้ซื้อ";

            //AcctDocCashSale = ขายสด,
            //AcctDocDebtSale = ขายเชื่อ,
            //AcctDocCrNote = ลดหนี้ขาย,
            //AcctDocDrNote = เพิ่มหนี้ขาย,

            //AcctDocCashPurchase = ซื้อสด,
            //AcctDocDebtPurchase = ซื้อเชื่อ,
            //AcctDocCrNotePurchase = ลดหนี้ซื้อ,
            //AcctDocDrNotePurchase = เพิ่มหนี้ซื้อ,

            //AcctDocArReceipt = ชำระลูกหนี้,
            //AcctDocApReceipt = ชำระเจ้าหนี้,

            return (DocTypeStr);
        }
    }
}
