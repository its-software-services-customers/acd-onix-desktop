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
    public class CReportProfit001_02_ProfitByProject : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private CTable extParam = null;
        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        private String prevGroupId = "";
        private ArrayList groupSum1 = null;

        public CReportProfit001_02_ProfitByProject() : base()
        {
        }

        private void configReport()
        {
            String rptType = Parameter.GetFieldValue("REPORT_TYPE");

            String yyyy = Parameter.GetFieldValue("DOCUMENT_YYYY");
            String m = Parameter.GetFieldValue("DOCUMENT_MM");
            String month = CUtil.IDToMonth(CUtil.StringToInt(m));
            String tmp = String.Format("{0} {1}", month, yyyy);

            addConfig("L0", 20, "", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L0", 156, tmp, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "PROJECT_CODE", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            if (rptType.Equals("1"))
            {
                addConfig("L1", 15, "project_code", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_CODE", "S", false);
            }
            else if (rptType.Equals("2"))
            {
                addConfig("L1", 15, "project_group", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_GROUP_CODE", "S", false);
            }
            addConfig("L1", 12, "doc_1", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "01", "D", true);
            addConfig("L1", 12, "doc_2", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "02", "D", true);
            addConfig("L1", 12, "doc_11", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "11", "D", true);
            addConfig("L1", 12, "doc_3", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "CR03", "D", true);
            addConfig("L1", 12, "doc_4", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "04", "D", true);
            addConfig("L1", 12, "total_receive", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL_RECEIVE", "D", true);
            addConfig("L1", 12, "doc_5", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "05", "D", true);
            addConfig("L1", 12, "doc_6", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "06", "D", true);
            addConfig("L1", 12, "doc_12", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "12", "D", true);
            addConfig("L1", 12, "doc_7", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "CR07", "D", true);
            addConfig("L1", 12, "doc_8", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "08", "D", true);
            addConfig("L1", 12, "total_expense", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL_EXPENSE", "D", true);
            addConfig("L1", 12, "gain_loss", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL", "D", true);
        }

        private void createDataHeaderRow1(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL0"];

            r.FillColumnsText(getColumnHederTexts("L0", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        private void createDataHeaderRow2(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];

            r.FillColumnsText(getColumnHederTexts("L1", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }        

        private void filterDrCrAmount(CTable o)
        {
            double r1 = CUtil.StringToDouble(o.GetFieldValue("01"));
            double r2 = CUtil.StringToDouble(o.GetFieldValue("02"));
            double r3 = CUtil.StringToDouble(o.GetFieldValue("11"));
            double r4 = CUtil.StringToDouble(o.GetFieldValue("03"));
            double r4cr = -1 * r4;
            double r5 = CUtil.StringToDouble(o.GetFieldValue("04"));
            double totalReceipt = r1 + r2 + r3 + r4cr + r5;

            double e1 = CUtil.StringToDouble(o.GetFieldValue("05"));
            double e2 = CUtil.StringToDouble(o.GetFieldValue("06"));
            double e3 = CUtil.StringToDouble(o.GetFieldValue("12"));
            double e4 = CUtil.StringToDouble(o.GetFieldValue("07"));
            double e4cr = -1 * e4;
            double e5 = CUtil.StringToDouble(o.GetFieldValue("08"));
            double totalExpense = e1 + e2 + e3 + e4cr + e5;

            double profit = totalReceipt - totalExpense;

            o.SetFieldValue("CR03", r4cr.ToString());
            o.SetFieldValue("CR07", e4cr.ToString());
            o.SetFieldValue("TOTAL_RECEIVE", totalReceipt.ToString());
            o.SetFieldValue("TOTAL_EXPENSE", totalExpense.ToString());
            o.SetFieldValue("TOTAL", profit.ToString());            
        }

        private String getGroupKey()
        {
            return ("GROUP");
        }

        protected override ArrayList getRecordSet()
        {
            String rptType = Parameter.GetFieldValue("REPORT_TYPE");
            ArrayList arr = new ArrayList();

            if (rptType.Equals("1"))
            {
                arr = OnixWebServiceAPI.GetProfitByDocTypeProject(Parameter);
            }
            else if(rptType.Equals("2"))
            {
                arr = OnixWebServiceAPI.GetProfitByDocTypeProjectGroup(Parameter);
            }

            return (arr);
        }

        protected override void createRowTemplates()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            configReport();

            nm = "HEADER_LEVEL0";
            CRow h0 = new CRow(nm, 30, getColumnCount("L0"), defMargin);
            h0.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h0;

            configRow("L0", h0, "H");

            nm = "HEADER_LEVEL1";
            CRow h2 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h2;

            configRow("L1", h2, "H");


            nm = "DATA_LEVEL1";
            CRow r0 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r0;

            configRow("L1", r0, "B");


            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            configRow("L1", f1, "F");

            baseTemplateName = "DATA_LEVEL1";
        }

        protected override UReportPage initNewArea(Size areaSize)
        {
            UReportPage page = new UReportPage();

            CreateGlobalHeaderRow(page);
            createDataHeaderRow1(page);
            createDataHeaderRow2(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        private String getTotalKeyword(String group)
        {
            if (group.Equals("1"))
            {
                return ("total_receive");
            }

            return ("total_expense");
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevGroupId;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            //ArrayList keepTotal1 = copyTotalArray(groupSums);
            ArrayList keepTotal2 = copyTotalArray(sums);


            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            filterDrCrAmount(o);

            if (newh > 0)
            {
                String groupId = o.GetFieldValue(getGroupKey());
                if (row == 0)
                {
                    prevGroupId = groupId;
                }

                if (!groupId.Equals(prevGroupId))
                {
                    prevGroupId = groupId;
                    groupSum1 = groupSums;
                }

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", sums);
                    newh = newh - h;
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                //groupSums = keepTotal1;
                sums = keepTotal2;
                prevGroupId = tmpPrevKey;
            }
            else
            {
                AvailableSpace = newh;
            }

            return (rpp);
        }

        public override Boolean IsNewVersion()
        {
            return (true);
        }        

        private void LoadCombo(ComboBox cbo, String id)
        {
            CUtil.LoadInventoryDocStatus(cbo, true, id);
        }

        private void LoadMonth(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.Months);
        }

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        public override ArrayList GetReportInputEntries()
        {
            extParam = GetExtraParam();

            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("formatDoc_MONTH", EntryType.ENTRY_COMBO_BOX, 200, false, "DOCUMENT_MM");
            entry.SetComboLoadAndInit(LoadMonth, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("year_century", EntryType.ENTRY_TEXT_BOX, 100, false, "DOCUMENT_YYYY");
            entries.Add(entry);

            return (entries);
        }
    }
}
