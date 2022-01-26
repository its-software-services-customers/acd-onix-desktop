using System;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using Wis.WsClientAPI;
using Onix.ClientCenter.Windows;
using Onix.Client.Model;
using Onix.Client.Helper;
using Onix.ClientCenter.Commons.Utils;
using Onix.ClientCenter.Commons.Windows;
using Onix.Client.Controller;

namespace Onix.ClientCenter.UI.Inventory.InventoryItem
{
    public partial class WinAddEditInventoryItem : WinBase
    {  
        private MInventoryItem mv = null;

        public WinAddEditInventoryItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "INVENTORY_ITEM_EDIT";

            createAPIName = "CreateInventoryItem";
            updateAPIName = "UpdateInventoryItem";
            getInfoAPIName = "GetInventoryItemInfo";

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, txtCode, false);
            registerValidateControls(lblReference, txtReference, false);
            registerValidateControls(lblReference, txtReference, false);
            registerValidateControls(lblNameThai, txtNameThai, false);
            registerValidateControls(lblNameEng, txtNameEng, false);
            registerValidateControls(lblType, cboType, false);
            registerValidateControls(lblUOM, cboUOM, false);
            registerValidateControls(lblBrand, cboBrand, true);
            registerValidateControls(lblUOMSale, cboUOMSale, false);
            registerValidateControls(lblItemGroup, cboItemGroup, false);
            registerValidateControls(lblMinimumAllowed, txtMinimumAllowed, false);

            CUtil.InitMasterRef(MasterRefEnum.MASTER_BARCODE_TYPE);
        }

        protected override MBaseModel createObject()
        {
            mv = new MInventoryItem(new CTable(""));
            mv.IsVatEligible = true;
            mv.IsForBorrow = false;
            mv.CreateDefaultValue();

            String categoryId = loadParam.GenericType;

            if (loadParam.Mode.Equals("A"))
            {
                if (!categoryId.Equals(""))
                {
                    mv.ItemCategory = categoryId;
                }
            }            

            return (mv);
        }        

        protected override Boolean postValidate()
        {
            Boolean result = validateBarcodes<MInventoryBarcodeItem>(mv.BarcodeItems, tbiBarcode);
            if (!result)
            {
                return (false);
            }

            return (true);
        }

        private Boolean validateBarcodes<T>(ObservableCollection<T> collection, TabItem titem) where T : MBaseModel
        {
            int idx = 0;
            foreach (MBaseModel c in collection)
            {
                idx++;
                MInventoryBarcodeItem bi = (MInventoryBarcodeItem)c;

                if (bi.Barcode.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;

                    return (false);
                }

                if (bi.BarcodeType.Equals(""))
                {
                    CHelper.ShowErorMessage(idx.ToString(), "ERROR_SELECTION_TYPE", null);
                    titem.IsSelected = true;

                    return (false);
                }
            }

            return (true);
        }
        
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveData();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }       

        private void cmdBarcodeAdd_Click(object sender, RoutedEventArgs e)
        {
            (mv as MInventoryItem).AddBarcode();
            mv.IsModified = true;
        }

        private void cmdBarcodeDelete_Click(object sender, RoutedEventArgs e)
        {
            MInventoryBarcodeItem pp = (MInventoryBarcodeItem)(sender as Button).Tag;
            mv.RemoveBarcodeItem(pp);

            mv.IsModified = true;
        }

        private void cmdInterval_Click(object sender, RoutedEventArgs e)
        {
            WinAddEditIntervalConfigEx w = new WinAddEditIntervalConfigEx(mv.PricingDefination, mv.ItemNameThai, "IP");
            w.ShowDialog();
            if (w.IsOK)
            {
                mv.PricingDefination = w.ConfigString;
                mv.CalculateSellUnitPrice();
                mv.IsModified = true;
            }
        }

        private void CmdMigrate_Click(object sender, RoutedEventArgs e)
        {
            CTable dat = new CTable("");
            dat.SetFieldValue("MAX_MIGRAGE", "5000");

            CUtil.EnableForm(false, this);
            CTable newobj = OnixWebServiceAPI.SubmitObjectAPI("MoveServiceToItem", dat);
            CUtil.EnableForm(true, this);
        }
    }
}
