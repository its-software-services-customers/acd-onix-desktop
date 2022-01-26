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
    public class CReportGeneral001_01_ServicesList : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private String prevEntityId = "";
        private ArrayList entitySums = new ArrayList();
        private ArrayList sums = new ArrayList();

        public CReportGeneral001_01_ServicesList() : base()
        {
        }

        private void configReport()
        {
            addConfig("L1", 5, "number", HorizontalAlignment.Center, HorizontalAlignment.Center, HorizontalAlignment.Center, "", "RN", false);
            addConfig("L1", 15, "service_code", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "SERVICE_CODE", "S", false);
            addConfig("L1", 40, "service_name", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "SERVICE_NAME", "S", false);
            addConfig("L1", 20, "service_type", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Center, "SERVICE_TYPE_NAME", "S", false);
            addConfig("L1", 20, "service_uom", HorizontalAlignment.Center, HorizontalAlignment.Left, HorizontalAlignment.Left, "SERVICE_UOM_NAME", "S", false);
        }        

        protected override ArrayList getRecordSet()
        {
            ArrayList arr = OnixWebServiceAPI.GetListAPI("GetServiceListAll", "SERVICE_LIST", Parameter);
            return (arr);
        }

        #region Create Layout Report

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

            //== START BODY ===

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
            createDataHeaderRow2(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        #endregion

        public override Boolean IsNewVersion()
        {
            return (true);
        }

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

            if (newh > 0)
            {
                ArrayList temps = getColumnDataTexts("L1", row + 1, o);
                nr.FillColumnsText(temps);
                rpp.AddReportRow(nr);

                sums = sumDataTexts("L1", sums, temps);
                entitySums = sumDataTexts("L1", entitySums, temps);

                if (row == rowcount - 1)
                {           
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

        private void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        private void LoadServiceTypeCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.ServiceTypes);
        }

        private void LoadServiceUomCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.Uoms);
        }

        private void LoadServiceCategoryCombo(ComboBox cbo, String id)
        {
            CUtil.LoadComboFromCollection(cbo, false, id, CMasterReference.Instance.ServiceCategories);
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("service_code", EntryType.ENTRY_TEXT_BOX, 200, true, "ENTITY_CODE");
            entries.Add(entry);

            entry = new CEntry("service_name", EntryType.ENTRY_TEXT_BOX, 350, true, "ENTITY_NAME");
            entries.Add(entry);

            entry = new CEntry("service_type", EntryType.ENTRY_COMBO_BOX, 200, true, "SERVICE_TYPE");
            entry.SetComboLoadAndInit(LoadServiceTypeCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("service_uom", EntryType.ENTRY_COMBO_BOX, 200, true, "SERVICE_UOM");
            entry.SetComboLoadAndInit(LoadServiceUomCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);

            entry = new CEntry("service_category", EntryType.ENTRY_COMBO_BOX, 200, true, "CATEGORY");
            entry.SetComboLoadAndInit(LoadServiceCategoryCombo, InitCombo, ObjectToIndex);
            entries.Add(entry);
            
            return (entries);
        }
    }
}
