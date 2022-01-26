using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MInventoryAdjustment : MBaseModel
    {
        //private Boolean forDelete = false;
        //private String oldFlag = "";
        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        //private MLocation locationObj = null;
        private DateTime documentDate = DateTime.Now;
        //private Boolean isBalanceFwd = false;

        private String amountLabel = "";
        private int seq = 0;

        public MInventoryAdjustment(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INVENTORY_ADJUSTMENT_SEQ", value.ToString());
                seq = value;
            }
        }

        public MBaseModel ItemObj
        {
            get
            {
                itm = new MInventoryItem(new CTable(""));

                itm.ItemCode = ItemCode;
                itm.ItemNameEng = ItemNameEng;
                itm.ItemNameThai = ItemNameThai;
                itm.ItemID = ItemID;

                return (itm);
            }

            set
            {
                if (value == null)
                {
                    ItemID = "";
                    ItemCode = "";
                    ItemNameEng = "";
                    ItemNameThai = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                itm.SetDbObject(ii.GetDbObject());

                ItemCode = ii.ItemCode;
                ItemNameEng = ii.ItemNameEng;
                ItemNameThai = ii.ItemNameThai;
                ItemID = ii.ItemID;

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
                NotifyPropertyChanged();

                updateFlag();
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

                updateFlag();
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

                updateFlag();
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

                updateFlag();
            }
        }

        public String AdjQuantity
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

                updateFlag();
            }
        }

        public String AdjAmount
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

                updateFlag();
            }
        }

        public override Boolean IsEmpty
        {
            get
            {
                if (ItemCode.Equals(""))
                {
                    return (true);
                }

                return (false);
            }
        }

        public override String ID
        {
            get
            {
                return (ItemID);
            }
        }

        public String AdjustmentByDetails
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("ADJUSTMENT_BY_DETAILS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADJUSTMENT_BY_DETAILS", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("AmountLabel");
            }
        }

        public String AmountLabel
        {
            get
            {
                if (AdjustmentByDetails.Equals("1"))
                {
                    amountLabel = CLanguage.getValue("amout_total");
                }
                else if (AdjustmentByDetails.Equals("2"))
                {
                    amountLabel = CLanguage.getValue("amount_unit");
                }
                return (amountLabel);
            }

            set
            {
                amountLabel = value;
            }
        }
    }
}