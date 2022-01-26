using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MAuxilaryDocRemark : MBaseModel
    {
        private int seq = 0;

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

        public MAuxilaryDocRemark(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
        }

        public String AuxilaryDocRemarkID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_REMARK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_REMARK_ID", value);
            }
        }

        public String AuxilaryDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUXILARY_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUXILARY_DOC_ID", value);
            }
        }

        public String CodeReference
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("CODE_REFERENCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CODE_REFERENCE", value);

                NotifyPropertyChanged();
                updateFlag();
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
                updateFlag();
            }
        }


    }
}

