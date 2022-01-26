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

namespace Onix.ClientCenter.UI.Inventory.InventoryDocument
{
    public class CCriteriaInventoryDoc : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private InventoryDocumentType docType = InventoryDocumentType.InvDocImport;

        public CCriteriaInventoryDoc() : base(new MInventoryDoc(new CTable("")), "CCriteriaInventoryDoc")
        {
        }

        public override void Init(String type)
        {            
        }

        public override void Initialize(string keyword)
        {
            Hashtable maps = new Hashtable();
            maps["mnuInventoryImport"] = InventoryDocumentType.InvDocImport;
            maps["mnuInventoryExport"] = InventoryDocumentType.InvDocExport;
            maps["mnuInventoryAdjust"] = InventoryDocumentType.InvDocAdjust;
            maps["mnuInventoryXfer"] = InventoryDocumentType.InvDocXfer;
            maps["mnuInventoryBorrow"] = InventoryDocumentType.InvDocBorrow;
            maps["mnuInventoryReturn"] = InventoryDocumentType.InvDocReturn;
            
            if (maps.ContainsKey(keyword))
            {
                InventoryDocumentType dt = (InventoryDocumentType)maps[keyword];
                SetDocumentType(dt);
            }
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
            if (!CMasterReference.IsLocationLoad())
            {
                CMasterReference.LoadLocation(true, null);
            }
        }

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuCopy", "copy", new RoutedEventHandler(mnuContextMenu_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        private void createGridColumns()
        {
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left);
            c0.RegisterCheckboxBindingVariable(CheckBox.IsEnabledProperty, "IsEditable");
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "inventory_doc_no", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "inventory_doc_date", "DocumentDateFmt", 12, HorizontalAlignment.Left);
            AddGridColumn(c2);
        
            CCriteriaColumnText c3 = new CCriteriaColumnText("colDocDesc", "inventory_doc_desc", "Note", 33, HorizontalAlignment.Left);
            AddGridColumn(c3);

            String fieldName = "LocationName";
            if (docType == InventoryDocumentType.InvDocBorrow)
            {
                fieldName = "FromLocationName";
            }
            else if (docType == InventoryDocumentType.InvDocReturn)
            {
                fieldName = "ToLocationName";
            }
            CCriteriaColumnText c4 = new CCriteriaColumnText("colLocation", "location_name", fieldName, 20, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colDocStatus", "inventory_doc_status", "DocumentStatusDesc", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_no"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "description"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "Note", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));

            if (docType == InventoryDocumentType.InvDocXfer)
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_location"));

                CCriteriaEntry locationFromEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "FromLocationObj", "");
                locationFromEntry.SetComboItemSources("Locations", "Description");
                AddCriteriaControl(locationFromEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_location"));

                CCriteriaEntry locationToEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ToLocationObj", "");
                locationToEntry.SetComboItemSources("Locations", "Description");
                AddCriteriaControl(locationToEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInternalDoc", "internal_doc"));
            }
            else if ((docType == InventoryDocumentType.InvDocBorrow) || (docType == InventoryDocumentType.InvDocReturn))
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "location_name"));

                CCriteriaEntry locationFromEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "LocationObj", "");
                locationFromEntry.SetComboItemSources("BorrowReturnLocations", "Description");
                AddCriteriaControl(locationFromEntry);
            }
            else
            {
                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "location_name"));

                CCriteriaEntry locationEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "LocationObj", "");
                locationEntry.SetComboItemSources("Locations", "Description");
                AddCriteriaControl(locationEntry);

                AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInternalDoc", "internal_doc"));
            }

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);

            (model as MInventoryDoc).IsInternalDoc = false;
        }

        private ArrayList createAdjustmentContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuAddByDelta", "adjust_by_delta", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuAddByTotal", "adjust_by_total", new RoutedEventHandler(cmdAdd_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct0 = new CCriteriaContextMenu("mnuAdd", "ADMIN_EDIT", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct0);

            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
            CWinLoadParam param = new CWinLoadParam();
            MInventoryDoc v = (MInventoryDoc)currentObj;
            
            param.Mode = "E";
            param.GenericType = ((int)docType).ToString();
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;

            String caption = "";
            int mode = 1;

            if (docType == InventoryDocumentType.InvDocImport)
            {
                caption = CLanguage.getValue("inventory_import");
                if (!CHelper.VerifyAccessRight("INVENTORY_IMPORT_VIEW"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocExport)
            {
                caption = CLanguage.getValue("inventory_export");
                if (!CHelper.VerifyAccessRight("INVENTORY_EXPORT_VIEW"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocXfer)
            {
                caption = CLanguage.getValue("inventory_xfer");
                if (!CHelper.VerifyAccessRight("INVENTORY_XFER_VIEW"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocAdjust)
            {
                caption = CLanguage.getValue("inventory_adjust");
                if (!CHelper.VerifyAccessRight("INVENTORY_ADJUST_VIEW"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocBorrow)
            {
                mode = 2;
                caption = CLanguage.getValue("inventory_borrow");
                if (!CHelper.VerifyAccessRight("INVENTORY_BORROW_VIEW"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocReturn)
            {
                mode = 2;
                caption = CLanguage.getValue("inventory_return");
                if (!CHelper.VerifyAccessRight("INVENTORY_RETURN_VIEW"))
                {
                    return;
                }
            }

            param.Caption = CLanguage.getValue("edit") + " " + caption; ;
            if (mode == 1)
            {                
                param.GenericFlag = v.IsAdjustByDelta;
                FactoryWindow.ShowWindow("WinAddEditInventoryDoc", param);
            }
            else if (mode == 2)
            {                
                FactoryWindow.ShowWindow("WinAddEditBorrowReturnDoc", param);
            }
        }

        #endregion

        public void SetDocumentType(InventoryDocumentType dt)
        {
            docType = dt;
            (Model as MInventoryDoc).DocumentType = ((int)dt).ToString();

            createGridColumns();
            loadRelatedReferences();
            createCriteriaEntries();
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            Boolean adjustByDelta = false;
            int mode = 1;

            CWinLoadParam param = new CWinLoadParam();
            param.Mode = "A";
            param.GenericType = ((int)docType).ToString();
            param.ActualView = currentObj;
            param.ParentItemSources = itemSources;

            String caption = "";
            if (docType == InventoryDocumentType.InvDocImport)
            {
                caption = CLanguage.getValue("inventory_import");
                if (!CHelper.VerifyAccessRight("INVENTORY_IMPORT_ADD"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocExport)
            {
                caption = CLanguage.getValue("inventory_export");
                if (!CHelper.VerifyAccessRight("INVENTORY_EXPORT_ADD"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocXfer)
            {
                caption = CLanguage.getValue("inventory_xfer");
                if (!CHelper.VerifyAccessRight("INVENTORY_XFER_ADD"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocAdjust)
            {
                caption = CLanguage.getValue("inventory_adjust");
                if (!CHelper.VerifyAccessRight("INVENTORY_ADJUST_ADD"))
                {
                    return;
                }

                String nm = (sender as MenuItem).Name;
                if (nm.Equals("mnuAddByDelta"))
                {
                    adjustByDelta = true;
                }
            }
            else if (docType == InventoryDocumentType.InvDocBorrow)
            {
                mode = 2;
                caption = CLanguage.getValue("inventory_borrow");
                if (!CHelper.VerifyAccessRight("INVENTORY_BORROW_ADD"))
                {
                    return;
                }
            }
            else if (docType == InventoryDocumentType.InvDocReturn)
            {
                mode = 2;
                caption = CLanguage.getValue("inventory_return");
                if (!CHelper.VerifyAccessRight("INVENTORY_RETURN_ADD"))
                {
                    return;
                }
            }

            param.Caption = CLanguage.getValue("add") + " " + caption; ;

            if (mode == 1)
            {
                param.GenericFlag = adjustByDelta;
                FactoryWindow.ShowWindow("WinAddEditInventoryDoc", param);
            }
            else if (mode == 2)
            {
                FactoryWindow.ShowWindow("WinAddEditBorrowReturnDoc", param);
            }
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = null;

            btn = new CCriteriaButton("add", true, menues, null);

            if (docType == InventoryDocumentType.InvDocAdjust)
            {
                menues = createAdjustmentContextMenu();
                btn = new CCriteriaButton("add", true, menues, cmdAdjustmentAdd_Click);
            }
            else
            {
                menues = createAddContextMenu();
                btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);
            }

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MInventoryDoc).DocumentType = ((int) docType).ToString();            
            items = OnixWebServiceAPI.GetListAPI("GetInventoryDocList", "INVENTORY_DOC_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MInventoryDoc v = new MInventoryDoc(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }


        private String getAccessRightDelete()
        {
            String acr = "INVENTORY_UNKNOW_DELETE";

            if (docType == InventoryDocumentType.InvDocImport)
            {
                acr = "INVENTORY_IMPORT_DELETE";
            }
            else if (docType == InventoryDocumentType.InvDocExport)
            {
                acr = "INVENTORY_EXPORT_DELETE";
            }
            else if (docType == InventoryDocumentType.InvDocXfer)
            {
                acr = "INVENTORY_XFER_DELETE";
            }
            else if (docType == InventoryDocumentType.InvDocAdjust)
            {
                acr = "INVENTORY_ADJUST_DELETE";
            }
            else if (docType == InventoryDocumentType.InvDocBorrow)
            {
                acr = "INVENTORY_BORROW_DELETE";
            }
            else if (docType == InventoryDocumentType.InvDocReturn)
            {
                acr = "INVENTORY_RETURN_DELETE";
            }

            return (acr);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight(getAccessRightDelete()))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteInventoryDoc");

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAdjustmentAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
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
                CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CopyInventoryDoc", currentObj.GetDbObject());

                if (newobj != null)
                {
                    MInventoryDoc ivd = new MInventoryDoc(newobj);
                    ItemAddedEvent(ivd, e);
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
