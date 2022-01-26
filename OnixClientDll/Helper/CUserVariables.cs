using System;
using System.Collections;
using Wis.WsClientAPI;
using Onix.Client.Model;
using Onix.Client.Models;

namespace Onix.Client.Helper    
{
    public static class CUserVariables
    {
        private static ArrayList variables = new ArrayList();

        public static void InitVariables(CTable t)
        {
            variables.Clear();

            ArrayList arr = t.GetChildArray("USER_VARIABLE_ITEM");
            foreach (CTable v in arr)
            {
                MUserVariable m = new MUserVariable(v);
                variables.Add(m);
            }
        }

        public static String GetVariableValue(String name)
        {
            foreach (MUserVariable m in variables)
            {
                if (m.VariableName.Equals(name))
                {
                    return(m.VariableValue);
                }
            }

            return ("");
        }

        public static void UpdateVariable(String name, String value)
        {
            foreach (MUserVariable m in variables)
            {
                if (m.VariableName.Equals(name))
                {
                    m.VariableValue = value;
                    m.ExtFlag = "E";

                    return;
                }
            }

            MUserVariable v = new MUserVariable(new CTable(""));
            v.ExtFlag = "A";
            v.VariableName = name;
            v.VariableValue = value;

            variables.Add(v);
        }

        public static CTable GetCTableObject(String id, String name)
        {
            CTable n = new CTable("");

            ArrayList arr = new ArrayList();
            foreach (MUserVariable m in variables)
            {
                m.UserID = id;
                m.UserName = name;

                arr.Add(m.GetDbObject());
            }

            n.AddChildArray("USER_VARIABLE_ITEM", arr);
            return (n);
        }
    }
}
