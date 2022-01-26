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

namespace Onix.ClientCenter.Criteria
{
    public class CCriteriaPurchaseOrderItem : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = ((int) AuxilaryDocumentType.AuxDocPO).ToString();

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaPurchaseOrderItem() : base(new MAuxilaryDocItem(new CTable("")), "CCriteriaPurchaseOrderItem")
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

            CCriteriaColumnText c3 = new CCriteriaColumnText("colDocumentNo", "inventory_doc_no", "DocumentNo", 13, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocumentDate", "inventory_doc_date", "DocumentDateFmt", 10, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colItemCode", "package_item_code", "SelectItemCode", 12, HorizontalAlignment.Left);
            AddGridColumn(c2);

            CCriteriaColumnText c2_1 = new CCriteriaColumnText("colItemName", "package_item_name", "SelectItemName", 30, HorizontalAlignment.Left);
            AddGridColumn(c2_1);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colQuantity", "quantity", "QuantityFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c4);
            c4.Sortable = false;

            CCriteriaColumnText c5 = new CCriteriaColumnText("colPrice", "unit_price", "UnitPriceFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c5);
            c5.Sortable = false;

            CCriteriaColumnText c6 = new CCriteriaColumnText("colAmount", "total_amount_afterDiscount", "TotalAfterDiscountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c6);
            c6.Sortable = false;
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "inventory_doc_no"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "DocumentNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_CHECK_BOX, "IsIncludable", "is_in_used"));

            //AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            //AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            //AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "-"));
            //AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private void createInfoEntries()
        {
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_code"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityCode", ""));

            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "supplier_name"));
            AddInfoControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "EntityName", ""));

            MAuxilaryDocItem m = (MAuxilaryDocItem) model;
            MAccountDoc d = (MAccountDoc) GetDefaultData();
            if (d != null)
            {
                m.IsIncludable = false;
                m.EntityCode = d.EntityCode;
                m.EntityName = d.EntityName;
                m.EntityID = d.EntityId;
                m.DocumentType = eType;
                m.PoInvoiceRefType = "1";
            }
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
            CTable tb = model.GetDbObject();            

            items = OnixWebServiceAPI.GetAuxilaryDocItemList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MAuxilaryDocItem v = new MAuxilaryDocItem(o);

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
