using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MPackageCustomer : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MEntity cst = new MEntity(new CTable(""));
        private MMasterRef grp = new MMasterRef(new CTable(""));
        private MMasterRef type = new MMasterRef(new CTable(""));
        private int seq = 0;

        public MPackageCustomer(CTable obj) : base(obj)
        {
        }

        public String PackageCustomerID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PACKAGE_CUSTOMER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PACKAGE_CUSTOMER_ID", value);
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

        #region Customer
        public MBaseModel CustomerObj
        {
            get
            {
                cst.EntityID = CustomerId;
                cst.EntityName = CustomerName;
                cst.EntityCode = CustomerCode;

                return (cst);
            }

            set
            {
                if (value == null)
                {
                    CustomerId = "";
                    CustomerName = "";
                    CustomerCode = "";

                    return;
                }

                MEntity ii = (value as MEntity);
                cst.SetDbObject(ii.GetDbObject());

                CustomerId = ii.EntityID;
                CustomerName = ii.EntityName;
                CustomerCode = ii.EntityCode;

                updateFlag();
            }
        }

        public String CustomerId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENTITY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENTITY_ID", value);
            }
        }

        public String CustomerName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_NAME", value);
            }
        }

        public String CustomerCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_CODE", value);
            }
        }
        #endregion

        #region CustomerType
        public MBaseModel CustomerTypeObj
        {
            get
            {
                type.MasterID = CustomerTypeID;
                type.Code = CustomerTypeCode;
                type.Description = CustomerTypeName;

                return (type);
            }

            set
            {
                if (value == null)
                {
                    CustomerTypeID = "";
                    CustomerTypeCode = "";
                    CustomerTypeName = "";

                    return;
                }

                MMasterRef ii = (value as MMasterRef);
                type.SetDbObject(ii.GetDbObject());

                CustomerTypeCode = ii.Code;
                CustomerTypeName = ii.Description;
                CustomerTypeID = ii.MasterID;

                updateFlag();
            }
        }

        public String CustomerTypeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_TYPE", value);
            }
        }

        public String CustomerTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_TYPE_NAME", value);
            }
        }

        public String CustomerTypeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_TYPE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_TYPE_CODE", value);
            }
        }
        #endregion

        #region CustomerGroup

        public MBaseModel CustomerGroupObj
        {
            get
            {
                grp.MasterID = CustomerGroupID;
                grp.Code = CustomerGroupCode;
                grp.Description = CustomerGroupName;

                return (grp);
            }

            set
            {
                if (value == null)
                {
                    CustomerGroupID = "";
                    CustomerGroupCode = "";
                    CustomerGroupName = "";

                    return;
                }

                MMasterRef ii = (value as MMasterRef);
                grp.SetDbObject(ii.GetDbObject());

                CustomerGroupCode = ii.Code;
                CustomerGroupName = ii.Description;
                CustomerGroupID = ii.MasterID;

                updateFlag();
            }
        }

        public String CustomerGroupID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_GROUP", value);
            }
        }

        public String CustomerGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_GROUP_NAME", value);
            }
        }

        public String CustomerGroupCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CUSTOMER_GROUP_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CUSTOMER_GROUP_CODE", value);
            }
        }
        #endregion

        public Boolean IsItemEmpty
        {
            get
            {
                if (CustomerId.Equals("") && CustomerGroupID.Equals("") && CustomerTypeID.Equals(""))
                {
                    return (true);
                }

                return (false);
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
    }
}
