using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using System.Collections;
using Onix.ClientCenter.UI.AccountPayable.TaxDocument.PP30;
using Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV3_53;
using Onix.Client.Model;
using System.Windows.Media;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument
{
    public class MVTaxDocument : MBaseModel
    {
        private MVTaxFormPP30 pp30 = new MVTaxFormPP30();
        private ObservableCollection<MVTaxFormPRV3_53> taxItems = new ObservableCollection<MVTaxFormPRV3_53>();

        public MVTaxDocument(CTable obj) : base(obj)
        {

        }        

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocApproved)
                {
                    return (false);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocCancelApproved)
                {
                    //Voided
                    return (false);
                }

                return (true);
            }
        }

        public Boolean? IsTaxDeductable
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_TAX_DEDUCTABLE");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("IS_TAX_DEDUCTABLE", flag);
            }
        }

        #region TaxFormPP30

        public MVTaxFormPP30 TaxFormPP30
        {
            get
            {
                return (pp30);
            }

            set
            {
            }
        }

        public void InitTaxFormPP30()
        {
            //pp30.ExtFlag = "A";
        }

        public void AddDefaultTaxPP30()
        {
            pp30.ExtFlag = "A";
            AddChildArray("PP30_DOC_LIST", pp30.GetDbObject());
        }

        #endregion

        #region Rev3_53

        public void PopulateWhItems(ArrayList items)
        {
            taxItems.Clear();
            foreach (CTable o in items)
            {
                MVTaxFormPRV3_53 m = new MVTaxFormPRV3_53(o);
                taxItems.Add(m);
            }
        }

        public ObservableCollection<MVTaxFormPRV3_53> WhItems
        {
            get
            {
                return (taxItems);
            }
        }

        #endregion

        public override void InitializeAfterLoaded()
        {
            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = GetChildArray("PP30_DOC_LIST");
            if (arr.Count > 0)
            {
                CTable t = (CTable)arr[0];
                pp30.SetDbObject(t);
                pp30.ExtFlag = "E";
            }

            ArrayList whItems = GetChildArray("WH_ITEMS");
            PopulateWhItems(whItems);
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("year", TaxYear);
            ttItems.Add(ct);

            CToolTipItem ct2 = new CToolTipItem("month", TaxMonthName);
            ttItems.Add(ct2);

            ct = new CToolTipItem("note", Note);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
        }
        
        public String TaxDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TAX_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_DOC_ID", value);
            }
        }

        public String TaxYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TAX_YEAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_YEAR", value);
            }
        }

        public String TaxYearBD
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                int y = CUtil.StringToInt(TaxYear);
                y = y + 543;
                return (y.ToString());
            }

            set
            {

            }
        }

        #region Tax Month

        public MMasterRef TaxMonthObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                TaxMonth = m.MasterID;
                TaxMonthName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.Months;
                if (items == null)
                {
                    return (null);
                }

                String tm = TaxMonth;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String TaxMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TAX_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_MONTH", value);
            }
        }

        public String TaxMonthName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String month = TaxMonth;
                if (month.Equals(""))
                {
                    return ("");
                }

                String tmp = CUtil.IDToMonth(CUtil.StringToInt(month));
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_MONTH_NAME", value);
                //NotifyPropertyChanged();
            }
        }

        #endregion Tax Month               

        #region Document Status

        public String DocumentStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentStatusDesc
        {
            get
            {
                if (DocumentStatus.Equals(""))
                {
                    return ("");
                }

                CashDocumentStatus dt = (CashDocumentStatus)Int32.Parse(DocumentStatus);
                String str = CUtil.CashDocStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
            }
        }

        #endregion

        #region Document Type

        public String DocumentType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_TYPE", value);
                NotifyPropertyChanged();
            }
        }        

        public String DocumentTypeDesc
        {
            get
            {
                if (DocumentType.Equals(""))
                {
                    return ("");
                }

                TaxDocumentType dt = (TaxDocumentType)Int32.Parse(DocumentType);
                String str = CUtil.TaxDocTypeToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef DocumentTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentType = m.MasterID;
            }
        }

        #endregion

        #region DocumentDate

        public DateTime? FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_DOCUMENT_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_DOCUMENT_DATE", str);
            }
        }

        public DateTime DocumentDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DOCUMENT_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        #endregion

        #region Cheque

        public String ChequeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String ChequeNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CHEQUE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("CHEQUE_NO", value);
                NotifyPropertyChanged();
            }
        }

        #endregion Cheque

        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                NotifyPropertyChanged();
            }
        }

        #region Rev3_53

        public String ItemCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_COUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ItemCountFmt");
            }
        }

        public String ItemCountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ItemCount);
                return (fmt);
            }

            set
            {
            }
        }

        public String WhAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WhAmountFmt");
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


        public String ExpenseAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_REVENUE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_REVENUE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ExpenseAmountFmt");
            }
        }

        public String ExpenseAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpenseAmount);
                return (fmt);
            }

            set
            {
            }
        }

        #endregion

        public String PreviousRunMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREVIOUS_RUN_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREVIOUS_RUN_MONTH", value);
                NotifyPropertyChanged();
            }
        }

        public String PreviousRunYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREVIOUS_RUN_YEAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREVIOUS_RUN_YEAR", value);
                NotifyPropertyChanged();
            }
        }
    }
}
