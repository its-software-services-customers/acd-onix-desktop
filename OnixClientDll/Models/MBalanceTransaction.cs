using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MBalanceTransaction : MBaseModel
    {
        private Boolean isBalanceFwd = false;
        private DateTime documentDate = DateTime.Now;

        public MBalanceTransaction(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);
        }

        public String TxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DIRECTION");
                return (str);

            }
        }

        public String InQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_QTY_AVG")));
                }

                return ("");
            }
        }

        public String OutQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_QTY_AVG")));
                }

                return ("");

            }
        }

        public String InAmountAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_AMT_AVG")));
                }

                return ("");

            }
        }

        public String InAmountFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_AMT_FIFO")));
                }

                return ("");

            }
        }

        public String OutAmountAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_AMT_AVG")));
                }

                return ("");

            }
        }

        public String OutAmountFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("TX_AMT_FIFO")));
                }

                return ("");

            }
        }

        public String LeftQuantityFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_QTY_AVG")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_QTY_AVG")));
                }
            }
        }

        public String LeftQuantityAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_QTY_AVG")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_QTY_AVG")));
                }
            }
        }

        public String LeftAmountFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_AMOUNT_FIFO")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_AMOUNT_FIFO")));
                }
            }
        }


        public String LeftAmountAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_AMOUNT_AVG")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_AMOUNT_AVG")));
                }
            }
        }

        public String UnitPriceAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                Double q;
                Double a;
                if (IsBalanceForward)
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("BEGIN_QTY_AVG"));
                    a = Double.Parse(GetDbObject().GetFieldValue("BEGIN_AMOUNT_AVG"));
                }
                else
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("END_QTY_AVG"));
                    a = Double.Parse(GetDbObject().GetFieldValue("END_AMOUNT_AVG"));
                }

                Double r = 0.00;
                if (q != 0)
                {
                    r = a / q;
                }

                return (CUtil.FormatNumber(r.ToString()));
            }
        }

        public String UnitPriceFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                Double q;
                Double a;
                if (IsBalanceForward)
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("BEGIN_QTY_FIFO"));
                    a = Double.Parse(GetDbObject().GetFieldValue("BEGIN_AMOUNT_FIFO"));
                }
                else
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("END_QTY_FIFO"));
                    a = Double.Parse(GetDbObject().GetFieldValue("END_AMOUNT_FIFO"));
                }

                Double r = 0.00;
                if (q != 0)
                {
                    r = a / q;
                }

                return (CUtil.FormatNumber(r.ToString()));
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

        public FontWeight FontWeight
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (FontWeights.Bold);
                }

                return (FontWeights.Normal);
            }
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
        }

        public String LocationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_NAME"));
            }
        }
        public String LocationToName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_TO_NAME"));
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

        public DateTime DocumentDate
        {
            get
            {
                return (documentDate);
            }

            set
            {
                documentDate = value;
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

        public String DocumentNo
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_NO");
                return (str);

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

                if (!IsBalanceForward)
                {
                    return (GetDbObject().GetFieldValue("NOTE"));
                }
                else
                {
                    return ("");
                }
            }

            set
            {
                if (!IsBalanceForward)
                {
                    GetDbObject().SetFieldValue("NOTE", value);
                }
              
            }
        }
    }
}