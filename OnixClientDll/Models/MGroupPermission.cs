using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MGroupPermission : MBaseModel
    {
        public MGroupPermission(CTable obj) : base(obj)
        {
        }

        public String PermissionName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String nm = GetDbObject().GetFieldValue("PERM_NAME");                
                return (nm.Replace("_", "__"));
            }

            set
            {
            }
        }

        public String Description
        {
            get
            {
                return ("");
            }

            set
            {
            }
        }

        public Boolean IsAllowed
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ALLOWED");                
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("IS_ALLOWED", flag);

                NotifyPropertyChanged("IsAllowedIcon");
            }
        }

        public String IsAllowedIcon
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("IS_ALLOWED");
                if (flag.Equals("Y"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/bullet-green-icon-16.png");
                }

                return ("pack://application:,,,/OnixClient;component/Images/bullet-red-icon-16.png");
            }

            set
            {
            }
        }
    }
}
