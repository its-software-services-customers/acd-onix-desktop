using System;
using System.Collections.ObjectModel;
using Onix.Client.Model;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1
{
    public class MVTaxFormPRV1 : MBaseModel
    {
        private ObservableCollection<MMasterRef> forms = new ObservableCollection<MMasterRef>();

        public MVTaxFormPRV1() : base(new CTable(""))
        {

        }

        public MVTaxFormPRV1(CTable o) : base(o)
        {

        }

        public String TaxID
        {
            get
            {
                return (GetFieldValue("TAX_ID"));
            }

            set
            {
                SetFieldValue("TAX_ID", value);
            }
        }

        public String SupplierName
        {
            get
            {
                return (GetFieldValue("SUPPLIER_NAME"));
            }

            set
            {
                SetFieldValue("SUPPLIER_NAME", value);
            }
        }

        public String DocumentType
        {
            get
            {
                return (GetFieldValue("DOCUMENT_TYPE"));
            }

            set
            {
                SetFieldValue("DOCUMENT_TYPE", value);
            }
        }

        public String SupplierAddress
        {
            get
            {
                return (GetFieldValue("SUPPLIER_ADDRESS"));
            }

            set
            {
                SetFieldValue("SUPPLIER_ADDRESS", value);
            }
        }

        public String SupplierTaxID
        {
            get
            {
                return (GetFieldValue("SUPPLIER_TAX_ID"));
            }

            set
            {
                SetFieldValue("SUPPLIER_TAX_ID", value);
            }
        }

        public String WhGroup
        {
            get
            {
                return (GetFieldValue("WH_GROUP"));
            }

            set
            {
                SetFieldValue("WH_GROUP", value);
            }
        }

        public String WhGroupDesc
        {
            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.GetMasterRefCollection(MasterRefEnum.MASTER_WH_GROUP);
                MMasterRef mr = CUtil.MasterIDToObject(items, WhGroup);
                if (mr == null)
                {
                    mr = new MMasterRef(new CTable(""));
                }

                return (mr.Description);
            }

            set
            {

            }
        }

        public String WhPayType
        {
            get
            {
                return (GetFieldValue("WH_PAY_TYPE"));
            }

            set
            {
                SetFieldValue("WH_PAY_TYPE", value);
            }
        }

        public String DocumentNo
        {
            get
            {
                return (GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                SetFieldValue("DOCUMENT_NO", value);
            }
        }

        public String DocumentDate
        {
            get
            {
                return (GetFieldValue("DOCUMENT_DATE"));
            }

            set
            {
                SetFieldValue("DOCUMENT_DATE", value);
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                String str = DocumentDate;
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {

            }
        }

        #region WH

        public String WhPct
        {
            get
            {
                return (GetFieldValue("WH_PCT"));
            }

            set
            {
                SetFieldValue("WH_PCT", value);
            }
        }

        public String WhAmount
        {
            get
            {
                return (GetFieldValue("WH_AMOUNT"));
            }

            set
            {
                SetFieldValue("WH_AMOUNT", value);
            }
        }

        public String WhAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(WhAmount);
                return (fmt);
            }

            set
            {
            }
        }        

        #endregion

        #region Purchase

        public String ExpenseAmt
        {
            get
            {
                return (GetFieldValue("EXPENSE_REVENUE_AMT"));
            }

            set
            {
                SetFieldValue("EXPENSE_REVENUE_AMT", value);
            }
        }

        public String ExpenseAmtFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpenseAmt);
                return (fmt);
            }

            set
            {
            }
        }

        #endregion

    }
}
