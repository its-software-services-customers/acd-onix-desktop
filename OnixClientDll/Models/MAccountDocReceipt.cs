using System;
using System.Windows;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MAccountDocReceipt : MBaseModel
    {
        private int seq = 0;
        private MMasterRef pmt = new MMasterRef(new CTable(""));
        private Boolean forDelete = false;
        private String oldFlag = "";

        public MAccountDocReceipt(CTable obj) : base(obj)
        {
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        public override void createToolTipItems()
        {
        }

        public String AccountDocReceiptID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACT_DOC_RECEIPT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACT_DOC_RECEIPT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String IndexProject
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INDEX_PROJECT"));
            }

            set
            {
                GetDbObject().SetFieldValue("INDEX_PROJECT", value);
                NotifyPropertyChanged();
            }
        }

        public String AccountDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ID", value);
                NotifyPropertyChanged();
            }
        }

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

        public String DocumentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String EntityID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDesc
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String DocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                updateFlag();
                NotifyPropertyChanged();
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

        public DateTime DueDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DUE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DueDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String ArApAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AR_AP_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AR_AP_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ArApAmtFmt");
            }
        }

        public String ArApAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(ArApAmt));
            }

            set
            {
            }
        }

        public String WHTaxAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(WHTaxAmt));
            }

            set
            {
            }
        }

        public String WHTaxAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("WHTaxAmtFmt");
            }
        }

        public String DayOverDue
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DAY_OVERDUE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DAY_OVERDUE", value);
                NotifyPropertyChanged();
            }
        }

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

        public String RevenueExpenseAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RevenueExpenseAmtFmt");
            }
        }

        public String RevenueExpenseForWhAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_FOR_WH_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_FOR_WH_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String RevenueExpenseForVatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String RevenueExpenseAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(RevenueExpenseAmt));
            }

            set
            {
            }
        }

        public String VatAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("VatAmtFmt");
            }
        }

        public String VatAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(VatAmt));
            }

            set
            {
            }
        }

        public String FinalDiscount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINAL_DISCOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINAL_DISCOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("FinalDiscountFmt");
            }
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(FinalDiscount));
            }

            set
            {
            }
        }

        public String CashReceiptAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CASH_RECEIPT_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("CASH_RECEIPT_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("CashReceiptAmtFmt");
            }
        }

        public String PricingAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_AMT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("PricingAmtFmt");
            }
        }

        public String PricingAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(PricingAmt));
            }

            set
            {
            }
        }

        public override SolidColorBrush RowColor
        {
            get
            {
                AccountDocumentType dt;
                dt = (AccountDocumentType) CUtil.StringToInt(DocumentType);
                if ((dt == AccountDocumentType.AcctDocCrNote) || (dt == AccountDocumentType.AcctDocCrNotePurchase))
                {
                    return (new SolidColorBrush(Colors.Red));
                }

                return (new SolidColorBrush(Colors.Blue));
            }
        }

        public String CashReceiptAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(CashReceiptAmt));
            }

            set
            {
            }
        }

        public String WhDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_DEFINITION", value);
                NotifyPropertyChanged();
            }
        }

        public String RefReceiptNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_RECEIPT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_RECEIPT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String RefPoNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_PO_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_PO_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String RefWhDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_WH_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_WH_DOC_NO", value);
                NotifyPropertyChanged();
            }
        }

        #region Project

        public MBaseModel ProjectObj
        {
            set
            {
                if (value == null)
                {
                    ProjectID = "";
                    ProjectName = "";
                    ProjectCode = "";

                    return;
                }

                MProject m = value as MProject;
                ProjectID = m.ProjectID;
                ProjectName = m.ProjectName;
                ProjectCode = m.ProjectCode;
                ProjectGroupName = m.ProjectGroupName;

                NotifyPropertyChanged("ProjectGroupName");
                NotifyPropertyChanged();
            }

            get
            {
                MProject m = new MProject(new CTable(""));
                m.ProjectID = ProjectID;
                m.ProjectName = ProjectName;
                m.ProjectCode = ProjectCode;

                return (m);
            }
        }

        public String ProjectCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_CODE", value);
            }
        }

        public String ProjectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_ID", value);
            }
        }

        public String ProjectName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_NAME", value);
            }
        }

        public String ProjectGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP_NAME", value);
            }
        }
        #endregion
    }
}
