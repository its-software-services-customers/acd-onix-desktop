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

namespace Onix.ClientCenter.Forms.AcDesign.Cheque
{
    public class CFormChequePay : CBaseReport
    {
        public CFormChequePay() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormChequePayment page = new UFormChequePayment(dataSource, pageIdx, pageCount, rptCfg, param);

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
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ObservableCollection<MAccountDocItem> dummyItems = new ObservableCollection<MAccountDocItem>();
            dummyItems.Add(new MAccountDocItem(new CTable("")));

            ArrayList arr = createPageParamEasy(dummyItems, itemPerPage);
            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("FontSize", "16", "double", "Font Size");
            rc.SetConfigValue("FontName", "AngsanaUPC", "String", "Font Name");       
            rc.SetConfigValue("Language", "TH", "String", "Language");

            //Custom Cheque form Height="9cm" Width="17.5cm" Tanachat Bank Cheque
            rc.SetConfigValue("PageWidthCm", "18", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "9", "double", "Page Height (CM)");

            rc.SetConfigValue("MarginLeftCm", "0", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0", "double", "Margin Bottom (CM)");

            rc.SetConfigValue("PositionChequeDateFmt", "500,10", "String", "ChequeDateFmt Position");
            rc.SetConfigValue("PositionCompany", "100,10", "String", "Company Position");
            rc.SetConfigValue("PositionPayeeName", "100,70", "String", "PayeeName Position");
            rc.SetConfigValue("PositionNumberAsText", "120,100", "String", "NumberAsText Position");
            rc.SetConfigValue("PositionChequeAmountFmt", "500,120", "String", "ChequeAmountFmt Position");
            rc.SetConfigValue("PositionAcPayeeOnly", "80,40", "String", "AcPayeeOnly Position");
            rc.SetConfigValue("PositionHolderMask", "300,70", "String", "Holder Mask Position");
            rc.SetConfigValue("MaskingText", "######", "String", "Masking Text");

            rc.SetConfigValue("IsAcPayeeOnlyText", "A/C PAYEE ONLY", "String", "IsAcPayeeOnly Text");

            return (rc);
        }
    }
}
