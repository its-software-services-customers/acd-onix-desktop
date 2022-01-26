using System;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Controller;

namespace Onix.Client.Helper
{
    public static class CGlobalVariable
    {
        private static ArrayList globalvariables = new ArrayList();

        public static void InitGlobalVariables()
        {
            globalvariables.Clear();
            CTable t = new CTable("");
            t = OnixWebServiceAPI.GetGlobalVariableInfo(t);
            ArrayList arr = t.GetChildArray("GLOBAL_VARIABLE_ITEM");
            foreach (CTable v in arr)
            {
                MGlobalVariable m = new MGlobalVariable(v);
                globalvariables.Add(m);
            }
        }

        public static String GetGlobalVariableValue(String name)
        {
            foreach (MGlobalVariable m in globalvariables)
            {
                if (m.VariableName.Equals(name))
                {
                    return (m.VariableValue);
                }
            }

            return ("");
        }

        public static Boolean IsInventoryNegativeAllow()
        {
            String str = GetGlobalVariableValue("ALLOW_NEGATIVE_STRING");
            String flag = str.Substring(0, 1);

            return (flag.Equals("Y"));
        }

        public static Boolean IsArApNegativeAllow()
        {
            String str = GetGlobalVariableValue("ALLOW_NEGATIVE_STRING");
            String flag = str.Substring(1, 1);

            return (flag.Equals("Y"));
        }

        public static Boolean IsCashNegativeAllow()
        {
            String str = GetGlobalVariableValue("ALLOW_NEGATIVE_STRING");
            String flag = str.Substring(2, 1);

            return (flag.Equals("Y"));
        }

        public static void UpdateGlobalVariable(String name, String value)
        {
            foreach (MGlobalVariable m in globalvariables)
            {
                if (m.VariableName.Equals(name))
                {
                    m.VariableValue = value;
                    m.ExtFlag = "E";

                    return;
                }
            }

            MGlobalVariable v = new MGlobalVariable(new CTable(""));
            v.ExtFlag = "A";
            v.VariableName = name;
            v.VariableValue = value;

            globalvariables.Add(v);
        }

        public static CTable GetCTableObject()
        {
            CTable n = new CTable("");

            ArrayList arr = new ArrayList();
            foreach (MGlobalVariable m in globalvariables)
            {
                //m.UserID = id;
                //m.UserName = name;

                arr.Add(m.GetDbObject());
            }

            n.AddChildArray("GLOBAL_VARIABLE_ITEM", arr);
            return (n);
        }
    }
}
