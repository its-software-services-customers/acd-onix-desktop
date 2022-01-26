using System;
using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Onix.Client.Helper
{
    public static class CLanguage
    {
        private static String lang = "En";
        private static ResourceManager resman = null;
        private static ResourceManager resTh = null;
        private static ResourceManager resEn = null;

        public static void setLanguage(String lng)
        {
            lang = lng;

            resTh = new ResourceManager("Onix.Client.Resources.Strings", Assembly.GetExecutingAssembly());
            resEn = new ResourceManager("Onix.Client.Resources.Strings_EN", Assembly.GetExecutingAssembly());

            if (lng.Equals("EN"))
            {
                resman = resEn;
            }
            else if (lng.Equals("TH"))
            {
                resman = resTh;
            }            
        }

        public static String getValue(String lng, String key)
        {
            ResourceManager res = null;

            if (resman == null)
            {
                return ("");
            }

            if (lng.Equals("EN"))
            {
                res = resEn;
            }
            else if (lng.Equals("TH"))
            {
                res = resTh;
            }

            String tmp = res.GetString(key);
            if (tmp == null)
            {
                return (key);
            }

            return (tmp);
        }

        public static String getValue(String key)
        {
            if (resman == null)
            {
                return ("");
            }

            String tmp = resman.GetString(key);
            if (tmp == null)
            {
                return (key);
            }

            return (tmp);
        }

        public static String getValueEx([CallerMemberName] string key = "")
        {
            if (resman == null)
            {
                return (key);
            }

            String tmp = resman.GetString(key);
            if (tmp == null)
            {
                return (key);
            }

            return (tmp);
        }

    }
}
