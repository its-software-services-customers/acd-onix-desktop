using System;
using Onix.Client.Model;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MCashTransaction : MBaseModel
    {
        private Boolean isBalanceFwd = false;

        public MCashTransaction(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public String CashTxID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_TX_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_TX_ID", value);
            }
        }

        public String CashDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_DOC_ID", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsBalanceForward
        {
            get
            {
                return (isBalanceFwd);
            }

            set
            {
                isBalanceFwd = value;
            }
        }

        public String DocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (CLanguage.getValue("balance_forward"));
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String CashAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String TxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String TxAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TxAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TxAmount));
            }
        }

        public String Direction
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DIRECTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DIRECTION", value);
                NotifyPropertyChanged();
            }
        }

        public String InfoAccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_ACCOUNT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String InfoAccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_ACCOUNT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_ACCOUNT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String InfoTotalAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP_TOTAL_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP_TOTAL_AMT", value);
                NotifyPropertyChanged();
            }
        }
        
        public String InAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (!Direction.Equals("I"))
                {
                    return ("");
                }

                return (TxAmount);
            }
        }

        public String InAmountFmt
        {
            get
            {
                if (InAmount.Equals(""))
                {
                    return ("");
                }

                return (CUtil.FormatNumber(InAmount));
            }
        }

        public String OutAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (!Direction.Equals("E"))
                {
                    return ("");
                }

                return (TxAmount);
            }
        }

        public String OutAmountFmt
        {
            get
            {
                if (OutAmount.Equals(""))
                {
                    return ("");
                }

                return (CUtil.FormatNumber(OutAmount));
            }
        }

        public String BeginAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String BeginAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(BeginAmount));
            }
        }

        public String EndAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String EndAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(EndAmount));
            }
        }
    }
}
