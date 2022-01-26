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
    public class CReportArAp001_01_Movement : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private String prevEntityId = "";
        private ArrayList entitySums = new ArrayList();
        private ArrayList sums = new ArrayList();

        public CReportArAp001_01_Movement() : base()
        {
        }

        private void configReport()
        {
            String category = Parameter.GetFieldValue("CATEGORY");

            String code = "customer_code";
            String name = "customer_name";
            String type = "customer_type";
            String group = "customer_group";

            if (category.Equals("2"))
            {
                code = "supplier_code";
                name = "supplier_name";
                type = "supplier_type";
                group = "supplier_group";
            }

            addConfig("L0", 20, code, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_CODE", "S", false);
            addConfig("L0", 25, name, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_NAME", "S", false);
            addConfig("L0", 44, type, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_TYPE_NAME554", "S", false);
            addConfig("L0", 26, group, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_GROUP_NAME", "S", false);

            addConfig("L1", 4, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 8, "approved_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "APPROVED_DATE", "DT", false);
            addConfig("L1", 8, "inventory_doc_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);            
            addConfig("L1", 8, "due_date1", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DUE_DATE", "DT", false);
            addConfig("L1", 17, "document_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_NO", "S", false);
            addConfig("L1", 11, "balance_forward", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "BEGIN_AMOUNT", "D", false);
            addConfig("L1", 11, "in_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "IN_AMOUNT", "D", true);
            addConfig("L1", 11, "out_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "OUT_AMOUNT", "D", true);
            addConfig("L1", 11, "balance_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "END_AMOUNT", "D", false);
            addConfig("L1", 26, "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_DESC", "S", false);
        }

        #region Create Layout Report

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetArApTransactionMovementList(Parameter);
            return (arr);
        }

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

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevEntityId;
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal1 = copyTotalArray(entitySums);
            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();
           
            double newh = AvailableSpace - nr.GetHeight();

            double amt = CUtil.StringToDouble(o.GetFieldValue("TX_AMOUNT"));
            String txType = o.GetFieldValue("TX_TYPE");

            o.SetFieldValue("IN_AMOUNT", "0");
            o.SetFieldValue("OUT_AMOUNT", amt.ToString());
            if (txType.Equals("I"))
            {
                o.SetFieldValue("IN_AMOUNT", amt.ToString());
                o.SetFieldValue("OUT_AMOUNT", "0");                
            }


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

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        private String getGroupKey()
        {
            return ("ENTITY_ID");
        }

        private void LoadComboCustomerGroup(ComboBox cbo, String id)
        {
            CUtil.LoadCustomerGroup(cbo, true, id);
        }

        private void LoadComboCustomerType(ComboBox cbo, String id)
        {
            CUtil.LoadCustomerType(cbo, true, id);
        }

        private void LoadComboSupplierGroup(ComboBox cbo, String id)
        {
            CUtil.LoadSupplierGroup(cbo, true, id);
        }

        private void LoadComboSupplierType(ComboBox cbo, String id)
        {
            CUtil.LoadSupplierType(cbo, true, id);
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

            entry = new CEntry(code, EntryType.ENTRY_TEXT_BOX, 200, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry(name, EntryType.ENTRY_TEXT_BOX, 350, true, "ENTITY_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
