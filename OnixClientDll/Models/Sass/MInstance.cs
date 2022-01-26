using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model.Sass
{
    public class MInstance : MBaseModel
    {
        public MInstance(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("vm_name", InstanceName);
            ttItems.Add(ct);

            ct = new CToolTipItem("project_name", ProjectName);
            ttItems.Add(ct);

            ct = new CToolTipItem("colVmIP", IPAddress);
            ttItems.Add(ct);           
        }

        public void CreateDefaultValue()
        {
        }

        public String InstanceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INSTANCE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("INSTANCE_ID", value);
            }
        }

        public String InstanceName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("INSTANCE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("INSTANCE_NAME", value);
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
        
        public String MachineType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MACHINE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("MACHINE_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        public String DiskType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_TYPE", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String DiskImage
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DISK_IMAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DISK_IMAGE", value);
                NotifyPropertyChanged();
            }
        }

        public String Stage
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("STAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("STAGE", value);
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

                return (GetDbObject().GetFieldValue("INSTANCE_STATUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("INSTANCE_STATUS", value);
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

                return (GetDbObject().GetFieldValue("INSTANCE_STATUS"));
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

        #region Project

        public String ProjectName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_NAME", value);
                NotifyPropertyChanged();
            }
        }

        public String ProjectID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PROJECT_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PROJECT_ID", value);
                NotifyPropertyChanged();
            }
        }

        #endregion
    }
}
