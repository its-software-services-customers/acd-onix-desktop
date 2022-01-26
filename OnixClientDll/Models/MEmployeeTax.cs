using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MEmployeeTax : MBaseModel
    {
        public MEmployeeTax(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
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
                NotifyPropertyChanged();
            }
        }

        public String TaxYearThai
        {
            get
            {
                int year = CUtil.StringToInt(TaxYear);
                year = year + 543;

                return year.ToString();
            }

            set
            {
            }
        }

        public String TaxAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TAX_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TaxAmountFmt
        {
            get
            {
                return CUtil.FormatNumber(TaxAmount);
            }

            set
            {
            }
        }

        public String SocialInsuranceAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SOCIAL_INSURANCE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("SOCIAL_INSURANCE_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String SocialInsuranceAmountFmt
        {
            get
            {
                return CUtil.FormatNumber(SocialInsuranceAmount);
            }

            set
            {
            }
        }

        public String RevenueAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REVENUE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REVENUE_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }


        public String RevenueAmountFmt
        {
            get
            {
                return CUtil.FormatNumber(RevenueAmount);
            }

            set
            {
            }
        }
    }
}
