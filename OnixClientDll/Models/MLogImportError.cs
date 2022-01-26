using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MLogImportError : MBaseModel
    {
        private int seq = 0;

        public MLogImportError(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public String LogImportErrorID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOG_IMPORT_ERROR_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOG_IMPORT_ERROR_ID", value);
            }
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ErrorDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ERROR_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("ERROR_DESC", value);
                updateFlag();
                NotifyPropertyChanged();
            }
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
    }


}
