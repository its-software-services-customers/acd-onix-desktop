using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model.Sass
{
    public class MMicroService : MBaseModel
    {
        public MMicroService(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("microservice_code", ServiceCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("microservice_name", ServiceName);
            ttItems.Add(ct);

            ct = new CToolTipItem("docker_url", DockerURL);
            ttItems.Add(ct);           
        }

        public void CreateDefaultValue()
        {
        }

        public String ServiceID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_ID", value);
            }
        }

        public String ServiceCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_CODE", value);
                NotifyPropertyChanged();
            }
        }

        public String ServiceName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SERVICE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("SERVICE_NAME", value);
                NotifyPropertyChanged();
            }
        }
        
        public String DockerURL
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCKER_URL"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCKER_URL", value);
                NotifyPropertyChanged();
            }
        }        
    }
}
