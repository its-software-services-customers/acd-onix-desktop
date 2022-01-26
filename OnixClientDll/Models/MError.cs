using System;
using Onix.Client.Helper;
using Onix.Client.Model;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MError : MBaseModel
    {
        public MError(CTable obj) : base(obj)
        {

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
                NotifyPropertyChanged();
            }
        }

        public String Description
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESC"));
            }
            set
            {
                GetDbObject().SetFieldValue("DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String Seq
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEQ"));
            }
            set
            {
                GetDbObject().SetFieldValue("SEQ", value);
                NotifyPropertyChanged();
            }
        }

        public String ErrorNormalizeDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String errorDesc = ErrorDesc;
                string[] stringSeparators = new string[] { "|" };
                String normalizeDesc = "";

                String[] result = errorDesc.Split(stringSeparators, StringSplitOptions.None);
                String errcd = result[0];

                if (errcd.Equals("ERROR_INVALID_DATE"))
                {
                    //ERROR_INVALID_DATE ข้อมูลวันที่ของเอกสาร[{ 0}] เกินจากวันที่[{ 1}]	

                    String docNo = result[1];
                    String docDate = CUtil.DateTimeToDateString(CUtil.InternalDateToDate(result[2]));
                    String gap = result[3];
                    String cmpDate = CUtil.DateTimeToDateString(CUtil.InternalDateToDate(result[4]));

                    normalizeDesc = String.Format(CLanguage.getValue("ERROR_INVALID_DATE"), docNo, cmpDate);
                }
                else if (errcd.Equals("ERROR_DOCUMENT_DATE"))
                {
                    //ERROR_DOCUMENT_DATE|H003|BREAD-001|2017/11/11 00:00:00

                    String locNo = result[1];
                    String itemNo = result[2];
                    String cmpDate = CUtil.DateTimeToDateString(CUtil.InternalDateToDate(result[3]));

                    normalizeDesc = String.Format(CLanguage.getValue("ERROR_DOCUMENT_DATE"), locNo, itemNo, cmpDate);
                }
                else if (errcd.Equals("ERROR_BALANCE"))
                {
                    //ERROR_BALANCE|H003|-14|BREAD-001|2017/11/14 00:00:00

                    String locNo = result[1];
                    String itemNo = result[3];
                    String qty = result[2];

                    normalizeDesc = String.Format(CLanguage.getValue("ERROR_BALANCE"), locNo, itemNo, qty);
                }

                return (normalizeDesc);
            }
        }
    }
}
