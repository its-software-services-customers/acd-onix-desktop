using System;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Controller;
using Onix.Client.Model;

namespace Onix.Client.Helper
{
    public static class CReportConfigs
    {
        public static MReportConfig GetReportConfig(GetInfoFunction getInfoFunc, String reportKey)
        {
            CTable o = new CTable("");
            o.SetFieldValue("REPORT_NAME", reportKey);

            ArrayList arr = OnixWebServiceAPI.GetReportConfigList(o);
            if (arr.Count <= 0)
            {
                return (null);
            }

            CTable obj = (CTable) arr[0];

            CTable info = OnixWebServiceAPI.GetReportConfigInfo(obj);

            MReportConfig m = new MReportConfig(info);
            m.InitReportConfig();

            return (m);
        }

        public static Boolean SaveReportConfig(UpdateFunction saveFunc, MReportConfig rptConfig)
        {
            rptConfig.PrepareForSaving();
            CTable o = OnixWebServiceAPI.SaveReportConfig(rptConfig.GetDbObject());
            return (true);
        }
    }
}
