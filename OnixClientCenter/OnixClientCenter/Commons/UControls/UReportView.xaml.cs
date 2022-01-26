using System.Windows;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Collections;
using Onix.ClientCenter.Windows;
using Onix.Client.Report;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Reports;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UReportView : UserControl
    {
        private int zoomCnt = 100;
        private Hashtable reports = new Hashtable();
		private ReportType reportType = new ReportType();
        public string typeReport = "";

        public UReportView()
        {
            InitializeComponent();
        }

		public ReportType ReportType
		{
			get
			{
				return (reportType);
			}

			set
			{
                reportType = value;
			}
		}


		private void cmdRun_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.ValidateComboBox(lblReportName, cboReport, false))
            {
                return;
            }

            MMasterRef mr = (MMasterRef) cboReport.SelectedItem;
            if (mr == null)
            {
                return;
            }

            WinReportParam w = new WinReportParam(mr);
            w.Title = mr.Optional;
            w.ShowDialog();

            if (w.IsDone)
            {
                CBaseReport paginator = w.Paginator;
                docViewer.Document = paginator.GetFixedDocument();
                CUtil.LoadPageNavigateCombo(cboPageNo, paginator.PageCount);

                reports[mr.MasterID] = paginator;
            }
        }

        private void cmdZoomIn_Click(object sender, RoutedEventArgs e)
        {
            if (zoomCnt > 200)
            {
                return;
            }
             
            zoomCnt = zoomCnt + 5;
            docViewer.Zoom = zoomCnt;
        }

        private void cmdZoomOut_Click(object sender, RoutedEventArgs e)
        {
            if (zoomCnt == 5)
            {
                return;
            }

            zoomCnt = zoomCnt - 5;
            docViewer.Zoom = zoomCnt;
        }

        private void cmdPrint_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.ValidateComboBox(lblReportName, cboReport, false))
            {
                return;
            }

            MMasterRef mr = (MMasterRef)cboReport.SelectedItem;
            if (mr == null)
            {
                return;
            }

            CBaseReport paginator = (CBaseReport)reports[mr.MasterID];
            if (paginator == null)
            {
                return;
            }

            //docViewer.Print();

            PrintDialog dialog = new PrintDialog();
            dialog.PrintQueue = LocalPrintServer.GetDefaultPrintQueue();
            dialog.PrintTicket = dialog.PrintQueue.DefaultPrintTicket;
            dialog.PrintTicket.PageOrientation = paginator.GetPageOrientation();
            bool? result = dialog.ShowDialog();
            
            if (result == true)
            {
                FixedDocument fd = paginator.GetFixedDocument();
                dialog.PrintDocument(fd.DocumentPaginator, "");
            }
        }

        private void cmdPDF_Click(object sender, RoutedEventArgs e)
        {

        }

        public void LoadData()
        {
            Hashtable map = new Hashtable()
            {
                {ReportType.ReportInv, ReportGroupEnum.ReportGroupInventory},
                {ReportType.ReportSale, ReportGroupEnum.ReportGroupSale},
                {ReportType.ReportCash, ReportGroupEnum.ReportGroupCash},
                {ReportType.ReportPurchase, ReportGroupEnum.ReportGroupPurchase},
                {ReportType.ReportGeneral, ReportGroupEnum.ReportGroupGeneral},
                {ReportType.ReportHr, ReportGroupEnum.ReportGroupHr},
            };

            ReportGroupEnum group = (ReportGroupEnum) map[ReportType];
            CHelper.LoadReportName(group, cboReport);

            lblReportName.Content = typeReport.ToString();
        }


        private void cmdFitHeigh_Click(object sender, RoutedEventArgs e)
        {
            docViewer.FitToHeight();
            zoomCnt = (int)docViewer.Zoom;            
        }

        private void cmdFitWidth_Click(object sender, RoutedEventArgs e)
        {
            docViewer.FitToWidth();
            zoomCnt = (int) docViewer.Zoom;
        }

        private void cboPageNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cbo = (ComboBox)sender;

            MChunkNavigate item = (MChunkNavigate)cbo.SelectedItem;
            if (item == null)
            {
                return;
            }

            int pageno = int.Parse(item.PageNo);
            docViewer.GoToPage(pageno);
        }

        private void cboReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MMasterRef mr = (MMasterRef)cboReport.SelectedItem;
            if (mr == null)
            {
                docViewer.Document = null;
                CUtil.LoadPageNavigateCombo(cboPageNo, 0);

                return;
            }

            CBaseReport paginator = (CBaseReport) reports[mr.MasterID];
            if (paginator == null)
            {
                docViewer.Document = null;
                CUtil.LoadPageNavigateCombo(cboPageNo, 0);

                return;
            }

            docViewer.Document = paginator.GetFixedDocument();
            CUtil.LoadPageNavigateCombo(cboPageNo, paginator.PageCount);
        }

        private void cmdExcel_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.ValidateComboBox(lblReportName, cboReport, false))
            {
                return;
            }

            MMasterRef mr = (MMasterRef)cboReport.SelectedItem;
            if (mr == null)
            {
                return;
            }

            CBaseReport paginator = (CBaseReport)reports[mr.MasterID];
            if (paginator == null)
            {
                return;
            }

            if (!paginator.IsNewVersion())
            {
                return;
            }

            CExcelRenderer excel = paginator.GetExcelRenderer();
            WinProgressRenderer w = new WinProgressRenderer(excel);
            w.Show();
        }
    }
}
