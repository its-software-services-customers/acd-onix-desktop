using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;

namespace Onix.Client.Model
{
	public class MLoginHistory : MBaseModel
	{
        public MLoginHistory(CTable obj) : base(obj)
		{
		}

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("user", UserName);
            ttItems.Add(ct);

            ct = new CToolTipItem("error_desc", ErrorDesc);
            ttItems.Add(ct);

            ct = new CToolTipItem("login_date_time", LoginDateTimeFmt);
            ttItems.Add(ct);
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

        public String LogoutDateTimeFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("LOGOUT_DATE");
                if (str.Equals(""))
                {
                    return ("");
                }

                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateStringTime(dt);

                return (str2);
            }

            set
            {
            }
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

		
		public String ErrorDesc
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("ERROR_DESC"));
			}

			set
			{
				GetDbObject().SetFieldValue("ERROR_DESC", value);
				NotifyPropertyChanged();
			}
		}

        public String Session
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SESSION"));
            }

            set
            {
                GetDbObject().SetFieldValue("SESSION", value);
                NotifyPropertyChanged();
            }
        }

		public DateTime? FromDocumentDate
		{
			set
			{
				String str = CUtil.DateTimeToDateStringInternalMin(value);

				GetDbObject().SetFieldValue("FROM_LOGIN_DATE", str);
			}
		}

		public DateTime? ToDocumentDate
		{
			set
			{
				String str = CUtil.DateTimeToDateStringInternalMax(value);

				GetDbObject().SetFieldValue("TO_LOGIN_DATE", str);
			}
		}


		public Boolean? IsLoginSuccess
		{
			get
			{
				String flag = GetDbObject().GetFieldValue("LOGIN_SUCCESS");
                if (flag.Equals(""))
                {
                    return (null);
                }

				return (flag.Equals("Y"));
			}

			set
			{
				String flag = "";
				if (value == true)
				{
					flag = "Y";
				}
                else if (value == false)
                {
                    flag = "N";
                }

				GetDbObject().SetFieldValue("LOGIN_SUCCESS", flag);

				NotifyPropertyChanged("IsSuccessIcon");
			}
		}

        public Boolean IsEditable
        {
            get
            {
                return (false);
            }

            set
            {
            }
        }

		public String IsSuccessIcon
		{
			get
			{
				String flag = GetDbObject().GetFieldValue("LOGIN_SUCCESS");
				if (flag.Equals("Y"))
				{

					return ("pack://application:,,,/OnixClient;component/Images/yes-icon-16.png");
				}

				return ("pack://application:,,,/OnixClient;component/Images/no-icon-16.png");
			}
		}
	}
}
