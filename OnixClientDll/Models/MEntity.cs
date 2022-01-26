using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Collections.ObjectModel;
using Onix.Client.Controller;
using System.Collections;
using System.Windows;

namespace Onix.Client.Model
{
    public class MEntity : MBaseModel
    {
        private Boolean isSelected = false;

        private ObservableCollection<MArApTransaction> txArApMovement = new ObservableCollection<MArApTransaction>();
        private ObservableCollection<MEntityAddress> addresses = new ObservableCollection<MEntityAddress>();
        private ObservableCollection<MEntityBankAccount> bankAccounts = new ObservableCollection<MEntityBankAccount>();

        private int internalSeq = 0;

        public MEntity(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {

        }

        public override void InitializeAfterLoaded()
        {
            InitEntityAddress();
            InitBankAccounts();            
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("ar_ap_balance", ArApBalanceFmt);
            ttItems.Add(ct);

            if (Category.Equals("1"))
            {
                ct = new CToolTipItem("customer_code", EntityCode);
                ttItems.Add(ct);

                ct = new CToolTipItem("customer_name", EntityName);
                ttItems.Add(ct);

                ct = new CToolTipItem("customer_type", EntityTypeName);
                ttItems.Add(ct);

                ct = new CToolTipItem("customer_group", EntityGroupName);
                ttItems.Add(ct);
            }
            else
            {
                ct = new CToolTipItem("supplier_code", EntityCode);
                ttItems.Add(ct);

                ct = new CToolTipItem("supplier_name", EntityName);
                ttItems.Add(ct);

                ct = new CToolTipItem("supplier_type", EntityTypeName);
                ttItems.Add(ct);

                ct = new CToolTipItem("supplier_group", EntityGroupName);
                ttItems.Add(ct);
            }
        }

        public ObservableCollection<MArApTransaction> TxArApMovement
        {
            get
            {
                return (txArApMovement);
            }
        }

        public void InitTxArApMovement()
        {
            CTable o = GetDbObject();
            ArrayList arr = OnixWebServiceAPI.GetArApTransactionList(o);

            if (arr == null)
            {
                return;
            }

            int seq = 0;
            txArApMovement.Clear();
            foreach (CTable t in arr)
            {
                MArApTransaction v = new MArApTransaction(t);

                if (seq == 0)
                {
                    MArApTransaction fw = new MArApTransaction(t.Clone());
                    fw.IsBalanceForward = true;
                    txArApMovement.Add(fw);
                }

                txArApMovement.Add(v);
                seq++;
            }
        }

        public String IDNumber
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
                NotifyPropertyChanged();
            }
        }

        public String EntityID
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
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String EntityAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String Email
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMAIL"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMAIL", value);
                NotifyPropertyChanged();
            }
        }

        public String WebSite
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WEBSITE"));
            }

            set
            {
                GetDbObject().SetFieldValue("WEBSITE", value);
                NotifyPropertyChanged();
            }
        }

        public String Phone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PHONE", value);
                NotifyPropertyChanged();
            }
        }

        public String MobilePhone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MOBILE_PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("MOBILE_PHONE", value);
                NotifyPropertyChanged();
            }
        }

        public String ContactBy
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CONTACT_BY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CONTACT_BY", value);
                NotifyPropertyChanged();
            }
        }

        public String Fax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("FAX", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String CreditLimit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_LIMIT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_LIMIT", value);
                NotifyPropertyChanged();
            }
        }

        public String CreditLimitFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(CreditLimit));
            }

            set
            {
            }
        }

        #region Name Prefix

        public MMasterRef NamePrefixObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    NamePrefix = m.MasterID;
                    NamePrefixDesc = m.Description;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.NamePrefixes;
                if (items == null)
                {
                    return (null);
                }

                String np = NamePrefix;
                return (CUtil.MasterIDToObject(items, np));
            }
        }

        public String NamePrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX", value);
                NotifyPropertyChanged();
            }
        }

        public String NamePrefixDesc
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
                NotifyPropertyChanged();
            }
        }
        #endregion

        public MMasterRef EntityTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    EntityType = m.MasterID;
                    EntityTypeName = m.Description;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = EntityType;
                mr.Description = EntityTypeName;

                return (mr);
            }
        }

        public String EntityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ENTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_TYPE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ArApBalance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_AP_BALANCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_AP_BALANCE", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ArApBalanceFmt");
            }
        }

        public String ArApBalanceFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(ArApBalance));
            }

            set
            {
            }
        }

        public MMasterRef EntityGroupObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    EntityGroup = m.MasterID;
                    EntityGroupName = m.Description;
                    NotifyAllPropertiesChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = EntityGroup;
                mr.Description = EntityGroupName;

                return (mr);
            }
        }

        public String EntityGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ENTITY_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String IDCardNumber
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
                NotifyPropertyChanged();
            }
        }

        public String ContactPerson
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CONTACT_PERSON"));
            }

            set
            {
                GetDbObject().SetFieldValue("CONTACT_PERSON", value);
                NotifyPropertyChanged();
            }
        }

        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsSelected
        {
            set
            {
                isSelected = value;
                NotifyPropertyChanged();
            }

            get
            {
                return (isSelected);
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

        #region Entity Address
        public void InitEntityAddress()
        {
            addresses.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("ADDRESS_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MEntityAddress v = new MEntityAddress(t);
                addresses.Add(v);

                v.Seq = internalSeq;
                internalSeq++;

                v.ExtFlag = "I";
            }
        }

        public void NotifyEntityAddress()
        {
            foreach (MEntityAddress me in addresses)
            {
                me.NotifyAllPropertiesChanged();
            }
        }

        public ObservableCollection<MEntityAddress> AddressItems
        {
            get
            {
                return (addresses);
            }

            set
            {
            }
        }

        public void AddAddress()
        {
            CTable t = new CTable("ENTITY_ADDRESS");
            MEntityAddress v = new MEntityAddress(t);

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ADDRESS_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ADDRESS_ITEM", arr);
            }

            v.Seq = internalSeq;
            internalSeq++;

            arr.Add(t);
            addresses.Add(v);

            v.ExtFlag = "A";
        }

        public void RemoveAddressItem(MEntityAddress vp)
        {
            removeAssociateItems(vp, "ADDRESS_ITEM", "INTERNAL_SEQ", "ENTITY_ADDRESS_ID");
            addresses.Remove(vp);
        }
        #endregion

        #region Bank Account
        public void InitBankAccounts()
        {
            bankAccounts.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("ACCOUNT_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MEntityBankAccount v = new MEntityBankAccount(t);
                bankAccounts.Add(v);

                v.Seq = internalSeq;
                internalSeq++;

                v.ExtFlag = "I";
            }
        }

        public void NotifyBankAccounts()
        {
            foreach (MEntityBankAccount me in bankAccounts)
            {
                me.NotifyAllPropertiesChanged();
            }
        }

        public ObservableCollection<MEntityBankAccount> BankAccounts
        {
            get
            {
                return (bankAccounts);
            }

            set
            {
            }
        }

        public void AddBankAccount()
        {
            CTable t = new CTable("");
            MEntityBankAccount v = new MEntityBankAccount(t);

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ACCOUNT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ACCOUNT_ITEM", arr);
            }

            v.AccountName = "NOT USED";
            v.Seq = internalSeq;
            internalSeq++;

            arr.Add(t);
            bankAccounts.Add(v);

            v.ExtFlag = "A";
        }

        public void RemoveBankAccount(MEntityBankAccount vp)
        {
            removeAssociateItems(vp, "ACCOUNT_ITEM", "INTERNAL_SEQ", "ENTITY_BACCT_ID");
            bankAccounts.Remove(vp);
        }
        #endregion

        #region RvTaxType

        public String RvTaxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("RV_TAX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("RV_TAX_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsRvType3
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (RvTaxType.Equals("3"));
            }

            set
            {
                if (value)
                {
                    RvTaxType = "3";
                    NotifyPropertyChanged();
                }
            }
        }

        public Boolean IsRvType53
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (RvTaxType.Equals("53"));
            }

            set
            {
                if (value)
                {
                    RvTaxType = "53";
                    NotifyPropertyChanged();
                }
            }
        }

        #endregion


        #region Credit Term

        public MMasterRef CreditTermObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    CreditTermID = m.MasterID;
                    CreditTermDesc = m.Description;
                    CreditTermDay = m.Optional;

                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.CreditTerms;
                if (items == null)
                {
                    return (null);
                }

                String np = CreditTermID;
                return (CUtil.MasterIDToObject(items, np));
            }
        }

        public String CreditTermID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CreditTermDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String CreditTermDay
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CREDIT_TERM_DAY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CREDIT_TERM_DAY", value);
                NotifyPropertyChanged();
            }
        }

        public String PromptPayID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROMPT_PAY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROMPT_PAY_ID", value);
                NotifyPropertyChanged();
            }
        }
        
        #endregion
    }
}
