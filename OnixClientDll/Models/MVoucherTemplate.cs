using Wis.WsClientAPI;
using System;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MVoucherTemplate : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        public MVoucherTemplate(CTable obj) : base(obj)
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

        public String VoucherTempID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOUCHER_TEMPLATE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOUCHER_TEMPLATE_ID", value);
            }
        }

        public String VoucherTempNO
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VC_TEMPLATE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("VC_TEMPLATE_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String VoucherTempName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VC_TEMPLATE_NNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VC_TEMPLATE_NNAME", value);
                NotifyPropertyChanged();
            }
        }

        public String EnableFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENABLE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENABLE_FLAG", value);
                NotifyPropertyChanged();
            }
        }

        public String Amount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String AmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(Amount));
            }
        }

        public String Quantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY", value);
                NotifyPropertyChanged();
            }
        }

        public String QuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(Quantity));
            }
        }

        public DateTime EffectiveDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EFFECTIVE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("EFFECTIVE_DATE", str);
                NotifyPropertyChanged();
            }
        }
        public String EffectiveDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("EFFECTIVE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);

            }
            set
            {
            }
        }
        public DateTime? From_EffectiveDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_EFFECTIVE_DATE", str);
                NotifyPropertyChanged();
            }
        }
        public DateTime? To_EffectiveDate
        {
            set
            {

                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_EFFECTIVE_DATE", str);
                //NotifyPropertyChanged();
            }
        }

        public DateTime ExpireDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EXPIRE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("EXPIRE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ExpireDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("EXPIRE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }
        public DateTime? From_ExpireDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_EXPIRE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public DateTime? To_ExpireDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_EXPIRE_DATE", str);
                //NotifyPropertyChanged();
            }
        }

        public DateTime CreateDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("CREATE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("CREATE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String IsEnabledIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ENABLE_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }
    }
}
