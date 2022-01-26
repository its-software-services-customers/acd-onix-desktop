using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.IO;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
	public class CReportInv002_01_InventoryMovementSum : CBaseReport
	{
        private Hashtable rowdef = new Hashtable();

        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        private String prevKey = "";

        public CReportInv002_01_InventoryMovementSum() : base()
        {
        }

        private void configReport()
        {
            addConfig("L0", 18, "item_code", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ITEM_CODE", "S", false);
            addConfig("L0", 55, "item_name_thai", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ITEM_NAME_THAI", "S", false);
            addConfig("L0", 27, "location_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "LOCATION_NAME", "S", false);

            addConfig("L1", 7, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 11, "DocuDate", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L1", 19, "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_NO", "S", false);
            addConfig("L1", 9, "in_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TX_QTY_AVG_IN", "DE", true);
            addConfig("L1", 9, "in_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TX_AMT_AVG_IN", "DE", true);
            addConfig("L1", 9, "out_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TX_QTY_AVG_OUT", "DE", true);
            addConfig("L1", 9, "out_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TX_AMT_AVG_OUT", "DE", true);
            addConfig("L1", 9, "balance_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_QTY_AVG", "D", true);
            addConfig("L1", 9, "balance_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_AMOUNT_AVG", "D", true);
            addConfig("L1", 9, "lot_avg", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_UNIT_PRICE", "D", true);
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




            nm = "DATA_LEVEL0";
            CRow r0 = new CRow(nm, 30, getColumnCount("L0"), defMargin);
            r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = r0;

            configRow("L0", r0, "B");

            nm = "DATA_LEVEL1";
            CRow r1 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            r1.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r1;

            configRow("L1", r1, "B");


            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            configRow("L1", f1, "F");

            baseTemplateName = "DATA_LEVEL1";
        }

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetInventoryItemMovementList", "INVENTORY_MOVEMENT_LIST", Parameter);
            return (arr);
        }

        private String getGroupKey(CTable o)
        {
            return o.GetFieldValue("ITEM_ID") + "-" + o.GetFieldValue("ITEM_ID"); ;
        }

        private void OverrideSumFields(ArrayList groupSums, CTable o)
        {
            double qty = CUtil.StringToDouble(o.GetFieldValue("END_QTY_AVG"));
            double amt = CUtil.StringToDouble(o.GetFieldValue("END_AMOUNT_AVG"));

            double price = 0.00;
            if (qty != 0.00)
            { 
                price = amt / qty;
            }

            groupSums[7] = qty;
            groupSums[8] = amt;
            groupSums[9] = price;
        }

        private void manipulateRow(CTable o)
        {
            string tx = o.GetFieldValue("DIRECTION");
            if (tx.Equals("B"))
            {
                o.SetFieldValue("DOCUMENT_NO", "ยกมา");
            }
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevKey;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal1 = copyTotalArray(groupSums);
            ArrayList keepTotal2 = copyTotalArray(sums);


            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            manipulateRow(o);

            if (newh > 0)
            {
                String groupKey = getGroupKey(o);
                if (row == 0)
                {
                    prevKey = groupKey;
                }

                if (!groupKey.Equals(prevKey) || (row == 0))
                {
                    if (row != 0)
                    {
                        CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
                        CRow ftr = ft.Clone();

                        ArrayList projTotals = displayTotalTexts("L1", groupSums, 1, "total");
                        ftr.FillColumnsText(projTotals);

                        rpp.AddReportRow(ftr);
                        newh = newh - ftr.GetHeight();

                        groupSums = new ArrayList();
                    }

                    prevKey = groupKey;

                    CRow dt0 = (CRow)rowdef["DATA_LEVEL0"];
                    CRow dtr0 = dt0.Clone();
                    ArrayList tempRows0 = getColumnDataTexts("L0", 0, o);
                    dtr0.FillColumnsText(tempRows0);                    

                    rpp.AddReportRow(dtr0);
                    newh = newh - dtr0.GetHeight();
                }

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                groupSums = sumDataTexts("L1", groupSums, temps);
                OverrideSumFields(groupSums, o);

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", groupSums);
                    newh = newh - h;
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                groupSums = keepTotal1;
                sums = keepTotal2;
                prevKey = tmpPrevKey;
            }
            else
            {
                AvailableSpace = newh;
            }

            return (rpp);
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DOCUMENT_DATE");
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

