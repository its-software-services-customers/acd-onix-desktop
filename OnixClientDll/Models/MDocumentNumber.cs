using System;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MDocumentNumber : MBaseModel
    {
        private ObservableCollection<MDocumentNumber> items = new ObservableCollection<MDocumentNumber>();
        private int seq = 0;

        public MDocumentNumber(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public ObservableCollection<MDocumentNumber> Items
        {
            get
            {
                return (items);
            }
        }

        public void InitItem(GenericStringTypeFilterCallback requireFunc)
        {
            items.Clear();

            CTable o = GetDbObject();
            ArrayList narr = o.GetChildArray("DOCUMENT_NUMBER_LIST");
            if (narr == null)
            {
                narr = new ArrayList();
                o.AddChildArray("DOCUMENT_NUMBER_LIST", narr);
            }

            foreach (CTable t in narr)
            {
                MDocumentNumber v = new MDocumentNumber(t);

                if (!requireFunc(v.DocType))
                {
                    continue;
                }

                items.Add(v);
                v.ExtFlag = "I";
            }
        }

        public String DocumentNumberID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NUMBER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NUMBER_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String DocType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOC_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOC_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String DocTypeName
        {
            get
            {
                return (CLanguage.getValue(DocType));
            }
        }

        public String LastRunYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LAST_RUN_YEAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("LAST_RUN_YEAR", value);
                NotifyPropertyChanged();
            }
        }

        public String LastRunMonth
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LAST_RUN_MONTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("LAST_RUN_MONTH", value);
                NotifyPropertyChanged();
            }
        }

        public String Formula
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FORMULA"));
            }

            set
            {
                GetDbObject().SetFieldValue("FORMULA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsResetMonthly
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ResetCriteria.Equals("1"));
            }

            set
            {
                if (value)
                {
                    ResetCriteria = "1";
                }
            }
        }

        public Boolean IsResetYearly
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (ResetCriteria.Equals("2"));
            }

            set
            {
                if (value)
                {
                    ResetCriteria = "2";
                }
            }
        }

        public String ResetCriteria
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RESET_CRITERIA"));
            }

            set
            {
                GetDbObject().SetFieldValue("RESET_CRITERIA", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CurrentSEQ
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CURRENT_SEQ"));
            }

            set
            {
                GetDbObject().SetFieldValue("CURRENT_SEQ", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String StartSEQ
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("START_SEQ"));
            }

            set
            {
                GetDbObject().SetFieldValue("START_SEQ", value);
                NotifyPropertyChanged();
            }
        }

        public String SEQLength
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQ_LENGTH"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQ_LENGTH", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String YearOffset
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("YEAR_OFFSET"));
            }

            set
            {
                GetDbObject().SetFieldValue("YEAR_OFFSET", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MDocumentNumber DocNoObj
        {
            set
            {
                MDocumentNumber m = value as MDocumentNumber;
                DocumentNumberID = m.DocumentNumberID;
                DocType = m.DocType;
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

        public String Prefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREFIX"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREFIX", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String SeqDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQUENCE_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEQUENCE_DEFINITION", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CustomSeqVar
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOM_SEQ_VAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOM_SEQ_VAR", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsCustomSeq
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("USE_CUSTOM_SEQ");
                if (flag.Equals(""))
                {
                    return (false);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("USE_CUSTOM_SEQ", flag);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
    }
}
