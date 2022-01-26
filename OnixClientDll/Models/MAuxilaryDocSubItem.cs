using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MAuxilaryDocSubItem : MBaseModel
    {
        private int seq = 0;
        private int rowType = 0;

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

        public String Index
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INDEX"));
            }

            set
            {
                GetDbObject().SetFieldValue("INDEX", value);
                NotifyPropertyChanged();
            }
        }

        public MAuxilaryDocSubItem(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
        }

        public String AuxilaryDocSubItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_SUBITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_SUBITEM_ID", value);
            }
        }

        public Boolean IsSubHeader
        {
            get;
            set;
        }

        public Boolean IsItem
        {
            get;
            set;
        }

        public Boolean IsSubTotal
        {
            get;
            set;
        }

        public Boolean IsGrandTotal
        {
            get;
            set;
        }

        public Boolean IsTotal
        {
            get
            {
                return (IsGrandTotal || IsSubTotal);
            }
        }

        public int RowType
        {
            get
            {
                return (rowType);
            }

            set
            {
                rowType = value;

                IsSubHeader = (rowType == 0);
                IsItem = (rowType == 1);                
                IsSubTotal = (rowType == 2);
                IsGrandTotal = (rowType == 3);
            }
        }

        public String Description
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public DateTime SubItemDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("SUB_ITEM_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("SUB_ITEM_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String SubItemDateStr
        {
            get
            {
                String str = GetDbObject().GetFieldValue("SUB_ITEM_DATE");
                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("SUB_ITEM_DATE", value);
            }
        }

        public String Remark
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("REMARK"));
            }

            set
            {
                GetDbObject().SetFieldValue("REMARK", value);

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String Unit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("UNIT"));
            }

            set
            {
                GetDbObject().SetFieldValue("UNIT", value);

                NotifyPropertyChanged();
                updateFlag();
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
                calculateAmount();

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity));
            }

            set
            {
            }
        }

        public String UnitPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("UNIT_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("UNIT_PRICE", value);
                calculateAmount();

                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String UnitPriceFmt
        {
            get
            {
                return (CUtil.FormatNumber(UnitPrice));
            }

            set
            {
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
                NotifyPropertyChanged("AmountFmt");

                updateFlag();
            }
        }

        public String AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Amount));
            }

            set
            {
            }
        }

        private void calculateAmount()
        {
            double price = CUtil.StringToDouble(UnitPrice);
            double qty = CUtil.StringToDouble(Quantity);

            double amt = price * qty;
            Amount = amt.ToString();
        }

        public override Boolean IsEmpty
        {
            get
            {
                if (Description.Equals("") || UnitPrice.Equals("") || Quantity.Equals(""))
                {
                    return (true);
                }

                return (false);
            }
        }

    }
}

