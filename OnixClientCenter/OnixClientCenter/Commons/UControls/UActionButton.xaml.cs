using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Onix.ClientCenter.Commons.CriteriaConfig;
using Onix.Client.Helper;

namespace Onix.ClientCenter.Commons.UControls
{
    public partial class UActionButton : UserControl
    {
        public static readonly DependencyProperty CustomContextMenuProperty =
        DependencyProperty.Register("CustomContextMenu", typeof(ArrayList), typeof(UActionButton),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnContextMenuChanged));

        //private ContextMenu btnContext = new ContextMenu();

        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent(
            "ClickEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UActionButton));

        public ArrayList CustomContextMenu
        {
            get { return (ArrayList)GetValue(CustomContextMenuProperty); }
            set { SetValue(CustomContextMenuProperty, value); }
        }

        public UActionButton()
        {
            InitializeComponent();            
        }

        private static void OnContextMenuChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            ContextMenu btnContext = new ContextMenu();

            ArrayList contextMenu = (ArrayList)e.NewValue;
            btnContext.Items.Clear();

            int cnt = 0;
            foreach (CCriteriaContextMenu ct in contextMenu)
            {
                cnt++;

                MenuItem mi = new MenuItem();
                mi.Name = ct.Name;
                mi.Click += (RoutedEventHandler)ct.ClickHandler;

                Binding menuItemHeaderBinding = new Binding();
                menuItemHeaderBinding.Source = CTextLabel.Instance;
                menuItemHeaderBinding.Path = new PropertyPath(ct.Caption);
                BindingOperations.SetBinding(mi, MenuItem.HeaderProperty, menuItemHeaderBinding);

                btnContext.Items.Add(mi);

                if (cnt < contextMenu.Count)
                {
                    Separator sp = new Separator();
                    btnContext.Items.Add(sp);
                }
            }

            UActionButton ctrl = obj as UActionButton;
            ctrl.cmdAction.ContextMenu = btnContext;
        }

        private void cmdAction_Click(object sender, RoutedEventArgs e)
        {
            cmdAction.ContextMenu.IsOpen = true;
            if (ClickEvent != null)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(UActionButton.ClickEvent);
                newEventArgs.Source = this;
                RaiseEvent(newEventArgs);
            }
        }
    }
}
