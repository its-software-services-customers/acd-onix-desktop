using System;
using System.Collections;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using Onix.Client.Model;

namespace Onix.ClientCenter.Commons.Loader
{
    public class CExcelLoader : CLoaderBase
    {
        private Application excelApp = new Application();
        private Workbook workbook = null;
        private Worksheet worksheet = null;
        private Range range = null;

        private ArrayList dataRows = new ArrayList();
        private ArrayList colNames = null;

        public CExcelLoader(String fileName, String sheetName) : base(fileName, sheetName)
        {
            workbook = excelApp.Workbooks.Open(getFileName());
            worksheet = (Worksheet)workbook.Sheets[getSheetName()];
            range = worksheet.UsedRange;
        }

        public void SetExcelLoaderFormat(String modelName, ArrayList properties)
        {
            colNames = properties;
            registerModelName(modelName);
        }

        private MBaseModel createObject(Range row)
        {
            int colCnt = row.Columns.Count;
            Hashtable hash = new Hashtable();

            for (int column = 1; column <= colCnt; column++)
            {
                Range cell = row.Columns[column];
                String data = "";
                if (cell.Value2 != null)
                {
                    data = cell.Value2.ToString();
                }

                String propertyName = (String) colNames[column-1];
                hash[propertyName] = data.Trim();
            }

            MBaseModel en = createModel();
            foreach (PropertyInfo prop in en.GetType().GetProperties())
            {
                String propertyName = prop.Name;
                if (hash.ContainsKey(propertyName))
                {
                    String value = (String) hash[propertyName];
                    prop.SetValue(en, value, null);
                }
            }

            return (en);
        }

        public override void ProcessFile()
        {
            int rc = range.Rows.Count;
            for (int row = 2; row <= rc; row++)
            {
                MBaseModel en = createObject(range.Rows[row]);
                LoadProgressFunc(row, rc);

                addRow(en);
            }

            workbook.Close();
            excelApp.Quit();
        }

        public override int GetColumnCount()
        {
            return (range.Columns.Count);
        }

        public override int GetRowCount()
        {
            return (range.Rows.Count);
        }
    }
}
