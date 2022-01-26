using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Report;
using System.Collections.Generic;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports.AcDesign
{
    public class CReportProfitACD_01_ProfitByMonth : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();

        private CTable extParam = null;
        private ArrayList sums = new ArrayList();
        private ArrayList groupSums = new ArrayList();
        //private String prevGroupId = "";
        //private ArrayList groupSum1 = null;

        public CReportProfitACD_01_ProfitByMonth() : base()
        {
        }

        private void configReport()
        {
            String yyyy = Parameter.GetFieldValue("DOCUMENT_YYYY");

            addConfig("L0", 20, "", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L0", 156, yyyy, HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "DOC_TYPE_DESC", "S", false);

            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 15, "description", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "DOC_TYPE_DESC", "S", false);
            addConfig("L1", 12, "total", HorizontalAlignment.Center, HorizontalAlignment.Right, HorizontalAlignment.Right, "TOTAL", "D", true);
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

        private void filterRecord(CTable o)
        {
            String docType = o.GetFieldValue("DOCUMENT_TYPE");
            o.SetFieldValue("DOC_TYPE_DESC", docTypeMapping(docType));

            if ("p001,m_001,m_002,r_tot,e_tot".Contains(docType))
            {
                o.SetFieldValue("ROW_TYPE", "FOOTER_LEVEL1");
            }
            else
            {
                o.SetFieldValue("ROW_TYPE", "DATA_LEVEL1");
            }
        }

        private String docTypeMapping(String dt)
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>
            {
                {"r001", "ยอดขาย"},
                {"r002", "ดอกเบี้ย, รับอื่น ๆ"},
                {"r_tot", "รวมรับ"},
                {"e001", "ยอดซื้อ"},
                {"c001", "ต้นทุนขาย"},
                {"e002", "น้ำมัน, จ่ายอื่น ๆ"},
                {"e003", "เงินเดือน"},
                {"e004", "ที่จอดรถ, ทางด่วน"},
                {"e_tot", "รวมจ่าย"},
                {"p001", "กำไร/ขาดทุน"},
                {"p_wh001", "ยอดซื้อที่ถูกหัก ณ ที่จ่าย"},
                {"p_wh002", "ภาษีหัก ณ ที่จ่าย (ซื้อ)"},
                {"s_wh001", "ยอดขายที่ถูกหัก ณ ที่จ่าย"},
                {"s_wh002", "ภาษีหัก ณ ที่จ่าย (ขาย)"},
                {"m_001", "%ต้นทุนขาย"},
                {"m_002", "%กำไร"},
            };

            String desc = "NOT FOUND";
            if (dictionary.ContainsKey(dt))
            {
                desc = dictionary[dt];
            }

            return (desc);
        }

        private String getGroupKey()
        {
            return ("GROUP");
        }

        protected override ArrayList getRecordSet()
        {
            //Parameter.SetFieldValue("DOCUMENT_YYYY", "2018");
            ArrayList arr = OnixWebServiceAPI.GetListAPI("AcdGetProfitByDocTypeMonth", "DOCTYPE_SUMMARY_LIST", Parameter);

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
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            //ArrayList keepTotal1 = copyTotalArray(groupSums);
            //ArrayList keepTotal2 = copyTotalArray(sums);

            filterRecord(o);
            String rowType = o.GetFieldValue("ROW_TYPE");

            CRow r = (CRow)rowdef[rowType];
            CRow nr = r.Clone();

            double newh = AvailableSpace - nr.GetHeight();
            
            if (newh > 0)
            {
                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);
            }

            if (newh < 1)
            {
                rpp.IsNewPageRequired = true;

                //พวก sum ทั้งหลายจะถูกคืนค่ากลับไปด้วย เพราะถูกบวกไปแล้ว
                //groupSums = keepTotal1;
                //sums = keepTotal2;
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

        private void LoadProjectGroupCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, true, id, CMasterReference.Instance.GetMasterRefCollection(MasterRefEnum.MASTER_PROJECT_GROUP));
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

            entry = new CEntry("project_group_code", EntryType.ENTRY_TEXT_BOX, 400, true, "PROJECT_GROUP_CODE");
            entries.Add(entry);

            entry = new CEntry("project_code", EntryType.ENTRY_TEXT_BOX, 400, true, "PROJECT_CODE");
            entries.Add(entry);

            entry = new CEntry("display_exception_expense", EntryType.ENTRY_CHECK_BOX, 300, true, "IS_EXPENSE_EXCEPTION");
            entries.Add(entry);

            entry = new CEntry("display_sale_cost", EntryType.ENTRY_CHECK_BOX, 300, true, "IS_SALE_COST");
            entries.Add(entry);

            return (entries);
        }
    }
}
