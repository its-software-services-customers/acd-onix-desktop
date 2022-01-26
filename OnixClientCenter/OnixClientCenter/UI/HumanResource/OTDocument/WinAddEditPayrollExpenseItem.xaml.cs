using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Utils;
using System.Windows.Controls;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{  
    public partial class WinAddEditPayrollExpenseItem : WinBase
    {
        private MVPayrollExpenseItem mv = null;
        private MVOTDocument mvParent = null;

        public WinAddEditPayrollExpenseItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_EXPENSE_EDIT";
            mvParent = (MVOTDocument)loadParam.ActualParentView;

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblExpense, cboExpense, false);
            registerValidateControls(lblQuantity, txtQuantity, false);
            registerValidateControls(lblPrice, txtPrice, false);
        }

        protected override MBaseModel createObject()
        {
            mv = new MVPayrollExpenseItem(new CTable(""));
            mv.ExpenseDate = DateTime.Now;
            mv.CreateDefaultValue();
            mv.ExpenseQuantity = "1.00";

            return (mv);
        }        

        protected override void addSubItem()
        {
            MVOTDocument parent = (MVOTDocument)loadParam.ActualParentView;
            parent.AddExpenseItem(mv);
        }

        private void cmdOK_Click(object sender, RoutedEventArgs e)
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
    }
}
