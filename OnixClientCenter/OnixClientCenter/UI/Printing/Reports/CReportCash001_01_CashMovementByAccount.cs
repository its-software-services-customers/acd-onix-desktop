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
    public class CReportCash001_01_CashMovementByAccount : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private CTable extParam = null;
        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        private String prevGroupId = "";

        public CReportCash001_01_CashMovementByAccount() : base()
        {
        }

        private void configReport()
        {
            String accountID = Parameter.GetFieldValue("CASH_ACCOUNT_ID");
            Hashtable accountHash = CUtil.ObserableCollectionToHash(CMasterReference.Instance.CashAccounts, "CashAccountID");
            MCashAccount acct = (MCashAccount) accountHash[accountID];

            addConfig("L0", 5, "", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L0", 30, acct.AccountNo, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "S", false);
            addConfig("L0", 30, acct.AccountName, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "S", false);
            addConfig("L0", 30, acct.BankName, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 12, "inventory_doc_date", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOCUMENT_DATE", "DT", false);
            addConfig("L1", 18, "docno", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Right, "DOCUMENT_NO", "S", false);
            addConfig("L1", 10, "deposit", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "DEPOSIT_AMOUNT", "DE", true);
            addConfig("L1", 10, "withdraw", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "WITHDRAW_AMOUNT", "DE", true);
            addConfig("L1", 10, "balance_amount", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL_AMOUNT", "D", false);
            addConfig("L1", 10, "inventory_doc_status", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DOCUMENT_STATUS_DESC", "S", false);
            addConfig("L1", 20, "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "DESCRIPTION", "S", false);
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

        private void manipulateRow(CTable o)
        {
            String payeeName = o.GetFieldValue("CHEQUE_ENTITY_NAME");
            String entityName = o.GetFieldValue("ENTITY_NAME");
            Boolean balanceFlag = o.GetFieldValue("BALANCE_FLAG").Equals("Y");
            int status = CUtil.StringToInt(o.GetFieldValue("DOCUMENT_STATUS"));

            String statusDesc = CUtil.InvDocStatusToString((InventoryDocumentStatus) status);

            o.SetFieldValue("DOCUMENT_STATUS_DESC", statusDesc);

            o.SetFieldValue("DESCRIPTION", payeeName);
            if (payeeName.Equals(""))
            {
                o.SetFieldValue("DESCRIPTION", entityName);
            }

            if (balanceFlag)
            {
                o.SetFieldValue("DOCUMENT_NO", CLanguage.getValue("balance_forward"));
            }
        }

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetCashMovementList", "CASH_MOVEMENT_LIST", Parameter);        
            return (arr);
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
            createDataHeaderRow1(page);
            createDataHeaderRow2(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            String tmpPrevKey = prevGroupId;
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
                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "total", sums);
                    newh = newh - h;
                }
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                groupSums = keepTotal1;
                sums = keepTotal2;
                prevGroupId = tmpPrevKey;
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

        public void LoadCashAccountCombo(ComboBox cbo, String id)
        {
            CUtil.LoadCashAccount(cbo, true, id);
        }

        private void InitCashAccountCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "BankAndAccountNo";
        }

        private String CashAccountToIndex(MBaseModel obj)
        {
            if (obj == null)
            {
                return ("");
            }

            return ((obj as MCashAccount).CashAccountID);
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

        public override ArrayList GetReportInputEntries()
        {
            extParam = GetExtraParam();

            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("AccNo", EntryType.ENTRY_COMBO_BOX, 500, false, "CASH_ACCOUNT_ID");
            entry.SetComboLoadAndInit(LoadCashAccountCombo, InitCashAccountCombo, CashAccountToIndex);
            entries.Add(entry);

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadStatusCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            return (entries);
        }
    }
}
