using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Model;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.IO;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;


namespace Onix.ClientCenter.Reports
{
    public class CReportCash001_01_CashBalance : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private MemoryStream stream = new MemoryStream();
        private double[] widths = new double[6] { 5, 15, 30, 15, 25, 10 };
        private double[] totals = new double[6] { 0, 0, 0, 0, 0, 0};

        public CReportCash001_01_CashBalance() : base()
        {

        }

        public override int PageCount
        {
            get
            {
                return (pages.Count);
            }
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPaginator dpg = keepFixedDoc.DocumentPaginator;
            DocumentPage dp = dpg.GetPage(pageNumber);
            return (dp);
        }

        protected override void createRowTemplates()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            nm = "HEADER_LEVEL1";
            CRow h2 = new CRow(nm, 30, 6, defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h2;

            CColumn c2_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            h2.AddColumn(c2_0);

            CColumn c2_1_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            h2.AddColumn(c2_1_0);

            CColumn c2_1_1 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
            h2.AddColumn(c2_1_1);

            CColumn c2_2 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            h2.AddColumn(c2_2);

            CColumn c2_3 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            h2.AddColumn(c2_3);

            CColumn c2_4 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths[5], GridUnitType.Star));
            h2.AddColumn(c2_4);


            nm = "DATA_LEVEL1";
            CRow r0 = new CRow(nm, 30, 6, defMargin);
            r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r0;


            CColumn r0_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            r0_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
            r0.AddColumn(r0_c0);

            CColumn r0_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            r0_c1_0.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c1_0);

            CColumn r0_c1_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
            r0_c1_1.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c1_1);

            CColumn r0_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            r0_c2.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c2);

            CColumn r0_c3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            r0_c3.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c3);

            CColumn r0_c4 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[5], GridUnitType.Star));
            r0_c4.SetHorizontalAlignment(HorizontalAlignment.Right);
            r0.AddColumn(r0_c4);


            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, 30, 6, defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            CColumn fc_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            f1.AddColumn(fc_0);

            CColumn fc_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            f1.AddColumn(fc_1);

            CColumn fc_2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
            f1.AddColumn(fc_2);

            CColumn fc_3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            f1.AddColumn(fc_3);

            CColumn fc_4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            fc_4.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_4);

            CColumn fc_5 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[5], GridUnitType.Star));
            fc_5.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_5);
        }

        private void createDataHeaderRow(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];
            r.FillColumnsText(
                "NO.",
                CLanguage.getValue("AccNo"),
                CLanguage.getValue("AccName"),
                CLanguage.getValue("Bank"),
                CLanguage.getValue("Branch"),
                CLanguage.getValue("money_quantity"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        protected override UReportPage initNewArea(Size areaSize)
        {
            UReportPage page = new UReportPage();

            CreateGlobalHeaderRow(page);
            createDataHeaderRow(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;
            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();
            
            MCashAccount v = new MCashAccount(o);

            double newh = AvailableSpace - nr.GetHeight();

                nr.FillColumnsText((row + 1).ToString(), v.AccountNo, v.AccountName, v.BankName, v.BankBranchName,
                    v.TotalAmountFmt);

                totals[5] = totals[5] + CUtil.StringToDouble(v.TotalAmount);

            rpp.AddReportRow(nr);

            if (row == rowcount - 1)
            {
                //End row
                CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
                CRow ftr = ft.Clone();

                ftr.FillColumnsText(CLanguage.getValue("total"), "", "", "", "", CUtil.FormatNumber(totals[5].ToString()));

                rpp.AddReportRow(ftr);
                newh = AvailableSpace - ftr.GetHeight();
            }

            if (newh < 0)
            {
                rpp.IsNewPageRequired = true;
            }
            else
            {
                AvailableSpace = newh;
            }

            return (rpp);
        }

        public override FixedDocument CreateFixedDocument()
        {
            FixedDocument fd = new FixedDocument();
            
            ReportProgressUpdate updateFunc = GetProgressUpdateFunc();
            ReportStatusUpdate doneFunc = GetProgressDoneFunc();

            fd.DocumentPaginator.PageSize = PageSize;
                        
            if (doneFunc != null)
            {
                doneFunc(false, false);
            }

            ArrayList arr = OnixWebServiceAPI.GetCashAccountList(Parameter);

            if (arr == null)
            {
                return (fd);
            }

            int cnt = arr.Count;
            UReportPage area = null;            

            createRowTemplates();
            int i = 0;

            Size areaSize = GetAreaSize();
            AvailableSpace = areaSize.Height;

            CReportDataProcessingProperty property = null;
            while (i < arr.Count)
            {
                CTable o = (CTable) arr[i];
                                
                if ((i == 0) || (property.IsNewPageRequired))
                {
                    AvailableSpace = areaSize.Height;

                    CurrentPage++;

                    FixedPage fp = new FixedPage();
                    fp.Margin = Margin;

                    PageContent pageContent = new PageContent();
                    ((System.Windows.Markup.IAddChild)pageContent).AddChild(fp);

                    fd.Pages.Add(pageContent);
                    area = initNewArea(areaSize);

                    pages.Add(area);
                    fp.Children.Add(area);                    
                }

                property = DataToProcessingProperty(o, arr, i);
                if (property.IsNewPageRequired)
                {
                    //Do not create row if that row caused new page flow
                    //But create it in the next page instead 
                    i--;
                }
                else
                {
                    ConstructUIRows(area, property);
                }

                if (updateFunc != null)
                {
                    updateFunc(i, cnt);
                }

                i++;
            }

            if (doneFunc != null)
            {
                doneFunc(true, false);
            }

            keepFixedDoc = fd;
            return (fd);
        }

        public override FixedDocument GetFixedDocument()
        {
            return (keepFixedDoc);
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("AccNo", EntryType.ENTRY_TEXT_BOX, 300, true, "ACCOUNT_NO");
            entries.Add(entry);

            entry = new CEntry("AccName", EntryType.ENTRY_TEXT_BOX, 300, true, "ACCOUNT_NNAME");
            entries.Add(entry);

            entry = new CEntry("Bank", EntryType.ENTRY_TEXT_BOX, 300, true, "BANK_NAME");
            entries.Add(entry);
            return (entries);
        }
    }
}
