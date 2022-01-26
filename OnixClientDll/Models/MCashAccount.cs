using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using System.Collections;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MCashAccount : MBaseModel
    {
        private ObservableCollection<MCashDoc> txCashMovement = new ObservableCollection<MCashDoc>();

        public MCashAccount(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("AccNo", AccountNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("AccName", AccountName);
            ttItems.Add(ct);

            ct = new CToolTipItem("money_quantity", TotalAmountFmt);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MCashDoc> TxCash
        {
            get
            {
                return (txCashMovement);
            }
        }

        public void InitTxCash()
        {
            CTable o = GetDbObject();
            ArrayList arr = OnixWebServiceAPI.GetCashTransactionList(o);

            if (arr == null)
            {
                return;
            }

            int seq = 0;
            txCashMovement.Clear();
            foreach (CTable t in arr)
            {
                MCashDoc v = new MCashDoc(t);

                if (seq == 0)
                {
                    MCashDoc fw = new MCashDoc(t.Clone());
                    fw.IsBalanceForward = true;
                    txCashMovement.Add(fw);
                }

                txCashMovement.Add(v);
                seq++;
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
                NotifyPropertyChanged();
            }
        }

        public String BankAndAccountNo
        {
            get
            {
                if (AccountNo.Equals(""))
                {
                    return ("");
                }

                return (String.Format("{0} : {1}", BankName, AccountNo));
            }

            set
            {
            }
        }
        
        public String AccountDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (AccountNo.Equals(""))
                {
                    return ("");
                }

                return (AccountNo);
            }
        }

        public String AccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NNAME", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BankObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BankID = m.MasterID;
                BankName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = BankID;
                mr.Description = BankName;

                return (mr);
            }
        }

        public String BankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_ID", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String BankBranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_BRANCH_NAME", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        //public String ChunkNo
        //{
        //    get
        //    {
        //        if (GetDbObject() == null)
        //        {
        //            return ("");
        //        }

        //        return (GetDbObject().GetFieldValue("EXT_CHUNK_NO"));
        //    }

        //    set
        //    {
        //        GetDbObject().SetFieldValue("EXT_CHUNK_NO", value);
        //        NotifyPropertyChanged();
        //    }
        //}

        public String TotalAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TotalAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmount));
            }
        }

        //public String ChunkCount
        //{
        //    get
        //    {
        //        if (GetDbObject() == null)
        //        {
        //            return ("");
        //        }

        //        return (GetDbObject().GetFieldValue("EXT_CHUNK_COUNT"));
        //    }

        //    set
        //    {
        //        GetDbObject().SetFieldValue("EXT_CHUNK_COUNT", value);
        //        NotifyPropertyChanged();
        //    }
        //}

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

        public Boolean? IsForOwner
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("OWNER_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("OWNER_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsForPayroll
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("PAYROLL_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("PAYROLL_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

    }
}
