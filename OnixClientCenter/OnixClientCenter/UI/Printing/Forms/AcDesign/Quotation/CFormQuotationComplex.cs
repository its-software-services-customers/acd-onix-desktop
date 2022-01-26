using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.Quotation
{
    public class CFormQuotationComplex : CBaseReport
    {
        public CFormQuotationComplex() : base()
        {
        }

        protected override ArrayList createPageParam()
        {           
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr = createPageParamComplex((dataSource as MAuxilaryDoc).AuxilaryDocItems, itemPerPage);
            //ArrayList arr = createPageParamEasy((dataSource as MAuxilaryDoc).AuxilaryDocItems, itemPerPage);
            return (arr);
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormQuotationComplex page = new UFormQuotationComplex(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "QUOTATION", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "QUOTATION", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
