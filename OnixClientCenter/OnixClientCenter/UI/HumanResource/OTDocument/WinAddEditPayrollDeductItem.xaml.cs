using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{  
    public partial class WinAddEditPayrollDeductItem : WinBase
    {
        private MVPayrollDeductionItem mv = null;
        private MVOTDocument mvParent = null;

        public WinAddEditPayrollDeductItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "HR_DEDUCTION_EDIT";
            mvParent = (MVOTDocument)loadParam.ActualParentView;

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblDeduction, cboDeduction, false);
            registerValidateControls(lblQuantity, txtDuration, false);
            registerValidateControls(lblNote, txtNote, false);
        }

        protected override MBaseModel createObject()
        {
            mv = new MVPayrollDeductionItem(new CTable(""));
            mv.DeductionDate = DateTime.Now;
            mv.CreateDefaultValue();

            return (mv);
        }        

        protected override void addSubItem()
        {
            MVOTDocument parent = (MVOTDocument)loadParam.ActualParentView;
            parent.AddDeductionItem(mv);
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
