using System;
using System.Windows;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MAccountDocDeposit : MBaseModel
    {
        private int seq = 0;

        public MAccountDocDeposit(CTable obj) : base(obj)
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

        public String AccountDocDepositID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACT_DOC_DEPOSIT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACT_DOC_DEPOSIT_ID", value);
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

        public String RatioType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RATIO_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("RATIO_TYPE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DepositAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("DEPOSIT_AMOUNT");
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
                GetDbObject().SetFieldValue("DEPOSIT_AMOUNT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DepositAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(DepositAmt));
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
    }
}
