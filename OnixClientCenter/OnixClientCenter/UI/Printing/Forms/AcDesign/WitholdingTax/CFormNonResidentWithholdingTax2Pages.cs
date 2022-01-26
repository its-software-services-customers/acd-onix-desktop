using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Model;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Printing;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.WitholdingTax
{
    public class CFormNonResidentWithholdingTax2Pages : CBaseReport
    {
        public CFormNonResidentWithholdingTax2Pages() : base()
        {
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr = createPageParamEasy((dataSource as MAccountDoc).AccountItem, itemPerPage);

            String isCopyReport = rptCfg.GetConfigValue("IsCopyReport");
            int howmanyIssue = CUtil.StringToInt(rptCfg.GetConfigValue("HowManyIssue"));

            howmanyIssue = 4;

            ArrayList accums = new ArrayList();
            for (int i = 1; i <= 2; i++)
            {
                foreach (CReportPageParam prm in arr)
                {
                    accums.Add(prm);
                }
            }

            return (accums);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormNoResidentWithholdingTax2Pages page = new UFormNoResidentWithholdingTax2Pages(dataSource, pageIdx, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }        

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("FontSize", "10", "double", "Font Size");
            rc.SetConfigValue("FontName", "AngsanaUPC", "String", "Font Name");
            rc.SetConfigValue("LineWidth", "300", "double", "Line Length");
            rc.SetConfigValue("CustomerBoxWidth", "450", "double", "Customer box Length");
            rc.SetConfigValue("AddressBoxWidth", "450", "double", "Address box Length");
            rc.SetConfigValue("Language", "TH", "String", "Language");

            rc.SetConfigValue("ItemPerPage", "22", "int", "Item per pages");

            rc.SetConfigValue("HeaderTextSize", "10", "int", "Table header text size");
            rc.SetConfigValue("TopHeaderTextSize", "10", "int", "Top most header text size");
            rc.SetConfigValue("BottomLeftBoxWidth", "250", "int", "Bottom Left Box Width");
            rc.SetConfigValue("BottomBoxHeight", "95", "int", "Bottom Box Height");

            //Custom A4 form Height="29.7cm" Width="21cm"
            rc.SetConfigValue("PageWidthCm", "29.7", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "21", "double", "Page Height (CM)");

            rc.SetConfigValue("MarginLeftCm", "0.30", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0.30", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0.30", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0.30", "double", "Margin Bottom (CM)");

            rc.SetConfigValue("IsCopyReport", "N", "String", "Is Copy Report?");
            rc.SetConfigValue("HowManyIssue", "4", "int", "How many issue?");
            rc.SetConfigValue("Issue1", "", "String", "ฉบับที่ 1 (สำหรับผู้ถูกหัก ณ ที่จ่าย ใช้แนบพร้อมกับใบแสดงรายการภาษี)");
            rc.SetConfigValue("Issue2", "", "String", "Issue 2");
            rc.SetConfigValue("Issue3", "", "String", "Issue 3");
            rc.SetConfigValue("Issue4", "", "String", "Issue 4");
            rc.SetConfigValue("DisplayNamePrefixFlag", "Y", "String", "Y - to show company prefix, N - not show");

            return (rc);
        }
    }
}
