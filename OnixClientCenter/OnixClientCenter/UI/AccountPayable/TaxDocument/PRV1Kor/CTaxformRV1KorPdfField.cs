using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument.PRV1Kor
{
    class CTaxformRV1KorPdfField
    {
        public CTaxformRV1KorPdfField(int group, int row, String pdfFieldName, String logicalName)
        {
            Group = group;
            Row = row;
            LogicalName = logicalName;
            PdfFieldName = pdfFieldName;
        }

        public int Group
        {
            get; set;
        }

        public int Row
        {
            get; set;
        }

        public String LogicalName
        {
            get; set;
        }

        public String PdfFieldName
        {
            get; set;
        }

        public String Key
        {
            get
            {
                String key = ConstructKey(Group, Row, LogicalName);
                return (key);
            }
        }

        public static String ConstructKey(int group, int row, String logicalName)
        {
            String key = String.Format("{0}-{1}-{2}", group, row, logicalName);
            return (key);
        }
    }
}
