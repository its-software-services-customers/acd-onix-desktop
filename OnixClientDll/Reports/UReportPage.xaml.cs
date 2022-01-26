using System.Windows.Controls;

namespace Onix.Client.Report
{
    public partial class UReportPage : UserControl
    {
        public UReportPage()
        {
            InitializeComponent();
        }

        public void AddRowPannel(Grid grd)
        {
            pnlMain.Children.Add(grd);
        }
    }
}
