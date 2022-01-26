using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MSelectedItem : MBaseModel
    {    
        private MInventoryItem ini = new MInventoryItem(new CTable(""));
        private MService svm = new MService(new CTable(""));
        private MVoucherTemplate vct = new MVoucherTemplate(new CTable(""));

        private int seq = 0;

        public MSelectedItem(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {
        }

        public String BillSelectedItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BILL_SIM_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BILL_SIM_ITEM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String SelectionType
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
                GetDbObject().SetFieldValue("SELECTION_TYPE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

#region Item
        public MBaseModel ItemObj
        {
            get
            {
                ini.ItemID = ItemID;
                ini.ItemCode = ItemCode;
                ini.ItemNameThai = ItemNameThai;
                ini.ItemCategory = ItemCategory;
                ini.PricingDefination = PricingDefination;
                ini.DefaultSellPrice = DefaultSellPriceItem;

                return (ini);
            }

            set
            {
                if (value == null)
                {
                    ItemID = "";
                    ItemCode = "";
                    ItemNameThai = "";
                    ItemCategory = "";
                    PricingDefination = "";
                    DefaultSellPriceItem = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                ini.SetDbObject(ii.GetDbObject());

                ItemCategory = ii.ItemCategory;
                ItemID = ii.ItemID;
                ItemCode = ii.ItemCode;
                ItemNameThai = ii.ItemNameThai;
                PricingDefination = ii.PricingDefination;
                DefaultSellPriceItem = ii.DefaultSellPrice;
            }
        }

        public String ItemID
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
                updateFlag();
                GetDbObject().SetFieldValue("ITEM_ID", value);
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
                updateFlag();
                GetDbObject().SetFieldValue("ITEM_CATEGORY", value);
            }
        }

        public String ItemCode
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String PricingDefination
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
            }
        }

        public String ItemNameThai
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ItemNameEng
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DefaultSellPriceItem
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEFAULT_SELL_PRICE_ITEM"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEFAULT_SELL_PRICE_ITEM", value);
            }
        }
        #endregion

        #region Voucher
        public MBaseModel VoucherObj
        {
            get
            {
                vct.VoucherTempID = VoucherID;
                vct.VoucherTempNO = VoucherNo;
                vct.VoucherTempName = VoucherName;

                return (vct);
            }

            set
            {
                if (value == null)
                {
                    VoucherID = "";
                    VoucherNo = "";
                    VoucherName = "";

                    return;
                }

                MVoucherTemplate ii = (value as MVoucherTemplate);
                vct.SetDbObject(ii.GetDbObject());

                VoucherID = ii.VoucherTempID;
                VoucherNo = ii.VoucherTempNO;
                VoucherName = ii.VoucherTempName;
            }
        }

        public String VoucherID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOUCHER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOUCHER_ID", value);
                updateFlag();
            }
        }

        public String VoucherNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOUCHER_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOUCHER_NO", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String VoucherName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOUCHER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOUCHER_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region Service
        public MBaseModel ServiceObj
        {
            get
            {
                svm.ServiceID = ServiceID;
                svm.ServiceCode = ServiceCode;
                svm.ServiceName = ServiceName;
                svm.PricingDefinition = ServicePricingDefinition;
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

                    return;
                }

                MService ii = (value as MService);
                svm.SetDbObject(ii.GetDbObject());

                ServiceID = ii.ServiceID;
                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;
                ServicePricingDefinition = ii.PricingDefinition;
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
                updateFlag();
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
                updateFlag();
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
                updateFlag();
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
            }
        }
        #endregion

        public string FreeText
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
            }
        }

        public string FreeTextID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FREE_TEXT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("FREE_TEXT_ID", value);
            }
        }

        public String ItemQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String qtys = GetDbObject().GetFieldValue("QUANTITY");
                double qty = CUtil.StringToDouble(qtys);
                int q = (int)qty;

                return (q.ToString());
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Key
        {
            get
            {
                String k = "";

                if (SelectionType.Equals("1"))
                {
                    //Service
                    k = SelectionType + "-" + ServiceID;
                }
                else if (SelectionType.Equals("2"))
                {
                    //Item
                    k = SelectionType + "-" + ItemID;
                }
                else if (SelectionType.Equals("4"))
                {
                    //Voucher
                    k = SelectionType + "-" + VoucherID;
                }
                else if (SelectionType.Equals("5"))
                {
                    //Free Text
                    k = SelectionType + "-" + FreeTextID;
                }

                return (k);
            }
        }

        public String DisplayName
        {
            get
            {
                if (SelectionType.Equals("1"))
                {
                    return (ServiceName);
                }
                else if (SelectionType.Equals("2"))
                {
                    return (ItemNameThai);
                }
                else if (SelectionType.Equals("4"))
                {
                    return (VoucherName);
                }
                else if (SelectionType.Equals("5"))
                {
                    return (FreeText);
                }

                return (FreeText);
            }
        }

        public String DisplayCode
        {
            get
            {
                if (SelectionType.Equals("1"))
                {
                    return (ServiceCode);
                }
                else if (SelectionType.Equals("2"))
                {
                    return (ItemCode);
                }
                else if (SelectionType.Equals("4"))
                {
                    return (VoucherNo);
                }
                else if (SelectionType.Equals("5"))
                {
                    return ("N/A");
                }

                return ("N/A");
            }
        }

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

        public string EnabledFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENABLE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENABLE_FLAG", value);
            }
        }

        public string TrayFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TRAY_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("TRAY_FLAG", value);
            }
        }
    }
}
