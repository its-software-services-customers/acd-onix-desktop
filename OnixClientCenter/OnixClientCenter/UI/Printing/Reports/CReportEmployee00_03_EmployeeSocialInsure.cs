using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Report;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
    public class CReportEmployee00_03_EmployeeSocialInsure : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private String prevKeyId = "";
        private ArrayList entitySums = new ArrayList();
        private ArrayList sums = new ArrayList();

        public CReportEmployee00_03_EmployeeSocialInsure() : base()
        {
        }

        private void configReport()
        {
            addConfig("L1", 10, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 20, "id_card_no", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "ID_NUMBER", "S", false);
            addConfig("L1", 20, "name_prefix", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "NAME_PREFIX_DESC", "S", false);
            addConfig("L1", 30, "employee_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "EMPLOYEE_NAME_LASTNAME", "S", false);

            addConfig("L1", 15, "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL", "DD", true);
            addConfig("L1", 15, "jan", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Center, "01", "DD", true);            
            addConfig("L1", 15, "feb", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "02", "DD", true);
            addConfig("L1", 15, "mar", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "03", "DD", true);
            addConfig("L1", 15, "apr", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "04", "DD", true);
            addConfig("L1", 15, "may", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "05", "DD", true);
            addConfig("L1", 15, "jun", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "06", "DD", true);
            addConfig("L1", 15, "jul", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "07", "DD", true);
            addConfig("L1", 15, "aug", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "08", "DD", true);
            addConfig("L1", 15, "sep", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "09", "DD", true);
            addConfig("L1", 15, "oct", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "10", "DD", true);
            addConfig("L1", 15, "nov", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "11", "DD", true);
            addConfig("L1", 15, "dec", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "12", "DD", true);

        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetEmployeeSocialInsuranceMonthSummary", "EMPLOYEE_TAX_RECORDS", Parameter);
            return (arr);
        }

        #region Create Layout Report

        protected override void createRowTemplates()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            configReport();

            nm = "HEADER_LEVEL1";
            CRow h2 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h2;

            configRow("L1", h2, "H");

            //== START BODY ===

            nm = "DATA_LEVEL1";
            CRow r01 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            r01.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r01;

            configRow("L1", r01, "B");
            //== END BODY ===

            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            configRow("L1", f1, "F");

            baseTemplateName = "DATA_LEVEL1";
        }

        private void createDataHeaderRow2(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];

            r.FillColumnsText(getColumnHederTexts("L1", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        protected override UReportPage initNewArea(Size areaSize)
        {
            UReportPage page = new UReportPage();

            CreateGlobalHeaderRow(page);
            createDataHeaderRow2(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        #endregion

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevKeyId;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal1 = copyTotalArray(entitySums);
            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();
           
            double newh = AvailableSpace - nr.GetHeight();

            double h = 0.00;

            if (newh > 0)
            {
                String keyId = o.GetFieldValue("EMPLOYEE_CODE");

                if (row == 0)
                {
                    prevKeyId = keyId;
                }

                h = addNewDataRow(rowdef, rpp, "DATA_LEVEL1", "L1", row, o);
                //newh = newh - h;

                ArrayList temps = getColumnDataTexts("L1", row, o);
                nr.FillColumnsText(temps);

                sums = sumDataTexts("L1", sums, temps);
                entitySums = sumDataTexts("L1", entitySums, temps);

                if (row == rowcount - 1)
                {
                    h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", sums);
                    newh = newh - h;
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                entitySums = keepTotal1;
                sums = keepTotal2;
                prevKeyId = tmpPrevKey;
            }
            else
            {
                AvailableSpace = newh;
            }

            return (rpp);
        }

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        private void LoadEmployeeTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.EmployeeTypes);
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("year", EntryType.ENTRY_TEXT_BOX, 200, false, "TAX_YEAR");
            entries.Add(entry);

            entry = new CEntry("employee_type", EntryType.ENTRY_COMBO_BOX, 200, true, "EMPLOYEE_TYPE");
            entry.SetComboLoadAndInit(LoadEmployeeTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("has_resigned", EntryType.ENTRY_CHECK_BOX, 200, true, "RESIGNED_FLAG");
            entries.Add(entry);

            entry = new CEntry("employee_code", EntryType.ENTRY_TEXT_BOX, 200, true, "EMPLOYEE_CODE");
            entries.Add(entry);

            entry = new CEntry("employee_name", EntryType.ENTRY_TEXT_BOX, 350, true, "EMPLOYEE_NAME");
            entries.Add(entry);            
            
            return (entries);
        }
    }
}
