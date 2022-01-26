using System;
using System.Collections;
using Onix.ClientCenter.UI.HumanResource.OTDocument;

namespace Onix.ClientCenter.UI.HumanResource.Utils
{
    public static class HRUtils
    {
        public static ArrayList ConstructOtRates()
        {
            ArrayList arr = new ArrayList();

            arr.Add(new MVOTRate("วันทำงาน", "00", "00", "08", "00", "1.50"));
            arr.Add(new MVOTRate("วันทำงาน", "08", "00", "17", "00", "1.00"));
            arr.Add(new MVOTRate("วันทำงาน", "17", "00", "24", "00", "1.50"));

            arr.Add(new MVOTRate("วันอาทิตย์", "00", "00", "08", "00", "3.00"));
            arr.Add(new MVOTRate("วันอาทิตย์", "08", "00", "17", "00", "1.00"));
            arr.Add(new MVOTRate("วันอาทิตย์", "17", "00", "24", "00", "3.00"));

            arr.Add(new MVOTRate("วันหยุด", "00", "00", "08", "00", "3.00"));
            arr.Add(new MVOTRate("วันหยุด", "08", "00", "17", "00", "2.00"));
            arr.Add(new MVOTRate("วันหยุด", "17", "00", "24", "00", "3.00"));

            return arr;
        }
    }
}
