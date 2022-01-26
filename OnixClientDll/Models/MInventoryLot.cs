using System;
using Onix.Client.Model;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MInventoryLot : MBaseModel
    {
        public MInventoryLot(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public String LocationID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID", value);
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

        public String LotTrackingID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INVENTORY_LOT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INVENTORY_LOT_ID", value);
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
                NotifyPropertyChanged();
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity));
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
            }
        }

        public String AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Amount));
            }
        }

        public String Avg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AMOUNT_PER_UNIT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AMOUNT_PER_UNIT", value);
                NotifyPropertyChanged();
            }
        }

        public String AvgFmt
        {
            get
            {
                return (CUtil.FormatNumber(Avg));
            }
        }

        public String LotNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String LotNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOT_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOT_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String AnnonymousFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ANNONYMOUS_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ANNONYMOUS_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public String IsAnnonymousIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ANNONYMOUS_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }
    }
}
