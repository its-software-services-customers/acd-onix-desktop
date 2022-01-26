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

namespace Onix.ClientCenter.Forms.AcDesign.HRPayrollSlip
{
    public class CFormPayrollSlip : CBaseReport
    {
        public CFormPayrollSlip() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormPayrollSlip page = new UFormPayrollSlip(dataSource, pageIdx, pageCount, rptCfg, param);

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

            rc.SetConfigValue("DocumentTypeThai", "ใบแจ้งเงินเดือน", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "Payroll Slip", "String", "Document Type Eng");

            rc.SetConfigValue("Filler1Height", "10", "String", "Heigh of top space filler");
            rc.SetConfigValue("Filler2Height", "10", "String", "Heigh of area above tax ID");
            rc.SetConfigValue("Filler3Height", "0", "String", "Heigh of area below tax ID");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
