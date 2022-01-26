using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.Quotation
{
    public class CFormQuotationAttachSheet : CBaseReport
    {
        public CFormQuotationAttachSheet() : base()
        {
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr = createPageParamEasy((dataSource as MAuxilaryDoc).AttachItems, itemPerPage);
            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
            (dataSource as MAuxilaryDoc).InitAttachItems();
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormQuotationAttachSheet page = new UFormQuotationAttachSheet(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ATTACH SHEET", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "QUOTATION", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
