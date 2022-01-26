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

namespace Onix.ClientCenter.UI.Cash.Cheque
{
    public class CCriteriaCheque : CCriteriaBase
    {
        private ArrayList items = new ArrayList();
        private CTable lastObjectReturned = null;
        private ObservableCollection<MBaseModel> itemSources = new ObservableCollection<MBaseModel>();
        private MCheque currentObj = null;
        private String eType = "1";

        private RoutedEventHandler checkHandler = null;
        private RoutedEventHandler unCheckHandler = null;
        private Boolean isActionEnable = true;

        public CCriteriaCheque() : base(new MCheque(new CTable("")), "CCriteriaCheque")
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
            if (keyword.Equals("mnuReceivableCheque"))
            {
                Init("1");
            }
            else if (keyword.Equals("mnuPayableCheque"))
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
            c0_1.SetButtonEnable(isActionEnable);
            AddGridColumn(c0_1);

            CCriteriaColumnText c1 = new CCriteriaColumnText("colChequeNo", "cheque_no", "ChequeNo", 20, HorizontalAlignment.Left);
            c1.Sortable = true;
            AddGridColumn(c1);

            CCriteriaColumnText c2 = new CCriteriaColumnText("colChequeDate", "cheque_due_date", "ChequeDateFmt", 15, HorizontalAlignment.Left);
            c2.Sortable = true;
            AddGridColumn(c2);

            CCriteriaColumnText c3 = new CCriteriaColumnText("colBankName", "cheque_bank_name", "ChequeBankName", 20, HorizontalAlignment.Left);
            c3.Sortable = true;
            AddGridColumn(c3);

            CCriteriaColumnText c4 = new CCriteriaColumnText("colPayeeName", "payee_name", "PayeeName", 20, HorizontalAlignment.Left);
            c4.Sortable = true;
            AddGridColumn(c4);

            CCriteriaColumnText c5 = new CCriteriaColumnText("colChequeStatus", "cheque_status", "ChequeStatusDesc", 15, HorizontalAlignment.Left);
            c5.Sortable = true;
            AddGridColumn(c5);
        }

        private void createCriteriaEntries()
        {
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_no"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "ChequeNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "payee_name"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "PayeeName", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "from_cheque_duedate"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MIN, "FromChequeDate", ""));

            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "to_cheque_duedate"));
            AddTopCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_DATE_MAX, "ToChequeDate", ""));


            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "AccNo"));
            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_TEXT_BOX, "AccountNo", ""));

            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "Bank"));

            CCriteriaEntry bankEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "BankObj", "");
            bankEntry.SetComboItemSources("Banks", "Description");
            AddCriteriaControl(bankEntry);


            AddCriteriaControl(new CCriteriaEntry(CriteriaEntryType.ENTRY_LABEL, "", "cheque_status"));

            CCriteriaEntry statusEntry = new CCriteriaEntry(CriteriaEntryType.ENTRY_COMBO_BOX, "ChequeStatusObj", "");
            statusEntry.SetComboItemSources("DocumentStatuses", "Description");
            AddCriteriaControl(statusEntry);
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

        public static void ShowEditWindow(String type, ObservableCollection<MBaseModel> items, MBaseModel defDat)
        {
            if (!CHelper.VerifyAccessRight("CASH_CHEQUE_VIEW"))
            {
                return;
            }

            MCheque cheque;
            if ((defDat is MCheque))
            {
                cheque = (MCheque) defDat;
            }
            else
            {
                MAccountDoc ad = (MAccountDoc)defDat;
                cheque = new MCheque(new CTable(""));
                cheque.ChequeID = ad.ChequeID;
            }

            String caption = "";
            CWinLoadParam param = new CWinLoadParam();

            if (type.Equals("1"))
            {
                caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("receivable_cheque");
            }
            else
            {
                caption = CLanguage.getValue("edit") + " " + CLanguage.getValue("payable_cheque");
            }

            param.Caption = caption;
            param.GenericType = type;
            param.Mode = "E";
            param.ActualView = cheque;
            param.ParentItemSources = items;
            FactoryWindow.ShowWindow("WinAddEditCheque", param);
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
            (model as MCheque).Direction = eType;
            CTable tb = model.GetDbObject();            

            items = OnixWebServiceAPI.GetChequeList(tb);
            lastObjectReturned = OnixWebServiceAPI.GetLastObjectReturned();

            itemSources.Clear();
            int idx = 0;
            foreach (CTable o in items)
            {
                MCheque v = new MCheque(o);

                v.RowIndex = idx;
                itemSources.Add(v);
                idx++;
            }

            Tuple<CTable, ObservableCollection<MBaseModel>> tuple = new Tuple<CTable, ObservableCollection<MBaseModel>>(lastObjectReturned, itemSources);
            return (tuple);
        }

        public override int DeleteData(int rc)
        {
            if (!CHelper.VerifyAccessRight("CASH_CHEQUE_DELETE"))
            {
                return(rc);
            }

            int rCount = CHelper.DeleteSelectedItems(itemSources, OnixWebServiceAPI.DeleteCheque, rc.ToString());

            return (rCount);
        }

        public override void DoubleClickData(MBaseModel m)
        {
            currentObj = (MCheque) m;
            ShowEditWindow(eType, itemSources, currentObj);
        }

        public override void Init(String type)
        {
            eType = type;

            //To differentiate between Customer and Supplier
            SetReferenceName("CCriteriaCheque_" + eType);

            createCriteriaEntries();
            createGridColumns();

            CMasterReference.LoadCashAccount();
        }

        public static void ShowAddChequeWindow(String type, ObservableCollection<MBaseModel> items, MBaseModel defDat)
        {
            MCheque cheque = null;
            if (defDat != null)
            {
                MAccountDoc ad = (MAccountDoc) defDat;
                cheque = new MCheque(new CTable(""));

                cheque.EntityObj = ad.EntityObj;
                cheque.PayeeName = ad.EntityName;
                if (ad.IsPopulateChequeAmt)
                {
                    cheque.ChequeAmount = ad.ArApAmt;
                }

                //Create default value here
            }

            String caption = "";
            CWinLoadParam param = new CWinLoadParam();

            if (type.Equals("1"))
            {
                caption = CLanguage.getValue("add") + " " + CLanguage.getValue("receivable_cheque");
            }
            else
            {
                caption = CLanguage.getValue("add") + " " + CLanguage.getValue("payable_cheque");
            }

            param.Caption = caption;
            param.GenericType = type;
            param.Mode = "A";
            param.ActualView = cheque;
            param.ParentItemSources = items;
            FactoryWindow.ShowWindow("WinAddEditCheque", param);
        }

        #region Event Handler

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!CHelper.VerifyAccessRight("CASH_CHEQUE_ADD"))
            {
                return;
            }

            ShowAddChequeWindow(eType, itemSources, null);
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            currentObj = (MCheque)(sender as UActionButton).Tag;
        }

        private void mnuContextMenu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;

            if (name.Equals("mnuEdit"))
            {
                ShowEditWindow(eType, itemSources, currentObj);
            }
            else if (name.Equals("mnuCopy"))
            {
                CUtil.EnableForm(false, ParentControl);

                CTable newobj = OnixWebServiceAPI.CopyCheque(currentObj.GetDbObject());
                if (newobj != null)
                {
                    MCheque ivd = new MCheque(newobj);
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
