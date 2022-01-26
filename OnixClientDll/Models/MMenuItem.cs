using System;
using System.Windows;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MMenuItem : MBaseModel
    {
        private RoutedEventHandler evt;
        private Boolean isAdmin = false;
        
        public MMenuItem(CTable obj) : base(obj)
        {
        }

        public String MenuName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MENU_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("MENU_NAME", value);
            }
        }

        public String ClassName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CLASS_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("CLASS_NAME", value);
            }
        }

        public String AccessRightName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RIGHT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("RIGHT_NAME", value);
            }
        }

        public String Image
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("IMAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("IMAGE", value);
            }
        }

        public String Group
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("GROUP", value);
            }
        }

        public Boolean IsOnlyAdmin
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (isAdmin);
            }

            set
            {
                isAdmin = value;
            }
        }

        public RoutedEventHandler MenuEvent
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                return (evt);
            }

            set
            {
                evt = value;
            }
        }

        public String Caption
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CAPTION"));
            }

            set
            {
                GetDbObject().SetFieldValue("CAPTION", value);
            }
        }

        public String ProductHopper
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PRODUCT_HOPPER"));
            }

            set
            {
                GetDbObject().SetFieldValue("PRODUCT_HOPPER", value);
            }
        }
    }
}
