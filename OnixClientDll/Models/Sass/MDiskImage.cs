using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model.Sass
{
    public class MDiskImage : MBaseModel
    {
        public MDiskImage(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("disk_image_name", DiskImageName);
            ttItems.Add(ct);

            ct = new CToolTipItem("description", DiskImageDescription);
            ttItems.Add(ct);  
        }

        public void CreateDefaultValue()
        {
        }

        public String DiskImageID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_IMAGE_ID", value);
            }
        }

        public String DiskImageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_IMAGE_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String DiskImageDescription
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_IMAGE_DESC", value);
                NotifyPropertyChanged();
            }
        }       

        #region Status
        public String Status
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_IMAGE_STATUS", value);
                NotifyPropertyChanged();
            }
        }

        public String StatusName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE_STATUS"));
            }

            set
            {
            }
        }
        #endregion

        #region Role
        public String Role
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INSTANCE_ROLE"));
            }

            set
            {
                GetDbObject().SetFieldValue("INSTANCE_ROLE", value);
                NotifyPropertyChanged();
            }
        }

        public String RoleName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INSTANCE_ROLE"));
            }

            set
            {
            }
        }
        #endregion Role

    }
}
