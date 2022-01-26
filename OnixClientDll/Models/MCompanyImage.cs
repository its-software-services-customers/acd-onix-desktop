using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MCompanyImage : MBaseModel
    {
        public MCompanyImage(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public String CompanyImageID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_IMAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_IMAGE_ID", value);
            }
        }

        public String CompanyID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_ID", value);
            }
        }

        public String ImageType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE_TYPE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ImageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE_NAME", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ImageFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE_EXT_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE_EXT_FLAG", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ImageNameWip
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE_NAME_WIP"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE_NAME_WIP", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String ImageUrl
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE_URL"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE_URL", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }        
    }


}
