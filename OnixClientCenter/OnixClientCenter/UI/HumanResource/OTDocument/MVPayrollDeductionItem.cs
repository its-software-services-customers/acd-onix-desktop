using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Collections.ObjectModel;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class MVPayrollDeductionItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        public MVPayrollDeductionItem(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
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

        #region

        public MMasterRef DeductionTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    DeductionType = m.MasterID;
                    DeductionTypeDesc = m.Description;
                    DurationUnit = m.Code;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.PayrollDeductionTypes;
                if (items == null)
                {
                    return (null);
                }

                return ((MMasterRef)CUtil.IDToObject(items, "MasterID", DeductionType));
            }
        }
        
        public String DeductionTypeDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (DeductionTypeObj.Description);
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_TYPE_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String DeductionType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_TYPE", value);
                NotifyPropertyChanged();
            }
        }
        
        #endregion

        public String OTDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("OT_DOC_ID", value);
            }
        }
        
        public String DeductionAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductionAmountFmt");

                updateFlag();
            }
        }

        public String DeductionAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductionAmount);
                return (fmt);
            }

            set
            {
            }
        }        

        public String DeductionNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCTION_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCTION_NOTE", value);
                updateFlag();
            }
        }

        public DateTime DeductionDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DEDUCTION_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DEDUCTION_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DeductionDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str2 = CUtil.DateTimeToDateString(DeductionDate);
                return (str2);
            }

            set
            {
            }
        }

        public String Duration
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DURATION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DURATION", value);
                populateDurationMin();

                updateFlag();
            }
        }

        private void populateDurationMin()
        {
            string unit = DurationUnit;
            string duration = Duration;
            string hint = "";

            double minutes = CUtil.StringToDouble(duration);
            if (unit.Equals("hour"))
            {
                minutes = minutes * 60;
                hint = String.Format("({0} {1})", minutes, CLanguage.getValue("minute"));
            }

            DurationMinHint = hint;
            DurationMin = minutes.ToString();
        }

        public String DurationMin
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DURATION_MINUTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DURATION_MINUTE", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DurationMinFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DurationMin);
                return (fmt);
            }

            set
            {
            }
        }
        
        public String DurationUnit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (DeductionTypeObj.Code);
            }

            set
            {
                GetDbObject().SetFieldValue("DURATION_UNIT", value);

                populateDurationMin();
                NotifyPropertyChanged("DurationUnitDesc");
                updateFlag();
            }
        }

        public String DurationUnitDesc
        {
            get
            {
                string desc = CLanguage.getValue(DurationUnit);
                return (desc);
            }

            set
            {

            }
        }

        public String DurationMinHint
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DURATION_HINT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DURATION_HINT", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        
        //public void CalculateExpenseAmount()
        //{
        //    double price = CUtil.StringToDouble(ExpensePrice);
        //    double quantity = CUtil.StringToDouble(ExpenseQuantity);
        //    double amt = price * quantity;

        //    ExpenseAmount = amt.ToString();
        //}
    }
}
