using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MEntityAddress : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private int seq = 0;

        public MEntityAddress(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
            Address = "";
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
            }
        }

        public String Address
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

                updateFlag();
            }
        }

        public MMasterRef AddressTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    AddressType = m.MasterID;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.AddressTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, AddressType));
            }
        }

        public String AddressType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADDRESS_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADDRESS_TYPE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public int AddressTypeIndex
        {
            get
            {
                ObservableCollection <MMasterRef> items = CMasterReference.Instance.BarcodeTypes;
                if (items == null)
                {
                    return (0);
                }

                int id = CUtil.MasterIDToIndex(items, AddressType);
                return (id);
            }

            set
            {
            }
        }

        public String SortOrder
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SORT_ORDER"));
            }

            set
            {
                GetDbObject().SetFieldValue("SORT_ORDER", value);
                NotifyPropertyChanged();

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
    }
}
