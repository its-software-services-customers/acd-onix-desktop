using System;
using System.Windows;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.StockCost
{    
    public partial class WinAddEditCostDocument : WinBase
    {
        public WinAddEditCostDocument(CWinLoadParam param) : base(param)
        {
            accessRightName = "PURCHASE_COST_EDIT";

            createAPIName = "CreateCostDocument";
            updateAPIName = "UpdateCostDocument";
            getInfoAPIName = "GetCostDocumentInfo";
            approveAPIName = "ApproveCostDocument";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblYear, txtYear, false);
        }

        protected override bool isEditable()
        {
            MVStockCostDocument mv = (MVStockCostDocument)loadParam.ActualView;
            if (mv != null)
            {
                return (mv.IsEditable);
            }

            return (true);
        }

        protected override MBaseModel createObject()
        {
            MVStockCostDocument mv = new MVStockCostDocument(new CTable(""));
            mv.DocumentStatus = "1";

            return (mv);
        }

        private void CmdAddProduct_Click(object sender, RoutedEventArgs e)
        {

        }       

        private void CmdOK_Click(object sender, RoutedEventArgs e)
        {
            constructObject();
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        private void constructObject()
        {
            MVStockCostDocument mv = (MVStockCostDocument) vw;
            mv.constructTableObject();
        }

        private void CmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            constructObject();
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
            MVStockCostDocument mv = (MVStockCostDocument) vw;

            Boolean r = approveData();
            if (r)
            {
                vw.IsModified = false;
                this.Close();
            }
        }

        private void LsvAccoutItem_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void LsvAccoutItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {            
        }

        private void UCostDistribution_TextChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
            (vw as MVStockCostDocument).CalculateOutStock();
        }
    }
}
