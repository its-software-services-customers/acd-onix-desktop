using Onix.Client.Helper;
using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MEmployee : MBaseModel
    {
        private MCommissionProfile csp = new MCommissionProfile(new CTable(""));
        private MEmployeeTax empTax = new MEmployeeTax(new CTable(""));

        public MEmployee(CTable obj) : base(obj)
        {
        }

        public void CreateDefaultValue()
        {
        }

        public MEmployeeTax EmployeeTax
        {
            get
            {
                return empTax;
            }

            set
            {
            }
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();

            CToolTipItem ct = new CToolTipItem("employee_code", EmployeeCode);
            ttItems.Add(ct);

            ct = new CToolTipItem("employee_name", EmployeeName);
            ttItems.Add(ct);

            ct = new CToolTipItem("employee_type", EmployeeTypeName);
            ttItems.Add(ct);

            ct = new CToolTipItem("employee_group", EmployeeGroupName);
            ttItems.Add(ct);
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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String EmployeeNameLastname
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME_LASTNAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME_LASTNAME", value);
                NotifyPropertyChanged();
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
                NotifyPropertyChanged();
            }
        }

        public String EmployeeNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_NAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_NAME_ENG", value);
                NotifyPropertyChanged();
            }
        }

        public String Address
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("ADDRESS", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Email
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMAIL"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMAIL", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Website
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("WEBSITE"));
            }

            set
            {
                GetDbObject().SetFieldValue("WEBSITE", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Phone
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("PHONE"));
            }

            set
            {
                GetDbObject().SetFieldValue("PHONE", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Fax
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FAX"));
            }

            set
            {
                GetDbObject().SetFieldValue("FAX", value);
                //updateFlag();
                NotifyPropertyChanged();
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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_CYCLE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_CYCLE_ID", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Gender
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("GENDER"));
            }

            set
            {
                GetDbObject().SetFieldValue("GENDER", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String GenderName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.GenderToString(Gender));
            }

            set
            {

            }
        }

        public Boolean IsMale
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (Gender.Equals("1"));
            }

            set
            {
                if (value)
                {
                    Gender = "1";
                    NotifyPropertyChanged("IsMale");
                }
            }
        }

        public Boolean IsFemale
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (Gender.Equals("2"));
            }

            set
            {
                if (value)
                {
                    Gender = "2";
                    NotifyPropertyChanged("IsFemale");
                }
            }
        }

        public String Position
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_POSITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_POSITION", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String Department
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_DEPARTMENT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_DEPARTMENT", value);
                //updateFlag();
                NotifyPropertyChanged();
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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String EmployeeLastNameEng
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_LASTNAME_ENG"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_LASTNAME_ENG", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String FingerPrintCode
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("FINGERPRINT_CODE"));
            }

            set
            {
                GetDbObject().SetFieldValue("FINGERPRINT_CODE", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String LineID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("LINE_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("LINE_ID", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region EmployeeTypeObj
        public MMasterRef EmployeeTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    EmployeeType = m.MasterID;
                    EmployeeTypeName = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = EmployeeType;
                mr.Description = EmployeeTypeName;
                return (mr);
            }
        }

        public String EmployeeType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_TYPE", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public Boolean IsDaily
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (EmployeeType.Equals("1"));
            }

            set
            {
                if (value)
                {
                    EmployeeType = "1";
                    NotifyPropertyChanged("IsDaily");
                }
            }
        }

        public Boolean IsMonthly
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (false);
                }

                return (EmployeeType.Equals("2"));
            }

            set
            {
                if (value)
                {
                    EmployeeType = "2";
                    NotifyPropertyChanged("IsMonthly");
                }
            }
        }

        public String EmployeeTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (CUtil.EmployeeTypeToString(EmployeeType));
            }

            set
            {

            }
        }
        #endregion

        public String EmployeeImageName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_PROFILE_IMAGE"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_PROFILE_IMAGE", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String EmployeeImageNameWip
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_PROFILE_IMAGE_WIP"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_PROFILE_IMAGE_WIP", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String EmployeeImageFlag
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_PROFILE_EXT_FLAG"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_PROFILE_EXT_FLAG", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        #region EmployeeGroupObj
        public MMasterRef EmployeeGroupObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    EmployeeGroup = m.MasterID;
                    EmployeeGroupName = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = EmployeeGroup;
                mr.Description = EmployeeGroupName;
                return (mr);
            }
        }

        public String EmployeeGroup
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_GROUP"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_GROUP", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String EmployeeGroupName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_GROUP_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_GROUP_NAME", value);
            }
        }
        #endregion

        #region CategoryeObj
        public MMasterRef CategoryeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    Category = m.MasterID;
                    CategoryDesc = m.Description;
                }
            }
        }
        public String Category
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("CATEGORY"));
            }

            set
            {
                GetDbObject().SetFieldValue("CATEGORY", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String CategoryDesc
        {
            get
            {
                if (Category.Equals(""))
                {
                    return ("");
                }

                String str = CUtil.EmployeeCategoryToString(Category);

                return (str);
            }

            set
            {

            }
        }
        #endregion

        #region BranchObj
        public MMasterRef BranchObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    BranchID = m.MasterID;
                    BranchName = m.Description;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = BranchID;
                mr.Description = BranchName;
                return (mr);
            }
        }

        public String BranchID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_ID", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }
        public String BranchName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_NAME", value);
            }
        }
        #endregion

        #region CommissionCycleTypeObj
        public MMasterRef CommissionCycleTypeObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    CycleType = m.MasterID;
                    CycleTypeName = m.Description;
                }
            }
        }
        public String CycleType
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_CYCLE_TYPE"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_CYCLE_TYPE", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String CycleTypeName
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("COMMISSION_CYCLE_TYPE_NAME"));
            }

            set
            {
                GetDbObject().SetFieldValue("COMMISSION_CYCLE_TYPE_NAME", value);
            }
        }
        #endregion

        #region NamePrefix

        public MMasterRef NamePrefixObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    NamePrefix = m.MasterID;
                }
            }

            get
            {
                MMasterRef mr = new MMasterRef(new CTable(""));
                mr.MasterID = NamePrefix;
                return (mr);
            }
        }

        public String NamePrefix
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("NAME_PREFIX"));
            }

            set
            {
                GetDbObject().SetFieldValue("NAME_PREFIX", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String NamePrefixDesc
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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        #endregion NamePrefix

        #region Bank Account
        
        public String BankBranchName
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
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String AccountNo
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
                NotifyPropertyChanged();

                updateFlag();
            }
        }

        public String BankID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BANK_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BANK_ID", value);
            }
        }

        public MMasterRef BankObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    BankID = m.MasterID;
                    NotifyPropertyChanged();
                }
            }

            get
            {
                if (GetDbObject() == null)
                {
                    return (null);
                }

                ObservableCollection<MMasterRef> items = CMasterReference.Instance.Banks;
                if (items == null)
                {
                    return (null);
                }

                return (CUtil.MasterIDToObject(items, BankID));
            }
        }

        #endregion

        public String IDNumber
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
                NotifyPropertyChanged();
            }
        }

        public String HourRate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("HOUR_RATE"));
            }

            set
            {
                GetDbObject().SetFieldValue("HOUR_RATE", value);
                NotifyPropertyChanged();
            }
        }


        public String Salary
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("SALARY"));
            }

            set
            {
                GetDbObject().SetFieldValue("SALARY", value);
                NotifyPropertyChanged();
            }
        }

        #region EmployeeDepartment

        public MMasterRef DepartmentObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    DepartmentID = m.MasterID;
                    DepartmentName = m.Description;
                }
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.EmployeeDepartments;
                if (items == null)
                {
                    return (null);
                }

                String tm = DepartmentID;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String DepartmentID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_DEPARTMENT"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_DEPARTMENT", value);
                //updateFlag();
                NotifyPropertyChanged();
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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        #endregion Department


        #region Employee Position

        public MMasterRef PositionObj
        {
            set
            {
                MMasterRef m = value as MMasterRef;
                if (m != null)
                {
                    PositionID = m.MasterID;
                    PositionName = m.Description;
                }
            }

            get
            {
                ObservableCollection<MMasterRef> items = CMasterReference.Instance.EmployeePositions;
                if (items == null)
                {
                    return (null);
                }

                String tm = PositionID;
                return (CUtil.MasterIDToObject(items, tm));
            }
        }

        public String PositionID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_POSITION"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_POSITION", value);
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

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
                //updateFlag();
                NotifyPropertyChanged();
            }
        }

        #endregion Position

        public Boolean? HasResignedFlag
        {
            get
            {
                String flag = GetDbObject().GetFieldValue("RESIGNED_FLAG");
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
                    flag = "N";
                }
                else if ((Boolean)value)
                {
                    flag = "Y";
                }

                GetDbObject().SetFieldValue("RESIGNED_FLAG", flag);
                NotifyPropertyChanged();
            }
        }

        public DateTime HiringDate
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return (DateTime.Now);
                }

                String str = GetDbObject().GetFieldValue("HIRING_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);

                return (dt);
            }

            set

            {
                String str = CUtil.DateTimeToDateStringInternal(value);

                GetDbObject().SetFieldValue("HIRING_DATE", str);
                NotifyPropertyChanged();
            }
        }

        public String HiringDateFmt
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                String str = GetDbObject().GetFieldValue("HIRING_DATE");
                DateTime dt = CUtil.InternalDateToDate(str);
                String str2 = CUtil.DateTimeToDateString(dt);

                return (str2);
            }

            set
            {
            }
        }

        public String EmployeeAddress
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("EMPLOYEE_ADDRESS"));
            }

            set
            {
                GetDbObject().SetFieldValue("EMPLOYEE_ADDRESS", value);
                NotifyPropertyChanged();
            }
        }
    }
}
