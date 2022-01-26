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
    public class CReportInvoice001_02_SalePurchase : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private CTable extParam = null;
        private ArrayList sums = new ArrayList();
        private ArrayList projSums = new ArrayList();
        private String prevProject = "";

        public CReportInvoice001_02_SalePurchase() : base()
        {
        }

        private void configReport()
        {
            String category = Parameter.GetFieldValue("CATEGORY");
            String rptType = Parameter.GetFieldValue("REPORT_TYPE");
            String rc = Parameter.GetFieldValue("REPORT_CATEGORY");

            String entityKey = "customer_name";
            String accountKey = "revenue";
            String docNo = "rv_no";
            if (category.Equals("2"))
            {
                entityKey = "supplier_name";
                accountKey = "expense";
                docNo = "pv_no";
            }

            String docNoField = "DOCUMENT_NO";
            String docDateColumn = "date";
            String docDateField = "DOCUMENT_DATE";
            String docNoColumn = "inventory_doc_no";
            if (rc.Equals("1"))
            {
                docDateColumn = "invoice_date";
                docDateField = "REF_DOCUMENT_DATE";
                docNoColumn = "invoice_no";
                docNoField = "REF_DOCUMENT_NO";
            }

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);

            if (rptType.Equals("1"))
            {
                addConfig("L1", 12, "date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
                addConfig("L1", 20, "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_NO", "S", false);
                addConfig("L1", 15, "project_group", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_GROUP_CODE", "S", false);
                addConfig("L1", 15, "project", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_CODE", "S", false);
            }
            else if (rptType.Equals("3"))
            {
                addConfig("L1", 15, "project_group", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_GROUP_CODE", "S", false);
                addConfig("L1", 15, "project", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_CODE", "S", false);
                addConfig("L1", 12, docDateColumn, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, docDateField, "DT", false);
                addConfig("L1", 20, docNoColumn, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, docNoField, "S", false);
            }
            else if (rptType.Equals("4"))
            {
                addConfig("L1", 15, "project", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_CODE", "S", false);
                addConfig("L1", 15, "project_group", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PROJECT_GROUP_CODE", "S", false);                
                addConfig("L1", 12, docDateColumn, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, docDateField, "DT", false);
                addConfig("L1", 20, docNoColumn, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, docNoField, "S", false);
            }

            addConfig("L1", 22, entityKey, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ENTITY_NAME", "S", false);
            addConfig("L1", 35, "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "TEMP_ITEM_DESC", "S", false);
            addConfig("L1", 15, accountKey, HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "REVENUE_EXPENSE_AMT", "D", true);
            addConfig("L1", 15, "vat_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "VAT_TAX_AMT", "D", true);
            addConfig("L1", 15, "wh_amt", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "WH_TAX_AMT", "D", true);
            addConfig("L1", 10, "wh_pct", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "WH_TAX_PCT", "D", false);
            addConfig("L1", 15, "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TEMP_TOTAL_AMT", "D", true);
            addConfig("L1", 20, docNo, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "RECEIPT_NO", "S", false);
            addConfig("L1", 35, "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "TEMP_INDEX_PAYMENT", "S", false);
        }

        private void createDataHeaderRow(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];

            r.FillColumnsText(getColumnHederTexts("L1", "H"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        private String constructPaymentDesc(String pmtDef)
        {
            if (pmtDef.Equals(""))
            {
                return ("");
            }

            string[] stringSeparators = new string[] { ";" };
            string[] result = pmtDef.Split(stringSeparators, StringSplitOptions.None);

            String firstPmt = result[0];

            string[] subSeparators = new string[] { "|" };
            string[] tokens = firstPmt.Split(subSeparators, StringSplitOptions.None);

            String pmt = tokens[0];
            String cqno = tokens[1];
            String acno = tokens[2];
            String amt = tokens[3];

            String refNo = cqno;
            if (refNo.Equals(""))
            {
                refNo = acno;
            }

            String temp = "";
            String pmtType = CUtil.PaymentTypeToString(pmt, "TH");
            if (refNo.Equals(""))
            {
                temp = String.Format("{0} : {1} ", pmtType, CUtil.FormatNumber(amt));
            }
            else
            {
                temp = String.Format("{0} : {1} : {2}", pmtType, refNo, CUtil.FormatNumber(amt));
            }

            return (temp);
        }

        private void filterDrCrAmount(CTable o)
        {
            double vatAmt = CUtil.StringToDouble(o.GetFieldValue("VAT_TAX_AMT"));
            double whAmt = CUtil.StringToDouble(o.GetFieldValue("WH_TAX_AMT"));
            double amt = CUtil.StringToDouble(o.GetFieldValue("REVENUE_EXPENSE_AMT"));
            String receipNo = o.GetFieldValue("RECEIPT_NO");
            String docNO = o.GetFieldValue("DOCUMENT_NO");
            
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
            whAmt = factor * Math.Abs(whAmt);
            amt = factor * Math.Abs(amt);

            o.SetFieldValue("VAT_TAX_AMT", vatAmt.ToString());
            o.SetFieldValue("WH_TAX_AMT", whAmt.ToString());
            o.SetFieldValue("REVENUE_EXPENSE_AMT", amt.ToString());

            if (receipNo.Equals("") && 
                ((dt == AccountDocumentType.AcctDocCashSale) || (dt == AccountDocumentType.AcctDocCashPurchase) ||
                (dt == AccountDocumentType.AcctDocMiscExpense) || (dt == AccountDocumentType.AcctDocMiscRevenue)))
            {
                o.SetFieldValue("RECEIPT_NO", docNO);
            }
        }

        private void populateTotalAmount(CTable data)
        {
            double vatamt = CUtil.StringToDouble(data.GetFieldValue("VAT_TAX_AMT"));
            double revExp = CUtil.StringToDouble(data.GetFieldValue("REVENUE_EXPENSE_AMT"));
            double wh = CUtil.StringToDouble(data.GetFieldValue("WH_TAX_AMT"));

            double amt = revExp + vatamt - wh;

            data.SetFieldValue("TEMP_TOTAL_AMT", amt.ToString());
        }

        private String getGroupKey()
        {
            String rptType = Parameter.GetFieldValue("REPORT_TYPE");

            if (rptType.Equals("3"))
            {
                return ("PROJECT_GROUP_CODE");
            }
            else if (rptType.Equals("4"))
            {
                return ("PROJECT_CODE");
            }

            return ("PROJECT_CODE");
        }


        protected override ArrayList getRecordSet()
        {
            String rptType = Parameter.GetFieldValue("REPORT_TYPE");
            ArrayList arr = new ArrayList();
            if (rptType.Equals("1"))
            {
                arr = OnixWebServiceAPI.GetListAPI("GetSalePurchaseTxList", "SALE_PURCHASE_DOC_TX_LIST", Parameter);
            }
            else if (rptType.Equals("3") || rptType.Equals("4"))
            {
                arr = OnixWebServiceAPI.GetListAPI("GetSalePurchaseTxByPoProjectGroup", "SALE_PURCHASE_DOC_TX_LIST", Parameter);
            }

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
                String selectionName = o.GetFieldValue("FREE_TEXT");
                String selType = o.GetFieldValue("SELECTION_TYPE");
                if (selType.Equals("1"))
                {
                    selectionName = o.GetFieldValue("SERVICE_NAME");
                }
                else if (selType.Equals("2"))
                {
                    selectionName = o.GetFieldValue("ITEM_NAME_THAI");
                }

                o.SetFieldValue("TEMP_ITEM_DESC", selectionName);

                String paymentDef = o.GetFieldValue("INDEX_PAYMENT");
                String rcpPaymentDef = o.GetFieldValue("RECEIPT_INDEX_PAYMENT");
                if (paymentDef.Equals(""))
                {
                    paymentDef = rcpPaymentDef;
                }
                o.SetFieldValue("TEMP_INDEX_PAYMENT", constructPaymentDesc(paymentDef));

                filterDrCrAmount(o);
                populateTotalAmount(o);

                String projCd = o.GetFieldValue(getGroupKey());
                if (row == 0)
                {
                    prevProject = projCd;
                }

                String rptType = Parameter.GetFieldValue("REPORT_TYPE");
                if (!projCd.Equals(prevProject))
                {
                    prevProject = projCd;

                    CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
                    CRow ftr = ft.Clone();

                    ArrayList projTotals = displayTotalTexts("L1", projSums, 1, "total");
                    ftr.FillColumnsText(projTotals);

                    if (rptType.Equals("4") || rptType.Equals("3") || rptType.Equals("2"))
                    {
                        //Only by project report
                        rpp.AddReportRow(ftr);
                        newh = newh - ftr.GetHeight();
                    }

                    //Reset
                    projSums = new ArrayList();
                }

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                projSums = sumDataTexts("L1", projSums, temps);

                if (row == rowcount - 1)
                {
                    //=== Project
                    CRow ft0 = (CRow)rowdef["FOOTER_LEVEL1"];
                    CRow ftr0 = ft0.Clone();

                    ArrayList projTotals = displayTotalTexts("L1", projSums, 1, "total");
                    ftr0.FillColumnsText(projTotals);

                    if (rptType.Equals("4") || rptType.Equals("3") || rptType.Equals("2"))
                    {
                        //Only by project report
                        rpp.AddReportRow(ftr0);
                        newh = newh - ftr0.GetHeight();
                    }

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

        private void LoadCombo(ComboBox cbo, String id)
        {
            CUtil.LoadInventoryDocStatus(cbo, true, id);
        }

        private void LoadDocTypeCombo(ComboBox cbo, String id)
        {
            if (extParam.GetFieldValue("CATEGORY").Equals("2"))
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchaseExpenseDocTypes);
            }
            else
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.SaleRevenueDocTypes);
            }
        }

        private void LoadPaymentTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.AccountSalePayTypes);
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
            extParam = GetExtraParam();
            String category = extParam.GetFieldValue("CATEGORY");

            CEntry entry = null;
            ArrayList entries = new ArrayList();

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
                entry = new CEntry("report_type", EntryType.ENTRY_COMBO_BOX, 200, true, "REPORT_CATEGORY");
                entry.SetComboLoadAndInit(LoadReportTypeCombo, InitCombo, ObjectToIndex);
                entries.Add(entry);
            }

            entry = new CEntry("project_code", EntryType.ENTRY_TEXT_BOX, 300, true, "PROJECT_CODE");
            entries.Add(entry);

            entry = new CEntry("project_group_code", EntryType.ENTRY_TEXT_BOX, 300, true, "PROJECT_GROUP_CODE");
            entries.Add(entry);

            return (entries);
        }
    }
}
