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

namespace Onix.ClientCenter.UI.HumanResource.OTDocument
{
    public class CCriteriaOTDocument : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        public CCriteriaOTDocument() : base(new MVOTDocument(new CTable("")), "CCriteriaOTDocument")
        {
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createGridColumns();
            loadRelatedReferences();
        }

        #region Criteria Configure

        private void loadRelatedReferences()
        {

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

            CCriteriaColumnText c4 = new CCriteriaColumnText("colDate", "inventory_doc_date", "DocumentDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colEmpCode", "employee_code", "EmployeeCode", 10, HorizontalAlignment.Left);
            AddGridColumn(c5);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colEmpName", "employee_name", "EmployeeNameLastName", 25, HorizontalAlignment.Left);
            AddGridColumn(c6);

            CCriteriaColumnText c7 = new CCriteriaColumnText("colNote", "inventory_doc_desc", "Note", 25, HorizontalAlignment.Left);
            AddGridColumn(c7);

            CCriteriaColumnText c9 = new CCriteriaColumnText("colType", "document_type", "EmployeeTypeDesc", 10, HorizontalAlignment.Left);
            AddGridColumn(c9);

            CCriteriaColumnText c8 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 10, HorizontalAlignment.Left);
            AddGridColumn(c8);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_code"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeCode", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "employee_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EmployeeName", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "document_type"));
            CCriteriaEntry docTypeEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "EmployeeTypeObj", "");
            docTypeEntry.SetComboItemSources("EmployeeTypes", "Description");
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

            CCriteriaContextMenu ct1 = new CCriteriaContextMenu("mnuDaily", "daily", new RoutedEventHandler(cmdAdd_Click), 1);
            contexts.Add(ct1);

            CCriteriaContextMenu ct2 = new CCriteriaContextMenu("mnuMonthly", "monthly", new RoutedEventHandler(cmdAdd_Click), 2);
            contexts.Add(ct2);

            return (contexts);
        }

        #endregion;

        #region Private        

        public void ShowEditWindowEx(MVOTDocument actDoc)
        {
            if (!CHelper.VerifyAccessRight("HR_OT_VIEW"))
            {
                return;
            }

            CWinLoadParam param = new CWinLoadParam();
            param.Caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("hr_ot_form");
            param.Mode = "E";
            param.GenericType = actDoc.EmployeeType;
            param.ActualView = actDoc;
            FactoryWindow.ShowWindow("WinAddEditOtDoc", param);
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
            MVOTDocument ad = (model as MVOTDocument);

            items = OnixWebServiceAPI.GetListAPI("GetOtDocList", "OT_DOC_LIST", ad.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MVOTDocument v = new MVOTDocument(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        private String getAccessRightDelete()
        {
            String acr = "HR_OT_DELETE";
            return (acr);
        }

        public override int DeleteData(int rc)
        {

            if (!CHelper.VerifyAccessRight(getAccessRightDelete()))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteAPI, rc.ToString(), "DeleteOtDoc");

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            ShowEditWindowEx((MVOTDocument) currentObj);
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("HR_OT_ADD"))
            {
                return;
            }

            MenuItem mnu = (MenuItem)sender;
            CWinLoadParam param = new CWinLoadParam();

            if (mnu.Name.Equals("mnuDaily"))
            {
                param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("hr_ot_form");
                param.Mode = "A";
                param.GenericType = "1";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditOtDoc", param);
            }
            else if (mnu.Name.Equals("mnuMonthly"))
            {
                param.Caption = CLanguage.getValue("add") + " " + CLanguage.getValue("hr_ot_form");
                param.Mode = "A";
                param.GenericType = "2";
                param.ParentItemSources = itemSources;
                FactoryWindow.ShowWindow("WinAddEditOtDoc", param);
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
                ShowEditWindowEx((MVOTDocument) currentObj);
            }
        }
#endregion
    }
}
