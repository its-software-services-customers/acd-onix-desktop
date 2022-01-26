using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
    public class MLoginSession : MBaseModel
    {
        public MLoginSession(CTable obj) : base(obj)
		{
		}

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("user", UserName);
            ttItems.Add(ct);

            ct = new CToolTipItem("session", LoginSession);
            ttItems.Add(ct);
        }

        public String LoginId
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOGIN_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOGIN_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String LoginSession
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOGIN_SESSION"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOGIN_SESSION", value);
                NotifyPropertyChanged();
            }
        }

        public String UserName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("USER_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("USER_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String IPAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IP_ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("IP_ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String LoginDateTimeFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("LOGIN_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateStringTime(dt);

                return (str2);

            }

            set
            {
            }
        }

        public String Period
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("LOGIN_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                System.TimeSpan date = DateTime.Now.Subtract(dt);

                String str2 = "";
                if(date.Days > 0 )
                    str2 = date.Days + " " + CLanguage.getValue("day") + " ";
                if(date.Days > 0 || date.Hours > 0)
                    str2 = str2 + date.Hours + " " + CLanguage.getValue("hour") + " ";

                str2 = str2 + date.Minutes + " " + CLanguage.getValue("minute");

                return (str2);

            }

            set
            {
            }
        }
    }
}
