using System;
using System.IO;
using System.Text;
using System.Xml;
using Wis.WsClientAPI;
using System.Security.Cryptography;
using System.Collections;
using System.Reflection;
using Onix.Client.Controller;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.Utils
{
    public static class CConfig
    {
        private static OnixConnectionXML db = new OnixConnectionXML("", "", "");
        private static CTable tbobj = new CTable("CONFIG");
        private static CTable param = new CTable("PARAM");
        private static String cfgName = "onix.xml";

        private const string initVector = "pemgail9uzpgzl88";
        private const string encryptKey = "HelloWorld1234SeubpongMonsar";

        // This constant is used to determine the keysize of the encryption algorithm
        private const int keysize = 256;
        private static Hashtable variables = new Hashtable();
        private static Hashtable svr_variables = new Hashtable();

        //Encrypt
        public static string EncryptString(string plainText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherTextBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(cipherTextBytes);
        }

        //Decrypt
        public static string DecryptString(string cipherText, string passPhrase)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        public static CTable ConfigRead()
        {
            try
            {
                String xml = File.ReadAllText(cfgName);
                CRoot root = db.XMLToRootObject(xml);
                
                APIResult rs = new APIResult(root, xml, "");

                tbobj = rs.GetResultObject();
                param = rs.GetParamObject();
            }
            catch
            {
                //File not found
                SetUrl("http://wiscon.cloudhost.in.th:88/dbos/dev/mps/mps_pgsql/cgi-bin/dbos_db_api_dispatcher.pl");
                SetKey("1234");
                SetTheme("2");
            }

            return (tbobj);    
        }

        private static String printXML(String XML)
        {
            String Result = "";

            MemoryStream mStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                // Load the XmlDocument with the XML.
                document.LoadXml(XML);

                writer.Formatting = Formatting.Indented;

                // Write the XML into a formatting XmlTextWriter
                document.WriteContentTo(writer);
                writer.Flush();
                mStream.Flush();

                // Have to rewind the MemoryStream in order to read
                // its contents.
                mStream.Position = 0;

                // Read MemoryStream contents into a StreamReader.
                StreamReader sReader = new StreamReader(mStream);

                // Extract the text from the StreamReader.
                String FormattedXML = sReader.ReadToEnd();

                Result = FormattedXML;
            }
            catch (XmlException)
            {
            }

            mStream.Close();
            writer.Close();

            return Result;
        }

        public static void ConfigWrite()
        {
            CRoot root = new CRoot(param, tbobj);
            String xml = db.CreateXMLString(root);
            String formatedXml = printXML(xml);

            using (StreamWriter writetext = new StreamWriter(cfgName))
            {
                writetext.WriteLine(formatedXml);
            }
        }

        public static String GetLanguage()
        {
            String lng = tbobj.GetFieldValue("LANGUAGE");
            if (lng.Equals(""))
            {
                lng = "TH";
            }

            return (lng);
        }

        public static String GetTheme()
        {
            String theme = tbobj.GetFieldValue("THEME");
            return (theme);
        }

        public static String GetUrl()
        {
            String url = tbobj.GetFieldValue("URL");
            return (url);
        }

        public static String GetProduct()
        {
            //"onix" and "lotto" and "sass"

            return (CBuild.Product);
        }

        public static String GetKey()
        {
            String plainText = "";
            try
            {
                String key = tbobj.GetFieldValue("KEY");
                plainText = CConfig.DecryptString(key, encryptKey);
            }
            catch
            {
                //Key has been changed manually in the config file
                plainText = "";
            }

            return (plainText);
        }

        public static void SetUrl(String url)
        {
            tbobj.SetFieldValue("URL", url);
        }

        public static void SetKey(String key)
        {
            String encrypted = CConfig.EncryptString(key, encryptKey);
            tbobj.SetFieldValue("KEY", encrypted);
        }

        public static void SetTheme(String theme)
        {
            tbobj.SetFieldValue("THEME", theme);
        }

        public static void SetLanguage(String lang)
        {
            tbobj.SetFieldValue("LANGUAGE", lang);
        }

        public static void AddParam(String key, String value)
        {
            variables[key] = value;
        }

        public static String GetParamValue(String key)
        {
            String s = (String)variables[key];

            if (s == null)
            {
                return ("");
            }

            return (s);
        }

        public static CTable CreateUserVariables(CTable u)
        {
            ArrayList original = u.GetChildArray("USER_VARIABLE_ITEM");
            Hashtable hs = CUtil.CTableArrayToHash(original, "VARIABLE_NAME");

            CTable t = new CTable("USER");
            ArrayList arr = new ArrayList();

            t.SetFieldValue("USER_ID", OnixWebServiceAPI.UserID().ToString());

            foreach (String key in variables.Keys)
            {
                CTable orig = null;
                String value = (String)variables[key];

                //if (key.Contains("REPORT_CASH_IN-TO_ACCOUNT_NO"))
                //{
                //    String s = key;
                //}

                if (hs.ContainsKey(key))
                {
                    orig = (CTable)hs[key];

                    String orgValue = orig.GetFieldValue("VARIABLE_VALUE");
                    if (!value.Equals(orgValue))
                    {
                        orig.SetFieldValue("EXT_FLAG", "E");
                    }
                }
                else
                {
                    orig = new CTable("");
                    orig.SetFieldValue("EXT_FLAG", "A");
                }
                
                orig.SetFieldValue("VARIABLE_NAME", key);
                orig.SetFieldValue("VARIABLE_VALUE", value);
                arr.Add(orig);
            }

            t.AddChildArray("USER_VARIABLE_ITEM", arr);
            return (t);
        }

        public static void LoadLastValueSaved(CTable o)
        {
            ArrayList arr = o.GetChildArray("USER_VARIABLE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                String n = t.GetFieldValue("VARIABLE_NAME");
                String value = t.GetFieldValue("VARIABLE_VALUE");

                variables[n] = value;
            }
        }

        public static void LoadServerVariable(CTable o)
        {
            ArrayList arr = o.GetChildArray("SERVER_VARIABLE_ITEM");

            if (arr == null)
            {
                return;
            }

            foreach (CTable t in arr)
            {
                String n = t.GetFieldValue("VARIABLE_NAME");
                String value = t.GetFieldValue("VARIABLE_VALUE");

                svr_variables[n] = value;
            }
        }

        public static String Version
        {
            get
            {
                AssemblyName asm = Assembly.GetExecutingAssembly().GetName();
                String version = String.Format("{0}-{1}.{2}", CBuild.BuildVersion, asm.Version.Major, asm.Version.Minor);

                return (version);
            }
        }

        public static String APIVersion
        {
            get
            {
                String v = OnixWebServiceAPI.WsApiVersion;
                if (v == null)
                {
                    return ("");
                }

                return (v);
            }
        }
    }
}
