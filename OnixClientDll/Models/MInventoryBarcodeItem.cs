using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MInventoryBarcodeItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private int seq = 0;

        public MInventoryBarcodeItem(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
            Barcode = "";
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

        public String BarcodeItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_BC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_BC_ID", value);
            }
        }

        public String Barcode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BARCODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BARCODE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public MMasterRef BarcodeTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                BarcodeType = m.MasterID;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.BarcodeTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, BarcodeType));
            }
        }

        public String BarcodeType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BARCODE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BARCODE_TYPE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public int BarcodeTypeIndex
        {
            get
            {
                ObservableCollection <MMasterRef> items = CMasterReference.Instance.BarcodeTypes;
                if (items == null)
                {
                    return (0);
                }

                int id = CUtil.MasterIDToIndex(items, BarcodeType);
                return (id);
            }

            set
            {
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
