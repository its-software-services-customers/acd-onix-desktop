using System;
using System.Windows;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;

namespace Onix.Client.Models
{
    public class MCommissionProfileDisplay : MBaseModel
    {
        private int seq = 0;
        public MCommissionProfileDisplay(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            //ct = new CToolTipItem("cycle_description", CycleDescription);
            //ttItems.Add(ct);
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                seq = value;
            }
        }

        public String CommProfileID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_PROF_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_PROF_ID", value);
            }
        }

        public String ProfileCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROFILE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ProfileDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROFILE_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String Commission
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISION"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISION", value);
                NotifyPropertyChanged();
            }
        }

        public String CommissionFmt
        {
            get
            {
                return (CUtil.FormatNumber(Commission.ToString()));
            }
        }

        #region Commission Pfofile Type
        public MMasterRef ProfileTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                ProfileType = m.MasterID;
                ProfileTypeName = m.Description;
            }
        }

        public String ProfileType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("0");
                }

                return (GetDbObject().GetFieldValue("PROFILE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROFILE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String ProfileTypeName
        {
            get
            {
                if (ProfileType.Equals(""))
                {
                    return ("");
                }

                String str = CUtil.CommissionProfileToString(ProfileType);

                return (str);
            }

            set
            {

            }
        }

        public String ProfileTypeIcon
        {
            get
            {
                if (ProfileType.Equals("1"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/standard-package-icon.png");
                }
                else if (ProfileType.Equals("2"))
                {
                    return ("pack://application:,,,/OnixClient;component/Images/gift-icon.png");
                }
                return ("pack://application:,,,/OnixClient;component/Images/coupon-icon.png");
            }
        }
        #endregion
    }
}
