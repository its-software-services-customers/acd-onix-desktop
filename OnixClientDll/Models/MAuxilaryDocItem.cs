using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Collections;

namespace Onix.Client.Model
{
    public class MAuxilaryDocItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private ObservableCollection<MAuxilaryDocSubItem> itemDetails = new ObservableCollection<MAuxilaryDocSubItem>();
        private Boolean isAssignInternal = false;
        private int seq = 0;

        #region ItemDetails

        public void SerializeItemDetails()
        {
            string[] sArr = new string[itemDetails.Count];

            int i = 0;
            foreach (MAuxilaryDocSubItem si in itemDetails)
            {
                if (si.ExtFlag.Equals("D"))
                {
                    continue;
                }

                String token = String.Format("{0}|{1}|{2}|{3}|{4}|{5}", 
                    si.Description, si.Quantity, si.Unit, si.UnitPrice, si.Amount, si.Remark);

                sArr[i] = token;
                i++;
            }

            ItemDetail = String.Join(";", sArr);
        }

        public void InitItemDetails()
        {
            string[] tupples = ItemDetail.Split(';');
            int len = tupples.Length;

            itemDetails.Clear();

            for (int i = 0; i < len; i++)
            {
                String tupple = tupples[i];
                if (tupple.Equals(""))
                {
                    continue;
                }

                string[] fields = tupple.Split('|');

                MAuxilaryDocSubItem v = new MAuxilaryDocSubItem(new CTable(""));
                v.Description = fields[0];
                v.Quantity = fields[1];
                v.Unit = fields[2];
                v.UnitPrice = fields[3];
                v.Amount = fields[4];
                v.Remark = fields[5];

                itemDetails.Add(v);
            }

            rearrangeIndex();
        }

        public void AddItemDetail(MAuxilaryDocSubItem pi)
        {
            itemDetails.Add(pi);
            rearrangeIndex();            
        }

        public void InsertItemDetail(int index, MAuxilaryDocSubItem pi)
        {
            itemDetails.Insert(index, pi);
            rearrangeIndex();
        }

        public ObservableCollection<MAuxilaryDocSubItem> ItemDetails
        {
            get
            {
                return (itemDetails);
            }
        }

        public void RemoveItemDetail(MAuxilaryDocSubItem pi)
        {
            itemDetails.Remove(pi);
            rearrangeIndex();
        }

        private void rearrangeIndex()
        {
            int idx = 0;
            foreach (MAuxilaryDocSubItem si in itemDetails)
            {
                idx++;
                si.Index = idx.ToString();
            }
        }

        public void CalculateSubItemTotal()
        {
            double total = 0.00;
            foreach (MAuxilaryDocSubItem si in itemDetails)
            {
                double amt = CUtil.StringToDouble(si.Amount);
                total = total + amt;
            }

            Quantity = "1.00";
            UnitPrice = total.ToString();
            TotalAmt = total.ToString();
        }

        public String ItemDetail
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_DETAIL"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_DETAIL", value);
                updateFlag();
            }
        }

        #endregion

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_date", DocumentDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("name", EntityName);
            ttItems.Add(ct);

            ct = new CToolTipItem("note", ItemNote);
            ttItems.Add(ct);
        }

        public MAuxilaryDocItem(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
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

        public String AuxilaryDocItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_ITEM_ID", value);
            }
        }

        public String AuxilaryDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_ID", value);
            }
        }

        #region Selection Type

        public MMasterRef SelectTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                SelectType = m.MasterID;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.ProductUnSpecificSelectionTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, SelectType));
            }
        }

        public String SelectType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SELECTION_TYPE"));
            }

            set
            {
                if (GetDbObject() == null)
                {
                    return;
                }

                GetDbObject().SetFieldValue("SELECTION_TYPE", value);
                NotifyPropertyChanged("SelectTypeObj");
                NotifyPropertyChanged("IsForLookup");
            }
        }

        public String SelectTypeDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SELECTION_TYPE_DESC"));
            }

            set
            {
                if (GetDbObject() == null)
                {
                    return;
                }

                GetDbObject().SetFieldValue("SELECTION_TYPE_DESC", value);
            }
        }
#endregion

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        public MBaseModel TempObj
        {
            get
            {
                if (SelectType.Equals("1"))
                {
                    return (ServiceObj);
                }

                return (ItemObj);
            }

            set
            {
                if (SelectType.Equals("1"))
                {
                    ServiceObj = value;
                }
                else
                {
                    ItemObj = value;
                }

                updateFlag();
            }
        }

        #region Item
        public MBaseModel ItemObj
        {
            get
            {
                MInventoryItem itm = new MInventoryItem(new CTable(""));
                itm.ItemID = ItemId;
                itm.ItemCode = ItemCode;
                itm.ItemNameEng = ItemNameEng;
                itm.ItemNameThai = ItemNameThai;
                itm.ItemCategory = ItemCategory;
                itm.PricingDefination = PricingDefinition;
                itm.ItemUOMName = ItemUOMName;
                //itm.IsVatEligible = IsVatFlag;

                return (itm);
            }

            set
            {
                if (value == null)
                {
                    ItemId = "";
                    ItemCode = "";
                    ItemNameEng = "";
                    ItemNameThai = "";
                    ItemCategory = "";
                    PricingDefinition = "";
                    ItemUOMName = "";
                    IsVatTax = false;

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);

                ItemId = ii.ItemID;
                ItemCode = ii.ItemCode;
                ItemNameEng = ii.ItemNameEng;
                ItemNameThai = ii.ItemNameThai;
                ItemCategory = ii.ItemCategory;
                PricingDefinition = ii.PricingDefination;
                ItemUOMName = ii.ItemUOMName;
                IsVatTax = (Boolean)ii.IsVatEligible;

                updateFlag();
            }
        }

        public String ItemCategory
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemCode //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemNameThai //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_NAME_THAI"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_NAME_THAI", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemNameEng //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_NAME_ENG", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String PricingDefinition //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_DEFINITION", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemUOMName //Temp
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_UOM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UOM_NAME", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("UOMName");

                updateFlag();
            }
        }
        #endregion

        #region Service
        public MBaseModel ServiceObj
        {
            get
            {
                MService svm = new MService(new CTable(""));
                svm.ServiceID = ServiceID;
                svm.ServiceCode = ServiceCode;
                svm.ServiceName = ServiceName;
                svm.PricingDefinition = ServicePricingDefinition;
                svm.ServiceUOMName = ServiceUOMName;

                return (svm);
            }

            set
            {
                if (value == null)
                {
                    ServiceID = "";
                    ServiceCode = "";
                    ServiceName = "";
                    ServicePricingDefinition = "";
                    ServiceUOMName = "";
                    IsWhTax = false;
                    IsVatTax = false;

                    return;
                }

                MService ii = (value as MService);

                ServiceID = ii.ServiceID;
                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;
                ServicePricingDefinition = ii.PricingDefinition;
                ServiceUOMName = ii.ServiceUOMName;
                IsWhTax = (Boolean) ii.IsWHTax;
                IsVatTax = (Boolean) ii.IsVatEligible;
                WHTaxPct = ii.WHTaxPct;                

                updateFlag();
            }
        }

        public String ServiceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_ID", value);
            }
        }

        public String ServiceCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceUOMName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_UOM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_UOM_NAME", value);
                NotifyPropertyChanged("UOMName");
                NotifyPropertyChanged();
            }
        }

        public String ServicePricingDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_PRICING_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_PRICING_DEFINITION", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        public String SelectItemCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Code = "";

                if (SelectType.Equals("1"))
                {
                    Code = ServiceCode;
                }
                else if (SelectType.Equals("2"))
                {
                    Code = ItemCode;
                }
                else
                {
                    Code = CLanguage.getValue("free_text");
                }

                return (Code);
            }

            set
            {
            }
        }

        public String SelectItemName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Code = "";

                if (SelectType.Equals("1"))
                {
                    Code = ServiceName;
                }
                else if (SelectType.Equals("2"))
                {
                    Code = ItemNameThai;
                }
                else
                {
                    Code = FreeText;
                }

                return (Code);
            }

            set
            {
            }
        }

        public String SelectItemNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Code = "";

                if (SelectType.Equals("1"))
                {
                    Code = ServiceName;
                }
                else if (SelectType.Equals("2"))
                {
                    Code = ItemNameEng;
                }
                else
                {
                    Code = FreeText;
                }

                return (Code);
            }

            set
            {
            }
        }

        private void calculateAmount()
        {
            double amt = CUtil.StringToDouble(Quantity) * CUtil.StringToDouble(UnitPrice);
            Amount = amt.ToString();

            double discountPct = CUtil.StringToDouble(DiscountPct);
            double discount = 0;
            if (IsDiscountByPct)
            {
                discount = amt * discountPct / 100;
                isAssignInternal = true;
                Discount = discount.ToString();
                isAssignInternal = false;
            }
            else
            {
                discount = CUtil.StringToDouble(Discount);
            }

            double total = amt - discount;

            TotalAfterDiscount = total.ToString();
            TotalAmt = TotalAfterDiscount;
        }       

        public String Quantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUANTITY"));
            }

            set
            {               
                GetDbObject().SetFieldValue("QUANTITY", value);
                calculateAmount();

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
        }

        public String UnitPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("UNIT_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("UNIT_PRICE", value);
                calculateAmount();

                NotifyPropertyChanged("UnitPriceFmt");
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String UnitPriceFmt
        {
            get
            {
                return (CUtil.FormatNumber(UnitPrice));
            }

            set
            {
            }
        }

        public String Amount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AMOUNT", value);

                NotifyPropertyChanged();
                NotifyPropertyChanged("AmountFmt");
                updateFlag();
            }
        }

        public String AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Amount));
            }

            set
            {
            }
        }

        public String DiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Discount));
            }

            set
            {
            }
        }

        public String TOTAL_AMT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return GetDbObject().GetFieldValue("TOTAL_AMT");
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMT", value);
                NotifyPropertyChanged("TOTAL_AMTFmt");
                NotifyPropertyChanged();
            }
        }

        public String TOTAL_AMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(TOTAL_AMT));
            }
        }

        public String TotalAfterDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("TOTAL_AFTER_DISCOUNT").ToString());
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AFTER_DISCOUNT", value);
                NotifyPropertyChanged("TotalAfterDiscountFmt");
                NotifyPropertyChanged();
            }
        }

        public String TotalAfterDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAfterDiscount));
            }

            set
            {
            }
        }

        public String UOMName
        {
            get
            {
                if (SelectType.Equals("1"))
                {
                    return (ServiceUOMName);
                }

                return (ItemUOMName);
            }

            set
            {
            }
        }

        public String ItemNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("ITEM_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_NOTE", value);
                NotifyPropertyChanged();
            }
        }

        public String WhGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        #region For Searching

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

        public String EntityID
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

        #endregion

        #region Extra fields

        public Boolean IsWhTax
        {
            get
            {
                if (WHTaxFlag.Equals("Y"))
                {
                    return (true);
                }

                return (false);
            }

            set
            {
                if (value)
                {
                    WHTaxFlag = "Y";
                }
                else
                {
                    WHTaxFlag = "N";
                }

                NotifyPropertyChanged();
            }
        }

        public String WHTaxFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("WH_TAX_FLAG");
                return (flag);
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_FLAG", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String WHTaxPct
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("WH_TAX_PCT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_PCT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String WHTaxAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("WH_TAX_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);

                NotifyPropertyChanged();
                NotifyPropertyChanged("WHTaxAmtFmt");
                updateFlag();
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

        public Boolean IsVatTax
        {
            get
            {
                if (VatTaxFlag.Equals("Y"))
                {
                    return (true);
                }

                return (false);
            }

            set
            {
                if (value)
                {
                    VatTaxFlag = "Y";
                }
                else
                {
                    VatTaxFlag = "N";
                }

                NotifyPropertyChanged();
            }
        }

        public String VatTaxFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("VAT_TAX_FLAG");
                return (flag);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_TAX_FLAG", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String VatTaxPct
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("VAT_TAX_PCT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_TAX_PCT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String VatTaxAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(VatTaxAmt));
            }
        }

        public String VatTaxAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("VAT_TAX_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_TAX_AMT", value);

                NotifyPropertyChanged();
                NotifyPropertyChanged("VatTaxAmtFmt");
                updateFlag();
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

        public String RevenueExpenseAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("REVENUE_EXPENSE_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DocumentNo
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("DOCUMENT_NO");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DocumentDate
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_DATE", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                String str = DocumentDate;
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);
                return (str2);
            }
        }

        public String ArApAmt
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("AR_AP_AMT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("AR_AP_AMT", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String TotalAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return GetDbObject().GetFieldValue("TOTAL_AMT");
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMT", value);
                NotifyPropertyChanged("TotalAmtFmt");
                NotifyPropertyChanged();
            }
        }

        public String TotalAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmt));
            }

            set
            {
            }
        }

        public String FinalDiscountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINAL_DISCOUNT_AVG"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINAL_DISCOUNT_AVG", value);
                NotifyPropertyChanged();
                //NotifyPropertyChanged("FinalDiscountAvgFmt");
            }
        }

        #endregion

        public Boolean IsForLookup
        {
            get
            {
                return (SelectType.Equals("1") || SelectType.Equals("2"));
            }

            set
            {
            }
        }

        public String FreeText
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("FREE_TEXT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FREE_TEXT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region Unit

        public String ItemUnitName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_UNIT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UNIT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemUnitNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_UNIT_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UNIT_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceUnitName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_UNIT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_UNIT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceUnitNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_UNIT_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_UNIT_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String UnitName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fldName = "";
                if (SelectType.Equals("1"))
                {
                    fldName = ServiceUnitName;
                }
                else
                {
                    fldName = ItemUnitName;
                }

                return (fldName);
            }

            set
            {
            }
        }

        public String UnitNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String fldName = "";
                if (SelectType.Equals("1"))
                {
                    fldName = ServiceUnitNameEng;
                }
                else
                {
                    fldName = ItemUnitNameEng;
                }

                return (fldName);
            }

            set
            {
            }
        }

        #endregion

        #region Project

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

        public String PoInvoiceRefType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_INVOICE_REF_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_INVOICE_REF_TYPE", value);
            }
        }

        public String RefByDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_BY_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_BY_DOC_NO", value);
            }
        }


        public Boolean? IsIncludable
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INCLUDE_ABLE_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("N"));
            }

            set
            {
                String flag = "Y";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "N";
                }

                GetDbObject().SetFieldValue("INCLUDE_ABLE_FLAG", flag);
                NotifyPropertyChanged();
            }           
        }

        #region Discount 

        public String Discount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_AMT", value);
                if (!isAssignInternal)
                {
                    //Prevent infinite recursive
                    calculateAmount();
                }

                NotifyPropertyChanged("DiscountFmt");
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public Boolean IsDiscountByPct
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("DISCOUNT_PCT_FLAG");
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

                GetDbObject().SetFieldValue("DISCOUNT_PCT_FLAG", flag);
                calculateAmount();

                NotifyPropertyChanged();
                NotifyPropertyChanged("IsDiscountByAmount");
            }
        }

        public Boolean IsDiscountByAmount
        {
            get
            {
                return (!IsDiscountByPct);
            }

            set
            {

            }
        }

        public String DiscountPct
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("DISCOUNT_PCT");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_PCT", value);
                calculateAmount();

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        #endregion
    }
}

