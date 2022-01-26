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
    public class CReportPurchase001_02_SalePurchaseWhTax : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private ArrayList sums = new ArrayList();
        private int rowProcess = 0;

        public CReportPurchase001_02_SalePurchaseWhTax() : base()
        {
        }

        private void configReport()
        {
            CTable extParam = GetExtraParam();
            String category = extParam.GetFieldValue("CATEGORY");
            String rt = Parameter.GetFieldValue("REPORT_TYPE");

            String desc = "revenue";
            String name = "customer_name";
            if (category.Equals("2"))
            {
                desc = "expense";
                name = "supplier_name";
            }

            String docDateField = "DOCUMENT_DATE";
            String docDateColumn = "date";
            if (rt.Equals("1"))
            {
                docDateColumn = "invoice_date";
                docDateField = "REF_DOCUMENT_DATE";
            }

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 12, "wh_doc_no_short", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "REF_WH_DOC_NO", "S", false);
            addConfig("L1", 9, docDateColumn, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, docDateField, "DT", false);
            addConfig("L1", 12, "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_NO", "S", false);
            addConfig("L1", 11, "invoice_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "REF_DOCUMENT_NO", "S", false);
            addConfig("L1", 19, name, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ENTITY_NAME", "S", false);
            addConfig("L1", 4, "%", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "WH_TAX_PCT", "S", false);
            addConfig("L1", 9, desc, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "REVENUE_EXPENSE_AMT", "D", true);
            addConfig("L1", 9, "total_vat", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "AR_AP_AMT", "D", true);
            addConfig("L1", 9, "wh_eligible", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "WH_TAX_AMT", "D", true);
            addConfig("L1", 9, "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "REMAIN_AMT", "D", true);
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

        private void createDataHeaderRow(UReportPage page)
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
            double whAmt = CUtil.StringToDouble(o.GetFieldValue("WH_TAX_AMT"));
            double amt = CUtil.StringToDouble(o.GetFieldValue("REVENUE_EXPENSE_AMT"));
            double arap = CUtil.StringToDouble(o.GetFieldValue("AR_AP_AMT"));

            int factor = 1;

            String refDt = o.GetFieldValue("REF_DOCUMENT_TYPE");
            if (refDt.Equals(""))
            {
                //ซื้อสด
                refDt = o.GetFieldValue("DOCUMENT_TYPE");
            }
            else
            {
                //จ่ายชำระหนี้
                //Do nothing
            }

            AccountDocumentType dt = (AccountDocumentType)CUtil.StringToInt(refDt);

            if ((dt == AccountDocumentType.AcctDocDrNote) || (dt == AccountDocumentType.AcctDocDrNotePurchase))
            {
                factor = 1;
            }
            else if ((dt == AccountDocumentType.AcctDocCrNotePurchase) || (dt == AccountDocumentType.AcctDocCrNote))
            {
                factor = -1;
            }

            whAmt = factor * Math.Abs(whAmt);
            amt = factor * Math.Abs(amt);
            arap = factor * Math.Abs(arap);
            double total = arap - whAmt; //ยอดหลังหัก

            o.SetFieldValue("WH_TAX_AMT", whAmt.ToString());
            o.SetFieldValue("REVENUE_EXPENSE_AMT", amt.ToString());
            o.SetFieldValue("AR_AP_AMT", arap.ToString());
            o.SetFieldValue("REMAIN_AMT", total.ToString());
        }

        protected override ArrayList getRecordSet()
        {
            //Parameter.SetFieldValue("CATEGORY", "2");
            Parameter.SetFieldValue("WH_TAX_FLAG", "Y");
            Parameter.SetFieldValue("ACTUAL_PAY", "Y");
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetSalePurchaseWhDocList", "SALE_PURCHASE_DOC_LIST", Parameter);
            return (arr);
        }

        private Boolean isSkipRow(CTable o)
        {
            double whAmt = CUtil.StringToDouble(o.GetFieldValue("WH_TAX_AMT"));
            return (whAmt <= 0);
        }

        private void addTotalRow(CReportDataProcessingProperty rpp, ArrayList sumArray)
        {
            CRow ft1 = (CRow)rowdef["FOOTER_LEVEL1"];
            CRow ftr1 = ft1.Clone();

            ArrayList totals = displayTotalTexts("L1", sumArray, 1, "total");
            ftr1.FillColumnsText(totals);

            rpp.AddReportRow(ftr1);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            if (isSkipRow(o))
            {
                //In case the last record is skipped

                if (row == rowcount - 1)
                {
                    addTotalRow(rpp, sums);
                }

                //Do Nothing
                return (rpp);
            }

            ArrayList keepTotal2 = copyTotalArray(sums);
            rowProcess++;

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            if (newh > 0)
            {
                filterDrCrAmount(o);

                String refDocNo = o.GetFieldValue("REF_DOCUMENT_NO");
                if (refDocNo.Equals(""))
                {
                    refDocNo = o.GetFieldValue("ACTUAL_INVOICE_NO");
                }
                o.SetFieldValue("REF_DOCUMENT_NO", refDocNo);

                String refDocDate = o.GetFieldValue("REF_DOCUMENT_DATE");
                if (refDocDate.Equals(""))
                {
                    refDocDate = o.GetFieldValue("ACTUAL_INVOICE_DATE");
                }
                o.SetFieldValue("REF_DOCUMENT_DATE", refDocDate);

                ArrayList temps = getColumnDataTexts("L1", rowProcess, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);

                if (row == rowcount - 1)
                {
                    addTotalRow(rpp, sums);
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
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchaseWhDocTypes);
            }
            else
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.SaleRevenueDocTypes);
            }            
        }

        private void LoadRevenueTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.RevenueTaxTypes);
        }

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        private void LoadReportTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.VatReportTypes);
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

            entry = new CEntry("tax_revenue_type", EntryType.ENTRY_COMBO_BOX, 200, true, "RV_TAX_TYPE");
            entry.SetComboLoadAndInit(LoadRevenueTypeCombo, InitCombo, ObjectToIndex);
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
