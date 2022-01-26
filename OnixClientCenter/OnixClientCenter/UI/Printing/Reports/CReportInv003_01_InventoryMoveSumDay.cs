using System;
using System.Windows;
using System.Collections;
using System.Windows.Documents;
using Onix.Client.Report;
using Wis.WsClientAPI;
using System.IO;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
	class CReportInv003_01_InventoryMoveSumDay : CBaseReport
	{

		private Hashtable rowdef = new Hashtable();
		private MemoryStream stream = new MemoryStream();
		private double[] widths = new double[9] {7, 14, 11.5, 11.5, 11.5, 11.5, 11, 11, 11 };
		private double[] widths2 = new double[3] { 21, 46, 33 };
		private double[] widths3 = new double[3] { 21, 46, 33 };
		private double[] totals = new double[7] { 0, 0, 0, 0, 0, 0, 0 };
        private int rowNo = 0;
        String Item, Location, H = "", D = "";

        public CReportInv003_01_InventoryMoveSumDay() : base()
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
			CRow h2 = new CRow(nm, 30, 3, defMargin);
			h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = h2;

			CColumn c2_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths2[0], GridUnitType.Star));
			h2.AddColumn(c2_0);

			CColumn c2_1_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths2[1], GridUnitType.Star));
			h2.AddColumn(c2_1_0);

			CColumn c2_1_1 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths2[2], GridUnitType.Star));
			h2.AddColumn(c2_1_1);


			nm = "HEADER_LEVEL2";
			CRow h3 = new CRow(nm, 30, 9, defMargin);
			h3.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = h3;

			CColumn c3_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
			h3.AddColumn(c3_0);

			CColumn c3_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
			h3.AddColumn(c3_1);

			CColumn c3_2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
			h3.AddColumn(c3_2);

			CColumn c3_3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
			h3.AddColumn(c3_3);

			CColumn c3_4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
			h3.AddColumn(c3_4);

			CColumn c3_5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
			h3.AddColumn(c3_5);

			CColumn c3_6 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
			h3.AddColumn(c3_6);

			CColumn c3_7 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
			h3.AddColumn(c3_7);

			CColumn c3_8 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[8], GridUnitType.Star));
			h3.AddColumn(c3_8);


			nm = "DATA_LEVEL1";
			CRow r0 = new CRow(nm, 30, 9, defMargin);
			r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
			rowdef[nm] = r0;

			CColumn r0_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
			r0_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r0.AddColumn(r0_c0);

			CColumn r0_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
			r0_c1_0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r0.AddColumn(r0_c1_0);

			CColumn r0_c1_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
			r0_c1_1.SetHorizontalAlignment(HorizontalAlignment.Center);
			r0.AddColumn(r0_c1_1);

			CColumn r0_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
			r0_c2.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c2);

			CColumn r0_c3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
			r0_c3.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c3);

			CColumn r0_c4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
			r0_c4.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c4);

			CColumn r0_c5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
			r0_c5.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c5);

			CColumn r0_c6 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
			r0_c6.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c6);

			CColumn r0_c7 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[8], GridUnitType.Star));
			r0_c7.SetHorizontalAlignment(HorizontalAlignment.Right);
			r0.AddColumn(r0_c7);



			nm = "DATA_LEVEL2";
			CRow r1 = new CRow(nm, 30, 3, defMargin);
			r1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = r1;

			CColumn r1_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths3[0], GridUnitType.Star));
			r1_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r1.AddColumn(r1_c0);

			CColumn r1_c1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths3[1], GridUnitType.Star));
			r1_c1.SetHorizontalAlignment(HorizontalAlignment.Left);
			r1.AddColumn(r1_c1);

			CColumn r1_c2 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths3[2], GridUnitType.Star));
			r1_c2.SetHorizontalAlignment(HorizontalAlignment.Left);
			r1.AddColumn(r1_c2);


			nm = "DATA_LEVEL3";
			CRow r2 = new CRow(nm, 30, 9, defMargin);
			r2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = r2;

			CColumn r2_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
			r2_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r2.AddColumn(r2_c0);

			CColumn r2_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
			r2_c1_0.SetHorizontalAlignment(HorizontalAlignment.Center);
			r2.AddColumn(r2_c1_0);

			CColumn r2_c1_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
			r2_c1_1.SetHorizontalAlignment(HorizontalAlignment.Center);
			r2.AddColumn(r2_c1_1);

			CColumn r2_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
			r2_c2.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c2);

			CColumn r2_c3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
			r2_c3.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c3);

			CColumn r2_c4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
			r2_c4.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c4);

			CColumn r2_c5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
			r2_c5.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c5);

			CColumn r2_c6 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
			r2_c6.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c6);

			CColumn r2_c7 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[8], GridUnitType.Star));
			r2_c7.SetHorizontalAlignment(HorizontalAlignment.Right);
			r2.AddColumn(r2_c7);



			nm = "FOOTER_LEVEL1";
			CRow f1 = new CRow(nm, 30, 9, defMargin);
			f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
			rowdef[nm] = f1;

			CColumn fc_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
			f1.AddColumn(fc_0);

			CColumn fc_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
			f1.AddColumn(fc_1);

			CColumn fc_2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
			f1.AddColumn(fc_2);

			CColumn fc_3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
			fc_3.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_3);

			CColumn fc_4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
			fc_4.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_4);

			CColumn fc_5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
			fc_5.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_5);

			CColumn fc_6 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
			fc_6.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_6);

			CColumn fc_7 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
			fc_7.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_7);

			CColumn fc_8 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[8], GridUnitType.Star));
			fc_8.SetHorizontalAlignment(HorizontalAlignment.Right);
			f1.AddColumn(fc_8);

		}

		private void createDataHeaderRow1(UReportPage page)
		{
			CRow r = (CRow)rowdef["HEADER_LEVEL1"];
			r.FillColumnsText(
				CLanguage.getValue("item_code"),
				CLanguage.getValue("item_name_thai"),
				CLanguage.getValue("location_name"));

			ConstructUIRow(page, r);
			AvailableSpace = AvailableSpace - r.GetHeight();
		}

		private void createDataHeaderRow2(UReportPage page)
		{
			CRow r = (CRow)rowdef["HEADER_LEVEL2"];
			r.FillColumnsText(
                CLanguage.getValue("number"),
                CLanguage.getValue("balance_date"),
				CLanguage.getValue("in_quantity"),
                CLanguage.getValue("in_amount"),
                CLanguage.getValue("out_quantity"),
				CLanguage.getValue("out_amount"),
				CLanguage.getValue("left_quantity"),
				CLanguage.getValue("balance_amount"),
				CLanguage.getValue("amount_per_unit"));

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

			CRow r1 = (CRow)rowdef["DATA_LEVEL2"];
			CRow nr1 = r1.Clone();

			CRow r = (CRow)rowdef["DATA_LEVEL1"];
			CRow nr = r.Clone();

			CRow r2 = (CRow)rowdef["DATA_LEVEL3"];
			CRow nr2 = r2.Clone();

			CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
			CRow ftr = ft.Clone();
            CRow ftrLast = ft.Clone();

            MInventoryCurrentBalance v = new MInventoryCurrentBalance(o);

            double newh = AvailableSpace;
            int temp = 0;
            D = v.ItemCode + v.LocationID;

			if (!H.Equals(D))// (Item != v.ItemCode && Location != v.LocationID)
			{

				nr1.FillColumnsText(v.ItemCode, v.ItemName, v.LocationName);
                if (Parameter.GetFieldValue("COSTING_TYPE").Equals("AVG"))
                {
                    nr2.FillColumnsText("", CLanguage.getValue("balance_forward"), "", "", "", "", CUtil.FormatNumber(v.BeginQuantityFmt, "-"), CUtil.FormatNumber(v.BeginAmountAvgFmt, "-"), "");
                }
                else if (Parameter.GetFieldValue("COSTING_TYPE").Equals("FIFO"))
                {
                    nr2.FillColumnsText("", CLanguage.getValue("balance_forward"), "", "", "", "", CUtil.FormatNumber(v.BeginQuantityFmt, "-"), CUtil.FormatNumber(v.BeginAmountFifoFmt, "-"), "");
                }
                Item = v.ItemCode;
                Location = v.LocationID;

                if ((!H.Equals("")) && (!H.Equals(D)))
                {
                    ftr.FillColumnsText("", CLanguage.getValue("total"), CUtil.FormatNumber(totals[0].ToString(), "-"), CUtil.FormatNumber(totals[2].ToString(), "-")
                        , CUtil.FormatNumber(totals[1].ToString(), "-"), CUtil.FormatNumber(totals[3].ToString(), "-")
                        , CUtil.FormatNumber(totals[4].ToString(), "-"), CUtil.FormatNumber(totals[5].ToString(), "-"), CUtil.FormatNumber(totals[6].ToString(), "-"));

                    newh = newh - ftr.GetHeight();
                    if (newh > 0)
                    {
                        rpp.AddReportRow(ftr);


                        newh = newh - nr1.GetHeight() - nr2.GetHeight();
                        if (newh > 0)
                        {
                            rpp.AddReportRow(nr1);
                            rpp.AddReportRow(nr2);
                            temp++;

                            H = Item + Location;

                            totals[0] = 0;
                            totals[1] = 0;
                            totals[2] = 0;
                            totals[3] = 0;
                            totals[4] = 0;
                            totals[5] = 0;
                            totals[6] = 0;

                            rowNo = 0;
                        }
                    }
                }
                else
                {
                    newh = newh - nr1.GetHeight() - nr2.GetHeight();
                    if (newh > 0)
                    {
                        rpp.AddReportRow(nr1);
                        rpp.AddReportRow(nr2);
                        temp++;
                    }
                }

			}

            if (newh > 0)
            {
                newh = newh - nr.GetHeight();
                if (newh > 0)
                {
                    if (Parameter.GetFieldValue("COSTING_TYPE").Equals("AVG")) //SUMAVG
                    {
                        rowNo++;
                        nr.FillColumnsText(CUtil.FormatInt(rowNo.ToString()), v.BalanceDateFmt, CUtil.FormatNumber(v.InQuantityFmt, "-"), CUtil.FormatNumber(v.InAmountAvgFmt, "-")
                            , CUtil.FormatNumber(v.OutQuantityFmt, "-"), CUtil.FormatNumber(v.OutAmountAvgFmt, "-"), CUtil.FormatNumber(v.EndQuantityFmt, "-")
                            , CUtil.FormatNumber(v.EndAmountAvgFmt, "-"), CUtil.FormatNumber(v.EndUnitPriceAVGFmt, "-"));

                        totals[0] += CUtil.StringToDouble(v.InQuantity);
                        totals[1] += CUtil.StringToDouble(v.OutQuantity);
                        totals[2] += CUtil.StringToDouble(v.InAmountAvg);
                        totals[3] += CUtil.StringToDouble(v.OutAmountAvg);
                        totals[4] = CUtil.StringToDouble(v.EndQuantity);
                        totals[5] = CUtil.StringToDouble(v.EndAmountAvg);
                        totals[6] = CUtil.StringToDouble(v.EndUnitPriceAVG);
                    }
                    else if (Parameter.GetFieldValue("COSTING_TYPE").Equals("FIFO"))//SUMFIFO
                    {
                        rowNo++;
                        nr.FillColumnsText(CUtil.FormatInt(rowNo.ToString()), v.BalanceDateFmt, CUtil.FormatNumber(v.InQuantityFmt, "-"), CUtil.FormatNumber(v.InAmountFifoFmt, "-")
                            , CUtil.FormatNumber(v.OutQuantityFmt, "-"), CUtil.FormatNumber(v.OutAmountFifoFmt, "-"), CUtil.FormatNumber(v.EndQuantityFmt, "-")
                            , CUtil.FormatNumber(v.EndAmountFifoFmt, "-"), CUtil.FormatNumber(v.EndUnitPriceFIFOFmt, "-"));

                        totals[0] += CUtil.StringToDouble(v.InQuantity);
                        totals[1] += CUtil.StringToDouble(v.OutQuantity);
                        totals[2] += CUtil.StringToDouble(v.InAmountFifo);
                        totals[3] += CUtil.StringToDouble(v.OutAmountFifo);
                        totals[4] = CUtil.StringToDouble(v.EndQuantity);
                        totals[5] = CUtil.StringToDouble(v.EndAmountFifo);
                        totals[6] = CUtil.StringToDouble(v.EndUnitPriceFIFO);
                    }
                    rpp.AddReportRow(nr);
                }  
            }

            if (newh > 0)
            {
                if (row == rowcount - 1) //(H != "" && H != D) || 
                {
                    ftrLast.FillColumnsText("", CLanguage.getValue("total"), CUtil.FormatNumber(totals[0].ToString(), "-"), CUtil.FormatNumber(totals[2].ToString(), "-")
                        , CUtil.FormatNumber(totals[1].ToString(), "-"), CUtil.FormatNumber(totals[3].ToString(), "-")
                        , CUtil.FormatNumber(totals[4].ToString(), "-"), CUtil.FormatNumber(totals[5].ToString(), "-"), CUtil.FormatNumber(totals[6].ToString(), "-"));

                    newh = newh - ftrLast.GetHeight();
                    if (newh > 0)
                    {
                        rpp.AddReportRow(ftrLast);

                        totals[0] = 0;
                        totals[1] = 0;
                        totals[2] = 0;
                        totals[3] = 0;
                        totals[4] = 0;
                        totals[5] = 0;
                        totals[6] = 0;
                    }
                }

                Item = v.ItemCode;
                Location = v.LocationID;
                H = Item + Location;
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

			ArrayList arr = OnixWebServiceAPI.GetInventoryBalanceSummaryList(Parameter);
            //string orderArr = string.Empty;
            //foreach (CTable o in arr)
            //{
            //    orderArr += o.GetFieldValue("ITEM_NAME_THAI");
            //    orderArr += "," + o.GetFieldValue("ITEM_ID");
            //    orderArr += "," + o.GetFieldValue("DESCRIPTION");
            //    orderArr += "," + o.GetFieldValue("LOCATION_ID");
            //    orderArr += "," + o.GetFieldValue("BALANCE_DATE");
            //    orderArr += "\n";
            //}
            //MessageBox.Show(orderArr);

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
                        ConstructUIRows(area, property);
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

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_BALANCE_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_BALANCE_DATE");
            entries.Add(entry);

            entry = new CEntry("item_code", EntryType.ENTRY_TEXT_BOX, 150, true, "ITEM_CODE");
            entries.Add(entry);

            entry = new CEntry("item_name_thai", EntryType.ENTRY_TEXT_BOX, 300, true, "ITEM_NAME_THAI");
            entries.Add(entry);

            entry = new CEntry("location_name", EntryType.ENTRY_TEXT_BOX, 300, true, "LOCATION_NAME");
            entries.Add(entry);

            return (entries);
        }
    }
}
