using System;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Collections.ObjectModel;
using System.Collections;
using Onix.Client.Controller;

namespace Onix.Client.Model
{
    public class MCompanyProfile : MBaseModel
    {
        private ArrayList companyImages = new ArrayList();

        public MCompanyProfile(CTable obj) : base(obj)
        {

        }

        public void CreateDefaultValue()
        {
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

        #region Name Prefix

        public String NamePrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREFIX_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREFIX_ID", value);
            }
        }

        public MMasterRef NamePrefixObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    NamePrefix = m.MasterID;
                    NamePrefixDesc = m.Description;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.NamePrefixes;
                if (items == null)
                {
                    return (null);
                }

                String np = NamePrefix;
                return (CUtil.MasterIDToObject(items, np));
            }
        }

        public String NamePrefixDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREFIX_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREFIX_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String NamePrefixDescEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PREFIX_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("PREFIX_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }
        #endregion


        public String CompanyCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String TaxID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TAX_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("TAX_ID", value);
                NotifyPropertyChanged();
            }
        }

        public String CompanyNameThai
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_NAME_THAI"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_NAME_THAI", value);
                NotifyPropertyChanged();
            }
        }

        public String CompanyNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMPANY_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMPANY_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String OperatorNameThai
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OPERATOR_NAME_THAI"));
            }

            set
            {
                GetDbObject().SetFieldValue("OPERATOR_NAME_THAI", value);
                NotifyPropertyChanged();
            }
        }

        public String OperatorNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OPERATOR_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("OPERATOR_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String Address
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String AddressEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADDRESS_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADDRESS_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String Telephone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TELEPHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TELEPHONE", value);
                NotifyPropertyChanged();
            }
        }

        public String Fax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("FAX", value);
                NotifyPropertyChanged();
            }
        }

        public String Email
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMAIL"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMAIL", value);
                NotifyPropertyChanged();
            }
        }

        public String Website
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WEBSITE"));
            }

            set
            {
                GetDbObject().SetFieldValue("WEBSITE", value);
                NotifyPropertyChanged();
            }
        }

        public String Logo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LOGO"));
            }

            set
            {
                GetDbObject().SetFieldValue("LOGO", value);
                NotifyPropertyChanged();
            }
        }

        #region Company Image

        public void InitCompanyImage()
        {
            companyImages.Clear();

            CTable o = GetDbObject();
            if (o == null)
            {
                return;
            }

            ArrayList arr = o.GetChildArray("COMPANY_IMAGES");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                MCompanyImage v = new MCompanyImage(t);
                companyImages.Add(v);
                if (!v.ExtFlag.Equals("A"))
                {
                    v.ExtFlag = "I";
                }
            }
        }

        public String LogoImageFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (companyImages.Count <= 0)
                {
                    return ("");
                }

                MCompanyImage img = (MCompanyImage)companyImages[0];
                return (img.ImageFlag);
            }

            set
            {
                if (companyImages.Count <= 0)
                {
                    return;
                }

                MCompanyImage img = (MCompanyImage)companyImages[0];
                img.ImageFlag = value;

                NotifyPropertyChanged();
            }
        }

        public String LogoImageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (companyImages.Count <= 0)
                {
                    return ("");
                }

                MCompanyImage img = (MCompanyImage) companyImages[0];
                return (img.ImageNameWip);
            }

            set
            {
                if (companyImages.Count <= 0)
                {
                    return;
                }

                MCompanyImage img = (MCompanyImage)companyImages[0];
                img.ImageNameWip = value;

                NotifyPropertyChanged();
            }
        }

        public String LogoImageUrl
        {
            get
            {
                String uri = OnixWebServiceAPI.GetUploadedFileUrl(LogoImageName, "storage");
                return (uri);
            }
        }

        #endregion

        #region TaxForm

        public String RegistrationName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REGISTRATION_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("REGISTRATION_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String RegistrationAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REGISTRATION_ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("REGISTRATION_ADDRESS", value);
                NotifyPropertyChanged();
            }
        }

        public String BuildingName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BUILDING_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BUILDING_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String RoomNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ROOM_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ROOM_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String FloorNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FLOOR_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("FLOOR_NO", value);
                NotifyPropertyChanged();
            }
        }


        public String VillageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("VILLAGE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("VILLAGE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String HomeNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("HOME_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("HOME_NO", value);
                NotifyPropertyChanged();
            }
        }

        public String Moo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MOO"));
            }

            set
            {
                GetDbObject().SetFieldValue("MOO", value);
                NotifyPropertyChanged();
            }
        }

        public String Soi
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SOI"));
            }

            set
            {
                GetDbObject().SetFieldValue("SOI", value);
                NotifyPropertyChanged();
            }
        }

        public String Road
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ROAD"));
            }

            set
            {
                GetDbObject().SetFieldValue("ROAD", value);
                NotifyPropertyChanged();
            }
        }

        public String District
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISTRICT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISTRICT", value);
                NotifyPropertyChanged();
            }
        }

        public String Town
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOWN"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOWN", value);
                NotifyPropertyChanged();
            }
        }

        public String Province
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROVINCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROVINCE", value);
                NotifyPropertyChanged();
            }
        }

        public String Zip
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ZIP"));
            }

            set
            {
                GetDbObject().SetFieldValue("ZIP", value);
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
