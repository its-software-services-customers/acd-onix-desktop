using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Windows;

namespace Onix.ClientCenter.UI.Inventory.InventoryDocument
{
    public partial class WinAddEditBorrowReturnItem : WinBase
    {
        private InventoryDocumentType dt;
        private MInventoryDoc mvParent = null;
        private MInventoryTransaction mv = null;

        public WinAddEditBorrowReturnItem(CWinLoadParam param) : base(param)
        {
            accessRightName = "INVENTORY_BORROW_EDIT";
            mvParent = (MInventoryDoc)loadParam.ActualParentView;

            dt = (InventoryDocumentType)CUtil.StringToInt(param.GenericType);

            InitializeComponent();

            //Need to be after InitializeComponent
            registerValidateControls(lblCode, lkupItem, false);
            registerValidateControls(lblQuantity, txtQuantity, false);
            registerValidateControls(lblUnitPrice, txtUnitPrice, false);
            registerValidateControls(lblLotNo, txtLotSerial1, false);
        }

        protected override void addSubItem()
        {
            MInventoryDoc parent = (MInventoryDoc)loadParam.ActualParentView;
            parent.AddTxItem(mv, dt);
        }

        protected override MBaseModel createObject()
        {
            mv = new MInventoryTransaction(new CTable(""));
            mv.CreateDefaultValue();

            mv.TxType = "E";
            mv.ReturnedAllFlag = "N";
            if (dt == InventoryDocumentType.InvDocReturn)
            {
                mv.TxType = "I";
            }

            return (mv);
        }

        protected override bool isEditable()
        {
            if (mvParent != null)
            {
                return (mvParent.IsEditable);
            }

            return (true);
        }

        public Boolean IsBorrowEditable
        {
            get
            {
                return (dt == InventoryDocumentType.InvDocBorrow);
            }
        }
        
        private void cmdOK_Click(object sender, RoutedEventArgs e)
        {
            Boolean r = saveDataItem();
            if (r)
            {
                vw.IsModified = false;
                IsOKClick = true;
                CUtil.EnableForm(true, this);
                this.Close();
            }
        }        
    }
}
