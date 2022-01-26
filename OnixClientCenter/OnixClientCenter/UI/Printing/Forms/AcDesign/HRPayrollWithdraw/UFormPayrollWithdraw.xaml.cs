using System;
using System.Windows;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Forms.AcDesign.HRPayrollWithdraw
{
    public partial class UFormPayrollWithdraw : UFormBase
    {
        private MVPayrollDocument payrollDoc = null;
        private MVPayrollDocumentItem item = null;
        private String[] format1Widths = new String[20];
        private String[] format2Widths = new String[20];

        public UFormPayrollWithdraw(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MVPayrollDocument(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            payrollDoc = (MVPayrollDocument) model;

            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            String format1 = rptConfig.GetConfigValue("Format1");
            format1Widths = format1.Split('|');

            String format2 = rptConfig.GetConfigValue("Format2");
            format2Widths = format2.Split('|');

            init();

            int idx = pageNo - 1;

            item = payrollDoc.GetItemByIndex(idx);
            if (item == null)
            {
                item = new MVPayrollDocumentItem(new Wis.WsClientAPI.CTable(""));
            }

            numberTextAmount = item.GrandTotalAmount;

            item.InitializeAfterLoaded();

            DataContext = model;
            InitializeComponent();
        }   
        
        public MEmployee EmployeeObj
        {
            get
            {
                return (item.EmployeeObj);
            }

            set
            {
            }
        }

        public MVPayrollDocumentItem ItemObj
        {
            get
            {
                return (item);
            }

            set
            {
            }
        }

        public String Filler1Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler1Height");
                return (p);
            }
        }

        public String Filler2Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler2Height");
                return (p);
            }
        }

        public String Filler3Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler3Height");
                return (p);
            }
        }

        public String Filler4Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler4Height");
                return (p);
            }
        }

        public String Filler5Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler5Height");
                return (p);
            }
        }

        public String Filler6Height
        {
            get
            {
                string p = rptConfig.GetConfigValue("Filler6Height");
                return (p);
            }
        }

        public Boolean DisplayGridFlag
        {
            get
            {
                string p = rptConfig.GetConfigValue("DisplayGridFlag");
                return (p.Equals("Y"));
            }
        }

        private double getThickness()
        {
            string p = rptConfig.GetConfigValue("DisplayGridFlag");
            if (p.Equals("Y"))
            {
                return (1);
            }

            return (0);
        }

        public Thickness BorderLeftThickness
        {
            get
            {
                double thickness = getThickness();
                Thickness t = new Thickness(thickness, 0, 0, 0);
                return (t);
            }
        }

        public Thickness BorderRightThickness
        {
            get
            {
                double thickness = getThickness();
                Thickness t = new Thickness(0, 0, thickness, 0);
                return (t);
            }
        }

        public Thickness BorderLeftRightThickness
        {
            get
            {
                double thickness = getThickness();
                Thickness t = new Thickness(thickness, 0, thickness, 0);
                return (t);
            }
        }

        public Thickness BorderOutsideThickness
        {
            get
            {
                double thickness = getThickness();
                Thickness t = new Thickness(thickness);
                return (t);
            }
        }

        private GridLength stringToLength(String w)
        {
            if (w.Equals("*"))
            {
                return (new GridLength(1, GridUnitType.Star));
            }
            else if (w.Equals("auto"))
            {
                return (new GridLength(1, GridUnitType.Auto));
            }

            double len = CUtil.StringToDouble(w);
            return (new GridLength(len));
        }

        public GridLength Format1_0_Width
        {
            get
            {
                return (stringToLength(format1Widths[0]));
            }
        }

        public GridLength Format1_1_Width
        {
            get
            {
                return (stringToLength(format1Widths[1]));
            }
        }

        public GridLength Format1_2_Width
        {
            get
            {
                return (stringToLength(format1Widths[2]));
            }
        }

        public GridLength Format1_3_Width
        {
            get
            {
                return (stringToLength(format1Widths[3]));
            }
        }

        public GridLength Format1_4_Width
        {
            get
            {
                return (stringToLength(format1Widths[4]));
            }
        }

        public GridLength Format1_5_Width
        {
            get
            {
                return (stringToLength(format1Widths[5]));
            }
        }

        public GridLength Format1_6_Width
        {
            get
            {
                return (stringToLength(format1Widths[6]));
            }
        }

        public GridLength Format1_7_Width
        {
            get
            {
                return (stringToLength(format1Widths[7]));
            }
        }

        public GridLength Format1_8_Width
        {
            get
            {
                return (stringToLength(format1Widths[8]));
            }
        }

        public GridLength Format1_9_Width
        {
            get
            {
                return (stringToLength(format1Widths[9]));
            }
        }

        public GridLength Format1_10_Width
        {
            get
            {
                return (stringToLength(format1Widths[10]));
            }
        }

        public GridLength Format1_11_Width
        {
            get
            {
                return (stringToLength(format1Widths[11]));
            }
        }

        public GridLength Format1_12_Width
        {
            get
            {
                return (stringToLength(format1Widths[12]));
            }
        }

        public GridLength Format1_13_Width
        {
            get
            {
                return (stringToLength(format1Widths[13]));
            }
        }

        public GridLength Format1_14_Width
        {
            get
            {
                return (stringToLength(format1Widths[14]));
            }
        }

        public GridLength Format2_0_Width
        {
            get
            {
                return (stringToLength(format2Widths[0]));
            }
        }

        public GridLength Format2_1_Width
        {
            get
            {
                return (stringToLength(format2Widths[1]));
            }
        }

        public GridLength Format2_2_Width
        {
            get
            {
                return (stringToLength(format2Widths[2]));
            }
        }

        public GridLength Format2_3_Width
        {
            get
            {
                return (stringToLength(format2Widths[3]));
            }
        }

        public GridLength Format2_4_Width
        {
            get
            {
                return (stringToLength(format2Widths[4]));
            }
        }

        private String BankBranch()
        {
            string p = rptConfig.GetConfigValue("BankBranchText");
            return (p);
        }
    }
}
