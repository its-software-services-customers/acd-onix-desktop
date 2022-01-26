using System;
using System.Windows;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Report;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
    public class CReportInventory003_03_MovementSummary : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private ArrayList sums = new ArrayList();
        private ArrayList projSums = new ArrayList();
        private String prevProject = "";

        public CReportInventory003_03_MovementSummary() : base()
        {
        }

        private void configReport()
        {
            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);

            addConfig("L1", 15, "item_code", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ITEM_CODE", "S", false);
            addConfig("L1", 30, "item_name_thai", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ITEM_NAME_THAI", "S", false);
            addConfig("L1", 20, "location_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "LOCATION_NAME", "S", false);
            addConfig("L1", 10, "begin_qty", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "BEGIN_QUANTITY", "D", false);
            addConfig("L1", 10, "begin_amt", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "BEGIN_AMOUNT_AVG", "D", true);
            addConfig("L1", 10, "in_qty", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "IN_QUANTITY", "D", false);
            addConfig("L1", 10, "in_amt", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "IN_AMOUNT_AVG", "D", true);
            addConfig("L1", 10, "out_qty", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "OUT_QUANTITY", "D", false);
            addConfig("L1", 10, "out_amt", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "OUT_AMOUNT_AVG", "D", true);
            addConfig("L1", 10, "end_qty", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_QUANTITY", "D", false);
            addConfig("L1", 10, "end_amt", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_AMOUNT_AVG", "D", true);
        }

        private void createDataHeaderRow(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];

            r.FillColumnsText(getColumnHederTexts("L1", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }        
        
        protected override ArrayList getRecordSet()
        {
            ArrayList arr = new ArrayList();

            arr = OnixWebServiceAPI.GetListAPI("GetInventoryMovementSummaryList", "SUMMARY_ITEM", Parameter);

            return (arr);
        }

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
            createDataHeaderRow(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevProject = prevProject;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal1 = copyTotalArray(projSums);
            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            if (newh > 0)
            {
                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                projSums = sumDataTexts("L1", projSums, temps);

                if (row == rowcount - 1)
                {
                    //=== End row
                    CRow ft1 = (CRow)rowdef["FOOTER_LEVEL1"];
                    CRow ftr1 = ft1.Clone();

                    ArrayList totals = displayTotalTexts("L1", sums, 1, "total");
                    ftr1.FillColumnsText(totals);

                    rpp.AddReportRow(ftr1);
                    newh = newh - ftr1.GetHeight();
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                projSums = keepTotal1;
                sums = keepTotal2;
                prevProject = tmpPrevProject;
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

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_BALANCE_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_BALANCE_DATE");
            entries.Add(entry);

            entry = new CEntry("item_code", EntryType.ENTRY_TEXT_BOX, 150, true, "ITEM_CODE");
            entries.Add(entry);

            entry = new CEntry("item_name_thai", EntryType.ENTRY_TEXT_BOX, 300, true, "ITEM_NAME_THAI");
            entries.Add(entry);

            entry = new CEntry("location_name", EntryType.ENTRY_TEXT_BOX, 300, true, "LOCATION_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
