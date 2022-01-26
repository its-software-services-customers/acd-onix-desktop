using System;
using System.Collections.ObjectModel;
using Wis.WsClientAPI;

namespace Onix.Client.Model
{
    public class MBranchConfigPosCenter : MBaseModel
    {
        private int seq = 0;

        public MBranchConfigPosCenter(CTable obj) : base(obj)
        {
        }

        public override void createToolTipItems()
        {
            ttItems.Clear();
        }

        public void CreateDefaultValue()
        {
        }

        public String BranchConfigID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_CONFIG_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_CONFIG_ID", value);
            }
        }

        public String BranchPosID
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("BRANCH_POS_ID"));
            }

            set
            {
                GetDbObject().SetFieldValue("BRANCH_POS_ID", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public String PosSerialNo
        {
            get
            {
                if (GetDbObject() == null)
                {
                    return ("");
                }

                return (GetDbObject().GetFieldValue("POS_SERIAL_NO"));
            }

            set
            {
                GetDbObject().SetFieldValue("POS_SERIAL_NO", value);
                updateFlag();
                NotifyPropertyChanged();
            }
        }

        public int Seq
        {
            get
            {
                return (seq);
            }

            set
            {
                GetDbObject().SetFieldValue("INTERNAL_SEQ", value.ToString());
                seq = value;
            }
        }
    }


}
