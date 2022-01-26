using System;
using System.Collections.ObjectModel;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Pricing;
using Onix.Client.Models;

namespace Onix.Client.Model
{
    public class MBillSimulate : MBaseModel
    {
        private ObservableCollection<MSelectedItem> selectedItems = new ObservableCollection<MSelectedItem>();
        private int seq = 0;
        private MEntity cst = new MEntity(new CTable(""));
        private MMasterRef branch = new MMasterRef(new CTable(""));
        private Boolean isSelected = false;

        private ObservableCollection<MBillSimulateResulte> resultItemsCTable = new ObservableCollection<MBillSimulateResulte>();

        private ObservableCollection<CBasketItemDisplay> resultItems = new ObservableCollection<CBasketItemDisplay>();
        private ObservableCollection<CBasketItemDisplay> freeItems = new ObservableCollection<CBasketItemDisplay>();
        private ObservableCollection<CBasketItemDisplay> voucherItems = new ObservableCollection<CBasketItemDisplay>();
        private ObservableCollection<CBasketItemDisplay> postFrees = new ObservableCollection<CBasketItemDisplay>();
        private ObservableCollection<CProcessingResultGroup> processingItems = new ObservableCollection<CProcessingResultGroup>();

        private ObservableCollection<MCommissionProfileDisplay> commissionItems = new ObservableCollection<MCommissionProfileDisplay>();

        public MBillSimulate(CTable obj) : base(obj)
        {

        }

        public int FreeItemCount
        {
            get
            {
                return (freeItems.Count);
            }
        }

        public int VoucherItemCount
        {
            get
            {
                return (voucherItems.Count);
            }
        }

        public int PostFreeItemCount
        {
            get
            {
                return (postFrees.Count);
            }
        }

        private ObservableCollection<CBasketItemDisplay> getNameMapArray(String arrName)
        {
            Hashtable arrMap = new Hashtable()
            {
                { "BILL_RESULT_ITEM", resultItems },
                { "BILL_FREE_ITEM", freeItems },
                { "BILL_VOUCHER_ITEM", voucherItems },
                { "BILL_POSTGIFT_ITEM", postFrees }
            };

            return ((ObservableCollection<CBasketItemDisplay>) arrMap[arrName]);
        }

        private int getDisplayCategory(String arrName)
        {
            Hashtable arrMap = new Hashtable()
            {
                { "BILL_RESULT_ITEM", 1 },
                { "BILL_FREE_ITEM", 2 },
                { "BILL_VOUCHER_ITEM", 3 },
                { "BILL_POSTGIFT_ITEM", 4 }
            };

            return ((int)arrMap[arrName]);
        }

        public void CleanUp()
        {
            resultItems.Clear();
            freeItems.Clear();
            voucherItems.Clear();
            postFrees.Clear();
            processingItems.Clear();
            resultItemsCTable.Clear();
            selectedItems.Clear();
        }

        public String VoucherQtyText
        {
            get
            {
                int cnt = voucherItems.Count;
                if (cnt > 0)
                {
                    return (String.Format(" ({0})", cnt));
                }

                return ("");

            }
        }

        public String FreeQtyText
        {
            get
            {
                int cnt = freeItems.Count;
                if (cnt > 0)
                {
                    return (String.Format(" ({0})", cnt));
                }

                return ("");

            }
        }

        public String PostGiftQtyText
        {
            get
            {
                int cnt = postFrees.Count;
                if (cnt > 0)
                {
                    return (String.Format(" ({0})", cnt));
                }

                return ("");

            }
        }

        public Boolean IsSelected
        {
            set
            {
                isSelected = value;
                NotifyPropertyChanged();
            }

            get
            {
                return (isSelected);
            }
        }

        #region Method

        public void InitSelectedItems()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("BILL_SIMULATE_ITEM");

            if (arr == null)
            {
                return;
            }

            selectedItems.Clear();
            foreach (CTable t in arr)
            {
                MSelectedItem v = new MSelectedItem(t);
                v.Seq = seq;
                selectedItems.Add(v);

                v.ExtFlag = "I";
                seq++;
            }
        }

        public void AddSelectedItem(MSelectedItem sitm)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("BILL_SIMULATE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("BILL_SIMULATE_ITEM", arr);
            }

            sitm.ExtFlag = "A";
            sitm.Seq = seq;

            CTable t = sitm.GetDbObject();
            //t.SetFieldValue("INTERNAL_SEQ", seq.ToString());

            arr.Add(t);
            
            selectedItems.Add(sitm);

            seq++;
        }

        public int GetSelectedItemCount()
        {
            return (selectedItems.Count);
        }

        public void ClearSelectedItem()
        {
            ArrayList tempArr = new ArrayList();

            foreach (MSelectedItem item in selectedItems)
            {
                //Keep in temp buffer to prevent self array modified error
                tempArr.Add(item);
            }

            foreach (MSelectedItem item in tempArr)
            {
                DeleteSelectedItem(item);
            }
        }

        public void DeleteSelectedItem(MSelectedItem itm)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("BILL_SIMULATE_ITEM");
            int seq = itm.Seq;

            foreach (CTable t in arr)
            {
                int q = CUtil.StringToInt(t.GetFieldValue("INTERNAL_SEQ"));
                if (q == seq)
                {
                    if (itm.BillSelectedItemID.Equals(""))
                    {
                        arr.Remove(t);
                        break;
                    }
                    else
                    {
                        itm.ExtFlag = "D";
                    }
                }
            }

            selectedItems.Remove(itm);
            //arr.Remove(itm.GetDbObject());
        }

        #endregion

        public ObservableCollection<MSelectedItem> SelectedItems
        {
            get
            {
                return (selectedItems);
            }
        }

        public ObservableCollection<MCommissionProfileDisplay> CommissionItems
        {
            get
            {
                return (commissionItems);
            }
        }

        public void CreateDefaultValue()
        {
        }

        public String BillSimulateID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BILL_SIMULATE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BILL_SIMULATE_ID", value);
            }
        }

        public String DocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String BranchId
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
                NotifyPropertyChanged();
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DOCUMENT_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public DateTime? FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String DocumentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
            }
        }

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

        #region Customer
        public MBaseModel CustomerObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));

                cst.EntityID = CustomerID;
                cst.EntityName = CustomerName;
                cst.EntityCode = CustomerCode;
                cst.EntityType = CustomerType;
                cst.EntityGroup = CustomerGroup;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    CustomerID = "";
                    CustomerName = "";
                    CustomerCode = "";

                    return;
                }

                MEntity ii = (value as MEntity);
                cst.SetDbObject(ii.GetDbObject());

                CustomerID = ii.EntityID;
                CustomerName = ii.EntityName;
                CustomerCode = ii.EntityCode;
                CustomerType = ii.EntityType;
                CustomerGroup = ii.EntityGroup;
            }
        }

        public String CustomerID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CustomerCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String CustomerName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String CustomerType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String CustomerGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_GROUP", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Branch
        public MBaseModel BranchObj
        {
            get
            {
                MMasterRef branch = new MMasterRef(new CTable(""));
                branch.MasterID = BranchId;
                branch.Code = BranchCode;
                branch.Description = BranchName;

                return (branch);
            }

            set
            {
                if (value == null)
                {
                    BranchId = "";
                    BranchName = "";
                    BranchCode = "";

                    return;
                }

                MMasterRef ii = (value as MMasterRef);
                branch.SetDbObject(ii.GetDbObject().Clone());

                BranchId = ii.MasterID;
                BranchName = ii.Description;
                BranchCode = ii.Code;
            }
        }

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
                NotifyPropertyChanged();
            }
        }
        #endregion

        public DateTime SimulateTime
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("SIMULATE_TIME");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("SIMULATE_TIME", str);
                NotifyPropertyChanged();
            }
        }

        #region Amount
        public String TotalAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("TOTAL_AMOUNT");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMOUNT", value);
                NotifyPropertyChanged("TotalAmountFmt");
                NotifyPropertyChanged();
            }
        }

        public String TotalAmountFmt
        {
            set
            {
            }

            get
            {
                return (CUtil.FormatNumber(TotalAmount.ToString()));
            }
        }

        public String TotalAmountCommission
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("TOTAL_AMOUNT_COMMISSION");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMOUNT_COMMISSION", value);
                NotifyPropertyChanged("TotalAmountCommissionFmt");
                NotifyPropertyChanged();
            }
        }

        public String TotalAmountCommissionFmt
        {
            set
            {
            }

            get
            {
                return (CUtil.FormatNumber(TotalAmountCommission.ToString()));
            }
        }

        public String DiscountAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DISCOUNT_AMOUNT");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_AMOUNT", value);
                NotifyPropertyChanged("DiscountAmountFmt");
                NotifyPropertyChanged();
            }
        }

        public String DiscountAmountFmt
        {
            set
            {
            }

            get
            {
                return (CUtil.FormatNumber(DiscountAmount.ToString()));
            }
        }

        public String NetAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("NET_AMOUNT");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("NET_AMOUNT", value);
                NotifyPropertyChanged("NetAmountFmt");
            }
        }

        public String NetAmountFmt
        {
            set
            {
            }

            get
            {
                return (CUtil.FormatNumber(NetAmount.ToString()));
            }
        }

        public ObservableCollection<MBillSimulateResulte> ResultItemsCTable
        {
            get
            {
                return (resultItemsCTable);
            }
        }

        public ObservableCollection<CBasketItemDisplay> ResultItems
        {
            get
            {
                return (resultItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> VoucherItems
        {
            get
            {
                return (voucherItems);
            }
        }

        public ObservableCollection<CBasketItemDisplay> PostGiftItems
        {
            get
            {
                return (postFrees);
            }
        }

        public ObservableCollection<CBasketItemDisplay> FreeItems
        {
            get
            {
                return (freeItems);
            }
        }

        public ObservableCollection<CProcessingResultGroup> ProcessingTree
        {
            get
            {
                return (processingItems);
            }
        }

        public String SimulationFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("SIMULATION_FLAG");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("SIMULATION_FLAG", value);
            }
        }
        #endregion

        public void ClearAll()
        {
            SelectedItems.Clear();
            ResultItemsCTable.Clear();
            ResultItems.Clear();
            ProcessingTree.Clear();

            CommissionItems.Clear();

            CustomerObj = null;

            Note = "";
            IsModified = true;
            BillSimulateID = "";
            BranchObj = null;

            SetDbObject(new CTable(""));

            DocumentDate = DateTime.Now;
            SimulateTime = DateTime.Now;
        }

        #region Commission Items
        public void InitCommissionItems(ArrayList _CommList)
        {
            Double totalAmountCommission = 0;
            int Sequence = 0;
            commissionItems.Clear();
            //foreach (CTable tt in _CommList)
            //{
            //    ArrayList bItemList = tt.GetChildArray("BILL_ITEM");
            //    foreach (CTable t in bItemList)
            //    {
            //        MCommissionProfileDisplay v = new MCommissionProfileDisplay(t);
            //        commissionItems.Add(v);

            //        totalAmountCommission += CUtil.StringToDouble(v.Commission);

            //        t.SetFieldValue("INTERNAL_SEQ", seq.ToString());
            //        v.ExtFlag = "I";
            //        Sequence++;
            //        v.Seq = Sequence;
            //    }
            //    TotalAmountCommission = totalAmountCommission.ToString();
            //}
            foreach (CTable t in _CommList)
            {

                MCommissionProfileDisplay v = new MCommissionProfileDisplay(t);
                commissionItems.Add(v);

                totalAmountCommission += CUtil.StringToDouble(v.Commission);

                t.SetFieldValue("INTERNAL_SEQ", seq.ToString());
                v.ExtFlag = "I";
                Sequence++;
                v.Seq = Sequence;       
            }
            TotalAmountCommission = totalAmountCommission.ToString();
        }
        #endregion

        #region Promotion Items

        private void initItems(String arrName)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(arrName);

            ObservableCollection<CBasketItemDisplay> items = getNameMapArray(arrName);

            if (arr == null)
            {
                return;
            }

            items.Clear();

            foreach (CTable t in arr)
            {
                CBasketItemDisplay v = new CBasketItemDisplay(t);
                items.Add(v);
                v.Seq = seq;
                t.SetFieldValue("INTERNAL_SEQ", seq.ToString());
                v.ExtFlag = "I";
                seq++;
            }
        }

        public void InitPromotionItems()
        {
            initItems("BILL_RESULT_ITEM");
            initItems("BILL_FREE_ITEM");
            initItems("BILL_VOUCHER_ITEM");
            initItems("BILL_POSTGIFT_ITEM");
        }

        public void AddPromotionItem(CBasketItemDisplay itm, String arrName)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(arrName);
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray(arrName, arr);
            }

            itm.ExtFlag = "A";
            itm.Seq = seq;
            itm.DisplayCategory = getDisplayCategory(arrName);
            ObservableCollection<CBasketItemDisplay> items = getNameMapArray(arrName);

            CTable t = itm.CreateCTableObject();
            t.SetFieldValue("INTERNAL_SEQ", seq.ToString());
            arr.Add(t);

            seq++;

            items.Add(itm);
        }

        public void ClearPromotionItem(String arrName)
        {
            ArrayList tempArr = new ArrayList();

            ObservableCollection<CBasketItemDisplay> items = getNameMapArray(arrName);
            foreach (CBasketItemDisplay item in items)
            {
                //Keep in temp buffer to prevent self array modified error
                tempArr.Add(item);                
            }

            foreach (CBasketItemDisplay item in tempArr)
            {
                DeletePromotionItem(item, arrName);
            }
        }

        public void DeletePromotionItem(CBasketItemDisplay itm, String arrName)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray(arrName);
            int seq = itm.Seq;

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                int q = CUtil.StringToInt(t.GetFieldValue("INTERNAL_SEQ"));
                if (q == seq)
                {
                    if (itm.ID.Equals(""))
                    {
                        arr.Remove(t);
                        break;
                    }
                    else
                    {
                        t.SetFieldValue("EXT_FLAG", "D");
                        itm.ExtFlag = "D";
                    }
                }
            }

            ObservableCollection<CBasketItemDisplay> items = getNameMapArray(arrName);

            items.Remove(itm);
        }

        #endregion
    }
}
