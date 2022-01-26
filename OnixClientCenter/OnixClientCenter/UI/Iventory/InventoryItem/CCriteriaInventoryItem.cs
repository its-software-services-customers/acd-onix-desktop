using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Wis.WsClientAPI;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Factories;

namespace Onix.ClientCenter.UI.Inventory.InventoryItem
{
    public class CCriteriaInventoryItem : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        private Boolean isForMultiSelect = false;

        public CCriteriaInventoryItem() : base(new MInventoryItem(new CTable("")), "CCriteriaInventoryItem")
        {
        }

        public Boolean IsForMultiSelect
        {
            get
            {
                return (isForMultiSelect);
            }

            set
            {
                isForMultiSelect = value;
            }
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();

            if (!CMasterReference.IsCategoryItemLoaded())
            {
                CMasterReference.LoadItemCategoryPathList(true, null);
            }
        }

        public override void SetActionEnable(Boolean en)
        {
            isActionEnable = en;
        }

        public override void SetCheckUncheckHandler(RoutedEventHandler chdler, RoutedEventHandler uhdler)
        {
            checkHandler = chdler;
            unCheckHandler = uhdler;
        }

        #region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuMovement", "item_balance_check", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        private void cbxSelect_Checked(object sender, RoutedEventArgs e)
        {
            if (checkHandler != null)
            {
                checkHandler(sender, e);
            }
        }

        private void cbxSelect_UnChecked(object sender, RoutedEventArgs e)
        {
            if (unCheckHandler != null)
            {
                unCheckHandler(sender, e);
            }
        }

        private void createGridColumns()
        {
            //0.05 0.05, 0.10, 0.40, 0.10, 0.15, 0.15
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colItemCode", "item_code", "ItemCode", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colItenName", "item_name_thai", "ItemNameThai", 40, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colUnit", "item_uom", "ItemUOMName", 10, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colItemCategory", "item_category", "ItemCategoryName", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colItemType", "item_type", "ItemTypeName", 15, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_code"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ItemCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_name_thai"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ItemNameThai", ""));

            if (!isForMultiSelect)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_name_en"));
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ItemNameEng", ""));

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_type"));

                CCriteriaEntry itemTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ItemTypeObj", "");
                itemTypeEntry.SetComboItemSources("ItemTypes", "Description");
                AddCriteriaControl(itemTypeEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "item_uom"));

                CCriteriaEntry itemUOMEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ItemUOMObj", "");
                itemUOMEntry.SetComboItemSources("Uoms", "Description");
                AddCriteriaControl(itemUOMEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsVatEligible", "vat_eligible"));

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsForBorrow", "borrow_eligible"));
            }
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAdd", "ADMIN_EDIT", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_VIEW"))
            {
                return;
            }

            MInventoryItem m = (MInventoryItem) currentObj;

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("inventory_item");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = m.ItemCategory;
            param.Mode = "E";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditInventoryItem", param);
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, null);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {           
            items = OnixWebServiceAPI.GetListAPI("GetInventoryItemList", "ITEM_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MInventoryItem v = new MInventoryItem(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteInventoryItem");
            if (rCount > 0)
            {
                CMasterReference.Instance.LoadItemCategoriesTree();
            }

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("INVENTORY_ITEM_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("inventory_item");
            CWinLoadParam param = new CWinLoadParam();
            
            param.Caption = caption;
            param.GenericType = "";
            param.Mode = "A";
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;
            FactoryWindow.ShowWindow("WinAddEditInventoryItem", param);            
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                showEditWindow();
            }
            else if (name.Equals("mnuMovement"))
            {
                if (!CHelper.VerifyAccessRight("INVENTORY_BALANCE_VIEW"))
                {
                    return;
                }

                MBaseModel v = currentObj;

                WinItemBalanceInfo w = new WinItemBalanceInfo();
                w.ViewData = (MInventoryItem)v;
                w.Caption = (String)mnu.Header;
                w.Mode = "E";
                w.ShowDialog();
            }
            else if (name.Equals("mnuCopy"))
            {
                CUtil.EnableForm(false, ParentControl);
                CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CopyInventoryItem", currentObj.GetDbObject());

                if (newobj != null)
                {
                    MInventoryItem ivd = new MInventoryItem(newobj);
                    ItemAddedEvent(ivd, e);
                    CMasterReference.Instance.LoadItemCategoriesTree();
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                CUtil.EnableForm(true, ParentControl);
            }
        }
#endregion
    }
}
