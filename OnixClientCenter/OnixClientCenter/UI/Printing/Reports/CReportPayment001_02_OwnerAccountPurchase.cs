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
    public class CReportPayment001_02_OwnerAccountPurchase : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private ArrayList sums = new ArrayList();

        private double[] totals = new double[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private CTable extParam = null;
        private Hashtable refundHash = null;
        private Hashtable paymentTypeHash = null;

        public CReportPayment001_02_OwnerAccountPurchase() : base()
        {
            refundHash = CUtil.ObserableCollectionToHash(CMasterReference.Instance.RefundStatus, "MasterID");
            paymentTypeHash = CUtil.ObserableCollectionToHash(CMasterReference.Instance.AccountSalePayTypes, "MasterID");            
        }

        private void configReport()
        {
            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 10, "date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L1", 12, "inventory_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOCUMENT_NO", "S", false);
            addConfig("L1", 10, "invoice_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "REF_DOCUMENT_NO", "S", false);
            addConfig("L1", 24, "supplier_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ENTITY_NAME", "S", false);
            addConfig("L1", 12, "payment_type", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PAYMENT_TYPE_DESC", "S", false);
            addConfig("L1", 15, "bank_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "BANK_NAME", "S", false);
            addConfig("L1", 15, "AccNo", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "ACCOUNT_NO", "S", false);
            addConfig("L1", 12, "refund_status", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "REFUND_STATUS_DESC", "S", false);
            addConfig("L1", 12, "payment_doc_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PAYMENT_DOCUMENT_NO", "S", false);
            addConfig("L1", 10, "amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "PAID_AMOUNT", "D", true);
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
            String refundStatus = Parameter.GetFieldValue("REFUND_STATUS");
            Parameter.SetFieldValue("IS_NULL_REFUND_STATUS", "");
            if (refundStatus.Equals("999"))
            {
                Parameter.SetFieldValue("IS_NULL_REFUND_STATUS", "Y");
                Parameter.SetFieldValue("REFUND_STATUS", "");
            }

            Parameter.SetFieldValue("OWNER_FLAG", "Y");
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetPaymentTransactionList", "PAYMENT_TRANSACTION_LIST", Parameter);
            return (arr);
        }

        private void populateData(CTable data)
        {
            String refundStatus = data.GetFieldValue("REFUND_STATUS");

            MMasterRef mr = (MMasterRef)refundHash[refundStatus];
            String refundDesc = CLanguage.getValue("NotSelected");
            if (mr != null)
            {
                refundDesc = mr.Description;
            }
            data.SetFieldValue("REFUND_STATUS_DESC", refundDesc);

            String paymentType = data.GetFieldValue("PAYMENT_TYPE");
            String paymentDesc = ((MMasterRef)paymentTypeHash[paymentType]).Description;
            data.SetFieldValue("PAYMENT_TYPE_DESC", paymentDesc);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            ArrayList keepTotal2 = copyTotalArray(sums);

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            populateData(o);

            double newh = AvailableSpace - nr.GetHeight();
            if (newh > 0)
            {
                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);

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
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.PurchasePaymentDocTypes);
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

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("document_type", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_TYPE");
            entry.SetComboLoadAndInit(LoadDocTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("payment_type", EntryType.ENTRY_COMBO_BOX, 200, true, "PAYMENT_TYPE");
            entry.SetComboLoadAndInit(LoadPaymentTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("refund_status", EntryType.ENTRY_COMBO_BOX, 200, true, "REFUND_STATUS");
            entry.SetComboLoadAndInit(LoadRefundStatusCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            return (entries);
        }
    }
}
