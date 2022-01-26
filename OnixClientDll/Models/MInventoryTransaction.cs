using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MInventoryTransaction : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        private MLocation locationObj = null;
        private DateTime documentDate = DateTime.Now;
        private Boolean isBalanceFwd = false;

        public MInventoryTransaction(CTable obj) : base(obj)
        {
            ExtFlag = "A";
        }

        public void CreateDefaultValue()
        {
            FifoAmount = "0.00";
            AvgAmount = "0.00";
        }

        public String InventoryTxID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_ID", value);
            }
        }

        public MBaseModel LotObj
        {
            get
            {
                MInventoryLot lt = new MInventoryLot(new CTable("LOT_TRACKING"));
                lt.LotNo = LotNo;
                lt.LotNote = LotNote;

                return (lt);
            }

            set
            {
                if (value == null)
                {
                    LotNo = "";
                    LotNote = "";

                    return;
                }

                MInventoryLot ii = (value as MInventoryLot);

                LotNo = ii.LotNo;
                LotNote = ii.LotNote;

                //NotifyPropertyChanged();
                updateFlag();
            }
        }

        public MBaseModel ItemObj
        {
            get
            {
                itm = new MInventoryItem(new CTable(""));
                itm.ItemCode = ItemCode;
                itm.ItemNameEng = ItemNameEng;
                itm.ItemNameThai = ItemNameThai;
                itm.ItemID = ItemID;

                return (itm);
            }

            set
            {
                if (value == null)
                {
                    ItemID = "";
                    ItemCode = "";
                    ItemNameEng = "";
                    ItemNameThai = "";
                    ItemUnitName = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                itm.SetDbObject(ii.GetDbObject());

                ItemCode = ii.ItemCode;
                ItemNameEng = ii.ItemNameEng;
                ItemNameThai = ii.ItemNameThai;
                ItemID = ii.ItemID;
                ItemUnitName = ii.ItemUOMName;

                //NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_ID", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CODE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemNameThai
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_NAME_THAI"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_NAME_THAI", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String SelectItemName
        {
            get
            {
                return (ItemNameThai);
            }

            set
            {
            }
        }

        public String ItemNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_NAME_ENG", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_QUANTITY", value);
                calculateItemAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemAdjQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_QUANTITY", value);
                calculateItemPrice();

                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemQuantityFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemQuantity));
            }
        }

        public String ItemAdjPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_PRICE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_PRICE", value);
                calculateItemAmount();

                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String CostAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_AMOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String CostAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(CostAmount));
            }
        }

        public String CostPerUnit
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_PRICE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String CostPerUnitFmt
        {
            get
            {
                return (CUtil.FormatNumber(CostPerUnit));
            }
        }

        private void calculateItemAmount()
        {
            Double v1 = CUtil.StringToDouble(ItemQuantity);
            Double v2 = CUtil.StringToDouble(ItemPrice);

            ItemAmount = (v1 * v2).ToString();
        }

        public String ItemPriceFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemPrice));
            }

            set
            {
            }
        }

        public String AvgAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AVG_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AVG_AMOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String FifoAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FIFO_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FIFO_AMOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_AMOUNT", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemAdjAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_AMOUNT", value);
                calculateItemPrice();
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        private void calculateItemPrice()
        {
            Double v1 = CUtil.StringToDouble(ItemAdjQuantity);
            Double v2 = CUtil.StringToDouble(ItemAdjAmount);

            if (v1 == 0)
            {
                ItemAdjPrice = "0.00";
            }
            else
            {
                ItemAdjPrice = (v2 / v1).ToString("0.##");
            }
        }

        public String ItemAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemAmount));
            }

            set
            {
            }
        }

        public String ItemPriceAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UNIT_PRICE_AVG"));
            }

            set
            {
                GetDbObject().SetFieldValue("UNIT_PRICE_AVG", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemPriceAvgFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemPriceAvg));
            }
        }

        public String ItemPriceFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UNIT_PRICE_FIFO"));
            }

            set
            {
                GetDbObject().SetFieldValue("UNIT_PRICE_FIFO", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemPriceFifoFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemPriceFifo));
            }
        }

        public String ItemAmountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AMOUNT_AVG"));
            }

            set
            {
                GetDbObject().SetFieldValue("AMOUNT_AVG", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemAmountAvgFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemAmountAvg));
            }
        }

        public String ItemAmountFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AMOUNT_FIFO"));
            }

            set
            {
                GetDbObject().SetFieldValue("AMOUNT_FIFO", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String ItemAmountFifoFmt
        {
            get
            {
                return (CUtil.FormatNumber(ItemAmountFifo));
            }
        }

        public String LotNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOT_NO", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String LotNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOT_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOT_NOTE", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        public MLocation LocationObj
        {
            set
            {
                locationObj = value;
            }

            get
            {
                return (locationObj);
            }
        }

        public Boolean UpSelected
        {
            set
            {
                TxType = "I";
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (TxType.Equals("I"));
            }
        }

        public Boolean DownSelected
        {
            set
            {
                TxType = "E";
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (TxType.Equals("E"));
            }
        }

        public String UIItemAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_AMOUNT"));
            }
        }
        public String UIItemAmountFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(UIItemAmount));
            }
        }

        public String UIItemUnitPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UI_ITEM_UNIT_PRICE"));
            }
        }
        public String UIItemUnitPriceFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(UIItemUnitPrice));
            }
        }


        #region Movement
        public DateTime DocumentDate
        {
            get
            {
                return (documentDate);
            }

            set
            {
                documentDate = value;
            }
        }

        public String DocumentDateFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (CLanguage.getValue("balance_forward"));
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);

            }

            set
            {
            }
        }

        public String DocumentNo
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_NO");
                return (str);

            }
        }

        public String TxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("TX_TYPE");
                return (str);

            }

            set
            {
                GetDbObject().SetFieldValue("TX_TYPE", value);
                String factor = "-1";
                if (value.Equals("I"))
                {
                    factor = "1";
                }
                GetDbObject().SetFieldValue("FACTOR", factor);
            }
        }

        public String TxText
        {
            get
            {
                return (CLanguage.getValue(TxType));
            }
        }

        public String InQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("ITEM_QUANTITY")));
                }

                return ("");

            }
        }

        public String OutQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("ITEM_QUANTITY")));
                }

                return ("");

            }
        }

        public String InAmountAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_AVG")));
                }

                return ("");

            }
        }

        public String InAmountFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("I"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_FIFO")));
                }

                return ("");

            }
        }

        public String OutAmountAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_AVG")));
                }

                return ("");

            }
        }

        public String OutAmountFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (TxType.Equals("E"))
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_FIFO")));
                }

                return ("");

            }
        }

        public String LeftQuantityFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_QUANTITY")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_QUANTITY")));
                }
            }
        }

        public String LeftQuantityAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_QUANTITY")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_QUANTITY")));
                }
            }
        }

        public String LeftAmountFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_AMOUNT_FIFO")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_AMOUNT_FIFO")));
                }
            }
        }

        public Boolean? IsLotSerial
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("LOT_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("LOT_FLAG", flag);
            }
        }

        public String LeftAmountAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("BEGIN_AMOUNT_AVG")));
                }
                else
                {
                    return (CUtil.FormatNumber(GetDbObject().GetFieldValue("END_AMOUNT_AVG")));
                }
            }
        }

        public String UnitPriceAvgFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                Double q;
                Double a;
                if (IsBalanceForward)
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("BEGIN_QUANTITY"));
                    a = Double.Parse(GetDbObject().GetFieldValue("BEGIN_AMOUNT_AVG"));
                }
                else
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("END_QUANTITY"));
                    a = Double.Parse(GetDbObject().GetFieldValue("END_AMOUNT_AVG"));
                }

                Double r = 0.00;
                if (q != 0)
                {
                    r = a / q;
                }

                return (CUtil.FormatNumber(r.ToString()));
            }
        }

        public String UnitPriceFifoFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                Double q;
                Double a;
                if (IsBalanceForward)
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("BEGIN_QUANTITY"));
                    a = Double.Parse(GetDbObject().GetFieldValue("BEGIN_AMOUNT_FIFO"));
                }
                else
                {
                    q = Double.Parse(GetDbObject().GetFieldValue("END_QUANTITY"));
                    a = Double.Parse(GetDbObject().GetFieldValue("END_AMOUNT_FIFO"));
                }

                Double r = 0.00;
                if (q != 0)
                {
                    r = a / q;
                }

                return (CUtil.FormatNumber(r.ToString()));
            }
        }

        public Boolean IsBalanceForward
        {
            get
            {
                return (isBalanceFwd);
            }

            set
            {
                isBalanceFwd = value;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (FontWeights.Bold);
                }

                return (FontWeights.Normal);
            }
        }

        public String LocationID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID", value);
                //NotifyPropertyChanged();

                //updateFlag();
            }
        }

        public String LocationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_NAME", value);
                //NotifyPropertyChanged();

                //updateFlag();
            }
        }

        public String LocationToName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_TO_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_TO_NAME", value);
                //NotifyPropertyChanged();

                //updateFlag();
            }
        }
        #endregion

        public String ItemUnitName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_UNIT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UNIT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemUnitNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_UNIT_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UNIT_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        #region Project

        public MBaseModel ProjectObj
        {
            set
            {
                if (value == null)
                {
                    ProjectID = "";
                    ProjectName = "";
                    ProjectCode = "";

                    return;
                }

                MProject m = value as MProject;
                ProjectID = m.ProjectID;
                ProjectName = m.ProjectName;
                ProjectCode = m.ProjectCode;
                ProjectGroupName = m.ProjectGroupName;

                NotifyPropertyChanged("ProjectGroupName");
            }

            get
            {
                MProject m = new MProject(new CTable(""));
                m.ProjectID = ProjectID;
                m.ProjectName = ProjectName;
                m.ProjectCode = ProjectCode;
                m.ProjectGroupName = ProjectGroupName;

                return (m);
            }
        }

        public String ProjectCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_CODE", value);
            }
        }

        public String ProjectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_ID", value);
            }
        }

        public String ProjectName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_NAME", value);
            }
        }

        public String ProjectGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP_NAME", value);
            }
        }
        #endregion

        public String ReturnedAllFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RETURNED_ALL_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("RETURNED_ALL_FLAG", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String BorrowID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BORROW_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BORROW_ID", value);
            }
        }

        public String BorrowDocumentNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BORROW_DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("BORROW_DOCUMENT_NO", value);
            }
        }
        
        public String ReturnQuantityNeed
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RETURNED_QUANTITY_NEED"));
            }

            set
            {
                GetDbObject().SetFieldValue("RETURNED_QUANTITY_NEED", value);
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        #region
        public MMasterRef ReasonObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ReasonID = m.MasterID;
                    ReasonDesc = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ReasonID;
                return (mr);
            }
        }

        public String ReasonID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("REASON_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("REASON_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String ReasonDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REASON_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("REASON_DESC", value);
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}