using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Controller;

namespace Onix.Client.Models
{
    public class MCommissionBatchDetail : MBaseModel
    {
        int seq = 0;
        private Boolean forDelete = false;
        private String oldFlag = "";
        public MCommissionBatchDetail(CTable obj) : base(obj)
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

        public String CommissionBatchDTLID
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_BATCH_DTL_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_BATCH_DTL_ID", value);
            }
        }

        public String CommissionBatchID
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_BATCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_BATCH_ID", value);
            }
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

        public String EmployeeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_CODE", value);
            }
        }

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
            }
        }


        public String TotalBillCount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_BILL_COUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_BILL_COUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String TotalBillCountFmt
        {
            get
            {
                return (CUtil.FormatInt32(TotalBillCount));
            }
        }

        public String TotalBillAMT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_BILL_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_BILL_AMT", value);
                NotifyPropertyChanged();
            }
        }

        public String TotalBillAMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(TotalBillAMT));
            }
        }

        public String CommissionAMT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_COMMISSION_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_COMMISSION_AMT", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CommissionAMTFmt
        {
            get
            {
                return (CUtil.FormatNumber(CommissionAMT));
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

        public DateTime ModiflyDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("MODIFY_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("MODIFY_DATE", str);
                NotifyPropertyChanged();
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

    }
}
