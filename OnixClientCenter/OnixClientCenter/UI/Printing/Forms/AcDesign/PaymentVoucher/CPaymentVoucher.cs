using System;
using System.Windows;
using System.Collections;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.Client.Helper;
using System.Windows.Controls;

namespace Onix.ClientCenter.Forms.AcDesign.PaymentVoucher
{
    public class CPaymentVoucher : CBaseReport
    {
        protected override UserControl createPageObject(Size s, int pageIdx, int pageCount, CReportPageParam param)
        {
            UPaymentVoucher page = new UPaymentVoucher(dataSource, pageIdx, pageCount, rptCfg, param);

            page.Width = rptCfg.AreaWidthDot;
            page.Height = rptCfg.AreaHeightDot;
            page.Measure(s);

            return (page);
        }

        protected override ArrayList createPageParam()
        {
            MAccountDoc ad = (MAccountDoc)dataSource;
            AccountDocumentType dt = (AccountDocumentType)CUtil.StringToInt(ad.DocumentType);

            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));

            ArrayList arr1 = null;
            if ((dt == AccountDocumentType.AcctDocApReceipt) || (dt == AccountDocumentType.AcctDocArReceipt))
            {
                //arr1 = createPageParamEasy(ad.ReceiptItems, itemPerPage);
                arr1 = createPageParamComplex(ad.ReceiptItems, itemPerPage);
            }
            else
            {
                //arr1 = createPageParamEasy(ad.AccountPoItems, itemPerPage);
                arr1 = createPageParamComplex(ad.AccountPoItems, itemPerPage);
            }

            return (arr1);
        }

        protected override void initPageCreateFlow()
        {
            IsPageRangeSupport = true;

            MAccountDoc ad = (MAccountDoc)dataSource;
            ad.InitAccountPoItems();
        }        

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

            rc.SetConfigValue("DocumentTypeThai", "ใบสำคัญจ่าย", "String", "Document Type Thai");
            rc.SetConfigValue("DocumentTypeEng", "PAYMENT VOUCHER", "String", "Document Type Eng");

            populateDefaultReportConfig(rc);

            return (rc);
        }
    }
}
