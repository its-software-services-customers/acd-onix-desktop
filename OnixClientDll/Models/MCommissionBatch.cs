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
    public class MCommissionBatch : MBaseModel
    {
        private ObservableCollection<MCommissionBatchDetail> commissioBatchItem = new ObservableCollection<MCommissionBatchDetail>();

        public MCommissionBatch(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("docno", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", DocumentDesc);
            ttItems.Add(ct);

            ct = new CToolTipItem("effective_date", DueDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("Docu_Status", DocumentStatusDesc);
            ttItems.Add(ct);


        }

        public ObservableCollection<MCommissionBatchDetail> CommissioBatchItem
        {
            get
            {
                return (commissioBatchItem);
            }
        }

        public void InitCommissionBatchItem(String mode)
        {
            commissioBatchItem.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_BATCH_ITEM");

            if (arr == null)
            {
                return;
            }

            int seq = 0;
            foreach (CTable t in arr)
            {
                MCommissionBatchDetail v = new MCommissionBatchDetail(t);
                commissioBatchItem.Add(v);
                seq++;
                v.Seq = seq;
                v.ExtFlag = mode;
            }
        }


        public void ProcessCommissionBatch(String mode)
        {
            commissioBatchItem.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COMMISSION_BATCH_ITEM");

            if (arr == null)
            {
                return;
            }

            int seq = 0;
            foreach (CTable t in arr)
            {
                t.RemoveChildArray("COMMISSION_INPUT");
                t.RemoveChildArray("COMMISSION_ITEM");

                MCommissionBatchDetail v = new MCommissionBatchDetail(t);
                commissioBatchItem.Add(v);
                seq++;
                v.Seq = seq;
                v.ExtFlag = mode;
            }
        }

        public void CreateDefaultValue()
        {
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

        public String CycleTypeIcon
        {
            get
            {
                if (CycleType.Equals("1"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/view-calendar-week.png");
                }
                else if (CycleType.Equals("2"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/view-calendar-month.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/view-calendar-week.png");
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

                return (GetDbObject().GetFieldValue("BATCH_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("BATCH_DESC", value);
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

        public String RunDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("RUN_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }
        }

        public DateTime RunDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("RUN_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("RUN_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String CycleID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_ID", value);
            }
        }

        public MMasterRef CycleTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    CycleType = m.MasterID;
                    CycleTypeName = m.Description;
                }
            }
        }
        public String CycleType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_TYPE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_TYPE_NAME", value);
            }
        }

        public MCycle CycleIDObj
        {
            set
            {
                MCycle m = value as MCycle;
                if (m != null)
                {
                    CycleID = m.CycleID;
                    CycleIDName = m.CycleDescription;
                    CycleCode = m.CycleCode;
                }
            }
        }
        public String CycleCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String CycleIDName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CYCLE_ID_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CYCLE_ID_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String CycleName
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
                NotifyPropertyChanged();
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

                return (GetDbObject().GetFieldValue("COMMISSION_AMT"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_AMT", value);
                NotifyPropertyChanged();
            }
        }


        public String CommissionBatchDocumentNo
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_BATCH_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_BATCH_NO", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsSalesmanSpecific
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("SALESMAN_SPECIFIC_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
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
                GetDbObject().SetFieldValue("SALESMAN_SPECIFIC_FLAG", flag);
                NotifyPropertyChanged();
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

                return (GetDbObject().GetFieldValue("EMP_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMP_ID", value);
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

                return (GetDbObject().GetFieldValue("EMP_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMP_CODE", value);
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

                return (GetDbObject().GetFieldValue("EMP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMP_NAME", value);
            }
        }

        public String BillAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BILL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BILL_AMOUNT", value);
            }
        }

        public String SumBilling
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SUM_BILLING"));
            }

            set
            {
                GetDbObject().SetFieldValue("SUM_BILLING", value);
            }
        }

        public String CommCalculate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMM_CALCULATE"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMM_CALCULATE", value);
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

        public DateTime? FromDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMin(value);

                GetDbObject().SetFieldValue("FROM_RUN_DATE", str);
            }
        }

        public DateTime? ToDocumentDate
        {
            set
            {
                String str = CUtil.DateTimeToDateStringInternalMax(value);

                GetDbObject().SetFieldValue("TO_RUN_DATE", str);
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

                return (GetDbObject().GetFieldValue("BATCH_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("BATCH_STATUS", value);
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

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)CashDocumentStatus.CashDocApproved)
                {
                    return (false);
                }

                return (true);
            }
        }

        public String CEmployeeID
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

        public String CEmployeeCode
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

        public String CEmployeeName
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
    }

}
