using System;
using System.Windows.Controls;
using System.Collections;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Windows;
using Onix.ClientCenter.Criteria;
using Onix.ClientCenter.Commons.Windows;
using Onix.ClientCenter.Commons.Factories;
using System.Collections.ObjectModel;

namespace Onix.ClientCenter.UI.Inventory.InventoryDocument
{
    public partial class WinAddEditBorrowReturnDoc : WinBase
    {
        private InventoryDocumentType dt = InventoryDocumentType.InvDocBorrow;
        private MInventoryDoc mv = null;
        private MInventoryTransaction currentViewObj = null;

        public WinAddEditBorrowReturnDoc(CWinLoadParam param) : base(param)
        {
            dt = (InventoryDocumentType) CUtil.StringToInt(param.GenericType);

            accessRightName = getAccessRightEdit();

            createAPIName = "CreateInventoryDoc";
            updateAPIName = "UpdateInventoryDoc";
            getInfoAPIName = "GetInventoryDocInfo";
            approveAPIName = "ApproveInventoryDoc";
            verifyAPIName = "VerifyInventoryDoc";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblDesc, txtDesc, false);
            registerValidateControls(lblFrom, cboFrom, false);
            registerValidateControls(lblTo, cboTo, false);
            registerValidateControls(lblSaleMan, uSalesman, false);

            double[] ratios = new double[9] { 0.05, 0.05, 0.10, 0.32, 0.08, 0.10, 0.10, 0.15, 0.05 };
            registerListViewSize(lsvImportItem.Name, ratios);
        }

        protected override MBaseModel createObject()
        {
            mv = new MInventoryDoc(new CTable(""));

            mv.DocumentType = ((int)dt).ToString();
            mv.IsInternalDoc = false;                        
            mv.AllowNegative = false;
            mv.DocumentDate = DateTime.Now;
            mv.DocumentDate = DateTime.Now;
            mv.ReturnDueDate = DateTime.Now;
            mv.CreateDefaultValue();

            if (dt == InventoryDocumentType.InvDocReturn)
            {
                cboFrom.IsEnabled = false;
            }
            else if (dt == InventoryDocumentType.InvDocBorrow)
            {
                cboTo.IsEnabled = false;
            }

            if (loadParam.Mode.Equals("A"))
            {
                ObservableCollection<MLocation> locations = CMasterReference.Instance.Locations;
                if (locations.Count > 1)
                {
                    MLocation b = locations[1];
                    if (cboFrom.IsEnabled)
                    {
                        mv.FromLocationObj = b;
                    }

                    if (cboTo.IsEnabled)
                    {
                        mv.ToLocationObj = b;
                    }
                }
            }

            return (mv);
        }

        private String getAccessRightEdit()
        {
            String acr = "INVENTORY_UNKNOW_EDIT";

            if (dt == InventoryDocumentType.InvDocBorrow)
            {
                acr = "INVENTORY_BORROW_EDIT";
            }
            else if (dt == InventoryDocumentType.InvDocReturn)
            {
                acr = "INVENTORY_RETURN_EDIT";
            }

            return (acr);
        }
        
        public String BorrowReturnPerson
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocBorrow)
                {
                    return (CLanguage.getValue("borrower"));
                }

                return (CLanguage.getValue("returner"));
            }
        }
        
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }

        protected override Boolean isEditable()
        {
            return (mv.IsEditable);
        }

        private void cmdSave_Click(object sender, RoutedEventArgs e)
        {
            if (!vw.IsModified)
            {
                return;
            }

            Boolean r = saveData();
            if (r)
            {
                loadParam.ActualView = vw;
                loadParam.Mode = "E";

                loadData();
                vw.IsModified = false;
            }
        }

        private void cmdApprove_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = approveData();
            if (r)
            {
                CUtil.EnableForm(true, this);
                vw.IsModified = false;
                this.Close();
            }
        }        
        
        public String ItemText
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocReturn)
                {
                    return (CLanguage.getValue("borrow_return_item"));
                }
                else if (dt == InventoryDocumentType.InvDocBorrow)
                {
                    return (CLanguage.getValue("borrow_return_item"));
                }

                return ("");
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            String caption = ItemText;
            if (dt == InventoryDocumentType.InvDocBorrow)
            {
                CWinLoadParam param = new CWinLoadParam();

                param.Caption = (String)(sender as Button).Content + " " + caption;
                param.Mode = "A";
                param.ActualParentView = mv;
                param.ActualView = currentViewObj;
                param.GenericType = loadParam.GenericType;

                vw.IsModified = FactoryWindow.ShowWindow("WinAddEditBorrowReturnItem", param);
            }
            else if (dt == InventoryDocumentType.InvDocReturn)
            {
                CCriteriaBorrowItem cr = new CCriteriaBorrowItem();
                cr.SetActionEnable(false);
                cr.SetDefaultData(vw);
                cr.Init("");

                WinMultiSelection w = new WinMultiSelection(cr, CLanguage.getValue("borrow_return_item"));
                w.ShowDialog();

                if (w.IsOK)
                {
                    addReturnDocItems(w.SelectedItems);
                    vw.IsModified = true;
                }
            }
        }

        private void addReturnDocItems(ArrayList items)
        {
            //To prevent the duplication selection
            Hashtable hash = CUtil.ObserableCollectionToHash(mv.TxItems, "BorrowID");

            foreach (MInventoryTransaction ai in items)
            {
                if (hash.ContainsKey(ai.InventoryTxID))
                {
                    continue;
                }

                MInventoryTransaction di = new MInventoryTransaction(new CTable(""));
                di.SetDbObject(ai.GetDbObject().CloneAll());
                di.InventoryTxID = "";
                di.ExtFlag = "A";
                di.TxType = "I";
                di.BorrowID = ai.InventoryTxID;
                di.BorrowDocumentNo = ai.DocumentNo;
                di.ItemQuantity = ai.ReturnQuantityNeed;

                mv.AddTxItem(di, dt);
            }
        }
        
        private void mnuDocumentEdit_Click(object sender, RoutedEventArgs e)
        {
            MenuItem mnu = (sender as MenuItem);
            string name = mnu.Name;
            String caption = ItemText;

            if (name.Equals("mnuDocumentEdit"))
            {
                ShowEditWindow();
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            currentViewObj = (MInventoryTransaction)btn.Tag;
            btn.ContextMenu.IsOpen = true;
        }
        

        private void cmdVerify_Click(object sender, RoutedEventArgs e)
        {
            verifyData();
        }

        private void lsvImportItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lsvImportItem.SelectedItems.Count == 1)
            {
                currentViewObj = (MInventoryTransaction)lsvImportItem.SelectedItems[0];
                ShowEditWindow();
            }
        }

        private void ShowEditWindow()
        {
            String caption = ItemText;
            CWinLoadParam param = new CWinLoadParam();

            param.Caption = CLanguage.getValue("edit") + " " + caption;
            param.Mode = "E";
            param.ActualParentView = mv;
            param.ActualView = currentViewObj;
            param.GenericType = loadParam.GenericType;       
            bool isModified = FactoryWindow.ShowWindow("WinAddEditBorrowReturnItem", param);
            if (isModified)
            {
                vw.IsModified = true;
            }
        }
        
        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "grpInventoryBorrow";
            if (dt == InventoryDocumentType.InvDocReturn)
            {
                group = "grpInventoryReturn";
            }

            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }
    }
}
