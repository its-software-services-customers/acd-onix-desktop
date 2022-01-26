using System;
using System.Windows;
using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Collections;
using System.Collections.ObjectModel;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.UI.General.MasterReference;
using Onix.ClientCenter.Commons.Factories;

namespace Onix.ClientCenter.UControls
{
    public partial class UMasterRef : UserControl
    {
        private ObservableCollection<MBaseModel> itemsSourceGrid = new ObservableCollection<MBaseModel>();
        private MBaseModel currentViewObj = null;
        private CTable lastQuery = null;
        private CTable lastObjectReturned = null;
        private Label lbl = null;
        private ComboBox cbo = null;
        private int rowCount = 0;

        public UMasterRef()
        {
            InitializeComponent();
        }

        private ArrayList submitCommand(CTable table)
        {
            refreshMasterRef();

            ArrayList itemsList = null;
            itemsList = OnixWebServiceAPI.GetMasterRefList(table);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            return (itemsList);
        }

        public void SetLabelCombobox(Label l, ComboBox c)
        {
            lbl = l;
            cbo = c;
        }

        private void refreshMasterRef()
        {
            CMasterReference.LoadAllMasterRefItems(OnixWebServiceAPI.GetMasterRefList);
        }

        public void QueryData(CTable tb)
        {
            lastQuery = tb;
            ArrayList itemsList = submitCommand(tb);
            if (itemsList == null)
            {
                return;
            }

            itemsSourceGrid = null;
            itemsSourceGrid = new ObservableCollection<MBaseModel>();

            int idx = 0;
            foreach (CTable o in itemsList)
            {
                MMasterRef v = new MMasterRef(o);

                v.RowIndex = idx;
                itemsSourceGrid.Add(v);
                idx++;
            }

            lsvMain.ItemsSource = itemsSourceGrid;
            //rowCount = CUtil.StringToInt(lastObjectReturned.GetFieldValue("EXT_RECORD_COUNT"));
            CUtil.LoadChunkNavigateCombo(cboNavigate, lastObjectReturned, tb.GetFieldValue("EXT_CHUNK_NO"));
        }

        public void InitUserControl()
        {
        }

        private GridView getGridView()
        {
            GridView gv = lsvMain.View as GridView; //lsv.View as GridView;
            return (gv);
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView gv = getGridView();

            double w = (this.ActualWidth * 1) - 35;
            double[] ratios = new double[5] { 0.05, 0.05, 0.20, 0.50, 0.20 };
            CUtil.ResizeGridViewColumns(gv, ratios, w);
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_MASTER_ADD"))
            {
                return;
            }

            Boolean result = CHelper.ValidateComboBox(lbl, cbo, false);
            if (!result)
            {
                return;
            }

            MMasterRef v = (MMasterRef)cbo.SelectedItem;

            String caption = CLanguage.getValue("add");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = v.MasterID;
            param.Mode = "A";
            param.ParentItemSources = (ObservableCollection<MBaseModel>) lsvMain.ItemsSource;            
            Boolean isOK = FactoryWindow.ShowWindow("WinAddEditMasterRef", param);

            if (isOK)
            {
                refreshMasterRef();
                rowCount = rowCount + 1;
                //lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            }
        }

        protected static Boolean showWindow(CWinLoadParam param, String className)
        {
            WinBase cr = (WinBase)Activator.CreateInstance(Type.GetType(className), new object[] { param });
            cr.ShowDialog();
            return (cr.IsOKClick);
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("GENERAL_MASTER_DELETE"))
            {
                return;
            }

            int rCount = CHelper.DeleteSelectedItems((ObservableCollection<MBaseModel>)lsvMain.ItemsSource, OnixWebServiceAPI.DeleteMasterRef, rowCount.ToString());
            if (!rCount.Equals(0))
            {
                rowCount = rCount;
            }
            rowCount = rCount;
            refreshMasterRef();
        }

        private void cbkRemove_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cmdNavigate_Click(object sender, RoutedEventArgs e)
        {
            int idx = cboNavigate.SelectedIndex;
            if (idx < 0)
            {
                return;
            }

            MChunkNavigate v = (MChunkNavigate)cboNavigate.SelectedItem;
            lastQuery.SetFieldValue("EXT_CHUNK_NO", v.ChunkNo);
            CUtil.EnableForm(false, this);
            QueryData(lastQuery);
            CUtil.EnableForm(true, this);
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MBaseModel)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("GENERAL_MASTER_VIEW"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = (currentViewObj as MMasterRef).RefType;
            param.Mode = "E";
            param.ActualView = currentViewObj;
            Boolean isOK = FactoryWindow.ShowWindow("WinAddEditMasterRef", param);

            if (isOK)
            {
                refreshMasterRef();
                rowCount = rowCount + 1;
                //lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            }

            refreshMasterRef();
        }

        private void mnuMasterRefEdit_Click(object sender, RoutedEventArgs e)
        {
            showEditWindow();
        }

        private void lsvMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvMain.SelectedItems.Count == 1)
            {
                currentViewObj = (MMasterRef)lsvMain.SelectedItems[0];
                showEditWindow();
            }
        }
    }
}
