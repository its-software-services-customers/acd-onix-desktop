using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MInventoryCurrentBalance : MBaseModel
    {
        private MInventoryItem itm = new MInventoryItem(new CTable("INVENTORY_ITEM"));
        private ObservableCollection<MInventoryLot> currentLotTrackings = new ObservableCollection<MInventoryLot>();
        private ObservableCollection<MBalanceTransaction> currentMovements = new ObservableCollection<MBalanceTransaction>();
        private ObservableCollection<MInventoryCurrentBalance> movementSummaries = new ObservableCollection<MInventoryCurrentBalance>();

        private Boolean isBalanceFwd = false;

        public MInventoryCurrentBalance(CTable obj) : base(obj)
        {

        }

        

        public void CreateDefaultValue()
        {

        }

        public ObservableCollection<MInventoryCurrentBalance> MovementSummaries
        {
            get
            {
                return (movementSummaries);
            }
        }

        public void InitMovementSummaries()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("SUMMARY_ITEM");

            if (arr == null)
            {
                return;
            }

            movementSummaries = null;
            movementSummaries = new ObservableCollection<MInventoryCurrentBalance>();

            int seq = 0;
            foreach (CTable t in arr)
            {
                MInventoryCurrentBalance v = new MInventoryCurrentBalance(t);
                if (seq == 0)
                {
                    MInventoryCurrentBalance fw = new MInventoryCurrentBalance(t.Clone());
                    fw.IsBalanceForward = true;

                    fw.EndQuantity = fw.BeginQuantity;
                    fw.EndAmountAvg = fw.BeginAmountAvg;
                    fw.EndAmountFifo = fw.BeginAmountFifo;
                    fw.EndAvg = fw.BeginUnitPriceAvg;
                    fw.EndAvgFifo = fw.BeginUnitPriceFifo;

                    movementSummaries.Add(fw);
                }

                movementSummaries.Add(v);

                seq++;
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

        public ObservableCollection<MBalanceTransaction> CurrentMovements
        {
            get
            {
                return (currentMovements);
            }
        }

        public void InitCurrentMovements()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("MOVEMENT_ITEM");

            if (arr == null)
            {
                return;
            }

            currentMovements = null;
            currentMovements = new ObservableCollection<MBalanceTransaction>();

            int seq = 0;
            foreach (CTable t in arr)
            {
                MBalanceTransaction v = new MBalanceTransaction(t);
                if (seq == 0)
                {
                    MBalanceTransaction fw = new MBalanceTransaction(t.Clone());
                    fw.IsBalanceForward = true;
                    currentMovements.Add(fw);
                }

                currentMovements.Add(v);

                seq++;
            }
        }

        public ObservableCollection<MInventoryLot> CurrentLotTrackings
        {
            get
            {
                return (currentLotTrackings);
            }

            set
            {
                //barcodes = value;
            }
        }

        public void InitCurrentLotTrackings()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("LOT_ITEM");

            if (arr == null)
            {
                return;
            }

            currentLotTrackings = null;
            currentLotTrackings = new ObservableCollection<MInventoryLot>(); 

            foreach (CTable t in arr)
            {
                MInventoryLot v = new MInventoryLot(t);
                currentLotTrackings.Add(v);
            }
        }

        public String CurrentBalanceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INV_BALANCE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INV_BALANCE_ID", value);
            }
        }

        public MInventoryItem ItemObj
        {
            get
            {
                return (itm);
            }

            set
            {
                itm = value;
                NotifyPropertyChanged();
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
            }
        }

        public String ItemName
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
            }
        }

        public MLocation LocationObj
        {
            set
            {
                MLocation m = value as MLocation;
                LocationID = m.LocationID;
                LocationName = m.Description;
            }
        }

        public String LocationID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("LOCATION_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_ID", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
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
                GetDbObject().SetFieldValue("LOCATION_DESTINATION_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String BalanceDateFmt
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

                String str = GetDbObject().GetFieldValue("BALANCE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);

            }
        }

		public String DocDate
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
		}

		public String BeginQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_QUANTITY", value);
            }
        }

        public String BeginQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(BeginQuantity));
            }
        }

        public String BeginAmountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT_AVG"));
            }
        }

		public String DocNo
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
			}
		}

		public String BeginAmountAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(BeginAmountAvg));
            }
        }

        public String BeginAmountFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT_FIFO"));
            }
        }

        public String BeginAmountFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(BeginAmountFifo));
            }
        }

        public String BeginUnitPriceAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_UNIT_PRICE_AVG"));
            }
        }

        public String BeginUnitPriceAvgFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(BeginUnitPriceAvg));
            }
        }

        public String BeginUnitPriceFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_UNIT_PRICE_FIFO"));
            }
        }

        public String BeginUnitPriceFifoFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(BeginUnitPriceFifo));
            }
        }

        public String EndQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_QUANTITY", value);
                NotifyPropertyChanged();
            }
        }

        public String EndQuantityFmt
        {
            get
            {
                //if (IsBalanceForward)
                //{
                //    return ("");
                //}

                return (CUtil.FormatNumber(EndQuantity));
            }
        }

        public String EndAmountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_AMOUNT_AVG"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT_AVG", value);
                NotifyPropertyChanged();
            }
        }

        public String EndAmountAvgFmt
        {
            get
            {
                //if (IsBalanceForward)
                //{
                //    return ("");
                //}

                return (CUtil.FormatNumber(EndAmountAvg));
            }
        }


        public String EndAmountFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_AMOUNT_FIFO"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT_FIFO", value);
                NotifyPropertyChanged();
            }
        }

        public String EndAmountFifoFmt
        {
            get
            {
                //if (IsBalanceForward)
                //{
                //    return ("");
                //}

                return (CUtil.FormatNumber(EndAmountFifo));
            }
        }

        public String EndAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_UNIT_PRICE_AVG"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_UNIT_PRICE_AVG", value);
                NotifyPropertyChanged();
            }
        }

        public String EndAvgFmt
        {
            get
            {
                //if (IsBalanceForward)
                //{
                //    return ("");
                //}

                return (CUtil.FormatNumber(EndAvg));
            }
        }

        public String EndAvgFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_UNIT_PRICE_FIFO"));
            }

            set
            {
                GetDbObject().SetFieldValue("END_UNIT_PRICE_FIFO", value);
                NotifyPropertyChanged();
            }
        }

        public String EndAvgFifoFmt
        {
            get
            {
                //if (IsBalanceForward) 
                //{
                //    return ("");
                //}

                return (CUtil.FormatNumber(EndAvgFifo));
            }
        }

        public String InQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IN_QUANTITY"));
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

                return (CUtil.FormatNumber(InQuantity));
            }
        }

        public String OutQuantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OUT_QUANTITY"));
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

                return (CUtil.FormatNumber(OutQuantity));
            }
        }

        public String InAmountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IN_AMOUNT_AVG"));
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

                return (CUtil.FormatNumber(InAmountAvg));
            }
        }

        public String OutAmountAvg
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OUT_AMOUNT_AVG"));
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

                return (CUtil.FormatNumber(OutAmountAvg));
            }
        }

        public String InAmountFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IN_AMOUNT_FIFO"));
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

                return (CUtil.FormatNumber(InAmountFifo));
            }
        }

        public String OutAmountFifo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OUT_AMOUNT_FIFO"));
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

                return (CUtil.FormatNumber(OutAmountFifo));
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
		}
        public String ItemQuantityFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(ItemQuantity));
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

			}
		}

		public String UnitPriceAVG
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("UNIT_PRICE_AVG"));
			}
		}

		public String UnitPriceFIFO
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("UNIT_PRICE_FIFO"));
			}
		}

        public String EndUnitPriceAVG
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_UNIT_PRICE_AVG"));
            }
        }

        public String EndUnitPriceAVGFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(EndUnitPriceAVG));
            }
        }

        public String EndUnitPriceFIFO
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("END_UNIT_PRICE_FIFO"));
            }
        }

        public String EndUnitPriceFIFOFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                return (CUtil.FormatNumber(EndUnitPriceFIFO));
            }
        }


        public String IsAlertIcon
        {
            get
            {
                Decimal minAllow = Convert.ToDecimal(GetDbObject().GetFieldValue("MINIMUM_ALLOW"));
                Decimal qty = Convert.ToDecimal(CUtil.FormatNumber(EndQuantity));
                if (minAllow > qty)
                {

                    return ("pack://application:,,,/OnixClient;component/Images/alert-icon.png");
                }

                return ("");
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

		public String InQuantityMovement
		{
			get
			{

				if (TxType.Equals("I"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("ITEM_QUANTITY")));
				}
				else
				{
					return ("0");
				}
			}
		}

		public String OutQuantityMovement
		{
			get
			{

				if (TxType.Equals("E"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("ITEM_QUANTITY")));
				}
				else
				{
					return ("0");
				}

			}
		}

		public String InAmountMovementFifo
		{
			get
			{
				if (TxType.Equals("I"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_FIFO")));
				}
				else
				{
					return ("0");
				}
			}

		}
		public String InAmountMovementAvg
		{
			get
			{
				if (TxType.Equals("I"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_AVG")));
				}
				else
				{
					return ("0");
				}
			}

		}



		public String OutAmountMovementFifo
		{
			get
			{
				if (TxType.Equals("E"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_FIFO")));
				}
				else
				{
					return ("0");
				}
			}
		}

		public String OutAmountMovementAvg
		{
			get
			{
				if (TxType.Equals("E"))
				{
					return (CUtil.FormatNumber(GetDbObject().GetFieldValue("AMOUNT_AVG")));
				}
				else
				{
					return ("0");
				}
			}
		}

        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }
                return (GetDbObject().GetFieldValue("NOTE"));
            }
            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                NotifyPropertyChanged();
            }
        }

    }
}
