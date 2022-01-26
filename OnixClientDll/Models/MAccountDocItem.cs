using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MAccountDocItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private ObservableCollection<MAuxilaryDocSubItem> itemDetails = new ObservableCollection<MAuxilaryDocSubItem>();

        private MInventoryItem itm = new MInventoryItem(new CTable(""));
        private MService svm = new MService(new CTable(""));
        private int seq = 0;
        private Boolean isAssignInternal = false;

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

        public int ItemCount
        {
            get
            {
                return (itemDetails.Count);
            }

            set
            {
            }
        }

        public MAccountDocItem(CTable obj) : base(obj)
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

        public Boolean? IsVoucherFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_VOUCH_FLAG");
                if (flag.Equals("N"))
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
                GetDbObject().SetFieldValue("IS_VOUCH_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsTrayFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_TRAY_FLAG");
                if (flag.Equals("N"))
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

                GetDbObject().SetFieldValue("IS_TRAY_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String IsVoucherIcon
        {
            get
            {
                String img = "";
                if (IsVoucherFlag.Equals(true))
                {
                    img = "pack://application:,,,/OnixClient;component/Images/free-icon.png";
                }
                return (img);
            }
        }

        public String IsTrayIcon
        {
            get
            {
                String img = "";
                if (IsTrayFlag.Equals(true))
                {
                    img = "pack://application:,,,/OnixClient;component/Images/warehouse-icon.png";
                }
                return (img);
            }
        }

        public String AccountDocItemId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ITEM_ID", value);
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

        #region Selection Type
        
        public MMasterRef SelectTypeMiscObj
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

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.ProductMiscUnSpecificSelectionTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, SelectType));
            }
        }


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

        public String SelectTypeMisc
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
                NotifyPropertyChanged("SelectTypeMiscObj");
                NotifyPropertyChanged("IsForLookup");
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
                itm = new MInventoryItem(new CTable(""));
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
                    IsVatFlag = null;

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
                svm = new MService(new CTable(""));
                svm.ServiceID = ServiceID;
                svm.ServiceCode = ServiceCode;
                svm.ServiceName = ServiceName;
                svm.PricingDefinition = ServicePricingDefinition;
                svm.ServiceUOMName = ServiceUOMName;
                svm.WHGroup = WhGroup;
                
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
                    WhGroup = "";
                    WhGroupCriteria = "";

                    return;
                }

                MService ii = (value as MService);

                ServiceID = ii.ServiceID;
                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;
                ServicePricingDefinition = ii.PricingDefinition;
                ServiceUOMName = ii.ServiceUOMName;
                IsWhTax = (Boolean)ii.IsWHTax;
                IsVatTax = (Boolean)ii.IsVatEligible;
                WHTaxPct = ii.WHTaxPct;
                WhGroup = ii.WHGroup;
                WhGroupCriteria = ii.WHGroup;
                
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

        public String Discount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT", value);
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

        public String DiscountFmt
        {
            get
            {
                if (Discount.Contains("%"))
                {
                    String _Discount = Discount.Replace("%", "");
                    return (CUtil.FormatNumber(_Discount) + "%");
                }
                else
                    return (CUtil.FormatNumber(Discount));
            }
        }

        public String DiscountAMT
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

                NotifyPropertyChanged();
                NotifyPropertyChanged("DiscountAMTFmt");

                updateFlag();
            }
        }

        public String DiscountAMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(DiscountAMT));
            }

            set
            {
            }
        }

        public Boolean? WH_TAX_FLAG
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("WH_TAX_FLAG");
                if (flag.Equals("N"))
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
                GetDbObject().SetFieldValue("WH_TAX_FLAG", flag);
                NotifyPropertyChanged();
                updateFlag();
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

        public void calculatePurchaseItemAmount()
        {
            Double v1 = CUtil.StringToDouble(Quantity);
            Double v2 = CUtil.StringToDouble(UnitPrice);
            Amount = "0.00";
            Amount = String.Format("{0:0.00}", (v1 * v2));

            Double v3 = GetFinalDiscountAMT(Discount, Amount);
            DiscountAMT = String.Format("{0:0.00}", v3);

            TotalAfterDiscount = "0.00";
            if (v1 != 0 || v2 != 0)
            {
                TotalAfterDiscount = String.Format("{0:0.00}", (v1 * v2 - v3));
            }
            TotalAmt = TotalAfterDiscount;
        }

        //This is being use in the POS
        public String GetConsolidateKey()
        {
            String key = String.Format("{0}-{1}-{2}-{3}", SelectType, ItemId, ServiceID, IsTrayFlag);
            return (key);
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
                NotifyPropertyChanged();
            }
        }

        public String ReferenceNumber
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("REFERENCE_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("REFERENCE_NUMBER", value);
                NotifyPropertyChanged();
            }
        }

        #region Extra fields

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

        public String RefWhDocNo
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("REF_WH_DOC_NO");
                return (temp);
            }

            set
            {
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

        public String CostAmtAvg
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("COST_AVG");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("COST_AVG", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ActualInvoiceNo
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("ACTUAL_INVOICE_NO");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("ACTUAL_INVOICE_NO", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DocumentType
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("DOCUMENT_TYPE");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_TYPE", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ActualDocumentNo
        {
            get
            {
                String temp = GetDbObject().GetFieldValue("ACTUAL_DOCUMENT_NO");
                return (temp);
            }

            set
            {
                GetDbObject().SetFieldValue("ACTUAL_DOCUMENT_NO", value);

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

        public String Value
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
                NotifyPropertyChanged("Value");
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

        public Boolean? IsVatFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("VAT_FLAG");
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

                GetDbObject().SetFieldValue("VAT_FLAG", flag);
            }
        }

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

        #endregion

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
            }

            get
            {
                MProject m = new MProject(new CTable(""));
                m.ProjectID = ProjectID;
                m.ProjectName = ProjectName;
                m.ProjectCode = ProjectCode;
                m.ProjectGroupName = ProjectGroupName;

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

        public String ActualDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACTUAL_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACTUAL_DOC_NO", value);
                NotifyPropertyChanged();
            }
        }

        public DateTime ActualDocumentDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("ACTUAL_DOC_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("ACTUAL_DOC_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ActualDocumentDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("ACTUAL_DOC_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        #region WH Group

        public String WhGroupCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP_CRITERIA", value);

                NotifyPropertyChanged();
                NotifyPropertyChanged("WHGroupCriteriaObj");

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

        public MMasterRef WHGroupCriteriaObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    WhGroupCriteria = m.MasterID;
                    //WHGroupName = m.Description;
                    //WHGroupNameEng = m.DescriptionEng;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = WhGroupCriteria;
                //mr.Description = WHGroupName;
                //mr.DescriptionEng = WHGroupNameEng;

                return (mr);
            }
        }

        #endregion

        public String PoID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String PoItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_ITEM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String PoCriteriaID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_CRITERIA_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_CRITERIA_ID", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

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
                    si.Description, si.Quantity, si.SubItemDateStr, si.UnitPrice, si.Amount, si.Remark);

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
                v.SubItemDateStr = fields[2];
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

            NotifyPropertyChanged("ItemCount");
        }

        public void InsertItemDetail(int index, MAuxilaryDocSubItem pi)
        {
            itemDetails.Insert(index, pi);
            rearrangeIndex();

            NotifyPropertyChanged("ItemCount");
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
    }
}

