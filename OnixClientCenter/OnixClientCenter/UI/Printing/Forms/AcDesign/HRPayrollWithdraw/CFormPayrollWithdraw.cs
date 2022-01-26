using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Printing;
using Onix.Client.Helper;
using System.Windows.Controls;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Forms.AcDesign.HRPayrollWithdraw
{
    public class CFormPayrollWithdraw : CBaseReport
    {
        public CFormPayrollWithdraw() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormPayrollWithdraw page = new UFormPayrollWithdraw(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        public override PageOrientation GetPageOrientation()
        {
            return (PageOrientation.Portrait);
        }

        protected override ArrayList createPageParam()
        {
            MVPayrollDocument ad = (MVPayrollDocument)dataSource;

            ArrayList arr = createPageParamEasy(ad.PayrollItems, 1);
            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบถอนเงินเพื่อฝากเข้าบัญชีเงินเดือน", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "Payroll Withdraw Slip", "String", "Document Type Eng");

            rc.SetConfigValue("FontSize", "10", "double", "Font Size");
            rc.SetConfigValue("FontName", "AngsanaUPC", "String", "Font Name");

            rc.SetConfigValue("PageWidthCm", "20.70", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "11.20", "double", "Page Height (CM)");
            rc.SetConfigValue("MarginLeftCm", "0.00", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0.00", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0.00", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0.00", "double", "Margin Bottom (CM)");
            rc.SetConfigValue("Language", "TH", "String", "Language");

            rc.SetConfigValue("Filler1Height", "10", "String", "");
            rc.SetConfigValue("Filler2Height", "20", "String", "");
            rc.SetConfigValue("Filler3Height", "50", "String", "");
            rc.SetConfigValue("Filler4Height", "20", "String", "");
            rc.SetConfigValue("Filler5Height", "60", "String", "");

            rc.SetConfigValue("DisplayGridFlag", "Y", "String", "Y=Display, N=Not display");

            rc.SetConfigValue("Format1", "150|50|50|50|50|50|50|50|50|50|50|50|50|50|*", "String", "");
            rc.SetConfigValue("Format2", "150|50|50|*", "String", "");
            rc.SetConfigValue("BankBranchText", "", "String", "Any text you want");

            return (rc);
        }
    }
}
