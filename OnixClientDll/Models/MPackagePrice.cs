using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MPackagePrice : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        private MService srv = new MService(new CTable("SERVICE"));
        private MMasterRef svt = new MMasterRef(new CTable("SERVICE_TYPE"));
        private MItemCategory itc = new MItemCategory(new CTable("ITEM_CATEGORY"));
        private int seq = 0;

        public MPackagePrice(CTable obj) : base(obj)
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
                if (ItemId.Equals("") && ItemType.Equals("") && CategoryId.Equals("") && 
                    ServiceType.Equals("") && ServiceId.Equals(""))
                {
                    return (true);
                }

                if (CUtil.StringToInt(ItemId) <= 0 &&
                    CUtil.StringToInt(ItemType) <= 0 &&
                    CUtil.StringToInt(CategoryId) <= 0 &&
                    CUtil.StringToInt(ServiceType) <= 0 &&
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

        #region Service Type

        public MBaseModel ServiceTypeObj
        {
            get
            {
                svt.Code = ServiceTypeCode;
                svt.Description = ServiceTypeName;
                svt.MasterID = ServiceType;

                return (svt);
            }

            set
            {
                if (value == null)
                {
                    ServiceType = "";
                    ServiceTypeCode = "";
                    ServiceTypeName = "";

                    return;
                }

                MMasterRef ii = (value as MMasterRef);
                svt.SetDbObject(ii.GetDbObject());

                ServiceTypeCode = ii.Code;
                ServiceTypeName = ii.Description;
                ServiceType = ii.MasterID;

                updateFlag();
            }
        }

        public String ServiceTypeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_TYPE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_TYPE_CODE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ServiceType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ServiceTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_TYPE_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #endregion
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

        public String PackagePriceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_ITM_PRICE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_ITM_PRICE_ID", value);
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
                else if (SelectionType.Equals("4"))
                {
                    //Category
                    k = SelectionType + "-" + ServiceType;
                }

                return (k);
            }
        }
    }
}
