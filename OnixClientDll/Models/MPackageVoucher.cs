using Wis.WsClientAPI;
using System;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MPackageVoucher : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";
        private Boolean voucherIs = false;

        private MInventoryItem ini = new MInventoryItem(new CTable(""));
        private MService svm = new MService(new CTable(""));
        private MItemCategory itc = new MItemCategory(new CTable(""));
        private MVoucherTemplate vvt = new MVoucherTemplate(new CTable(""));
        private int seq = 0;

        public MPackageVoucher(CTable obj) : base(obj)
        {
        }

        public String Key
        {
            get
            {
                String k = "";

                if (SelectionType.Equals("1"))
                {
                    //Service
                    k = SelectionType + "-" + ServiceID;
                }
                else if (SelectionType.Equals("2"))
                {
                    //Item
                    k = SelectionType + "-" + ItemID;
                }
                else if (SelectionType.Equals("3"))
                {
                    //Voucher
                    k = SelectionType + "-" + VoucherTempID;
                }
                else if (SelectionType.Equals("4"))
                {
                    //Voucher
                    k = SelectionType + "-" + FreeText;
                }

                return (k);
            }
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }

        public String PackageVoucherID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_VOUCHER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_VOUCHER_ID", value);
            }
        }

        public String PackageId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_ID", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String SelectionType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SELECTION_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SELECTION_TYPE", value);
            }
        }

        public String FreeText
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FREE_TEXT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FREE_TEXT", value);
            }
        }

        public String Quantity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUANTITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String QuantityType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("QUANTITY_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("QUANTITY_TYPE", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String EnabledFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENABLE_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENABLE_FLAG", value);

                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public bool IsVoucher
        {
            get
            {
                return (voucherIs);
            }

            set
            {
                voucherIs = value;
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

        #region Item
        public MBaseModel ItemObj
        {
            get
            {
                ini.ItemID = ItemID;
                ini.ItemCode = ItemCode;
                ini.ItemNameThai = ItemNameThai;

                return (ini);
            }

            set
            {
                if (value == null)
                {
                    ItemID = "";
                    ItemCode = "";
                    ItemNameThai = "";

                    return;
                }

                MInventoryItem ii = (value as MInventoryItem);
                ini.SetDbObject(ii.GetDbObject());

                ItemID = ii.ItemID;
                ItemCode = ii.ItemCode;
                ItemNameThai = ii.ItemNameThai;

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
        #endregion

        #region Service
        public MBaseModel ServiceObj
        {
            get
            {
                svm.ServiceID = ServiceID;
                svm.ServiceCode = ServiceCode;
                svm.ServiceName = ServiceName;

                return (svm);
            }

            set
            {
                if (value == null)
                {
                    ServiceID = "";
                    ServiceCode = "";
                    ServiceName = "";

                    return;
                }

                MService ii = (value as MService);
                svm.SetDbObject(ii.GetDbObject());

                ServiceID = ii.ServiceID;
                ServiceCode = ii.ServiceCode;
                ServiceName = ii.ServiceName;

                updateFlag();
            }
        }

        public String ServiceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_ID", value);
            }
        }

        public String ServiceCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_NAME", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        #region ItemCategory
        public MBaseModel ItemCategoryObj
        {
            get
            {
                itc.ItemCategoryID = ItemCategoryID;
                itc.CategoryName = ItemCategoryName;

                return (itc);
            }

            set
            {
                if (value == null)
                {
                    ItemCategoryID = "";
                    ItemCategoryName = "";

                    return;
                }

                MItemCategory ii = (value as MItemCategory);
                itc.SetDbObject(ii.GetDbObject());

                ItemCategoryID = ii.ItemCategoryID;
                ItemCategoryName = ii.CategoryName;

                updateFlag();
            }
        }

        public String ItemCategoryID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_CATEGORY", value);
                NotifyPropertyChanged();
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
        #endregion

        #region VoucheTemplate
        public MBaseModel VoucherObj
        {
            get
            {
                vvt.VoucherTempID = VoucherTempID;
                vvt.VoucherTempNO = VoucherTempNO;
                vvt.VoucherTempName = VoucherTempName;

                return (vvt);
            }

            set
            {
                if (value == null)
                {
                    VoucherTempID = "";
                    VoucherTempNO = "";
                    VoucherTempName = "";

                    return;
                }

                MVoucherTemplate ii = (value as MVoucherTemplate);
                vvt.SetDbObject(ii.GetDbObject());

                VoucherTempID = ii.VoucherTempID;
                VoucherTempNO = ii.VoucherTempNO;
                VoucherTempName = ii.VoucherTempName;

                updateFlag();
            }
        }

        public String VoucherTempID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VOUCHER_TEMPLATE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("VOUCHER_TEMPLATE_ID", value);
            }
        }

        public String VoucherTempNO
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VC_TEMPLATE_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("VC_TEMPLATE_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String VoucherTempName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VC_TEMPLATE_NNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VC_TEMPLATE_NNAME", value);
                NotifyPropertyChanged();
            }
        }
        #endregion

        public override Boolean IsEmpty
        {
            get
            {
                if (ItemID.Equals("") && ItemCategoryID.Equals("") && ServiceID.Equals("") 
                    && VoucherTempID.Equals("") && FreeText.Equals(""))
                {
                    return (true);
                }

                if (CUtil.StringToInt(ItemID) <= 0 &&
                    CUtil.StringToInt(ItemCategoryID) <= 0 &&
                    CUtil.StringToInt(VoucherTempID) <= 0 &&
                    CUtil.StringToInt(ServiceID) <= 0 &&
                    FreeText.Equals(""))
                {
                    return (true);
                }

                if (CUtil.StringToInt(Quantity) <= 0)
                {
                    return (true);
                }

                return (false);
            }
        }

        public override Boolean IsEnabled
        {
            get
            {
                return (EnabledFlag.Equals("Y"));
            }
        }

        public override String ID
        {
            get
            {
                String k = "";

                if (SelectionType.Equals("1"))
                {
                    //Service
                    k = SelectionType + "-" + ServiceID;
                }
                else if (SelectionType.Equals("2"))
                {
                    //Item
                    k = SelectionType + "-" + ItemID;
                }
                else if (SelectionType.Equals("3"))
                {
                    //Voucher
                    k = SelectionType + "-" + VoucherTempID;
                }
                else if (SelectionType.Equals("4"))
                {
                    //Voucher
                    k = SelectionType + "-" + FreeText;
                }

                return (k);
            }
        }

    }
}

   

