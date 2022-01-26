using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Windows.Media;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Collections;

namespace Onix.ClientCenter.UI.AccountPayable.StockCost
{
    public class MVStockCostDocument : MBaseModel
    {
        private MVStockCostDocumentItem beginStock = new MVStockCostDocumentItem(new CTable(""));
        private MVStockCostDocumentItem endStock = new MVStockCostDocumentItem(new CTable(""));
        private MVStockCostDocumentItem inStock = new MVStockCostDocumentItem(new CTable(""));
        private MVStockCostDocumentItem outStock = new MVStockCostDocumentItem(new CTable(""));

        private ObservableCollection<MVStockCostDocumentItem> items = new ObservableCollection<MVStockCostDocumentItem>();

        public MVStockCostDocument(CTable obj) : base(obj)
        {

        }

        public void constructTableObject()
        {
            //API will need to delete all item first and then add items again!!!

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COST_DOC_ITEMS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COST_DOC_ITEMS", arr);
            }

            arr.Clear();

            populateItem(arr, beginStock, "1");
            populateItem(arr, endStock, "2");
            populateItem(arr, inStock, "3");
            populateItem(arr, outStock, "4");
        }

        private void populateItem(ArrayList arr, MVStockCostDocumentItem item, String type)
        {
            item.ItemType = type;
            if (!item.ExtFlag.Equals("E"))
            {
                item.ExtFlag = "A";
            }
            arr.Add(item.GetDbObject());
        }

        public override void InitializeAfterLoaded()
        {
            Hashtable hash = new Hashtable();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("COST_DOC_ITEMS");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("COST_DOC_ITEMS", arr);
            }

            foreach (CTable t in arr)
            {
                String type = t.GetFieldValue("ITEM_TYPE");
                hash.Add(type, t);
            }

            for (int i = 1; i <= 4; i++)
            {
                String type = i.ToString();
                CTable obj = null;
                if (!hash.Contains(type))
                {
                    obj = new CTable("");
                    obj.SetFieldValue("ITEM_TYPE", type);
                    obj.SetFieldValue("EXT_FLAG", "A");

                    hash[type] = obj;
                }
            }

            beginStock = new MVStockCostDocumentItem((CTable)hash["1"]);
            endStock = new MVStockCostDocumentItem((CTable)hash["2"]);
            inStock = new MVStockCostDocumentItem((CTable)hash["3"]);
            outStock = new MVStockCostDocumentItem((CTable)hash["4"]);
        }

        public void CalculateOutStock()
        {
            MVStockCostDocumentItem o = new MVStockCostDocumentItem(new CTable(""));
            calculateField(o, "JanAmount");
            calculateField(o, "FebAmount");
            calculateField(o, "MarAmount");
            calculateField(o, "AprAmount");
            calculateField(o, "MayAmount");
            calculateField(o, "JunAmount");
            calculateField(o, "JulAmount");
            calculateField(o, "AugAmount");
            calculateField(o, "SepAmount");
            calculateField(o, "OctAmount");
            calculateField(o, "NovAmount");
            calculateField(o, "DecAmount");
            calculateField(o, "TotAmount");

            OutStock = o;

            BeginStockBalance = BeginStock.JanAmount;
            EndStockBalance = EndingStock.DecAmount;
            InAmount = InStock.TotAmount;
            OutAmount = OutStock.TotAmount;
        }

        private void calculateField(MVStockCostDocumentItem o, String fieldName)
        {
            Type typeBegin = BeginStock.GetType();
            PropertyInfo infoBegin = typeBegin.GetProperty(fieldName);
            double beginAmt = CUtil.StringToDouble((String) infoBegin.GetValue(BeginStock, null));

            Type typeIn = InStock.GetType();
            PropertyInfo infoIn = typeIn.GetProperty(fieldName);
            double inAmt = CUtil.StringToDouble((String) infoIn.GetValue(InStock, null));

            Type typeEnd = EndingStock.GetType();
            PropertyInfo infoEnd = typeEnd.GetProperty(fieldName);
            double endAmt = CUtil.StringToDouble((String)infoEnd.GetValue(EndingStock, null));

            double outAmt = beginAmt + inAmt - endAmt;

            Type typeOut = o.GetType();
            PropertyInfo infoOut = typeOut.GetProperty(fieldName);
            infoOut.SetValue(o, outAmt.ToString());                           
        }

        public MVStockCostDocumentItem BeginStock
        {
            get
            {
                return (beginStock);
            }
        }

        public MVStockCostDocumentItem EndingStock
        {
            get
            {
                return (endStock);
            }
        }

        public MVStockCostDocumentItem InStock
        {
            get
            {
                return (inStock);
            }
        }

        public MVStockCostDocumentItem OutStock
        {
            get
            {
                return (outStock);
            }

            set
            {
                outStock = value;
                NotifyPropertyChanged();
            }
        }

        public override SolidColorBrush RowColor
        {
            get
            {
                return (CUtil.DocStatusToColor(DocumentStatus));
            }
        }

        public Boolean IsEditable
        {
            get
            {
                String status = DocumentStatus;
                if (status.Equals(""))
                {
                    return (true);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocApproved)
                {
                    return (false);
                }

                if (Int32.Parse(status) == (int)InventoryDocumentStatus.InvDocCancelApproved)
                {
                    //Voided
                    return (false);
                }

                return (true);
            }
        }        

        public void CreateDefaultValue()
        {
        }
        
        public String CostDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COST_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COST_DOC_ID", value);
            }
        }

        public String CostDocYear
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COST_YEAR"));
            }

            set
            {
                GetDbObject().SetFieldValue("COST_YEAR", value);
            }
        }       
                
        #region Document Status

        public String DocumentStatus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public String DocumentStatusDesc
        {
            get
            {
                if (DocumentStatus.Equals(""))
                {
                    return ("");
                }

                CashDocumentStatus dt = (CashDocumentStatus)Int32.Parse(DocumentStatus);
                String str = CUtil.CashDocStatusToString(dt);

                return (str);
            }

            set
            {

            }
        }

        public MMasterRef DocumentStatusObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                DocumentStatus = m.MasterID;
            }
        }

        #endregion

        public String BeginStockBalance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_STOCK_BALANCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_STOCK_BALANCE", value);
                NotifyPropertyChanged();
            }
        }

        public String BeginStockBalanceFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(BeginStockBalance);
                return (fmt);
            }

            set
            {
            }
        }

        public String EndStockBalance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_STOCK_BALANCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_STOCK_BALANCE", value);
                NotifyPropertyChanged();
            }
        }

        public String EndStockBalanceFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(EndStockBalance);
                return (fmt);
            }

            set
            {
            }
        }

        public String InAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("IN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String InAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(InAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String OutAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OUT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("OUT_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String OutAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OutAmount);
                return (fmt);
            }

            set
            {
            }
        }
    }
}
