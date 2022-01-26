using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Collections;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MCheque : MBaseModel
    {
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();

        public MCheque(CTable obj) : base(obj)
        {

        }

        public ObservableCollection<MError> ErrorItems
        {
            get
            {
                return (errorItems);
            }
        }

        public void InitErrorItem()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ERROR_ITEM");

            if (arr == null)
            {
                return;
            }

            errorItems.Clear();
            foreach (CTable t in arr)
            {
                MError v = new MError(t);
                errorItems.Add(v);
            }
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("cheque_no", ChequeNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("payee_name", PayeeName);
            ttItems.Add(ct);

            ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);

            ct = new CToolTipItem("money_quantity", ChequeAmountFmt);
            ttItems.Add(ct);
        }

        public override void InitializeAfterNotified()
        {
            ReloadCashAccountObject();
        }

        public void CreateDefaultValue()
        {
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
                NotifyPropertyChanged();
            }
        }

        public String PayeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYEE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        #region Bank

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

                NotifyPropertyChanged();
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

        public String AccountBankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_BANK_NAME", value);
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

        #endregion Bank

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

        public String ChequeAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String ChequeAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(ChequeAmount));
            }
        }

        public DateTime IssueDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("ISSUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("ISSUE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String IssueDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("ISSUE_DATE");
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

        public DateTime? FromChequeDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_CHEQUE_DATE", str);
            }
        }

        public DateTime? ToChequeDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_CHEQUE_DATE", str);
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

        public String AccountDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String Direction
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DIRECTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DIRECTION", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef ChequeStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                ChequeStatus = m.MasterID;
            }
        }

        public String ChequeStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public String ChequeStatusDesc
        {
            get
            {
                if (ChequeStatus.Equals(""))
                {
                    return ("");
                }

                InventoryDocumentStatus dt = (InventoryDocumentStatus)Int32.Parse(ChequeStatus);
                String str = CUtil.InvDocStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public Boolean? AllowNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_NEGATIVE");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsAcPayeeOnly
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("AC_PAYEE_ONLY");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("AC_PAYEE_ONLY", flag);
                NotifyPropertyChanged();
            }
        }

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

        #region Account

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

                return (GetDbObject().GetFieldValue("CASH_ACCT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_ACCT_ID", value);
                NotifyPropertyChanged();
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

        #endregion

        public DateTime ApproveDate
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("APPROVED_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("APPROVED_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ApproveDateFmt
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("APPROVED_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }


        public Boolean IsEditable
        {
            get
            {
                String status = ChequeStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocApproved)
                {
                    return (false);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocCancelApproved)
                {
                    //Voided
                    return (false);
                }

                return (true);
            }
        }

        #region Entity

        public MBaseModel EntityObj
        {
            get
            {
                MEntity cst = new MEntity(new CTable(""));
                cst.EntityID = EntityId;
                cst.EntityName = EntityName;
                cst.EntityCode = EntityCode;
                cst.EntityGroup = EntityGroup;
                cst.EntityType = EntityType;
                cst.CreditTerm = CreditTerm;
                cst.Phone = EntityPhone;
                cst.Fax = EntityFax;
                cst.IDCardNumber = EntityIDNumber;
                cst.NamePrefixDesc = EntityNamePrefixDesc;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    EntityId = "";
                    EntityName = "";
                    EntityCode = "";
                    EntityGroup = "";
                    EntityType = "";
                    CreditTerm = "";
                    EntityPhone = "";
                    EntityFax = "";
                    EntityIDNumber = "";
                    EntityNamePrefixDesc = "";

                    return;
                }

                MEntity ii = (value as MEntity);

                EntityId = ii.EntityID;
                EntityName = ii.EntityName;
                EntityCode = ii.EntityCode;
                EntityGroup = ii.EntityGroup;
                EntityType = ii.EntityType;
                CreditTerm = ii.CreditTerm;
                EntityPhone = ii.Phone;
                EntityFax = ii.Fax;
                EntityIDNumber = ii.IDCardNumber;
                EntityNamePrefixDesc = ii.NamePrefixDesc;

                updateFlag();
            }
        }

        public String EntityCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_CODE", value);
            }
        }

        public String CreditTerm
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM", value);

            }
        }

        public String EntityId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ID", value);
            }
        }

        public String EntityGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_GROUP", value);
            }
        }

        public String EntityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_TYPE", value);
            }
        }

        public String EntityNamePrefixDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX_DESC", value);
            }
        }

        public String EntityName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_NAME", value);
            }
        }

        public String EntityPhone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_PHONE", value);
            }
        }

        public String EntityFax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_FAX", value);
            }
        }

        public String EntityIDNumber
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ID_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("ID_NUMBER", value);
            }
        }

        #endregion

        public virtual void ReloadCashAccountObject()
        {
            //Need to help WPF to read AccountObj one more time
            NotifyPropertyChanged("AccountObj");
        }

        #region Cheque Bank

        public MMasterRef ChequeBankObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                ChequeBankID = m.MasterID;
                ChequeBankName = m.Description;
            }

            get
            {
                MMasterRef l = new MMasterRef(new CTable(""));
                l.MasterID = ChequeBankID;
                l.Description = ChequeBankName;

                return (l);
            }
        }

        public String ChequeBankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_BANK_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String ChequeBankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_BANK_NAME", value);
                NotifyPropertyChanged();
            }
        }

        #endregion Bank

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(ChequeStatus));
            }
        }
    }
}
