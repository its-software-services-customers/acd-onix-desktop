using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Report;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
    public class CReportInvoice005_PoTransaction : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();


        private double[] totals = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private CTable extParam = null;

        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        private String prevKey = "";

        public CReportInvoice005_PoTransaction() : base()
        {
        }

        private void configReport()
        {
            addConfig("L0", 16, "docno", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_NO", "S", false);
            addConfig("L0", 13, "DocuDate", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L0", 14, "invoice_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "REF_BY_DOC_NO", "S", false);
            addConfig("L0", 18, "invoice_date", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "REF_BY_DOC_DATE", "DT", false);
            addConfig("L0", 18, "supplier_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_NAME", "S", false);
            addConfig("L0", 18, "project_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_NAME", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 11, "item_code", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DISPLAY_CODE", "S", false);
            addConfig("L1", 27, "item_name_thai", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DISPLAY_NAME", "S", false);
            addConfig("L1", 9, "quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "QUANTITY", "D", false);
            addConfig("L1", 9, "unit_price", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "UNIT_PRICE", "D", false);
            addConfig("L1", 9, "total_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL_AMT", "D", true);
            addConfig("L1", 9, "discount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "DISCOUNT_AMT", "D", true);
            addConfig("L1", 9, "total_amount_afterDiscount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL_AFTER_DISCOUNT", "D", true);
            addConfig("L1", 9, "vat_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "VAT_TAX_AMT", "D", true);
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

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetPurchasePoTxList", "SALE_PO_TX_LIST", Parameter);
            return (arr);
        }

        private void manipulateRow(CTable o)
        {
            string code = "";
            string name = "";
            string typeName = "";
            string selectType = o.GetFieldValue("SELECTION_TYPE");

            if (selectType.Equals("2"))
            {
                code = o.GetFieldValue("ITEM_CODE");
                name = o.GetFieldValue("ITEM_NAME_THAI");
                typeName = CLanguage.getValue("item");
            }
            else if (selectType.Equals("1"))
            {
                code = o.GetFieldValue("SERVICE_CODE");
                name = o.GetFieldValue("SERVICE_NAME");
                typeName = CLanguage.getValue("service");
            }
            else
            {
                code = CLanguage.getValue("free_text");
                name = o.GetFieldValue("FREE_TEXT");
                typeName = code;
            }

            o.SetFieldValue("SELECTION_TYPE_NAME", typeName);
            o.SetFieldValue("DISPLAY_CODE", code);
            o.SetFieldValue("DISPLAY_NAME", name);
        }

        private String getGroupKey(CTable o)
        {
            return o.GetFieldValue("DOCUMENT_NO");
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

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", groupSums);
                    newh = newh - h;

                    h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", sums);
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

        private void LoadCombo(ComboBox cbo, String id)
        {
            CUtil.LoadInventoryDocStatus(cbo, true, id);
        }

        private void LoadDocTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchaseExpenseDocTypes);
        }

        private void LoadPaymentTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.AccountSalePayTypes);
        }

        private void LoadRefundStatusCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.RefundStatus);
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

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("project_code", EntryType.ENTRY_TEXT_BOX, 200, true, "PROJECT_CODE");
            entries.Add(entry);

            entry = new CEntry("supplier_code", EntryType.ENTRY_TEXT_BOX, 200, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry("supplier_name", EntryType.ENTRY_TEXT_BOX, 350, true, "ENTITY_NAME");
            entries.Add(entry);

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("document_type", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_TYPE");
            entry.SetComboLoadAndInit(LoadDocTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);


            return (entries);
        }
    }
}
