using System;
using Onix.Client.Model;
using Onix.Client.Report;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Forms.AcDesign.HREmployeeLeave
{
    public partial class UFormEmployeeLeave : UFormBase
    {
        private MEmployeeLeave leaveDoc = null;

        public UFormEmployeeLeave(MBaseModel model, int page, int totalPage, MReportConfig cfg, CReportPageParam param)
        {
            if (model == null)
            {
                model = new MEmployeeLeave(new Wis.WsClientAPI.CTable(""));
            }

            dataSource = model;
            leaveDoc = (MEmployeeLeave) model;

            pageNo = page;
            pageCount = totalPage;
            pageParam = param;
            rptConfig = cfg;

            init();

            int idx = pageNo - 1;

            //item = payrollDoc.LeaveRecords(idx);
            //if (item == null)
            //{
            //    item = new MLeaveRecord(new Wis.WsClientAPI.CTable(""));
            //}

            //item.InitializeAfterLoaded();
            //PopulateDummyRecords(leaveDoc);

            DataContext = leaveDoc;
            InitializeComponent();        
        }
    }
}
