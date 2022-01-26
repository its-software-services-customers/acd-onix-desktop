using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MPaymentCriteria : MBaseModel
    {
        private int seq = 0;
        private Boolean forDelete = false;
        private String oldFlag = "";

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

        public MPaymentCriteria(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CalculateExtraFields()
        {
            double exp = CUtil.StringToDouble(PaymentAmount);
            double vat = CUtil.StringToDouble(VatAmount);
            double wh = CUtil.StringToDouble(WhTaxAmount);

            VatIncludeAmount = (exp + vat).ToString();
            RemainAmount = (exp + vat - wh).ToString();
        }

        public void CalculateAuto(String tot)
        {
            double total = CUtil.StringToDouble(tot);

            double vatPct = CUtil.StringToDouble(VatPercent);
            double whPct = CUtil.StringToDouble(WhPercent);
            double exp = CUtil.StringToDouble(PaymentAmount);
            double pct = CUtil.StringToDouble(Percent);

            double payment = Math.Round(total * pct / 100, 2, MidpointRounding.AwayFromZero); // (total * pct / 100);
            double vatAmt = Math.Round(payment * vatPct / 100, 2, MidpointRounding.AwayFromZero); //(payment * vatPct / 100);
            double whAmt = Math.Round(payment * whPct / 100, 2, MidpointRounding.AwayFromZero);  //(payment * whPct / 100);

            PaymentAmount = payment.ToString();
            VatAmount = vatAmt.ToString();
            WhTaxAmount = whAmt.ToString();

            CalculateExtraFields();
        }

        public void CreateDefaultValue()
        {
        }

        public override bool IsEmpty
        {
            get
            {
                return (PaymentAmount.Trim().Equals("") || VatAmount.Trim().Equals("") ||
                    WhTaxAmount.Trim().Equals("") || (Description.Trim().Equals("")));
            }
        }

        public String PaymentCriteriaID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYMENT_CRITERIA_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_CRITERIA_ID", value);
            }
        }

        public Boolean IsCalculateManual
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ManualCalculateFlag.Equals("Y"));
            }

            set
            {
                if (value)
                {
                    ManualCalculateFlag = "Y";
                }
                else
                {
                    ManualCalculateFlag = "N";
                }
            }
        }

        public String ManualCalculateFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MANUAL_CALCULATE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("MANUAL_CALCULATE_FLAG", value);
                updateFlag();
            }
        }

        public String Percent
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PERCENT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PERCENT", value);
                updateFlag();
            }
        }

        public String VatPercent
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VAT_PERCENT"));
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_PERCENT", value);
                updateFlag();
            }
        }

        public String WhPercent
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_PERCENT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_PERCENT", value);
                updateFlag();
            }
        }

        public String Description
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);
                updateFlag();
            }
        }

        public String PaymentAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("PAYMENT_AMT");
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("PAYMENT_AMT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        
        public String PaymentAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(PaymentAmount));
            }

            set
            {
            }
        }

        public String VatAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("VAT_AMT");
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_AMT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String VatAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(VatAmount));
            }

            set
            {
            }
        }

        public String WhTaxAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("WH_TAX_AMT");
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);
                updateFlag();
                NotifyPropertyChanged("IsWhGroupRequired");
                NotifyPropertyChanged();
            }
        }

        public String WhTaxAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(WhTaxAmount));
            }

            set
            {
            }
        }

        public String VatIncludeAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("VAT_INCLUDE_AMT");
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("VAT_INCLUDE_AMT", value);
                updateFlag();                
                NotifyPropertyChanged();
                NotifyPropertyChanged("VatIncludeAmount");
            }
        }

        public String VatIncludeAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(VatIncludeAmount));
            }

            set
            {
            }
        }

        public String RemainAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String tmp = GetDbObject().GetFieldValue("REMAIN_AMT");
                return (tmp);
            }

            set
            {
                GetDbObject().SetFieldValue("REMAIN_AMT", value);
                updateFlag();
                NotifyPropertyChanged();
                NotifyPropertyChanged("RemainAmountFmt");
            }
        }

        public String RemainAmountFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(RemainAmount));
            }

            set
            {
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

        #region For Searching

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
                NotifyPropertyChanged();
            }
        }

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
            }
        }

        public String EntityCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_CODE", value);
            }
        }

        public String EntityName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_NAME", value);
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
            }
        }

        #endregion

        #region Project

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

        public String AuxilaryDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_ID", value);
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

        #region WH Group

        public MMasterRef WHGroupObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    WHGroup = m.MasterID;
                    WHGroupName = m.Description;
                    WHGroupNameEng = m.DescriptionEng;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = WHGroup;
                mr.Description = WHGroupName;
                mr.DescriptionEng = WHGroupNameEng;

                return (mr);
            }
        }

        public String WHGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        public String WHGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String WHGroupNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsWhGroupRequired
        {
            get
            {
                double amt = CUtil.StringToDouble(WhTaxAmount);
                return (amt > 0);
            }

            set
            {
            }
        }

        #endregion

        public String PoInvoiceRefType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PO_INVOICE_REF_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PO_INVOICE_REF_TYPE", value);
            }
        }

        public String RefByDocNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_BY_DOC_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_BY_DOC_NO", value);
            }
        }

        public Boolean? IsIncludable
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INCLUDE_ABLE_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("N"));
            }

            set
            {
                String flag = "Y";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "N";
                }

                GetDbObject().SetFieldValue("INCLUDE_ABLE_FLAG", flag);
                NotifyPropertyChanged();
            }
        }
    }
}
