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
    public class CReportProfit001_01_ProfitByMonth : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private CTable extParam = null;
        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        private String prevGroupId = "";
        private ArrayList groupSum1 = null;

        public CReportProfit001_01_ProfitByMonth() : base()
        {
        }

        private void configReport()
        {
            String yyyy = Parameter.GetFieldValue("DOCUMENT_YYYY");

            addConfig("L0", 20, "", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L0", 156, yyyy, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOC_TYPE_DESC", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 15, "document_type", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOC_TYPE_DESC", "S", false);
            addConfig("L1", 12, "jan", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "01", "D", true);
            addConfig("L1", 12, "feb", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "02", "D", true);
            addConfig("L1", 12, "mar", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "03", "D", true);
            addConfig("L1", 12, "apr", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "04", "D", true);
            addConfig("L1", 12, "may", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "05", "D", true);
            addConfig("L1", 12, "jun", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "06", "D", true);
            addConfig("L1", 12, "jul", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "07", "D", true);
            addConfig("L1", 12, "aug", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "08", "D", true);
            addConfig("L1", 12, "sep", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "09", "D", true);
            addConfig("L1", 12, "oct", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "10", "D", true);
            addConfig("L1", 12, "nov", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "11", "D", true);
            addConfig("L1", 12, "dec", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "12", "D", true);
            addConfig("L1", 12, "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL", "D", true);
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

        private void filterDrCrAmount(CTable o)
        {
            int factor = 1;

            String docType = o.GetFieldValue("DOCUMENT_TYPE");
            AccountDocumentType dt = (AccountDocumentType)CUtil.StringToInt(docType);

            if ((dt == AccountDocumentType.AcctDocDrNote) || (dt == AccountDocumentType.AcctDocDrNotePurchase))
            {
                factor = 1;
            }
            else if ((dt == AccountDocumentType.AcctDocCrNotePurchase) || (dt == AccountDocumentType.AcctDocCrNote))
            {
                factor = -1;
            }

            ArrayList temps = new ArrayList { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "TOTAL" };
            for (int i = 0; i <= 12; i++)
            {
                String mm = (String) temps[i];
                double amt = CUtil.StringToDouble(o.GetFieldValue(mm));

                amt = factor * Math.Abs(amt);
                o.SetFieldValue(mm, amt.ToString());
            }

            o.SetFieldValue("DOC_TYPE_DESC", CLanguage.getValue("doc_" + docType));
        }

        private String getGroupKey()
        {
            return ("GROUP");
        }

        protected override ArrayList getRecordSet()
        {
            //Parameter.SetFieldValue("DOCUMENT_YYYY", "2018");
            ArrayList arr = OnixWebServiceAPI.GetProfitByDocTypeMonth(Parameter);

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

        private String getTotalKeyword(String group)
        {
            if (group.Equals("1"))
            {
                return ("total_receive");
            }

            return ("total_expense");
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
            filterDrCrAmount(o);

            if (newh > 0)
            {
                String groupId = o.GetFieldValue(getGroupKey());
                if (row == 0)
                {
                    prevGroupId = groupId;
                }

                if (!groupId.Equals(prevGroupId))
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", getTotalKeyword(prevGroupId), groupSums);
                    newh = newh - h;

                    prevGroupId = groupId;
                    groupSum1 = groupSums;

                    //Reset
                    groupSums = new ArrayList();
                }

                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                groupSums = sumDataTexts("L1", groupSums, temps);

                if (row == rowcount - 1)
                {
                    double h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", getTotalKeyword(prevGroupId), groupSums);
                    newh = newh - h;

                    ArrayList profitSums = sumDataTexts("L1", groupSum1, groupSums, -1);

                    h = addNewFooterRow(rowdef, rpp, "FOOTER_LEVEL1", "L1", "gain_loss", profitSums);
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

        private void LoadCombo(ComboBox cbo, String id)
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

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("year_century", EntryType.ENTRY_TEXT_BOX, 100, false, "DOCUMENT_YYYY");
            entries.Add(entry);

            return (entries);
        }
    }
}
