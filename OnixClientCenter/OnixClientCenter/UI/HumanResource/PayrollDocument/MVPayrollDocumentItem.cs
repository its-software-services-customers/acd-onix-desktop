using System;
using Onix.Client.Helper;
using Wis.WsClientAPI;
using Onix.Client.Model;
using System.Collections;

namespace Onix.ClientCenter.UI.HumanResource.PayrollDocument
{
    public class MVPayrollDocumentItem : MBaseModel
    {
        private Boolean forDelete = false;
        private String oldFlag = "";

        private MVPayrollDocumentItem balanceTotalItem = null;
        private MVPayrollDocumentItem balanceYearItem = null;
        private MVPayrollDocumentItem endingTotalItem = null;
        private MVPayrollDocumentItem endingYearItem = null;

        public MVPayrollDocumentItem(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        private Boolean isAccumField(String field)
        {
            if (field.Contains("RECEIVE"))
            {
                return (true);
            }

            if (field.Contains("REMAIN"))
            {
                return (true);
            }

            if (field.Contains("DEDUCT"))
            {
                return (true);
            }

            return (field.Contains("SOCIAL_SECURITY_COMPANY"));
        }

        private String deriveDefinitionText()
        {
            CTable t = GetDbObject();
            ArrayList fields = t.GetTableFields();

            String definition = "";
            int i = 0;
            foreach (CField field in fields)
            {
                String fname = field.getName();

                if (!isAccumField(fname))
                {
                    continue;
                }

                String value = t.GetFieldValue(fname);
                String tmp = String.Format("{0}:{1}", fname, value);

                if (i == 0)
                {
                    definition = tmp;
                }
                else
                {
                    definition = String.Format("{0}|{1}", definition, tmp);
                }

                i++;
            }

            return (definition);
        }

        public String GetDefinitionText()
        {
            String definition = deriveDefinitionText();
            return (definition);
        }

        public void SumItem(MVPayrollDocumentItem accum, MVPayrollDocumentItem tx)
        {
            CTable a = accum.GetDbObject();
            CTable t = tx.GetDbObject();

            ArrayList fields = t.GetTableFields();

            foreach (CField field in fields)
            {
                String fname = field.getName();

                if (!isAccumField(fname))
                {
                    continue;
                }

                double txValue = CUtil.StringToDouble(t.GetFieldValue(fname));
                double acValue = CUtil.StringToDouble(a.GetFieldValue(fname));
                double sum = txValue + acValue;

                SetFieldValue(fname, sum.ToString());
            }
        }

        private MVPayrollDocumentItem createDefinitionObject(String definition)
        {
            String[] fields = definition.Split('|');
            CTable t = new CTable("");

            foreach (String field in fields)
            {
                String[] tokens = field.Split(':');
                if (tokens.Length != 2)
                {
                    continue;
                }

                String name = tokens[0];
                String value = tokens[1];

                t.SetFieldValue(name, value);
            }

            MVPayrollDocumentItem m = new MVPayrollDocumentItem(t);
            return (m);
        }

        public override void InitializeAfterLoaded()
        {
            balanceTotalItem = createDefinitionObject(BalanceTotalDefinition);
            balanceYearItem = createDefinitionObject(BalanceYearDefinition);
            endingTotalItem = createDefinitionObject(EndingTotalDefinition);
            endingYearItem = createDefinitionObject(EndingYearDefinition);
        }

        #region DefinitionObj

        public MVPayrollDocumentItem BalanceTotalObj
        {
            get
            {
                return (balanceTotalItem);
            }

            set
            {
            }
        }

        public MVPayrollDocumentItem BalanceYearObj
        {
            get
            {
                return (balanceYearItem);
            }

            set
            {
            }
        }

        public MVPayrollDocumentItem EndingTotalObj
        {
            get
            {
                return (endingTotalItem);
            }

            set
            {
            }
        }

        public MVPayrollDocumentItem EndingYearObj
        {
            get
            {
                return (endingYearItem);
            }

            set
            {
            }
        }

        #endregion

        #region Definition

        public String BalanceTotalDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BAL_TOTAL_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("BAL_TOTAL_DEFINITION", value);
                updateFlag();
            }
        }

        public String BalanceYearDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BAL_YEAR_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("BAL_YEAR_DEFINITION", value);
                updateFlag();
            }
        }

        public String EndingTotalDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENDING_TOTAL_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENDING_TOTAL_DEFINITION", value);
                updateFlag();
            }
        }

        public String EndingYearDefinition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ENDING_YEAR_DEFINITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("ENDING_YEAR_DEFINITION", value);
                updateFlag();
            }
        }

        #endregion

        public override String ID
        {
            get
            {
                return (EmployeeID);
            }
        }

        public bool IsDeleted
        {
            get
            {
                return (forDelete);
            }

            set
            {
                forDelete = value;
                if (forDelete)
                {
                    oldFlag = ExtFlag;
                    ExtFlag = "D";
                }
                else
                {
                    ExtFlag = oldFlag;
                }
            }
        }

        public String PayrollDocID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PAYROLL_DOC_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("PAYROLL_DOC_ID", value);
            }
        }

        #region Employee

        public MEmployee EmployeeObj
        {
            set
            {
                if (value == null)
                {
                    EmployeeID = "";
                    EmployeeCode = "";
                    EmployeeName = "";
                    EmployeeLastName = "";

                    return;
                }

                MEmployee ii = (value as MEmployee);

                EmployeeID = ii.EmployeeID;
                EmployeeCode = ii.EmployeeCode;
                EmployeeName = ii.EmployeeName;
                EmployeeLastName = ii.EmployeeLastName;
                EmployeeNamePrefixDesc = ii.NamePrefixDesc;
                EmployeeAccountNo = ii.AccountNo;
                EmployeeBankBranch = ii.BankBranchName;
                EmployeeBankName = ii.BankObj.Description;
                EmployeeIDNumber = ii.IDNumber;
                PositionName = ii.PositionName;
                DepartmentName = ii.DepartmentName;
                

                updateFlag();
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                MEmployee m = new MEmployee(new CTable(""));
                m.EmployeeID = EmployeeID;
                m.EmployeeCode = EmployeeCode;
                m.EmployeeName = EmployeeName;
                m.EmployeeLastName = EmployeeLastName;
                m.EmployeeNameLastname = EmployeeNameLastName;
                m.NamePrefixDesc = EmployeeNamePrefixDesc;
                m.AccountNo = EmployeeAccountNo;
                m.BankBranchName = EmployeeBankBranch;
                m.BankObj.Description = EmployeeBankName;
                m.IDNumber = EmployeeIDNumber;
                m.DepartmentName = DepartmentName;
                m.PositionName = PositionName;

                return (m);
            }
        }

        public String EmployeeID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ID", value);
            }
        }

        public String EmployeeCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_CODE", value);
            }
        }

        public String EmployeeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME", value);
            }
        }

        public String EmployeeLastName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_LASTNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_LASTNAME", value);
            }
        }

        public String EmployeeNameLastName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (EmployeeName + " " + EmployeeLastName);
            }

            set
            {
            }
        }

        public String EmployeeNamePrefixDesc
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX_DESC"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX_DESC", value);
            }
        }

        public String EmployeeAccountNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ACCOUNT_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("ACCOUNT_NO", value);
            }
        }

        public String EmployeeBankBranch
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_BRANCH_NAME", value);
            }
        }

        public String EmployeeBankName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_NAME", value);
            }
        }

        public String EmployeeIDNumber
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ID_NUMBER"));
            }

            set
            {
                GetDbObject().SetFieldValue("ID_NUMBER", value);
            }
        }

        #endregion region

        public String PositionName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("POSITION_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("POSITION_NAME", value);
            }
        }

        public String DepartmentName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEPARTMENT_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEPARTMENT_NAME", value);
            }
        }

        public String ReceiveAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("ReceiveAmountFmt");

                updateFlag();
            }
        }

        public String ReceiveAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String DeductAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("DeductAmountFmt");
                updateFlag();
            }
        }

        public String DeductAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String RemainAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("REMAIN_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("REMAIN_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("RemainAmountFmt");
                updateFlag();
            }
        }

        public String RemainAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(RemainAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String GrandTotalAmount
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GRAND_TOTAL_AMOUNT"));
            }

            set
            {
                GetDbObject().SetFieldValue("GRAND_TOTAL_AMOUNT", value);
                NotifyPropertyChanged();
                NotifyPropertyChanged("GrandTotalAmountFmt");
                updateFlag();
            }
        }

        public String GrandTotalAmountFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(GrandTotalAmount);
                return (fmt);
            }

            set
            {
            }
        }

        public String GrandTotalAmountFmtInt
        {
            get
            {
                int pos = GrandTotalAmountFmt.IndexOf(".");
                String part1 = "=" + GrandTotalAmountFmt.Substring(0, pos) + "=";

                return (part1);
            }

            set
            {
            }
        }

        public String GrandTotalAmountFmtDec
        {
            get
            {
                int pos = GrandTotalAmountFmt.IndexOf(".");
                String part1 = "";
                if (pos > 0)
                {
                    part1 = GrandTotalAmountFmt.Substring(pos+1);
                }
                return (part1);
            }

            set
            {
            }
        }

        public String Note
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("NOTE", value);
                updateFlag();
            }
        }

        #region receive fields

        public String ReceiveIncome
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_INCOME"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_INCOME", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveIncomeFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveIncome);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveOT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_OT"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_OT", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveOTFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveOT);
                return (fmt);
            }

            set
            {
            }
        }


        public String SlipReceiveOT
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SLIP_RECEIVE_OT"));
            }

            set
            {
                GetDbObject().SetFieldValue("SLIP_RECEIVE_OT", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String SlipReceiveOTFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(SlipReceiveOT);
                return (fmt);
            }

            set
            {
            }
        }
        
        //เงินประจำตำแหน่ง
        public String ReceivePosition
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_POSITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_POSITION", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceivePositionFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceivePosition);
                return (fmt);
            }

            set
            {
            }
        }

        //ค่ายานพาหนะ
        public String ReceiveTransaportation
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_TRANSPORTATION"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_TRANSPORTATION", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveTransaportationFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveTransaportation);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveTelephone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_TELEPHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_TELEPHONE", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveTelephoneFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveTelephone);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveCommission
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_COMMISSION"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_COMMISSION", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveCommissionFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveCommission);
                return (fmt);
            }

            set
            {
            }
        }

        //เบี้ยขยัน
        public String ReceiveAllowance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_ALLOWANCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_ALLOWANCE", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveAllowanceFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveAllowance);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveBonus
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_BONUS"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_BONUS", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveBonusFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveBonus);
                return (fmt);
            }

            set
            {
            }
        }

        public String ReceiveOtherTotal
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_OTHERS_TOTAL"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_OTHERS_TOTAL", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveOtherTotalFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveOtherTotal);
                return (fmt);
            }

            set
            {
            }
        }

        //ใช้หนี้คืนพนักงาน ที่ออกเงินบางอย่าง เช่น ค่าที่จอดรถ ทางด่วนไปก่อน
        public String ReceiveRefund
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("RECEIVE_REFUND"));
            }

            set
            {
                GetDbObject().SetFieldValue("RECEIVE_REFUND", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String ReceiveRefundFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(ReceiveRefund);
                return (fmt);
            }

            set
            {
            }
        }

        #endregion receive fields

        #region deduct fields

        public String DeductTax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_TAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_TAX", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductTaxFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductTax);
                return (fmt);
            }

            set
            {
            }
        }

        //ขาด ลา มาสาย
        public String DeductPenalty
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_PENALTY"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_PENALTY", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductPenaltyFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductPenalty);
                return (fmt);
            }

            set
            {
            }
        }

        //หักเงินที่พนักงานยืมบริษัทไป หรือเป็นหนี้อื่น ๆ 
        public String DeductBorrow
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_BORROW"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_BORROW", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        //หักที่เบิกล่วงหน้า 
        public String DeductAdvance
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_ADVANCE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_ADVANCE", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductAdvanceFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductAdvance);
                return (fmt);
            }

            set
            {
            }
        }

        //ประกันสังคม
        public String DeductSocialSecurity
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_SOCIAL_SECURITY"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_SOCIAL_SECURITY", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductSocialSecurityFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductSocialSecurity);
                return (fmt);
            }

            set
            {
            }
        }

        //เงินประกัน
        public String DeductCoverage
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_COVERAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_COVERAGE", value);
                calculateFields();
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductCoverageFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductCoverage);
                return (fmt);
            }

            set
            {
            }
        }

        public String DeductOther
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_OTHER"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_OTHER", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String DeductOtherFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(DeductOther);
                return (fmt);
            }

            set
            {
            }
        }

        //รายละเอียดของการหักอื่น ๆ 
        public String DeductOtherNote
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("DEDUCT_OTHER_NOTE"));
            }

            set
            {
                GetDbObject().SetFieldValue("DEDUCT_OTHER_NOTE", value);
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        //ประกันสังคม ส่วนนายจ้าง 
        public String SocialSecurityCompany
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SOCIAL_SECURITY_COMPANY"));
            }

            set
            {
                GetDbObject().SetFieldValue("SOCIAL_SECURITY_COMPANY", value);                
                NotifyPropertyChanged();
                updateFlag();
            }
        }

        public String SocialSecurityCompanyFmt
        {
            get
            {
                String fmt = CUtil.FormatNumber(SocialSecurityCompany);
                return (fmt);
            }

            set
            {
            }
        }
        #endregion deduct fields

        public void calculateFields()
        {
            double income = CUtil.StringToDouble(ReceiveIncome);
            double ot = CUtil.StringToDouble(ReceiveOT);
            double position = CUtil.StringToDouble(ReceivePosition);
            double transport = CUtil.StringToDouble(ReceiveTransaportation);
            double telephone = CUtil.StringToDouble(ReceiveTelephone);
            double commission = CUtil.StringToDouble(ReceiveCommission);
            double allowance = CUtil.StringToDouble(ReceiveAllowance);
            double bonus = CUtil.StringToDouble(ReceiveBonus);
            double refund = CUtil.StringToDouble(ReceiveRefund); //จะไม่ถูกนำไปรวมคิดรายรับของพนักงาน เพราะเป็นการใช้เงินคืนจากบริษัท

            double tax = CUtil.StringToDouble(DeductTax);
            double penalty = CUtil.StringToDouble(DeductPenalty);
            double borrow = CUtil.StringToDouble(DeductBorrow);
            double advance = CUtil.StringToDouble(DeductAdvance);
            double socialSecurity = CUtil.StringToDouble(DeductSocialSecurity);
            double coverage = CUtil.StringToDouble(DeductCoverage);

            double deductOther = borrow + advance + coverage;
            double receiveOthers = commission + allowance + bonus + refund; //ลูกค้าต้องการให้แสดงแบบนี้

            ot = Math.Floor(ot - penalty); //ปัดลงทุกกรณี, ตอนแสดง OT ให้แสดงแบบที่หักขาดลาสายไปแล้ว พนักงานจะไม่ตั้งคำถามว่าหักอะไร

            double received = income + ot + position + transport + telephone + receiveOthers; 
            double deduced = tax + socialSecurity + deductOther;

            double remain = received - deduced;
            double grandTotal = remain;//ตัวเงินจริง ๆ ทั้งหมดที่พนักงานได้รับ

            RemainAmount = remain.ToString();
            ReceiveAmount = received.ToString();
            DeductAmount = deduced.ToString();
            ReceiveOtherTotal = receiveOthers.ToString();
            DeductOther = deductOther.ToString();
            SlipReceiveOT = ot.ToString();

            GrandTotalAmount = grandTotal.ToString();
        }
    }
}
