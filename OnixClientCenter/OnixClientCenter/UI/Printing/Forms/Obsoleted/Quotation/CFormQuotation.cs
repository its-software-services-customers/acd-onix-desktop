﻿using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Model;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.Collections.ObjectModel;
using System.Printing;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Forms.Quotation
{
    public class CFormQuotation : CBaseReport
    {
        public CFormQuotation() : base()
        {
        }

        public override int PageCount
        {
            get
            {
                return (pages.Count);
            }
        }

        public override Size PageSize
        {
            get
            {
                Size s = new Size(rptCfg.PageWidthDot, rptCfg.PageHeightDot);
                return (s);
            }
        }

        public override PageOrientation GetPageOrientation()
        {
            return (PageOrientation.Portrait);
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPaginator dpg = keepFixedDoc.DocumentPaginator;
            DocumentPage dp = dpg.GetPage(pageNumber);
            return (dp);
        }

        public override MBaseModel CreateDefaultData()
        {
            MAuxilaryDoc ma = new MAuxilaryDoc(new CTable(""));
            return (ma);
        }

        private int getPageCount()
        {
            int itemPerPage = CUtil.StringToInt(rptCfg.GetConfigValue("ItemPerPage"));
            ObservableCollection<MAuxilaryDocItem> arr = (dataSource as MAuxilaryDoc).AuxilaryDocItems;
            int arrcnt = 0;
            foreach (MAuxilaryDocItem mi in arr)
            {
                if (!mi.ExtFlag.Equals("D"))
                {
                    arrcnt++;
                }
            }

            int pcnt = arrcnt / itemPerPage;
            int remainder = arrcnt % itemPerPage;

            if (pcnt == 0)
            {
                return (1);
            }

            if (remainder == 0)
            {
                return (pcnt);
            }

            return (pcnt + 1);
        }

        private int getRemarkCount()
        {
            ObservableCollection<MAuxilaryDocRemark> arr = (dataSource as MAuxilaryDoc).Remarks;
            int arrcnt = 0;
            foreach (MAuxilaryDocRemark mi in arr)
            {
                if (!mi.ExtFlag.Equals("D"))
                {
                    arrcnt++;
                }
            }
            if(arrcnt <= 4)
            {
                arrcnt = 0;
            }
            else
            {
                arrcnt = arrcnt - 4;
            }
            return (arrcnt);
        }

        public override FixedDocument CreateFixedDocument()
        {
            int pc = getPageCount();

            int rowRemark = getRemarkCount();

            FixedDocument fd = new FixedDocument();
            Size s = new Size(rptCfg.PageWidthDot, rptCfg.PageHeightDot);

            for (int i = 1; i <= pc; i++)
            {
                UFormQuotation page = new UFormQuotation(dataSource, i, pc, rowRemark, rptCfg);
                page.Width = rptCfg.AreaWidthDot;
                page.Height = rptCfg.AreaHeightDot;
                page.Measure(s);

                if (i == 1)
                {
                    fd.DocumentPaginator.PageSize = s;
                }

                pages.Add(page);

                FixedPage fp = new FixedPage();
                fp.Margin = new Thickness(rptCfg.MarginLeftDot, rptCfg.MarginTopDot, rptCfg.MarginRightDot, rptCfg.MarginBottomDot);

                PageContent pageContent = new PageContent();
                ((System.Windows.Markup.IAddChild)pageContent).AddChild(fp);

                fd.Pages.Add(pageContent);
                fp.Children.Add(page);
                //double a = page.AreaHeight;
            }

            keepFixedDoc = fd;
            return (fd);
        }

        public override FixedDocument GetFixedDocument()
        {
            return (keepFixedDoc);
        }

        public override MReportConfig CreateDefaultConfigValues()
        {
            MReportConfig rc = new MReportConfig(new Wis.WsClientAPI.CTable(""));

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
            rc.SetConfigValue("PageWidthCm", "21", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "29.7", "double", "Page Height (CM)");

            rc.SetConfigValue("MarginLeftCm", "0.54", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0.54", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0.54", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0.54", "double", "Margin Bottom (CM)");

            return (rc);
        }
    }
}
