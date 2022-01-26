using System.Windows.Controls;
using Wis.WsClientAPI;
using System.Windows;
using Onix.Client.Controller;
using Onix.ClientCenter.Windows;
using Onix.Client.Model;
using Onix.Client.Helper;
using System;
using Onix.ClientCenter.Commons.Utils;

namespace Onix.ClientCenter.UI.Inventory.InventoryItem
{
    public partial class UInventoryItemByCategorySearch : UserControl
    {
        private UserControl provider = null;
        private MInventoryItem param = new MInventoryItem(new CTable("ITEM"));
        private MItemCategory cvItemObj = new MItemCategory(new CTable("ITEM_CATEGORY"));
        private MPackagePrice vpkp = null;

        public UInventoryItemByCategorySearch()
        {
            CUtil.EnableForm(false, this);
            CMasterReference.Instance.InitItemCategoriesTree();
            CUtil.EnableForm(true, this);

            DataContext = param;
            InitializeComponent();            
        }

        public void SetProvider(UserControl u)
        {
            provider = u;
        }

        public void SetCategoryItem(MPackagePrice pkp)
        {
            vpkp = pkp;
        }

        public void LoadData()
        {
            CUtil.EnableForm(false, this);
            CMasterReference.LoadItemCategoryPathList(true, null);
            CUtil.EnableForm(true, this);
        }

        private void cmdSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.IsEnabled = false;

            CUtil.EnableForm(false, this);
            param.ChunkNo = "";
            (provider as UInventoryItem).QueryData(param.GetDbObject());
            CUtil.EnableForm(true, this);

            this.IsEnabled = true;
        }

        private void mnuHeadAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("ITEM_CATEGORY_ADD"))
                return;

            MenuItem mnu = (sender as MenuItem);
            MItemCategory currentViewHeadObj = new MItemCategory(new CTable("ITEM_CATEGORY"));

            WinAddEditItemCategory w = new WinAddEditItemCategory();
            w.Caption = (String)mnu.Header;
            w.ViewData = currentViewHeadObj;
            w.Mode = "A";
            w.ShowDialog();
        }

        private void mnuAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("ITEM_CATEGORY_ADD"))
                return;

            if (!cvItemObj.ItemCount.ToString().Equals("0"))
            {
                CHelper.ShowErorMessage(cvItemObj.CategoryName, "ERROR_ITEM_ADD", null);
                return;
            }

            MenuItem mnu = (sender as MenuItem);

            WinAddEditItemCategory w = new WinAddEditItemCategory();
            w.Caption = (String)mnu.Header;
            w.ViewData = cvItemObj;
            w.Mode = "A";
            w.ShowDialog();
        }

        private void mnuEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("ITEM_CATEGORY_EDIT"))
                return;

            MenuItem mnu = (sender as MenuItem);

            WinAddEditItemCategory w = new WinAddEditItemCategory();
            w.Caption = (String)mnu.Header;
            w.ViewData = cvItemObj;
            w.Mode = "E";
            w.ShowDialog();
        }

        private void mnuDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("ITEM_CATEGORY_DELETE"))
                return;

            if (cvItemObj.ChildCount.ToString().Equals("0")
                && cvItemObj.ItemCount.ToString().Equals("0"))
            {
                if (CHelper.AskConfirmDeleteItem(cvItemObj.CategoryName.ToString()))
                {
                    CUtil.EnableForm(false, this);
                    OnixWebServiceAPI.DeleteItemCategory(cvItemObj.GetDbObject());
                    CUtil.EnableForm(true, this);
                    CMasterReference.Instance.DeleteCategoryFromTree(cvItemObj);
                }
            }
            else
            {
                CHelper.ShowErorMessage(cvItemObj.CategoryName, "ERROR_ITEM_DELETE", null);
            }
        }

        private void trvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            cvItemObj = (MItemCategory) trvMain.SelectedItem;
            if (cvItemObj == null)
            {
                return;
            }

            param.ItemCategory = cvItemObj.ItemCategoryID;

            int ccnt = CUtil.StringToInt(cvItemObj.ChildCount);

            if (provider != null)
            {
                (provider as UInventoryItem).SetCategoryItem(cvItemObj);
                CUtil.EnableForm(false, this);
                param.ChunkNo = "";
                if (ccnt <= 0)
                {
                    (provider as UInventoryItem).QueryData(param.GetDbObject());
                }
                CUtil.EnableForm(true, this);
            }            
        }

        private void mnuRefresh_Click(object sender, RoutedEventArgs e)
        {
            CUtil.EnableForm(false, this);
            CMasterReference.Instance.LoadItemCategoriesTree();
            CUtil.EnableForm(true, this);
        }

    }
}
