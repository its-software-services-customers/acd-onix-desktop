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

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument
{
    public class CCriteriaTaxDocument : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String menuType = "";

        public CCriteriaTaxDocument() : base(new MVTaxDocument(new CTable("")), "CCriteriaTaxDocument")
        {
        }

        public override void Init(String type)
        {
            menuType = type;

            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {
            if (!CMasterReference.IsCashAccountLoad())
            {
                CMasterReference.LoadCashAccount(OnixWebServiceAPI.GetCashAccountList);
            }
        }

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuEdit", "ADMIN_EDIT", new RoutedEventHandler(mnuContextMenu_Click), 1);
            contexts.Add(ct1);
            
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

            CCriteriaColumnText c4 = new CCriteriaColumnText("colDate", "inventory_doc_date", "DocumentDateFmt", 20, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colMonth", "month", "TaxMonthName", 20, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colYear", "year", "TaxYear", 10, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 20, HorizontalAlignment.Left);
            AddGridColumn(c5);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colType", "tax_document_type", "DocumentTypeDesc", 20, HorizontalAlignment.Left);
            AddGridColumn(c6);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "year"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "TaxYear", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "month"));
            CCriteriaEntry monthEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "TaxMonthObj", "");
            monthEntry.SetComboItemSources("Months", "Description");
            AddCriteriaControl(monthEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "tax_document_type"));
            CCriteriaEntry docTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentTypeObj", "");
            docTypeEntry.SetComboItemSources("TaxDocTypes", "Description");
            AddCriteriaControl(docTypeEntry);

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_status"));
            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "DocumentStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);

            //===
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuPP30", "tax_pp30", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuPP53", "rv_tax_53", new RoutedEventHandler(cmdAdd_Click), 2);
            contexts.Add(ct2);

            CCriteriaContextMenu ct3 = new CCriteriaContextMenu("mnuPP3", "rv_tax_3", new RoutedEventHandler(cmdAdd_Click), 3);
            contexts.Add(ct3);

            return (contexts);
        }

        #endregion;

        #region Private        

        public void ShowEditWindowEx(MVTaxDocument actDoc)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_TAXDOC_EDIT"))
            {
                return;
            }

            CWinLoadParam param = new CWinLoadParam();
            TaxDocumentType dt = (TaxDocumentType) CUtil.StringToInt(actDoc.DocumentType);
            if (dt == TaxDocumentType.TaxDocPP30)
            {
                param.Caption = CUtil.TaxDocTypeToString(dt);
                param.Mode = "E";
                param.ActualView = actDoc;
                FactoryWindow.ShowWindow("WinAddEditTaxPP30", param);
            }
            else
            {
                param.Caption = CUtil.TaxDocTypeToString(dt);
                param.Mode = "E";
                param.GenericType = "";
                param.ActualView = actDoc;
                FactoryWindow.ShowWindow("WinAddEditTaxFormPRV3_53", param);
            }
        }
        
        #endregion

        public void SetDocumentType(AccountDocumentType dt)
        {
        }

        private void cmdTaxDocAdd_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsOpen = true;
        }

        public override Button GetButton()
        {
            ArrayList menues = createAddContextMenu();
            CCriteriaButton btn = new CCriteriaButton("add", true, menues, cmdTaxDocAdd_Click);

            return (btn.GetButton());
        }

        public override Tuple<CTable, ObservableCollection<MBaseModel>> QueryData()
        {
            MVTaxDocument ad = (model as MVTaxDocument);
            CTable tb = ad.GetDbObject();

            tb.SetFieldValue("DOCUMENT_TYPE_SET", "(1,2,3)");
            items = OnixWebServiceAPI.GetListAPI("GetTaxDocList", "TAX_DOC_LIST", tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MVTaxDocument v = new MVTaxDocument(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        private String getAccessRightDelete()
        {
            String acr = "PURCHASE_TAXDOC_DELETE";
            return (acr);
        }

        public override int DeleteData(int rc)
        {

            if (!CHelper.VerifyAccessRight(getAccessRightDelete()))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteTaxDoc");

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            ShowEditWindowEx((MVTaxDocument) currentObj);
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("PURCHASE_TAXDOC_ADD"))
            {
                return;
            }

            MenuItem mnu = (MenuItem)sender;
            CWinLoadParam param = new CWinLoadParam();

            if (mnu.Name.Equals("mnuPP30"))
            {
                param.Caption = mnu.Header.ToString();
                param.Mode = "A";
                param.GenericType = "1";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditTaxPP30", param);
            }
            else if (mnu.Name.Equals("mnuPP53"))
            {
                param.Caption = mnu.Header.ToString();
                param.Mode = "A";
                param.GenericType = "3";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditTaxFormPRV3_53", param);
            }
            else if (mnu.Name.Equals("mnuPP3"))
            {
                param.Caption = mnu.Header.ToString();
                param.Mode = "A";
                param.GenericType = "2";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditTaxFormPRV3_53", param);
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
                ShowEditWindowEx((MVTaxDocument) currentObj);
            }
        }
#endregion
    }
}
