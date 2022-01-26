using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.BillSummary
{
    public class CFormBillSummary : CBaseReport
    {
        public CFormBillSummary() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormBillSummary page = new UFormBillSummary(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));
            itemPropertyName = "DocumentDesc";

            //ArrayList arr = createPageParamEasy((dataSource as MAccountDoc).ReceiptItems, itemPerPage);
            ArrayList arr = createPageParamComplex((dataSource as MAccountDoc).ReceiptItems, itemPerPage);

            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }        
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบวางบิล", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "BILL SUMMARY", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
