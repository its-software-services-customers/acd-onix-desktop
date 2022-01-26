using System;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Controller;

namespace Onix.Client.Helper
{
    public static class CAccessValidator
    {
        private static Hashtable accessRights = new Hashtable();
        private static double refreshIntervalMinute = 10;

        private static MAccessRight retrieveAccessRight(String accessRightMode)
        {
            CTable data = new CTable("");
            data.SetFieldValue("ACCESS_RIGHT_CODE", accessRightMode);

            CTable acr = OnixWebServiceAPI.SubmitObjectAPI("GetUserContextAccessRight", data);
            MAccessRight m = null;

            if (acr == null)
            {
                acr = new CTable("");
                m = new MAccessRight(acr);

                //Next time will call API immediately
                m.LastUpdateDate = DateTime.Now;
                m.ExpireDate = m.LastUpdateDate;

                //If permission has not been added then allow (I don't want to annoy user if permission is not yet added)
                m.IsEnable = true;
            }
            else
            {
                m = new MAccessRight(acr);
                m.LastUpdateDate = DateTime.Now;
                //Refresh from API in every 5 minutes
                m.ExpireDate = m.LastUpdateDate.AddMinutes(refreshIntervalMinute);
            }

            return (m);
        }

        private static MAccessRight getAccessRight(String accessRightMode)
        {
            MAccessRight acr = (MAccessRight) accessRights[accessRightMode];
            if (acr == null)
            {
                acr = retrieveAccessRight(accessRightMode);
            }
            else
            {
                if (acr.ExpireDate < DateTime.Now)
                {
                    //Expire
                    acr = retrieveAccessRight(accessRightMode);
                }
            }

            accessRights[accessRightMode] = acr;

            return (acr);
        }

        public static Boolean VerifyAccessRight(String accessRightMode)
        {
            MAccessRight acr = getAccessRight(accessRightMode);

            return (acr.IsEnable);
        }

    }
}
