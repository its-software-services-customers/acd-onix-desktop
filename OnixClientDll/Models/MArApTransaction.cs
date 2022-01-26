using System;
using System.Windows;
using Wis.WsClientAPI;
using Onix.Client.Helper;
using System.Windows.Media;

namespace Onix.Client.Model
{
    public class MArApTransaction : MBaseModel
    {
        private Boolean isBalanceFwd = false;

        public MArApTransaction(CTable obj) : base(obj)
        {

        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("note", NoteTX);
            ttItems.Add(ct);
        }

        public Boolean IsBalanceForward
        {
            get
            {
                return (isBalanceFwd);
            }

            set
            {
                isBalanceFwd = value;
            }
        }

        public FontWeight FontWeight
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (FontWeights.Bold);
                }

                return (FontWeights.Normal);
            }
        }

        public override SolidColorBrush RowColor
        {
            get
            {
                String tmp = "2";
                if (IsBalanceForward)
                {
                    tmp = "1";
                }

                return (CUtil.DocStatusToColor(tmp));
            }
        }

        public void CreateDefaultValue()
        {

        }

        //public override void createToolTipItems()
        //{
        //    ttItems.Clear();

        //    if (!IsBalanceForward)
        //    {
        //        if (ReceiptFlag.Equals(false) && (DateTime.Now.Date > DueDate.Date))
        //        {
        //            CToolTipItem ct = new CToolTipItem("over_due_date", ((DateTime.Now.Date - DueDate.Date).TotalDays).ToString());
        //            ttItems.Add(ct); 

        //        }
        //    }
        //}

        public Boolean? ReceiptFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("RECEIPT_FLAG");
                if (flag.Equals(""))
                {
                    return (false);
                }
                return (flag.Equals("Y"));
            }

            set
            {
                String flag = "N";
                if (value == null)
                {
                    flag = "";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }
                GetDbObject().SetFieldValue("RECEIPT_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public String DocumentMovementNo
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_NO", value);
                NotifyPropertyChanged();
            }
        }

        public DateTime DocumentDateMovement
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DOCUMENT_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DocumentDateMovementFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return (CLanguage.getValue("balance_forward"));
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DOCUMENT_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime ApproveDate
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("APPROVED_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("APPROVED_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String ApproveDateFmt
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("APPROVED_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public DateTime DueDate
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set
            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("DUE_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String DueDateFmt
        {
            get
            {

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("DUE_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String TxAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String TxType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("TX_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("TX_TYPE", value);
                NotifyPropertyChanged();
            }
        }
        public String BeginAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("BEGIN_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }
        public String EndAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                if (IsBalanceForward)
                {
                    return (GetDbObject().GetFieldValue("BEGIN_AMOUNT"));
                }
                else
                {
                    return (GetDbObject().GetFieldValue("END_AMOUNT"));
                }
            }

            set
            {
                GetDbObject().SetFieldValue("END_AMOUNT", value);
                NotifyPropertyChanged();
            }
        }

        public String InAmount
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Amount = "";
                if (TxType.Equals("I"))
                {
                    Amount = TxAmount;
                }

                return (Amount);
            }
        }
        public String OutAmount
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                String Amount = "";
                if (TxType.Equals("E"))
                {
                    Amount = TxAmount;
                }

                return (Amount);
            }
        }

        public String InAmountFmt
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (InAmount.Equals(""))
                    return "";

                return (CUtil.FormatNumber(InAmount));
            }
        }
        public String OutAmountFmt
        {
            get
            {
                if (OutAmount.Equals(""))
                    return "";

                return (CUtil.FormatNumber(OutAmount));
            }
        }

        public String NoteTX
        {
            get
            {
                if (IsBalanceForward)
                {
                    return ("");
                }

                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DOCUMENT_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("DOCUMENT_DESC", value);
                NotifyPropertyChanged();
            }
        }

        public String BeginAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(BeginAmount));
            }
        }

        public String EndAmountFmt
        {
            get
            {
                return (CUtil.FormatNumber(EndAmount));
            }
        }

		#region Customer
		public String EntityCode
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("ENTITY_CODE"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_CODE", value);
				NotifyPropertyChanged();
			}
		}

		public String EntityName
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("ENTITY_NAME"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_NAME", value);
				NotifyPropertyChanged();
			}
		}

		public String EntityType
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("0");
				}

				return (GetDbObject().GetFieldValue("ENTITY_TYPE"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_TYPE", value);
				NotifyPropertyChanged();
			}
		}

		public String EntityTypeName
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("ENTITY_TYPE_NAME"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_TYPE_NAME", value);
				NotifyPropertyChanged();
			}
		}

		public String EntityGroup
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("0");
				}

				return (GetDbObject().GetFieldValue("ENTITY_GROUP"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_GROUP", value);
				NotifyPropertyChanged();
			}
		}

		public String EntityGroupName
		{
			get
			{
				if (GetDbObject() == null)
				{
					return ("");
				}

				return (GetDbObject().GetFieldValue("ENTITY_GROUP_NAME"));
			}

			set
			{
				GetDbObject().SetFieldValue("ENTITY_GROUP_NAME", value);
				NotifyPropertyChanged();
			}
		}

		#endregion
	}
}
