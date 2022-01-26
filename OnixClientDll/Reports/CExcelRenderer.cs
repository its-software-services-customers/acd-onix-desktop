using System;
using System.Collections;
using Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Runtime.InteropServices;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Drawing;

namespace Onix.Client.Report
{
    public class CMergeCellParam
    {
        public Boolean IsMerged = false;
        public int FromCell = 0;
        public int ToCell = 0;
        public int CellMergedCount = 0;

        public CMergeCellParam(int start, int end, int len)
        {
            FromCell = start;
            ToCell = end;
            CellMergedCount = len;

            if (len > 1)
            {
                IsMerged = true;
            }
        }
    }

    public class CExcelRenderer
    {
        private Hashtable rowTemplate = new Hashtable();
        private Hashtable rowMergedParam = new Hashtable();
        private int maxColumn = 0;
        private String rptName = "";
        private Application xlApp = null;
        private Workbook xlWorkBook;
        private Worksheet xlWorkSheet;
        private object misValue = Missing.Value;
        private int currentRow = 0;
        private int currentPage = 0;
        private CTable cacheObj = null;
        private ArrayList cacheRows = null;

        public CExcelRenderer(String reportName)
        {
            cacheRows = new ArrayList();
            cacheObj = new CTable(reportName);
            cacheObj.AddChildArray("ROWS", cacheRows);

            rptName = reportName;
        }

        public void Init()
        {
            xlApp = new Application();

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);
        }

        public void AddRowTemplate(CRow row)
        {
            String nm = row.GetName();
            rowTemplate[nm] = row;

            int colCount = row.GetColumnCount();
            if (colCount > maxColumn)
            {
                maxColumn = colCount;
            }

            cacheObj.SetFieldValue("MAX_COLUMN", maxColumn.ToString());
        }

        public void CalculateMergeCellRange(String baseTemplate)
        {
            CRow row = (CRow) rowTemplate[baseTemplate];

            foreach (String key in rowTemplate.Keys)
            {
                CRow r = (CRow) rowTemplate[key];
                ArrayList mergedParams = createCellMergeParam(row, r);

                rowMergedParam.Add(key, mergedParams);
            }
        }

        public void AddRow(CRow row)
        {
            String name = row.GetName();

            if (name.Contains("GLB_HEADER_LEVEL0"))
            {
                currentPage++;
            }

            if ((currentPage > 1) && name.Contains("HEADER"))
            {
                //Skip header if page greater than 1
                return;
            }

            currentRow++;

            ArrayList arr = (ArrayList) rowMergedParam[name];
            if (arr == null)
            {
                return;
            }

            int cnt = row.GetColumnCount();
            String rowText = String.Format("{0}", name);
            
            CTable cacheRow = new CTable("ROW");
            cacheRow.SetFieldValue("ROW_ID", currentRow.ToString());
            cacheRow.SetFieldValue("ROW_TYPE", row.GetName());
            cacheRow.AddChildArray("COLUMNS", new ArrayList());
            cacheRows.Add(cacheRow);

            for (int i = 0; i < cnt; i++)
            {                
                CMergeCellParam mergedParam = (CMergeCellParam) arr[i];
                CColumn col = row.GetColumn(i);
                String text = col.GetText().Text;

                populateCacheRow(cacheRow, mergedParam, row, col);
            }

            String dummy = rowText;
        }

        public void Render(ReportProgressUpdate updateFunc, ReportStatusUpdate doneFunc)
        {
            ArrayList rows = cacheObj.GetChildArray("ROWS");
            int rowCnt = rows.Count;

            int r = 0;
            foreach (CTable row in rows)
            {
                r++;
                String rowType = row.GetFieldValue("ROW_TYPE");
                ArrayList columns = row.GetChildArray("COLUMNS");

                updateFunc(r, rowCnt);                

                foreach (CTable column in columns)
                {
                    int fromCell = CUtil.StringToInt( column.GetFieldValue("FROM_CELL"));
                    int toCell = CUtil.StringToInt(column.GetFieldValue("TO_CELL"));
                    Boolean isMerged = column.GetFieldValue("IS_MERGED").Equals("True");

                    Range r1 = xlWorkSheet.Cells[r, fromCell];
                    Range rowRange = null;

                    if (isMerged)
                    {
                        Range r2 = xlWorkSheet.Cells[r, toCell];
                        Range newRange = xlWorkSheet.Range[r1, r2];
                        newRange.Merge();

                        rowRange = newRange;
                        applyCellStyle(column, newRange);
                    }
                    else
                    {
                        rowRange = r1;
                        applyCellStyle(column, r1);
                    }

                    if (rowType.Contains("HEADER"))
                    {
                        rowRange.Interior.Color = ColorTranslator.ToOle(Color.Orange);
                    }
                    else if (rowType.Contains("FOOTER"))
                    {
                        rowRange.Interior.Color = ColorTranslator.ToOle(Color.Tan);
                    }
                }
            }

            doneFunc(true, false);
        }

        public void SaveFile()
        {
            xlWorkSheet.Columns.AutoFit();
            xlWorkSheet.UsedRange.Borders.LineStyle = XlLineStyle.xlContinuous;

            xlApp.Visible = true;

            //String fname = String.Format(@"D:\{0}.xls", rptName);
            //xlWorkBook.SaveAs(fname, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
            //xlWorkBook.Close(true, misValue, misValue);
            //xlApp.Quit();
        }

        public void CleanUp()
        {
            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
        }

        #region Private

        private void populateCacheRow(CTable cacheRow, CMergeCellParam mergedParam, CRow row, CColumn col)
        {
            String text = col.GetText().Text;
            ArrayList columns = cacheRow.GetChildArray("COLUMNS");

            CTable cacheColumn = new CTable("");
            columns.Add(cacheColumn);

            int fromCell = mergedParam.FromCell + 1;
            int toCell = mergedParam.ToCell + 1;
            int span = mergedParam.CellMergedCount;

            cacheColumn.SetFieldValue("TEXT", text);
            cacheColumn.SetFieldValue("FROM_CELL", fromCell.ToString());
            cacheColumn.SetFieldValue("TO_CELL", toCell.ToString());
            cacheColumn.SetFieldValue("CELL_SPAN", span.ToString());

            Boolean isBold = row.GetFontWeight().Equals(System.Windows.FontWeights.Bold);
            String dt = col.getDataType();
            System.Windows.HorizontalAlignment align = col.GetHorizontalAlignment();

            cacheColumn.SetFieldValue("IS_BOLD", isBold.ToString());
            cacheColumn.SetFieldValue("TYPE", dt);
            cacheColumn.SetFieldValue("TEXT_ALIGN", align.ToString());
            cacheColumn.SetFieldValue("IS_MERGED", mergedParam.IsMerged.ToString());
        }

        private void applyCellStyle(CTable col, Range r1)
        {
            String text = col.GetFieldValue("TEXT");
            String dt = col.GetFieldValue("TYPE");
            String bold = col.GetFieldValue("IS_BOLD");
            Boolean isBold = bold.Equals("True");
            String align = col.GetFieldValue("TEXT_ALIGN");

            r1.HorizontalAlignment = getHorizontalAlignment(align);
            r1.Font.Bold = isBold;

            r1.Font.Name = "Angsana New";
            r1.Font.Size = 16;
            r1.NumberFormat = getNumberFormat(dt);

            if (dt.Equals("D"))
            {
                r1.Value = text.Replace(",", "");
            }
            else
            {
                r1.Value = text;
            }
        }

        private String getNumberFormat(String dt)
        {
            if (dt.Equals("DT"))
            {
                return ("@");
            }
            else if (dt.Equals("D"))
            {
                return ("###,###,###.00");
            }

            return ("@");
        }

        private XlHAlign getHorizontalAlignment(String align)
        {
            if (align.Equals("Center"))
            {
                return (XlHAlign.xlHAlignCenter);
            }
            else if (align.Equals("Right"))
            {
                return (XlHAlign.xlHAlignRight);
            }

            return (XlHAlign.xlHAlignLeft);
        }

        private Tuple<int, int, int> getStartEndLen(CColumn col, CRow baseRow, int start)
        {
            System.Windows.GridLength width = col.GetWidth();
            double w = width.Value;

            int last = baseRow.GetColumnCount() - 1;
            double sum = 0.00;

            int cnt = 0;
            int i;

            for (i = start; i < last; i++)
            {
                double colWidth = baseRow.GetColumn(i).GetWidth().Value;
                sum = sum + colWidth;

                cnt++;
                if (sum >= w)
                {
                    break;
                }
            }

            return (Tuple.Create(start, i, cnt));
        }

        private ArrayList createCellMergeParam(CRow baseRow, CRow row)
        {
            ArrayList arr = new ArrayList();

            int colCnt = row.GetColumnCount();
            int end = -1;

            for (int i = 0; i < colCnt; i++)
            {
                CColumn col = row.GetColumn(i);
                Tuple<int, int, int> t = getStartEndLen(col, baseRow, end+1);

                int start = t.Item1;
                int length = t.Item3;
                end = t.Item2;

                CMergeCellParam cell = new CMergeCellParam(start, end, length);
                arr.Add(cell);
            }

            return (arr);
        }

        #endregion
    }
}
