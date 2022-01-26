using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Factories;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Inventory.InventoryItem
{
    public partial class UInventoryItem : UserControl
    {
        private ObservableCollection<MBaseModel> itemsSourceGrid = new ObservableCollection<MBaseModel>();
        private MItemCategory cit = null;
        private int typ = 1;
        private CTable lastQuery = null;
        public CTable lastObjectReturned = null;
        private MBaseModel currentViewObj = null;

        private int rowCount;

        public UInventoryItem(int type)
        {
            typ = type;
            InitializeComponent();
        }

        private ArrayList submitCommand(CTable table)
        {
            ArrayList itemsList = null;

            itemsList = OnixWebServiceAPI.GetInventoryItemList(table);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            return (itemsList);
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
                MInventoryItem v = new MInventoryItem(o);

                v.RowIndex = idx;
                itemsSourceGrid.Add(v);
                idx++;
            }

            lsvMain.ItemsSource = itemsSourceGrid;
            rowCount = CUtil.StringToInt(lastObjectReturned.GetFieldValue("EXT_RECORD_COUNT"));
            lblTotal.Content = CUtil.FormatInt(lastObjectReturned.GetFieldValue("EXT_RECORD_COUNT"));
            CUtil.LoadChunkNavigateCombo(cboNavigate, lastObjectReturned, tb.GetFieldValue("EXT_CHUNK_NO"));
        }

        public void InitUserControl()
        {
        }

        public void SetCategoryItem(MItemCategory ci)
        {
            cit = ci;
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_ADD"))
            {
                return;
            }

            if ((typ == 2) && (cit == null))
            {
                CHelper.ShowErorMessage("", "ERROR_ITEM_ADD_PLS", null);
                return;
            }

            if ((typ == 2) && !cit.ChildCount.Equals("0"))
            {
                CHelper.ShowErorMessage(cit.CategoryName, "ERROR_NOT_LAST_CHILD", null);
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("inventory_item");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = cit.ItemCategoryID;
            param.Mode = "A";
            param.ParentItemSources = itemsSourceGrid;
            Boolean isOK = FactoryWindow.ShowWindow("WinAddEditInventoryItem", param);
            
            if (isOK)
            {
                rowCount = rowCount + 1;
                lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            }
        }

        private void cmdDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_DELETE"))
            {
                return;
            }

            int rCount = CHelper.DeleteSelectedItems(itemsSourceGrid, OnixWebServiceAPI.DeleteInventoryItem ,rowCount.ToString());
            if (rCount > 0)
            {
                rowCount = rCount;
                CMasterReference.Instance.LoadItemCategoriesTree();
            }
            lblTotal.Content = CUtil.FormatInt((rowCount).ToString());
        }

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_VIEW"))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("inventory_item");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = "";
            param.Mode = "E";
            param.ActualView = currentViewObj;
            param.ParentItemSources = itemsSourceGrid;
            FactoryWindow.ShowWindow("WinAddEditInventoryItem", param);
        }

        private void mnuSubMenu_Click(object sender, EventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuItemEdit"))
            {
                showEditWindow();
            }
            else if (name.Equals("mnuItemBalanceCheck"))
            {
                if (!CHelper.VerifyAccessRight("INVENTORY_BALANCE_VIEW"))
                {
                    return;
                }

                MBaseModel v = currentViewObj; 

                WinItemBalanceInfo w = new WinItemBalanceInfo();
                w.ViewData = (MInventoryItem) v;
                w.Caption = (String)mnu.Header;
                w.Mode = "E";
                //w.ParentItemSource = grdViewInventoryItems.GridItemsSource;
                w.ShowDialog();
            }
        }

        private void cbkRemove_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void cbkRemove_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MBaseModel)btn.Tag;
            btn.ContextMenu.Visibility = Visibility.Visible;
            btn.ContextMenu.IsOpen = true;
        }

        private void lsvMain_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lsvMain.SelectedItems.Count == 1)
            {
                currentViewObj = (MBaseModel) lsvMain.SelectedItems[0];
                showEditWindow();
            }
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

        private void mnuItemCopy_Click(object sender, RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);
            CTable newobj = OnixWebServiceAPI.CopyInventoryItem(currentViewObj.GetDbObject());

            if (newobj != null)
            {
                MInventoryItem ivd = new MInventoryItem(newobj);
                itemsSourceGrid.Insert(0, ivd);

                CMasterReference.Instance.LoadItemCategoriesTree();
                rowCount = rowCount + 1;
                lblTotal.Content = CUtil.FormatInt(rowCount.ToString());
            }
            else
            {
                //Error here
                CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
            }

            CUtil.EnableForm(true, this);
        }

        private GridView getGridView()
        {
            GridView gv = lsvMain.View as GridView;
            return (gv);
        }

        private void grdView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GridView grdv = getGridView();

            double w = (this.ActualWidth * 1) - 35;
            double[] ratios = new double[7] { 0.05, 0.05, 0.10, 0.40, 0.10, 0.15, 0.15 };
            CUtil.ResizeGridViewColumns(grdv, ratios, w);
        }
    }
}
