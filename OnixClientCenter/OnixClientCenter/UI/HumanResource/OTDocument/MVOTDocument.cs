using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Windows.Media;
using System.Collections;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class MVOTDocument : MBaseModel
    {
        private ObservableCollection<MVOTDocumentItem> docItems = new ObservableCollection<MVOTDocumentItem>();
        private ObservableCollection<MVPayrollExpenseItem> expenseItems = new ObservableCollection<MVPayrollExpenseItem>();
        private ObservableCollection<MVPayrollDeductionItem> deductionItems = new ObservableCollection<MVPayrollDeductionItem>();

        public MVOTDocument(CTable obj) : base(obj)
        {

        }

        public override void InitializeAfterLoaded()
        {
            initOTDocItem();
            initExpenseItem();
            initDeductionItem();
        }


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

        #region OtDocItems

        public void CalculateOTDocItem()
        {
            foreach (MVOTDocumentItem it in docItems)
            {
                if (it.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (it.OtRate.Equals(OtRate))
                {
                    continue;
                }

                it.OtRate = OtRate;
                it.CalculateOTAmount();
                it.NotifyAllPropertiesChanged();
            }
        }

        public void AddOTDocItem(MVOTDocumentItem m)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("OT_DOC_LIST");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("OT_DOC_LIST", arr);
            }

            m.ExtFlag = "A";
            arr.Add(m.GetDbObject());
            docItems.Add(m);

            //m.Seq = internalSeq;
            //internalSeq++;
        }

        private void initOTDocItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("OT_DOC_LIST");

            if (arr == null)
            {
                docItems.Clear();
                return;
            }

            docItems.Clear();
            foreach (CTable t in arr)
            {
                MVOTDocumentItem v = new MVOTDocumentItem(t);

                docItems.Add(v);
                v.ExtFlag = "I";
            }
        }

        public ObservableCollection<MVOTDocumentItem> OTItems
        {
            get
            {
                return (docItems);
            }
        }

        public MVOTDocumentItem GetItemByIndex(int idx)
        {
            int i = 0;
            foreach (MVOTDocumentItem it in docItems)
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

        #endregion OTItems

        #region ExpenseItems

        public void AddExpenseItem(MVPayrollExpenseItem m)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("OT_EXPENSE_LIST");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("OT_EXPENSE_LIST", arr);
            }

            m.ExtFlag = "A";
            arr.Add(m.GetDbObject());
            expenseItems.Add(m);
        }

        private void initExpenseItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("OT_EXPENSE_LIST");

            if (arr == null)
            {
                expenseItems.Clear();
                return;
            }

            expenseItems.Clear();
            foreach (CTable t in arr)
            {
                MVPayrollExpenseItem v = new MVPayrollExpenseItem(t);

                expenseItems.Add(v);
                v.ExtFlag = "I";
            }
        }

        public ObservableCollection<MVPayrollExpenseItem> Expensetems
        {
            get
            {
                return (expenseItems);
            }
        }

        public MVPayrollExpenseItem GetExpenseItemByIndex(int idx)
        {
            int i = 0;
            foreach (MVPayrollExpenseItem it in expenseItems)
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

        #endregion ExpenseItems


        #region DeductionItems

        public void AddDeductionItem(MVPayrollDeductionItem m)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("DEDUCTION_LIST");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("DEDUCTION_LIST", arr);
            }

            m.ExtFlag = "A";
            arr.Add(m.GetDbObject());
            deductionItems.Add(m);
        }

        private void initDeductionItem()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("DEDUCTION_LIST");

            if (arr == null)
            {
                deductionItems.Clear();
                return;
            }

            deductionItems.Clear();
            foreach (CTable t in arr)
            {
                MVPayrollDeductionItem v = new MVPayrollDeductionItem(t);

                deductionItems.Add(v);
                v.ExtFlag = "I";
            }
        }

        public ObservableCollection<MVPayrollDeductionItem> DeductionItems
        {
            get
            {
                return (deductionItems);
            }
        }

        public MVPayrollDeductionItem GetDeductionItemByIndex(int idx)
        {
            int i = 0;
            foreach (MVPayrollDeductionItem it in deductionItems)
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

        #endregion ExpenseItems

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

            CToolTipItem ct = new CToolTipItem("employee_code", EmployeeCode);
            ttItems.Add(ct);

            CToolTipItem ct2 = new CToolTipItem("employee_name", EmployeeName);
            ttItems.Add(ct2);

            ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }
        
        public String OTDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_DOC_ID", value);
            }
        }

        #region Employee

        public MEmployee EmployeeObj
        {
            set
            {
                if (value == null)
                {
                    EmployeeID = "";
                    EmployeeCode = "";
                    EmployeeName = "";
                    EmployeeLastName = "";

                    return;
                }

                MEmployee ii = (value as MEmployee);

                EmployeeID = ii.EmployeeID;
                EmployeeCode = ii.EmployeeCode;
                EmployeeName = ii.EmployeeName;
                EmployeeLastName = ii.EmployeeLastName;
                EmployeeNamePrefixDesc = ii.NamePrefixDesc;

                updateFlag();
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MEmployee m = new MEmployee(new CTable(""));
                m.EmployeeID = EmployeeID;
                m.EmployeeCode = EmployeeCode;
                m.EmployeeName = EmployeeName;
                m.EmployeeLastName = EmployeeLastName;
                m.EmployeeNameLastname = EmployeeNameLastName;
                m.NamePrefixDesc = EmployeeNamePrefixDesc;

                return (m);
            }
        }

        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
            }
        }

        public String EmployeeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_CODE", value);
            }
        }

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
            }
        }

        public String EmployeeLastName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_LASTNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_LASTNAME", value);
            }
        }

        public String EmployeeNameLastName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (EmployeeName + " " + EmployeeLastName);
            }

            set
            {
            }
        }

        public String EmployeeNamePrefixDesc
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
        
        #endregion Employee

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

        #region Amount


        public String DeductionItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_ITEM_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_ITEM_COUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductionItemCountFmt");
            }
        }

        public String DeductionItemCountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductionItemCount);
                return (fmt);
            }

            set
            {
            }
        }

        public String DeductionMinuteTotal
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_MINUTE_TOTAL"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_MINUTE_TOTAL", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductionMinuteTotalFmt");
            }
        }

        public String DeductionMinuteTotalFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductionMinuteTotal);
                return (fmt);
            }

            set
            {
            }
        }

        public String OtAdjustAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_ADJUST_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_ADJUST_AMOUNT", value);
                NotifyPropertyChanged();
                CalculateTotalFields();
            }
        }

        public String AdjustAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADJUST_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADJUST_AMOUNT", value);
                NotifyPropertyChanged();
                CalculateTotalFields();
            }
        }

        public String DeductionHourRoundedTotal
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_HOUR_ROUNDED_TOTAL"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_HOUR_ROUNDED_TOTAL", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductionHourRoundedTotalFmt");
            }
        }

        public String DeductionHourRoundedTotalFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductionHourRoundedTotal);
                return (fmt);
            }

            set
            {
            }
        }

        public String DeductionAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductionAmountFmt");
            }
        }

        public String DeductionAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductionAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String ExpenseItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_ITEM_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_ITEM_COUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ExpenseItemCountFmt");
            }
        }

        public String ExpenseItemCountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpenseItemCount);
                return (fmt);
            }

            set
            {
            }
        }

        public String ExpenseAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ExpenseAmountFmt");
            }
        }

        public String ExpenseAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpenseAmount);
                return (fmt);
            }

            set
            {
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

        public String WorkedAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WORKED_AMOUNT_TOTAL"));
            }

            set
            {
                GetDbObject().SetFieldValue("WORKED_AMOUNT_TOTAL", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WorkedAmountFmt");
            }
        }

        public String WorkedAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(WorkedAmount);
                return (fmt);
            }

            set
            {
            }
        }
        
        #endregion Amount

        public String OtRate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_RATE"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_RATE", value);
                NotifyPropertyChanged("OtRateFmt");
                updateFlag();
            }
        }

        public String OtRateFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OtRate);
                return (fmt);
            }

            set
            {
            }
        }

        public void CalculateTotalFields()
        {
            double workedAmt = 0.00;
            double received = 0.00;
            double expense = 0.00;
            double deduction = 0.00;

            int i = 0;
            foreach (MVOTDocumentItem di in docItems)
            {
                if (di.ExtFlag.Equals("D"))
                {
                    continue;
                }
                
                i++;
                received = received + CUtil.StringToDouble(di.OtAmount);
                workedAmt = workedAmt + CUtil.StringToDouble(di.WorkAmount);
            }

            int expenseCount = 0;
            foreach (MVPayrollExpenseItem exp in expenseItems)
            {
                if (exp.ExtFlag.Equals("D"))
                {
                    continue;
                }

                expenseCount++;
                expense = expense + CUtil.StringToDouble(exp.ExpenseAmount);
            }

            int deductionCount = 0;
            double deductMinuteTotal = 0;
            foreach (MVPayrollDeductionItem exp in deductionItems)
            {
                if (exp.ExtFlag.Equals("D"))
                {
                    continue;
                }
                
                deductionCount++;                
                deductMinuteTotal = deductMinuteTotal + CUtil.StringToDouble(exp.DurationMin);
            }

            double adjust = CUtil.StringToDouble(AdjustAmount);
            double otAdjust = CUtil.StringToDouble(OtAdjustAmount);
            double rate = CUtil.StringToDouble(OtRate);
            double roundedHour = roundHour(deductMinuteTotal / 60.00);
            if (roundedHour < 1.00)
            {
                //ถ้าไม่ถึง 1 ให้ปัดลง
                roundedHour = 0.00;
            }
            deduction = roundedHour * rate * 1.50; //คิดหักที่ 1.5 เท่า
            deduction = deduction - adjust;

            DeductionHourRoundedTotal = roundedHour.ToString();

            ItemCount = i.ToString();
            ReceiveAmount = (received-otAdjust).ToString(); //OT
            WorkedAmount = workedAmt.ToString(); //ค่าแรงสำหรับรายวัน

            DeductionAmount = deduction.ToString(); //ขาด ลา สาย in hour 
            DeductionItemCount = deductionCount.ToString();
            DeductionMinuteTotal = deductMinuteTotal.ToString();

            ExpenseItemCount = expenseCount.ToString();
            ExpenseAmount = expense.ToString();            
        }

        private double roundHour(double num)
        {
            double floor = Math.Floor(num);
            double diff = num - floor;

            if (diff == 0.00)
            {
                return num;
            }
            else if (diff >= 0.50)
            {
                double midpoint = floor + 0.50;
                return midpoint;
            }

            return floor;
        }
    }
}
