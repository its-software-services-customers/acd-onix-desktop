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

namespace Onix.ClientCenter.UI.Cash.CashXfer
{
    public class CCriteriaCashXfer : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private CashDocumentType docType = CashDocumentType.CashDocXfer;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaCashXfer() : base(new MCashDocXfer(new CTable("")), "CCriteriaCashXfer")
        {
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
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

        public void SetDocumentType(CashDocumentType dt)
        {
            docType = dt;
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
            CMasterReference.LoadCashAccount();
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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocumentNo", "inventory_doc_no", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDate", "date", "DocumentDateFmt", 15, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colFromAcc", "FromAcc", "FromAccountNo", 25, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colToAcc", "ToAcc", "ToAccountNo", 25, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_no"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "FromAcc"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "FromAccountNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "ToAcc"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ToAccountNo", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cash_xfer_type"));

            CCriteriaEntry xferTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "CashXferTypeObj", "");
            xferTypeEntry.SetComboItemSources("CashXferTypes", "Description");
            AddCriteriaControl(xferTypeEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuRegularXfer", "regular_cash_xfer", new RoutedEventHandler(cmdAddInternal_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuExternalXfer", "external_cash_xfer", new RoutedEventHandler(cmdAddExternal_Click), 2);
            contexts.Add(ct2);


            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight("CASH_XFER_VIEW"))
            {
                return;
            }

            MCashDocXfer v = (MCashDocXfer) currentObj;

            String winName = "WinAddEditCashExternalXfer";
            if (v.CashXferType.Equals("1"))
            {                
                winName = "WinAddEditCashTransfer";
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("Trans");

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int)docType).ToString();
            param.Mode = "E";
            param.ParentItemSources = itemSources;
            param.ActualView = currentObj;
            FactoryWindow.ShowWindow(winName, param);
        }

        private void cmdAddSelect_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAddSelect_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MCashDoc).DocumentType = ((int) CashDocumentType.CashDocXfer).ToString();

            items = OnixWebServiceAPI.GetListAPI("GetCashDocList", "CASH_DOC_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();

            foreach (CTable o in items)
            {
                MCashDocXfer v = null;
                v = new MCashDocXfer(o);

                itemSources.Add(v);
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("CASH_XFER_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteCashDoc");
            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        #region Event Handler

        private void cmdAddInternal_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("CASH_XFER_ADD"))
            {
                return;
            }

            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("Trans");
            param.GenericType = ((int)docType).ToString();
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            param.ActualView = currentObj;
            FactoryWindow.ShowWindow("WinAddEditCashTransfer", param);
        }

        private void cmdAddExternal_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("CASH_XFER_ADD"))
            {
                return;
            }

            String caption = CLanguage.getValue("add") + " " + CLanguage.getValue("Trans");
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int)docType).ToString();
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            param.ActualView = currentObj;
            FactoryWindow.ShowWindow("WinAddEditCashExternalXfer", param);
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
                CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("CopyCashDoc", currentObj.GetDbObject());

                if (newobj != null)
                {
                    MCashDoc ivd = new MCashDocXfer(newobj);
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
