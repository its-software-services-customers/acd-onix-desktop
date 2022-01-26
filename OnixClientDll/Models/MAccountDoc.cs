using System;
using System.Collections.ObjectModel;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MAccountDoc : MBaseModel
    {
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();

        private ObservableCollection<MAccountDocItem> accountItem = new ObservableCollection<MAccountDocItem>();
        private ObservableCollection<MAccountDocPayment> paymentItem = new ObservableCollection<MAccountDocPayment>();
        private ObservableCollection<MAccountDocDiscount> discountItem = new ObservableCollection<MAccountDocDiscount>();
        private ObservableCollection<MAccountDocDeposit> depositItem = new ObservableCollection<MAccountDocDeposit>();
        private ObservableCollection<MAccountDocPayment> paymentItemNoChange = new ObservableCollection<MAccountDocPayment>();
        private ObservableCollection<MAccountDocReceipt> receiptItem = new ObservableCollection<MAccountDocReceipt>();
        private ObservableCollection<MEntityAddress> entityAddresses = new ObservableCollection<MEntityAddress>();
        private ObservableCollection<MAccountDocItem> accountPoItem = new ObservableCollection<MAccountDocItem>();

        private MEntityAddress ea = new MEntityAddress(new CTable(""));
        private MEntity cst = new MEntity(new CTable(""));
        private MEmployee sm = new MEmployee(new CTable(""));
        private MBillSimulate billSim = new MBillSimulate(new CTable(""));
        private int internalSeq = 0;

        private double eligibleWhAmt = 0.00;
        private MBaseModel scrStage = null;

        private Boolean populateChqueAmt = false;

        public Boolean IsPopulateChequeAmt
        {
            get
            {
                return (populateChqueAmt);
            }

            set
            {
                populateChqueAmt = value;
            }
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_date", DocumentDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_desc", DocumentDesc);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_status", DocumentStatusDesc);
            ttItems.Add(ct);

            if ((DocumentTypeEnum == AccountDocumentType.AcctDocCashPurchase) ||
                (DocumentTypeEnum == AccountDocumentType.AcctDocDebtPurchase) ||
                (DocumentTypeEnum == AccountDocumentType.AcctDocMiscExpense))
            {
                ct = new CToolTipItem("expense", RevenueExpenseAmtFmt);
                ttItems.Add(ct);

                ct = new CToolTipItem("vat_amount", VatAmtFmt);
                ttItems.Add(ct);

                ct = new CToolTipItem("TotalVat", ArApAmtFmt);
                ttItems.Add(ct);
            }
            else if ((DocumentTypeEnum == AccountDocumentType.AcctDocDebtSale) ||
                (DocumentTypeEnum == AccountDocumentType.AcctDocCashSale) ||
                (DocumentTypeEnum == AccountDocumentType.AcctDocSaleOrder) ||
                (DocumentTypeEnum == AccountDocumentType.AcctDocMiscRevenue))
            {
                ct = new CToolTipItem("note", Note);
                ttItems.Add(ct);

                ct = new CToolTipItem("revenue", RevenueExpenseAmtFmt);
                ttItems.Add(ct);

                ct = new CToolTipItem("vat_amount", VatAmtFmt);
                ttItems.Add(ct);

                ct = new CToolTipItem("TotalVat", ArApAmtFmt);
                ttItems.Add(ct);
            }
        }

        public String PromotionTotalAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMOTION_TOTAL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMOTION_TOTAL_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        
        public AccountDocumentType DocumentTypeEnum
        {
            get
            {
                AccountDocumentType dt = (AccountDocumentType) CUtil.StringToInt(DocumentType);
                return (dt);
            }
        }

        public String IndexDocInclude
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INDEX_DOC_INCLUDE"));
            }

            set
            {
                GetDbObject().SetFieldValue("INDEX_DOC_INCLUDE", value);
                NotifyPropertyChanged();
            }
        }


        public String PromotionAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMOTION_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMOTION_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String PromotionFinalDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMOTION_FINAL_DISCOUNT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMOTION_FINAL_DISCOUNT_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String PromotionTotalAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PromotionTotalAmt));
            }

            set
            {
            }
        }

        public String PromotionAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(PromotionAmount));
            }

            set
            {
            }
        }

        public String PromotionFinalDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(PromotionFinalDiscount));
            }

            set
            {
            }
        }

        public void NotifyPromotionCalulation()
        {
            NotifyPropertyChanged("PromotionTotalAmtFmt");
            NotifyPropertyChanged("PromotionAmountFmt");
            NotifyPropertyChanged("PromotionFinalDiscountFmt");
        }

        public MAccountDoc(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public void CleanUp()
        {
            billSim.SetDbObject(new CTable(""));
            billSim.CleanUp();
            accountItem.Clear();
        }

        public MBillSimulate BillSimulate
        {
            get
            {
                CTable o = GetDbObject();
                ArrayList arr = o.GetChildArray("ACCOUNT_BILLSIM_ITEM");

                ArrayList items = new ArrayList();
                if (arr == null)
                {
                    o.AddChildArray("ACCOUNT_BILLSIM_ITEM", items);

                    items.Add(billSim.GetDbObject());
                    return (billSim);
                }

                if (arr.Count == 0)
                {
                    items.Add(billSim.GetDbObject());
                    return (billSim);
                }

                o = (CTable) arr[0];
                billSim.SetDbObject(o);

                return (billSim);
            }
        }

        public String FreeItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMO_FREE_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMO_FREE_COUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("FreeItemCountTxt");
                NotifyPropertyChanged("TotalFreeItemCountTxt");
            }
        }

        public String FreeItemCountTxt
        {
            get
            {
                if (CUtil.StringToInt(FreeItemCount) > 0)
                {
                    return (String.Format(" ({0})", FreeItemCount));
                }

                return ("");
            }
        }

        public String VoucherItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMO_VOUCHER_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMO_VOUCHER_COUNT", value);                
                NotifyPropertyChanged();
                NotifyPropertyChanged("VoucherItemCountTxt");
                NotifyPropertyChanged("TotalFreeItemCountTxt");
            }
        }

        public String VoucherItemCountTxt
        {
            get
            {
                if (CUtil.StringToInt(VoucherItemCount) > 0)
                {
                    return (String.Format(" ({0})", VoucherItemCount));
                }

                return ("");
            }
        }

        public String PostFreeItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMO_POSTFREE_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMO_POSTFREE_COUNT", value);                
                NotifyPropertyChanged();
                NotifyPropertyChanged("PostFreeItemCountTxt");
                NotifyPropertyChanged("TotalFreeItemCountTxt");
            }
        }

        public String PostFreeItemCountTxt
        {
            get
            {
                if (CUtil.StringToInt(PostFreeItemCount) > 0)
                {
                    return (String.Format(" ({0})", PostFreeItemCount));
                }

                return ("");
            }
        }

        public String TotalFreeItemCountTxt
        {
            get
            {
                int cnt = CUtil.StringToInt(FreeItemCount) + CUtil.StringToInt(PostFreeItemCount) + CUtil.StringToInt(VoucherItemCount);

                if (cnt > 0)
                {
                    return (String.Format(" ({0})", cnt));
                }

                return ("");
            }
        }

        public ObservableCollection<MError> ErrorItems
        {
            get
            {
                return (errorItems);
            }
        }

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int) InventoryDocumentStatus.InvDocApproved)
                {
                    return (false);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocCancelApproved)
                {
                    //Voided
                    return (false);
                }

                return (true);
            }
        }

        public String IsEnabledIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ENABLE_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public String AccountDocId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ID", value);
            }
        }

        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
            }
        }
        
        public DateTime ActualTxDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("ACTUAL_TX_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("ACTUAL_TX_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ActualTxDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("ACTUAL_TX_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
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

        public String BEDocumentDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToBEDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String Year
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                int year = (dt.Year) + 543;
                String str2 = year.ToString();

                return (str2);
            }

            set
            {
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

        public String BillSummaryDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BILL_SUMMARY_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("BILL_SUMMARY_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_DESC", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ChequeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_ID", value);
            }
        }

        public String ChequeNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_NO", value);
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

        public String DocumentStatusDesc
        {
            get
            {
                if (DocumentStatus.Equals(""))
                {
                    return ("");
                }

                InventoryDocumentStatus dt = (InventoryDocumentStatus)Int32.Parse(DocumentStatus);
                String str = CUtil.InvDocStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public String RvTaxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("RV_TAX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("RV_TAX_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public MBaseModel CustomerObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));
                cst.EntityID = EntityId;
                cst.EntityName = EntityName;
                cst.EntityCode = EntityCode;
                cst.EntityGroup = EntityGroup;
                cst.EntityType = EntityType;
                cst.CreditTerm = CreditTerm;
                cst.Phone = EntityPhone;
                cst.IDCardNumber = EntityIDNumber;
                cst.NamePrefixDesc = EntityNamePrefixDesc;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    EntityId = "";
                    EntityName = "";
                    EntityCode = "";
                    EntityGroup = "";
                    EntityType = "";
                    CreditTerm = "";
                    EntityPhone = "";
                    EntityIDNumber = "";
                    EntityNamePrefixDesc = "";

                    return;
                }

                MEntity ii = (value as MEntity);
                cst.SetDbObject(ii.GetDbObject());

                EntityId = ii.EntityID;
                EntityName = ii.EntityName;
                EntityCode = ii.EntityCode;
                EntityGroup = ii.EntityGroup;
                EntityType = ii.EntityType;
                CreditTerm = ii.CreditTerm;
                EntityPhone = ii.Phone;
                EntityIDNumber = ii.IDCardNumber;
                EntityNamePrefixDesc = ii.NamePrefixDesc;

                updateFlag();
            }
        }

        public MBaseModel SupplierObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));
                cst.EntityID = EntityId;
                cst.EntityName = EntityName;
                cst.EntityCode = EntityCode;
                cst.EntityGroup = EntityGroup;
                cst.EntityType = EntityType;
                cst.CreditTerm = CreditTerm;
                cst.Phone = EntityPhone;
                cst.Fax = EntityFax;
                cst.IDCardNumber = EntityIDNumber;
                cst.NamePrefixDesc = EntityNamePrefixDesc;
                cst.RvTaxType = RvTaxType;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    EntityId = "";
                    EntityName = "";
                    EntityCode = "";
                    EntityGroup = "";
                    EntityType = "";
                    CreditTerm = "";
                    EntityPhone = "";
                    EntityFax = "";
                    EntityIDNumber = "";
                    EntityNamePrefixDesc = "";
                    RvTaxType = "";

                    return;
                }

                MEntity ii = (value as MEntity);
                cst.SetDbObject(ii.GetDbObject());

                EntityId = ii.EntityID;
                EntityName = ii.EntityName;
                EntityCode = ii.EntityCode;
                EntityGroup = ii.EntityGroup;
                EntityType = ii.EntityType;
                CreditTerm = ii.CreditTerm;
                EntityPhone = ii.Phone;
                EntityFax = ii.Fax;
                EntityIDNumber = ii.IDCardNumber;
                EntityNamePrefixDesc = ii.NamePrefixDesc;
                RvTaxType = ii.RvTaxType;

                updateFlag();
            }
        }

        public MBaseModel EntityObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));
                cst.EntityID = EntityId;
                cst.EntityName = EntityName;
                cst.EntityCode = EntityCode;
                cst.EntityGroup = EntityGroup;
                cst.EntityType = EntityType;
                cst.CreditTerm = CreditTerm;
                cst.Phone = EntityPhone;
                cst.Fax = EntityFax;
                cst.IDCardNumber = EntityIDNumber;
                cst.NamePrefixDesc = EntityNamePrefixDesc;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    EntityId = "";
                    EntityName = "";
                    EntityCode = "";
                    EntityGroup = "";
                    EntityType = "";
                    CreditTerm = "";
                    EntityPhone = "";
                    EntityFax = "";
                    EntityIDNumber = "";
                    EntityNamePrefixDesc = "";

                    return;
                }

                MEntity ii = (value as MEntity);
                cst.SetDbObject(ii.GetDbObject());

                EntityId = ii.EntityID;
                EntityName = ii.EntityName;
                EntityCode = ii.EntityCode;
                EntityGroup = ii.EntityGroup;
                EntityType = ii.EntityType;
                CreditTerm = ii.CreditTerm;
                EntityPhone = ii.Phone;
                EntityFax = ii.Fax;
                EntityIDNumber = ii.IDCardNumber;
                EntityNamePrefixDesc = ii.NamePrefixDesc;

                updateFlag();
            }
        }


        public String EntityId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ID", value);
            }
        }

        public String EntityGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_GROUP", value);
            }
        }

        public String EntityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_TYPE", value);
            }
        }

        public String EntityNamePrefixDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX_DESC", value);
            }
        }

        public String EntityName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_NAME", value);
            }
        }

        public String EntityPhone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_PHONE", value);
            }
        }

        public String EntityFax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_FAX", value);
            }
        }

        public String EntityIDNumber
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ID_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("ID_NUMBER", value);
            }
        }

        public String RefDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_DOCUMENT_NO", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public DateTime RefDocDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("REF_DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("REF_DOCUMENT_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String RefDocDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("REF_DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String EntityCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_CODE", value);
            }
        }

        public String CreditTerm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM", value);

            }
        }

        #region Location
        public MLocation LocationObj
        {
            set
            {
                MLocation m = value as MLocation;
                if (m != null)
                {
                    LocationId = m.LocationID;
                    LocationCode = m.LocationCode;
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

                return ((MLocation) CUtil.IDToObject(items, "LocationID", LocationId));
            }
        }
        public String LocationId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID", value);
                NotifyPropertyChanged();
            }
        }
        public String LocationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String LocationCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public IList Locations
        {
            get
            {
                IList colls = CUtil.GetCollectionByID(CMasterReference.Instance.Locations, "BranchID", BranchId);
                return (colls);
            }
        }
        #endregion

        #region BranchObj
        public MMasterRef BranchObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BranchId = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;
                BranchNameEng = m.DescriptionEng;

                updateFlag();
                NotifyPropertyChanged("Locations");
                NotifyPropertyChanged("LocationObj");
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

                MMasterRef mr = CUtil.MasterIDToObject(branches, BranchId);
                if (mr == null)
                {
                    return(null);
                }

                BranchId = mr.MasterID;
                BranchCode = mr.Code;
                BranchName = mr.Description;
                BranchNameEng = mr.DescriptionEng;

                return (mr);
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
                NotifyPropertyChanged();
            }
        }

        public String BranchNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        public MBaseModel SalesmanObj
        {
            get
            {
                MEmployee em = new MEmployee(new CTable(""));
                em.EmployeeID = EmployeeID;
                em.EmployeeCode = EmployeeCode;
                em.EmployeeName = EmployeeName;
                em.EmployeeNameLastname = EmployeeNameLastname;

                return (em);
            }

            set
            {
                if (value == null)
                {
                    EmployeeID = "";
                    EmployeeCode = "";
                    EmployeeName = "";
                    EmployeeNameLastname = "";

                    return;
                }

                MEmployee ii = (value as MEmployee);
                sm.SetDbObject(ii.GetDbObject());

                EmployeeID = ii.EmployeeID;
                EmployeeCode = ii.EmployeeCode;
                EmployeeName = ii.EmployeeName;
                EmployeeNameLastname = ii.EmployeeNameLastname;

                updateFlag();
            }
        }
        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
            }
        }
        public String EmployeeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_CODE", value);
            }
        }

        public String EmployeeNameLastname
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME_LASTNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME_LASTNAME", value);
            }
        }

        //public String EmployeeNameLastname2
        //{
        //    get
        //    {
        //        if (GetDbObject() == null)
        //        {
        //            return ("");
        //        }

        //        return (EmployeeName + " " + EmployeeLastName);
        //    }

        //    set
        //    {
        //    }
        //}

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
            }
        }

        public String EmployeeLastName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_LAST_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_LAST_NAME", value);
            }
        }

        public String AccountSide
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_SIDE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_SIDE", value);
            }
        }

        public String FinalDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINAL_DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINAL_DISCOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("FinalDiscountFmt");

                updateFlag();
            }
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(FinalDiscount));
            }

            set
            {
            }
        }

        public String FinalDiscountAMT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINAL_DISCOUNT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINAL_DISCOUNT_AMT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public String FinalDiscountAMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(FinalDiscountAMT));
            }
        }

        public String ItemDiscountAMT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_DISCOUNT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_DISCOUNT_AMT", value);
                NotifyPropertyChanged("ItemDiscountAMTFmt");
                NotifyPropertyChanged();
            }
        }
        public String ItemDiscountAMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemDiscountAMT.ToString()));
            }
        }

        public String VAT_PCT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_PCT"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_PCT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public String VAT_PCTFmt
        {
            get
            {
                return (CUtil.FormatNumber(VAT_PCT));
            }
        }      

        public String TotalAmountAfterDiscount //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_AMT_AFTER_DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMT_AFTER_DISCOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public String TotalAmountAfterDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmountAfterDiscount));
            }
        }

        public String PaymentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TYPE", value);
            }
        }

        public String InventoryDocId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INVENTORY_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INVENTORY_DOC_ID", value);
            }
        }

        public String CashDocId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_DOC_ID", value);
            }
        }

        public String AR_TX_Type
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_TX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_TX_TYPE", value);
            }
        }

        public String AR_TX_Amount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_TX_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_TX_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String AR_TX_AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(AR_TX_Amount));
            }
        }

        public String Sum_Amount_Pay
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SUM_AMOUNT_PAY"));
            }

            set
            {
                GetDbObject().SetFieldValue("SUM_AMOUNT_PAY", value);
                NotifyPropertyChanged("Sum_Amount_PayFmt");
                NotifyPropertyChanged();
            }
        }
        public String Sum_Amount_PayFmt
        {
            get
            {
                return (CUtil.FormatNumber(Sum_Amount_Pay));
            }
        }

        public String Sum_Wh_Tax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SUM_WH_TAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("SUM_WH_TAX", value);
                NotifyPropertyChanged();
            }
        }
        public String Sum_Wh_TaxFmt
        {
            get
            {
                return (CUtil.FormatNumber(Sum_Wh_Tax));
            }
        }

        public String TotalAmountAfterDiscountInVAT //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_AMT_AFTER_DISCOUNT_IN_VAT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMT_AFTER_DISCOUNT_IN_VAT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public String TotalAmountAfterDiscountInVATFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmountAfterDiscountInVAT));
            }
        }

        public String AR_BeginAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_BEGIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_BEGIN_AMOUNT", value);
            }
        }

        public String AR_EndAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_END_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_END_AMOUNT", value);
            }
        }

        public MCashAccount AccountObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    CashAccountId = m.CashAccountID;
                    AccountNo = m.AccountNo;
                    AccountName = m.AccountName;

                    BankName = m.BankName;
                }
            }
        }
        public String CashAccountId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID", value);
            }
        }
        public String AccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NO", value);
                NotifyPropertyChanged();
            }
        }
        public String AccountDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (AccountNo.Equals(""))
                {
                    return ("");
                }

                return (AccountNo + "-" + BankName);
            }
        }
        public String AccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NNAME", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BankObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BankID = m.MasterID;
                BankName = m.Description;
            }
        }
        public String BankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_ID", value);
                NotifyPropertyChanged();
            }
        }
        public String BankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_NAME", value);
                NotifyPropertyChanged();
            }
        }    
        
        public Boolean? ForExpenseRevenue
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("DRCR_FOR_EXPENSE_REVENUE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("DRCR_FOR_EXPENSE_REVENUE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowARAPNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_AR_AP_NEGATIVE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_AR_AP_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowCashNegative
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
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_CASH_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowInventoryNegative
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
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_INVENTORY_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public String VATType
        {
            get
            {
                String defaultValue = "1";
                String type = GetDbObject().GetFieldValue("VAT_TYPE");

                if (GetDbObject() == null)
                {
                    return (defaultValue);
                }

                if (type.Equals(""))
                {
                    return (defaultValue);
                }

                return (type);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_TYPE", value);
                
                NotifyPropertyChanged("IsIncludeVat");
                NotifyPropertyChanged("IsExcludeVat");
                NotifyPropertyChanged("IsNoVat");
            }
        }

        public Boolean IsNoVat
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (VATType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    VATType = "1";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public Boolean IsVat
        {
            get
            {
                return (!IsNoVat);
            }

            set
            {
            }
        }

        public Boolean IsIncludeVat
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (VATType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    VATType = "2";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public Boolean IsExcludeVat
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (VATType.Equals("3"));
            }

            set
            {
                if (value)
                {
                    VATType = "3";
                    NotifyPropertyChanged("IsVat");
                }
            }
        }

        public Boolean IsPromotionMode
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_PROMOTION_MODE");
                if (GetDbObject() == null)
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

                GetDbObject().SetFieldValue("IS_PROMOTION_MODE", flag);
            }
        }

        public ObservableCollection<MAccountDocItem> AccountItem
        {
            get
            {
                return (accountItem);
            }
        }       

        private Double GetFinalDiscountAMT(String strDiscount, String strTotal)
        {
            Double discount = 0;
            if (strDiscount.Contains("%"))
            {
                strDiscount = strDiscount.Replace("%", "");
                Double cal = 0;
                Double c1 = CUtil.StringToDouble(strDiscount);
                Double c2 = CUtil.StringToDouble(strTotal);
                cal = c2 * c1 / 100;
                discount = cal;
            }
            else
            {
                Double c1 = CUtil.StringToDouble(strDiscount);
                discount = c1;
            }
            return discount;
        }

        //This is being use in the POS
        public void RemoveAccountDocItem(MAccountDocItem vp)
        {
            removeAssociateItems(vp, "ACCOUNT_DOC_ITEM", "INTERNAL_SEQ", "ACCOUNT_DOC_ITEM_ID");
            accountItem.Remove(vp);
        }

        //This is being use in the POS
        public void AddAccountDocItemConsolidate(MAccountDocItem di, Boolean includeBill)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_ITEM", arr);
            }

            String key = di.GetConsolidateKey();
            MAccountDocItem found = null;

            foreach (MAccountDocItem adi in accountItem)
            {
                if (adi.Equals("D"))
                {
                    continue;
                }

                if (includeBill)
                {
                    String skey = adi.GetConsolidateKey();
                    if (key.Equals(skey))
                    {
                        found = adi;
                        break;
                    }
                }
            }

            if (found == null)
            {
                di.ExtFlag = "A";
                arr.Add(di.GetDbObject());
                accountItem.Add(di);

                di.Seq = internalSeq;
                internalSeq++;
            }
            else
            {
                String flag = found.ExtFlag;
                if (!flag.Equals("A") && !flag.Equals("D"))
                {
                    found.ExtFlag = "E";
                }

                double currQty = CUtil.StringToDouble(found.Quantity);
                double qty = CUtil.StringToDouble(di.Quantity);
                found.Quantity = (qty + currQty).ToString();
            }
        }

        public void AddAccountDocItem(MAccountDocItem TxType)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_ITEM", arr);
            }
            TxType.ExtFlag = "A";
            arr.Add(TxType.GetDbObject());
            accountItem.Add(TxType);

            TxType.Seq = internalSeq;
            internalSeq++;
        }

        public void InitAccountDocItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_ITEM");

            if (arr == null)
            {
                accountItem.Clear();
                return;
            }

            accountItem.Clear();
            foreach (CTable t in arr)
            {
                MAccountDocItem v = new MAccountDocItem(t);

                accountItem.Add(v);
                v.ExtFlag = "I";
            }
        }

        public void RestoreAccountDocItem()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_ITEM");

            if (arr == null)
            {
                accountItem.Clear();
                return;
            }

            accountItem.Clear();
            foreach (CTable t in arr)
            {
                MAccountDocItem v = new MAccountDocItem(t);
                accountItem.Add(v);
            }
        }

        public void InitErrorItem()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ERROR_ITEM");

            if (arr == null)
            {
                return;
            }

            errorItems.Clear();
            foreach (CTable t in arr)
            {
                t.SetFieldValue("TYPE_ERROR", "AccountDoc");
                MError v = new MError(t);
                errorItems.Add(v);
            }
        }

        public String VatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("VatAmtFmt");
            }
        }

        public String VatAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(VatAmt));
            }

            set
            {
            }
        }

        public String WHTaxAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WHTaxAmtFmt");
            }
        }

        public String WHTaxPCT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX_PCT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_PCT", value);
                NotifyPropertyChanged();
            }
        }

        public String WHTaxAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(WHTaxAmt));
            }

            set
            {
            }
        }

        public String WHTax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WHTaxFmt");
            }
        }

        public String WHTaxFmt
        {
            get
            {
                return (CUtil.FormatNumber(WHTax));
            }

            set
            {
            }
        }

        public String ArApAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_AP_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_AP_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ArApAmtFmt");
            }
        }
        
        public String ArApAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ArApAmt));
            }

            set
            {
            }
        }

        public String CashReceiptAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String amt = GetDbObject().GetFieldValue("CASH_RECEIPT_AMT");
                return (amt);
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashReceiptAmtFmt");
            }
        }

        public String CashReceiptAmtReport
        {
            get
            {
                if (!DocumentNo.Equals(""))
                    return (CashReceiptAmtFmt);
                else
                    return ("");
            }

            set
            {
            }
        }

        public String RevenueExpenseAmtReport
        {
            get
            {
                if (!DocumentNo.Equals(""))
                    return (RevenueExpenseAmtFmt);
                else
                    return ("");
            }

            set
            {
            }
        }

        public String CashReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashReceiptAmt));
            }

            set
            {
            }
        }

        public String CashActualReceiptAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACTUAL_RECEIPT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACTUAL_RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashActualReceiptAmtFmt");
            }
        }

        public String CashActualReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashActualReceiptAmt));
            }

            set
            {
            }
        }

        public String RevenueExpenseForWhAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_FOR_WH_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_FOR_WH_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String RevenueExpenseForVatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String RevenueExpenseAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RevenueExpenseAmtFmt");
            }
        }

        public String RevenueExpenseAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(RevenueExpenseAmt));
            }

            set
            {
            }
        }

        public String PricingAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("PricingAmtFmt");
            }
        }

        public String PricingAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PricingAmt));
            }

            set
            {
            }
        }

        public String PricingAmtValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_AMT", value);
                NotifyPropertyChanged();

                //CalculateExtraFields();
                NotifyPropertyChanged("PricingAmt");
            }
        }

        public String PrimaryRevenueExpenseAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_REVENUE_EXPENSE_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_REVENUE_EXPENSE_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String PrimaryItemDiscountAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_ITEM_DISCOUNT_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_ITEM_DISCOUNT_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String PrimaryFinalDiscountAvgAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("PRIMARY_FINAL_DISCOUNT_AVG_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("PRIMARY_FINAL_DISCOUNT_AVG_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        private double GetSumPricingAmt()
        {
            double totalPricingAmt = 0.00;

            foreach (MAccountDocItem di in accountItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                double pricingAmt = CUtil.StringToDouble(di.TotalAmt);
                totalPricingAmt = totalPricingAmt + pricingAmt;
            }

            return (totalPricingAmt);
        }

        private double GetSumPricingAmtValue()
        {
            double totalPricingAmt = 0.00;

            foreach (MAccountDocItem di in accountItem)
            {
                double pricingAmt = CUtil.StringToDouble(di.Value);
                totalPricingAmt = totalPricingAmt + pricingAmt;
            }

            return (totalPricingAmt);
        }

        public void CalculateExtraFields()
        {
            String vt = VATType;
            double vatPct = CUtil.StringToDouble(VAT_PCT);

            double totalRevenueExpenseForWhAmt = 0.00; //เก็บค่า Revenue/Expense ที่คิด WH tax เท่านั้นต่อ invoice
            double totalRevenueExpenseForVatAmt = 0.00; //เก็บค่า Revenue/Expense ที่คิด Vat เท่านั้นต่อ invoice

            double totalVatAmt = 0.00;
            double totalWhAmt = 0.00;
            double totalRevenueExpenseAmt = 0.00;
            double totalArApAmt = 0.00;
            double totalItemDiscount = 0.00;

            double primaryTotalRevenueExpenseAmt = 0.00;
            double primaryTotalItemDiscountAmt = 0.00;
            double primaryTotalItemFinalDiscountAmt = 0.00;

            double finalDiscount = CUtil.StringToDouble(FinalDiscount);
            double totalPricingAmt = GetSumPricingAmt();
            double finalDiscRatio = 0.00;
            double totalQuantity = 0.00;

            if (finalDiscount > totalPricingAmt)
            {
                finalDiscount = totalPricingAmt;
            }

            if (totalPricingAmt != 0)
            {
                finalDiscRatio = finalDiscount / totalPricingAmt;
            }

            foreach (MAccountDocItem di in accountItem)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                //This field should be equal di.Amount - di.DiscountAmt
                double pricingAmt = CUtil.StringToDouble(di.TotalAmt);
                double rawAmount = CUtil.StringToDouble(di.Amount);

                double fcDiscount = finalDiscRatio * pricingAmt;                
                pricingAmt = pricingAmt - fcDiscount;

                double vatAmt = 0.00;
                double revenueExpenseAmt = 0.00;
                double primaryRevenueExpenseAmt = 0.00;
                double primaryItemDiscountAmt = 0.00;
                double primaryFinalDiscountAmt = 0.00;
                double arApAmt = 0.00;
                double discount = CUtil.StringToDouble(di.Discount);
                double qty = CUtil.StringToDouble(di.Quantity);

                revenueExpenseAmt = pricingAmt;
                primaryRevenueExpenseAmt = rawAmount;
                primaryItemDiscountAmt = discount;
                primaryFinalDiscountAmt = fcDiscount;

                if (vt.Equals("1"))
                {
                    //Novat
                    vatAmt = 0.00;
                    arApAmt = pricingAmt;
                }
                else if (vt.Equals("2"))
                {
                    if (di.IsVatTax.Equals(true))
                    {
                        revenueExpenseAmt = (100 * pricingAmt) / (vatPct + 100);
                        primaryRevenueExpenseAmt = (100 * rawAmount) / (vatPct + 100);
                        primaryItemDiscountAmt = (100 * discount) / (vatPct + 100);
                        primaryFinalDiscountAmt = (100 * fcDiscount) / (vatPct + 100);

                        //Vat Included
                        totalRevenueExpenseForVatAmt = totalRevenueExpenseForVatAmt + revenueExpenseAmt;
                        vatAmt = (vatPct / 100) * revenueExpenseAmt;
                        vatAmt = Math.Round(vatAmt, 2, MidpointRounding.AwayFromZero);
                        arApAmt = revenueExpenseAmt + vatAmt;
                    }
                    else
                    {
                        vatAmt = 0.00;
                        arApAmt = pricingAmt;
                    }
                }
                else
                {
                    if (di.IsVatTax == true)
                    {
                        //Vat Excluded (3)                 
                        totalRevenueExpenseForVatAmt = totalRevenueExpenseForVatAmt + revenueExpenseAmt;

                        vatAmt = (vatPct / 100) * revenueExpenseAmt;
                        vatAmt = Math.Round(vatAmt, 2, MidpointRounding.AwayFromZero); //ปัดเศษก่อนแล้วค่อยรวม 09/11/2018 , ปัดแบบ 0.5 ปัดขึ้น
                        arApAmt = revenueExpenseAmt + vatAmt;
                    }
                    else
                    {
                        vatAmt = 0.00;
                        arApAmt = pricingAmt;
                    }
                }

                double whTaxAmount = 0.00;
                if (di.IsWhTax)
                {
                    totalRevenueExpenseForWhAmt = totalRevenueExpenseForWhAmt + revenueExpenseAmt;
                    double whPct = CUtil.StringToDouble(di.WHTaxPct);                    
                    whTaxAmount = (whPct / 100) * revenueExpenseAmt;
                    whTaxAmount = Math.Round(whTaxAmount, 2, MidpointRounding.AwayFromZero); //ปัดเศษก่อนแล้วค่อยรวม 09/11/2018, ปัดแบบ 0.5 ปัดขึ้น
                }

                di.FinalDiscountAvg = fcDiscount.ToString();                
                di.VatTaxAmt = vatAmt.ToString();
                di.VatTaxPct = VAT_PCT;
                di.WHTaxAmt = whTaxAmount.ToString();
                di.ArApAmt = arApAmt.ToString();
                di.RevenueExpenseAmt = revenueExpenseAmt.ToString();

                di.PrimaryRevenueExpenseAmt = primaryRevenueExpenseAmt.ToString();
                di.PrimaryFinalDiscountAvgAmt = primaryFinalDiscountAmt.ToString();
                di.PrimaryItemDiscountAmt = primaryItemDiscountAmt.ToString();

                totalVatAmt = totalVatAmt + vatAmt;
                totalRevenueExpenseAmt = totalRevenueExpenseAmt + revenueExpenseAmt;
                totalWhAmt = totalWhAmt + whTaxAmount;
                totalArApAmt = totalArApAmt + arApAmt;
                totalItemDiscount = totalItemDiscount + discount;
                totalQuantity = totalQuantity + qty;

                primaryTotalRevenueExpenseAmt = primaryTotalRevenueExpenseAmt + primaryRevenueExpenseAmt;
                primaryTotalItemDiscountAmt = primaryTotalItemDiscountAmt + primaryItemDiscountAmt;
                primaryTotalItemFinalDiscountAmt = primaryTotalItemFinalDiscountAmt + primaryFinalDiscountAmt;

            } //For Loop

            PricingAmt = totalPricingAmt.ToString();            
            WHTaxAmt = totalWhAmt.ToString();
            VatAmt = totalVatAmt.ToString();
            ItemDiscountAMT = totalItemDiscount.ToString();
            RevenueExpenseAmt = totalRevenueExpenseAmt.ToString();

            double normalizeVat = totalRevenueExpenseForVatAmt * (vatPct / 100);
            double v1 = Math.Round(normalizeVat, 2, MidpointRounding.AwayFromZero);
            double v2 = Math.Round(totalVatAmt, 2, MidpointRounding.AwayFromZero);
            if (v1 != v2)
            {
                VatAmt = normalizeVat.ToString();
                totalArApAmt = totalRevenueExpenseAmt + normalizeVat;
            }

            ArApAmt = totalArApAmt.ToString();

            RevenueExpenseForWhAmt = totalRevenueExpenseForWhAmt.ToString();
            RevenueExpenseForVatAmt = totalRevenueExpenseForVatAmt.ToString();

            Quantity = totalQuantity.ToString();

            PrimaryRevenueExpenseAmt = primaryTotalRevenueExpenseAmt.ToString();
            PrimaryItemDiscountAmt = primaryTotalItemDiscountAmt.ToString();
            PrimaryFinalDiscountAvgAmt = primaryTotalItemFinalDiscountAmt.ToString();

            CashReceiptAmt = (totalArApAmt - totalWhAmt).ToString();

            //TODO : In some business we have to round the decimal value up or down, should be configurable
            //CashActualReceiptAmt = CUtil.RoundUp25(totalArApAmt - totalWhAmt).ToString();

            CashActualReceiptAmt = CashReceiptAmt;
        }

        public String DueDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }
        }

        public DateTime DueDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
               
                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);
                GetDbObject().SetFieldValue("DUE_DATE", str);
                NotifyPropertyChanged();
            }
        }
        
        public MBaseModel PosScreenStage
        {
            get
            {
                return (scrStage);
            }

            set
            {
                scrStage = value;
            }
        }

        public Boolean IsPayByCash
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (true);
                }

                return (DocumentType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    DocumentType = "1";
                }
                else
                {
                    DocumentType = "2";
                }
            }
        }

        public String Quantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0.00");
                }
                return (GetDbObject().GetFieldValue("QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("QuantityFmt");
                updateFlag();
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity));
            }

            set
            {
            }
        }

        public String ChangeAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHANGE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHANGE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ChangeAmtFmt");
            }
        }

        public String ChangeFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHANGE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHANGE_FLAG", value);
            }
        }
        
        public String ChangeAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ChangeAmt));
            }

            set
            {
            }
        }

        public String ReceiveAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIPT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ReceiveAmtFmt");
            }
        }

        public String ReceiveAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ReceiveAmt));
            }

            set
            {
            }
        }

        #region Entity Address

        public String EntityAddressID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityAddressFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ADDRESS_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ADDRESS_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MEntityAddress> EntityAddresses
        {
            get
            {
                return (entityAddresses);
            }

            set
            {
                entityAddresses = value;
                NotifyPropertyChanged();
            }
        }

        public void ReloadEntityAddresses(ObservableCollection<MEntityAddress> addresses)
        {
            entityAddresses.Clear();
            foreach (MEntityAddress me in addresses)
            {
                entityAddresses.Add(me);
            }

            NotifyPropertyChanged("EntityAddresses");
        }

        public void InitEntityAddresses()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ENTITY_ADDRESS_ITEMS");

            if (arr == null)
            {
                entityAddresses.Clear();
                return;
            }

            entityAddresses.Clear();
            foreach (CTable t in arr)
            {
                MEntityAddress v = new MEntityAddress(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    entityAddresses.Add(v);
                }
            }

            //NotifyPropertyChanged("AddressObj");
        }

        public MBaseModel AddressObj
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MEntityAddress> items = entityAddresses;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(entityAddresses, EntityAddressID));
            }

            set
            {
                if (value == null)
                {
                    EntityAddressID = "";
                    EntityAddress = "";
                    return;
                }

                MEntityAddress ii = (value as MEntityAddress);

                EntityAddressID = ii.EntityAddressID;
                EntityAddress = ii.Address;

                updateFlag();
            }
        }

        #endregion Entity Address

        #region AccountDoc Payment

        public ObservableCollection<MAccountDocPayment> PaymentItemsNoChange
        {
            get
            {
                return (paymentItemNoChange);
            }
        }

        public ObservableCollection<MAccountDocPayment> PaymentItems
        {
            get
            {
                return (paymentItem);
            }
        }

        public void RemoveAccountDocPayment(MAccountDocPayment vp)
        {
            removeAssociateItems(vp, "ACCOUNT_DOC_PAYMENTS", "INTERNAL_SEQ", "ACT_DOC_PAYMENT_ID");
            paymentItem.Remove(vp);
            paymentItemNoChange.Remove(vp);
        }

        public void AddAccountDocPayment(MAccountDocPayment vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_PAYMENTS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_PAYMENTS", arr);
            }

            vp.ExtFlag = "A";
            arr.Add(vp.GetDbObject());
            paymentItem.Add(vp);
            paymentItemNoChange.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;
        }

        public void InitAccountDocPayment()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_PAYMENTS");

            if (arr == null)
            {
                paymentItem.Clear();
                paymentItemNoChange.Clear();
                return;
            }

            paymentItem.Clear();
            paymentItemNoChange.Clear();

            foreach (CTable t in arr)
            {
                MAccountDocPayment v = new MAccountDocPayment(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    if (v.Direction.Equals("1"))
                    {
                        //Not include Change transaction
                        paymentItemNoChange.Add(v);                        
                    }

                    paymentItem.Add(v);
                }
                //v.ExtFlag = "I";
            }
        }

        #endregion

        #region AccountDoc Receipt

        public ObservableCollection<MAccountDocReceipt> ReceiptItems
        {
            get
            {
                return (receiptItem);
            }
        }

        public void RemoveAccountDocReceipt(MAccountDocReceipt vp)
        {
            removeAssociateItems(vp, "ACCOUNT_DOC_RECEIPTS", "INTERNAL_SEQ", "ACT_DOC_RECEIPT_ID");
            receiptItem.Remove(vp);
        }

        public void AddAccountDocReceipt(MAccountDocReceipt vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_RECEIPTS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_RECEIPTS", arr);
            }

            vp.ExtFlag = "A";
            arr.Add(vp.GetDbObject());
            receiptItem.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;
        }

        public void AddAccountDocReceipts(ArrayList items)
        {
            foreach (MAccountDocReceipt r in items)
            {
                AddAccountDocReceipt(r);
            }
        }

        public void InitAccountDocReceipt()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_RECEIPTS");

            if (arr == null)
            {
                receiptItem.Clear();
                return;
            }

            receiptItem.Clear();
            foreach (CTable t in arr)
            {
                MAccountDocReceipt v = new MAccountDocReceipt(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    receiptItem.Add(v);

                }
            }
        }

        #endregion

        #region Change Type

        public String ChangeType
        {
            get
            {
                String defaultValue = "1";
                String type = GetDbObject().GetFieldValue("CHANGE_TYPE");

                if (GetDbObject() == null)
                {
                    return (defaultValue);
                }

                if (type.Equals(""))
                {
                    return (defaultValue);
                }

                return (type);
            }

            set
            {
                GetDbObject().SetFieldValue("CHANGE_TYPE", value);

                NotifyPropertyChanged("IsChangeByCash");
                NotifyPropertyChanged("IsChangeByCredit");
            }
        }


        public Boolean IsChangeByCash
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ChangeType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    ChangeType = "1";
                }
            }
        }

        public Boolean IsChangeByCredit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ChangeType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    ChangeType = "2";
                }
            }
        }

        #endregion

        public void CalculateARAmountForDrCr()
        {
            double amt = CUtil.StringToDouble(RevenueExpenseAmt) + CUtil.StringToDouble(VatAmt);
            double receive = amt - CUtil.StringToDouble(WHTaxAmt);
            PricingAmt = RevenueExpenseAmt;
            ArApAmt = amt.ToString();
            CashReceiptAmt = receive.ToString();
            RevenueExpenseForWhAmt = RevenueExpenseAmt;
            RevenueExpenseForVatAmt = RevenueExpenseAmt;
        }
        
        public override SolidColorBrush RowColor
        {
            get
            {
                return(CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public String ExcludeDocSet
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXCLUDE_DOC_ID_SET"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXCLUDE_DOC_ID_SET", value);
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<MWhGroup> constructWhItems()
        {
            eligibleWhAmt = 0.00;
            ObservableCollection<MWhGroup> items = new ObservableCollection<MWhGroup>();

            if (WhDefinition.Equals(""))
            {
                return(items);
            }

            string[] tupples = WhDefinition.Split(';');
            int len = tupples.Length;

            Hashtable whHash = new Hashtable();
            Hashtable expHash = new Hashtable();
            for (int i = 0; i < len; i++)
            {
                String tupple = tupples[i];
                if (tupple.Equals(""))
                {
                    continue;
                }

                string[] fields = tupple.Split('|');

                String whGroup = fields[0];
                double whAmt = CUtil.StringToDouble(fields[1]);

                double expRevAmt = 0.00;
                if (fields.Length > 2)
                {
                    expRevAmt = CUtil.StringToDouble(fields[2]);
                }

                double amt = 0.00;                
                if (whHash.ContainsKey(whGroup))
                {
                    amt = (double)whHash[whGroup];
                }

                amt = amt + whAmt;
                whHash[whGroup] = amt;

                //Expense Revenue
                double exp = 0.00;

                if (expHash.ContainsKey(whGroup))
                {
                    exp = (double)expHash[whGroup];
                }

                exp = exp + expRevAmt;
                expHash[whGroup] = exp;
            }

            foreach (String key in whHash.Keys)
            {
                MMasterRef mr = CUtil.MasterIDToObject(CMasterReference.Instance.WHGroups, key);

                double whAmt = (double) whHash[key];
                double expAmt = (double)expHash[key];
                MWhGroup w = new MWhGroup(new CTable(""));

                w.WHGroup = key;

                w.WHGroupName = "อื่น ๆ";
                w.WHGroupNameEng = "Others";
                if (!mr.Code.Equals(""))
                {
                    w.WHGroupName = mr.Description;
                    w.WHGroupNameEng = mr.DescriptionEng;
                }

                w.WhTaxAmt = whAmt.ToString();
                w.RevenueExpenseAmt = expAmt.ToString();
                eligibleWhAmt = eligibleWhAmt + expAmt;

                items.Add(w);
            }

            return (items);
        }

        public ObservableCollection<MWhGroup> WhItems
        {
            get
            {
                return (constructWhItems());
            }

            set
            {
            }
        }

        public String TotalEligibleForWhFmt
        {
            get
            {
                return (CUtil.FormatNumber(eligibleWhAmt.ToString()));
            }
        }

        public String RefWhDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_WH_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_WH_DOC_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String RefPoNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_PO_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_PO_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String RefQuotationNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_QUOTATION_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_QUOTATION_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String RefQuotationID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_QUOTATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_QUOTATION_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String RefSaleOrderNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_SALE_ORDER_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_SALE_ORDER_NO", value);
                NotifyPropertyChanged();
            }
        }
        
        public String DocStatusSet
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS_SET"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS_SET", value);
                NotifyPropertyChanged();
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

        public String RefSaleOrderID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_SALE_ORDER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_SALE_ORDER_ID", value);
                NotifyPropertyChanged("IsRefToSaleOrder");
                NotifyPropertyChanged();
            }
        }

        public Boolean IsRefToSaleOrder
        {
            get
            {

                return (CUtil.StringToInt(RefSaleOrderID) > 0);
            }

            set
            {
            }
        }

        public String RefReceiptNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_RECEIPT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_RECEIPT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String WhDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_DEFINITION", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsSaleOrderInUsedByInvoice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                String flag = GetDbObject().GetFieldValue("SO_IN_USED_BY_INVOICE");
                return (flag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    GetDbObject().SetFieldValue("SO_IN_USED_BY_INVOICE", "Y");
                }
                else
                {
                    GetDbObject().SetFieldValue("SO_IN_USED_BY_INVOICE", "N");
                }

                NotifyPropertyChanged();
            }
        }

        public void ConstructWhDefinitionFromDocItem()
        {
            String definition = "";

            foreach (MAccountDocItem adi in accountItem)
            {
                if (adi.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (!adi.IsWhTax)
                {
                    continue;
                }

                if (!adi.WhGroupCriteria.Equals(""))
                {
                    adi.WhGroup = adi.WhGroupCriteria;
                }

                //if (adi.WhGroup.Equals(""))
                //{
                //    adi.WhGroup = adi.WhGroupCriteria;
                //}

                String field = String.Format("{0}|{1}|{2};", adi.WhGroup, adi.WHTaxAmt, adi.RevenueExpenseAmt);
                definition = definition + field;
            }

            WhDefinition = definition;
        }

        public void ConstructWhDefinitionFromCrDr()
        {
            String definition = String.Format("{0}|{1}|{2};", "", WHTaxAmt, RevenueExpenseAmt);
            WhDefinition = definition;
        }

        public void ConstructWhDefinitionFromReceiptItem()
        {
            String definition = "";

            foreach (MAccountDocReceipt adi in receiptItem)
            {
                if (adi.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (adi.WhDefinition.Trim().Equals(""))
                {
                    continue;
                }

                definition = definition + adi.WhDefinition.Trim();
            }

            WhDefinition = definition;
        }

        #region Project

        public MBaseModel ProjectObj
        {
            set
            {
                if (value == null)
                {
                    ProjectID = "";
                    ProjectName = "";
                    ProjectCode = "";

                    return;
                }

                MProject m = value as MProject;
                ProjectID = m.ProjectID;
                ProjectName = m.ProjectName;
                ProjectCode = m.ProjectCode;
                ProjectGroupName = m.ProjectGroupName;
                
                NotifyPropertyChanged("ProjectGroupName");
                NotifyPropertyChanged();
            }

            get
            {
                MProject m = new MProject(new CTable(""));
                m.ProjectID = ProjectID;
                m.ProjectName = ProjectName;
                m.ProjectCode = ProjectCode;

                return (m);
            }
        }

        public String ProjectCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_CODE", value);
            }
        }

        public String ProjectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_ID", value);
            }
        }

        public String ProjectName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_NAME", value);
            }
        }

        public String ProjectGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP_NAME", value);
            }
        }
        #endregion

        public String CashReceiveAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_RECEIVE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_RECEIVE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashReceiveAmtFmt");
            }
        }

        public String CashReceiveAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashReceiveAmt));
            }

            set
            {
            }
        }

        public String CashChangeAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_CHANGE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_CHANGE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashChangeAmtFmt");
            }
        }

        public String CashChangeAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashChangeAmt));
            }

            set
            {
            }
        }

        public String GetRefPoString()
        {
            String poStr = "";
            Hashtable tables = new Hashtable();

            foreach (MAccountDocItem mr in accountItem)
            {
                if (mr.ExtFlag.Equals("D"))
                {
                    continue;
                }

                String po = mr.RefPoNo;

                if (!po.Equals("") && !tables.ContainsKey(po))
                {
                    String sep = ", ";
                    if (poStr.Equals(""))
                    {
                        sep = "";
                    }

                    poStr = poStr + sep + po;
                    tables.Add(po, true);
                }
            }

            return (poStr.Trim());
        }

        public void CalculateReceiveAndChange()
        {
            int changeCnt = 0;
            double received = 0.00;
            double totalAmt = CUtil.StringToDouble(CashReceiptAmt);

            MAccountDocPayment changePmt = null;
            
            foreach (MAccountDocPayment mr in paymentItem)
            {
                if (mr.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (mr.Direction.Equals("2"))
                {
                    //Only one occurence
                    changePmt = mr;
                    changeCnt++;
                }
                else
                {
                    //เสมือนกับออกค่าธรรมเนียมให้
                    received = received + CUtil.StringToDouble(mr.PaidAmount) + CUtil.StringToDouble(mr.FeeAmount);
                }
            }

            if (changeCnt <= 0)
            {
                changePmt = new MAccountDocPayment(new CTable(""));
                changePmt.ExtFlag = "A";
                changePmt.RefundStatus = "1";
                AddAccountDocPayment(changePmt);
            }
            else
            {
                if (!changePmt.ExtFlag.Equals("A"))
                {
                    changePmt.ExtFlag = "E";
                }
            }

            changePmt.Direction = "2";
            changePmt.PaymentType = "1";
            changePmt.PaidAmount = (received - totalAmt).ToString();
            changePmt.ChangeType = ChangeType;

            CashReceiveAmt = received.ToString();
            CashChangeAmt = changePmt.PaidAmount;
        }

        #region Final Discount

        public ObservableCollection<MAccountDocDiscount> DiscountItems
        {
            get
            {
                return (discountItem);
            }
        }

        public void RemoveAccountDocDiscount(MAccountDocDiscount vp)
        {
            removeAssociateItems(vp, "ACCOUNT_DOC_DISCOUNTS", "INTERNAL_SEQ", "ACT_DOC_DISCOUNT_ID");
            discountItem.Remove(vp);
        }

        public void AddAccountDocDiscount(MAccountDocDiscount vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_DISCOUNTS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_DISCOUNTS", arr);
            }

            vp.ExtFlag = "A";
            arr.Add(vp.GetDbObject());
            discountItem.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;
        }

        public void InitAccountDocDiscount()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_DISCOUNTS");

            if (arr == null)
            {
                discountItem.Clear();
                return;
            }

            discountItem.Clear();

            foreach (CTable t in arr)
            {
                MAccountDocDiscount v = new MAccountDocDiscount(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    discountItem.Add(v);
                }
                //v.ExtFlag = "I";
            }
        }

        #endregion

        #region Deposit

        public ObservableCollection<MAccountDocDeposit> DepositItems
        {
            get
            {
                return (depositItem);
            }
        }

        public void RemoveAccountDocDeposit(MAccountDocDeposit vp)
        {
            removeAssociateItems(vp, "ACCOUNT_DOC_DEPOSITS", "INTERNAL_SEQ", "ACT_DOC_DEPOSIT_ID");
            depositItem.Remove(vp);
        }

        public void AddAccountDocDeposit(MAccountDocDeposit vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_DEPOSITS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_DOC_DEPOSITS", arr);
            }

            vp.ExtFlag = "A";
            arr.Add(vp.GetDbObject());
            depositItem.Add(vp);

            vp.Seq = internalSeq;
            internalSeq++;
        }

        public void InitAccountDocDeposit()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_DOC_DEPOSITS");

            if (arr == null)
            {
                depositItem.Clear();
                return;
            }

            depositItem.Clear();

            foreach (CTable t in arr)
            {
                MAccountDocDeposit v = new MAccountDocDeposit(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    depositItem.Add(v);
                }
                //v.ExtFlag = "I";
            }
        }

        #endregion

        public void InitAccountPoItems()
        {
            Hashtable poHash = new Hashtable();

            accountPoItem.Clear();

            foreach (MAccountDocItem md in accountItem)
            {
                if (md.Equals("D"))
                {
                    continue;
                }

                CTable t = md.GetDbObject().CloneAll();
                MAccountDocItem newMd = new MAccountDocItem(t);

                double vat = CUtil.StringToDouble(md.VatTaxAmt);
                double wh = CUtil.StringToDouble(md.WHTaxAmt);
                double revExp = CUtil.StringToDouble(md.RevenueExpenseAmt);
                double fdisc = CUtil.StringToDouble(md.FinalDiscountAvg);
                double amt = CUtil.StringToDouble(md.TotalAfterDiscount);
                double arAp = CUtil.StringToDouble(md.ArApAmt);

                double totVat = 0.00;
                double totWh = 0.00;
                double totRevExp = 0.00;
                double totFdisc = 0.00;
                double totAmt = 0.00;
                double totArAp = 0.00;

                String key = md.RefPoNo;
                newMd.ProjectCode = md.ProjectCode;

                if (!poHash.ContainsKey(key))
                {
                    poHash.Add(key, newMd);
                    accountPoItem.Add(newMd);

                    if (key.Equals(""))
                    {
                        newMd.RefPoNo = RefPoNo;
                    }
                }
                else
                {
                    newMd = (MAccountDocItem)poHash[key];

                    totVat = CUtil.StringToDouble(newMd.VatTaxAmt);
                    totWh = CUtil.StringToDouble(newMd.WHTaxAmt);
                    totRevExp = CUtil.StringToDouble(newMd.RevenueExpenseAmt);
                    totFdisc = CUtil.StringToDouble(newMd.FinalDiscountAvg);
                    totAmt = CUtil.StringToDouble(newMd.TotalAfterDiscount);
                    totArAp = CUtil.StringToDouble(newMd.ArApAmt);
                }

                totVat = totVat + vat;
                totWh = totWh + wh;
                totRevExp = totRevExp + revExp;
                totFdisc = totFdisc + fdisc;
                totAmt = totAmt + amt;
                totArAp = totArAp + arAp;

                double totCash = totAmt - totFdisc + totVat - totWh;

                newMd.VatTaxAmt = totVat.ToString();
                newMd.WHTaxAmt = totWh.ToString();
                newMd.RevenueExpenseAmt = totRevExp.ToString();
                newMd.FinalDiscountAvg = totFdisc.ToString();
                newMd.TotalAfterDiscount = totAmt.ToString();
                newMd.ArApAmt = totArAp.ToString();
                newMd.TotalAmt = totCash.ToString();
            }
        }

        public ObservableCollection<MAccountDocItem> AccountPoItems
        {
            get
            {
                return (accountPoItem);
            }
        }

        public Boolean? IsInvoiceAvailable
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INVOICE_AVAILABLE_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("INVOICE_AVAILABLE_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        #region WhPayType

        public Boolean IsWhPayType1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (WhPayType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    WhPayType = "1";
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsWhPayType2
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (WhPayType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    WhPayType = "2";
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsWhPayType3
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (WhPayType.Equals("3"));
            }

            set
            {
                if (value)
                {
                    WhPayType = "3";
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsWhPayType4
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (WhPayType.Equals("4"));
            }

            set
            {
                if (value)
                {
                    WhPayType = "4";
                    NotifyPropertyChanged();
                }
            }
        }

        public String WhPayType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_PAY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_PAY_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        public void CalculateDopositTotal()
        {
            ObservableCollection<MAccountDocDeposit> items = depositItem;
            double amtTotal = 0.00;

            foreach (MAccountDocDeposit mr in items)
            {
                if (mr.ExtFlag.Equals("D"))
                {
                    continue;
                }

                double amt = CUtil.StringToDouble(mr.DepositAmt);

                amtTotal = amtTotal + amt;
            }

            double total = amtTotal;

            CashActualReceiptAmt = total.ToString();
            CashReceiptAmt = total.ToString();
        }

        public Boolean IsInInventory()
        {
            int cnt = 0;
            ObservableCollection<MAccountDocItem> items = accountItem;

            foreach (MAccountDocItem mr in items)
            {
                if (mr.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (mr.SelectType.Equals("2"))
                {
                    cnt++;
                }
            }

            return (cnt > 0);
        }

        public Boolean IsChangeByDrCr
        {
            get
            {
                //CashChangeAmt
                Boolean flag = IsChangeByCredit && 
                    !CashChangeAmt.Equals("0.00") &&
                    !CashChangeAmt.Equals("0") &&
                    !CashChangeAmt.Equals("");

                return (flag);
            }
        }

        public DateTime VatMonthYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("VAT_MONTH_YEAR");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("VAT_MONTH_YEAR", str);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsInternalDrCr
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INTERNAL_DRCR_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("INTERNAL_DRCR_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String RefByInvoiceNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_BY_INVOICE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_BY_INVOICE_NO", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsVatClaimable
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("VAT_CLAIMABLE");
                if (GetDbObject() == null)
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

                GetDbObject().SetFieldValue("VAT_CLAIMABLE", flag);
            }
        }
    }
}