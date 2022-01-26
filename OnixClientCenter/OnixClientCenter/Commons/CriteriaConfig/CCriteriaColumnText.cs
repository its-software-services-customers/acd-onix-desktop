using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaColumnText : CCriteriaColumnBase
    {
        private String cptKey = "";
        private String bdField = "";
        private HorizontalAlignment halgn;
        private double pctWd = 0.00;

        public CCriteriaColumnText() : base()
        {
        }

        public CCriteriaColumnText(String columnName, String captionKey, String bindingField, double pctWidth, HorizontalAlignment alignment)
        {
            ColumnCaptionKey = captionKey;
            ColumnName = columnName;
            cptKey = captionKey;
            bdField = bindingField;
            halgn = alignment;
            pctWd = pctWidth;
        }

        public override double PctWidth
        {
            get
            {
                return (pctWd);
            }
        }

        public override GridViewColumn GetGridViewColumn(double actualWidth)
        {
            GridViewColumn v = new GridViewColumn();
            v.Width = actualWidth;
            
            ((INotifyPropertyChanged) v).PropertyChanged += GetHandler();

            Binding cptBinding = new Binding();
            cptBinding.Source = CTextLabel.Instance;
            cptBinding.Path = new PropertyPath(cptKey);
            BindingOperations.SetBinding(v, GridViewColumn.HeaderProperty, cptBinding);

            Binding txtBinding = new Binding();
            txtBinding.Path = new PropertyPath(bdField);

            Binding colorBinding = new Binding();
            colorBinding.Path = new PropertyPath(BindingColorField);

            DataTemplate template = new DataTemplate();

            FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.HorizontalAlignmentProperty, halgn);
            factory.SetBinding(TextBlock.TextProperty, txtBinding);
            if (!BindingColorField.Equals(""))
            {
                factory.SetBinding(TextBlock.ForegroundProperty, colorBinding);
            }
            template.VisualTree = factory;

            v.CellTemplate = template;

            return (v);
        }
    }
}
