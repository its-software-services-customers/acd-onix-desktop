using System;
using System.ComponentModel;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;

namespace Onix.Client.Helper
{
    public class CMasterReference : INotifyPropertyChanged
    {
        private static CMasterReference obj = new CMasterReference();

        private static ObservableCollection<MMasterRef> xferTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> refundStatus = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> vatReportTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> products = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> stages = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> roles = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> vmSpects = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> otMultipliers = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> payrollExpenseTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> payrollDeductionTypes = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<MLocation> borrowReturnlocations = new ObservableCollection<MLocation>();
        private static ObservableCollection<MItemCategory> itemCategories = new ObservableCollection<MItemCategory>();
        private static ObservableCollection<MLocation> locations = new ObservableCollection<MLocation>();
        private static ObservableCollection<MCashAccount> cashAccounts = new ObservableCollection<MCashAccount>();
        private static ObservableCollection<MCycle> cycle = new ObservableCollection<MCycle>();
        private static ObservableCollection<MCycle> cycle_weekly = new ObservableCollection<MCycle>();
        private static ObservableCollection<MCycle> cycle_monthly = new ObservableCollection<MCycle>();
        private static ObservableCollection<MMasterRef> cycleTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> months = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<MMasterRef> serviceCategories = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> posImportStatus = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> poPayments = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> purchaseExpenseDocTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> salePaymentDocTypes = new ObservableCollection<MMasterRef>();        
        private static ObservableCollection<MMasterRef> purchaseWhDocTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> purchasePaymentDocTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> leaveDurations = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<MMasterRef> saleRevenueDocTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> revenueTaxTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> taxDocTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> employeeTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> payrollDocTypes = new ObservableCollection<MMasterRef>();

        private static MCompanyPackage companyPackage = null;

        private static ObservableCollection<String> productSelectType = new ObservableCollection<String>();
        private static ObservableCollection<String> intervalOutputTypes = new ObservableCollection<String>();
        private static ObservableCollection<String> customerSelectTypes = new ObservableCollection<String>();
        private static ObservableCollection<MMasterRef> productSpecificSelectType = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> productUnSpecificSelectType = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> productMiscUnSpecificSelectType = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<String> discountMappingTypes = new ObservableCollection<String>();
        private static ObservableCollection<String> discountOutputTypes = new ObservableCollection<String>();

        private static ObservableCollection<String> voucherSelectFreeTypes = new ObservableCollection<String>();
        private static ObservableCollection<String> finalDiscountBasketTypes = new ObservableCollection<String>();

        private static ObservableCollection<MMasterRef> accountSalePayTypes = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<String> lookupSelectType = new ObservableCollection<String>();

        private static ObservableCollection<String> booleanType = new ObservableCollection<String>();
        private static ObservableCollection<MMasterRef> packageTypes = new ObservableCollection<MMasterRef>();

        private static MItemCategory categoryRootNode = new MItemCategory(new CTable(""));
        private static ObservableCollection<MMasterRef> docStatuses = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> poStatuses = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> quotationTypes = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> variableTypes = new ObservableCollection<MMasterRef>();

        private static ObservableCollection<MUserGroup> userGroups = new ObservableCollection<MUserGroup>();
        private static ObservableCollection<MMasterRef> employeeDepartments = new ObservableCollection<MMasterRef>();
        private static ObservableCollection<MMasterRef> employeePositions = new ObservableCollection<MMasterRef>();

        private static MCompanyProfile company = null;

        private static Boolean isCategoryITemLoaded = false;
        private static Boolean isMasterRefLoaded = false;
        private static Boolean isLocationLoaded = false;
        private static Boolean isCashAccountLoaded = false;
        private static Boolean isCycleLoaded = false;
        private static Boolean isCycleWeeklyLoaded = false;
        private static Boolean isCycleMonthlyLoaded = false;
        private static Boolean isPosImportStatusLoaded = false;
        private static Boolean isPackageTypeLoaded = false;

        private static Hashtable collHash = new Hashtable();
        private static Hashtable collHashBase = new Hashtable();

        public event PropertyChangedEventHandler PropertyChanged;

        public CMasterReference()
        {

        }

        public static CMasterReference Instance
        {
            get
            {
                return (obj);
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void removeAllCategoryItems(ObservableCollection<MItemCategory> items)
        {
            ArrayList temps = new ArrayList();
            foreach (MItemCategory v in items)
            {
                temps.Add(v);
            }

            foreach (MItemCategory v in temps)
            {
                items.Remove(v);
            }
        }

        private static void removeItems(ObservableCollection<MMasterRef> items)
        {
            ArrayList temps = new ArrayList();
            foreach (MMasterRef v in items)
            {
                temps.Add(v);
            }

            foreach (MMasterRef v in temps)
            {
                items.Remove(v);
            }
        }

        private static void removeAllLocationItems(ObservableCollection<MLocation> items)
        {
            ArrayList temps = new ArrayList();
            foreach (MLocation v in items)
            {
                temps.Add(v);
            }

            foreach (MLocation v in temps)
            {
                items.Remove(v);
            }
        }

        private static void removeAllCycleItems(ObservableCollection<MCycle> items)
        {
            ArrayList temps = new ArrayList();
            foreach (MCycle v in items)
            {
                temps.Add(v);
            }

            foreach (MCycle v in temps)
            {
                items.Remove(v);
            }
        }

        private static void removeAllCashAccountItems(ObservableCollection<MCashAccount> items)
        {
            ArrayList temps = new ArrayList();
            foreach (MCashAccount v in items)
            {
                temps.Add(v);
            }

            foreach (MCashAccount v in temps)
            {
                items.Remove(v);
            }
        }

        private ObservableCollection<MMasterRef> getMasterRefCollection(MasterRefEnum mtype)
        {
            ObservableCollection<MMasterRef> coll = (ObservableCollection<MMasterRef>)collHash[mtype];

            if (coll == null)
            {
                coll = new ObservableCollection<MMasterRef>();
                collHash[mtype] = coll;
            }

            return (coll);
        }

        #region Item Category
        public static void LoadItemCategoryPathList(Boolean allowEmpty, ArrayList parray)
        {
            removeAllCategoryItems(itemCategories);

            CTable t = new CTable("");
            ArrayList arr = null;

            if (parray != null)
            {
                arr = parray;
            }
            else
            {
                arr = OnixWebServiceAPI.GetItemCategoryPathList(t);
            }

            MItemCategory c = new MItemCategory(new CTable(""));
            itemCategories.Add(c);

            foreach (CTable o in arr)
            {
                MItemCategory v = new MItemCategory(o);
                itemCategories.Add(v);
            }

            isCategoryITemLoaded = true;
        }

        public static Boolean IsCategoryItemLoaded()
        {
            return (isCategoryITemLoaded);
        }

        #endregion

        #region Company Package
        public static MCompanyPackage GetCompanyPackage(Boolean reloadFlag)
        {
            if ((reloadFlag) || (companyPackage == null))
            {
                CTable o = OnixWebServiceAPI.GetCompanyPackageInfo(new CTable(""));
                if (companyPackage == null)
                {
                    companyPackage = new MCompanyPackage(o);
                }
                else
                {
                    companyPackage.SetDbObject(o);
                }

                companyPackage.InitChildItems();
            }

            return (companyPackage);
        }

        public static Boolean IsCompanyPackageLoad()
        {
            return (companyPackage != null);
        }
        #endregion

        #region Cash Accounts
        public static void LoadCashAccount(GetListFunction listFunc)
        {
            removeAllCashAccountItems(cashAccounts);

            CTable t = new CTable("");
            ArrayList arr = null;

            //arr = OnixWebServiceAPI.GetCashAccountList(t);
            arr = listFunc(t);

            MCashAccount c = new MCashAccount(new CTable(""));
            cashAccounts.Add(c);

            foreach (CTable o in arr)
            {
                MCashAccount v = new MCashAccount(o);
                cashAccounts.Add(v);
            }

            isCashAccountLoaded = true;
        }

        public static void LoadCashAccount()
        {
            removeAllCashAccountItems(cashAccounts);

            CTable t = new CTable("");
            ArrayList arr = null;

            arr = OnixWebServiceAPI.GetCashAccountList(t);

            MCashAccount c = new MCashAccount(new CTable(""));
            cashAccounts.Add(c);

            foreach (CTable o in arr)
            {
                MCashAccount v = new MCashAccount(o);
                cashAccounts.Add(v);
            }

            isCashAccountLoaded = true;
        }

        public static Boolean IsCashAccountLoad()
        {
            return (isCashAccountLoaded);
        }
        #endregion

        #region posImportStatus
        public static void LoadPosImportStatus()
        {
            posImportStatus.Clear();

            MMasterRef c = new MMasterRef(new CTable(""));
            posImportStatus.Add(c);

            MMasterRef v1 = new MMasterRef(new CTable(""));
            v1.MasterID = "1";
            v1.Description = CUtil.PosImportStatusToString(v1.MasterID);
            posImportStatus.Add(v1);

            MMasterRef v2 = new MMasterRef(new CTable(""));
            v2.MasterID = "2";
            v2.Description = CUtil.PosImportStatusToString(v2.MasterID);
            posImportStatus.Add(v2);

            isPosImportStatusLoaded = true;
        }

        public static Boolean IsPosImportStatusLoad()
        {
            return (isPosImportStatusLoaded);
        }
        #endregion

        #region Location
        public static void LoadLocation(Boolean allowEmpty, ArrayList parray)
        {
            removeAllLocationItems(locations);
            removeAllLocationItems(borrowReturnlocations);

            CTable t = new CTable("");
            ArrayList arr = null;

            arr = OnixWebServiceAPI.GetLocationList(t);

            MLocation c = new MLocation(new CTable(""));
            locations.Add(c);

            MLocation c2 = new MLocation(new CTable(""));
            borrowReturnlocations.Add(c2);

            foreach (CTable o in arr)
            {
                MLocation v = new MLocation(o);
                locations.Add(v);

                if (v.IsForBorrow == true)
                {
                    MLocation v2 = new MLocation(o.CloneAll());
                    borrowReturnlocations.Add(v2);
                }
            }

            isLocationLoaded = true;
        }

        public static Boolean IsLocationLoad()
        {
            return (isLocationLoaded);
        }
        #endregion

        #region User Group
        public static void LoadUserGroup(GetListFunction listFunc)
        {
            userGroups.Clear();

            CTable t = new CTable("");
            ArrayList arr = null;

            arr = listFunc(t);

            MUserGroup c = new MUserGroup(new CTable(""));
            userGroups.Add(c);

            foreach (CTable o in arr)
            {
                MUserGroup v = new MUserGroup(o);
                userGroups.Add(v);
            }
        }

        public static Boolean IsUserGroupLoaded()
        {
            return (userGroups.Count <= 0);
        }
        #endregion

        #region Cycle
        public static void LoadCycle(Boolean allowEmpty, ArrayList parray)
        {
            removeAllCycleItems(cycle);

            CTable t = new CTable("");
            ArrayList arr = null;

            arr = OnixWebServiceAPI.GetCycleList(t);

            MCycle c = new MCycle(new CTable(""));
            cycle.Add(c);

            foreach (CTable o in arr)
            {
                MCycle v = new MCycle(o);
                cycle.Add(v);
            }

            isCycleLoaded = true;
        }

        public static Boolean IsCycleLoad()
        {
            return (isCycleLoaded);
        }

        public static void LoadCycleWeekly(Boolean allowEmpty, ArrayList parray)
        {
            removeAllCycleItems(cycle_weekly);

            CTable t = new CTable("");
            ArrayList arr = null;

            t.SetFieldValue("CYCLE_TYPE", "1");
            arr = OnixWebServiceAPI.GetCycleList(t);

            MCycle c = new MCycle(new CTable(""));
            cycle_weekly.Add(c);

            foreach (CTable o in arr)
            {
                MCycle v = new MCycle(o);
                cycle_weekly.Add(v);
            }

            isCycleWeeklyLoaded = true;
        }

        public static Boolean IsCycleWeeklyLoad()
        {
            return (isCycleWeeklyLoaded);
        }

        public static void LoadCycleMonthly(Boolean allowEmpty, ArrayList parray)
        {
            removeAllCycleItems(cycle_monthly);

            CTable t = new CTable("");
            ArrayList arr = null;

            t.SetFieldValue("CYCLE_TYPE", "2");
            arr = OnixWebServiceAPI.GetCycleList(t);

            MCycle c = new MCycle(new CTable(""));
            cycle_monthly.Add(c);

            foreach (CTable o in arr)
            {
                MCycle v = new MCycle(o);
                cycle_monthly.Add(v);
            }

            isCycleMonthlyLoaded = true;
        }

        public static Boolean IsCycleMonthlyLoad()
        {
            return (isCycleMonthlyLoaded);
        }
        #endregion

        #region Package Type
        public static void LoadPackageType()
        {
            packageTypes.Clear();

            MMasterRef c = new MMasterRef(new CTable(""));
            packageTypes.Add(c);

            for (int i = 1; i <= 10; i++)
            {
                MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                v.MasterID = i.ToString();
                v.Description = CUtil.PackageTypeToString(i.ToString());
                v.RefType = CUtil.PackageTypeToGroup(v.MasterID);

                packageTypes.Add(v);
            }

            isPackageTypeLoaded = true;
        }

        public static Boolean IsPackageTypeLoad()
        {
            return (isPackageTypeLoaded);
        }
        #endregion

        #region Master Ref
        public ObservableCollection<MMasterRef> GetMasterRefCollection(MasterRefEnum mtype)
        {
            ObservableCollection<MMasterRef> coll = getMasterRefCollection(mtype);
            return (coll);
        }

        public static Boolean IsMasterRefLoad(MasterRefEnum mtype)
        {
            return (isMasterRefLoaded);
        }

        public static void LoadAllMasterRefItems(GetListFunction listFunc)
        {
            CTable t = new CTable("");
            //ArrayList arr = OnixWebServiceAPI.GetMasterRefList(t);
            ArrayList arr = listFunc(t);

            //Sorted by master ref type and decription

            String type = "";
            foreach (CTable o in arr)
            {
                MMasterRef m = new MMasterRef(o);

                ObservableCollection<MMasterRef> coll = Instance.getMasterRefCollection((MasterRefEnum)int.Parse(m.RefType));
                if (!m.RefType.Equals(type))
                {
                    removeItems(coll);
                    type = m.RefType;

                    MMasterRef c = new MMasterRef(new CTable(""));
                    coll.Add(c);
                }

                coll.Add(m);
            }

            isMasterRefLoaded = true;
        }

        public static void LoadAllMasterRefItems(GetListFunction listFunc, MasterRefEnum rt)
        {
            CTable t = new CTable("");
            ArrayList arr = listFunc(t);

            //Sorted by master ref type and decription

            //String type = "";
            foreach (CTable o in arr)
            {
                MMasterRef m = new MMasterRef(o);

                if (((MasterRefEnum)int.Parse(m.RefType)) != rt)
                {
                    continue;
                }

                ObservableCollection<MMasterRef> coll = Instance.getMasterRefCollection((MasterRefEnum)int.Parse(m.RefType));
                //if (!m.RefType.Equals(type))
                //{
                //    removeItems(coll);
                //    type = m.RefType;

                //    MMasterRef c = new MMasterRef(new CTable(""));
                //    coll.Add(c);
                //}

                //coll.Add(m);
                fillInObject(coll, m);
            }

            isMasterRefLoaded = true;
        }

        private static void fillInObject(ObservableCollection<MMasterRef> coll, MMasterRef obj)
        {
            MMasterRef curr = null;

            foreach (MMasterRef o in coll)
            {
                if (obj.MasterID.Equals(o.MasterID))
                {
                    curr = obj;
                    break;
                }
            }

            if (curr != null)
            {
                //Update here
                curr.Description = obj.Description;
                curr.Code = obj.Description;
            }
            else
            {
                if (coll.Count <= 0)
                {
                    MMasterRef c = new MMasterRef(new CTable(""));
                    coll.Add(c);
                }

                coll.Add(obj);
            }
        }

        #endregion

        #region ItemCategoriesTree

        public ObservableCollection<MItemCategory> ItemCategoriesTree
        {
            get
            {
                return (categoryRootNode.ChildrenNodes);
            }
        }

        public void InitItemCategoriesTree()
        {
            if (categoryRootNode.ChildrenNodes.Count > 0)
            {
                return;
            }

            LoadItemCategoriesTree();
        }

        public void AddCategoryToTree(MItemCategory ct)
        {
            String pid = ct.ParentID;

            MItemCategory tmp = new MItemCategory(new CTable(""));
            tmp.ItemCategoryID = pid;
            //Find parent node
            MItemCategory pnode = findItemCategoryNode(tmp, categoryRootNode.ChildrenNodes);

            if (pnode != null)
            {
                pnode.ChildrenNodes.Insert(0, ct);
            }
            else
            {
                categoryRootNode.ChildrenNodes.Insert(0, ct);
            }
        }

        public void EditCategoryInTree(MItemCategory ct)
        {
            MItemCategory node = findItemCategoryNode(ct, categoryRootNode.ChildrenNodes);

            if (node != null)
            {
                node.CategoryName = ct.CategoryName;
                node.NotifyAllPropertiesChanged();
            }
        }

        public void DeleteCategoryFromTree(MItemCategory ct)
        {
            String pid = ct.ParentID;

            MItemCategory tmp = new MItemCategory(new CTable(""));
            tmp.ItemCategoryID = pid;
            //Find parent node
            MItemCategory pnode = findItemCategoryNode(tmp, categoryRootNode.ChildrenNodes);

            if (pnode != null)
            {
                pnode.ChildrenNodes.Remove(ct);
            }
            else
            {
                categoryRootNode.ChildrenNodes.Remove(ct);
            }
        }

        private MItemCategory findItemCategoryNode(MItemCategory nd, ObservableCollection<MItemCategory> items)
        {
            foreach (MItemCategory v in items)
            {
                if (v.ItemCategoryID.Equals(nd.ItemCategoryID))
                {
                    return (v);
                }

                MItemCategory r = findItemCategoryNode(nd, v.ChildrenNodes);
                if (r != null)
                {
                    return (r);
                }
            }

            return (null);
        }

        private void createChildren(ArrayList arr, MItemCategory parentNode)
        {
            foreach (CTable o in arr)
            {
                MItemCategory v = new MItemCategory(o);

                if (v.ParentID.Equals(parentNode.ItemCategoryID))
                {
                    createChildren(arr, v);
                    parentNode.ChildrenNodes.Add(v);
                }
            }
        }

        public void LoadItemCategoriesTree()
        {
            categoryRootNode.ChildrenNodes.Clear();

            ArrayList arr = OnixWebServiceAPI.GetItemCategoryList(new CTable(""));
            createChildren(arr, categoryRootNode);
        }

        #endregion

        #region Company Profile
        public static Boolean IsCompanyLoad()
        {
            return (!(company == null));
        }

        [Obsolete("LoadCompanyProfile()")]
        public static void LoadCompanyProfile(GetListFunction listFunc)
        {
        }

        public static void LoadCompanyProfile()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetCompanyProfileList", "COMPANY_LIST", new CTable(""));
            if (arr.Count > 0)
            {
                CTable t = (CTable)arr[0];
                CTable cpf = OnixWebServiceAPI.SubmitObjectAPI("GetCompanyProfileInfo", t);

                company = new MCompanyProfile(cpf);
                company.InitCompanyImage();
            }
        }

        public MCompanyProfile CompanyProfile
        {
            get
            {
                MCompanyProfile cp = new MCompanyProfile(new CTable(""));
                if (company == null)
                {
                    return (cp);
                }

                return (company);
            }
        }

        #endregion

        #region for binding
        public ObservableCollection<String> BooleanTypes
        {
            get
            {
                if (booleanType.Count <= 0)
                {
                    booleanType.Add(CLanguage.getValue("boolean_true"));
                    booleanType.Add(CLanguage.getValue("boolean_false"));
                }

                return (booleanType);
            }
        }

        public ObservableCollection<MMasterRef> CycleTypes
        {
            get
            {
                if (cycleTypes.Count <= 0)
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        if (i == 1)
                        {
                            MMasterRef o = new MMasterRef(new CTable("MASTER_REF"));
                            o.MasterID = "0";
                            o.Description = "";

                            cycleTypes.Add(o);
                        }

                        MMasterRef v = new MMasterRef(new CTable("MASTER_REF"));
                        v.MasterID = i.ToString();
                        v.Description = CUtil.CycleTypeToString(i.ToString());

                        cycleTypes.Add(v);
                    }
                }

                return (cycleTypes);
            }
        }

        public ObservableCollection<String> FinalDiscountBasketTypes
        {
            get
            {
                if (finalDiscountBasketTypes.Count <= 0)
                {
                    finalDiscountBasketTypes.Add(CLanguage.getValue("exclude_item_discounted"));
                    finalDiscountBasketTypes.Add(CLanguage.getValue("include_item_discounted"));
                }

                return (finalDiscountBasketTypes);
            }
        }

        public ObservableCollection<String> ProductSelectionTypes
        {
            get
            {
                if (productSelectType.Count <= 0)
                {
                    productSelectType.Add(CLanguage.getValue("service"));
                    productSelectType.Add(CLanguage.getValue("item"));
                    productSelectType.Add(CLanguage.getValue("item_category"));
                    productSelectType.Add(CLanguage.getValue("service_type"));
                }

                return (productSelectType);
            }
        }

        public ObservableCollection<String> LookupSelectType
        {
            get
            {
                if (lookupSelectType.Count <= 0)
                {
                    lookupSelectType.Add(CLanguage.getValue("bySaleUnitPrice"));
                    lookupSelectType.Add(CLanguage.getValue("byQuantity"));
                }

                return (lookupSelectType);
            }
        }

        public ObservableCollection<String> IntervalOutputTypes
        {
            get
            {
                if (intervalOutputTypes.Count <= 0)
                {
                    intervalOutputTypes.Add(CLanguage.getValue("for_unit_price"));
                    intervalOutputTypes.Add(CLanguage.getValue("for_total_price"));
                }

                return (intervalOutputTypes);
            }
        }

        public ObservableCollection<String> CustomerSelectTypes
        {
            get
            {
                if (customerSelectTypes.Count <= 0)
                {
                    customerSelectTypes.Add(CLanguage.getValue("customer_type"));
                    customerSelectTypes.Add(CLanguage.getValue("customer_group"));
                    customerSelectTypes.Add(CLanguage.getValue("customer_name"));
                }

                return (customerSelectTypes);
            }
        }

        public ObservableCollection<MMasterRef> Products
        {
            get
            {
                if (products.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    products.Add(mr0);

                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "onix";
                    mr1.Description = mr1.MasterID;
                    products.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "sass";
                    mr2.Description = mr2.MasterID;
                    products.Add(mr2);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "lotto";
                    mr3.Description = mr3.MasterID;
                    products.Add(mr3);
                }

                return (products);
            }
        }

        public ObservableCollection<MMasterRef> Stages
        {
            get
            {
                if (stages.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    stages.Add(mr0);

                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "dev";
                    mr1.Description = CLanguage.getValue(mr1.MasterID);
                    stages.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "qa";
                    mr2.Description = CLanguage.getValue(mr2.MasterID);
                    stages.Add(mr2);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "prod";
                    mr3.Description = CLanguage.getValue(mr3.MasterID);
                    stages.Add(mr3);
                }

                return (stages);
            }
        }

        public ObservableCollection<MMasterRef> MachineSpecs
        {
            get
            {
                if (vmSpects.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    vmSpects.Add(mr0);

                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "micro";
                    mr1.Description = CLanguage.getValue(mr1.MasterID);
                    vmSpects.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "small";
                    mr2.Description = CLanguage.getValue(mr2.MasterID);
                    vmSpects.Add(mr2);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "vcpu1";
                    mr3.Description = CLanguage.getValue(mr3.MasterID);
                    vmSpects.Add(mr3);
                }

                return (vmSpects);
            }
        }

        public ObservableCollection<MMasterRef> Roles
        {
            get
            {
                if (roles.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    roles.Add(mr0);

                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "storage";
                    mr1.Description = CLanguage.getValue(mr1.MasterID);
                    roles.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "master";
                    mr2.Description = CLanguage.getValue(mr2.MasterID);
                    roles.Add(mr2);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "worker";
                    mr3.Description = CLanguage.getValue(mr3.MasterID);
                    roles.Add(mr3);
                }

                return (roles);
            }
        }

        public ObservableCollection<MMasterRef> ProductSpecificSelectionTypes
        {
            get
            {
                if (productSpecificSelectType.Count <= 0)
                {
                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "1";
                    mr1.Description = CLanguage.getValue("service");
                    productSpecificSelectType.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "2";
                    mr2.Description = CLanguage.getValue("item");
                    productSpecificSelectType.Add(mr2);
                }

                return (productSpecificSelectType);
            }
        }

        public ObservableCollection<MMasterRef> ProductUnSpecificSelectionTypes
        {
            get
            {
                if (productUnSpecificSelectType.Count <= 0)
                {
                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "1";
                    mr1.Description = CLanguage.getValue("service");
                    productUnSpecificSelectType.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "2";
                    mr2.Description = CLanguage.getValue("item");
                    productUnSpecificSelectType.Add(mr2);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "3";
                    mr3.Description = CLanguage.getValue("free_text");
                    productUnSpecificSelectType.Add(mr3);
                }

                return (productUnSpecificSelectType);
            }
        }

        public ObservableCollection<MMasterRef> ProductMiscUnSpecificSelectionTypes
        {
            get
            {
                if (productMiscUnSpecificSelectType.Count <= 0)
                {
                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "1";
                    mr1.Description = CLanguage.getValue("service");
                    productMiscUnSpecificSelectType.Add(mr1);

                    MMasterRef mr3 = new MMasterRef(new CTable(""));
                    mr3.MasterID = "3";
                    mr3.Description = CLanguage.getValue("free_text");
                    productMiscUnSpecificSelectType.Add(mr3);
                }

                return (productMiscUnSpecificSelectType);
            }
        }

        public ObservableCollection<MMasterRef> CashXferTypes
        {
            get
            {
                if (xferTypes.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    xferTypes.Add(mr0);

                    MMasterRef mr1 = new MMasterRef(new CTable(""));
                    mr1.MasterID = "1";
                    mr1.Description = CLanguage.getValue("regular_cash_xfer");
                    xferTypes.Add(mr1);

                    MMasterRef mr2 = new MMasterRef(new CTable(""));
                    mr2.MasterID = "2";
                    mr2.Description = CLanguage.getValue("external_cash_xfer");
                    xferTypes.Add(mr2);
                }

                return (xferTypes);
            }
        }

        public ObservableCollection<String> VoucherSelectFree
        {
            get
            {
                if (voucherSelectFreeTypes.Count <= 0)
                {
                    voucherSelectFreeTypes.Add(CLanguage.getValue("service"));
                    voucherSelectFreeTypes.Add(CLanguage.getValue("item"));
                    voucherSelectFreeTypes.Add(CLanguage.getValue("voucher"));
                    voucherSelectFreeTypes.Add(CLanguage.getValue("other"));
                }

                return (voucherSelectFreeTypes);
            }
        }

        public ObservableCollection<String> DiscuntMappingTypes
        {
            get
            {
                if (discountMappingTypes.Count <= 0)
                {
                    discountMappingTypes.Add(CLanguage.getValue("discount_map_quantity"));
                    discountMappingTypes.Add(CLanguage.getValue("discount_map_amount"));
                }

                return (discountMappingTypes);
            }
        }

        public ObservableCollection<String> DiscountOutputTypes
        {
            get
            {
                if (discountOutputTypes.Count <= 0)
                {
                    discountOutputTypes.Add(CLanguage.getValue("discount_output_fix"));
                    discountOutputTypes.Add(CLanguage.getValue("discount_output_per_unit"));
                    discountOutputTypes.Add(CLanguage.getValue("discount_output_percent"));
                }

                return (discountOutputTypes);
            }
        }
        
        public ObservableCollection<MMasterRef> ServiceCategories
        {
            get
            {
                if (serviceCategories.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = CLanguage.getValue("");
                    p0.MasterID = "0";
                    serviceCategories.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("for_sale");
                    p1.MasterID = "1";
                    serviceCategories.Add(p1);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CLanguage.getValue("for_purchase");
                    p3.MasterID = "2";
                    serviceCategories.Add(p3);
                }

                return (serviceCategories);
            }
        }
        
        public ObservableCollection<MMasterRef> LeaveDurations
        {
            get
            {
                if (leaveDurations.Count <= 0)
                {
                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = "";
                    p1.MasterID = "0.00";
                    leaveDurations.Add(p1);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = "ครึ่งวัน";
                    p3.MasterID = "0.50";
                    leaveDurations.Add(p3);

                    MMasterRef p3_1 = new MMasterRef(new CTable(""));
                    p3_1.Description = "เต็มวัน";
                    p3_1.MasterID = "1.00";
                    leaveDurations.Add(p3_1);                    
                }

                return (leaveDurations);
            }
        }

        public ObservableCollection<MMasterRef> PoPayments
        {
            get
            {
                if (poPayments.Count <= 0)
                {
                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("Cash");
                    p1.MasterID = "1";
                    poPayments.Add(p1);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CLanguage.getValue("Credit"); //Same as Transfer
                    p3.MasterID = "2";
                    poPayments.Add(p3);

                    MMasterRef p3_1 = new MMasterRef(new CTable(""));
                    p3_1.Description = CLanguage.getValue("CreditCard");
                    p3_1.MasterID = "3";
                    poPayments.Add(p3_1);

                    MMasterRef p5 = new MMasterRef(new CTable(""));
                    p5.Description = CLanguage.getValue("personal_account");
                    p5.MasterID = "5";
                    poPayments.Add(p5);

                    MMasterRef p6 = new MMasterRef(new CTable(""));
                    p6.Description = CLanguage.getValue("company_saving_account2");
                    p6.MasterID = "6"; //ใช้ ID เดียวกันแต่คนละชื่อกับหน้าจอ payment
                    poPayments.Add(p6);

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CLanguage.getValue("Cheque");
                    p4.MasterID = "4";
                    poPayments.Add(p4);
                }

                return (poPayments);
            }
        }

        public ObservableCollection<MMasterRef> OtMultipliers
        {
            get
            {
                if (otMultipliers.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "";
                    otMultipliers.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = "1.00";
                    p1.MasterID = "1.00";
                    otMultipliers.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = "1.50";
                    p2.MasterID = "1.50";
                    otMultipliers.Add(p2);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = "2.00";
                    p3.MasterID = "2.00";
                    otMultipliers.Add(p3);

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = "3.00";
                    p4.MasterID = "3.00";
                    otMultipliers.Add(p4);
                }

                return (otMultipliers);
            }
        }


        public ObservableCollection<MMasterRef> PayrollExpenseTypes
        {
            get
            {
                if (payrollExpenseTypes.Count <= 0)
                {
                    MMasterRef e0 = new MMasterRef(new CTable(""));
                    e0.Description = "";
                    e0.MasterID = "";
                    payrollExpenseTypes.Add(e0);

                    MMasterRef e1 = new MMasterRef(new CTable(""));
                    e1.Description = CLanguage.getValue("expense_fuel");
                    e1.MasterID = "1";
                    payrollExpenseTypes.Add(e1);

                    MMasterRef e2 = new MMasterRef(new CTable(""));
                    e2.Description = CLanguage.getValue("expense_tollway");
                    e2.MasterID = "2";
                    payrollExpenseTypes.Add(e2);

                    MMasterRef e3 = new MMasterRef(new CTable(""));
                    e3.Description = CLanguage.getValue("expense_carpark");
                    e3.MasterID = "3";
                    payrollExpenseTypes.Add(e3);

                    MMasterRef e4 = new MMasterRef(new CTable(""));
                    e4.Description = CLanguage.getValue("expense_vehicle");
                    e4.MasterID = "4";
                    payrollExpenseTypes.Add(e4);

                    MMasterRef e5 = new MMasterRef(new CTable(""));
                    e5.Description = CLanguage.getValue("expense_other");
                    e5.MasterID = "5";
                    payrollExpenseTypes.Add(e5);
                }

                return (payrollExpenseTypes);
            }
        }

        public ObservableCollection<MMasterRef> PayrollDeductionTypes
        {
            get
            {
                if (payrollDeductionTypes.Count <= 0)
                {
                    MMasterRef e0 = new MMasterRef(new CTable(""));
                    e0.Description = "";
                    e0.MasterID = "";
                    payrollDeductionTypes.Add(e0);

                    MMasterRef e1 = new MMasterRef(new CTable(""));
                    e1.Description = "ขาด";
                    e1.MasterID = "1";
                    e1.Code = "hour";
                    payrollDeductionTypes.Add(e1);

                    MMasterRef e2 = new MMasterRef(new CTable(""));
                    e2.Description = "ทำงานไม่ครบ";
                    e2.MasterID = "2";
                    e2.Code = "hour";
                    payrollDeductionTypes.Add(e2);

                    MMasterRef e3 = new MMasterRef(new CTable(""));
                    e3.Description = "สาย";
                    e3.MasterID = "3";
                    e3.Code = "minute";
                    payrollDeductionTypes.Add(e3);

                    MMasterRef e4 = new MMasterRef(new CTable(""));
                    e4.Description = "อื่น ๆ";
                    e4.MasterID = "4";
                    e4.Code = "minute";
                    payrollDeductionTypes.Add(e4);
                }

                return (payrollDeductionTypes);
            }
        }

        public ObservableCollection<MMasterRef> PurchaseExpenseDocTypes
        {
            get
            {
                if (purchaseExpenseDocTypes.Count <= 0)
                {
                    AccountDocumentType dt;

                    dt = AccountDocumentType.AcctDocCashPurchase;

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.AccountDocTypeToString(dt);
                    p1.MasterID = ((int)dt).ToString();
                    purchaseExpenseDocTypes.Add(p1);


                    dt = AccountDocumentType.AcctDocDebtPurchase;

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.AccountDocTypeToString(dt);
                    p2.MasterID = ((int)dt).ToString();
                    purchaseExpenseDocTypes.Add(p2);


                    dt = AccountDocumentType.AcctDocMiscExpense;

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.AccountDocTypeToString(dt);
                    p3.MasterID = ((int)dt).ToString();
                    purchaseExpenseDocTypes.Add(p3);

                    dt = AccountDocumentType.AcctDocCrNotePurchase;

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CUtil.AccountDocTypeToString(dt);
                    p4.MasterID = ((int)dt).ToString();
                    purchaseExpenseDocTypes.Add(p4);

                    dt = AccountDocumentType.AcctDocDrNotePurchase;

                    MMasterRef p5 = new MMasterRef(new CTable(""));
                    p5.Description = CUtil.AccountDocTypeToString(dt);
                    p5.MasterID = ((int)dt).ToString();
                    purchaseExpenseDocTypes.Add(p5);
                }

                return (purchaseExpenseDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> VatReportTypes
        {
            get
            {
                if (vatReportTypes.Count <= 0)
                {
                    vatReportTypes.Clear();

                    //MMasterRef p0 = new MMasterRef(new CTable(""));
                    //p0.Description = "";
                    //p0.MasterID = "0";
                    //vatReportTypes.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = "แสดงวันที่ตามใบกำกับ";
                    p1.MasterID = "1";
                    vatReportTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = "แสดงวันที่ตามใบสำคัญจ่าย";
                    p2.MasterID = "2";
                    vatReportTypes.Add(p2);
                }

                return (vatReportTypes);
            }
        }

        public ObservableCollection<MMasterRef> PurchasePaymentDocTypes
        {
            get
            {
                if (purchasePaymentDocTypes.Count <= 0)
                {
                    AccountDocumentType dt;

                    dt = AccountDocumentType.AcctDocCashPurchase;

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.AccountDocTypeToString(dt);
                    p1.MasterID = ((int)dt).ToString();
                    purchasePaymentDocTypes.Add(p1);


                    dt = AccountDocumentType.AcctDocMiscExpense;

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.AccountDocTypeToString(dt);
                    p3.MasterID = ((int)dt).ToString();
                    purchasePaymentDocTypes.Add(p3);

                    dt = AccountDocumentType.AcctDocApReceipt;

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.AccountDocTypeToString(dt);
                    p2.MasterID = ((int)dt).ToString();
                    purchasePaymentDocTypes.Add(p2);

                }

                return (purchasePaymentDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> SalePaymentDocTypes
        {
            get
            {
                if (salePaymentDocTypes.Count <= 0)
                {
                    AccountDocumentType dt;

                    dt = AccountDocumentType.AcctDocCashSale;

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.AccountDocTypeToString(dt);
                    p1.MasterID = ((int)dt).ToString();
                    salePaymentDocTypes.Add(p1);


                    dt = AccountDocumentType.AcctDocMiscRevenue;

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.AccountDocTypeToString(dt);
                    p3.MasterID = ((int)dt).ToString();
                    salePaymentDocTypes.Add(p3);

                    dt = AccountDocumentType.AcctDocArReceipt;

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.AccountDocTypeToString(dt);
                    p2.MasterID = ((int)dt).ToString();
                    salePaymentDocTypes.Add(p2);

                }

                return (salePaymentDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> PurchaseWhDocTypes
        {
            get
            {
                if (purchaseWhDocTypes.Count <= 0)
                {
                    AccountDocumentType dt;

                    dt = AccountDocumentType.AcctDocCashPurchase;

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.AccountDocTypeToString(dt);
                    p1.MasterID = ((int)dt).ToString();
                    purchaseWhDocTypes.Add(p1);


                    dt = AccountDocumentType.AcctDocMiscExpense;

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.AccountDocTypeToString(dt);
                    p3.MasterID = ((int)dt).ToString();
                    purchaseWhDocTypes.Add(p3);


                    dt = AccountDocumentType.AcctDocApReceipt;

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.AccountDocTypeToString(dt);
                    p2.MasterID = ((int)dt).ToString();
                    purchaseWhDocTypes.Add(p2);


                    dt = AccountDocumentType.AcctDocCrNotePurchase;

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CUtil.AccountDocTypeToString(dt);
                    p4.MasterID = ((int)dt).ToString();
                    purchaseWhDocTypes.Add(p4);

                    dt = AccountDocumentType.AcctDocDrNotePurchase;

                    MMasterRef p6 = new MMasterRef(new CTable(""));
                    p6.Description = CUtil.AccountDocTypeToString(dt);
                    p6.MasterID = ((int)dt).ToString();
                    purchaseWhDocTypes.Add(p6);
                }

                return (purchaseWhDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> RevenueTaxTypes
        {
            get
            {
                if (revenueTaxTypes.Count <= 0)
                {
                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("rv_tax_3");
                    p1.MasterID = "3";
                    revenueTaxTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CLanguage.getValue("rv_tax_53");
                    p2.MasterID = "53";
                    revenueTaxTypes.Add(p2);
                }

                return (revenueTaxTypes);
            }
        }

        public ObservableCollection<MMasterRef> TaxDocTypes
        {
            get
            {
                if (taxDocTypes.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "";
                    taxDocTypes.Add(p0);

                    MMasterRef p0_1 = new MMasterRef(new CTable(""));
                    p0_1.MasterID = ((int)TaxDocumentType.TaxDocPP30).ToString();
                    p0_1.Description = CUtil.TaxDocTypeToString(TaxDocumentType.TaxDocPP30);
                    taxDocTypes.Add(p0_1);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.MasterID = ((int) TaxDocumentType.TaxDocRev3).ToString();
                    p1.Description = CUtil.TaxDocTypeToString(TaxDocumentType.TaxDocRev3);
                    taxDocTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.MasterID = ((int)TaxDocumentType.TaxDocRev53).ToString();
                    p2.Description = CUtil.TaxDocTypeToString(TaxDocumentType.TaxDocRev53);
                    taxDocTypes.Add(p2);
                }

                return (taxDocTypes);
            }
        }



        public ObservableCollection<MMasterRef> PayrollDocTypes
        {
            get
            {
                if (payrollDocTypes.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "";
                    payrollDocTypes.Add(p0);

                    MMasterRef p0_0 = new MMasterRef(new CTable(""));
                    p0_0.MasterID = ((int)PayrollDocType.PayrollBalanceForward).ToString();
                    p0_0.Description = CUtil.PayrollDocTypeToString(PayrollDocType.PayrollBalanceForward);
                    payrollDocTypes.Add(p0_0);

                    MMasterRef p0_1 = new MMasterRef(new CTable(""));
                    p0_1.MasterID = ((int)PayrollDocType.PayrollDaily).ToString();
                    p0_1.Description = CUtil.PayrollDocTypeToString(PayrollDocType.PayrollDaily);
                    payrollDocTypes.Add(p0_1);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.MasterID = ((int)PayrollDocType.PayrollMonthly).ToString();
                    p1.Description = CUtil.PayrollDocTypeToString(PayrollDocType.PayrollMonthly);
                    payrollDocTypes.Add(p1);
                }

                return (payrollDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> EmployeeTypes
        {
            get
            {
                if (employeeTypes.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "";
                    employeeTypes.Add(p0);

                    MMasterRef p0_1 = new MMasterRef(new CTable(""));
                    p0_1.MasterID = ((int)EmployeeType.EmployeeDaily).ToString();
                    p0_1.Description = CUtil.EmployeeTypeToString(EmployeeType.EmployeeDaily);
                    employeeTypes.Add(p0_1);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.MasterID = ((int)EmployeeType.EmployeeMonthly).ToString();
                    p1.Description = CUtil.EmployeeTypeToString(EmployeeType.EmployeeMonthly);
                    employeeTypes.Add(p1);
                }

                return (employeeTypes);
            }
        }

        public ObservableCollection<MMasterRef> SaleRevenueDocTypes
        {
            get
            {
                if (saleRevenueDocTypes.Count <= 0)
                {
                    AccountDocumentType dt;

                    dt = AccountDocumentType.AcctDocCashSale;

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.AccountDocTypeToString(dt);
                    p1.MasterID = ((int)dt).ToString();
                    saleRevenueDocTypes.Add(p1);


                    dt = AccountDocumentType.AcctDocDebtSale;

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.AccountDocTypeToString(dt);
                    p2.MasterID = ((int)dt).ToString();
                    saleRevenueDocTypes.Add(p2);


                    dt = AccountDocumentType.AcctDocMiscRevenue;

                    MMasterRef p5 = new MMasterRef(new CTable(""));
                    p5.Description = CUtil.AccountDocTypeToString(dt);
                    p5.MasterID = ((int)dt).ToString();
                    saleRevenueDocTypes.Add(p5);

                    dt = AccountDocumentType.AcctDocCrNote;

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CUtil.AccountDocTypeToString(dt);
                    p4.MasterID = ((int)dt).ToString();
                    saleRevenueDocTypes.Add(p4);

                    dt = AccountDocumentType.AcctDocDrNote;

                    MMasterRef p6 = new MMasterRef(new CTable(""));
                    p6.Description = CUtil.AccountDocTypeToString(dt);
                    p6.MasterID = ((int)dt).ToString();
                    saleRevenueDocTypes.Add(p6);
                }

                return (saleRevenueDocTypes);
            }
        }

        public ObservableCollection<MMasterRef> RefundStatus
        {
            get
            {
                if (refundStatus.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = CLanguage.getValue("NotSelected");
                    p0.MasterID = "999";
                    refundStatus.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("NoRefunded");
                    p1.MasterID = "1";
                    refundStatus.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CLanguage.getValue("Refunded");
                    p2.MasterID = "2";
                    refundStatus.Add(p2);                    
                }

                return (refundStatus);
            }
        }

        public ObservableCollection<MMasterRef> AccountSalePayTypes
        {
            get
            {
                if (accountSalePayTypes.Count <= 0)
                {
                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("Cash");
                    p1.MasterID = "1";
                    accountSalePayTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CLanguage.getValue("Transfer");
                    p2.MasterID = "2";
                    accountSalePayTypes.Add(p2);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CLanguage.getValue("CreditCard");
                    p3.MasterID = "3";
                    accountSalePayTypes.Add(p3);

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CLanguage.getValue("Cheque");
                    p4.MasterID = "4";
                    accountSalePayTypes.Add(p4);

                    MMasterRef p5 = new MMasterRef(new CTable(""));
                    p5.Description = CLanguage.getValue("personal_account");
                    p5.MasterID = "5";
                    accountSalePayTypes.Add(p5);

                    MMasterRef p6 = new MMasterRef(new CTable(""));
                    p6.Description = CLanguage.getValue("company_saving_account");
                    p6.MasterID = "6";
                    accountSalePayTypes.Add(p6);
                }

                return (accountSalePayTypes);
            }
        }

        public ObservableCollection<MMasterRef> VariableTypes
        {
            get
            {
                if (variableTypes.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "0";
                    variableTypes.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue(VariableType.VarString.ToString());
                    p1.MasterID = ((int) VariableType.VarString).ToString();
                    p1.Code = VariableType.VarString.ToString();
                    variableTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CLanguage.getValue(VariableType.VarNumber.ToString());
                    p2.MasterID = ((int)VariableType.VarNumber).ToString();
                    p2.Code = VariableType.VarNumber.ToString();
                    variableTypes.Add(p2);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CLanguage.getValue(VariableType.VarLangKey.ToString());
                    p3.MasterID = ((int)VariableType.VarLangKey).ToString();
                    p3.Code = VariableType.VarLangKey.ToString();
                    variableTypes.Add(p3);

                    MMasterRef p4 = new MMasterRef(new CTable(""));
                    p4.Description = CLanguage.getValue(VariableType.VarBinding.ToString());
                    p4.MasterID = ((int)VariableType.VarBinding).ToString();
                    p4.Code = VariableType.VarBinding.ToString();
                    variableTypes.Add(p4);
                }

                return (variableTypes);
            }
        }

        public ObservableCollection<MMasterRef> QuotationTypes
        {
            get
            {
                if (quotationTypes.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "0";
                    quotationTypes.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CLanguage.getValue("quotation_by_summary");
                    p1.MasterID = "1";
                    quotationTypes.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CLanguage.getValue("quotation_by_detail");
                    p2.MasterID = "2";
                    quotationTypes.Add(p2);
                }

                return (quotationTypes);
            }
        }

        public ObservableCollection<MMasterRef> PoStatuses
        {
            get
            {
                if (poStatuses.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "0";
                    poStatuses.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.PoStatusToString(PoDocumentStatus.PoPending);
                    p1.MasterID = ((int)PoDocumentStatus.PoPending).ToString();
                    poStatuses.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.PoStatusToString(PoDocumentStatus.PoApproved);
                    p2.MasterID = ((int)PoDocumentStatus.PoApproved).ToString();
                    poStatuses.Add(p2);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.PoStatusToString(PoDocumentStatus.PoCancelApproved);
                    p3.MasterID = ((int)PoDocumentStatus.PoCancelApproved).ToString();
                    poStatuses.Add(p3);
                }

                return (poStatuses);
            }
        }

        public ObservableCollection<MMasterRef> DocumentStatuses
        {
            get
            {
                if (docStatuses.Count <= 0)
                {
                    MMasterRef p0 = new MMasterRef(new CTable(""));
                    p0.Description = "";
                    p0.MasterID = "0";
                    docStatuses.Add(p0);

                    MMasterRef p1 = new MMasterRef(new CTable(""));
                    p1.Description = CUtil.InvDocStatusToString(InventoryDocumentStatus.InvDocPending);
                    p1.MasterID = ((int) InventoryDocumentStatus.InvDocPending).ToString();
                    docStatuses.Add(p1);

                    MMasterRef p2 = new MMasterRef(new CTable(""));
                    p2.Description = CUtil.InvDocStatusToString(InventoryDocumentStatus.InvDocApproved);
                    p2.MasterID = ((int)InventoryDocumentStatus.InvDocApproved).ToString();
                    docStatuses.Add(p2);

                    MMasterRef p3 = new MMasterRef(new CTable(""));
                    p3.Description = CUtil.InvDocStatusToString(InventoryDocumentStatus.InvDocCancelApproved);
                    p3.MasterID = ((int)InventoryDocumentStatus.InvDocCancelApproved).ToString();
                    docStatuses.Add(p3);
                }

                return (docStatuses);
            }
        }

        public ObservableCollection<MCashAccount> CashAccounts
        {
            get
            {
                return (cashAccounts);
            }
        }

        public ObservableCollection<MLocation> Locations
        {
            get
            {
                return (locations);
            }
        }

        public ObservableCollection<MLocation> BorrowReturnLocations
        {
            get
            {
                return (borrowReturnlocations);
            }
        }


        
        public ObservableCollection<MMasterRef> PackageTypes
        {
            get
            {
                return (packageTypes);
            }
        }

        private ObservableCollection<MMasterRef> filterPackageTypeByBroup(String grpID)
        {
            ObservableCollection<MMasterRef> temps = new ObservableCollection<MMasterRef>();
            foreach (MMasterRef m in packageTypes)
            {
                if (m.RefType.Equals(grpID))
                {
                    temps.Add(m);
                }
            }

            return (temps);
        }

        public ObservableCollection<MMasterRef> PackageTypes1
        {
            get
            {
                return (filterPackageTypeByBroup("1"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes2
        {
            get
            {
                return (filterPackageTypeByBroup("2"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes3
        {
            get
            {
                return (filterPackageTypeByBroup("3"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes4
        {
            get
            {
                return (filterPackageTypeByBroup("4"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes5
        {
            get
            {
                return (filterPackageTypeByBroup("5"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes6
        {
            get
            {
                return (filterPackageTypeByBroup("6"));
            }
        }

        public ObservableCollection<MMasterRef> PackageTypes7
        {
            get
            {
                return (filterPackageTypeByBroup("7"));
            }
        }

        public ObservableCollection<MItemCategory> ItemCategoryPaths
        {
            get
            {
                return (itemCategories);
            }
        }

        public ObservableCollection<MCycle> Cycle
        {
            get
            {
                return (cycle);
            }
        }
        public ObservableCollection<MCycle> Cycle_Weekly
        {
            get
            {
                return (cycle_weekly);
            }

            set
            {
            }
        }

        public ObservableCollection<MCycle> Cycle_Monthly
        {
            get
            {
                return (cycle_monthly);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Branches
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BRANCH);
                return (coll);
            }
        }

        private ObservableCollection<MBaseModel> xformToBaseModel<T>(ObservableCollection<T> srcs) where T : MBaseModel
        {
            ObservableCollection<MBaseModel> coll2 = new ObservableCollection<MBaseModel>();
            foreach (MBaseModel c in srcs)
            {
                coll2.Add(c);
            }

            return (coll2);
        }

        public ObservableCollection<MBaseModel> BranchesEx
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BRANCH);
                ObservableCollection<MBaseModel> coll2 = xformToBaseModel(coll);

                return (coll2);
            }
        }

        public ObservableCollection<MMasterRef> NamePrefixes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_NAME_PREFIX);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> AddressTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_ADDRESS_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> ItemTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_ITEM_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Brands
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BRAND);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> CustomerGroups
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_CUSTOMER_GROUP);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> CustomerTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_CUSTOMER_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> VoidReasons
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_VOID_REASON);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> SupplierGroups
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_SUPPLIER_GROUP);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> SupplierTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_SUPPLIER_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> LocationTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_LOCATION_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> MemberTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_MEMBER_TYPE);
                return (coll);
            }

            set
            {
            }
        }        

        public ObservableCollection<MMasterRef> BarcodeTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BARCODE_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> ServiceTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_SERVICE_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> ProjectGroups
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_PROJECT_GROUP);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Uoms
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_UOM);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Reasons
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_REASON_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Banks
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BANK);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Branchs
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BRANCH);
                return (coll);
            }
            set
            {
            }
        }

        public ObservableCollection<MMasterRef> EmployeeGroups
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_EMPLOYEE_GROUP);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> BankAccountTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_BANK_ACCOUNT_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> Currencies
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_CURRENCY);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> PaymentTerms
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_TERM_OF_PAYMENT);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> WHGroups
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_WH_GROUP);
                return (coll);
            }

            set
            {
            }
        }

        public ObservableCollection<MMasterRef> PosImportStatus
        {
            get
            {
                return (posImportStatus);
            }
        }

        public ObservableCollection<MUserGroup> UserGroups
        {
            get
            {
                return (userGroups);
            }
        }

        public ObservableCollection<MMasterRef> CreditTerms
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_CREDIT_TERM);
                return (coll);
            }
        }

        public ObservableCollection<MMasterRef> DiscountTypes
        {
            get
            {
                ObservableCollection<MMasterRef> coll = getMasterRefCollection(MasterRefEnum.MASTER_DISCOUNT_TYPE);
                return (coll);
            }

            set
            {
            }
        }

        #endregion

        public ObservableCollection<MMasterRef> Months
        {
            get
            {
                if (months.Count <= 0)
                {
                    MMasterRef mr0 = new MMasterRef(new CTable(""));
                    mr0.MasterID = "";
                    mr0.Description = "";
                    months.Add(mr0);

                    for (int i = 0; i < 12; i++)
                    {
                        int id = i + 1;
                        MMasterRef mr = new MMasterRef(new CTable(""));
                        mr.MasterID = id.ToString();
                        mr.Description = CUtil.IDToMonth(id);
                        months.Add(mr);
                    }
                }

                return (months);
            }
        }

        private static void loadVirtualDirectoryPathList(string category, ObservableCollection<MMasterRef> colls)
        {
            CTable tb = new CTable("");
            tb.SetFieldValue("CATEGORY", category);

            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetVirtualPathList", "DIRECTORY_LIST", tb);

            colls.Clear();

            CTable blank = new CTable("");
            MMasterRef b = new MMasterRef(blank);
            colls.Add(b);

            foreach (CTable o in arr)
            {
                CTable t = new CTable("");                                   
                MMasterRef mr = new MMasterRef(t);

                mr.MasterID = o.GetFieldValue("DIRECTORY_ID");
                string s = o.GetFieldValue("PATH");
                mr.Description = s;
                if (s.Length >= 1)
                {
                    mr.Description = s.Substring(1);
                }

                colls.Add(mr);
            }
        }

        public static void LoadEmployeeDepartments()
        {
            loadVirtualDirectoryPathList("1", employeeDepartments);
        }

        public ObservableCollection<MMasterRef> EmployeeDepartments
        {
            get
            {
                return (employeeDepartments);
            }
        }


        public static void LoadEmployeePositions()
        {
            loadVirtualDirectoryPathList("2", employeePositions);
        }

        public ObservableCollection<MMasterRef> EmployeePositions
        {
            get
            {
                return (employeePositions);
            }
        }
    }
}


