using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MCashDocXferDetail : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        public MCashDocXferDetail(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();            
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

        public void CreateDefaultValue()
        {
        }

        public String CashXferID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_XFER_DTL_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_XFER_DTL_ID", value);
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
            }
        }

        public String AccountDocPaymentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACT_DOC_PAYMENT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACT_DOC_PAYMENT_ID", value);
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DOCUMENT_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDateFmt
        {
            get
            {
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
                if (GetDbObject() == null)
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

        public String DocumentStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public String PaymentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_TYPE", value);
                updateFlag();
                NotifyPropertyChanged("IsEditable");
                NotifyPropertyChanged();
            }
        }

        public String PaymentTypeDesc
        {
            get
            {
                return (CUtil.PaymentTypeToString(PaymentType, "TH"));
            }

            set
            {
            }
        }

        public String BankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String AccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NO", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String PaidAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("PAID_AMOUNT");
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
                GetDbObject().SetFieldValue("PAID_AMOUNT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String PaidAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(PaidAmount));
            }

            set
            {
            }
        }

        public DateTime? FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }

    }


}
