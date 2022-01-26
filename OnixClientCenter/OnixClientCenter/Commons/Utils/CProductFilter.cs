using System;
using System.Collections;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.Utils
{
    public static class CProductFilter
    {
        private static Hashtable refTypeHoppers = new Hashtable();
        private static Hashtable docTypeHoppers = new Hashtable();
        private static Hashtable globalVariableHoppers = new Hashtable();

        private static void initRefTypeHopper()
        {
            refTypeHoppers[MasterRefEnum.MASTER_ITEM_TYPE] = "YNN";
            refTypeHoppers[MasterRefEnum.MASTER_UOM] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_BRAND] = "YNN";
            refTypeHoppers[MasterRefEnum.MASTER_LOCATION_TYPE] = "YNN";
            refTypeHoppers[MasterRefEnum.MASTER_CUSTOMER_TYPE] = "YYY";
            refTypeHoppers[MasterRefEnum.MASTER_CUSTOMER_GROUP] = "YYY";
            refTypeHoppers[MasterRefEnum.MASTER_SERVICE_TYPE] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_MEMBER_TYPE] = "NNN";
            refTypeHoppers[MasterRefEnum.MASTER_BANK] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_BRANCH] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_EMPLOYEE_TYPE] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_EMPLOYEE_GROUP] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_BARCODE_TYPE] = "YNN";
            refTypeHoppers[MasterRefEnum.MASTER_ADDRESS_TYPE] = "YYY";
            refTypeHoppers[MasterRefEnum.MASTER_SUPPLIER_TYPE] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_SUPPLIER_GROUP] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_NAME_PREFIX] = "YYY";
            refTypeHoppers[MasterRefEnum.MASTER_PROJECT_GROUP] = "YYY";
            refTypeHoppers[MasterRefEnum.MASTER_VOID_REASON] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_BANK_ACCOUNT_TYPE] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_CURRENCY] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_TERM_OF_PAYMENT] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_WH_GROUP] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_DISCOUNT_TYPE] = "YYN";
            refTypeHoppers[MasterRefEnum.MASTER_REASON_TYPE] = "YNN";
            refTypeHoppers[MasterRefEnum.MASTER_CREDIT_TERM] = "YNN";
        }

        private static void initDocTypeFormatHopper()
        {
            //1=ONix, 2=Lotto, 3=Sass
            docTypeHoppers["ACCOUNT_DOC_CASH_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CASH_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CASH_BUY_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CASH_BUY_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CASH_BUY_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CASH_BUY_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CASH_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CASH_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CR_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CR_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CR_BUY_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CR_BUY_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CR_BUY_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CR_BUY_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_CR_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_CR_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DEPT_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DEPT_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DEPT_BUY_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DEPT_BUY_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DEPT_BUY_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DEPT_BUY_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DEPT_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DEPT_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DR_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DR_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DR_BUY_APPROVED"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DR_BUY_APPROVED_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DR_BUY_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DR_BUY_TEMP_NV"] = "NNN";
            docTypeHoppers["ACCOUNT_DOC_DR_TEMP"] = "YYY";
            docTypeHoppers["ACCOUNT_DOC_DR_TEMP_NV"] = "NNN";
            docTypeHoppers["AUX_DOC_PO_TEMP"] = "YYY";
            docTypeHoppers["AUX_DOC_QUOTATION_TEMP"] = "YYY";
            docTypeHoppers["DEPOSIT_DOC_PURCHASE_APPROVED"] = "NNN";
            docTypeHoppers["DEPOSIT_DOC_PURCHASE_TEMP"] = "NNN";
            docTypeHoppers["DEPOSIT_DOC_SALE_APPROVED"] = "NNN";
            docTypeHoppers["DEPOSIT_DOC_SALE_TEMP"] = "NNN";
            docTypeHoppers["INVENTORY_DOC_ADJ_APPROVED"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_ADJ_TEMP"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_EXFER_APPROVED"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_EXFER_TEMP"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_EXPORT_APPROVED"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_EXPORT_TEMP"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_IMPORT_APPROVED"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_IMPORT_TEMP"] = "YNN";
            docTypeHoppers["PURCHASE_DOC_EXPENSE_APPROVED"] = "YNN";
            docTypeHoppers["PURCHASE_DOC_EXPENSE_TEMP"] = "YNN";
            docTypeHoppers["PURCHASE_DOC_RECEIPT_APPROVED"] = "YYY";
            docTypeHoppers["PURCHASE_DOC_RECEIPT_TEMP"] = "YYY";
            docTypeHoppers["SALE_BILL_SUMMARY"] = "YYY";
            docTypeHoppers["SALE_DOC_RECEIPT_APPROVED"] = "YYY";
            docTypeHoppers["SALE_DOC_RECEIPT_TEMP"] = "YYY";
            docTypeHoppers["SALE_DOC_REVENUE_APPROVED"] = "YYY";
            docTypeHoppers["SALE_DOC_REVENUE_TEMP"] = "YYY";
            docTypeHoppers["SO_DOC_NUMBER"] = "YYY";
            docTypeHoppers["WH_DOC_NUMBER"] = "YYY";
            docTypeHoppers["INVENTORY_DOC_BORROW_TEMP"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_BORROW_APPROVED"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_RETURN_TEMP"] = "YNN";
            docTypeHoppers["INVENTORY_DOC_RETURN_APPROVED"] = "YNN";
        }

        private static void initGlobalVariableHopper()
        {
            //1=Onix, 2=Lotto, 3=Sass
            globalVariableHoppers["VAT_PERCENTAGE"] = "YYY";
            globalVariableHoppers["DOC_SEQ_LENGTH_DEFAULT"] = "YYY";
            globalVariableHoppers["DOC_NO_CASH_DEFAULT"] = "NNN";
            globalVariableHoppers["DOC_NO_DEBT_DEFAULT"] = "NNN";
            globalVariableHoppers["DOC_NO_CASH_DEFAULT_NV"] = "NNN";
            globalVariableHoppers["DOC_NO_DEBT_DEFAULT_NV"] = "NNN";
            globalVariableHoppers["DOC_NO_YEAR_OFFSET_DEFAULT"] = "NNN";
            globalVariableHoppers["DOC_NO_RESET_DEFAULT"] = "NNN";
            globalVariableHoppers["COMPANY_LOGO_URL"] = "NNN"; //Obsolete
            globalVariableHoppers["DEFAULT_VAT_TYPE_PERCHASE"] = "YYY";
            globalVariableHoppers["DEFAULT_SELECTION_TYPE_PERCHASE"] = "YYY";
            globalVariableHoppers["SALE_CASH_CHANGE_TYPE"] = "YYY";
            globalVariableHoppers["MAX_OVERHEAD_APPROVE_DATE"] = "YYY";
            globalVariableHoppers["DEFAULT_VAT_TYPE_SALE"] = "YYY";
            globalVariableHoppers["DEFAULT_SELECTION_TYPE_SALE"] = "YYY";
            globalVariableHoppers["SALE_DEBT_CHANGE_TYPE"] = "YYY";
            globalVariableHoppers["PURCHASE_CASH_CHANGE_TYPE"] = "YYY";
            globalVariableHoppers["PURCHASE_DEBT_CHANGE_TYPE"] = "YYY";
            globalVariableHoppers["SALE_PETTY_CASH_ACCT_NO"] = "YYY";
            globalVariableHoppers["BALANCE_DATE_BY_CURRENT_DATE"] = "YYY";
            globalVariableHoppers["ALLOW_NEGATIVE_STRING"] = "YYY";
            globalVariableHoppers["APPROVED_DOC_NUMBER_STRING"] = "YYY";
            globalVariableHoppers["OWNER_PAYMENT_TYPE_SET"] = "YYY";
            globalVariableHoppers["CHEQUE_APPROVE_IMMEDIATE_FLAG"] = "YYY";
            globalVariableHoppers["REVENUE_DEP_SUPPLIER_CODE"] = "YNY";
            globalVariableHoppers["PAYROLL_SERVICE_CODE"] = "YNN";
            globalVariableHoppers["PAYROLL_PROJECT_CODE"] = "YNN";
            globalVariableHoppers["SOCIAL_SECURITY_SERVICE_CODE"] = "YNN";
        }

        public static Boolean IsRequiredByProduct(String hopper)
        {
            String product = CConfig.GetProduct();

            Boolean isForOnix = hopper.Substring(0, 1).Equals("Y");
            Boolean isForLotto = hopper.Substring(1, 1).Equals("Y");
            Boolean isForSass = hopper.Substring(2, 1).Equals("Y");

            if (product.Equals("onix") && isForOnix)
            {
                return (true);
            }

            if (product.Equals("lotto") && isForLotto)
            {
                return (true);
            }

            if (product.Equals("sass") && isForSass)
            {
                return (true);
            }

            return (false);
        }

        public static int GetThemeIndex()
        {
            String product = CConfig.GetProduct();

            if (product.Equals("onix"))
            {
                return (5);
            }

            if (product.Equals("sass"))
            {
                return (6);
            }

            return (5);
        }

        public static Boolean IsRefTypeRequired(MasterRefEnum rt)
        {
            initRefTypeHopper();

            String hopper = (String) refTypeHoppers[rt];
            Boolean isRequired = IsRequiredByProduct(hopper);

            return (isRequired);
        }

        public static Boolean IsDocTypeRequired(String docType)
        {
            initDocTypeFormatHopper();

            String hopper = (String) docTypeHoppers[docType];
            Boolean isRequired = IsRequiredByProduct(hopper);

            return (isRequired);
        }

        public static Boolean IsGlobalVaribleRequired(String variable)
        {
            initGlobalVariableHopper();

            String hopper = (String)globalVariableHoppers[variable];
            Boolean isRequired = IsRequiredByProduct(hopper);

            return (isRequired);
        }
    }
}
