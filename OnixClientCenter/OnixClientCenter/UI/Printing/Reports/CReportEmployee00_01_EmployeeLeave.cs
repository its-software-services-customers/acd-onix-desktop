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
    public class CReportEmployee00_01_EmployeeLeave : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private String prevKeyId = "";
        private ArrayList entitySums = new ArrayList();
        private ArrayList sums = new ArrayList();

        private string[] empReceives = { "RECEIVE_INCOME", "RECEIVE_OT", "RECEIVE_POSITION", "RECEIVE_BONUS", "RECEIVE_TRANSPORTATION", "RECEIVE_TELEPHONE", "RECEIVE_ALLOWANCE", "RECEIVE_COMMISSION" };
        private string[] empDeducts = { "DEDUCT_TAX", "DEDUCT_SOCIAL_SECURITY", "DEDUCT_PENALTY" };

        public CReportEmployee00_01_EmployeeLeave() : base()
        {
        }

        private void configReport()
        {
            addConfig("L1", 10, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 20, "code", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "EMPLOYEE_CODE", "S", false);
            addConfig("L1", 30, "employee_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "EMPLOYEE_NAME_LASTNAME", "S", false);

            addConfig("L1", 20, "hiring_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "HIRING_DATE", "DT", false);
            addConfig("L1", 0, "hiring_duration", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "HIRING_DURATION", "S", false);
            
            addConfig("L1", 15, "sick_leave", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "SICK_LEAVE", "DD", true);
            addConfig("L1", 15, "personal_leave", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "PERSONAL_LEAVE", "DD", true);
            addConfig("L1", 20, "extra_personal_leave", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "EXTRA_LEAVE", "DD", true);
            addConfig("L1", 20, "annual_leave", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "ANNUAL_LEAVE", "DD", true);
            addConfig("L1", 20, "late_min", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "LATE", "DD", true);
            addConfig("L1", 25, "deduction_leave_hr", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "DEDUCTION_LEAVE", "DD", true);
            addConfig("L1", 25, "abnormal_leave_hr", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "ABNORMAL_LEAVE", "DD", true);
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetEmployeeLeaveSummary", "EMPLOYEE_LEAVE_RECORDS", Parameter);
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

        private double getSumArray(CTable o, string[] arr)
        {
            double sum = 0.00;

            foreach (String field in arr)
            {
                String tmp = o.GetFieldValue(field);
                double amt = CUtil.StringToDouble(tmp);

                sum = sum + amt;
            }

            return (sum);
        }

        private void populateTempFields(CTable o)
        {
            String name = o.GetFieldValue("EMPLOYEE_NAME");
            String lastName = o.GetFieldValue("EMPLOYEE_LASTNAME");
            double refund = CUtil.StringToDouble(o.GetFieldValue("RECEIVE_REFUND"));
            double coverage = CUtil.StringToDouble(o.GetFieldValue("DEDUCT_COVERAGE"));

            o.SetFieldValue("EMPLOYEE_NAME_LASTNAME", name + " " + lastName);
            double received = getSumArray(o, empReceives);
            double deduct = getSumArray(o, empDeducts);
            double total = received + refund - deduct - coverage;

            o.SetFieldValue("EMP_DEDUCT_TOTAL", deduct.ToString());
            o.SetFieldValue("EMP_RECEIVED_TOTAL", received.ToString());
            o.SetFieldValue("EMP_AMOUNT_TOTAL", total.ToString());
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
            populateTempFields(o);

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

        private void LoadMonthCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.Months);
        }

        private void LoadEmployeeTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.EmployeeTypes);
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("year", EntryType.ENTRY_TEXT_BOX, 200, false, "LEAVE_YEAR");
            entries.Add(entry);

            entry = new CEntry("employee_type", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
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
