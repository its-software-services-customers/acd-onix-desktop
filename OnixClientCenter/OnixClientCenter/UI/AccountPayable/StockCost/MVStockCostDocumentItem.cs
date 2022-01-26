using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Windows.Media;

namespace Onix.ClientCenter.UI.AccountPayable.StockCost
{
    public class MVStockCostDocumentItem : MBaseModel
    {
        public MVStockCostDocumentItem(CTable obj) : base(obj)
        {

        }        
        
        public void CreateDefaultValue()
        {
        }

        public String CostDocItemID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COST_DOC_ITEM_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COST_DOC_ITEM_ID", value);
            }
        }

        public String CostDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COST_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COST_DOC_ID", value);
            }
        }
   
        public String ItemType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ITEM_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("ITEM_TYPE", value);
                NotifyPropertyChanged();
            }
        }

        #region amount

        private void calculateTotal()
        {
            //double jan = CUtil.StringToDouble(JanAmount);
            //double feb = CUtil.StringToDouble(FebAmount);
            //double mar = CUtil.StringToDouble(MarAmount);
            //double apr = CUtil.StringToDouble(AprAmount);
            //double may = CUtil.StringToDouble(MayAmount);
            //double jun = CUtil.StringToDouble(JunAmount);
            //double jul = CUtil.StringToDouble(JulAmount);
            //double aug = CUtil.StringToDouble(AugAmount);
            //double sep = CUtil.StringToDouble(SepAmount);
            //double oct = CUtil.StringToDouble(OctAmount);
            //double nov = CUtil.StringToDouble(NovAmount);
            //double dec = CUtil.StringToDouble(DecAmount);

            //double tot = jan + feb + mar + apr + may + jun + jul + aug + sep + oct + nov + dec;
            //TotAmount = tot.ToString();
        }

        public String JanAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("JAN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("JAN_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String FebAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FEB_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("FEB_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String MarAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MAR_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("MAR_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String AprAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("APR_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("APR_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String MayAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("MAY_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("MAY_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String JunAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("JUN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("JUN_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String JulAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("JUL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("JUL_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String AugAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("AUG_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("AUG_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String SepAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SEP_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("SEP_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String OctAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("OCT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("OCT_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String NovAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOV_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOV_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String DecAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEC_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEC_AMOUNT", value);
                calculateTotal();
                NotifyPropertyChanged();
            }
        }

        public String TotAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TOTAL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TOTAL_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region amount format

        public String JanAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(JanAmount);
                return (fmt);
            }

            set
            {
            }
        }


        public String FebAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(FebAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String MarAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(MarAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String AprAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(AprAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String MayAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(MayAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String JunAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(JunAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String JulAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(JulAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String AugAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(AugAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String SepAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(SepAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String OctAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(OctAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String NovAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(NovAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String DecAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DecAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String TotAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(TotAmount);
                return (fmt);
            }

            set
            {
            }
        }
        #endregion
    }
}
