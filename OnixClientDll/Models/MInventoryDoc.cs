using System;
using System.Collections.ObjectModel;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MInventoryDoc : MBaseModel
    {
        private ObservableCollection<MInventoryTransaction> txitems = new ObservableCollection<MInventoryTransaction>();
        private ObservableCollection<MInventoryAdjustment> adjustmentItems = new ObservableCollection<MInventoryAdjustment>();
        private ObservableCollection<MError> errorItems = new ObservableCollection<MError>();

        private int internalSeq = 0;

        public MInventoryDoc(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_date", DocumentDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_desc", Note);
            ttItems.Add(ct);

            ct = new CToolTipItem("location_name", LocationName);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_status", DocumentStatusDesc);
            ttItems.Add(ct);

        }

        public override void InitializeAfterLoaded()
        {
            InventoryDocumentType dt = (InventoryDocumentType) CUtil.StringToInt(DocumentType);
            InitTxItem(dt);
        }

        public void CreateDefaultValue()
        {
            IsTotalRemainedAmount = true;
        }

        public ObservableCollection<MError> ErrorItems
        {
            get
            {
                return (errorItems);
            }
        }

        public ObservableCollection<MInventoryTransaction> TxItems
        {
            get
            {
                return (txitems);
            }
        }

        public ObservableCollection<MInventoryAdjustment> AdjustmentItems
        {
            get
            {
                return (adjustmentItems);
            }
        }

        public void AddAdjustment(MInventoryAdjustment v)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ADJUSTMENT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ADJUSTMENT_ITEM", arr);
            }

            v.Seq = internalSeq;
            internalSeq++;
            v.AdjustmentByDetails = AdjustmentBy;
            arr.Add(v.GetDbObject());
            adjustmentItems.Add(v);

            v.ExtFlag = "A";
        }

        public void AddAdjustment()
        {
            CTable t = new CTable("");
            MInventoryAdjustment v = new MInventoryAdjustment(t);

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ADJUSTMENT_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("ADJUSTMENT_ITEM", arr);
            }
            v.Seq = internalSeq;
            internalSeq++;
            v.AdjustmentByDetails = AdjustmentBy;
            arr.Add(v.GetDbObject());
            adjustmentItems.Add(v);

            v.ExtFlag = "A";
        }

        public void RemoveAdjustment(MInventoryAdjustment vp)
        {
            removeAssociateItems(vp, "ADJUSTMENT_ITEM", "INVENTORY_ADJUSTMENT_SEQ", "INVENTORY_ADJ_ID");
            adjustmentItems.Remove(vp);
        }

        public void AddTxItem(MInventoryTransaction tx, InventoryDocumentType idt)
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("TX_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("TX_ITEM", arr);
            }

            arr.Add(tx.GetDbObject());
            txitems.Add(tx);
        }

        public void InitErrorItem()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("ERROR_ITEM");

            if (arr == null)
            {
                return;
            }

            errorItems.Clear();
            foreach (CTable t in arr)
            {
                t.SetFieldValue("TYPE_ERROR", "InventoryDoc");
                MError v = new MError(t);
                errorItems.Add(v);
            }
        }

        public void ChangeAmountLabel()
        {
            if (DocumentType == ((int) InventoryDocumentType.InvDocAdjust).ToString())
            {
                foreach (MInventoryAdjustment ma in adjustmentItems)
                {
                    ma.AdjustmentByDetails = AdjustmentBy;
                }
            }
        }

        public void InitTxItem(InventoryDocumentType idt)
        {
            if ((idt == InventoryDocumentType.InvDocAdjust) && !IsAdjustByDelta)
            {
                adjustmentItems.Clear();

                CTable o = GetDbObject();
                ArrayList arr = o.GetChildArray("ADJUSTMENT_ITEM");

                if (arr == null)
                {
                    return;
                }

                foreach (CTable t in arr)
                {
                    MInventoryAdjustment v = new MInventoryAdjustment(t);
                    adjustmentItems.Add(v);

                    v.Seq = internalSeq;
                    internalSeq++;

                    v.ExtFlag = "I";
                    v.AdjustmentByDetails = AdjustmentBy;
                }
            }
            else
            {
                CTable o = GetDbObject();
                ArrayList arr = o.GetChildArray("TX_ITEM");

                if (arr == null)
                {
                    return;
                }

                txitems.Clear();
                foreach (CTable t in arr)
                {
                    MInventoryTransaction v = null;
                    if (idt == InventoryDocumentType.InvDocImport)
                    {
                        v = new MInventoryTransactionImport(t);
                    }
                    else
                    {
                        v = new MInventoryTransaction(t);
                    }
                    txitems.Add(v);
                    v.ExtFlag = "I";
                }
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

                return (GetDbObject().GetFieldValue("DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_ID", value);
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

        public String ReturnDueDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("RETURN_DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime ReturnDueDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("RETURN_DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("RETURN_DUE_DATE", str);
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

                return (true);
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

                InventoryDocumentStatus dt = (InventoryDocumentStatus) Int32.Parse(DocumentStatus);
                String str = CUtil.InvDocStatusToString(dt);

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

        public MLocation LocationObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MLocation m = value as MLocation;
                InventoryDocumentType dt = (InventoryDocumentType)Int32.Parse(DocumentType);
                if ((dt == InventoryDocumentType.InvDocImport) || (dt == InventoryDocumentType.InvDocAdjust))
                {
                    ToLocation = m.LocationID;
                    ToLocationName = m.Description;
                    ToLocationCode = m.LocationCode;
                }
                else if (dt == InventoryDocumentType.InvDocExport)
                {
                    FromLocation = m.LocationID;
                    FromLocationName = m.Description;
                    FromLocationCode = m.LocationCode;
                }
            }

            get
            {
                if (DocumentType.Equals(""))
                {
                    return (null);
                }

                MLocation l = new MLocation(new CTable("DUMMY"));
                InventoryDocumentType dt = (InventoryDocumentType)Int32.Parse(DocumentType);
                if ((dt == InventoryDocumentType.InvDocImport) || (dt == InventoryDocumentType.InvDocAdjust))
                {
                    l.LocationID = ToLocation;
                    l.Description = ToLocationName;
                    l.LocationCode = ToLocationCode;
                }
                else if ((dt == InventoryDocumentType.InvDocExport) || (dt == InventoryDocumentType.InvDocXfer))
                {
                    l.LocationID = FromLocation;
                    l.Description = FromLocationName;
                    l.LocationCode = FromLocationCode;
                }

                return (l);
            }
        }

        public String LocationName
        {
            get
            {
                InventoryDocumentType dt = (InventoryDocumentType)Int32.Parse(DocumentType);
                if ((dt == InventoryDocumentType.InvDocImport) || (dt == InventoryDocumentType.InvDocAdjust))
                {
                    return (ToLocationName);
                }
                else if ((dt == InventoryDocumentType.InvDocExport) || (dt == InventoryDocumentType.InvDocXfer))
                {
                    return (FromLocationName);
                }

                return ("");
            }
        }

        public MLocation FromLocationObj
        {
            set
            {
                MLocation m = value as MLocation;
                if (m != null)
                {                    
                    FromLocation = m.LocationID;
                    FromLocationName = m.Description;
                    FromLocationCode = m.LocationCode;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MLocation mr = new MLocation(new CTable(""));
                mr.LocationID = FromLocation;
                mr.Description = FromLocationName;
                mr.LocationCode = FromLocationCode;

                return (mr);
            }
        }

        public String FromLocation
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID1"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID1", value);
                NotifyPropertyChanged();
            }
        }

        public String FromLocationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_LOCATION"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_LOCATION", value);
                NotifyPropertyChanged();
            }
        }

        public String FromLocationCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FROM_LOCATION_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("FROM_LOCATION_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public MLocation ToLocationObj
        {
            set
            {
                MLocation m = value as MLocation;
                if (m != null)
                {
                    ToLocation = m.LocationID;
                    ToLocationName = m.Description;
                    ToLocationCode = m.LocationCode;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MLocation mr = new MLocation(new CTable(""));
                mr.LocationID = ToLocation;
                mr.Description = ToLocationName;
                mr.LocationCode = ToLocationCode;

                return (mr);
            }
        }

        public String ToLocation
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID2"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID2", value);
                NotifyPropertyChanged();
            }
        }

        public String ToLocationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_LOCATION"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_LOCATION", value);
                NotifyPropertyChanged();
            }
        }

        public String ToLocationCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TO_LOCATION_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TO_LOCATION_CODE", value);
                NotifyPropertyChanged();
            }
        }

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

        public String ApproveFlag
        {
            set
            {
                GetDbObject().SetFieldValue("EXT_APPROVED", value);
                NotifyPropertyChanged();
            }
        }

        public String TotalAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_NEGATIVE");
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
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("ALLOW_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        #region Cash Vat Reset Criteria
        public String AdjustmentBy
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("1");
                }

                return (GetDbObject().GetFieldValue("ADJUSTMENT_BY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADJUSTMENT_BY", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsPerUnitAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (AdjustmentBy.Equals("2"));
            }

            set
            {
                if (value)
                {
                    AdjustmentBy = "2";

                    updateFlag();
                    NotifyPropertyChanged();
                    ChangeAmountLabel();
                }
            }
        }

        public String AdjustByDeltaFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("N");
                }

                return (GetDbObject().GetFieldValue("ADJUST_BY_DELTA_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADJUST_BY_DELTA_FLAG", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsAdjustByDelta
        {
            get
            {
                return (AdjustByDeltaFlag.Equals("Y"));
            }
        }

        public Boolean IsTotalRemainedAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (AdjustmentBy.Equals("1"));
            }

            set
            {
                if (value)
                {
                    AdjustmentBy = "1";

                    updateFlag();
                    NotifyPropertyChanged();
                    ChangeAmountLabel();
                }
            }
        }
        #endregion

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public MBaseModel EmployeeObj
        {
            get
            {
                MEmployee em = new MEmployee(new CTable(""));
                em.EmployeeID = EmployeeID;
                em.EmployeeCode = EmployeeCode;
                em.EmployeeName = EmployeeName;
                em.EmployeeNameLastname = EmployeeNameLastname;

                return (em);
            }

            set
            {
                if (value == null)
                {
                    EmployeeID = "";
                    EmployeeCode = "";
                    EmployeeName = "";
                    EmployeeNameLastname = "";

                    return;
                }

                MEmployee ii = (value as MEmployee);

                EmployeeID = ii.EmployeeID;
                EmployeeCode = ii.EmployeeCode;
                EmployeeName = ii.EmployeeName;
                EmployeeNameLastname = ii.EmployeeNameLastname;

                updateFlag();
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

        public String EmployeeNameLastname
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME_LASTNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME_LASTNAME", value);
            }
        }

        public Boolean? IsInternalDoc
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("INTERNAL_DOC_FLAG");
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
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("INTERNAL_DOC_FLAG", flag);
                NotifyPropertyChanged();
            }
        }
    }
}
