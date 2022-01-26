using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using System.Collections;
using Onix.Client.Pricing;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MInventoryItem : MBaseModel
    {
        private ObservableCollection<MInventoryBarcodeItem> barcodes = new ObservableCollection<MInventoryBarcodeItem>();
        private ObservableCollection<MInventoryCurrentBalance> currentBalance = new ObservableCollection<MInventoryCurrentBalance>();

        private int internalSeq = 0;

        public MInventoryItem(CTable obj) : base(obj)
        {
		
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("item_code", ItemCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("item_name_thai", ItemNameThai);
            ttItems.Add(ct);

            ct = new CToolTipItem("minimum_allowed", MinimumAllowed);
            ttItems.Add(ct);

            ct = new CToolTipItem("default_sell_price", DefaultSellPriceFmt);
            ttItems.Add(ct);

        }

        public override void InitializeAfterNotified()
        {
            InitSubItem();
        }

        public String PricingDefination
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICING_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICING_DEFINITION", value);
                NotifyPropertyChanged("PriceConfigIcon");
            }
        }

        #region temp

        public String Temp0
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP0"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP0", value);
            }
        }

        public String Temp1
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP1"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP1", value);
            }
        }

        public String Temp2
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP2"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP2", value);
            }
        }

        public String Temp3
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP3"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP3", value);
            }
        }

        public String Temp4
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP4"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP4", value);
            }
        }

        public String Temp5
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP5"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP5", value);
            }
        }

        public String Temp6
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TEMP6"));
            }

            set
            {
                GetDbObject().SetFieldValue("TEMP6", value);
            }
        }

        #endregion temp

        public String DefaultSellPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEFAULT_SELL_PRICE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEFAULT_SELL_PRICE", value);
            }
        }

        public String DefaultSellPriceFmt
        {
            get
            {

                return (CUtil.FormatNumber(DefaultSellPrice));
            }

            set
            {
            }
        }

        public String PriceConfigIcon
        {
            get
            {
                if (PricingDefination.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }

        public void CreateDefaultValue()
        {
            IsRawMaterrial = false;
            IsFinishedGood = false;
            IsForProduction = false;
            IsForPurchase = false;
            IsForSale = false;
            IsPartItem = false;
        }

        public ObservableCollection<MInventoryBarcodeItem> BarcodeItems
        {
            get
            {
                return (barcodes);
            }

            set
            {
                //barcodes = value;
            }
        }

		public ObservableCollection<MInventoryCurrentBalance> BalanceItems
        {
            get
            {
                return (currentBalance);
            }

            set
            {
                //barcodes = value;
            }
        }

        public void AddBarcode()
        {
            CTable t = new CTable("ITEM_BARCODE");
            MInventoryBarcodeItem v = new MInventoryBarcodeItem(t);

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("BARCODE_ITEM");
            if (arr == null)
            {
                arr = new ArrayList();
                o.AddChildArray("BARCODE_ITEM", arr);
            }

            v.Seq = internalSeq;
            internalSeq++;

            arr.Add(t);
            barcodes.Add(v);

            v.ExtFlag = "A";
        }

        public void RemoveBarcodeItem(MInventoryBarcodeItem vp)
        {
            removeAssociateItems(vp, "BARCODE_ITEM", "INTERNAL_SEQ", "ITEM_BC_ID");
            barcodes.Remove(vp);
        }

		public void InitBalanceItem()
        {
            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("CURRENT_BALANCE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MInventoryCurrentBalance v = new MInventoryCurrentBalance(t);
                currentBalance.Add(v);                
            }
        }

        public void InitSubItem()
        {
            barcodes.Clear();

            CTable o = GetDbObject();
            ArrayList arr = o.GetChildArray("BARCODE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MInventoryBarcodeItem v = new MInventoryBarcodeItem(t);
                barcodes.Add(v);

                v.Seq = internalSeq;
                internalSeq++;

                v.ExtFlag = "I";
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
            }
        }

        public String ItemType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ITEM_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemCategory
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ITEM_CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

        public MItemCategory ItemCategoryObj
        {
            set
            {
                MItemCategory m = value;
                if (m != null)
                {
                    ItemCategory = m.ItemCategoryID;
                    ItemCategoryName = m.CategoryName;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MItemCategory> items = CMasterReference.Instance.ItemCategoryPaths;
                if (items == null)
                {
                    return (null);
                }

                return ((MItemCategory)CUtil.IDToObject(items, "ItemCategoryID", ItemCategory));
            }
        }

        public String ItemCategoryName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef ItemTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ItemType = m.MasterID;
                    ItemTypeName = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ItemType;
                return (mr);
            }
        }

        public String ItemTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("TYPE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef ItemUOMObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ItemUOM = m.MasterID;
                    ItemUOMName = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ItemUOM;
                return (mr);
            }
        }
    
        public String ItemUOM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("ITEM_UOM"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_UOM", value);
                NotifyPropertyChanged();
            }
        }

        public String ItemUOMName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("UOM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("UOM_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String BrandID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("BRAND"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRAND", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef BrandObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BrandID = m.MasterID;
                BrandName = m.Description;
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = BrandID;
                return (mr);
            }
        }

        public String BrandName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRAND_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRAND_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ReferenceCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REFERENCE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("REFERENCE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsFinishedGood
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("FINISH_GOOD_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("FINISH_GOOD_FLAG", flag);
            }
        }

        public Boolean? IsPartItem
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("PART_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("PART_FLAG", flag);
            }
        }

        public Boolean? IsVatEligible
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("VAT_FLAG");
                if (flag.Equals("") || (flag == null))
                {
                    return (null);
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

                GetDbObject().SetFieldValue("VAT_FLAG", flag);
            }
        }

        public Boolean? IsRawMaterrial
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("RM_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("RM_FLAG", flag);
            }
        }

        public Boolean? IsForPurchase
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("PURCHASE_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("PURCHASE_FLAG", flag);
            }
        }

        public Boolean? IsForSale
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("SALE_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("SALE_FLAG", flag);
            }
        }

        public Boolean? IsForProduction
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("PRODUCTION_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("PRODUCTION_FLAG", flag);
            }
        }

		public String MinimumAllowed
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}
				return (GetDbObject().GetFieldValue("MINIMUM_ALLOW"));
			}
			set
			{
				GetDbObject().SetFieldValue("MINIMUM_ALLOW", value);
				NotifyPropertyChanged();
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

		public MMasterRef ItemUOMSaleObj
		{
			set
			{
                if (value == null)
                {
                    return;
                }

				MMasterRef m = value as MMasterRef;
				ItemUOMSale = m.MasterID;
				ItemUOMName = m.Description;

			}

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ItemUOMSale;
                return (mr);
            }
        }

		public String ItemUOMSale
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("0");
				}

				return (GetDbObject().GetFieldValue("SALE_UOM"));
			}

			set
			{
				GetDbObject().SetFieldValue("SALE_UOM", value);
				NotifyPropertyChanged();
			}
		}
		public String ItemUOMSaleName
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("UOM_NAME"));
			}

			set
			{
				GetDbObject().SetFieldValue("UOM_NAME", value);
				NotifyPropertyChanged();
			}
		}

        public String LinearPrice
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String price = CUtil.FormatNumber(GetDbObject().GetFieldValue("ITEM_LINEAR"));
                return (price);
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_LINEAR", value);
                NotifyPropertyChanged();
            }
        }

        public String PriceCategory
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRICE_CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRICE_CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

		public String FromQty
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("FROM_QTY"));
			}

			set
			{
				GetDbObject().SetFieldValue("FROM_QTY", value);
				NotifyPropertyChanged();
			}
		}


		public String ToQty
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("TO_QTY"));
			}

			set
			{
				GetDbObject().SetFieldValue("TO_QTY", value);
				NotifyPropertyChanged();
			}
		}

		public String UnitQty
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("UNIT_PRICE"));
			}

			set
			{
				GetDbObject().SetFieldValue("UNIT_PRICE", value);
				NotifyPropertyChanged();
			}
		}

        public String ToolTipText
        {
            get
            {
                return (ItemNameEng);
            }
        }

        public void CalculateSellUnitPrice()
        {
            CPromotionProcessor pp = new CPromotionProcessor(null, "", null);

            MIntervalConfig ivc = new MIntervalConfig(new CTable(""));
            ivc.DeserializeConfig(PricingDefination);

            MIntervalConfig ivcm = new MIntervalConfig(ivc.GetDbObject());
            ivcm.DeserializeConfig(PricingDefination);

            MInventoryItem mi = new MInventoryItem(this.GetDbObject());

            CBasketItem bi = new CBasketItem(ItemCode, mi, 1);
            CPrice o = null;
            if (ivcm.SelectionType == 1)
            {
                //step
                o = pp.getStepPrice(ivcm, bi);
            }
            else
            {
                //Tier
                o = pp.getTierPrice(ivcm, bi);
            }

            if (o != null)
            {
                DefaultSellPrice = o.UnitPrice.ToString();
            }
        }

        public Boolean? IsForBorrow
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("BORROW_FLAG");
                if (flag.Equals(""))
                {
                    return (null);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("BORROW_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String BorrowFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BORROW_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("BORROW_FLAG", value);
                NotifyPropertyChanged();
            }
        }
    }
}
