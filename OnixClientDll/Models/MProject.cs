using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MProject : MBaseModel
    {
        public MProject(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("project_code", ProjectCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("project_name", ProjectName);
            ttItems.Add(ct);

            ct = new CToolTipItem("project_description", ProjectDescription);
            ttItems.Add(ct);
        }

        public void CreateDefaultValue()
        {
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        private String constructProjectName()
        {
            String name = String.Format("{0}-{1}-{2}", Product, CBU, System);
            return (name);
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ProjectDescription
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_DESC", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region Project Group

        public String ProjectGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ProjectGroupCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_GROUP_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_GROUP_CODE", value);
                updateFlag();
                NotifyPropertyChanged();
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
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public MMasterRef ProjectGroupObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    ProjectGroup = m.MasterID;
                    ProjectGroupName = m.Description;
                    NotifyAllPropertiesChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = ProjectGroup;
                mr.Description = ProjectGroupName;

                return (mr);
            }
        }

        #endregion

        #region Product

        public MMasterRef ProductObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    Product = m.MasterID;
                    NotifyAllPropertiesChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MMasterRef mr = CUtil.MasterIDToObject(CMasterReference.Instance.Products, Product);
                //mr.MasterID = Product;
                //mr.Description = mr.MasterID;

                return (mr);
            }
        }

        public String Product
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRODUCT"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRODUCT", value);
                ProjectName = constructProjectName();
                ProjectCode = constructProjectName();
                updateFlag();
                NotifyPropertyChanged();
            }
        }
        #endregion Product

        public String CBU
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CBU"));
            }

            set
            {
                GetDbObject().SetFieldValue("CBU", value);
                ProjectName = constructProjectName();
                ProjectCode = constructProjectName();
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String System
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SYSTEM"));
            }

            set
            {
                GetDbObject().SetFieldValue("SYSTEM", value);
                ProjectName = constructProjectName();
                ProjectCode = constructProjectName();
                updateFlag();
                NotifyPropertyChanged();
            }
        }
    }


}
