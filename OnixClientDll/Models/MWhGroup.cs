using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{

    public class MWhGroup : MBaseModel
    {
        public MWhGroup(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {

        }

        #region WH Group

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

        #endregion

        public String RevenueExpenseAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String amt = GetDbObject().GetFieldValue("EXPENSE_REVENUE_AMT");

                return (amt);
            }

            set
            {
                GetDbObject().SetFieldValue("EXPENSE_REVENUE_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String WhTaxAmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String amt = GetDbObject().GetFieldValue("WH_TAX_AMT");

                return (amt);
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String WhTaxAmtFmt
        {
            get
            {
                return (CUtil.FormatNumber(WhTaxAmt));
            }

            set
            {
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
    }
}
