using System;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MLogImportIssue : MBaseModel
    {
        private ObservableCollection<MLogImportError> details = new ObservableCollection<MLogImportError>();
        private int internalSeq = 0;

        public MLogImportIssue(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("inventory_doc_no", DocumentNo);
            ttItems.Add(ct);

            ct = new CToolTipItem("inventory_doc_date", DocumentDateFmt);
            ttItems.Add(ct);

            ct = new CToolTipItem("branch_name", BranchName);
            ttItems.Add(ct);

        }

        public ObservableCollection<MLogImportError> BranchPOSError
        {
            get
            {
                return (details);
            }

            set
            {
            }
        }

        public void CreateDefaultValue()
        {
        }

        public String LogImportIssueID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOG_IMPORT_ISSUE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOG_IMPORT_ISSUE_ID", value);
            }
        }

        public DateTime ImportDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("IMPORT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("IMPORT_DATE", str);
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

        public String RefID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REF_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("REF_ID", value);
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

        public String DocumentDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String ImportBy
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMPORT_BY"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMPORT_BY", value);
                NotifyPropertyChanged();
            }
        }

        #region Branch
        public String BranchID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
            }
        }

        public String BranchCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BranchObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                BranchID = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                if (branches == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(branches, BranchID));
            }
        }

        #endregion Branch

        #region Status
        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
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

                return (GetDbObject().GetFieldValue("STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("STATUS", value);
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

                String str = CUtil.PosImportStatusToString(DocumentStatus);

                return (str);
            }

            set
            {

            }
        }
        #endregion

        #region Error 
        public void InitBranchPOSError()
        {
            details.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("LOG_IMPORT_ERROR_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("LOG_IMPORT_ERROR_ITEM", arr);
            }

            foreach (CTable t in arr)
            {
                MError e = new MError(t);
                MLogImportError v = new MLogImportError(t);
                details.Add(v);


                if (!e.ErrorNormalizeDesc.Equals(""))
                {
                    v.ErrorDesc = e.ErrorNormalizeDesc;
                }

                v.ExtFlag = "I";

                v.Seq = internalSeq;
                internalSeq++;
            }
        }

#endregion
    }
}
