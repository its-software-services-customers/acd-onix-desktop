using System;
using System.Windows;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Forms.AcDesign.HRPayrollDeposit
{
    public partial class UFormPayrollDeposit : UFormBase
    {
        private MVPayrollDocument payrollDoc = null;
        private MVPayrollDocumentItem item = null;

        public UFormPayrollDeposit(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
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
    }
}
