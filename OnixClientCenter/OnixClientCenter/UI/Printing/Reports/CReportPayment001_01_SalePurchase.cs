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
    public class CReportPayment001_01_SalePurchase : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private ArrayList sums = new ArrayList();

        //private double[] widths = new double[8] { 5, 8, 15, 11, 26, 15, 10, 10 };
        private double[] totals = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private CTable extParam = null;

        public CReportPayment001_01_SalePurchase() : base()
        {
            
        }

        private void configReport()
        {
            String column1 = "customer_name";
            String column2 = "po_no";

            if (extParam.GetFieldValue("CATEGORY").Equals("2"))
            {
                column1 = "supplier_name";
                column2 = "invoice_no";
            }

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 8, "date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "REF_WH_DOC_NO", "S", false);
            addConfig("L1", 15, "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L1", 11, column2, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_NO", "S", false);
            addConfig("L1", 26, column1, HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "REF_DOCUMENT_NO", "S", false);
            addConfig("L1", 15, "bank_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "TEMP_ITEM_DESC", "S", false);
            addConfig("L1", 10, "cheque_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "WH_TAX_PCT", "S", false);
            addConfig("L1", 10, "amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "REVENUE_EXPENSE_AMT", "D", true);
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

        protected override ArrayList getRecordSet()
        {
            //Parameter.SetFieldValue("CATEGORY", "2");
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetPaymentTransactionList", "PAYMENT_TRANSACTION_LIST", Parameter);

            return (arr);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            double[] keepTotal2 = copyTotalArray(totals);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            double amt = CUtil.StringToDouble(o.GetFieldValue("PAID_AMOUNT"));

            double newh = AvailableSpace - nr.GetHeight();
            if (newh > 0)
            {
                String pmtType = o.GetFieldValue("PAYMENT_TYPE");
                String chequeNo = "";
                String bankName = "";
                if (pmtType.Equals("4"))
                {
                    //Cheque
                    chequeNo = o.GetFieldValue("CHEQUE_NO");
                    bankName = o.GetFieldValue("CHEQUE_TO_BANK_NAME");
                }
                else
                {
                    bankName = o.GetFieldValue("BANK_NAME");
                }

                String refDocField = "REF_PO_NO";
                if (extParam.GetFieldValue("CATEGORY").Equals("2"))
                {
                    refDocField = "REF_DOCUMENT_NO";                    
                }

                nr.FillColumnsText((row + 1).ToString(),
                        CUtil.DateTimeToDateString(CUtil.InternalDateToDate(o.GetFieldValue("DOCUMENT_DATE"))),
                        o.GetFieldValue("DOCUMENT_NO"),
                        o.GetFieldValue(refDocField),
                        o.GetFieldValue("ENTITY_NAME"),
                        bankName,
                        chequeNo,
                        CUtil.FormatNumber(amt.ToString()));

                totals[7] = totals[7] + amt;


                rpp.AddReportRow(nr);

                if (row == rowcount - 1)
                {
                    //End row
                    CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
                    CRow ftr = ft.Clone();

                    ftr.FillColumnsText(CLanguage.getValue("total"), "", "", "", "", "", "", CUtil.FormatNumber(totals[7].ToString()));

                    rpp.AddReportRow(ftr);
                    newh = newh - ftr.GetHeight();
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                totals = keepTotal2;
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
            if (extParam.GetFieldValue("CATEGORY").Equals("2"))
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchasePaymentDocTypes);
            }
            else
            {
                CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.SalePaymentDocTypes);
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

        public override ArrayList GetReportInputEntries()
        {
            extParam = GetExtraParam();

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

            entry = new CEntry("payment_type", EntryType.ENTRY_COMBO_BOX, 200, true, "PAYMENT_TYPE");
            entry.SetComboLoadAndInit(LoadPaymentTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            return (entries);
        }
    }
}
