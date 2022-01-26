using System;
using System.Collections;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.Commons.Factories
{
    class FactoryWindow
    {
        private static Hashtable windowMaps = null;

        private static void initMap()
        {
            windowMaps = new Hashtable();

            windowMaps["WinAddEditUser"] = "Onix.ClientCenter.UI.Admin.User.WinAddEditUser";
            windowMaps["WinAdminEditPassword"] = "Onix.ClientCenter.UI.Admin.User.WinAdminEditPassword";
            windowMaps["WinAddEditUserGroup"] = "Onix.ClientCenter.UI.Admin.UserGroup.WinAddEditUserGroup";
            windowMaps["WinAddEditGroupAccessRight"] = "Onix.ClientCenter.UI.Admin.UserGroup.WinAddEditGroupAccessRight";

            windowMaps["WinAddEditLocation"] = "Onix.ClientCenter.UI.Inventory.Location.WinAddEditLocation";
            windowMaps["WinAddEditInventoryItem"] = "Onix.ClientCenter.UI.Inventory.InventoryItem.WinAddEditInventoryItem";
            windowMaps["WinAddEditBorrowReturnItem"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.WinAddEditBorrowReturnItem";
            windowMaps["WinAddEditBorrowReturnDoc"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.WinAddEditBorrowReturnDoc";
            windowMaps["WinAddEditInventoryDoc"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.WinAddEditInventoryDoc";
            windowMaps["WinAddEditImportItem"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.WinAddEditImportItem";
            windowMaps["WinAddEditExportItem"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.WinAddEditExportItem";

            windowMaps["WinAddEditCustomer"] = "Onix.ClientCenter.UI.General.Entity.WinAddEditCustomer";
            windowMaps["WinAddEditSupplier"] = "Onix.ClientCenter.UI.General.Entity.WinAddEditSupplier";

            windowMaps["WinAddEditEmployeeInfo"] = "Onix.ClientCenter.UI.HumanResource.EmployeeInfo.WinAddEditEmployeeInfo";
            windowMaps["WinAddEditEmployeeLeave"] = "Onix.ClientCenter.UI.HumanResource.Leave.WinAddEditEmployeeLeave";            
            windowMaps["WinAddEditOrgChart"] = "Onix.ClientCenter.UI.HumanResource.OrgChart.WinAddEditOrgChart";
            windowMaps["WinAddEditPayrollDoc"] = "Onix.ClientCenter.UI.HumanResource.PayrollDocument.WinAddEditPayrollDoc";
            windowMaps["WinAddEditPayrollDocItem"] = "Onix.ClientCenter.UI.HumanResource.PayrollDocument.WinAddEditPayrollDocItem";
            windowMaps["WinAddEditOtDoc"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.WinAddEditOtDoc";
            windowMaps["WinAddEditOtDocItem"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.WinAddEditOtDocItem";
            windowMaps["WinAddEditOtDocItem2"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.WinAddEditOtDocItem2";
            windowMaps["WinAddEditPayrollExpenseItem"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.WinAddEditPayrollExpenseItem";
            windowMaps["WinAddEditPayrollDeductItem"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.WinAddEditPayrollDeductItem";

            windowMaps["WinAddEditCashAcc"] = "Onix.ClientCenter.UI.Cash.CashAccount.WinAddEditCashAcc";
            windowMaps["WinAddEditCashInOut"] = "Onix.ClientCenter.UI.Cash.CashInOut.WinAddEditCashInOut";
            windowMaps["WinAddEditCashTransfer"] = "Onix.ClientCenter.UI.Cash.CashXfer.WinAddEditCashTransfer";
            windowMaps["WinAddEditCashExternalXfer"] = "Onix.ClientCenter.UI.Cash.CashXfer.WinAddEditCashExternalXfer";
            windowMaps["WinAddEditCheque"] = "Onix.ClientCenter.UI.Cash.Cheque.WinAddEditCheque";
            windowMaps["WinAddEditService"] = "Onix.ClientCenter.UI.General.Service.WinAddEditService";
            windowMaps["WinAddEditMasterRef"] = "Onix.ClientCenter.UI.General.MasterReference.WinAddEditMasterRef";
            windowMaps["WinAddEditProject"] = "Onix.ClientCenter.UI.General.Project.WinAddEditProject";
            
            windowMaps["WinAddEditTaxPP30"] = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.PP30.WinAddEditTaxPP30";
            windowMaps["WinAddEditTaxFormPRV3_53"] = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53.WinAddEditTaxFormPRV3_53";
            windowMaps["WinAddEditTaxFormPRV1"] = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1.WinAddEditTaxFormPRV1";
            windowMaps["WinAddEditTaxFormPRV1Kor"] = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1Kor.WinAddEditTaxFormPRV1Kor";
            windowMaps["WinAddEditCostDocument"] = "Onix.ClientCenter.UI.AccountPayable.StockCost.WinAddEditCostDocument";            
        }

        private static String getClassName(String name)
        {
            if (windowMaps == null)
            {
                initMap();
            }

            String className = (String)windowMaps[name];
            if (className == null)
            {
                className = "Onix.ClientCenter.Windows." + name; //Legacy code
            }

            return (className);
        }

        public static WinBase GetObject(String name, CWinLoadParam loadParam)
        {
            if (windowMaps == null)
            {
                initMap();
            }

            String className = getClassName(name);

            WinBase wb = (WinBase) Activator.CreateInstance(Type.GetType(className), new object[] { loadParam });

            return (wb);
        }

        public static String GetFqdnClassName(String name)
        {
            String className = getClassName(name);
            return (className);
        }

        public static Boolean ShowWindow(String name, CWinLoadParam param)
        {
            String className = getClassName(name);
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
            Boolean result = cr.IsOKClick;

            return (result);
        }
    }
}
