using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.ClientCenter.Commons.UControls;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaColumnButton : CCriteriaColumnBase
    {
        private String cptKey = "";
        //private String bdField = "";
        private HorizontalAlignment halgn;
        private double pctWd = 0.00;
        private ArrayList contextMenu = new ArrayList();
        private RoutedEventHandler buttonClickHandler = null;
        private Boolean btnEnable = true;

        public CCriteriaColumnButton() : base()
        {
        }

        public CCriteriaColumnButton(String columnName, String captionKey, ArrayList ctxMenu, double pctWidth, HorizontalAlignment alignment, RoutedEventHandler clickFunc)
        {
            cptKey = captionKey;
            halgn = alignment;
            pctWd = pctWidth;
            //contextMenu = ctxMenu;
            copyToInternal(ctxMenu);
            buttonClickHandler = clickFunc;
        }

        private void copyToInternal(ArrayList ctxMenu)
        {
            foreach (CCriteriaContextMenu mnu in ctxMenu)
            {
                contextMenu.Add(mnu);
            }
        }

        public void SetButtonEnable(Boolean en)
        {
            btnEnable = en;
        }

        public override double PctWidth
        {
            get
            {
                return (pctWd);
            }
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            buttonClickHandler(sender, e);
        }

        public override GridViewColumn GetGridViewColumn(double actualWidth)
        {
            GridViewColumn v = new GridViewColumn();

            v.Width = actualWidth;

            DataTemplate template = new DataTemplate();

            //==== Stack Panel
            Binding btnSelfBinding = new Binding();
            btnSelfBinding.Path = new PropertyPath("ObjSelf");

            FrameworkElementFactory bottonFactory = new FrameworkElementFactory(typeof(UActionButton));
            bottonFactory.SetValue(UActionButton.CustomContextMenuProperty, contextMenu);
            bottonFactory.SetValue(UActionButton.TagProperty, btnSelfBinding);
            bottonFactory.SetValue(UActionButton.IsEnabledProperty, btnEnable);
            
            bottonFactory.AddHandler(UActionButton.ClickEvent, new RoutedEventHandler(cmdAction_Click));
            //====

            template.VisualTree = bottonFactory;
            v.CellTemplate = template;

            return (v);
        }
    }
}
