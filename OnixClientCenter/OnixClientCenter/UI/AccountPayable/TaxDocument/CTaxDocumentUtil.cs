using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Windows;
using Onix.Client.Controller;
using Onix.Client.Helper;
using Onix.Client.Model;
using Onix.ClientCenter.Commons.Utils;
using Wis.WsClientAPI;

namespace Onix.ClientCenter.UI.AccountPayable.TaxDocument
{
    class CTaxDocumentUtil
    {
        public static CTable ApproveChequeFromTaxDoc(MVTaxDocument doc)
        {
            CTable dat = new CTable("");

            Boolean approveCheque = CGlobalVariable.GetGlobalVariableValue("CHEQUE_APPROVE_IMMEDIATE_FLAG").Equals("Y");
            if (!approveCheque)
            {
                return (dat);
            }

            dat.SetFieldValue("CHEQUE_ID", doc.ChequeID);

            CTable cheque = OnixWebServiceAPI.SubmitObjectAPI("GetChequeInfo", dat);
            if (cheque != null)
            {
                cheque = OnixWebServiceAPI.SubmitObjectAPI("ApproveCheque", cheque);
            }

            return (cheque);
        }

        public static String FormatNumberField(String number, String ifZeroStr)
        {
            if (number.Equals(""))
            {
                return ("");
            }

            String tmp = FormatNumberField(number);

            return (tmp);
        }

        public static String FormatNumberField(String number)
        {
            if (number.Equals(""))
            {
                return ("");
            }

            String tmp = CUtil.FormatNumber(number, "");
            if (tmp.Contains("."))
            {
                tmp = tmp.Replace(".", " ");
            }

            return (tmp);
        }

        public static String FormatTaxIDField(String number)
        {
            String pat = "(.)(....)(.....)(..)(.)";

            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(number);

            if (!m.Success)
            {
                return (number);
            }

            String s1 = m.Groups[1].Value;
            String s2 = m.Groups[2].Value;
            String s3 = m.Groups[3].Value;
            String s4 = m.Groups[4].Value;
            String s5 = m.Groups[5].Value;

            String tmp = String.Format("{0} {1} {2} {3} {4}", s1, s2, s3, s4, s5);
            return (tmp);
        }

        public static String FormatTaxIDField2(String number)
        {
            String pat = "(.)(..)(.)(...)(.....)(.)";

            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(number);

            if (!m.Success)
            {
                return (number);
            }

            String s1 = m.Groups[1].Value;
            String s2 = m.Groups[2].Value;
            String s3 = m.Groups[3].Value;
            String s4 = m.Groups[4].Value;
            String s5 = m.Groups[5].Value;
            String s6 = m.Groups[6].Value;

            String tmp = String.Format("{0} {1} {2} {3} {4} {5}", s1, s2, s3, s4, s5, s6);
            return (tmp);
        }

        public static String[] GetNameLastname(String longName)
        {
            String pat = "(.+)\\s+(.+)";
            String[] s = new String[2];


            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(longName);

            if (!m.Success)
            {
                s[0] = longName;
                s[1] = "";

                return (s);
            }           

            s[0] = m.Groups[1].Value;
            s[1] = m.Groups[2].Value;

            return (s);
        }

        public static MAccountDoc TaxDocToAccountDoc(MVTaxDocument td, UIElement parent)
        {
            MAccountDoc ad = new MAccountDoc(new CTable(""));

            if (td.ChequeID.Equals(""))
            {
                String supplierCode = CGlobalVariable.GetGlobalVariableValue("REVENUE_DEP_SUPPLIER_CODE");
                if (supplierCode.Equals(""))
                {
                    CHelper.ShowErorMessage("REVENUE_DEP_SUPPLIER_CODE", "error_revenue_dep_code_missing", null);
                    return (null);
                }

                MEntity en = new MEntity(new CTable(""));
                en.EntityCode = supplierCode;
                en.Category = "2";

                CUtil.EnableForm(false, parent);
                ArrayList arr = OnixWebServiceAPI.GetListAPI("GetEntityList", "ENTITY_LIST", en.GetDbObject());
                CUtil.EnableForm(true, parent);

                if ((arr == null) || (arr.Count <= 0))
                {
                    CHelper.ShowErorMessage(supplierCode, "error_supplier_code_missing", null);
                    return (null);
                }

                CTable obj = (CTable)arr[0];
                en.SetDbObject(obj);

                ad.EntityObj = en;
                ad.EntityName = en.EntityName;
            }
            else
            {
                ad.ChequeID = td.ChequeID;
            }

            ad.IsPopulateChequeAmt = true;

            return (ad);
        }
    }
}
