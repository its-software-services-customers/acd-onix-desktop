using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MInventoryTransactionImport : MInventoryTransaction
    {
        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        private MInventoryDoc cDoc = new MInventoryDoc(new CTable(""));
        private DateTime documentDate = DateTime.Now;

        public MInventoryTransactionImport(CTable obj) : base(obj)
        {
            //ExtFlag = "A";
        }

        private void calculateItemTotalAmount()
        {
            Double v1 = CUtil.StringToDouble(ItemQuantity);
            Double v2 = CUtil.StringToDouble(UIItemUnitPrice); 
            Double v3 = CUtil.StringToDouble(UIItemDiscount);

            UIItemAmount = "0.00";
            if (v1 != 0 || v2 != 0  )
            {
                UIItemAmount = (v1 * v2 - v3).ToString("0.##");
            }
        }

        public String UIItemDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("UI_ITEM_DISCOUNT", value);
                calculateItemTotalAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public String UIItemDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(UIItemDiscount));
            }
        }

        public new String UIItemUnitPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_UNIT_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("UI_ITEM_UNIT_PRICE", value);
                calculateItemTotalAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public new String UIItemUnitPriceFmt
        {
            get
            {
                return (CUtil.FormatNumber(UIItemUnitPrice));
            }
        }

        public new String UIItemAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("UI_ITEM_AMOUNT", value);

                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public new String UIItemAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(UIItemAmount));
            }
        }

        public new String ItemAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("UI_ITEM_AMOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public new String ItemAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemAmount));
            }
        }

        public new String ItemPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_UNIT_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("UI_ITEM_UNIT_PRICE", value);
                calculateItemTotalAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public new String ItemPriceFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemPrice));
            }
        }

        public new String ItemQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_QUANTITY", value);
                calculateItemTotalAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }
        public new String ItemQuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemQuantity));
            }
        }
    }
}
