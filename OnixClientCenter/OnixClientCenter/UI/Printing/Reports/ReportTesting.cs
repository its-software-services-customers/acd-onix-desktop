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

namespace Onix.ClientCenter.Reports
{
    public class ReportTesting : CBaseReport
    {
        private Hashtable rowdef = new Hashtable();
        private MemoryStream stream = new MemoryStream();
        private int totalFoot = 0, rowNo = 0;
        private double[] widths2 = new double[2] { 50,50 };
        private double[] widths3 = new double[4] { 10,20, 50, 20 };
        private double[] totals = new double[3] { 30, 50, 20 };

        String custType, custGroup, H = "", D = "";

        public ReportTesting(CTable param) : base(param)
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
            return (null);
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

            ArrayList arr = OnixWebServiceAPI.GetEntityList(Parameter);

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

        protected override UReportPage initNewArea(Size areaSize)
        {
            UReportPage page = new UReportPage();

            CreateGlobalHeaderRow(page); //From CBaseReport
            createDataHeaderRow1(page);
            createDataHeaderRow2(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;

            page.Measure(areaSize);

            return (page);
        }

        
        private void createDataHeaderRow1(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL1"];
            r.FillColumnsText(
                CLanguage.getValue("customer_type"),
                CLanguage.getValue("customer_group"));


            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        private void createDataHeaderRow2(UReportPage page)
        {
            CRow r = (CRow)rowdef["HEADER_LEVEL2"];
            r.FillColumnsText(
                CLanguage.getValue("number"),
                CLanguage.getValue("customer_code"),
                CLanguage.getValue("customer_name"),
                CLanguage.getValue("telephone"));

            ConstructUIRow(page, r);
            AvailableSpace = AvailableSpace - r.GetHeight();
        }

        public override FixedDocument GetFixedDocument()
        {
            return (keepFixedDoc);
        }

        protected override void createRowTemplates()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            #region HEADER_LEVEL1
            nm = "HEADER_LEVEL1";
            CRow h1 = new CRow(nm, 30, 2, defMargin);
            h1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h1;

            CColumn c1_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0), new GridLength(widths2[0], GridUnitType.Star));
            h1.AddColumn(c1_0);

            CColumn c1_1 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0), new GridLength(widths2[1], GridUnitType.Star));
            h1.AddColumn(c1_1);
            #endregion

            #region HEADER_LEVEL2
            nm = "HEADER_LEVEL2";
            CRow h2 = new CRow(nm, 30, 4, defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = h2;

            CColumn c2_0 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths3[0], GridUnitType.Star));
            h2.AddColumn(c2_0);

            CColumn c2_1 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths3[1], GridUnitType.Star));
            h2.AddColumn(c2_1);

            CColumn c2_2 = new CColumn(new Thickness(0.5, 0.5, 0, 0.5), new GridLength(widths3[2], GridUnitType.Star));
            h2.AddColumn(c2_2);

            CColumn c2_3 = new CColumn(new Thickness(0.5, 0.5, 0.5, 0.5), new GridLength(widths3[3], GridUnitType.Star));
            h2.AddColumn(c2_3);
            #endregion


            #region DATA_LEVEL1
            nm = "DATA_LEVEL1";
            CRow r0 = new CRow(nm, 30, 2, defMargin);
            r0.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = r0;

            CColumn r0_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths2[0], GridUnitType.Star));
            r0_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
            r0.AddColumn(r0_c0);

            CColumn r0_c1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths2[1], GridUnitType.Star));
            r0_c1.SetHorizontalAlignment(HorizontalAlignment.Center);
            r0.AddColumn(r0_c1);
            #endregion

            #region DATA_LEVEL2
            nm = "DATA_LEVEL2";
            CRow r1 = new CRow(nm, 30, 4, defMargin);
            r1.SetFont(null, FontStyles.Normal, 0, FontWeights.Normal);
            rowdef[nm] = r1;

            CColumn r1_c0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths3[0], GridUnitType.Star));
            r1_c0.SetHorizontalAlignment(HorizontalAlignment.Center);
            r1.AddColumn(r1_c0);

            CColumn r1_c1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths3[1], GridUnitType.Star));
            r1_c1.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c1);

            CColumn r1_c2 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths3[2], GridUnitType.Star));
            r1_c2.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c2);

            CColumn r1_c3 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(widths3[3], GridUnitType.Star));
            r1_c2.SetHorizontalAlignment(HorizontalAlignment.Left);
            r1.AddColumn(r1_c3);
            #endregion

            #region FOOTER_LEVEL1
            nm = "FOOTER_LEVEL1";
            CRow f1 = new CRow(nm, 30, 3, defMargin);
            f1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            rowdef[nm] = f1;

            CColumn fc_0 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(totals[0], GridUnitType.Star));
            f1.AddColumn(fc_0);

            CColumn fc_1 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(totals[1], GridUnitType.Star));
            f1.AddColumn(fc_1);

            CColumn fc_2 = new CColumn(new Thickness(0.5, 0, 0.5, 0.5), new GridLength(totals[2], GridUnitType.Star));
            f1.AddColumn(fc_2);

            //CColumn fc_3 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[3], GridUnitType.Star));
            //fc_3.SetHorizontalAlignment(HorizontalAlignment.Right);
            //f1.AddColumn(fc_3);

            //CColumn fc_4 = new CColumn(new Thickness(0.5, 0, 0, 0.5), new GridLength(widths[4], GridUnitType.Star));
            //fc_4.SetHorizontalAlignment(HorizontalAlignment.Right);
            //f1.AddColumn(fc_4);
            #endregion

        }

        public override CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int row)
        {
            int rowcount = rows.Count;

            CReportDataProcessingProperty rpp = new CReportDataProcessingProperty();

            CRow r = (CRow)rowdef["DATA_LEVEL1"];
            CRow nr = r.Clone();

            CRow r1 = (CRow)rowdef["DATA_LEVEL2"];
            CRow nr1 = r1.Clone();

            CRow ft = (CRow)rowdef["FOOTER_LEVEL1"];
            CRow ftr = ft.Clone();

            MEntity v = new MEntity(o);

            double newh = AvailableSpace - nr.GetHeight();
            D = v.EntityType + v.EntityGroup;

            if (!H.Equals(D))// (Item != v.ItemCode && Location != v.LocationID)
            {

                nr.FillColumnsText(v.EntityTypeName, v.EntityGroupName);
                //nr2.FillColumnsText("", CLanguage.getValue("balance_forward"), "", "", "", "", "", v.EndQuantity, v.EndAmountAvg, "");
                custType = v.EntityType;
                custGroup = v.EntityGroup;

                if ((!H.Equals("")) && (!H.Equals(D)))
                {
                    
                    ftr.FillColumnsText(CLanguage.getValue("total"), CUtil.FormatNumber(totalFoot.ToString()), "");
                    rpp.AddReportRow(ftr);
                    rpp.AddReportRow(nr);
                    H = custType + custGroup;

                    rowNo = 0;
                    totalFoot = 0;
                }
                else
                {
                    rpp.AddReportRow(nr);
                }
            }
            rowNo++;
            totalFoot++;
            nr1.FillColumnsText(CUtil.FormatInt(rowNo.ToString()), v.EntityCode, v.EntityName, v.Phone);
            //if (Parameter.GetFieldValue("COSTING_TYPE").Equals("AVG"))
            //{
            //    nr.FillColumnsText((row + 1).ToString(), v.DocDate, v.DocNo, v.InQuantityMovement, v.OutQuantityMovement, v.InAmountMovementAvg, v.OutAmountMovementAvg, v.EndQuantity, v.EndAmountAvg, v.UnitPriceAVG);
            //    totals[0] = totals[0] + double.Parse(v.InQuantityMovement);
            //    totals[1] = totals[1] + double.Parse(v.OutQuantityMovement);
            //    totals[2] = totals[2] + double.Parse(v.InAmountMovementAvg);
            //    totals[3] = totals[3] + double.Parse(v.OutAmountMovementAvg);
            //    totals[4] = double.Parse(v.EndQuantity);
            //    totals[5] = double.Parse(v.EndAmountAvg);
            //    totals[6] = double.Parse(v.UnitPriceAVG);
            //}


            rpp.AddReportRow(nr1);

            if (row == rowcount - 1) //(H != "" && H != D) || 
            {
                ftr.FillColumnsText(CLanguage.getValue("total"), CUtil.FormatNumber(totalFoot.ToString()), "");
                rpp.AddReportRow(ftr);

                rowNo = 0;
                totalFoot = 0;
                newh = AvailableSpace - ftr.GetHeight();
            }

            custType = v.EntityType;
            custGroup = v.EntityGroup;
            H = custType + custGroup;

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
    }
}
