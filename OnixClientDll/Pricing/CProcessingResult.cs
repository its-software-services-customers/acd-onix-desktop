using System;
using Onix.Client.Helper;
using Onix.Client.Model;

namespace Onix.Client.Pricing
{
    public enum ProcessingResultStatus
    {
        ProcessingSuccess = 1,
        ProcessingFail,
        ProcessingSkip
    }

    public class CProcessingResult
    {
        private String eName = "";
        private CBasketSet inpBs = null;
        private CBasketSet outBs = null;
        private String grpName = "";
        private String errStr = "";
        private ProcessingResultStatus sts = ProcessingResultStatus.ProcessingSuccess;
        private MPackage package = null;
        private Boolean isOperation = false;
        private double finalDiscount = 0.00;
        private Boolean isFinalDisc = false;

        public CProcessingResult(MPackage pkg)
        {
            package = pkg;
        }

        public CProcessingResult(String execName)
        {
            eName = execName;
        }

        public String FinalDiscountFmt
        {
            get
            {
                return (CUtil.FormatNumber(finalDiscount.ToString()));
            }
        }

        public Boolean IsFinalDiscount
        {
            get
            {
                return (isFinalDisc);
            }
        }

        public double FinalDiscount
        {
            get
            {
                return (finalDiscount);
            }

            set
            {
                isFinalDisc = true;
                finalDiscount = value;
            }
        }

        public void SetInputBasketSet(CBasketSet bks)
        {            
            inpBs = bks;
            inpBs.Direction = "INPUT";
        }

        public CBasketSet GetInputBasketSet()
        {
            return (inpBs);
        }

        public void SetOutputBasketSet(CBasketSet bks)
        {
            outBs = bks;
            outBs.Direction = "OUTPUT";
        }

        public CBasketSet GetOutputBasketSet()
        {
            return (outBs);
        }

        public void SetGroupName(String groupName)
        {
            grpName = groupName;
        }

        public String GetGroupName()
        {
            return(grpName);
        }

        public void SetStatus(ProcessingResultStatus status)
        {
            sts = status;
        }

        public ProcessingResultStatus GetStatus()
        {
            return(sts);
        }

        public void SetErrorCode(String err)
        {
            errStr = err;
        }

        public String GetErrorCode()
        {
            return (errStr);
        }

        public String GetExecutioName()
        {
            return(eName);
        }

        public MPackage GetPackage()
        {
            return (package);
        }

        public Boolean IsOperation
        {
            get
            {
                return (isOperation);
            }

            set
            {
                isOperation = value;
            }
        }
    }
}
