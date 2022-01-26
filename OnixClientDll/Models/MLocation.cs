using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MLocation : MBaseModel
    {
        public MLocation(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("location_code", LocationCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("location_name", Description);
            ttItems.Add(ct);

            ct = new CToolTipItem("location_type", LocationTypeName);
            ttItems.Add(ct);
           
        }


        public void CreateDefaultValue()
        {
            IsForSale = false;
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
            }
        }

        public String LocationCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOCATION_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String Description
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);
                NotifyPropertyChanged();
            }
        }

        public MMasterRef LocationTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m == null)
                {
                    return;
                }

                LocationType = m.MasterID;
                LocationTypeName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = LocationType;
                mr.Description = LocationTypeName;

                return (mr);
            }
        }

        public String LocationType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("LOCATION_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOCATION_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String LocationTypeName
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
                NotifyPropertyChanged();
            }
        }

        public String IsForSaleIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("SALE_FLAG");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        #region Branch
        public String BranchID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String BranchCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        public MMasterRef BranchObj
        {
            set
            {
                if (value == null)
                {
                    return;
                }

                MMasterRef m = value as MMasterRef;
                BranchID = m.MasterID;
                BranchCode = m.Code;
                BranchName = m.Description;
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> branches = CMasterReference.Instance.Branches;
                if (branches == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(branches, BranchID));
            }
        }
        #endregion

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
    }
}
