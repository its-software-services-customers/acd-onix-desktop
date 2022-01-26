using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.ClientCenter.Commons.UControls;
using Onix.ClientCenter.UControls;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaColumnCheckbox : CCriteriaColumnBase
    {
        private String cptKey = "";
        private String bdField = "";
        private HorizontalAlignment halgn;
        private double pctWd = 0.00;
        private Hashtable bindingVars = new Hashtable();
        private RoutedEventHandler checkBoxCheckedHandler = null;
        private RoutedEventHandler unCheckBoxCheckedHandler = null;

        public CCriteriaColumnCheckbox() : base()
        {
        }

        public CCriteriaColumnCheckbox(String columnName, String captionKey, String bindingField, double pctWidth, HorizontalAlignment alignment)
        {
            ColumnCaptionKey = captionKey;
            ColumnName = columnName;
            cptKey = captionKey;
            bdField = bindingField;
            halgn = alignment;
            pctWd = pctWidth;
        }

        public CCriteriaColumnCheckbox(String columnName, String captionKey, String bindingField, double pctWidth, HorizontalAlignment alignment, RoutedEventHandler checkedFunc, RoutedEventHandler unCheckedFunc)
        {
            cptKey = captionKey;
            bdField = bindingField;
            halgn = alignment;
            pctWd = pctWidth;
            checkBoxCheckedHandler = checkedFunc;
            unCheckBoxCheckedHandler = unCheckedFunc;
        }

        public override double PctWidth
        {
            get
            {
                return (pctWd);
            }
        }

        public void RegisterCheckboxBindingVariable(DependencyProperty dp, String fieldName)
        {
            bindingVars[fieldName] = dp;
        }

        public override GridViewColumn GetGridViewColumn(double actualWidth)
        {
            GridViewColumn v = new GridViewColumn();
            

            v.Width = actualWidth;
            
            DataTemplate template = new DataTemplate();

            //==== Stack Panel
            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            //====

            //==== Check Box
            Binding cbxSelfBinding = new Binding();
            cbxSelfBinding.Path = new PropertyPath("ObjSelf");

            Binding cbxCheckBinding = new Binding();
            cbxCheckBinding.Path = new PropertyPath("IsSelectedForDelete");

            FrameworkElementFactory cbxFactory = new FrameworkElementFactory(typeof(CheckBox));           
            cbxFactory.SetBinding(CheckBox.TagProperty, cbxSelfBinding);
            cbxFactory.SetBinding(CheckBox.IsCheckedProperty, cbxCheckBinding);
            cbxFactory.SetValue(CheckBox.MarginProperty, new Thickness(5,0,0,0));
            if (checkBoxCheckedHandler != null)
            {
                cbxFactory.AddHandler(CheckBox.UncheckedEvent, new RoutedEventHandler(unCheckBoxCheckedHandler));
                cbxFactory.AddHandler(CheckBox.CheckedEvent, new RoutedEventHandler(checkBoxCheckedHandler));
            }

            foreach (String nm in bindingVars.Keys)
            {
                DependencyProperty dp = (DependencyProperty) bindingVars[nm];

                Binding bd = new Binding();
                bd.Path = new PropertyPath(nm);

                cbxFactory.SetBinding(dp, bd);
            }

            //====

            //==== Image
            Binding tagBinding = new Binding();
            tagBinding.Path = new PropertyPath("ObjSelf");

            UToolTipText imgTT = new UToolTipText();
            Binding itemSourceBinding = new Binding();
            itemSourceBinding.Path = new PropertyPath("ToolTipItems");
            BindingOperations.SetBinding(imgTT, UToolTipText.ItemsSourceProperty, itemSourceBinding);


            FrameworkElementFactory imgFactory = new FrameworkElementFactory(typeof(Image));
            imgFactory.SetValue(Image.SourceProperty, Application.Current.TryFindResource("bmpInfo"));
            imgFactory.SetValue(Image.WidthProperty, 16.00);
            imgFactory.SetValue(Image.HeightProperty, 16.00);
            imgFactory.SetBinding(Image.TagProperty, tagBinding);

            imgFactory.SetValue(Image.ToolTipProperty, imgTT);
            //====

            stackPanelFactory.AppendChild(imgFactory);
            stackPanelFactory.AppendChild(cbxFactory);

            template.VisualTree = stackPanelFactory;
            v.CellTemplate = template;

            return (v);
        }
    }
}
