using System;

namespace Onix.Client.Helper
{
    class CBahtText
    {
        private static string s1 = "";
        private static string s2 = "";
        private static string s3 = "";
        private static string[] suffix = { "", "", "สิบ", "ร้อย", "พัน", "หมื่น", "แสน", "ล้าน" };
        private static string[] numSpeak = { "", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "แปด", "เก้า" };

        public static string ToBahtText(double m)
        {
            string result;

            if (m == 0) return ("");

            splitCurr(m);
            result = "";
            if (s1.Length > 0)
            {
                result = result + Speak(s1) + "ล้าน";
            }

            if ((s2.Length > 0) || (s1.Length > 0))
            {
                result = result + Speak(s2) + "บาท";
            }

            if (s3.Length > 0)
            {
                result = result + speakStang(s3) + "สตางค์";
            }
            else
            {
                result = result + "ถ้วน";
            }

            return (result);
        }

        private static string Speak(string s)
        {
            int L, c;
            string result;

            if (s == "") return ("");

            result = "";
            L = s.Length;
            for (int i = 0; i < L; i++)
            {
                if ((s.Substring(i, 1) == "-"))
                {
                    result = result + "ติดลบ";
                }
                else
                {
                    c = System.Convert.ToInt32(s.Substring(i, 1));
                    if ((i == L - 1) && (c == 1))
                    {
                        if (L == 1)
                        {
                            return ("หนึ่ง");
                        }
                        if ((L > 1) && (s.Substring(L - 1, 1) == "0"))
                        {
                            result = result + "หนึ่ง";
                        }
                        else
                        {
                            result = result + "เอ็ด";
                        }
                    }
                    else if ((i == L - 2) && (c == 2))
                    {
                        result = result + "ยี่สิบ";
                    }
                    else if ((i == L - 2) && (c == 1))
                    {
                        result = result + "สิบ";
                    }
                    else
                    {
                        if (c != 0)
                        {
                            result = result + numSpeak[c] + suffix[L - i];
                        }
                    }
                }
            }

            return (result);
        }

        private static string speakStang(string s)
        {
            int L, c;
            string result;

            L = s.Length;

            if (L == 0) return ("");

            if (L == 1)
            {
                s = s + "0";
                L = 2;
            }
            if (L > 2)
            {
                s = s.Substring(0, 2);
                L = 2;
            }

            result = "";
            for (int i = 0; i < 2; i++)
            {
                c = Convert.ToInt32(s.Substring(i, 1));
                if ((i == L - 1) && (c == 1))
                {
                    if (Convert.ToInt32(s.Substring(0, 1)) == 0)
                        result = result + "หนึ่ง";
                    else
                        result = result + "เอ็ด";
                }
                else if ((i == L - 2) && (c == 2))
                {
                    result = result + "ยี่สิบ";
                }
                else if ((i == L - 2) && (c == 1))
                {
                    result = result + "สิบ";
                }
                else
                {
                    if (c != 0)
                    {
                        result = result + numSpeak[c] + suffix[L - i];
                    }
                }
            }

            return (result);
        }

        private static void splitCurr(double m)
        {
            string s;
            int L;
            int position;

            s = System.Convert.ToString(m);
            position = s.IndexOf(".");
            if ((position >= 0))
            {
                s1 = s.Substring(0, position);
                s3 = s.Substring(position + 1);
                if (s3 == "00")
                {
                    s3 = "";
                }
            }
            else
            {
                s1 = s;
                s3 = "";
            }
            L = s1.Length;
            if ((L > 6))
            {
                s2 = s1.Substring(L - 6);
                s1 = s1.Substring(0, L - 6);
            }
            else
            {
                s2 = s1;
                s1 = "";
            }

            if ((s1 != "") && (Convert.ToInt32(s1) == 0)) s1 = "";
            if ((s2 != "") && (Convert.ToInt32(s2) == 0)) s2 = "";
        }
    }
}
