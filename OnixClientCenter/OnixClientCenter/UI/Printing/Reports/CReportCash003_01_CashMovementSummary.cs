using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Wis.WsClientAPI;
using System.IO;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Report;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
	public class CReportCash003_01_CashMovementSummary : CBaseReport
	{
		private Hashtable rowdef = new Hashtable();
		private MemoryStream stream = new MemoryStream();
        private double[] widths4Col = new double[4] {25, 25, 25, 25 };
        private double[] widths5Col = new double[5] {10, 15, 25, 25, 25};
		private double[] totals = new double[5] { 0, 0, 0, 0, 0,};
        private int rowNo = 0;
        String Account, H = "", D = "";
	
        public CReportCash003_01_CashMovementSummary() : base()
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
			//Size areaSize = new Size(PageSize.Width - (0.5 * 96 * 2), PageSize.Height - (0.5 * 96 * 2));
			//Rect recArea = new Rect(new Point(0.5 * 96, 0.5 * 96), areaSize);

			//UReportPage page = (UReportPage) pages[pageNumber];
			//DocumentPage dc = new DocumentPage(page, PageSize, recArea, recArea);

			return (null);
		}

        protected override void createRowTemplates()
		{
			String nm = "";
			Thickness defMargin = new Thickness(3, 1, 3, 1);

			nm = "HEADER_LEVEL1";
			CRow h1 = new CRow(nm, 30, 4, defMargin);
            h1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = h1;

			CColumn c2_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0), new GridLength(widths4Col[0], GridUnitType.Star));
            h1.AddColumn(c2_0);

			CColumn c2_1_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0), new GridLength(widths4Col[1], GridUnitType.Star));
            h1.AddColumn(c2_1_0);

            CColumn c2_1_1 = new CColumn(new Thickness(0.5, 0.5, 0, 0), new GridLength(widths4Col[2], GridUnitType.Star));
            h1.AddColumn(c2_1_1);

            CColumn c2_1_2 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0), new GridLength(widths4Col[3], GridUnitType.Star));
            h1.AddColumn(c2_1_2);


			nm = "HEADER_LEVEL2";
			CRow h2 = new CRow(nm, 30, 5, defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = h2;

			CColumn c3_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths5Col[0], GridUnitType.Star));
            h2.AddColumn(c3_0);

			CColumn c3_1 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths5Col[1], GridUnitType.Star));
            h2.AddColumn(c3_1);

			CColumn c3_2 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths5Col[2], GridUnitType.Star));
            h2.AddColumn(c3_2);

			CColumn c3_3 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths5Col[3], GridUnitType.Star));
            h2.AddColumn(c3_3);

			CColumn c3_4 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths5Col[4], GridUnitType.Star));
            h2.AddColumn(c3_4);

			//CColumn c3_5 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths7Col[5], GridUnitType.Star));
   //         h2.AddColumn(c3_5);

			//CColumn c3_6 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths7Col[6], GridUnitType.Star));
   //         h2.AddColumn(c3_6);


            nm = "DATA_LEVEL1";
            CRow r1 = new CRow(nm, 30, 4, defMargin);
            r1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = r1;

            CColumn r1_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths4Col[0], GridUnitType.Star));
            r1_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
            r1.AddColumn(r1_c0);

            CColumn r1_c1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths4Col[1], GridUnitType.Star));
            r1_c1.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c1);

            CColumn r1_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths4Col[2], GridUnitType.Star));
            r1_c2.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c2);

            CColumn r1_c3 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths4Col[3], GridUnitType.Star));
            r1_c3.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c3);


            nm = "DATA_LEVEL2";
			CRow r0 = new CRow(nm, 30, 5, defMargin);
			r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = r0;

			CColumn r0_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[0], GridUnitType.Star));
			r0_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r0.AddColumn(r0_c0);

			CColumn r0_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[1], GridUnitType.Star));
			r0_c1_0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r0.AddColumn(r0_c1_0);

			CColumn r0_c1_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[2], GridUnitType.Star));
			r0_c1_1.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c1_1);

			CColumn r0_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[3], GridUnitType.Star));
			r0_c2.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c2);

			CColumn r0_c3 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths5Col[4], GridUnitType.Star));
			r0_c3.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c3);

			//CColumn r0_c4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths7Col[5], GridUnitType.Star));
			//r0_c4.SetHorizontalAlignment(HorizontalAlignment.Right);
			//r0.AddColumn(r0_c4);

			//CColumn r0_c5 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths7Col[6], GridUnitType.Star));
			//r0_c5.SetHorizontalAlignment(HorizontalAlignment.Left);
			//r0.AddColumn(r0_c5);


			nm = "DATA_LEVEL3";
			CRow r2 = new CRow(nm, 30, 5, defMargin);
			r2.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
			rowdef[nm] = r2;

			CColumn r2_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[0], GridUnitType.Star));
			r2_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r2.AddColumn(r2_c0);

			CColumn r2_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[1], GridUnitType.Star));
			r2_c1_0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r2.AddColumn(r2_c1_0);

			CColumn r2_c1_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[2], GridUnitType.Star));
			r2_c1_1.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c1_1);

			CColumn r2_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[3], GridUnitType.Star));
			r2_c2.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c2);

			CColumn r2_c3 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths5Col[4], GridUnitType.Star));
			r2_c3.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c3);

			//CColumn r2_c4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths7Col[5], GridUnitType.Star));
			//r2_c4.SetHorizontalAlignment(HorizontalAlignment.Right);
			//r2.AddColumn(r2_c4);

			//CColumn r2_c5 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths7Col[6], GridUnitType.Star));
			//r2_c5.SetHorizontalAlignment(HorizontalAlignment.Left);
			//r2.AddColumn(r2_c5);

			nm = "FOOTER_LEVEL1";
			CRow f1 = new CRow(nm, 30, 5, defMargin);
			f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = f1;

			CColumn fc_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[0], GridUnitType.Star));
            f1.AddColumn(fc_0);

			CColumn fc_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[1], GridUnitType.Star));
			f1.AddColumn(fc_1);

			CColumn fc_2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[2], GridUnitType.Star));
            fc_2.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_2);

			CColumn fc_3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths5Col[3], GridUnitType.Star));
			fc_3.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_3);

			CColumn fc_4 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths5Col[4], GridUnitType.Star));
			fc_4.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_4);

			//CColumn fc_5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths7Col[5], GridUnitType.Star));
			//fc_5.SetHorizontalAlignment(HorizontalAlignment.Right);
			//f1.AddColumn(fc_5);

			//CColumn fc_6 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths7Col[6], GridUnitType.Star));
			//fc_6.SetHorizontalAlignment(HorizontalAlignment.Right);
			//f1.AddColumn(fc_6);
		}

		private void createDataHeaderRow1(UReportPage page)
		{
			CRow r = (CRow)rowdef["HEADER_LEVEL1"];
			r.FillColumnsText(
				CLanguage.getValue("AccNo"),
				CLanguage.getValue("AccName"),
				CLanguage.getValue("Bank"),
                CLanguage.getValue("Branch"));

			ConstructUIRow(page, r);
			AvailableSpace = AvailableSpace - r.GetHeight();
		}

		private void createDataHeaderRow2(UReportPage page)
		{
			CRow r = (CRow)rowdef["HEADER_LEVEL2"];
            r.FillColumnsText(
                CLanguage.getValue("number"),
                CLanguage.getValue("DocuDate"),
                CLanguage.getValue("in_quantity"),
                CLanguage.getValue("out_quantity"),
                CLanguage.getValue("balance_quantity"));

			ConstructUIRow(page, r);
			AvailableSpace = AvailableSpace - r.GetHeight();
		}

        protected override UReportPage initNewArea(Size areaSize)
		{
			UReportPage page = new UReportPage();

			CreateGlobalHeaderRow(page);
			createDataHeaderRow1(page);
			createDataHeaderRow2(page);

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

            CRow r1 = (CRow)rowdef["DATA_LEVEL2"];
			CRow nr1 = r1.Clone();

			CRow r2 = (CRow)rowdef["DATA_LEVEL3"];
			CRow nr2 = r2.Clone();

			CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
			CRow ftr = ft.Clone();
            CRow ftrLast = ft.Clone();

            double newh = AvailableSpace;
            int temp = 0;

            MCashDoc v = new MCashDoc(o);
			D = v.AccountNo;

			if (!H.Equals(D))
			{
                //v.IsBalanceForward = true;
                nr.FillColumnsText(v.AccountNo, v.AccountName, v.BankName, v.BankBranchName);
                nr1.FillColumnsText("",
                    CLanguage.getValue("balance_forward"),
                    "","",
                    CUtil.FormatNumber(v.BeginAmount_BalanceeFmt, "-"));

                Account = v.AccountNo;
               // v.IsBalanceForward = false;

                if ((!H.Equals("")) && (!H.Equals(D)))
				{
					ftr.FillColumnsText("", 
                        CLanguage.getValue("total"), 
                        CUtil.FormatNumber(totals[0].ToString(),"-"),
						CUtil.FormatNumber(totals[1].ToString(),"-"),
                        CUtil.FormatNumber(totals[2].ToString(), "-"));

                    newh = newh - ftr.GetHeight();
                    if (newh > 0 )
                    {
                        rpp.AddReportRow(ftr);

                        newh = newh - nr.GetHeight() - nr1.GetHeight();
                        if (newh > 0)
                        {
                            rpp.AddReportRow(nr);
                            rpp.AddReportRow(nr1);
                            temp++;

                            H = Account;

                            totals[0] = 0;
                            totals[1] = 0;
                            totals[2] = 0;

                            rowNo = 0;
                        }
                    } 
                }
				else
				{
                    newh = newh - nr.GetHeight() - nr1.GetHeight();
                    if (newh > 0)
                    {
                        rpp.AddReportRow(nr);
                        rpp.AddReportRow(nr1);
                        temp++;
                    }
				}
			}

            if (newh > 0)
            {
                newh = newh - nr2.GetHeight();
                if (newh > 0)
                {
                    rowNo++;
                    nr2.FillColumnsText(
                        CUtil.FormatInt(rowNo.ToString()),
                        v.DocumentDateFmt,
                        CUtil.FormatNumber(v.InAmount_BalnceFmt, "-"),
                        CUtil.FormatNumber(v.OutAmount_BalanceFmt, "-"),
                        CUtil.FormatNumber(v.EndAmount_BalanceFmt, "-"));

                    totals[0] += CUtil.StringToDouble(v.InAmount_BalnceFmt);
                    totals[1] += CUtil.StringToDouble(v.OutAmount_BalanceFmt);
                    totals[2] = CUtil.StringToDouble(v.EndAmount_BalanceFmt);
                    rpp.AddReportRow(nr2);
                }
            }

            if (newh > 0)
            {
                if (row == rowcount - 1) 
                {
                    ftrLast.FillColumnsText("", 
                        CLanguage.getValue("total"), 
                        CUtil.FormatNumber(totals[0].ToString(), "-"), 
                        CUtil.FormatNumber(totals[1].ToString(), "-"), 
                        CUtil.FormatNumber(totals[2].ToString(), "-"));

                    newh = newh - ftrLast.GetHeight();
                    if (newh > 0)
                    {
                        rpp.AddReportRow(ftrLast);

                        rowNo = 0;

                        totals[0] = 0;
                        totals[1] = 0;
                        totals[2] = 0;
                    }
                }

                Account = v.AccountNo;
               
                H = Account;
            }

			if (newh < 0)
			{
				rpp.IsNewPageRequired = true;
                rpp.TempNotRowDetails = temp;
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

			ArrayList arr = OnixWebServiceAPI.GetCashBalanceSummaryList(Parameter);

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
				CTable o = (CTable)arr[i];

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
                    if (property.TempNotRowDetails > 0)
                    {
                        ConstructUIRows(area, property); //add 6/6/2017 for case  first footer on new page
                    }
                       
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

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DOCUMENT_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DOCUMENT_DATE");
            entries.Add(entry);

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

