﻿using System;
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
    public class CReportPurchase001_01_SalePurchaseTax : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private ArrayList sums = new ArrayList();

        public CReportPurchase001_01_SalePurchaseTax() : base()
        {
        }

        private void configReport()
        {
            String category = Parameter.GetFieldValue("CATEGORY");
            String rt = Parameter.GetFieldValue("REPORT_TYPE");

            String code = "customer_code";
            String name = "customer_name";
            String type = "revenue";
            String docType = "sale_order";
            String docField = "REF_SALE_ORDER_NO";

            int[] widths = new int[10] { 5, 10, 10, 0, 12, 25, 17, 10, 10, 10 };

            if (category.Equals("2"))
            {
                code = "supplier_code";
                name = "supplier_name";
                type = "expense";
                docType = "invoice_no";
                docField = "REF_DOCUMENT_NO";

                widths = new int[10] { 5, 12, 15, 11, 12, 25, 0, 10, 10, 10 };
            }

            String docDateField = "DOCUMENT_DATE";
            String docDateColumn = "date";
            if (rt.Equals("1"))
            {
                docDateColumn = "invoice_date";
                docDateField = "REF_DOCUMENT_DATE";
            }

            addConfig("L1", widths[0], "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", widths[1], docDateColumn, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, docDateField, "DT", false);
            addConfig("L1", widths[2], "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_NO", "S", false);

            addConfig("L1", widths[3], docType, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, docField, "S", false);

            addConfig("L1", widths[4], code, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ENTITY_CODE", "S", false);
            addConfig("L1", widths[5], name, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ENTITY_NAME", "S", false);

            addConfig("L1", widths[6], "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "NOTE", "S", false);

            addConfig("L1", widths[7], type, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "REVENUE_EXPENSE_FOR_VAT_AMT", "D", true);
            addConfig("L1", widths[8], "vat_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "VAT_AMT", "D", true);
            addConfig("L1", widths[9], "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "AR_AP_AMT", "D", true);
        }

        private void createDataHeaderRow(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];

            r.FillColumnsText(getColumnHederTexts("L1", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
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

        public override Boolean IsNewVersion()
        {
            return (true);
        }

        private void filterDrCrAmount(CTable o)
        {
            double vatAmt = CUtil.StringToDouble(o.GetFieldValue("VAT_AMT"));
            double amt = CUtil.StringToDouble(o.GetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT"));
            double arap = CUtil.StringToDouble(o.GetFieldValue("AR_AP_AMT"));

            int factor = 1;

            AccountDocumentType dt = (AccountDocumentType)CUtil.StringToInt(o.GetFieldValue("DOCUMENT_TYPE"));

            if ((dt == AccountDocumentType.AcctDocDrNote) || (dt == AccountDocumentType.AcctDocDrNotePurchase))
            {
                factor = 1;
            }
            else if ((dt == AccountDocumentType.AcctDocCrNotePurchase) || (dt == AccountDocumentType.AcctDocCrNote))
            {
                factor = -1;
            }

            vatAmt = factor * Math.Abs(vatAmt);
            amt = factor * Math.Abs(amt);
            arap = factor * Math.Abs(arap);

            o.SetFieldValue("VAT_AMT", vatAmt.ToString());
            o.SetFieldValue("REVENUE_EXPENSE_FOR_VAT_AMT", amt.ToString());
            o.SetFieldValue("AR_AP_AMT", arap.ToString());
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = new ArrayList();

            //Parameter.SetFieldValue("CATEGORY", "2");
            arr = OnixWebServiceAPI.GetListAPI("GetSalePurchaseDocumentList", "SALE_PURCHASE_DOC_LIST", Parameter);

            return (arr);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            if (newh > 0)
            {
                filterDrCrAmount(o);

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);

                if (row == rowcount - 1)
                {
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
                sums = keepTotal2;
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
            CTable extParam = GetExtraParam();
            String category = extParam.GetFieldValue("CATEGORY");
            if (category.Equals("2"))
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchaseExpenseDocTypes);
            }
            else
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.SaleRevenueDocTypes);
            }
        }

        private void LoadReportTypeCombo(ComboBox cbo, String id)
        {
            CTable extParam = GetExtraParam();
            String category = extParam.GetFieldValue("CATEGORY");

            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.VatReportTypes);
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

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("document_type", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_TYPE");
            entry.SetComboLoadAndInit(LoadDocTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            if (category.Equals("2"))
            {
                entry = new CEntry("report_type", EntryType.ENTRY_COMBO_BOX, 200, true, "REPORT_TYPE");
                entry.SetComboLoadAndInit(LoadReportTypeCombo, InitCombo, ObjectToIndex);
                entries.Add(entry);
            }

            entry = new CEntry(code, EntryType.ENTRY_TEXT_BOX, 200, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry(name, EntryType.ENTRY_TEXT_BOX, 350, true, "ENTITY_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
