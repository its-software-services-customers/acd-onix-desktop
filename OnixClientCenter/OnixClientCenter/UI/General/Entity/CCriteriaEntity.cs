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

namespace Onix.ClientCenter.UI.General.Entity
{
    public class CCriteriaEntity : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaEntity() : base(new MEntity(new CTable("")), "CCriteriaEntity")
        {

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

        public override void Initialize(string keyword)
        {
            if (keyword.Equals("mnuCustomer"))
            {
                Init("1");
            }
            else if (keyword.Equals("mnuSupplier"))
            {
                Init("2");
            }
        }

        #region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuARAPMovement", "ar_ap_movement", new RoutedEventHandler(mnuContextMenu_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        private String getKeyMap(String name)
        {
            Hashtable map = new Hashtable();
            map["code"] = 0;
            map["name"] = 1;
            map["type"] = 2;
            map["group"] = 3;
            map["group"] = 3;
            map["type_array"] = 4;
            map["group_array"] = 5;

            String[] arr1 = { "customer_code", "customer_name", "customer_type", "customer_group", "CustomerTypes", "CustomerGroups" };
            String[] arr2 = { "supplier_code", "supplier_name", "supplier_type", "supplier_group", "SupplierTypes", "SupplierGroups" };

            String[] arr = arr1;
            if (eType.Equals("2"))
            {
                arr = arr2;
            }

            int idx = (int) map[name];
            String key = arr[idx];

            return (key);
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
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colCustomerCode", getKeyMap("code"), "EntityCode", 10, HorizontalAlignment.Left);
            c1.Sortable = true;
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colCustomerName", getKeyMap("name"), "EntityName", 30, HorizontalAlignment.Left);
            c2.Sortable = true;
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colCustomerType", getKeyMap("type"), "EntityTypeName", 15, HorizontalAlignment.Left);
            c3.Sortable = true;
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colCustomerGroup", getKeyMap("group"), "EntityGroupName", 15, HorizontalAlignment.Left);
            c4.Sortable = true;
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colPhone", "telephone", "Phone", 20, HorizontalAlignment.Left);
            c5.Sortable = true;
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("code")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("name")));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "telephone"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "Phone", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "id_card_no"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "IDCardNumber", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("type")));

            CCriteriaEntry cstTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "EntityTypeObj", "");
            cstTypeEntry.SetComboItemSources(getKeyMap("type_array"), "Description");
            AddCriteriaControl(cstTypeEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", getKeyMap("group")));

            CCriteriaEntry cstGroupEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "EntityGroupObj", "");
            cstGroupEntry.SetComboItemSources(getKeyMap("group_array"), "Description");
            AddCriteriaControl(cstGroupEntry);
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

        private void showMovementHistoryWindow(String captionKey)
        {
            CCriteriaArApMovementHistory cr = new CCriteriaArApMovementHistory();
            cr.SetActionEnable(false);
            cr.SetDefaultData(currentObj);
            cr.Init(eType);

            WinMovementHistory w = new WinMovementHistory(cr, CLanguage.getValue(captionKey));
            w.ShowDialog();
        }


        private String getPermissionKey(String mode, String eType)
        {
            String tmp = "";

            if (eType.Equals("1"))
            {
                if (mode.Equals("A"))
                {
                    tmp = "GENERAL_CUSTOMER_ADD";
                }
                else if (mode.Equals("E"))
                {
                    tmp = "GENERAL_CUSTOMER_VIEW";
                }
                else if (mode.Equals("D"))
                {
                    tmp = "GENERAL_CUSTOMER_DELETE";
                }
            }
            else
            {
                if (mode.Equals("A"))
                {
                    tmp = "GENERAL_SUPPLIER_ADD";
                }
                else if (mode.Equals("E"))
                {
                    tmp = "GENERAL_SUPPLIER_VIEW";
                }
                else if (mode.Equals("D"))
                {
                    tmp = "GENERAL_SUPPLIER_DELETE";
                }
            }

            return (tmp);
        }

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("V", eType)))
            {
                return;
            }

            if (eType.Equals("1"))
            {
                String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("sale_customer");
                CWinLoadParam param = new CWinLoadParam();

                param.Caption = caption;
                param.Mode = "E";
                param.ActualView = currentObj;
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditCustomer", param);
            }
            else
            {
                String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("purchase_supplier");
                CWinLoadParam param = new CWinLoadParam();

                param.Caption = caption;
                param.Mode = "E";
                param.ActualView = currentObj;
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditSupplier", param);
            }
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
            (model as MEntity).Category = eType;
            CTable tb = model.GetDbObject();            

            items = OnixWebServiceAPI.GetEntityList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MEntity v = new MEntity(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("D", eType)))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteEntity, rc.ToString());

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        public override void Init(String type)
        {
            eType = type;

            //To differentiate between Customer and Supplier
            SetReferenceName("CCriteriaEntity_" + eType);

            createCriteriaEntries();
            createGridColumns();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("A", eType)))
            {
                return;
            }

            if (eType.Equals("1"))
            {
                String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("sale_customer");
                CWinLoadParam param = new CWinLoadParam();

                param.Caption = caption;
                param.Mode = "A";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditCustomer", param);
            }
            else
            {
                String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("purchase_supplier");
                CWinLoadParam param = new CWinLoadParam();

                param.Caption = caption;
                param.Mode = "A";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditSupplier", param);
            }
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
            else if (name.Equals("mnuCopy"))
            {
                CUtil.EnableForm(false, ParentControl);

                CTable newobj = OnixWebServiceAPI.CopyEntity(currentObj.GetDbObject());
                if (newobj != null)
                {
                    MEntity ivd = new MEntity(newobj);
                    ItemAddedEvent(ivd, e);
                }
                else
                {
                    //Error here
                    CHelper.ShowErorMessage(OnixWebServiceAPI.GetLastErrorDescription(), "ERROR_USER_ADD", null);
                }

                CUtil.EnableForm(true, ParentControl);
            }
            else if (name.Equals("mnuARAPMovement"))
            {
                if (!CHelper.VerifyAccessRight("GENERAL_ENTITY_MOVEMENT"))
                {
                    return;
                }

                showMovementHistoryWindow("ar_ap_movement");
            }
        }
#endregion
    }
}
