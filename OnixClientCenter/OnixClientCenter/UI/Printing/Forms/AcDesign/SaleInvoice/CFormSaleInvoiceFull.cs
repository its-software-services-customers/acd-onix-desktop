using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.SaleInvoice
{
    public class CFormSaleInvoiceFull : CBaseReport
    {
        public CFormSaleInvoiceFull() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormSaleInvoiceFull page = new UFormSaleInvoiceFull(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            //ArrayList arr = createPageParamEasy((dataSource as MAccountDoc).AccountItem, itemPerPage);
            ArrayList arr = createPageParamComplex((dataSource as MAccountDoc).AccountItem, itemPerPage);

            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }        
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบแจ้งหนี้/ใบกำกับภาษี", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "DELIVERY ORDER/TAX INVOICE", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
