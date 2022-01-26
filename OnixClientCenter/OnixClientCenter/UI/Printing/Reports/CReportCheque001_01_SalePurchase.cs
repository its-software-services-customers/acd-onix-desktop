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
    public class CReportCheque001_01_SalePurchase : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private ArrayList sums = new ArrayList();

        private CTable extParam = null;

        public CReportCheque001_01_SalePurchase() : base()
        {         
        }

        private void configReport()
        {
            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 12, "cheque_due_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "CHEQUE_DATE", "DT", false);
            addConfig("L1", 12, "cheque_no", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "CHEQUE_NO", "S", false);
            addConfig("L1", 15, "Bank", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ACCOUNT_BANK_NAME", "S", false);
            addConfig("L1", 15, "AccNo", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "ACCOUNT_NO", "S", false);
            addConfig("L1", 20, "payee_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "PAYEE_NAME", "S", false);
            addConfig("L1", 12, "cheque_status", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "CHEQUE_STATUS_DESC", "S", false);
            addConfig("L1", 10, "money_quantity", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "CHEQUE_AMOUNT", "D", true);
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
            Parameter.SetFieldValue("OWNER_FLAG", "Y");
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetChequeListAll", "CHEQUE_LIST", Parameter);
            return (arr);
        }

        private void populateData(CTable data)
        {
            String status = data.GetFieldValue("CHEQUE_STATUS");

            InventoryDocumentStatus dt = (InventoryDocumentStatus)Int32.Parse(status);
            String statusDesc = CUtil.InvDocStatusToString(dt);

            data.SetFieldValue("CHEQUE_STATUS_DESC", statusDesc);
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

        private void LoadStatusCombo(ComboBox cbo, String id)
        {
            CUtil.LoadInventoryDocStatus(cbo, true, id);
        }

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        public void LoadBankCombo(ComboBox cbo, String id)
        {
            CUtil.LoadMasterRefCombo(cbo, true, MasterRefEnum.MASTER_BANK, id);
        }

        public override ArrayList GetReportInputEntries()
        {
            extParam = GetExtraParam();

            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("from_cheque_duedate", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_CHEQUE_DATE");
            entries.Add(entry);

            entry = new CEntry("to_cheque_duedate", EntryType.ENTRY_DATE_MAX, 200, true, "TO_CHEQUE_DATE");
            entries.Add(entry);

            entry = new CEntry("cheque_status", EntryType.ENTRY_COMBO_BOX, 200, true, "CHEQUE_STATUS");
            entry.SetComboLoadAndInit(LoadStatusCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("Bank", EntryType.ENTRY_COMBO_BOX, 200, true, "ACCOUNT_BANK_ID");
            entry.SetComboLoadAndInit(LoadBankCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("AccNo", EntryType.ENTRY_TEXT_BOX, 300, true, "ACCOUNT_NO");
            entries.Add(entry);

            entry = new CEntry("code", EntryType.ENTRY_TEXT_BOX, 300, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry("name", EntryType.ENTRY_TEXT_BOX, 300, true, "ENTITY_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
