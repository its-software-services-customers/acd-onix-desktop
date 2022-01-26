using Wis.WsClientAPI;
using System;

namespace Onix.Client.Model
{
    public class MPackageTest : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MInventoryItem ini = new MInventoryItem(new CTable(""));
        private MService svm = new MService(new CTable(""));

        public MPackageTest(CTable obj) : base(obj)
        {
        }

        public String PackageBonusID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_BONUS_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_BONUS_ID", value);
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

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String QuantityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUANTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY_TYPE", value);

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
    }
}
