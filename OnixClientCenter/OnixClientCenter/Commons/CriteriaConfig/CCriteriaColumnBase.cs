using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public interface ICriteriaColumn
    {
        GridViewColumn GetGridViewColumn(double actualWidth);
    }

    public class CCriteriaColumnBase : ICriteriaColumn
    {
        private PropertyChangedEventHandler handler = null;
        private Boolean sortAble = false;
        private String columnName = "";
        private String columnCaptionKey = "";
        private String colorBindingField = "";

        public virtual GridViewColumn GetGridViewColumn(double actualWidth)
        {
            return (null);
        }

        public void AddCriteriaColumnPropertyChangedHandler(PropertyChangedEventHandler hdlr)
        {
            handler = hdlr;
        }

        public PropertyChangedEventHandler GetHandler()
        {
            return (handler);
        }

        public virtual String BindingColorField
        {
            get
            {
                return (colorBindingField);
            }

            set
            {
                colorBindingField = value;
            }
        }

        public virtual double PctWidth
        {
            get
            {
                return (0.00);
            }
        }

        public virtual Boolean Sortable
        {
            get
            {
                return (sortAble);
            }

            set
            {
                sortAble = value;
            }
        }

        public virtual String ColumnName
        {
            get
            {
                return (columnName);
            }

            set
            {
                columnName = value;
            }
        }

        public virtual String ColumnCaptionKey
        {
            get
            {
                return (columnCaptionKey);
            }

            set
            {
                columnCaptionKey = value;
            }
        }
    }
}
