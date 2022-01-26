using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Windows.Media;
using System.Collections;

namespace Onix.ClientCenter.UI.HumanResource.PayrollDocument
{
    public class MVPayrollDocument : MBaseModel
    {
        private ObservableCollection<MVPayrollDocumentItem> docItems = new ObservableCollection<MVPayrollDocumentItem>();
        private String accountNo = "";

        public MVPayrollDocument(CTable obj) : base(obj)
        {

        }

        public override void InitializeAfterLoaded()
        {
            initPayrollDocItem();
        }

        #region PayrollDocItems

        public void AddPayrollDocItem(MVPayrollDocumentItem m)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("PAYROLL_DOC_LIST");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("PAYROLL_DOC_LIST", arr);
            }

            m.ExtFlag = "A";
            arr.Add(m.GetDbObject());
            docItems.Add(m);

            //m.Seq = internalSeq;
            //internalSeq++;
        }

        private void initPayrollDocItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("PAYROLL_DOC_LIST");

            if (arr == null)
            {
                docItems.Clear();
                return;
            }

            docItems.Clear();
            foreach (CTable t in arr)
            {
                MVPayrollDocumentItem v = new MVPayrollDocumentItem(t);

                docItems.Add(v);
                v.ExtFlag = "I";
            }
        }

        public ObservableCollection<MVPayrollDocumentItem> PayrollItems
        {
            get
            {
                return (docItems);
            }
        }

        public MVPayrollDocumentItem GetItemByIndex(int idx)
        {
            int i = 0;
            foreach (MVPayrollDocumentItem it in docItems)
            {
                if (it.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (i == idx)
                {
                    return (it);
                }

                i++;
            }

            return (null);
        }

        #endregion PayrollDocItems

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
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

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("year", PayrollYear);
            ttItems.Add(ct);

            CToolTipItem ct2 = new CToolTipItem("month", PayrollMonthName);
            ttItems.Add(ct2);

            ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }
        
        public String PayrollDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_DOC_ID", value);
            }
        }

        public String PayrollYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_YEAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_YEAR", value);
            }
        }        

        #region Payroll Month

        public MMasterRef PayrollMonthObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                PayrollMonth = m.MasterID;
                PayrollMonthName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.Months;
                if (items == null)
                {
                    return (null);
                }

                String tm = PayrollMonth;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String PayrollMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_MONTH", value);
            }
        }

        public String PayrollMonthName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String month = PayrollMonth;
                if (month.Equals(""))
                {
                    return ("");
                }

                String tmp = CUtil.IDToMonth(CUtil.StringToInt(month));
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_MONTH_NAME", value);
                //NotifyPropertyChanged();
            }
        }

        #endregion Payroll Month               

        #region Document Status

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

        #endregion

        #region Employee Type

        public String EmployeeType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_TYPE", value);
                NotifyPropertyChanged();
            }
        }        

        public String EmployeeTypeDesc
        {
            get
            {
                if (EmployeeType.Equals(""))
                {
                    return ("");
                }

                PayrollDocType et = (PayrollDocType)Int32.Parse(EmployeeType);
                String str = CUtil.PayrollDocTypeToString(et);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef EmployeeTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                EmployeeType = m.MasterID;
            }
        }

        #endregion

        #region DocumentDate

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

        public String DocumentDateNormalized
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = String.Format("{0} - {1}", FromSalaryDateFmt, ToSalaryDateFmt);

                return (str);
            }

            set
            {
            }
        }

        public DateTime FromSalaryDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("FROM_SALARY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("FROM_SALARY_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String FromSalaryDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("FROM_SALARY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime ToSalaryDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("TO_SALARY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("TO_SALARY_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ToSalaryDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("TO_SALARY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        #endregion

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


        public String ItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_COUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ItemCountFmt");
            }
        }

        public String ItemCountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ItemCount);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ReceiveAmountFmt");
            }
        }

        public String ReceiveAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String DeductAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductAmountFmt");
            }
        }

        public String DeductAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String RemainAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REMAIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REMAIN_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RemainAmountFmt");
            }
        }

        public String RemainAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(RemainAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public void CalculateTotalFields()
        {
            double received = 0.00;
            double deduced = 0.00;
            double remained = 0.00;
            double companySocialSecurity = 0.00;

            int i = 0;
            foreach (MVPayrollDocumentItem di in docItems)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }

                i++;
                received = received + CUtil.StringToDouble(di.ReceiveAmount);
                deduced = deduced + CUtil.StringToDouble(di.DeductAmount);
                remained = remained + CUtil.StringToDouble(di.RemainAmount);
                companySocialSecurity = companySocialSecurity + CUtil.StringToDouble(di.SocialSecurityCompany);
            }

            ItemCount = i.ToString();
            ReceiveAmount = received.ToString();
            DeductAmount = deduced.ToString();
            RemainAmount = remained.ToString();
            SocialSecurityCompanyAmount = companySocialSecurity.ToString();
        }

        #region Payin Date

        public DateTime PayinDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("PAY_IN_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("PAY_IN_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String PayinDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str2 = CUtil.DateTimeToDateString(PayinDate);

                return (str2);
            }

            set
            {
            }
        }

        #endregion

        #region Payroll Cash Account

        public String PayrollCashAccountID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_CASH_ACCOUNT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_CASH_ACCOUNT_ID", value);
            }
        }

        public String PayrollAccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_ACCOUNT_NO", value);
                accountNo = value;
                accountNo = accountNo.Replace("-", "").Substring(0, 10);

                NotifyPropertyChanged();
            }
        }

        public String PayrollAccountName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_ACCOUNT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_ACCOUNT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String SocialSecurityCompanyAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_SOCIAL_SECURITY_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_SOCIAL_SECURITY_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("SocialSecurityCompanyAmountFmt");
            }
        }

        public String SocialSecurityCompanyAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(SocialSecurityCompanyAmount);
                return (fmt);
            }

            set
            {
            }
        }

        #endregion

        #region Account No Digit

        private String MySubString(String str, int start)
        {
            try
            {
                String s = str.Substring(start, 1);
                return (s);
            }
            catch
            {
                return ("");
            }
        }

        public String AccountDigit0
        {
            get
            {
                return (MySubString(accountNo, 0));
            }
        }

        public String AccountDigit1
        {
            get
            {
                return (MySubString(accountNo, 1));
            }
        }

        public String AccountDigit2
        {
            get
            {
                return (MySubString(accountNo, 2));
            }
        }

        public String AccountDigit3
        {
            get
            {
                return (MySubString(accountNo, 3));
            }
        }

        public String AccountDigit4
        {
            get
            {
                return (MySubString(accountNo, 4));
            }
        }

        public String AccountDigit5
        {
            get
            {
                return (MySubString(accountNo, 5));
            }
        }

        public String AccountDigit6
        {
            get
            {
                return (MySubString(accountNo, 6));
            }
        }

        public String AccountDigit7
        {
            get
            {
                return (MySubString(accountNo, 7));
            }
        }

        public String AccountDigit8
        {
            get
            {
                return (MySubString(accountNo, 8));
            }
        }

        public String AccountDigit9
        {
            get
            {
                return (MySubString(accountNo, 9));
            }
        }

        #endregion
    }
}
