using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MVoidDoc : MBaseModel
    {
        public MVoidDoc(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public String VoidedDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOIDED_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOIDED_DOC_ID", value);
            }
        }

        public String AccountDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_DOC_ID", value);
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

#region Document Date
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

#endregion

        #region Cancel Reason

        public String VoidReason
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CANCEL_REASON"));
            }

            set
            {
                GetDbObject().SetFieldValue("CANCEL_REASON", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String VoidReasonName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOID_REASON_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOID_REASON_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MMasterRef VoidReasonObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    VoidReason = m.MasterID;
                    VoidReasonName = m.Description;
                    NotifyAllPropertiesChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = VoidReason;
                mr.Description = VoidReasonName;

                return (mr);
            }
        }

        #endregion

        public Boolean? AllowARAPNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_AR_AP_NEGATIVE");
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

                GetDbObject().SetFieldValue("ALLOW_AR_AP_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowCashNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_CASH_NEGATIVE");
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

                GetDbObject().SetFieldValue("ALLOW_CASH_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }

        public Boolean? AllowInventoryNegative
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("ALLOW_INVENTORY_NEGATIVE");
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

                GetDbObject().SetFieldValue("ALLOW_INVENTORY_NEGATIVE", flag);
                NotifyPropertyChanged();
            }
        }
    }
}
