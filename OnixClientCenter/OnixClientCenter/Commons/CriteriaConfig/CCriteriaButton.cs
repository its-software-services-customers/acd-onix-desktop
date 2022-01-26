using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.ClientCenter.UControls;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.CriteriaConfig
{
    public class CCriteriaButton : CCriteriaColumnBase
    {
        private String cptKey = "";
        private ArrayList contextMenu = null;
        private RoutedEventHandler buttonClickHandler = null;

        public CCriteriaButton() : base()
        {
        }

        public CCriteriaButton(String captionKey, Boolean isEnable, ArrayList ctxMenu, RoutedEventHandler clickHandler)
        {
            cptKey = captionKey;
            contextMenu = ctxMenu;
            buttonClickHandler = clickHandler;
        }

        public Button GetButton()
        {
            Button b = new Button();

            b.Width = 80;
            ContextMenuService.SetIsEnabled(b, false);

            Binding buttonBinding = new Binding();
            buttonBinding.Source = CTextLabel.Instance;
            buttonBinding.Path = new PropertyPath(cptKey);
            BindingOperations.SetBinding(b, Button.ContentProperty, buttonBinding);
           
            if (contextMenu.Count == 1)
            {
                CCriteriaContextMenu ct = (CCriteriaContextMenu)contextMenu[0];
                b.Click += (RoutedEventHandler)ct.ClickHandler;
            }
            else
            {
                if (buttonClickHandler != null)
                {
                    b.Click += buttonClickHandler;
                }
                ContextMenu btnContext = new ContextMenu();

                int cnt = 0;
                int prevGroup = -1;
                foreach (CCriteriaContextMenu ct in contextMenu)
                {
                    if (cnt == 0)
                    {
                        prevGroup = ct.Group;
                    }
                    else if ((cnt > 0) && (prevGroup != ct.Group))
                    {
                        Separator sp = new Separator();
                        btnContext.Items.Add(sp);

                        prevGroup = ct.Group;
                    }

                    cnt++;

                    MenuItem mi = new MenuItem();
                    mi.Name = ct.Name;
                    mi.Click += (RoutedEventHandler)ct.ClickHandler;

                    Binding menuItemHeaderBinding = new Binding();
                    menuItemHeaderBinding.Source = CTextLabel.Instance;
                    menuItemHeaderBinding.Path = new PropertyPath(ct.Caption);
                    BindingOperations.SetBinding(mi, MenuItem.HeaderProperty, menuItemHeaderBinding);

                    btnContext.Items.Add(mi);
                }

                if (cnt > 0)
                {
                    b.ContextMenu = btnContext;
                }
            }

            return (b);
        }
    }
}
