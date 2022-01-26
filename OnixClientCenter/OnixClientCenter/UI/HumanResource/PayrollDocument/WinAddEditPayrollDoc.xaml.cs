using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Windows;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.HumanResource.PayrollDocument
{
    public partial class WinAddEditPayrollDoc : WinBase
    {
        private MVPayrollDocument mv = null;
        private MVPayrollDocumentItem currentViewObj = null;

        public WinAddEditPayrollDoc(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_PAYROLL_EDIT";

            createAPIName = "CreatePayrollDoc";
            updateAPIName = "UpdatePayrollDoc";
            getInfoAPIName = "GetPayrollDocInfo";
            approveAPIName = "ApprovePayrollDoc";

            InitializeComponent();

            //Need to be after InitializeComponent
            //registerValidateControls(lblYear, dtFromDate, false);
            

            double[] ratios = new double[8] {0.04, 0.04, 0.12, 0.41, 0.00, 0.13, 0.13, 0.13 };
            registerListViewSize(lsvItem.Name, ratios);
        }

        protected override bool isEditable()
        {
            mv = (MVPayrollDocument)loadParam.ActualView;
            if (mv != null)
            {
                return (mv.IsEditable);
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

        protected override MBaseModel createObject()
        {
            mv = new MVPayrollDocument(new CTable(""));

            if (loadParam.GenericType.Equals("1"))
            {
                mv.EmployeeType = ((int)PayrollDocType.PayrollDaily).ToString();
            }
            else if(loadParam.GenericType.Equals("2"))
            {
                mv.EmployeeType = ((int)PayrollDocType.PayrollMonthly).ToString();
            }
            else
            {
                mv.EmployeeType = ((int)PayrollDocType.PayrollBalanceForward).ToString();
            }

            mv.DocumentStatus = "1";
            mv.PayinDate = DateTime.Now;
            mv.DocumentDate = DateTime.Now;

            populateDefaultCashAccount();

            return (mv);
        }

        private void populateDefaultCashAccount()
        {
            MCashAccount ca = new MCashAccount(new CTable(""));
            ca.IsForPayroll = true;
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetCashAccountList", "CASH_ACCOUNT_LIST", ca.GetDbObject());
            if (arr.Count > 0)
            {
                CTable tb = (CTable) arr[0];
                ca.SetDbObject(tb);
                mv.PayrollCashAccountID = ca.CashAccountID;
                mv.PayrollAccountNo = ca.AccountNo;
                mv.PayrollAccountName = ca.AccountName;
            }
        }

        private void CmdAddEmployee_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            Boolean r = saveData();
            if (r)
            {
                loadParam.ActualView = vw;
                loadParam.Mode = "E";

                loadData();
                vw.IsModified = false;
            }
        }

        protected override Boolean postValidate()
        {
            Boolean result = validateDocItems(mv.PayrollItems, tbiItem, true);
            if (!result)
            {
                return (false);
            }

            return (true);
        }

        private Boolean validateDocItems<T>(ObservableCollection<T> collection, TabItem titem, Boolean chkCnt) where T : MBaseModel
        {
            Hashtable hash = new Hashtable();

            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;

                if (c.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (hash.ContainsKey(c.ID))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_DUPLICATED", null);
                    titem.IsSelected = true;
                    return (false);
                }

                hash.Add(c.ID, c);
            }

            return (true);
        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = approveData();
            if (r)
            {
                //Approve cheque as well
                CUtil.EnableForm(false, this);
                //CTaxDocumentUtil.ApproveChequeFromTaxDoc(mv);
                CUtil.EnableForm(true, this);

                vw.IsModified = false;
                this.Close();
            }
        }

        private void CmdPreview_Click(object sender, RoutedEventArgs e)
        {
            WinFormPrinting w = new WinFormPrinting("grpHRSlip", mv);
            w.ShowDialog();
        }

        private void LsvAccoutItem_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void LsvAccoutItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MVPayrollDocumentItem)lsvItem.SelectedItems[0];
                showEditWindow();
            }
        }

        private void CbkRemove_Checked(object sender, RoutedEventArgs e)
        {
            (vw as MVPayrollDocument).CalculateTotalFields();
            vw.IsModified = true;
        }

        private void CmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MVPayrollDocumentItem)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void MnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuDocumentEdit"))
            {
                showEditWindow();
            }
        }

        private void showEditWindow()
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = loadParam.Caption;
            param.Mode = "E";
            param.ActualParentView = mv;
            param.ActualView = currentViewObj;
            param.GenericType = loadParam.GenericType;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollDocItem", param);

            if (okClick)
            {
                (vw as MVPayrollDocument).CalculateTotalFields();
                vw.IsModified = true;
            }
        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = loadParam.Caption;
            param.Mode = "A";
            param.ActualParentView = mv;
            param.GenericType = loadParam.GenericType;
            //param.ActualView = actDoc;
            Boolean okClick = FactoryWindow.ShowWindow("WinAddEditPayrollDocItem", param);

            if (okClick)
            {
                (vw as MVPayrollDocument).CalculateTotalFields();
                vw.IsModified = true;
            }

        }

        private void CmdPayrollAccount_Click(object sender, RoutedEventArgs e)
        {
            populateDefaultCashAccount();
            vw.IsModified = true;
        }
    }
}
