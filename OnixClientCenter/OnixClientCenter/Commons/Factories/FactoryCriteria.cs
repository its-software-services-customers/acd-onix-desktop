using System;
using System.Collections;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.CriteriaConfig;

namespace Onix.ClientCenter.Commons.Factories
{
    class FactoryCriteria
    {
        private static Hashtable criteriaMaps = null;

        private static void initMap()
        {
            criteriaMaps = new Hashtable();

            criteriaMaps["CCriteriaLocation"] = "Onix.ClientCenter.UI.Inventory.Location.CCriteriaLocation";
            criteriaMaps["CCriteriaInventoryItem"] = "Onix.ClientCenter.UI.Inventory.InventoryItem.CCriteriaInventoryItem";
            criteriaMaps["CCriteriaInventoryDoc"] = "Onix.ClientCenter.UI.Inventory.InventoryDocument.CCriteriaInventoryDoc";

            criteriaMaps["CCriteriaTaxDocument"] = "Onix.ClientCenter.UI.AccountPayable.TaxDocument.CCriteriaTaxDocument";
            criteriaMaps["CCriteriaStockCostDocument"] = "Onix.ClientCenter.UI.AccountPayable.StockCost.CCriteriaStockCostDocument";
            
            criteriaMaps["CCriteriaEmployee"] = "Onix.ClientCenter.UI.HumanResource.EmployeeInfo.CCriteriaEmployee";
            criteriaMaps["CCriteriaPayrollDocument"] = "Onix.ClientCenter.UI.HumanResource.PayrollDocument.CCriteriaPayrollDocument";
            criteriaMaps["CCriteriaOTDocument"] = "Onix.ClientCenter.UI.HumanResource.OTDocument.CCriteriaOTDocument";
            criteriaMaps["CCriteriaHrTaxDocument"] = "Onix.ClientCenter.UI.HumanResource.TaxForm.CCriteriaHrTaxDocument";
            criteriaMaps["CCriteriaHrTaxDocumentKor"] = "Onix.ClientCenter.UI.HumanResource.TaxForm.CCriteriaHrTaxDocumentKor";            
            criteriaMaps["CCriteriaEmployeeLeave"] = "Onix.ClientCenter.UI.HumanResource.Leave.CCriteriaEmployeeLeave";

            criteriaMaps["CCriteriaUser"] = "Onix.ClientCenter.UI.Admin.User.CCriteriaUser";
            criteriaMaps["CCriteriaLoginHistory"] = "Onix.ClientCenter.UI.Admin.LoginHistory.CCriteriaLoginHistory";
            criteriaMaps["CCriteriaUserGroup"] = "Onix.ClientCenter.UI.Admin.UserGroup.CCriteriaUserGroup";

            criteriaMaps["CCriteriaCashAccount"] = "Onix.ClientCenter.UI.Cash.CashAccount.CCriteriaCashAccount";
            criteriaMaps["CCriteriaCheque"] = "Onix.ClientCenter.UI.Cash.Cheque.CCriteriaCheque";
            criteriaMaps["CCriteriaCashInOut"] = "Onix.ClientCenter.UI.Cash.CashInOut.CCriteriaCashInOut";
            criteriaMaps["CCriteriaCashXfer"] = "Onix.ClientCenter.UI.Cash.CashXfer.CCriteriaCashXfer";

            criteriaMaps["CCriteriaService"] = "Onix.ClientCenter.UI.General.Service.CCriteriaService";
            criteriaMaps["CCriteriaMasterRef"] = "Onix.ClientCenter.UI.General.MasterReference.CCriteriaMasterRef";
            criteriaMaps["CCriteriaEntity"] = "Onix.ClientCenter.UI.General.Entity.CCriteriaEntity";
            criteriaMaps["CCriteriaProject"] = "Onix.ClientCenter.UI.General.Project.CCriteriaProject";
        }

        private static String getClassName(String name)
        {
            if (criteriaMaps == null)
            {
                initMap();
            }

            String className = (String)criteriaMaps[name];
            if (className == null)
            {
                className = "Onix.ClientCenter.Criteria." + name; //Legacy code
            }

            return (className);
        }

        public static CCriteriaBase GetObject(String name, MBaseModel param)
        {
            if (criteriaMaps == null)
            {
                initMap();
            }

            String className = getClassName(name);

            CCriteriaBase cr = (CCriteriaBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });

            return (cr);
        }

        public static CCriteriaBase GetObject(String name)
        {
            if (criteriaMaps == null)
            {
                initMap();
            }

            String className = getClassName(name);

            CCriteriaBase cr = (CCriteriaBase)Activator.CreateInstance(Type.GetType(className));

            return (cr);
        }

        public static String GetFqdnClassName(String name)
        {
            String className = getClassName(name);
            return (className);
        }
    }
}
