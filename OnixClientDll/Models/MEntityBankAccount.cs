using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MEntityBankAccount : MBaseModel
    {
        private int seq = 0;

        public MEntityBankAccount(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
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

        public String EntityBankAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_BACCT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_BACCT_ID", value);
            }
        }

        public String BankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_ID", value);
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

                updateFlag();
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

                return (GetDbObject().GetFieldValue("ACCOUNT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NAME", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String AccountType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_TYPE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String SequenceNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQ_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQ_NO", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String AccountTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_TYPE_NAME", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public MMasterRef AccountTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    AccountType = m.MasterID;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.BankAccountTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, AccountType));
            }
        }

        public MMasterRef BankObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    BankID = m.MasterID;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.Banks;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, BankID));
            }
        }
    }
}
