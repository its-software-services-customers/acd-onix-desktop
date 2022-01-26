using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{

    public class MService : MBaseModel
    {
        public MService(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("service_code", ServiceCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("service_name", ServiceName);
            ttItems.Add(ct);

            ct = new CToolTipItem("service_type", ServiceTypeName);
            ttItems.Add(ct);

            ct = new CToolTipItem("wh_tax_pct", WHTaxPct);
            ttItems.Add(ct);

        }

        #region DefaultValue

        public void CreateDefaultValue()
        {

        }

        #endregion

        #region Service

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

        public String PricingDefinition
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
                NotifyPropertyChanged();
                NotifyPropertyChanged("PriceDefinitionConfigIcon");
            }
        }

        public String PriceDefinitionConfigIcon
        {
            get
            {
                if (PricingDefinition.Equals(""))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/error_24.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/ok-icon.png");
            }

            set
            {
            }
        }

        public String WHTaxPct
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_TAX_PCT"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_TAX_PCT", value);
            }
        }

        public Boolean? IsWHTax
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("WH_TAX_FLAG");
                if (flag.Equals("N"))
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
                GetDbObject().SetFieldValue("WH_TAX_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Service Type

        public String ServiceLevel
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_LEVEL"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_LEVEL", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceCategory
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef ServiceTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ServiceType = m.MasterID;
                    ServiceTypeName = m.Description;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ServiceType;
                mr.Description = ServiceTypeName;

                return (mr);
            }
        }

        public String ServiceType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("SERVICE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_TYPE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        #endregion


        #region UOM

        public MMasterRef ServiceUomObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ServiceUOM = m.MasterID;
                    ServiceUOMName = m.Description;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ServiceUOM;
                mr.Description = ServiceUOMName;

                return (mr);
            }
        }

        public String ServiceUOM
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("SERVICE_UOM"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_UOM", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceUOMName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_UOM_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_UOM_NAME", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region WH Group

        public MMasterRef WHGroupObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    WHGroup = m.MasterID;
                    WHGroupName = m.Description;
                    WHGroupNameEng = m.DescriptionEng;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = WHGroup;
                mr.Description = WHGroupName;
                mr.DescriptionEng = WHGroupNameEng;

                return (mr);
            }
        }

        public String WHGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP", value);
                NotifyPropertyChanged();
            }
        }

        public String WHGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String WHGroupNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WH_GROUP_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("WH_GROUP_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        public Boolean? IsVatEligible
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("VAT_FLAG");
                if (flag.Equals("") || (flag == null))
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
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("VAT_FLAG", flag);
            }
        }

        public Boolean IsForSale
        {
            get
            {
                return (Category.Equals("1"));
            }

            set
            {
                if (value)
                {
                    Category = "1";
                }
            }
        }

        public Boolean IsForPurchase
        {
            get
            {
                return (Category.Equals("2"));
            }

            set
            {
                if (value)
                {
                    Category = "2";
                }
            }
        }

        public Boolean IsForBoth
        {
            get
            {
                return (Category.Equals("0"));
            }

            set
            {
                if (value)
                {
                    Category = "0";
                }
            }
        }

        public MMasterRef CategoryObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    Category = m.MasterID;
                    CategoryName = m.Description;
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = Category;
                mr.Description = CategoryName;

                return (mr);
            }
        }

        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                String cat = GetDbObject().GetFieldValue("CATEGORY");
                if (cat.Equals(""))
                {
                    cat = "0";
                }

                return (cat);
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
                //NotifyPropertyChanged();
            }
        }

        public String CategoryName
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
                //NotifyPropertyChanged();
            }
        }

        public Boolean? IsSalary
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_SALARY");
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
                GetDbObject().SetFieldValue("IS_SALARY", flag);
            }
        }
    }
}
