using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.PurchaseOrder
{
    public class CFormPurchaseOrder : CBaseReport
    {
        public CFormPurchaseOrder() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormPurchaseOrder page = new UFormPurchaseOrder(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr = createPageParamComplex((dataSource as MAuxilaryDoc).AuxilaryDocItems, itemPerPage);
            //ArrayList arr = createPageParamEasy((dataSource as MAuxilaryDoc).AuxilaryDocItems, itemPerPage);

            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบสั่งซื้อ / PURCHASE ORDER", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "PURCHASE ORDER", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
