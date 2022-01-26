using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MPackageDiscount : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MInventoryItem ini = new MInventoryItem(new CTable(""));
        private MService svm = new MService(new CTable(""));
        private MItemCategory itc = new MItemCategory(new CTable(""));
        private int seq = 0;

        public MPackageDiscount(CTable obj) : base(obj)
        {
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

        public String PackageDiscountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_DISCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_DISCOUNT_ID", value);
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
            }
        }

        public String DiscountValue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_VALUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_VALUE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DiscountType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISCOUNT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
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

        #region Item
        public MBaseModel ItemObj
        {
            get
            {
                ini.ItemID = ItemID;
                ini.ItemCode = ItemCode;
                ini.ItemNameThai = ItemNameThai;

                return (ini);
            }

            set
            {
                if (value == null)
                {
                    ItemID = "";
                    ItemCode = "";
                    ItemNameThai = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                ini.SetDbObject(ii.GetDbObject());

                ItemID = ii.ItemID;
                ItemCode = ii.ItemCode;
                ItemNameThai = ii.ItemNameThai;

                updateFlag();
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
                GetDbObject().SetFieldValue("ITEM_ID", value);
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

                return (svm);
            }

            set
            {
                if (value == null)
                {
                    ServiceID = "";
                    ServiceCode = "";
                    ServiceName = "";

                    return;
                }

                MService ii = (value as MService);
                svm.SetDbObject(ii.GetDbObject());

                ServiceID = ii.ServiceID;
                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;

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
        #endregion

        #region ItemCategory
        public MBaseModel ItemCategoryObj
        {
            get
            {
                itc.ItemCategoryID = ItemCategoryID;
                itc.CategoryName = ItemCategoryName; 

                return (itc);
            }

            set
            {
                if (value == null)
                {
                    ItemCategoryID = "";
                    ItemCategoryName = "";

                    return;
                }

                MItemCategory ii = (value as MItemCategory);
                itc.SetDbObject(ii.GetDbObject());

                ItemCategoryID = ii.ItemCategoryID;
                ItemCategoryName = ii.CategoryName;

                updateFlag();
            }
        }

        public String ItemCategoryID
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

        public String ItemCategoryName
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
                NotifyPropertyChanged();
            }
        }
        #endregion

        public override Boolean IsEmpty
        {
            get
            {
                if (ItemID.Equals("") && ItemCategoryID.Equals("") && ServiceID.Equals(""))
                {
                    return (true);
                }

                if (CUtil.StringToInt(ItemID) <= 0 &&
                    CUtil.StringToInt(ItemCategoryID) <= 0 &&
                    CUtil.StringToInt(ServiceID) <= 0)
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

        public String IntervalType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INTERVAL_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("INTERVAL_TYPE", value);
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
                    k = SelectionType + "-" + ServiceID;
                }
                else if (SelectionType.Equals("2"))
                {
                    //Item
                    k = SelectionType + "-" + ItemID;
                }
                else if (SelectionType.Equals("3"))
                {
                    //Category
                    k = SelectionType + "-" + ItemCategoryID;
                }

                return (k);
            }
        }
    }
}
