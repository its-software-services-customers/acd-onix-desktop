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

namespace Onix.ClientCenter.Forms.AcDesign.SaleOrder
{
    public class CFormSaleOrder : CBaseReport
    {
        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormSaleOrder page = new UFormSaleOrder(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));            

            ArrayList arr = createPageParamComplex((dataSource as MAccountDoc).AccountItem, itemPerPage);
            //ArrayList arr = createPageParamEasy((dataSource as MAccountDoc).AccountItem, itemPerPage);

            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }       

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบสั่งขาย", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "SALE ORDER", "String", "Document Type Eng");
           
            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
