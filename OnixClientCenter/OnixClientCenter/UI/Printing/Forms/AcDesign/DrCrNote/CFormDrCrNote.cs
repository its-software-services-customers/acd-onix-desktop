using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.DrCrNote
{
    public class CFormDrCrNote : CBaseReport
    {
        public CFormDrCrNote() : base()
        {
        }

        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UFormDrCrNote page = new UFormDrCrNote(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));
            MAccountDoc ad = dataSource as MAccountDoc;
            ad.AccountItem.Clear();

            //ArrayList arr = createPageParamEasy((dataSource as MAccountDoc).AccountItem, itemPerPage);

            MAccountDocItem mi = new MAccountDocItem(new Wis.WsClientAPI.CTable(""));
            mi.SelectType = "3";
            mi.FreeText = ad.RefDocNo;
            mi.Quantity = "1";
            mi.UnitPrice = ad.RevenueExpenseAmt;
            mi.TotalAfterDiscount = ad.RevenueExpenseAmt;
            ad.AccountItem.Add(mi);

            ArrayList arr = createPageParamComplex(ad.AccountItem, itemPerPage);

            return (arr);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;
        }
        
        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบลดหนี้/DEBIT NOTE", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "DEBIT NOTE", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
