using System;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Forms.AcDesign.HRPayrollSlip
{
    public partial class UFormPayrollSlip : UFormBase
    {
        private MVPayrollDocument payrollDoc = null;
        private MVPayrollDocumentItem item = null;

        public UFormPayrollSlip(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
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
    }
}
