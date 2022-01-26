using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.PurchaseOrder
{
    public class CFormPurchaseOrderService : CBaseReport
    {
        public CFormPurchaseOrderService() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormPurchaseOrderService page = new UFormPurchaseOrderService(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);
            
            page.DisplayQR();

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr = createPageParamEasy((dataSource as MAuxilaryDoc).PaymentCriteriaes, itemPerPage);
            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบเบิกจ่าย ค่าของ ค่าแรง", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "EXPENSE CLAIM FORM", "String", "Document Type Eng");

            rc.SetConfigValue("FontSize", "18", "double", "Font Size");
            rc.SetConfigValue("FontName", "AngsanaUPC", "String", "Font Name");
            rc.SetConfigValue("LineWidth", "300", "double", "Line Length");
            rc.SetConfigValue("CustomerBoxWidth", "450", "double", "Customer box Length");
            rc.SetConfigValue("AddressBoxWidth", "450", "double", "Address box Length");
            rc.SetConfigValue("Language", "TH", "String", "Language");
            rc.SetConfigValue("DisplayLogoFlag", "Y", "Boolean", "Y=Show, N=Hide");
            rc.SetConfigValue("DisplayNamePrefixFlag", "Y", "Boolean", "Y=Show, N=Hide");
            rc.SetConfigValue("DisplayItemCodeFlag", "N", "Boolean", "Y=Show, N=Hide");
            rc.SetConfigValue("DisplayBranchFlag", "N", "Boolean", "Y=Show, N=Hide");
            rc.SetConfigValue("DisplayShadowFlag", "Y", "Boolean", "Y=Show, N=Hide");

            rc.SetConfigValue("ItemPerPage", "22", "int", "Item per pages");

            //Custom A4 form Height="29.7cm" Width="21cm"
            rc.SetConfigValue("PageWidthCm", "29.7", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "21", "double", "Page Height (CM)");

            rc.SetConfigValue("MarginLeftCm", "0.54", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0.54", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0.54", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0.54", "double", "Margin Bottom (CM)");

            return (rc);
        }
    }
}
