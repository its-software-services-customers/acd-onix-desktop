using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.OfficialReceipt
{
    public class CFormOfficialReceipt : CBaseReport
    {
        public CFormOfficialReceipt() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormOfficialReceipt page = new UFormOfficialReceipt(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));
            itemPropertyName = "DocumentNo";

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

            rc.SetConfigValue("DocumentTypeThai", "ใบเสร็จรับเงิน", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "OFFICIAL RECEIPT", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
