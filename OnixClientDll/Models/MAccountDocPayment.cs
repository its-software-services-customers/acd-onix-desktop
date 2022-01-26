using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MAccountDocPayment : MBaseModel
    {
        private int seq = 0;
        private MMasterRef pmt = new MMasterRef(new CTable(""));

        public MAccountDocPayment(CTable obj) : base(obj)
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

        public Boolean IsEditable
        {
            get
            {
                String status = PaymentType;
                if (status.Equals("1"))
                {
                    return (false);
                }
                return (true);
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
                updateFlag();
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

        public String FeeAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("FEE_AMOUNT");
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
                GetDbObject().SetFieldValue("FEE_AMOUNT", value);
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ChequeDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("CHEQUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime ChequeDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("CHEQUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("CHEQUE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ChequeNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_NO", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ChequeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsBankRequired
        {
            get
            {
                return (!PaymentType.Equals("1"));
                //return (true);
            }

            set
            {
            }
        }

        public Boolean IsCheque
        {
            get
            {
                return (PaymentType.Equals("4"));
            }

            set
            {
            }
        }

        private ObservableCollection<MCashAccount> getCashAccountByBankID(String bankID)
        {
            ObservableCollection<MCashAccount> temp = new ObservableCollection<MCashAccount>();

            foreach (MCashAccount ca in CMasterReference.Instance.CashAccounts)
            {
                if (ca.BankID.Equals(bankID))
                {
                    temp.Add(ca);
                }
            }

            return (temp);
        }

        public ObservableCollection<MCashAccount> CashAccounts
        {
            get
            {
                if (PaymentType.Equals("1"))
                {
                    return (new ObservableCollection<MCashAccount>());
                }

                return (getCashAccountByBankID(BankID));
            }
        }

        public ObservableCollection<MMasterRef> Banks
        {
            get
            {
                if (PaymentType.Equals("1"))
                {
                    return (new ObservableCollection<MMasterRef>());
                }

                return (CMasterReference.Instance.Banks);
            }
        }

        #region CashAccount

        public MCashAccount CashAccountObj
        {
            set
            {
                if (value == null)
                {
                    CashAccountID = "";
                    return;
                }

                MCashAccount m = value as MCashAccount;
                CashAccountID = m.CashAccountID;
                AccountNo = m.AccountNo;

                updateFlag();
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

                return (CUtil.CashAccountIDToObject(items, CashAccountID));
            }
        }

        #endregion

        #region Bank

        public MMasterRef BankObj
        {
            set
            {
                if (value == null)
                {
                    BankID = "";
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BankID = m.MasterID;

                updateFlag();
                NotifyPropertyChanged("CashAccounts");
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

        #endregion

        #region PaymentType

        public MMasterRef PaymentTypeObj
        {
            set
            {
                if (value == null)
                {
                    PaymentType = "";
                    return;
                }

                MMasterRef m = value as MMasterRef;
                PaymentType = m.MasterID;

                updateFlag();
                NotifyPropertyChanged("IsBankRequired");
                NotifyPropertyChanged("IsCheque");
                NotifyPropertyChanged("CashAccounts");
                NotifyPropertyChanged("Banks");                                    
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.AccountSalePayTypes;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, PaymentType));
            }
        }

        #endregion

        #region Change Type

        public String ChangeType
        {
            get
            {
                String defaultValue = "1";
                String type = GetDbObject().GetFieldValue("CHANGE_TYPE");

                if (GetDbObject() == null)
                {
                    return (defaultValue);
                }

                if (type.Equals(""))
                {
                    return (defaultValue);
                }

                return (type);
            }

            set
            {
                GetDbObject().SetFieldValue("CHANGE_TYPE", value);

                NotifyPropertyChanged("IsChangeByCash");
                NotifyPropertyChanged("IsChangeByCredit");
            }
        }


        public Boolean IsChangeByCash
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ChangeType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    ChangeType = "1";
                }
            }
        }

        public Boolean IsChangeByCredit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ChangeType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    ChangeType = "2";
                }
            }
        }

        #endregion

        public String RefundStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REFUND_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("REFUND_STATUS", value);
                updateFlag();
                NotifyPropertyChanged();
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
    }
}
