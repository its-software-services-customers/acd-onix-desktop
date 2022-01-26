using System;
using System.Windows;
using System.Collections;
using System.IO;
using System.Windows.Documents;
using Onix.Client.Report;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using System.Windows.Controls;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.Reports
{
	class CReportInv004_02_InventoryMove : CBaseReport
	{
		private Hashtable rowdef = new Hashtable();
		private MemoryStream stream = new MemoryStream();

        private int defCol = 10;
        private double defHeightCol = 30;

        private double[] widths;
        private double totals = 0;

        public CReportInv004_02_InventoryMove() : base()
        {
            widths = new double[] { 5, 8, 12, 12, 15, 10, 14, 8, 8, 8 };
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
            CRow h = new CRow(nm, defHeightCol, defCol, defMargin);
            h.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h;

            CColumn c_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            h.AddColumn(c_0);

            CColumn c_1 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            h.AddColumn(c_1);

            CColumn c_2 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
            h.AddColumn(c_2);

            CColumn c_3 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            h.AddColumn(c_3);

            CColumn c_4 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            h.AddColumn(c_4);

            CColumn c_5 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
            h.AddColumn(c_5);

            CColumn c_6 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
            h.AddColumn(c_6);

            CColumn c_7 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
            h.AddColumn(c_7);

            CColumn c_8 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths[8], GridUnitType.Star));
            h.AddColumn(c_8);

            CColumn c_9 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths[9], GridUnitType.Star));
            h.AddColumn(c_9);


            nm = "DATA_LEVEL1";
            CRow r0 = new CRow(nm, defHeightCol, defCol, defMargin);
            r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r0;

            CColumn r0_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            r0_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
            r0.AddColumn(r0_c0);

            CColumn r0_c1_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            r0_c1_0.SetHorizontalAlignment(HorizontalAlignment.Center);
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

            CColumn r0_c4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
            r0_c4.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c4);

            CColumn r0_c5 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
            r0_c5.SetHorizontalAlignment(HorizontalAlignment.Left);
            r0.AddColumn(r0_c5);

            CColumn r0_c6 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
            r0_c6.SetHorizontalAlignment(HorizontalAlignment.Right);
            r0.AddColumn(r0_c6);

            CColumn r0_c7 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[8], GridUnitType.Star));
            r0_c7.SetHorizontalAlignment(HorizontalAlignment.Right);
            r0.AddColumn(r0_c7);

            CColumn r0_c8 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[9], GridUnitType.Star));
            r0_c8.SetHorizontalAlignment(HorizontalAlignment.Right);
            r0.AddColumn(r0_c8);


            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, defHeightCol, defCol, defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            CColumn fc_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[0], GridUnitType.Star));
            f1.AddColumn(fc_0);

            CColumn fc_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[1], GridUnitType.Star));
            f1.AddColumn(fc_1);

            CColumn fc_2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[2], GridUnitType.Star));
            f1.AddColumn(fc_2);

            CColumn fc_3 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            fc_3.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_3);

            CColumn fc_4 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            fc_4.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_4);

            CColumn fc_5 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[5], GridUnitType.Star));
            fc_5.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_5);

            CColumn fc_6 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[6], GridUnitType.Star));
            fc_6.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_6);

            CColumn fc_7 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[7], GridUnitType.Star));
            fc_7.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_7);

            CColumn fc_8 = new CColumn(new Thickness(0, 0, 0, 0.5), new GridLength(widths[8], GridUnitType.Star));
            fc_8.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_8);

            CColumn fc_9 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths[9], GridUnitType.Star));
            fc_9.SetHorizontalAlignment(HorizontalAlignment.Right);
            f1.AddColumn(fc_9);
        }

		private void createDataHeaderRow1(UReportPage page)
		{
			CRow r = (CRow)rowdef["HEADER_LEVEL1"];
			r.FillColumnsText(
                CLanguage.getValue("number"),
				CLanguage.getValue("date"),
				CLanguage.getValue("inventory_doc_no"),
				CLanguage.getValue("location_name"),
                CLanguage.getValue("locationTo_name"),
                CLanguage.getValue("item_code"),
				CLanguage.getValue("item_name_thai"),
				CLanguage.getValue("lot_quantity"),
				CLanguage.getValue("lot_avg"),
				CLanguage.getValue("ProdValue"));

			ConstructUIRow(page, r);
			AvailableSpace = AvailableSpace - r.GetHeight();
		}

		protected override UReportPage initNewArea(Size areaSize)
		{
			UReportPage page = new UReportPage();

			CreateGlobalHeaderRow(page);
			createDataHeaderRow1(page);

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

            CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
            CRow ftr = ft.Clone();

            MInventoryTransaction v = new MInventoryTransaction(o);

			double newh = AvailableSpace - nr.GetHeight();

            if (Parameter.GetFieldValue("COSTING_TYPE").Equals("MOVE"))
            {
                nr.FillColumnsText((row + 1).ToString(), v.DocumentDateFmt, v.DocumentNo, v.LocationName, v.LocationToName, v.ItemCode, v.ItemNameThai
                   , CUtil.FormatNumber(v.ItemQuantity, "-"), CUtil.FormatNumber(v.ItemPrice, "-"), CUtil.FormatNumber(v.ItemAmount, "-"));

                totals += CUtil.StringToDouble(v.ItemAmount);
            }
			rpp.AddReportRow(nr);
            if (row == rowcount - 1)
            {
                newh = newh - ftr.GetHeight();
                if (newh > 0)
                {
                    ftr.FillColumnsText("", CLanguage.getValue("total"), "", "", "", "", "", "", "", CUtil.FormatNumber(totals.ToString(), "-"));
                    rpp.AddReportRow(ftr);

                    totals = 0;
                }
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

            if (Parameter.GetFieldValue("COSTING_TYPE").Equals("MOVE"))
                Parameter.SetFieldValue("DOCUMENT_TYPE", "3");
            ArrayList arr = OnixWebServiceAPI.GetInventoryTransactionList(Parameter);

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
            #region First Header Case : Row is 0
            if (arr.Count == 0)
                arr.Add(Parameter); //add for show header and first row empty
            #endregion
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

        public void LoadCombo(ComboBox cbo, String id)
        {
            CUtil.LoadInventoryDocStatus(cbo, true, id);
        }

        public void InitCombo(ComboBox cbo)
        {
            cbo.SelectedItem = "ObjSelf";
            cbo.DisplayMemberPath = "Description";
        }

        public override ArrayList GetReportInputEntries()
        {
            CEntry entry = null;
            ArrayList entries = new ArrayList();

            entry = new CEntry("from_date", EntryType.ENTRY_DATE_MIN, 200, true, "FROM_DATE");
            entries.Add(entry);

            entry = new CEntry("to_date", EntryType.ENTRY_DATE_MAX, 200, true, "TO_DATE");
            entries.Add(entry);

            entry = new CEntry("inventory_doc_status", EntryType.ENTRY_COMBO_BOX, 200, true, "DOCUMENT_STATUS");
            entry.SetComboLoadAndInit(LoadCombo, InitCombo, ObjectToIndex);
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
