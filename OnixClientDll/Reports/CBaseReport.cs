using System;
using System.Windows;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Collections;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Onix.Client.Report
{
    public delegate void ReportProgressUpdate(int current, int maximum);
    public delegate void ReportStatusUpdate(Boolean done, Boolean fail);
    public delegate String ReportNameCallback(int reportID);

    public class CReportPageParam
    {
        public int ItemPerPage = 0;
        public int StartIndex = 0;
        public int EndIndex = 0;
        public int PatchRow = 0;
        public int TotalItemCount = 0;

        public ArrayList Items = new ArrayList();
    }

    public interface IReport
    {
        Boolean RetrieveData();
        FixedDocument CreateFixedDocument();
        FixedDocument GetFixedDocument();
        //CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int rownum);
        ArrayList GetReportInputEntries();
        MReportConfig CreateDefaultConfigValues();
        MFormConfig CreateDefaultFormConfigValues();
        MFormConfig GetFormConfig();
        void UpdateReportConfig(MReportConfig cfg);
        MBaseModel CreateDefaultData();
        void SetExtraParam(CTable extParam);
    }

    public class CReportDataProcessingProperty
    {
        private Boolean newpageRequired = false;
        private int tempNotRowDetails = 0;
        private ArrayList rows = new ArrayList();

        public Boolean IsNewPageRequired
        {
            get
            {
                return (newpageRequired);
            }

            set
            {
                newpageRequired = value;
            }
        }

        public int TempNotRowDetails
        {
            get
            {
                return (tempNotRowDetails);
            }

            set
            {
                tempNotRowDetails = value;
            }
        }

        public ArrayList ReportRows
        {
            get
            {
                return (rows);
            }
        }

        public void AddReportRow(CRow r)
        {
            rows.Add(r);
        }
    }

    public class CBaseReport : DocumentPaginator, IReport
    {
        private CTable rptparam;
        //private FixedDocument flodocSource = null;
        private int pcount = 0;
        private ReportProgressUpdate updateProgFunc = null;
        private ReportStatusUpdate updateDoneFunc = null;

        private Hashtable rowdef = new Hashtable();
        private MMasterRef reportObj = null;

        public int DPI = 96;
        protected int CurrentPage = 0;
        protected Double AvailableSpace = 0;
        public MReportConfig rptCfg = null;
        public MBaseModel dataSource = null;       
        public Boolean isPageRange = false;
        public int fromPage = 0;
        public int toPage = 0;

        private Hashtable reportConfigHash = new Hashtable();

        protected FixedDocument keepFixedDoc = null;
        protected ArrayList pages = new ArrayList();
        protected String baseTemplateName = "";
        protected String itemPropertyName = "SelectItemName";

        private CTable extParam = null;
        private Boolean isPageRangeSupport = false;
        private ReportNameCallback rptNameCallback = null;
        private CExcelRenderer excel = null;
        private ArrayList reportEntries = null;

        private void CreateDefaultHeader()
        {
            String nm = "";
            Thickness defMargin = new Thickness(3, 1, 3, 1);

            CColumn c0_0 = new CColumn(new Thickness(0, 0, 0, 0), new GridLength(50, GridUnitType.Star));
            c0_0.SetHorizontalAlignment(HorizontalAlignment.Left);

            CColumn c0_1 = new CColumn(new Thickness(0, 0, 0, 0), new GridLength(50, GridUnitType.Star));
            c0_1.SetHorizontalAlignment(HorizontalAlignment.Right);

            nm = "GLB_HEADER_LEVEL0";

            CRow h0 = new CRow(nm, 20, 2, defMargin);
            h0.SetFont(null, FontStyles.Normal, 10, FontWeights.Normal);
            h0.AddColumn(c0_0);
            h0.AddColumn(c0_1);
            rowdef[nm] = h0;

            CColumn c1_0 = new CColumn(new Thickness(0, 0, 0, 0), new GridLength(100, GridUnitType.Star));
            nm = "GLB_HEADER_LEVEL1";
            CRow h1 = new CRow(nm, 30, 1, defMargin);
            h1.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            h1.AddColumn(c1_0);
            rowdef[nm] = h1;

            CColumn c1_2 = new CColumn(new Thickness(0, 0, 0, 0), new GridLength(100, GridUnitType.Star));
            nm = "GLB_HEADER_PARAM";
            CRow h2 = new CRow(nm, 30, 1, defMargin);
            h2.SetFont(null, FontStyles.Normal, 0, FontWeights.Bold);
            h2.AddColumn(c1_2);
            rowdef[nm] = h2;
        }

        public void SetReportEntry(ArrayList entries)
        {
            reportEntries = entries;
        }

        public void SetReportObject(MMasterRef dat)
        {
            reportObj = dat;
        }

        protected void addConfig(String group, int width, String key, HorizontalAlignment hAlign, HorizontalAlignment bAligh, HorizontalAlignment fAligh, String fldName, String dataType, Boolean sumFlag)
        {
            ArrayList arr = null;
            if (reportConfigHash.ContainsKey(group))
            {
                arr = (ArrayList)reportConfigHash[group];
            }
            else
            {
                arr = new ArrayList();
                reportConfigHash[group] = arr;
            }

            CReportColConfig col = new CReportColConfig(width, key, hAlign, bAligh, fAligh, fldName, dataType, sumFlag);
            arr.Add(col);
        }

        protected int getColumnCount(String group)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];
            return (arr.Count);
        }

        protected void configRow(String group, CRow row, String rowType)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];
            int cnt = arr.Count;
            int i = 0;

            foreach (CReportColConfig cc in arr)
            {
                Thickness tck = new Thickness(0.5, 0.5, 0, 0.5);
                if (i == cnt - 1)
                {
                    tck = new Thickness(0.5, 0.5, 0.5, 0.5);
                }

                HorizontalAlignment al = HorizontalAlignment.Left;
                if (rowType.Equals("H"))
                {
                    al = cc.HeaderAlighment;
                }
                else if (rowType.Equals("B"))
                {
                    al = cc.BodyAlighment;
                }
                else if (rowType.Equals("F"))
                {
                    al = cc.FooterAlighment;
                }

                CColumn c = new CColumn(tck, new GridLength(cc.ColumnWidth, GridUnitType.Star));
                c.SetHorizontalAlignment(al);
                c.SetDataType(cc.DataType);

                row.AddColumn(c);

                i++;
            }

            excel.AddRowTemplate(row);
        }

        protected ArrayList sumDataTexts(String group, ArrayList accum, ArrayList dats, int factor)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];
            ArrayList temps = new ArrayList();

            if (accum.Count == 0)
            {
                foreach (CReportColConfig rc in arr)
                {
                    accum.Add(0.00);
                }
            }

            int i = 0;
            foreach (CReportColConfig rc in arr)
            {
                double amt = (double)accum[i];
                double sum = 0.00;
                if (rc.IsSum)
                {
                    String tmp = "0.00";
                    if (dats[i] is double)
                    {
                        tmp = dats[i].ToString();
                    }
                    else
                    {
                        tmp = (String)dats[i];
                    }

                    double value = CUtil.StringToDouble(tmp);
                    sum = amt + factor * value;
                }

                temps.Add(sum);
                i++;
            }

            return (temps);
        }


        protected ArrayList sumDataTexts(String group, ArrayList accum, ArrayList dats)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];
            ArrayList temps = new ArrayList();

            if (accum.Count == 0)
            {
                foreach (CReportColConfig rc in arr)
                {
                    accum.Add(0.00);
                }
            }

            int i = 0;
            foreach (CReportColConfig rc in arr)
            {
                double amt = (double)accum[i];
                double sum = 0.00;
                if (rc.IsSum)
                {
                    String tmp = "0.00";
                    if (dats[i] is double)
                    {
                        tmp = dats[i].ToString();
                    }
                    else
                    {
                        tmp = (String) dats[i];
                    }

                    double value = CUtil.StringToDouble(tmp);
                    sum = amt + value;
                }

                temps.Add(sum);
                i++;
            }

            return (temps);
        }

        protected ArrayList copyTotalArray(ArrayList accums)
        {
            ArrayList temps = new ArrayList();
            foreach (double amt in accums)
            {
                temps.Add(amt);
            }

            return (temps);
        }

        protected double[] copyTotalArray(double[] accums)
        {
            double[] temps = new double[100];

            int cnt = accums.Length;
            for (int i=0; i<cnt; i++)
            {
                temps[i] = accums[i];
            }

            return (temps);
        }

        protected ArrayList displayTotalTexts(String group, ArrayList accum, int idx, String key)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];
            ArrayList temps = new ArrayList();

            int i = 0;
            foreach (CReportColConfig rc in arr)
            {
                if (i == idx)
                {
                    temps.Add(CLanguage.getValue(key));
                }
                else if (rc.IsSum)
                {
                    String tmp = "";
                    if (accum.Count > 0)
                    {
                        tmp = CUtil.FormatNumber(accum[i].ToString());
                    }

                    temps.Add(tmp);
                }
                else
                {
                    temps.Add("");
                }

                i++;
            }

            return (temps);
        }

        protected ArrayList getColumnHederTexts(String group, String recType)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];

            ArrayList temps = new ArrayList();
            foreach (CReportColConfig rc in arr)
            {
                String txt = CLanguage.getValue(rc.ColumnLabel);
                temps.Add(txt);
            }

            return (temps);
        }

        protected ArrayList getColumnDataTexts(String group, int rowNum, CTable data)
        {
            ArrayList arr = (ArrayList)reportConfigHash[group];

            ArrayList temps = new ArrayList();


            foreach (CReportColConfig rc in arr)
            {

                if (rc.DataType.Equals("RN"))
                {
                    temps.Add(rowNum.ToString());
                }
                else if (rc.DataType.Equals("S"))
                {
                    temps.Add(data.GetFieldValue(rc.FieldName));
                }
                else if (rc.DataType.Equals("DT"))
                {
                    String dateStr = data.GetFieldValue(rc.FieldName).Trim();
                    String dt = "";
                    if (!dateStr.Equals(""))
                    {
                        dt = CUtil.DateTimeToDateString(CUtil.InternalDateToDate(dateStr));
                    }
                    temps.Add(dt);
                }
                else if (rc.DataType.Equals("D"))
                {
                    String amt = CUtil.FormatNumber(data.GetFieldValue(rc.FieldName));
                    temps.Add(amt);
                }
                else if (rc.DataType.Equals("DE"))
                {
                    //Display empty if zero

                    String amt = CUtil.FormatNumber(data.GetFieldValue(rc.FieldName));
                    if (amt.Equals("0.00"))
                    {
                        amt = "";
                    }
                    temps.Add(amt);
                }
                else if (rc.DataType.Equals("DD"))
                {
                    //Display dash if zero

                    String amt = CUtil.FormatNumber(data.GetFieldValue(rc.FieldName));
                    if (amt.Equals("0.00"))
                    {
                        amt = "-";
                    }
                    temps.Add(amt);
                }
                else
                {
                    temps.Add(data.GetFieldValue(rc.FieldName));
                }
            }

            return (temps);
        }

        public virtual MBaseModel CreateDefaultData()
        {
            return (null);
        }

        protected ReportProgressUpdate GetProgressUpdateFunc()
        {
            return (updateProgFunc);
        }

        protected ReportStatusUpdate GetProgressDoneFunc()
        {
            return (updateDoneFunc);
        }

        protected Size GetAreaSize()
        {
            Thickness m = Margin;

            Double w = PageSize.Width - m.Left - m.Right;
            Double h = PageSize.Height - m.Top - m.Bottom;

            return (new Size(w, h));
        }

        public Boolean IsPageRangeSupport
        {
            get
            {
                return (isPageRangeSupport);
            }

            set
            {
                isPageRangeSupport = value;
            }
        }

        public void SetReportNameCallback(ReportNameCallback rptNameFunc)
        {
            rptNameCallback = rptNameFunc;
        }

        public void SetProgressUpdateFunc(ReportProgressUpdate func)
        {
            updateProgFunc = func;
        }

        public void SetProgressDoneFunc(ReportStatusUpdate func)
        {
            updateDoneFunc = func;
        }

        public virtual void SetExtraParam(CTable param)
        {
            extParam = param;
        }

        public virtual CTable GetExtraParam()
        {
            return (extParam);
        }

        public virtual MReportConfig CreateDefaultConfigValues()
        {
            return (null);
        }

        public virtual MFormConfig CreateDefaultFormConfigValues()
        {
            return (null);
        }

        public virtual MFormConfig GetFormConfig()
        {
            return (null);
        }

        public virtual ArrayList GetReportInputEntries()
        {
            return (null);
        }

        public virtual void SetReportParam(CTable param)
        {
            rptparam = param;
        }

        public virtual PageOrientation GetPageOrientation()
        {
            if (rptparam == null)
            {
                //Form

                double w = rptCfg.PageWidthDot;
                double h = rptCfg.PageHeightDot;

                if (w > h)
                {
                    return (PageOrientation.Landscape);
                }

                return (PageOrientation.Portrait);
            }

            String ot = rptparam.GetFieldValue("PAPER_ORIENTATION");

            if (ot.Equals("LANDSCAPE"))
            {
                return (PageOrientation.Landscape);
            }

            return (PageOrientation.Portrait);
        }

        protected void CreateGlobalHeaderRow(UReportPage page)
        {
            CRow r = (CRow)rowdef["GLB_HEADER_LEVEL0"];

            CRow nr = r.Clone();
            String info = String.Format("{0} ({1})", CUtil.DateTimeToDateStringTime(DateTime.Now), OnixWebServiceAPI.GetLastUserLogin());
            nr.FillColumnsText(info, "Page " + CurrentPage);
            ConstructUIRow(page, nr);

            AvailableSpace = AvailableSpace - nr.GetHeight();

            String rpid = rptparam.GetFieldValue("REPORT_ID");

            r = (CRow)rowdef["GLB_HEADER_LEVEL1"];            
            r.FillColumnsText(reportObj.Optional);
            ConstructUIRow(page, r);

            AvailableSpace = AvailableSpace - r.GetHeight();

            String header = createReportParamHeader();
            if (!header.Equals(""))
            {
                CRow paramRow = (CRow)rowdef["GLB_HEADER_PARAM"];
                paramRow.FillColumnsText(header);
                ConstructUIRow(page, paramRow);

                AvailableSpace = AvailableSpace - paramRow.GetHeight();
            }
        }

        private String createReportParamHeader()
        {
            Boolean matched1 = false;
            Boolean matched2 = false;

            String[] keywords = new[] 
            {
                "FROM_DOCUMENT_DATE", "FROM_BALANCE_DATE", "FROM_CHEQUE_DATE",
                "TO_DOCUMENT_DATE", "TO_BALANCE_DATE", "TO_CHEQUE_DATE",
                "LEAVE_YEAR", "TAX_YEAR"
            };

            String year = "";
            String fromDateFmt = "N/A";
            String toDateFmt = "N/A";
            for (int i = 0; i < keywords.Length; i++)
            {
                String keyword = keywords[i];

                String dateStr = Parameter.GetFieldValue(keyword);
                DateTime dateStamp = CUtil.InternalDateToDate(dateStr);

                if (dateStr.Equals(""))
                {
                    continue;
                }

                if (keyword.Contains("FROM_"))
                {
                    fromDateFmt = CUtil.DateTimeToBEDateString(dateStamp);
                    matched1 = true;
                }

                if (keyword.Contains("TO_"))
                {
                    toDateFmt = CUtil.DateTimeToBEDateString(dateStamp);
                    matched1 = true;
                }

                if (keyword.Contains("LEAVE_YEAR") || keyword.Contains("TAX_YEAR"))
                {
                    year = Parameter.GetFieldValue(keyword);
                    matched2 = true;
                }
            }

            String header = "";

            if (matched1)
            {
                header = String.Format("{0} {1} : {2} {3}",
                    CLanguage.getValue("from_date"), fromDateFmt,
                    CLanguage.getValue("to_date"), toDateFmt);
            }

            if (matched2)
            {
                header = "ปี " + year;
            }

            return (header);
        }

        public CBaseReport(CTable param)
        {
            rptparam = param;
            CreateDefaultHeader();
        }

        public CBaseReport()
        {
            CreateDefaultHeader();
        }

        public CBaseReport(MBaseModel model)
        {
            CreateDefaultHeader();
        }

        public override DocumentPage GetPage(int pageNumber)
        {
            DocumentPaginator dpg = keepFixedDoc.DocumentPaginator;
            DocumentPage dp = dpg.GetPage(pageNumber);
            return (dp);
        }

        protected virtual ArrayList createPageParam()
        {
            return (new ArrayList());
        }

        private double getItemHeight(double maxWidth, MBaseModel m, String fieldName)
        {
            Boolean wrappFlag = rptCfg.GetConfigValue("WrapFlag").Equals("Y");
            if (!wrappFlag || maxWidth <= 0)
            {
                double rh = CUtil.StringToDouble(rptCfg.GetConfigValue("ItemRowHeight"));
                if (rh <= 0)
                {
                    rh = 30; //Default value
                }

                return (rh);
            }

            String itemDesc = " ";

            if (!fieldName.Equals(""))
            {
                itemDesc = (String) m.GetType().GetProperty(fieldName).GetValue(m, null);
            }
            
            String fontName = rptCfg.GetConfigValue("FontName");
            double fontSize = CUtil.StringToDouble(rptCfg.GetConfigValue("FontSize"));

            TextBlock tb = new TextBlock();
            tb.Text = itemDesc;
            tb.TextWrapping = TextWrapping.Wrap;
            tb.FontFamily = new FontFamily(fontName); ;
            tb.FontSize = fontSize;
            tb.HorizontalAlignment = HorizontalAlignment.Left;
            tb.VerticalAlignment = VerticalAlignment.Center;
            tb.FontWeight = FontWeights.Bold;
            tb.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            tb.Arrange(new Rect(0, 0, maxWidth, 1000));

            double h1 = tb.ActualHeight;
            int lineCnt = (int) Math.Ceiling(tb.ActualWidth / maxWidth);

            double h = (lineCnt * h1) + 10; //Margin top and bottom, ONLY applicable to 5pt margin top and bottom

            tb = null;

            return (h);
        }

        protected ArrayList createPageParamComplex<T>(ObservableCollection<T> collection, int itemPerPage) where T : MBaseModel
        {
            ArrayList pageParams = new ArrayList();
            
            double areaHeight = CUtil.StringToDouble(rptCfg.GetConfigValue("PrevAreaHeight"));
            double itemWidth = CUtil.StringToDouble(rptCfg.GetConfigValue("PrevItemWidth"));

            double defaultItemHeigh = getItemHeight(itemWidth, null, "");

            int arrcnt = 0;
            foreach (T mi in collection)
            {
                if (!mi.ExtFlag.Equals("D"))
                {
                    arrcnt++;
                }
            }

            CReportPageParam prm = null;
            double remainHeight = areaHeight;
            int itemNo = 0;

            foreach (T mi in collection)
            {
                if (mi.ExtFlag.Equals("D"))
                {
                    continue;
                }

                if (itemNo == 0)
                {
                    prm = new CReportPageParam();
                    prm.StartIndex = 1;
                    prm.EndIndex = 1;
                    
                    pageParams.Add(prm);
                }

                itemNo++;
                double itemHeight = getItemHeight(itemWidth, mi, itemPropertyName); //getItemHeight(itemWidth, mi);

                if (itemHeight > remainHeight)
                {
                    //New page
                    prm = new CReportPageParam();
                    prm.StartIndex = itemNo;

                    pageParams.Add(prm);
                    
                    remainHeight = areaHeight;
                }

                remainHeight = remainHeight - itemHeight;
                prm.Items.Add(mi);
                prm.EndIndex = itemNo;
                prm.ItemPerPage = prm.EndIndex - prm.StartIndex + 1;
                prm.TotalItemCount = arrcnt;
                prm.PatchRow = (int) Math.Ceiling(remainHeight / defaultItemHeigh);
            }

            return (pageParams);
        }

        protected void populateDefaultReportConfig(MReportConfig rc)
        {
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
            rc.SetConfigValue("CustomerBoxHeight", "150", "double", "Customer box Height");

            rc.SetConfigValue("ItemPerPage", "22", "int", "Item per pages");
            rc.SetConfigValue("WrapFlag", "N", "String", "Y=Wrap, N=No wrap");
            rc.SetConfigValue("ItemRowHeight", "30", "double", "Item row heignt in case WrapFlag = N");

            //Custom A4 form Height="29.7cm" Width="21cm"
            rc.SetConfigValue("PageWidthCm", "21", "double", "Page Width (CM)");
            rc.SetConfigValue("PageHeightCm", "29.7", "double", "Page Height (CM)");

            rc.SetConfigValue("MarginLeftCm", "0.54", "double", "Margin Left (CM)");
            rc.SetConfigValue("MarginTopCm", "0.54", "double", "Margin Top (CM)");
            rc.SetConfigValue("MarginRightCm", "0.54", "double", "Margin Right (CM)");
            rc.SetConfigValue("MarginBottomCm", "0.54", "double", "Margin Bottom (CM)");
        }

        protected ArrayList createPageParamEasy<T>(ObservableCollection<T> collection, int itemPerPage) where T : MBaseModel
        {
            int arrcnt = 0;
            foreach (T mi in collection)
            {
                if (!mi.ExtFlag.Equals("D"))
                {
                    arrcnt++;
                }
            }

            int pcnt = 0;
            int remainder = 0;

            if (itemPerPage > 0)
            {
                pcnt = arrcnt / itemPerPage;
                remainder = arrcnt % itemPerPage;
            }

            int pageCount = 0;

            if (pcnt == 0)
            {
                pageCount = 1;
            }
            else if (remainder == 0)
            {
                pageCount = pcnt;
            }
            else
            {
                pageCount = pcnt + 1;
            }

            ArrayList pageParams = new ArrayList();
            for (int i = 0; i < pageCount; i++)
            {
                CReportPageParam prm = new CReportPageParam();
                prm.ItemPerPage = itemPerPage;

                populatePageItems(prm, itemPerPage, i+1, collection);

                pageParams.Add(prm);
            }

            return (pageParams);
        }

        private void populatePageItems<T>(CReportPageParam prm, int itemPerPage, int pageNo, ObservableCollection<T> collection) where T : MBaseModel
        {
            int start = (pageNo - 1) * itemPerPage + 1;
            int end = start + itemPerPage - 1;

            prm.Items.Clear();

            int i = 0;
            foreach (T m in collection)
            {
                if (m.ExtFlag.Equals("D"))
                {
                    continue;
                }

                i++;

                if ((i >= start) && (i <= end))
                {                    
                    prm.Items.Add(m);
                }
            }

            prm.StartIndex = start;
            prm.EndIndex = end;
            prm.TotalItemCount = collection.Count;

            int remain = itemPerPage - prm.Items.Count;
            prm.PatchRow = remain;
        }

        protected virtual void initPageCreateFlow()
        {
        }

        protected virtual UserControl createPageObject(Size s, int page, int pageCount, CReportPageParam param)
        {
            return (null);
        }

        public virtual void SetDataSourceAndParam(MBaseModel ds, MReportConfig cfg)
        {
            rptCfg = cfg;
            dataSource = ds;
        }

        public virtual void UpdateReportConfig(MReportConfig cfg)
        {
            rptCfg = cfg;
        }

        public MReportConfig GetReportConfig()
        {
            return (rptCfg);
        }

        public virtual Boolean RetrieveData()
        {
            return (true);
        }

        //TODO : Change it to protected
        public virtual CReportDataProcessingProperty DataToProcessingProperty(CTable o, ArrayList rows, int rownum)
        {
            return (null);
        }

        //Form fixed document
        public virtual FixedDocument CreateFixedDocument()
        {           
            ArrayList pageParams = new ArrayList();
            if (dataSource != null)
            {
                initPageCreateFlow();
                pageParams = createPageParam();
            }
            else
            {
                CReportPageParam dummyPage = new CReportPageParam();
                pageParams.Add(dummyPage);
            }

            int pc = pageParams.Count;

            FixedDocument fd = new FixedDocument();
            Size s = new Size(rptCfg.PageWidthDot, rptCfg.PageHeightDot);
            fd.DocumentPaginator.PageSize = s;

            pages.Clear();

            for (int i = 1; i <= pc; i++)
            {
                if (!isInRange(i))
                {
                    continue;
                }

                CReportPageParam pageParam = (CReportPageParam)pageParams[i - 1];
                UserControl page = createPageObject(s, i, pc, pageParam);

                page.Width = rptCfg.AreaWidthDot;
                page.Height = rptCfg.AreaHeightDot;
                page.Measure(s);

                FixedPage fp = new FixedPage();
                fp.Margin = new Thickness(rptCfg.MarginLeftDot, rptCfg.MarginTopDot, rptCfg.MarginRightDot, rptCfg.MarginBottomDot);

                PageContent pageContent = new PageContent();
                ((System.Windows.Markup.IAddChild)pageContent).AddChild(fp);

                fd.Pages.Add(pageContent);
                fp.Children.Add(page);

                pages.Add(fp);
            }

            keepFixedDoc = fd;

            return (fd);
        }

        protected virtual void createRowTemplates()
        {
        }

        protected virtual UReportPage initNewArea(Size areaSize)
        {
            UReportPage page = new UReportPage();

            CreateGlobalHeaderRow(page);

            page.Width = areaSize.Width;
            page.Height = areaSize.Height;
            page.Measure(areaSize);

            return (page);
        }

        protected virtual ArrayList getRecordSet()
        {
            return (null);
        }

        protected String ObjectToIndex(MBaseModel obj)
        {
            if (obj == null)
            {
                return ("");
            }

            MMasterRef mr = (MMasterRef)obj;
            return (mr.MasterID);
        }

        public CExcelRenderer GetExcelRenderer()
        {
            return (excel);
        }

        public virtual FixedDocument CreateReportFixedDocument()
        {
            excel = new CExcelRenderer(rptCfg.ReportName);
            FixedDocument fd = new FixedDocument();

            ReportProgressUpdate updateFunc = GetProgressUpdateFunc();
            ReportStatusUpdate doneFunc = GetProgressDoneFunc();

            fd.DocumentPaginator.PageSize = PageSize;

            if (doneFunc != null)
            {
                doneFunc(false, false);
            }

            ArrayList arr = getRecordSet();
            if (arr == null)
            {
                return (fd);
            }

            int cnt = arr.Count;
            UReportPage area = null;

            createRowTemplates();
            excel.CalculateMergeCellRange(baseTemplateName);

            int i = 0;
            int r = 0;

            Size areaSize = GetAreaSize();
            AvailableSpace = areaSize.Height;

            CReportDataProcessingProperty property = null;
            while (i < arr.Count)
            {
                CTable o = (CTable)arr[i];

                if ((r == 0) || (property.IsNewPageRequired))
                {
                    AvailableSpace = areaSize.Height;

                    CurrentPage++;

                    FixedPage fp = new FixedPage();
                    fp.Margin = Margin;

                    PageContent pageContent = new PageContent();
                    ((System.Windows.Markup.IAddChild)pageContent).AddChild(fp);

                    area = initNewArea(areaSize);
                    fp.Children.Add(area);

                    if (isInRange(CurrentPage))
                    {
                        fd.Pages.Add(pageContent);
                        pages.Add(area);
                    }                                        
                }

                property = DataToProcessingProperty(o, arr, i);
                if (property.IsNewPageRequired)
                {
                    //Do not create row if that row caused new page flow
                    //But create it in the next page instead 
                    i--;
                    r--;
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
                r++;
            }

            if (doneFunc != null)
            {
                doneFunc(true, false);
            }

            keepFixedDoc = fd;
            return (fd);
        }

        public virtual FixedDocument GetFixedDocument()
        {
            return (keepFixedDoc);
        }

        public override bool IsPageCountValid
        {
            get
            {
                return(true);
            }
        }

        public override int PageCount
        {
            get
            {
                return (pages.Count);
            }
        }

        public override IDocumentPaginatorSource Source
        {
            get
            {
                return keepFixedDoc.DocumentPaginator.Source;
            }
        }

        private Size getReportPageSize()
        {
            PaperTypeEnum ps = (PaperTypeEnum)int.Parse(rptparam.GetFieldValue("PAPER_TYPE"));
            String ot = rptparam.GetFieldValue("PAPER_ORIENTATION");

            double w = 0;
            double h = 0;

            if (ps == PaperTypeEnum.PAPER_TYPE_A4)
            {
                w = 8.3 * DPI;
                h = 11.7 * DPI;
            }
            else if (ps == PaperTypeEnum.PAPER_TYPE_LETTER)
            {
                w = 8.5 * DPI;
                h = 11.0 * DPI;
            }
            else if (ps == PaperTypeEnum.PAPER_TYPE_A3)
            {
                w = 11.7 * DPI;
                h = 16.5 * DPI;
            }

            if (ot.Equals("LANDSCAPE"))
            {
                double temp = h;
                h = w;
                w = temp;
            }

            return (new Size(w, h));
        }

        private Size getFormPageSize()
        {
            Size s = new Size(rptCfg.PageWidthDot, rptCfg.PageHeightDot);
            return (s);
        }

        private Size getPageSizeParam()
        {
            if (rptparam != null)
            {
                return (getReportPageSize());
            }

            return (getFormPageSize());
        }

        public override Size PageSize
        {
            get
            {
                return (getPageSizeParam());
            }

            set
            {
            }
        }

        public virtual void SetDataSource(MBaseModel ds)
        {
        }

        protected Boolean isInRange(int page)
        {
            if (!isPageRange)
            {
                return (true);
            }

            if ((page >= fromPage) && (page <= toPage))
            {
                return (true);
            }

            return (false);
        }

        public Thickness Margin
        {
            get
            {
                double l = double.Parse(rptparam.GetFieldValue("PAPER_MARGIN_LEFT"));
                double t = double.Parse(rptparam.GetFieldValue("PAPER_MARGIN_TOP"));
                double r = double.Parse(rptparam.GetFieldValue("PAPER_MARGIN_RIGHT"));
                double b = double.Parse(rptparam.GetFieldValue("PAPER_MARGIN_BOTTOM"));


                return (new Thickness(l * DPI, t * DPI, r * DPI, b * DPI));
            }

            set
            {
            }
        }

        public virtual Boolean IsNewVersion()
        {
            return (false);
        }

        public CTable Parameter
        {
            get
            {
                return (rptparam);
            }

            set
            {
            }
        }

        protected void IncreasePage()
        {
            pcount++;
        }

        protected int GetCurrentPageCount()
        {
            return (pcount);
        }

        protected double addNewDataRow(Hashtable rowdef, CReportDataProcessingProperty rpp, String key, String format, int row, CTable data)
        {
            CRow d0 = (CRow)rowdef[key];
            CRow d00 = d0.Clone();

            ArrayList temps00 = getColumnDataTexts(format, row + 1, data);
            d00.FillColumnsText(temps00);
            rpp.AddReportRow(d00);

            return (d00.GetHeight());
        }

        protected double addNewFooterRow(Hashtable rowdef, CReportDataProcessingProperty rpp, String key, String format, string caption, ArrayList totals)
        {
            CRow ft = (CRow)rowdef[key];
            CRow ftr = ft.Clone();

            ArrayList subTotals = displayTotalTexts(format, totals, 1, caption);
            ftr.FillColumnsText(subTotals);

            rpp.AddReportRow(ftr);

            return (ftr.GetHeight());
        }

        protected void ConstructUIRows(UReportPage page, CReportDataProcessingProperty rpp)
        {
            ArrayList rows = rpp.ReportRows;

            foreach (CRow row in rows)
            {
                ConstructUIRow(page, row);                
            }
        }

        protected void ConstructUIRow(UReportPage page, CRow row)
        {
            double maxh = 0.00;
            Grid grd = new Grid();

            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(row.GetHeight());
            grd.RowDefinitions.Add(rd);

            if (excel != null)
            {
                excel.AddRow(row);
            }

            int cnt = row.GetColumnCount();
            for (int i = 0; i < cnt; i++)
            {
                CColumn clm = row.GetColumn(i);
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = clm.GetWidth();
                grd.ColumnDefinitions.Add(cd);
            }

            for (int i = 0; i < cnt; i++)
            {
                CColumn clm = row.GetColumn(i);
                
                Border bd = new Border();

                bd.BorderBrush = clm.GetBorderColor();
                bd.BorderThickness = clm.GetBorderThickness();
                bd.VerticalAlignment = VerticalAlignment.Stretch;
                bd.HorizontalAlignment = HorizontalAlignment.Stretch;

                TextBlock tblk = new TextBlock();

                tblk.Margin = row.GetMargin();
                tblk.HorizontalAlignment = clm.GetHorizontalAlignment();
                tblk.VerticalAlignment = clm.GetVertocalAlignment();
                tblk.Text = clm.GetText().Text;
                //tblk.TextWrapping = TextWrapping.Wrap;
                if (row.GetFontFamily() != null)
                {
                    tblk.FontFamily = row.GetFontFamily();
                }
                tblk.FontWeight = row.GetFontWeight();
                tblk.FontStyle = row.GetFontStyle();
                if (row.GetFontSize() > 0)
                {
                    tblk.FontSize = row.GetFontSize();
                }

                if (tblk.ActualHeight > maxh)
                {
                    maxh = tblk.ActualHeight;
                }

                bd.Child = tblk;

                Grid.SetColumn(bd, i);
                grd.Children.Add(bd);
            }

            page.AddRowPannel(grd);
        }
    }
}
