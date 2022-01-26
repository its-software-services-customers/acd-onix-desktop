using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Collections.ObjectModel;

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class MVPayrollExpenseItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private int seq = 0;

        public MVPayrollExpenseItem(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
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

        #region

        public MMasterRef ExpenseTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ExpenseType = m.MasterID;
                    ExpenseTypeDesc = m.Description;
                    NotifyPropertyChanged();
                    updateFlag();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.PayrollExpenseTypes;
                if (items == null)
                {
                    return (null);
                }

                return ((MMasterRef)CUtil.IDToObject(items, "MasterID", ExpenseType));
            }
        }
        
        public String ExpenseTypeDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (ExpenseTypeObj.Description);
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_TYPE_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String ExpenseType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_TYPE", value);
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
        
        public String ExpenseAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ExpenseAmountFmt");

                updateFlag();
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

        public String ExpenseNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_NOTE", value);
                updateFlag();
            }
        }

        public DateTime ExpenseDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EXPENSE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("EXPENSE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ExpenseDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str2 = CUtil.DateTimeToDateString(ExpenseDate);
                return (str2);
            }

            set
            {
            }
        }        

        public String ExpenseQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_QUANTITY", value);
                CalculateExpenseAmount();
                NotifyPropertyChanged("ExpenseQuantityFmt");
                updateFlag();
            }
        }

        public String ExpenseQuantityFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpenseQuantity);
                return (fmt);
            }

            set
            {
            }
        }

        public String ExpensePrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EXPENSE_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_PRICE", value);
                CalculateExpenseAmount();
                NotifyPropertyChanged("ExpensePriceFmt");
                updateFlag();
            }
        }

        public String ExpensePriceFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ExpensePrice);
                return (fmt);
            }
        }
        
        public void CalculateExpenseAmount()
        {
            double price = CUtil.StringToDouble(ExpensePrice);
            double quantity = CUtil.StringToDouble(ExpenseQuantity);
            double amt = price * quantity;

            ExpenseAmount = amt.ToString();
        }
    }
}
