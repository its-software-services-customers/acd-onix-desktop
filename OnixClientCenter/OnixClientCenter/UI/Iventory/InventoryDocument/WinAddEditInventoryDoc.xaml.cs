using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;
using Wis.WsClientAPI;
using System.Windows.Input;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Windows;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Inventory.InventoryDocument
{
    public partial class WinAddEditInventoryDoc : WinBase
    {
        private InventoryDocumentType dt = InventoryDocumentType.InvDocBorrow;
        private MInventoryDoc mv = null;
        private MInventoryTransaction currentViewObj = null;
        private Boolean adjustByDelta = false;

        public WinAddEditInventoryDoc(CWinLoadParam param) : base(param)
        {
            dt = (InventoryDocumentType)CUtil.StringToInt(param.GenericType);
            adjustByDelta = param.GenericFlag;

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

            if (AdjustItemVisibility == Visibility.Visible)
            {
                tbiAdjustment.IsSelected = true;
            }
            else
            {
                tbiItem.IsSelected = true;
            }            
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

            mv.AdjustByDeltaFlag = "N";
            if (adjustByDelta)
            {
                mv.AdjustByDeltaFlag = "Y";
            }

            if ((dt == InventoryDocumentType.InvDocImport) || (dt == InventoryDocumentType.InvDocAdjust))
            {
                cboFrom.IsEnabled = false;
            }
            else if (dt == InventoryDocumentType.InvDocExport)
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

        protected override Boolean isEditable()
        {
            return (mv.IsEditable);
        }

        protected override Boolean postValidate()
        {
            if (dt == InventoryDocumentType.InvDocXfer)
            {
                MInventoryDoc d = (MInventoryDoc)vw;

                if (d.FromLocation.Equals(d.ToLocation))
                {
                    //Not allow to xfer in the same location
                    CHelper.ShowErorMessage("", "ERROR_XFER_SAME_LOCATION", null);
                    return (false);
                }
            }
            else if ((dt == InventoryDocumentType.InvDocAdjust) && !adjustByDelta)
            {
                Boolean result = validateAdjustItems(mv.AdjustmentItems, tbiAdjustment, true);
                if (!result)
                {
                    return (result);
                }
            }

            return (true);
        }

        public Visibility AdjustItemVisibility
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocAdjust && !adjustByDelta)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);
            }
        }

        public Visibility NonAdjustItemVisibility
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocAdjust && !adjustByDelta)
                {
                    return (Visibility.Collapsed);
                }

                return (Visibility.Visible);
            }
        }

        public Visibility IsImportVisible
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocImport)
                {
                    return (Visibility.Visible);
                }

                return (Visibility.Collapsed);                
            }
        }


        private Boolean validateAdjustItems<T>(ObservableCollection<T> collection, TabItem titem, Boolean chkCnt) where T : MBaseModel
        {
            Hashtable hash = new Hashtable();

            int idx = 0;
            int cnt = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;

                if (c.IsEmpty)
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;
                    return (false);
                }

                cnt++;

                if (hash.ContainsKey(c.ID))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_DUPLICATED", null);
                    titem.IsSelected = true;
                    return (false);
                }

                hash.Add(c.ID, c);
            }

            if ((cnt <= 0) && chkCnt)
            {
                CHelper.ShowErorMessage(idx.ToString(), "ERROR_ITEM_COUNT", null);
                titem.IsSelected = true;
                return (false);
            }

            int limit = 100;
            if ((cnt > limit) && chkCnt)
            {
                CHelper.ShowErorMessage(limit.ToString(), "ERROR_ITEM_COUNT_EXCEED", null);
                titem.IsSelected = true;
                return (false);
            }

            return (true);
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

        private String getAccessRightEdit()
        {
            String acr = "INVENTORY_UNKNOW_EDIT";

            if (dt == InventoryDocumentType.InvDocImport)
            {
                acr = "INVENTORY_IMPORT_EDIT";
            }
            else if (dt == InventoryDocumentType.InvDocExport)
            {
                acr = "INVENTORY_EXPORT_EDIT";
            }
            else if (dt == InventoryDocumentType.InvDocXfer)
            {
                acr = "INVENTORY_XFER_EDIT";
            }
            else if (dt == InventoryDocumentType.InvDocAdjust)
            {
                acr = "INVENTORY_ADJUST_EDIT";
            }

            return (acr);
        }        

        public String ItemText
        {
            get
            {
                if (dt == InventoryDocumentType.InvDocImport)
                {
                    return (CLanguage.getValue("import_item"));
                }
                else if (dt == InventoryDocumentType.InvDocExport)
                {
                    return (CLanguage.getValue("export_item"));
                }
                else if (dt == InventoryDocumentType.InvDocXfer)
                {
                    return (CLanguage.getValue("xfer_item"));
                }
                else if (dt == InventoryDocumentType.InvDocAdjust)
                {
                    return (CLanguage.getValue("adjust_item"));
                }

                return ("");
            }
        }

        private void cmdAdd_Click(object sender, RoutedEventArgs e)
        {
            String caption = ItemText;
            if (dt == InventoryDocumentType.InvDocImport)
            {
                WinAddEditImportItem w = new WinAddEditImportItem(dt);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.Mode = "A";
                w.ParentView = (vw as MInventoryDoc);
                w.ShowDialog();
                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
            }
            else if((dt == InventoryDocumentType.InvDocExport) || (dt == InventoryDocumentType.InvDocXfer))
            {
                WinAddEditExportItem w = new WinAddEditExportItem(dt);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.DocumentDate = (vw as MInventoryDoc).DocumentDate;
                w.LocationObj = (vw as MInventoryDoc).LocationObj;
                w.Mode = "A";
                w.ParentView = (vw as MInventoryDoc);
                w.ShowDialog();
                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
            }
            else if ((dt == InventoryDocumentType.InvDocAdjust) && adjustByDelta)
            {
                WinAddEditAdjustItem w = new WinAddEditAdjustItem(dt);
                w.Caption = (String)(sender as Button).Content + " " + caption;
                w.Mode = "A";
                w.ParentView = (vw as MInventoryDoc);
                w.ShowDialog();
                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
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

            if (dt == InventoryDocumentType.InvDocImport)
            {
                WinAddEditImportItem w = new WinAddEditImportItem(dt);
                w.ViewData = (MInventoryTransactionImport)currentViewObj;
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.Mode = "E";
                w.ShowDialog();

                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
            }
            else if ((dt == InventoryDocumentType.InvDocExport) || (dt == InventoryDocumentType.InvDocXfer))
            {
                WinAddEditExportItem w = new WinAddEditExportItem(dt);
                w.ViewData = currentViewObj;
                w.LocationObj = (vw as MInventoryDoc).LocationObj;
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.Mode = "E";
                w.ShowDialog();

                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
            }
            else if ((dt == InventoryDocumentType.InvDocAdjust) && adjustByDelta)
            {
                WinAddEditAdjustItem w = new WinAddEditAdjustItem(dt);
                w.ViewData = currentViewObj;
                w.Caption = CLanguage.getValue("edit") + " " + caption;
                w.Mode = "E";
                w.ShowDialog();

                if (w.HasModified)
                {
                    vw.IsModified = true;
                }
            }            
        }        

        private void cmdAdjustItemAdd_Click(object sender, RoutedEventArgs e)
        {
            vw.IsModified = true;
            (vw as MInventoryDoc).AddAdjustment();
            vw.IsModified = true;
        }

        private void cmdAdjustmentDelete_Click(object sender, RoutedEventArgs e)
        {
            MInventoryAdjustment pp = (MInventoryAdjustment)(sender as Button).Tag;
            mv.RemoveAdjustment(pp);

            vw.IsModified = true;
        }
        
        private void cmdPreview_Click(object sender, RoutedEventArgs e)
        {
            String group = "xxxx";
            if (dt == InventoryDocumentType.InvDocExport)
            {
                group = "grpInventoryExport";
            }

            WinFormPrinting w = new WinFormPrinting(group, vw);
            w.ShowDialog();
        }

        private void UInventoryItemAdjustment_OnChanged(object sender, EventArgs e)
        {
            vw.IsModified = true;
        }
    }
}
