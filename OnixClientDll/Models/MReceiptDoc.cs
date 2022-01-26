using System;
using System.Collections.ObjectModel;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MReceiptDoc : MAccountDoc
    {
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();
        private ObservableCollection<MReceiptItem> receiptItems = new ObservableCollection<MReceiptItem>();
        //private int internalSeq = 0;

        public MReceiptDoc(CTable obj) : base(obj)
        {
        }

        public String ReceiptDebtAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIPT_DEBT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIPT_DEBT_AMT", value);
            }
        }

        public String ReceiptPaidAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIPT_PAID_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIPT_PAID_AMT", value);
            }
        }

        public String ReceiptLeftAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIPT_LEFT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIPT_LEFT_AMT", value);
            }
        }

        public new ObservableCollection<MReceiptItem> ReceiptItems
        {
            get
            {
                return (receiptItems);
            }
        }

        public void AddReceiptItem(MAccountDoc doc)
        {
        }
    }
}