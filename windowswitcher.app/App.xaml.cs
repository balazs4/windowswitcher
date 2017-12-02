using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Unity;

namespace windowswitcher.app
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer ioc = new UnityContainer();
            ioc.RegisterType<IDispatcherService, DispatcherService>();
            ioc.RegisterType<IWindowSwitcher, MockWindowSwticher>();
            ioc.RegisterInstance(Current.Dispatcher);
            var appwindow = new MainWindow
            {
                DataContext = ioc.Resolve<AppViewModel>()
            };
            appwindow.Show();
        }
    }

    public interface IDispatcherService
    {
        void Run(Action action);
    }

    public class DispatcherService : IDispatcherService
    {
        private Dispatcher dispatcher { get; }
        public DispatcherService(Dispatcher _dispatcher)
        {
            dispatcher = _dispatcher;
        }

        public void Run(Action action)
        {
            dispatcher.Invoke(action, DispatcherPriority.Normal);
        }
    }


    public class MockWindowSwticher : IWindowSwitcher
    {
        public void ActivateWindow(IWindow window_in) => MessageBox.Show("Activate " + window_in.Title);
        
        public IEnumerable<IWindow> GetWindowsUnderCurrentDesktop() => throw new NotImplementedException();

        public IEnumerable<IWindow> QueryWindows() => Enumerable.Range(0, 16).Select(MockWindow.Create);
    }

    public class MockWindow : IWindow
    {
        public static IWindow Create(int number) => new MockWindow { Title = number.ToString() };
        
        public string Title { get; private set; }

        public string Desktop { get; private set; }
    }
}
