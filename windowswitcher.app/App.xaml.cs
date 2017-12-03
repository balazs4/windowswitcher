using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;


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
            var appwindow = new MainWindow
            {
                DataContext = new AppViewModel(new WindowSwitcher())
            };
            appwindow.Show();
        }
    }
    
    public class MockWindowSwitcher : IWindowSwitcher
    {
        public void ActivateWindow(IWindow window_in) => MessageBox.Show("Activate " + window_in.Title);
    
        public IEnumerable<IWindow> GetWindows() => Enumerable.Range(0, 123).Select(MockWindow.Create);
    }

    public class MockWindow : IWindow
    {
        private static string[] text = new[] { "Lorem", "Ipsum", "foo", "Bar" };

        public static IWindow Create(int number) => new MockWindow { Title = $"{text[number % text.Length]} {number}" };

        public string Title { get; private set; }
    }
}
