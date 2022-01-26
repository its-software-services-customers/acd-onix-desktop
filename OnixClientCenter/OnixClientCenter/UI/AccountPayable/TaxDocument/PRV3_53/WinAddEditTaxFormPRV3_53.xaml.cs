using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.UI.Cash.Cheque;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53
{
    public partial class WinAddEditTaxFormPRV3_53 : WinBase
    {
        public WinAddEditTaxFormPRV3_53(CWinLoadParam param) : base(param)
        {
            accessRightName = "PURCHASE_TAXDOC_EDIT";

            createAPIName = "CreateTaxDoc";
            updateAPIName = "UpdateTaxDoc";
            getInfoAPIName = "GetTaxDocInfo";
            approveAPIName = "ApproveTaxDoc";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblYear, txtYear, false);
            registerValidateControls(lblMonth, cboMonth, false);

            double[] ratios = new double[8] {0.04, 0.34, 0.12, 0.15, 0.1, 0.05, 0.1, 0.1 };
            registerListViewSize(lsvExpense.Name, ratios);
        }

        protected override bool isEditable()
        {
            MVTaxDocument mv = (MVTaxDocument)loadParam.ActualView;
            if (mv != null)
            {
                return (mv.IsEditable);
            }

            return (true);
        }

        protected override MBaseModel createObject()
        {
            MVTaxDocument mv = new MVTaxDocument(new CTable(""));

            if (loadParam.GenericType.Equals("2"))
            {
                mv.DocumentType = ((int)TaxDocumentType.TaxDocRev3).ToString();
            }
            else
            {
                mv.DocumentType = ((int)TaxDocumentType.TaxDocRev53).ToString();
            }
            mv.DocumentStatus = "1";

            mv.DocumentDate = DateTime.Now;

            return (mv);
        }

        private void CmdAddProduct_Click(object sender, RoutedEventArgs e)
        {
            MVTaxDocument td = (MVTaxDocument)vw;
            if (td.TaxDocID.Equals(""))
            {
                CHelper.ShowErorMessage("", "ERROR_PLEASE_SAVE_FIRST", null);
                return;
            }

            Boolean result = validateData();
            if (!result)
            {
                return;
            }
            
            TaxDocumentType tdt = (TaxDocumentType) CUtil.StringToInt(td.DocumentType);

            String tt = "53";
            if (tdt == TaxDocumentType.TaxDocRev3)
            {
                tt = "3";
            }

            CTable t = vw.GetDbObject();
            t.SetFieldValue("RV_TAX_TYPE", tt);

            CUtil.EnableForm(false, this);
            CTable obj = OnixWebServiceAPI.SubmitObjectAPI("PopulateWhItems", t);
            CUtil.EnableForm(true, this);

            if (obj == null)
            {
                CHelper.ShowErorMessage("", "ERROR_NO_ITEM_FOUND", null);
                return;
            }

            if (obj != null)
            {
                td.ItemCount = obj.GetFieldValue("ITEM_COUNT");
                td.WhAmount = obj.GetFieldValue("WH_AMOUNT");
                td.ExpenseAmount = obj.GetFieldValue("EXPENSE_REVENUE_AMT");
                td.PreviousRunMonth = obj.GetFieldValue("PREVIOUS_RUN_MONTH");
                td.PreviousRunYear = obj.GetFieldValue("PREVIOUS_RUN_YEAR");

                ArrayList arr = obj.GetChildArray("WH_ITEMS");
                if (arr.Count <= 0)
                {
                    //Just notify
                    CHelper.ShowErorMessage("", "ERROR_NO_ITEM_FOUND", null);
                }

                td.PopulateWhItems(arr);
                td.IsModified = true;
            }
            
        }

        private Boolean isDataConsistent()
        {
            MVTaxDocument td = (MVTaxDocument)vw;
            if (td.PreviousRunYear.Equals(""))
            {
                //Add mode
                return(true);
            }

            if (td.PreviousRunYear.Equals(td.TaxYear) && td.PreviousRunMonth.Equals(td.TaxMonth))
            {
                return (true);
            }

            CHelper.ShowErorMessage("", "ERROR_PLEASE_RE_CALCULATE", null);

            return (false);
        }

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            if (!isDataConsistent())
            {
                return;
            }

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
            if (!isDataConsistent())
            {
                return;
            }

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

        private void CmdVerify_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdApprove_Click(object sender, RoutedEventArgs e)
        {
            MVTaxDocument mv = (MVTaxDocument) vw;

            if (mv.ChequeID.Equals(""))
            {
                CHelper.ShowErorMessage("", "ERROR_CREATE_CHEQUE_FIRST", null);
                return;
            }

            Boolean r = approveData();
            if (r)
            {
                //Approve cheque as well
                CUtil.EnableForm(false, this);
                CTaxDocumentUtil.ApproveChequeFromTaxDoc(mv);
                CUtil.EnableForm(true, this);

                vw.IsModified = false;
                this.Close();
            }
        }

        private void CmdPreview_Click(object sender, RoutedEventArgs e)
        {
            MVTaxDocument td = (MVTaxDocument)vw;
            
            Boolean result = validateData();
            if (!result)
            {
                return;
            }

            CWinLoadParam parm = new CWinLoadParam();
            parm.ActualView = td;
            WinTaxDocRendererProgress trp = new WinTaxDocRendererProgress(parm);
            trp.ShowDialog();

            WinPDFViewer wp = new WinPDFViewer(trp.PDFName, this.Title);
            wp.ShowDialog();
            wp = null;
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

        private void CmdCheque_Click(object sender, RoutedEventArgs e)
        {
            MVTaxDocument vm = (MVTaxDocument)vw;
            MAccountDoc ad = CTaxDocumentUtil.TaxDocToAccountDoc(vm, this);

            if (ad == null)
            {
                return;
            }

            if (vm.ChequeID.Equals(""))
            {
                ad.ArApAmt = vm.WhAmount;

                ObservableCollection<MBaseModel> arr = new ObservableCollection<MBaseModel>();
                CCriteriaCheque.ShowAddChequeWindow("2", arr, ad);

                if (arr.Count > 0)
                {
                    MCheque cq = (MCheque)arr[0];
                    vm.ChequeID = cq.ChequeID;
                    vm.ChequeNo = cq.ChequeNo;
                    vm.IsModified = true;
                }
            }
            else
            {
                CCriteriaCheque.ShowEditWindow("2", null, ad);
            }
        }
    }
}
