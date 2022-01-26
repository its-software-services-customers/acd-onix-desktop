using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaColumnImageText : CCriteriaColumnBase
    {
        private String cptKey = "";
        private String bdField = "";
        private String ibdField = "";
        private HorizontalAlignment halgn;
        private double pctWd = 0.00;

        public CCriteriaColumnImageText() : base()
        {
        }

        public CCriteriaColumnImageText(String columnName, String captionKey, String txtBindingField, String imgBindingField, double pctWidth, HorizontalAlignment alignment)
        {
            ColumnCaptionKey = captionKey;
            ColumnName = columnName;
            cptKey = captionKey;
            bdField = txtBindingField;
            ibdField = imgBindingField;
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
        
        //public override GridViewColumn GetGridViewColumn(double actualWidth)
        //{
        //    GridViewColumn v = new GridViewColumn();
        //    v.Width = actualWidth;

        //    Binding cptBinding = new Binding();
        //    cptBinding.Source = CTextLabel.Instance;
        //    cptBinding.Path = new PropertyPath(cptKey);
        //    BindingOperations.SetBinding(v, GridViewColumn.HeaderProperty, cptBinding);

        //    Binding txtBinding = new Binding();
        //    txtBinding.Path = new PropertyPath(bdField);

        //    DataTemplate template = new DataTemplate();

        //    FrameworkElementFactory factory = new FrameworkElementFactory(typeof(TextBlock));
        //    factory.SetValue(TextBlock.HorizontalAlignmentProperty, halgn);
        //    factory.SetBinding(TextBlock.TextProperty, txtBinding);
        //    template.VisualTree = factory;

        //    v.CellTemplate = template;

        //    return (v);
        //}

        public override GridViewColumn GetGridViewColumn(double actualWidth)
        {
            GridViewColumn v = new GridViewColumn();

            v.Width = actualWidth;

            Binding cptBinding = new Binding();
            cptBinding.Source = CTextLabel.Instance;
            cptBinding.Path = new PropertyPath(cptKey);
            BindingOperations.SetBinding(v, GridViewColumn.HeaderProperty, cptBinding);

            DataTemplate template = new DataTemplate();

            //==== Stack Panel
            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            //====

            //==== Image
            Binding srcBinding = new Binding();
            srcBinding.Path = new PropertyPath(ibdField);

            FrameworkElementFactory imgFactory = new FrameworkElementFactory(typeof(Image));
            imgFactory.SetBinding(Image.SourceProperty, srcBinding);
            imgFactory.SetValue(Image.WidthProperty, 16.00);
            imgFactory.SetValue(Image.HeightProperty, 16.00);
            //====

            stackPanelFactory.AppendChild(imgFactory);

            if (!bdField.Equals(""))
            {
                //==== Text Block
                Binding txtBinding = new Binding();
                txtBinding.Path = new PropertyPath(bdField);

                FrameworkElementFactory tblckFactory = new FrameworkElementFactory(typeof(TextBlock));
                tblckFactory.SetBinding(TextBlock.TextProperty, txtBinding);
                tblckFactory.SetValue(CheckBox.MarginProperty, new Thickness(10, 0, 0, 0));

                stackPanelFactory.AppendChild(tblckFactory);
                //====
            }

            template.VisualTree = stackPanelFactory;
            v.CellTemplate = template;

            return (v);
        }
    }
}
