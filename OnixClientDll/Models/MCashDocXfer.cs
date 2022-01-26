using System;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MCashDocXfer : MCashDoc
    {
        public MCashDocXfer(CTable obj) : base(obj)
        {
        }

        public String CashAccountID1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID1"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID1", value);
            }
        }

        public String CashAccountID2
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

        public IList FromCashAccounts
        {
            get
            {
                IList colls = CUtil.GetCollectionByID(CMasterReference.Instance.CashAccounts, "BankID", FromBankID);
                return (colls);
            }

            set
            {
            }
        }

        public MCashAccount FromAccountObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    FromAccountID = m.CashAccountID;
                    FromAccountNo = m.AccountNo;
                    FromAccountName = m.AccountName;
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

                MCashAccount ca = (MCashAccount)CUtil.IDToObject(items, "CashAccountID", FromAccountID);
                return (ca);
            }
        }

        public String FromAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CASH_ACCOUNT_ID1"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCOUNT_ID1", value);
                NotifyPropertyChanged();
            }
        }

        public String FromAccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_ACCOUNT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_ACCOUNT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String FromAccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_ACCOUNT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public IList ToCashAccounts
        {
            get
            {
                IList colls = CUtil.GetCollectionByID(CMasterReference.Instance.CashAccounts, "BankID", ToBankID);
                return (colls);
            }

            set
            {
            }
        }

        public MCashAccount ToAccountObj
        {
            set
            {
                MCashAccount m = value as MCashAccount;
                if (m != null)
                {
                    ToAccountID = m.CashAccountID;
                    ToAccountNo = m.AccountNo;
                    ToAccountName = m.AccountName;
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

                MCashAccount ca = (MCashAccount)CUtil.IDToObject(items, "CashAccountID", ToAccountID);
                return (ca);
            }
        }

        public String ToAccountID
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

        public String ToAccountName
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

        public String ToAccountNo
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

        #region From Bank

        public MMasterRef FromBankObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                FromBankID = m.MasterID;
                FromBankName = m.Description;

                NotifyPropertyChanged("FromCashAccounts");
                NotifyPropertyChanged("FromAccountObj");
            }

            get
            {
                MMasterRef l = new MMasterRef(new CTable(""));
                l.MasterID = FromBankID;
                l.Description = FromBankName;

                return (l);
            }
        }

        public String FromBankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("FROM_BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_BANK_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String FromBankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_BANK_NAME", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region To Bank

        public MMasterRef ToBankObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                ToBankID = m.MasterID;
                ToBankName = m.Description;

                NotifyPropertyChanged("ToCashAccounts");
                NotifyPropertyChanged("ToAccountObj");
            }

            get
            {
                MMasterRef l = new MMasterRef(new CTable(""));
                l.MasterID = ToBankID;
                l.Description = ToBankName;

                return (l);
            }
        }

        public String ToBankID
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

        public String ToBankName
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

        #endregion

        public override void ReloadCashAccountObject()
        {
            //Need to help WPF to read AccountObj one more time
            NotifyPropertyChanged("FromAccountObj");
            NotifyPropertyChanged("ToAccountObj");
        }

        public MMasterRef CashXferTypeObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                CashXferType = m.MasterID;
            }

            get
            {
                MMasterRef l = new MMasterRef(new CTable(""));
                l.MasterID = CashXferType;

                return (l);
            }
        }

        public String CashXferType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_XFER_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_XFER_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public override void InitializeAfterNotified()
        {
            if (CashXferType.Equals("2"))
            {
                InitXferItems();
                CalculateTransferTotal();
                ReloadCashAccountObject();
            }
            else
            {
                ReloadCashAccountObject();
            }
        }
    }
}
