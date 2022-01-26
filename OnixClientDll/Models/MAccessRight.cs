using Onix.Client.Helper;
using System;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MAccessRight : MBaseModel
    {
        public MAccessRight(CTable obj) : base(obj)
        {
        }

        public String AcccessRightID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCESS_RIGHT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCESS_RIGHT_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String AccessRight
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCESS_RIGHT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCESS_RIGHT_CODE", value);
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

                return (GetDbObject().GetFieldValue("RIGHT_DESCRIPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("RIGHT_DESCRIPTION", value);
                NotifyPropertyChanged();
            }
        }

        public String GroupID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

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
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String GroupDesc
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

        public String EnabledFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IS_ENABLE"));
            }

            set
            {
                GetDbObject().SetFieldValue("IS_ENABLE", value);
                NotifyPropertyChanged();
            }
        }

        public Boolean IsEnable
        {
            get
            {
                return (EnabledFlag.Equals("Y"));
            }

            set
            {
                EnabledFlag = value ? "Y":"N";
            }
        }

        public Boolean? IsEnableQuery
        {
            get
            {
                if (EnabledFlag.Equals(""))
                {
                    return (null);
                }

                return (EnabledFlag.Equals("Y"));
            }

            set
            {
                if (value == null)
                {
                    EnabledFlag = "";
                    return;
                }

                Boolean flag = (Boolean) value;
                EnabledFlag = flag ? "Y" : "N";
            }
        }

        public DateTime LastUpdateDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("LAST_UPDATE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);
                GetDbObject().SetFieldValue("LAST_UPDATE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public DateTime ExpireDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("EXPIRE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);
                GetDbObject().SetFieldValue("EXPIRE_DATE", str);
                NotifyPropertyChanged();
            }
        }
    }
}
