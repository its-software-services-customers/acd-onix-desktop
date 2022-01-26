using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MBranchConfigCenter : MBaseModel
    {
        private ObservableCollection<MBranchConfigPosCenter> details = new ObservableCollection<MBranchConfigPosCenter>();
        private int internalSeq = 0;

        public MBranchConfigCenter(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("branch_no", BranchCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("branch_name", BranchName);
            ttItems.Add(ct);

            ct = new CToolTipItem("key", Key);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
            IsCashVatResetByYear = true;
            IsDebtVatResetByYear = true;
            IsCashNoVatResetByYear = true;
            IsDebtNoVatResetByYear = true;
        }

        public String BranchConfigID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CONFIG_ID", value);
            }
        }

        public String BranchID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Key
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("KEY"));
            }

            set
            {
                GetDbObject().SetFieldValue("KEY", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String VoidBillPassword
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOID_BILL_PASSWORD"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOID_BILL_PASSWORD", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DefaultEmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEF_EMP_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEF_EMP_ID", value);
                NotifyPropertyChanged();
            }
        }

        #region Allow Negative

        public Boolean AllowInventoryNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_INVENTORY_NEGATIVE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_INVENTORY_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean AllowInventoryNegativeNV
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_INVENTORY_NEGATIVE_NV");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_INVENTORY_NEGATIVE_NV", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean AllowCashNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_CASH_NEGATIVE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_CASH_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean AllowCashNegativeNV
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_CASH_NEGATIVE_NV");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_CASH_NEGATIVE_NV", flag);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Branch
        public String BranchCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BranchObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BranchID = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;

                updateFlag();
                NotifyPropertyChanged("Locations");
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                if (branches == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(branches, BranchID));
            }
        }
        #endregion Branch

        #region CashAccountVat

        public String DefaultCashAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DEF_CASH_ACCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEF_CASH_ACCOUNT_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MCashAccount CashAccountVatObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    DefaultCashAccountID = m.CashAccountID;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MCashAccount> items = CMasterReference.Instance.CashAccounts;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, DefaultCashAccountID));
            }
        }

        #endregion

        #region CashAccountNoVat

        public String DefaultCashAccountIDNoVat
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DEF_CASH_ACCOUNT_ID_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEF_CASH_ACCOUNT_ID_NV", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MCashAccount CashAccountNoVatObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    DefaultCashAccountIDNoVat = m.CashAccountID;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MCashAccount> items = CMasterReference.Instance.CashAccounts;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, DefaultCashAccountIDNoVat));
            }
        }

        #endregion

        #region LocationVat
        public String DefaultLocationID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DEF_LOCATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEF_LOCATION_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MLocation LocationVatObj
        {
            set
            {
                MLocation m = value as MLocation;
                if (m != null)
                {
                    DefaultLocationID = m.LocationID;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MLocation> items = CMasterReference.Instance.Locations;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, DefaultLocationID));
            }
        }

        public int LocationIndex
        {
            get
            {
                ObservableCollection<MLocation> items = CMasterReference.Instance.Locations;
                if (items == null)
                {
                    return (0);
                }

                int id = CUtil.MasterIDToIndex(items, DefaultLocationID);
                return (id);
            }

            set
            {
            }
        }

        private ObservableCollection<MLocation> getLocationByBranchID(String branchID)
        {
            ObservableCollection<MLocation> temp = new ObservableCollection<MLocation>();

            foreach (MLocation ca in CMasterReference.Instance.Locations)
            {
                if (ca.BranchID.Equals(branchID))
                {
                    temp.Add(ca);
                }
            }

            return (temp);
        }

        public ObservableCollection<MLocation> Locations
        {
            get
            {
                return (getLocationByBranchID(BranchID));
            }
        }
        #endregion

        #region LocationNoVat

        public String DefaultLocationIDNoVat
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DEF_LOCATION_ID_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEF_LOCATION_ID_NV", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MLocation LocationNoVatObj
        {
            set
            {
                MLocation m = value as MLocation;
                if (m != null)
                {
                    DefaultLocationIDNoVat = m.LocationID;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MLocation> items = CMasterReference.Instance.Locations;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, DefaultLocationIDNoVat));
            }
        }

        #endregion


        #region Cash Vat Reset Criteria
        public String DocNoCashVatResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_RESET_CRITERIA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsCashVatResetByYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoCashVatResetCriteria.Equals("2"));
            }

            set
            {
                if (value)
                {
                    DocNoCashVatResetCriteria = "2";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsCashVatResetByMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoCashVatResetCriteria.Equals("1"));
            }

            set
            {
                if (value)
                {
                    DocNoCashVatResetCriteria = "1";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Debt Vat Reset Criteria
        public String DocNoDebtVatResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_RESET_CRITERIA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsDebtVatResetByYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoDebtVatResetCriteria.Equals("2"));
            }

            set
            {
                if (value)
                {
                    DocNoDebtVatResetCriteria = "2";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsDebtVatResetByMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoDebtVatResetCriteria.Equals("1"));
            }

            set
            {
                if (value)
                {
                    DocNoDebtVatResetCriteria = "1";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Cash No Vat Reset Criteria
        public String DocNoCashNoVatResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_NV_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_NV_RESET_CRITERIA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsCashNoVatResetByYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoCashNoVatResetCriteria.Equals("2"));
            }

            set
            {
                if (value)
                {
                    DocNoCashNoVatResetCriteria = "2";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsCashNoVatResetByMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoCashNoVatResetCriteria.Equals("1"));
            }

            set
            {
                if (value)
                {
                    DocNoCashNoVatResetCriteria = "1";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region Debt No Vat Reset Criteria
        public String DocNoDebtNoVatResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }
                
                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_NV_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_NV_RESET_CRITERIA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsDebtNoVatResetByYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoDebtNoVatResetCriteria.Equals("2"));
            }

            set
            {
                if (value)
                {
                    DocNoDebtNoVatResetCriteria = "2";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsDebtNoVatResetByMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (DocNoDebtNoVatResetCriteria.Equals("1"));
            }

            set
            {
                if (value)
                {
                    DocNoDebtNoVatResetCriteria = "1";

                    updateFlag();
                    NotifyPropertyChanged();
                }
            }
        }
        #endregion

        #region DocNoCash

        public String DocNoCashPrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_PREFIX"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_PREFIX", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashPattern
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_PATTERN"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_PATTERN", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashYearOffset
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_YEAR_OFFSET"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_YEAR_OFFSET", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_RESET_CRITERIA", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashSeqLength
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_SEQ_LENGTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_SEQ_LENGTH", value);
                NotifyPropertyChanged();
            }
        }

        #endregion DocNoCash

        #region DocNoDebt

        public String DocNoDebtPrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_PREFIX"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_PREFIX", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtPattern
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_PATTERN"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_PATTERN", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtYearOffset
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_YEAR_OFFSET"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_YEAR_OFFSET", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_RESET_CRITERIA", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtSeqLength
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_SEQ_LENGTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_SEQ_LENGTH", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region DocNoDebtNV

        public String DocNoDebtNVPrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_PREFIX_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_PREFIX_NV", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtNVPattern
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_PATTERN_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_PATTERN_NV", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtNVYearOffset
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_NV_YEAR_OFFSET"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_NV_YEAR_OFFSET", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtNVResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                
                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_NV_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_NV_RESET_CRITERIA", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoDebtNVSeqLength
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_DEBT_NV_SEQ_LENGTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_DEBT_NV_SEQ_LENGTH", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region DocNoCashNV

        public String DocNoCashNVPrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_PREFIX_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_PREFIX_NV", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashNVPattern
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_PATTERN_NV"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_PATTERN_NV", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashNVYearOffset
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_NV_YEAR_OFFSET"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_NV_YEAR_OFFSET", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashNVResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_NV_RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_NV_RESET_CRITERIA", value);
                NotifyPropertyChanged();
            }
        }

        public String DocNoCashNVSeqLength
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_NO_CASH_NV_SEQ_LENGTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_NO_CASH_NV_SEQ_LENGTH", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Branch POS
        public ObservableCollection<MBranchConfigPosCenter> BranchConfigPos
        {
            get
            {
                return (details);
            }

            set
            {
            }
        }

        public void AddBranchPOS()
        {
            CTable t = new CTable("");
            MBranchConfigPosCenter vp = new MBranchConfigPosCenter(t);

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("BRNCH_CONFIG_POS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("BRNCH_CONFIG_POS_ITEM", arr);
            }

            arr.Add(vp.GetDbObject());
            details.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;

            vp.ExtFlag = "A";
        }

        public void InitBranchPOSs()
        {
            details.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("BRNCH_CONFIG_POS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("BRNCH_CONFIG_POS_ITEM", arr);
            }

            foreach (CTable t in arr)
            {
                MBranchConfigPosCenter v = new MBranchConfigPosCenter(t);
                details.Add(v);

                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }
        }

        public void RemovePOSSeriesItem(MBranchConfigPosCenter vp)
        {
            removeAssociateItems(vp, "BRNCH_CONFIG_POS_ITEM", "INTERNAL_SEQ", "BRANCH_POS_ID");
            details.Remove(vp);
        }

        #endregion;

        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                NotifyPropertyChanged();
            }
        }
    }
}
