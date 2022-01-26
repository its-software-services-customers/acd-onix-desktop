using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MReceiptItem : MBaseModel
    {
        private int seq = 0;

        public MReceiptItem(CTable obj) : base(obj)
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

        public String ReceiptItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIPT_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIPT_ITEM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public void CreateDefaultValue()
        {
        }

        public String DebtAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEBT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEBT_AMOUNT", value);
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

                return (GetDbObject().GetFieldValue("PAID_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAID_AMOUNT", value);
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

        public String DocumentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_ID", value);
                NotifyPropertyChanged();
            }
        }
    }
}

