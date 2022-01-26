using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MUserView : MBaseModel
    {
        public MUserView(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("user", UserName);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", Description);
            ttItems.Add(ct);

            ct = new CToolTipItem("user_group", GroupName);
            ttItems.Add(ct);
        }

        public String UserID
        {
            get
            {
                return (GetDbObject().GetFieldValue("USER_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("USER_ID", value.Trim());
                NotifyPropertyChanged();
            }
        }

        public String UserName
        {
            get
            {
                return (GetDbObject().GetFieldValue("USER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("USER_NAME", value.Trim());
                NotifyPropertyChanged();
            }
        }

        public String Description
        {
            get
            {
                return (GetDbObject().GetFieldValue("DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("DESCRIPTION", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean? IsAdminCriteria
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ADMIN");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == true)
                {
                    flag = "Y";
                }
                else if (value == null)
                {
                    flag = "";
                }

                GetDbObject().SetFieldValue("IS_ADMIN", flag);
            }
        }

        public Boolean? IsActiveCriteria
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ENABLE");
                if (flag.Equals(""))
                {
                    return (null);
                }

                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == true)
                {
                    flag = "Y";
                }
                else if (value == null)
                {
                    flag = "";
                }

                GetDbObject().SetFieldValue("IS_ENABLE", flag);
            }
        }

        public Boolean IsAdmin
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ADMIN");
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("IS_ADMIN", flag);
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsAdminIcon");
            }
        }

        public Boolean IsActive
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ENABLE");
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("IS_ENABLE", flag);
                NotifyPropertyChanged();
                NotifyPropertyChanged("IsActiveIcon");
            }
        }

        public String Password
        {
            get
            {
                return (GetDbObject().GetFieldValue("PASSWORD"));
            }

            set
            {
                GetDbObject().SetFieldValue("PASSWORD", value);
                NotifyPropertyChanged();
            }
        }

        public String NewPassword
        {
            get
            {
                return (GetDbObject().GetFieldValue("NEW_PASSWORD"));
            }

            set
            {
                GetDbObject().SetFieldValue("NEW_PASSWORD", value);
                NotifyPropertyChanged();
            }
        }

        public String IsAdminIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ADMIN");
                if (flag.Equals("Y"))
                {

                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        public String IsActiveIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ENABLE");
                if (flag.Equals("Y"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
            }
        }

        #region UserGroup

        public String GroupID
        {
            get
            {
                return (GetDbObject().GetFieldValue("GROUP_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String GroupName
        {
            get
            {
                return (GetDbObject().GetFieldValue("GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public MUserGroup GroupObj
        {
            set
            {
                MUserGroup m = value as MUserGroup;

                if (m != null)
                {
                    GroupID = m.GroupID;
                    GroupName = m.GroupName;
                }
                else
                {
                    GroupID = "";
                    GroupName = "";
                }
            }

            get
            {
                MUserGroup mr = (MUserGroup) CUtil.IDToObject(CMasterReference.Instance.UserGroups, "GroupID", GroupID);
                return (mr);
            }
        }


        #endregion
    }
}
