using System;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using Onix.Client.Model;
using Onix.Client.Controller;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.ClientCenter.UI.HumanResource.PayrollDocument;

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaExpenseFromPayroll : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaExpenseFromPayroll() : base(new MVPayrollDocument(new CTable("")), "CCriteriaExpenseFromPayroll")
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

#region Criteria Configure

        private ArrayList createActionContextMenu()
        {
            ArrayList contexts = new ArrayList();
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
            ArrayList ctxMenues = createActionContextMenu();

            CCriteriaColumnCheckbox c0 = new CCriteriaColumnCheckbox("colChecked", "", "", 5, HorizontalAlignment.Left, cbxSelect_Checked, cbxSelect_UnChecked);
            AddGridColumn(c0);

            CCriteriaColumnButton c0_1 = new CCriteriaColumnButton("colAction", "action", ctxMenues, 5, HorizontalAlignment.Center, cmdAction_Click);
            c0_1.SetButtonEnable(false);
            AddGridColumn(c0_1);            

            CCriteriaColumnText c4 = new CCriteriaColumnText("colDate", "inventory_doc_date", "DocumentDateNormalized", 20, HorizontalAlignment.Left);
            AddGridColumn(c4);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colMonth", "inventory_doc_desc", "Note", 30, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colStatus", "inventory_doc_status", "DocumentStatusDesc", 20, HorizontalAlignment.Left);
            AddGridColumn(c5);

            CCriteriaColumnText c6 = new CCriteriaColumnText("colType", "document_type", "EmployeeTypeDesc", 20, HorizontalAlignment.Left);
            AddGridColumn(c6);            
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "-"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private void createInfoEntries()
        {
            //AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_code"));
            //AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityCode", ""));

            //AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_name"));
            //AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            //MAuxilaryDocItem m = (MAuxilaryDocItem) model;
            //MAccountDoc d = (MAccountDoc)GetDefaultData();
            //if (d != null)
            //{
            //    m.IsIncludable = false;
            //    m.EntityCode = d.EntityCode;
            //    m.EntityName = d.EntityName;
            //    m.EntityID = d.EntityId;
            //    m.DocumentType = eType;
            //    m.PoInvoiceRefType = "1";
            //}
        }

        private ArrayList createAddContextMenu()
        {
            ArrayList contexts = new ArrayList();
            return (contexts);
        }

        #endregion;

        #region Private

        private void showEditWindow()
        {
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
            MVPayrollDocument ad = (model as MVPayrollDocument);

            items = OnixWebServiceAPI.GetListAPI("GetPayrollDocList", "PAYROLL_DOC_LIST", ad.GetDbObject());
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MVPayrollDocument v = new MVPayrollDocument(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }        

        public override int DeleteData(int rc)
        {
            return (0);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = m;
            showEditWindow();
        }

        public override void Init(String type)
        {
            createCriteriaEntries();
            createInfoEntries();
            createGridColumns();
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MBaseModel)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
        }
#endregion
    }
}
