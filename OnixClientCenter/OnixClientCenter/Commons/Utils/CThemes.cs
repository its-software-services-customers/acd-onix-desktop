using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Onix.ClientCenter
{
    class CThemes : INotifyPropertyChanged
    {
        private static CThemes obj = new CThemes("BubbleCreme");
        private static String theme = "BubbleCreme";

        public event PropertyChangedEventHandler PropertyChanged;

        public CThemes(String thm)
        {

        }

        public static CThemes Instance
        {
            get
            {
                return (obj);
            }
        }

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static void SetTheme(String tme)
        {
            theme = tme;

            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                obj.NotifyPropertyChanged(prop.Name);
            }
        }

        public static String GetTheme()
        {
            return (theme);
        }

        public String ThemeName
        {
            get
            {
                return (theme);
            }

            set
            {
                theme = value;
            }
        }
        

    }
}
