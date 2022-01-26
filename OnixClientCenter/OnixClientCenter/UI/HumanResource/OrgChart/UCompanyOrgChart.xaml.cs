using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Factories;

namespace Onix.ClientCenter.UI.HumanResource.OrgChart
{
    public partial class UCompanyOrgChart : UserControl
    {
        private MVVirtialDirectory vw = new MVVirtialDirectory();
        private MVOrgChart currentViewObj = null;

        public UCompanyOrgChart()
        {
            DataContext = vw;
            InitializeComponent();
        }

        public void InitUserControl()
        {
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TrvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }

        private void LsvMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvMain.SelectedItems.Count == 1)
            {
                MVOrgChart obj = (MVOrgChart)lsvMain.SelectedItems[0];                
                vw.AddDirectoryPath(obj);
                vw.CurrentDirectory.ParentDirectoryID = obj.DirectoryID;
                QueryData(vw.CurrentDirectory.GetDbObject());                
            }
        }

        private void CbkRemove_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MVOrgChart) btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void showWindow(CWinOrgChartLoadParam param, String className)
        {
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
        }

        private void MnuMasterRefEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_ORGCHART_VIEW"))
            {
                return;
            }

            CWinOrgChartLoadParam param = new CWinOrgChartLoadParam();
           
            param.Mode = "E";
            param.ActualView = currentViewObj;
            param.GenericType = currentViewObj.Category;
            param.ParentItemSources = vw.CurrentItemSource;
            param.CurrentPaths = vw.CurrentPath;
            FactoryWindow.ShowWindow("WinAddEditOrgChart", param);
        }

        private void LsvMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double w = (lsvMain.ActualWidth * 1) - 35;
            double[] ratios = new double[4] { 0.05, 0.05, 0.40, 0.50 };
            CUtil.ResizeGridViewColumns(lsvMain.View as GridView, ratios, w);
        }

        private ArrayList submitCommand(CTable table)
        {
            CUtil.EnableForm(false, this);
            ArrayList itemsList = null;
            itemsList = OnixWebServiceAPI.GetListAPI("GetVirtualDirectoryList", "DIRECTORY_LIST", table);
            CUtil.EnableForm(true, this);

            return (itemsList);
        }

        public void QueryData(CTable tb)
        {
            ArrayList itemsList = submitCommand(tb);
            if (itemsList == null)
            {
                return;
            }

            vw.CurrentItemSource.Clear();
            foreach (CTable o in itemsList)
            {
                MVOrgChart v = new MVOrgChart(o);
                vw.CurrentItemSource.Add(v);
            }
        }

        private void CmdSearch_Click(object sender, RoutedEventArgs e)
        {
            QueryData(vw.CurrentDirectory.GetDbObject());
        }

        private void CmdNavigate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_ORGCHART_ADD"))
            {
                return;
            }

            CWinOrgChartLoadParam param = new CWinOrgChartLoadParam();

            param.Mode = "A";
            param.GenericType = vw.CurrentDirectory.Category;
            param.ParentItemSources = vw.CurrentItemSource;
            param.CurrentPaths = vw.CurrentPath;
            FactoryWindow.ShowWindow("WinAddEditOrgChart", param);

            CMasterReference.LoadEmployeeDepartments();
            CMasterReference.LoadEmployeePositions();
        }

        private void CmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_ORGCHART_DELETE"))
            {
                return;
            }

            int rowCount = vw.CurrentItemSource.Count;

            CHelper.DeleteSelectedItems((ObservableCollection<MBaseModel>)lsvMain.ItemsSource, OnixWebServiceAPI.DeleteAPI, rowCount.ToString(), "DeleteVirtualDirectory");

            CmdSearch_Click(sender, e);

            CMasterReference.LoadEmployeeDepartments();
            CMasterReference.LoadEmployeePositions();
        }

        private void CboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CmdTest_Click(object sender, RoutedEventArgs e)
        {
            //vw.AddDirectoryPath("HELLO1");
        }

        private void CmdUp_Click(object sender, RoutedEventArgs e)
        {
            vw.ChangeDirectoryUp();

            int last = vw.CurrentPath.Count - 1;
            if (last < 0)
            {
                vw.CurrentDirectory.ParentDirectoryID = "";
            }
            else
            {
                MVOrgChart obj = vw.CurrentPath[last];
                vw.CurrentDirectory.ParentDirectoryID = obj.DirectoryID;
            }

            QueryData(vw.CurrentDirectory.GetDbObject());
        }

        private void UPathLabels_DirectoryClicked(object sender, RoutedEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            MVOrgChart obj = (MVOrgChart) tb.Tag;
            obj.ParentDirectoryID = obj.DirectoryID;

            vw.NavigateToDirectory(obj);

            vw.CurrentDirectory.ParentDirectoryID = obj.DirectoryID;
            QueryData(vw.CurrentDirectory.GetDbObject());
        }
    }
}
