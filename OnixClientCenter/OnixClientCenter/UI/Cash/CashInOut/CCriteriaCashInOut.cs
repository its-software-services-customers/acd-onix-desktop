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

namespace Onix.ClientCenter.UI.Cash.CashInOut
{
    public class CCriteriaCashInOut : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private CashDocumentType docType = CashDocumentType.CashDocImport;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaCashInOut(MCashDocIn docIn) : base(docIn, "CCriteriaCashInOut")
        {
        }

        public CCriteriaCashInOut(MCashDocOut docOut) : base(docOut, "CCriteriaCashInOut")
        {
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        public override void Initialize(string keyword)
        {
            if (keyword.Equals("mnuCashIn"))
            {
                Init("");
                SetDocumentType(CashDocumentType.CashDocImport);
            }
            else if (keyword.Equals("mnuCashOut"))
            {
                Init("");
                SetDocumentType(CashDocumentType.CashDocExport);
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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocumentNo", "inventory_doc_no", "DocumentNo", 25, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDate", "date", "DocumentDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colAccountNo", "AccNo", "AccountNo", 25, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colBank", "Bank", "BankName", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 15, HorizontalAlignment.Left);
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_no"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccNo"));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "Bank"));

            CCriteriaEntry bankEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "BankObj", "");
            bankEntry.SetComboItemSources("Banks", "Description");
            AddCriteriaControl(bankEntry);

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));


            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsInternalDoc", "internal_doc"));

            (model as MCashDoc).IsInternalDoc = false;
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

        private String getPermissionKey(String mode, CashDocumentType dt)
        {
            String tmp = "";

            if (dt == CashDocumentType.CashDocImport)
            {
                if (mode.Equals("A"))
                {
                    tmp = "CASH_IN_ADD";
                }
                else if (mode.Equals("E"))
                {
                    tmp = "CASH_IN_VIEW";
                }
                else if (mode.Equals("D"))
                {
                    tmp = "CASH_IN_DELETE";
                }
            }
            else if (dt == CashDocumentType.CashDocExport)
            {
                if (mode.Equals("A"))
                {
                    tmp = "CASH_OUT_ADD";
                }
                else if (mode.Equals("E"))
                {
                    tmp = "CASH_OUT_VIEW";
                }
                else if (mode.Equals("D"))
                {
                    tmp = "CASH_OUT_DELETE";
                }
            }

            return (tmp);
        }

        private String getCaptionKey(CashDocumentType dt)
        {
            String tmp = "";

            if (dt == CashDocumentType.CashDocImport)
            {
                tmp = "CashIn";
            }
            else if (dt == CashDocumentType.CashDocExport)
            {
                tmp = "CashOut";
            }

            return (tmp);
        }

        private void showEditWindow()
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("E", docType)))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue(getCaptionKey(docType));
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int) docType).ToString();
            param.Mode = "E";
            param.ParentItemSources = itemSources;
            param.ActualView = currentObj;
            FactoryWindow.ShowWindow("WinAddEditCashInOut", param);
        }

        #endregion

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            (model as MCashDoc).DocumentType = ((int) docType).ToString();

            items = OnixWebServiceAPI.GetListAPI("GetCashDocList", "CASH_DOC_LIST", model.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();

            foreach (CTable o in items)
            {
                MCashDoc v = null;
                if (docType == CashDocumentType.CashDocImport)
                {
                    v = new MCashDocIn(o);
                }
                else
                {
                    v = new MCashDocOut(o);
                }

                itemSources.Add(v);
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("D", docType)))
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

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight(getPermissionKey("A", docType)))
            {
                return;
            }

            String caption = CLanguage.getValue("edit") + " " + CLanguage.getValue(getCaptionKey(docType));
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = caption;
            param.GenericType = ((int)docType).ToString();
            param.Mode = "A";
            param.ParentItemSources = itemSources;
            param.ActualView = currentObj;
            FactoryWindow.ShowWindow("WinAddEditCashInOut", param);
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
                    MCashDoc ivd = null;
                    if (docType == CashDocumentType.CashDocImport)
                    {
                        ivd = new MCashDocIn(newobj);
                    }
                    else
                    {
                        ivd = new MCashDocOut(newobj);
                    }

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
