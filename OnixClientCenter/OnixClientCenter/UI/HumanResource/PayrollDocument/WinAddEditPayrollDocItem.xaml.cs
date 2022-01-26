using System;
using System.Collections;
using System.Windows;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Windows;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.HumanResource.PayrollDocument
{
    public partial class WinAddEditPayrollDocItem : WinBase
    {
        private MVPayrollDocumentItem mv = null;
        private MVPayrollDocument mvParent = null;

        public WinAddEditPayrollDocItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_PAYROLL_EDIT";
            mvParent = (MVPayrollDocument)loadParam.ActualParentView;

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblEmployee, uEmployee, false);
            registerValidateControls(lblSocialSec, txtSocialSec, false);
            //registerValidateControls(lblMonth, cboMonth, false);
        }

        protected override bool isEditable()
        {            
            if (mvParent != null)
            {
                return (mvParent.IsEditable);
            }

            return (true);
        }

        public Boolean IsPreviewAble
        {
            get
            {
                return (!loadParam.GenericType.Equals("3"));
            }

            set
            {
            }
        }

        public Visibility BalanceVisibility
        {
            get
            {
                if (loadParam.GenericType.Equals("3"))
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }

            set
            {
            }
        }        

        protected override MBaseModel createObject()
        {
            mv = new MVPayrollDocumentItem(new CTable(""));

            if (loadParam.Mode.Equals("A"))
            {
                mv.InitializeAfterLoaded();
            }

            return (mv);
        }

        public LookupSearchType2 EmployeeLookupType
        {
            get
            {
                if (loadParam.GenericType.Equals("1"))
                {
                    return (LookupSearchType2.EmployeeLookupDaily);
                }
                else if (loadParam.GenericType.Equals("2"))
                {
                    return (LookupSearchType2.EmployeeLookupMonthly);
                }

                return (LookupSearchType2.EmployeeLookup);
            }

            set
            {
            }
        }

        private void CmdAddEmployee_Click(object sender, RoutedEventArgs e)
        {
        }

        protected override void addSubItem()
        {
            MVPayrollDocument parent = (MVPayrollDocument) loadParam.ActualParentView;
            parent.AddPayrollDocItem(mv);
        }

        protected override void beforeSaveItem()
        {
            MVPayrollDocumentItem balTotal = mv.BalanceTotalObj;
            MVPayrollDocumentItem balYear = mv.BalanceYearObj;

            mv.BalanceTotalDefinition = balTotal.GetDefinitionText();
            mv.BalanceYearDefinition = balYear.GetDefinitionText();

            mv.EndingTotalObj.SumItem(balTotal, mv);
            mv.EndingYearObj.SumItem(balYear, mv);

            mv.EndingYearDefinition = mv.EndingYearObj.GetDefinitionText();
            mv.EndingTotalDefinition = mv.EndingTotalObj.GetDefinitionText();
        }

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveDataItem();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }        

        private void CmdPreview_Click(object sender, RoutedEventArgs e)
        {
            MVPayrollDocumentItem di = new MVPayrollDocumentItem(mv.GetDbObject().CloneAll());
            MVPayrollDocument pd = new MVPayrollDocument(mvParent.GetDbObject().CloneAll());

            String temp = pd.PayrollAccountNo;
            pd.PayrollAccountNo = temp; //Refresh AccountNo Digit

            pd.PayrollItems.Clear();
            pd.AddPayrollDocItem(di);
            
            WinFormPrinting w = new WinFormPrinting("grpHRSlip", pd);
            w.ShowDialog();
        }

        private void LsvAccoutItem_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void LsvAccoutItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void CbkRemove_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CmdAction_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private CTable getAccumObject(String accumType)
        {
            CTable dat = new CTable("");
            dat.SetFieldValue("START_DATE", CUtil.DateTimeToDateStringInternalMin(mvParent.FromSalaryDate));
            dat.SetFieldValue("ACCUM_TYPE", accumType);
            dat.SetFieldValue("EMPLOYEE_ID", mv.EmployeeID);

            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetEmployeeAccumulate", "EMPLOYEE_PAYROLL_ACCUM_LIST", dat);
            if (arr.Count > 0)
            {
                return ((CTable) arr[0]);
            }

            return (new CTable(""));
        }

        private void calculateBalance()
        {
            if (mv.EmployeeID.Equals(""))
            {
                return;
            }

            CUtil.EnableForm(false, this);

            MVPayrollDocumentItem balTotal = mv.BalanceTotalObj;
            MVPayrollDocumentItem balYear = mv.BalanceYearObj;

            CTable yearAccum = getAccumObject("Y");
            CTable allAccum = getAccumObject("A");

            balTotal.SetDbObject(allAccum);
            balTotal.NotifyAllPropertiesChanged();

            balYear.SetDbObject(yearAccum);
            balYear.NotifyAllPropertiesChanged();

            CUtil.EnableForm(true, this);
        }

        private void CmdCalculate_Click(object sender, RoutedEventArgs e)
        {
            calculateBalance();
            mv.calculateFields();
            vw.IsModified = true;
        }

        private void populateSalary()
        {
            if (isInLoad)
            {
                return;
            }

            if (mv.EmployeeID.Equals(""))
            {
                return;
            }

            String empID = mv.EmployeeID;

            CTable dat = new CTable("");
            dat.SetFieldValue("EMPLOYEE_ID", empID);

            CUtil.EnableForm(false, this);
            CTable empObj = OnixWebServiceAPI.SubmitObjectAPI("GetEmployeeInfo", dat);
            CUtil.EnableForm(true, this);

            MEmployee emp = new MEmployee(empObj);
            mv.ReceiveIncome = emp.Salary;
        }

        private void UEmployee_SelectedObjectChanged(object sender, EventArgs e)
        {
            calculateBalance();
            populateSalary();

            vw.IsModified = true;
        }

        private String getAmount(ArrayList arr, String key)
        {
            foreach (CTable obj in arr)
            {
                String k = obj.GetFieldValue("ACCUM_TYPE");
                if (k.Equals(key))
                {
                    String amt = obj.GetFieldValue("AMOUNT");
                    return (amt);
                }
            }

            return ("0.00");
        }

        private void CmdAuto_Click(object sender, RoutedEventArgs e)
        {
            if (mv.EmployeeID.Equals(""))
            {
                return;
            }

            CUtil.EnableForm(false, this);

            CTable dat = new CTable("");
            dat.SetFieldValue("FROM_DOCUMENT_DATE", CUtil.DateTimeToDateStringInternalMin(mvParent.FromSalaryDate));
            dat.SetFieldValue("TO_DOCUMENT_DATE", CUtil.DateTimeToDateStringInternalMax(mvParent.ToSalaryDate));
            dat.SetFieldValue("EMPLOYEE_ID", mv.EmployeeID);

            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetEmployeePayrollAccumulate", "PAYROLL_EMPLOYEE_ACCUM_LIST", dat);
            mv.ReceiveOT = getAmount(arr, "EMPLOYEE_OT");
            if (mvParent.EmployeeType.Equals("1"))
            {
                mv.ReceiveIncome = getAmount(arr, "EMPLOYEE_WORK");
            }
            mv.ReceiveTransaportation = getAmount(arr, "EMPLOYEE_EXPENSE");
            mv.DeductPenalty = getAmount(arr, "EMPLOYEE_DEDUCTION");

            CUtil.EnableForm(true, this);
        }
    }
}
