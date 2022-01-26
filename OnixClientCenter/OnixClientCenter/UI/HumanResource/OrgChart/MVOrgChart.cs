using System;
using System.Collections.ObjectModel;
using Onix.Client.Helper;
using Onix.Client.Model;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.HumanResource.OrgChart
{
    public class MVOrgChart : MBaseModel
    {        
        public MVOrgChart(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("name", DirectoryName);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", Description);
            ttItems.Add(ct);
        }


        public String DirectoryName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DIRECTORY_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("DIRECTORY_NAME", value);
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

                return (GetDbObject().GetFieldValue("DIRECTORY_DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DIRECTORY_DESCRIPTION", value);
                NotifyPropertyChanged();
            }
        }

        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
                NotifyPropertyChanged();
            }
        }

        public String DirectoryID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DIRECTORY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DIRECTORY_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String ParentDirectoryID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PARENT_DIRECTORY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PARENT_DIRECTORY_ID", value);
                NotifyPropertyChanged();
            }
        }        
    }
}
