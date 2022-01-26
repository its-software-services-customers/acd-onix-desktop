using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MPackageTrayPriceDiscount : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        private MService srv = new MService(new CTable("SERVICE"));
        private MItemCategory itc = new MItemCategory(new CTable("ITEM_CATEGORY"));
        private int seq = 0;

        public MPackageTrayPriceDiscount(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public MBaseModel ItemCategoryObj
        {
            get
            {
                itc.ItemCategoryID = CategoryId;
                itc.CategoryName = CategoryName;

                return (itc);
            }
            set
            {
                if (value == null)
                {
                    CategoryId = "";
                    CategoryName = "";

                    return;
                }

                MItemCategory ii = (value as MItemCategory);
                itc.SetDbObject(ii.GetDbObject());

                CategoryId = ii.ItemCategoryID;
                CategoryName = ii.CategoryName;

                updateFlag();
            } 
        }

        public MBaseModel ServiceObj
        {
            get
            {
                srv.ServiceCode = ServiceCode;
                srv.ServiceName = ServiceName;
                srv.ServiceID = ServiceId;

                return (srv);
            }

            set
            {
                if (value == null)
                {
                    ServiceId = "";
                    ServiceCode = "";
                    ServiceName = "";

                    return;
                }

                MService ii = (value as MService);
                srv.SetDbObject(ii.GetDbObject());

                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;
                ServiceId = ii.ServiceID;

                updateFlag();
            }
        }

        public MBaseModel ItemObj
        {
            get
            {
                itm.ItemCode = ItemCode;
                itm.ItemNameThai = ItemNameThai;
                itm.ItemID = ItemId;

                return (itm);
            }

            set
            {
                if (value == null)
                {
                    ItemId = "";
                    ItemCode = "";
                    ItemNameThai = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                itm.SetDbObject(ii.GetDbObject());

                ItemCode = ii.ItemCode;
                ItemNameThai = ii.ItemNameThai;
                ItemId = ii.ItemID;

                updateFlag();
            }
        }

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

        public String CategoryId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY_ID", value);
            }
        }

        public String CategoryName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY_NAME", value);
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

                updateFlag();
                NotifyPropertyChanged();
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

        public String ItemType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String PackageId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_ID", value);

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

                updateFlag();
                NotifyPropertyChanged("PriceConfigIcon");
            }
        }

        public String PricingType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public override Boolean IsEmpty
        {
            get
            {
                if (ItemId.Equals("") && ItemType.Equals("") && CategoryId.Equals("") && ServiceId.Equals(""))
                {
                    return (true);
                }

                if (CUtil.StringToInt(ItemId) <= 0 &&
                    CUtil.StringToInt(ItemType) <= 0 &&
                    CUtil.StringToInt(CategoryId) <= 0 &&
                    CUtil.StringToInt(ServiceId) <= 0)
                {
                    return (true);
                }

                return (false);
            }
        }

        public override Boolean IsEnabled
        {
            get
            {
                return (EnabledFlag.Equals("Y"));
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

                return (GetDbObject().GetFieldValue("PRODUCT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRODUCT_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String SeqNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQUENCE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQUENCE_NO", value);

                updateFlag();
                NotifyPropertyChanged();
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

        public String ServiceId
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

        public String EnabledFlag
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String StdPriceFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("STD_PRICE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("STD_PRICE_FLAG", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsCustomPrice
        {
            get
            {
                if (EnabledFlag.Equals("Y") && StdPriceFlag.Equals("N"))
                {
                    return (true);
                }

                return (false);
            }

            set
            {
            }
        }

        public String PriceConfigIcon
        {
            get
            {
                if (PricingDefination.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }

        public String Code
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_CODE", value);
            }
        }

        public String Name
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_NAME", value);
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

        public String PackageTrayPriceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_TRAY_PRICE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_TRAY_PRICE_ID", value);
            }
        }

        public override String ID
        {
            get
            {
                String k = "";

                if (SelectionType.Equals("1"))
                {
                    //Service
                    k = SelectionType + "-" + ServiceId;
                }
                else if (SelectionType.Equals("2"))
                {
                    //Item
                    k = SelectionType + "-" + ItemId;
                }
                else if (SelectionType.Equals("3"))
                {
                    //Category
                    k = SelectionType + "-" + CategoryId;
                }

                return (k);
            }
        }

        public String DiscountDefination
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_DEFINITION", value);

                updateFlag();
                NotifyPropertyChanged("DiscountConfigIcon");
            }
        }

        public String DiscountConfigIcon
        {
            get
            {
                if (DiscountDefination.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }
    }
}
