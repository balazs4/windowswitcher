using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace windowswitcher.app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.txtSearch.Focus();
            // Didn't find a smoother solution for directly handling a mouseclick on a listviewitem
            //ToDo: Check if there is a solution which is databound by using Itemstyles? :/
            EventManager.RegisterClassHandler(typeof(ListBoxItem),
            ListBoxItem.MouseLeftButtonDownEvent,
            new RoutedEventHandler(this.MouseLeftButtonDownRoutedEvent));

        }

        private void MouseLeftButtonDownRoutedEvent(object sender, RoutedEventArgs e)
        {
            //ToDo: Check if try / catch is needed, pretty hard casting stuff.
                var IWasClicked = (sender as ListViewItem);
                var IWasTheWindowClicked = IWasClicked.Content as IWindow;
                (DataContext as AppViewModel).SelectedWindow = IWasTheWindowClicked;
                (DataContext as AppViewModel).Activate.Execute(IWasClicked);
        }

        private void OnCloseExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
