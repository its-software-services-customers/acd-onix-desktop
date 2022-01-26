using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Onix.Client.Helper
{
    public class CToolTipItem : INotifyPropertyChanged
    {
        private String lblKeyWord = "";
        private String desc = "";

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public CToolTipItem(String lblKey, String dsc)
        {
            lblKeyWord = lblKey;
            desc = dsc;
        }

        public String Label
        {
            get
            {
                return (CLanguage.getValue(lblKeyWord));
            }
        }

        public String Description
        {
            get
            {
                return (desc);
            }
        }
    }
}
