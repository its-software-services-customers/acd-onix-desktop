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
    public class CReportInvoice001_03_SalePurchase : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private String prevEntityId = "";
        private ArrayList entitySums = new ArrayList();
        private ArrayList sums = new ArrayList();

        public CReportInvoice001_03_SalePurchase() : base()
        {
        }

        private void configReport()
        {
            String category = Parameter.GetFieldValue("CATEGORY");

            String code = "customer_code";
            String name = "customer_name";

            if (category.Equals("2"))
            {
                code = "supplier_code";
                name = "supplier_name";
            }

            addConfig("L0", 25, code, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_CODE", "S", false);
            addConfig("L0", 75, name, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_NAME", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 20, "document_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_NO", "S", false);
            addConfig("L1", 12, "DocuDate", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L1", 12, "due_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DUE_DATE", "DT", false);
            addConfig("L1", 16, "document_type", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_TYPE_NAME", "S", false);
            addConfig("L1", 20, "salesman", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "EMPLOYEE_CODE", "S", false);
            addConfig("L1", 15, "debt_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "AR_AP_AMT_TEMP", "D", true);
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetArApInvoiceList(Parameter);
            return (arr);
        }

        #region Create Layout Report

        protected override void createRowTemplates()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            configReport();

            //== START HEADER ===
            nm = "HEADER_LEVEL0";
            CRow h0 = new CRow(nm, 30, getColumnCount("L0"), defMargin);
            h0.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h0;

            configRow("L0", h0, "H");

            nm = "HEADER_LEVEL1";
            CRow h1 = new CRow(nm, 30, getColumnCount("L1"), defMargin);
            h1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h1;

            configRow("L1", h1, "H");
            //== END HEADER ===


            //== START BODY ===
            nm = "DATA_LEVEL0";
            CRow r00 = new CRow(nm, 30, getColumnCount("L0"), defMargin);
            r00.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = r00;

            configRow("L0", r00, "B");

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

        #endregion

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        private String getGroupKey()
        {
            return ("ENTITY_ID");
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevEntityId;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal1 = copyTotalArray(entitySums);
            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();
           
            String dt = o.GetFieldValue("DOCUMENT_TYPE");

            double amt = CUtil.StringToDouble(o.GetFieldValue("AR_AP_AMT"));
            o.SetFieldValue("AR_AP_AMT_TEMP", amt.ToString());
            if (dt.Equals("3") || dt.Equals("7"))
            {
                //Apply Abs() to prevent sign toggling if start new page
                amt = (-1) * amt;
                o.SetFieldValue("AR_AP_AMT_TEMP", amt.ToString());
            }
            o.SetFieldValue("DOCUMENT_TYPE_NAME", CUtil.DocumentTypeToText(dt));

            double newh = AvailableSpace - nr.GetHeight();

            if (newh > 0)
            {
                String entityId = o.GetFieldValue(getGroupKey());
                if (row == 0)
                {
                    prevEntityId = entityId;

                    double h = addNewDataRow(rowdef, rpp, "DATA_LEVEL0", "L0", row, o);
                    newh = newh - h;
                }

                if (!entityId.Equals(prevEntityId))
                {
                    prevEntityId = entityId;

                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", entitySums);
                    newh = newh - h;

                    //Reset
                    entitySums = new ArrayList();

                    h = addNewDataRow(rowdef, rpp, "DATA_LEVEL0", "L0", row, o);
                    newh = newh - h;
                }

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                entitySums = sumDataTexts("L1", entitySums, temps);

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", entitySums);
                    newh = newh - h;

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
                prevEntityId = tmpPrevKey;
            }
            else
            {
                AvailableSpace = newh;
            }

            return (rpp);
        }
        
        private void LoadComboCustomerGroup(ComboBox cbo, String id)
        {
            CUtil.LoadCustomerGroup(cbo, true, id);
        }

        private void LoadComboCustomerType(ComboBox cbo, String id)
        {
            CUtil.LoadCustomerType(cbo, true, id);
        }

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            CTable extParam = GetExtraParam();
            String category = extParam.GetFieldValue("CATEGORY");

            String code = "customer_code";
            String name = "customer_name";

            if (category.Equals("2"))
            {
                code = "supplier_code";
                name = "supplier_name";
            }

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("from_duedate", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DUE_DATE");
            entries.Add(entry);

            entry = new CEntry("to_duedate", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DUE_DATE");
            entries.Add(entry);

            entry = new CEntry(code, EntryType.ENTRY_TEXT_BOX, 200, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry(name, EntryType.ENTRY_TEXT_BOX, 350, true, "ENTITY_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
