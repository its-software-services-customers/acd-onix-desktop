using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MCashDocIn : MCashDoc
    {
        public MCashDocIn(CTable obj) : base(obj)
        {
        }

        public new String CashAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID2"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID2", value);
            }
        }

        #region Bank

        public new MMasterRef BankObj
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

                NotifyPropertyChanged("CashAccounts");
                NotifyPropertyChanged("AccountObj");
            }

            get
            {
                MMasterRef l = new MMasterRef(new CTable(""));
                l.MasterID = BankID;
                l.Description = BankName;

                return (l);
            }
        }

        public new String BankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("TO_BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_BANK_ID", value);
                NotifyPropertyChanged();
            }
        }

        public new String BankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_BANK_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public new String BankBranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_BANK_BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_BANK_BRANCH_NAME", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        public IList CashAccounts
        {
            get
            {
                IList colls = CUtil.GetCollectionByID(CMasterReference.Instance.CashAccounts, "BankID", BankID);
                return (colls);
            }

            set
            {
            }
        }

        public MCashAccount AccountObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    AccountID = m.CashAccountID;
                    AccountNo = m.AccountNo;
                    AccountName = m.AccountName;

                    BankName = m.BankName;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MCashAccount> items = CMasterReference.Instance.CashAccounts;
                if (items == null)
                {
                    return (null);
                }

                MCashAccount ca = (MCashAccount)CUtil.IDToObject(items, "CashAccountID", AccountID);
                return (ca);
            }
        }

        public String AccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID2"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID2", value);
                NotifyPropertyChanged();
            }
        }

        public new String AccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_ACCOUNT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_ACCOUNT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public new String AccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_ACCOUNT_NO", value);
                NotifyPropertyChanged();
            }
        }
    }
}
