using System;
using System.Windows;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MAccountDocDiscount : MBaseModel
    {
        private int seq = 0;

        public MAccountDocDiscount(CTable obj) : base(obj)
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
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

        public override void createToolTipItems()
        {
        }

        public String AccountDocDiscountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACT_DOC_DISCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACT_DOC_DISCOUNT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String AccountDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ID", value);
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

        public String DiscountAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("DISCOUNT_AMOUNT");
                if (tmp.Equals(""))
                {
                    return ("");
                }

                //Display 2 decimal place

                double d = CUtil.StringToDouble(tmp);
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("DISCOUNT_AMOUNT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DiscountAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(DiscountAmt));
            }

            set
            {
            }
        }


        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region DiscountType

        public MMasterRef DiscountTypeObj
        {
            set
            {
                if (value == null)
                {
                    DiscountType = "";
                    return;
                }

                MMasterRef m = value as MMasterRef;
                DiscountType = m.MasterID;

                updateFlag();                             
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.DiscountTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, DiscountType));
            }
        }

        #endregion
    }
}
