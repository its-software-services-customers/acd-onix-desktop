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
    public class CCriteriaExternalPayment : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MBaseModel currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaExternalPayment() : base(new MAccountDocPayment(new CTable("")), "CCriteriaExternalPayment")
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

            CCriteriaColumnText c1 = new CCriteriaColumnText("colDocNo", "docno", "DocumentNo", 15, HorizontalAlignment.Left);
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colDocDate", "DocuDate", "DocumentDateFmt", 15, HorizontalAlignment.Center);
            AddGridColumn(c2);

            CCriteriaColumnText c2_1 = new CCriteriaColumnText("colAcctNo", "AccNo", "AccountNo", 20, HorizontalAlignment.Left);
            AddGridColumn(c2_1);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colBank", "Bank", "BankName", 20, HorizontalAlignment.Left);
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colPaymentType", "payment_type", "PaymentTypeDesc", 15, HorizontalAlignment.Left);
            AddGridColumn(c4);
            c4.Sortable = false;

            CCriteriaColumnText c6 = new CCriteriaColumnText("colAmount", "money_quantity", "PaidAmountFmt", 10, HorizontalAlignment.Right);
            AddGridColumn(c6);
            c6.Sortable = false;
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromDocumentDate", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_date"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToDocumentDate", ""));
        }

        private void createInfoEntries()
        {
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
            MAccountDocPayment mp = model as MAccountDocPayment;

            CTable tb = model.GetDbObject();

            tb.SetFieldValue("CHUNK_FLAG", "Y");
            tb.SetFieldValue("OWNER_FLAG", "Y");
            tb.SetFieldValue("DOCUMENT_STATUS", "2");
            tb.SetFieldValue("IS_NULL_REFUND_STATUS", "Y");
            tb.SetFieldValue("REFUND_STATUS", "");

            items = OnixWebServiceAPI.GetListAPI("GetPaymentTransactionList", "PAYMENT_TRANSACTION_LIST", tb);

            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MAccountDocPayment v = new MAccountDocPayment(o);

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
            eType = type;
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
