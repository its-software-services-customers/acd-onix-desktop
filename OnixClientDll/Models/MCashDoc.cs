using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using System.Collections;
using System.Windows;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MCashDoc : MBaseModel
    {
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();
        private ObservableCollection<MCashDocXferDetail> xferItems = new ObservableCollection<MCashDocXferDetail>();

        private Boolean isBalanceFwd = false;

        public MCashDoc(CTable obj) : base(obj)
        {

        }

        public ObservableCollection<MError> ErrorItems
        {
            get
            {
                return (errorItems);
            }
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);

            ct = new CToolTipItem("money_quantity", TotalAmountFmt);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }

        public override void InitializeAfterNotified()
        {
            ReloadCashAccountObject();
        }

        public Boolean IsBalanceForward
        {
            get
            {
                return (isBalanceFwd);
            }

            set
            {
                isBalanceFwd = value;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (FontWeights.Bold);
                }

                return (FontWeights.Normal);
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

        public String DocumentNo
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_NO");

                return (str);
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                NotifyPropertyChanged();
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
                if (IsBalanceForward)
                {
                    return (CLanguage.getValue("balance_forward"));
                }

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

        public String DocumentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_TYPE", value);
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

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)CashDocumentStatus.CashDocApproved)
                {
                    return (false);
                }

                return (true);
            }
        }

        public String DocumentStatusDesc
        {
            get
            {
                if (DocumentStatus.Equals(""))
                {
                    return ("");
                }

                CashDocumentStatus dt = (CashDocumentStatus)Int32.Parse(DocumentStatus);
                String str = CUtil.CashDocStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
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
                NotifyPropertyChanged("TotalAmountFmt");
                NotifyPropertyChanged();
            }
        }

        public String TotalAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalAmount));
            }

            set
            {
            }
        }

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

        #region Cash Transaction (TX)
        public String TxAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String BeginAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String EndAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
                }
                else
                {
                    return (GetDbObject().GetFieldValue("END_AMOUNT"));
                }
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("EndAmountFmt");
            }
        }
        public String NoteTX
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NOTE", value);
                NotifyPropertyChanged();
            }
        }

        public String InAmount
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Amount = "";
                if (TxType.Equals("I"))
                {
                    Amount = TxAmount;
                }

                return(Amount);
            }
        }
        public String OutAmount
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Amount = "";
                if (TxType.Equals("E"))
                {
                    Amount = TxAmount;
                }

                return (Amount);
            }
        }

        public String InAmountFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (InAmount.Equals(""))
                    return "";

                return (CUtil.FormatNumber(InAmount));
            }
        }
        public String OutAmountFmt
        {
            get
            {
                if (OutAmount.Equals(""))
                    return "";

                return (CUtil.FormatNumber(OutAmount));
            }
        }

        public String BeginAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(BeginAmount));
            }
        }

        public String EndAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(EndAmount));
            }

            set
            {

            }
        }

        //Used by POS
        public String CashReconcileDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_RCL_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_RCL_DOC_ID", value);
            }
        }
        #endregion

        #region Cash Account

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

                return (AccountNo + "-" + BankName);
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
        #endregion

        #region Cash Balance
        public String BeginAmount_Balance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String EndAmount_Balance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String InAmount_Balance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("IN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String OutAmount_Balance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OUT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("OUT_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String InAmount_BalnceFmt
        {
            get
            {
                if (InAmount_Balance.Equals(""))
                    return "";

                return (CUtil.FormatNumber(InAmount_Balance));
            }
        }
        public String OutAmount_BalanceFmt
        {
            get
            {
                if (OutAmount_Balance.Equals(""))
                    return "";

                return (CUtil.FormatNumber(OutAmount_Balance));
            }
        }
        public String BeginAmount_BalanceeFmt
        {
            get
            {
                if (BeginAmount_Balance.Equals(""))
                    return "";

                return (CUtil.FormatNumber(BeginAmount_Balance));
            }
        }
        public String EndAmount_BalanceFmt
        {
            get
            {
                if (EndAmount_Balance.Equals(""))
                    return "";

                return (CUtil.FormatNumber(EndAmount_Balance));
            }
        }
        #endregion

        public virtual void ReloadCashAccountObject()
        {
            //Need to help WPF to read AccountObj one more time
            NotifyPropertyChanged("AccountObj");
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

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public Boolean? IsInternalDoc
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INTERNAL_FLAG");
                if (flag.Equals("") || (flag == null))
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
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("INTERNAL_FLAG", flag);
            }
        }

        #region CashXferDetail

        public void CalculateTransferTotal()
        {
            double total = 0.00;
            foreach (MCashDocXferDetail xd in xferItems)
            {
                if (xd.ExtFlag.Equals("D"))
                {
                    continue;
                }

                total = total + CUtil.StringToDouble(xd.PaidAmount);
            }

            TotalAmount = total.ToString();
        }

        public ObservableCollection<MCashDocXferDetail> XferItems
        {
            get
            {
                return (xferItems);
            }
        }

        public void AddXferItem(MCashDocXferDetail vp)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("CASH_XFER_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("CASH_XFER_ITEM", arr);
            }

            vp.ExtFlag = "A";
            arr.Add(vp.GetDbObject());
            xferItems.Add(vp);
        }

        public void InitXferItems()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("CASH_XFER_ITEM");

            if (arr == null)
            {
                xferItems.Clear();
                return;
            }

            xferItems.Clear();
            foreach (CTable t in arr)
            {
                MCashDocXferDetail v = new MCashDocXferDetail(t);

                if (!v.ExtFlag.Equals("D"))
                {
                    xferItems.Add(v);

                }
            }
        }
        #endregion
    }
}
