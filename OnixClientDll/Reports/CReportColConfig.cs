using System;
using System.Windows;
using System.Collections;
using System.Windows.Media;

namespace Onix.Client.Report
{
    public class CReportColConfig
    {
        public int ColumnWidth{ get; set; }
        public String ColumnLabel { get; set; }
        public HorizontalAlignment HeaderAlighment { get; set; }
        public HorizontalAlignment BodyAlighment { get; set; }
        public HorizontalAlignment FooterAlighment { get; set; }
        public String FieldName { get; set; }
        public String DataType { get; set; }
        public Boolean IsSum { get; set; }

        public CReportColConfig(int width, String key, HorizontalAlignment hAlign, HorizontalAlignment bAligh, HorizontalAlignment fAligh, String fldName, String dt, Boolean isSum)
        {
            FooterAlighment = fAligh;
            IsSum = isSum;
            DataType = dt;
            ColumnWidth = width;
            ColumnLabel = key;
            HeaderAlighment = hAlign;
            BodyAlighment = bAligh;
            FieldName = fldName;
        }

    }
}
