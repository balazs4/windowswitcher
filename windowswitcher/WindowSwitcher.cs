using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace windowswitcher
{
    internal class Window : IWindow
    {
        public string Title { get; internal set; }

        public string ProcessName { get; internal set; }

        public int WindowHandle { get; internal set; }
    }

    public class WindowSwitcher : IWindowSwitcher
    {
        private const string WindowSwitcherLogDomain = "WindowSwitcher c Logging:";
        public void ActivateWindow(IWindow window_in)
        {
            if (window_in is Window && ((Window)window_in).WindowHandle != 0)
            {
                var element = AutomationElement.RootElement.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.NativeWindowHandleProperty, ((Window)window_in).WindowHandle));
                var foo = (element.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern);
                var foostate = foo.Current.WindowVisualState;
                foo.SetWindowVisualState(foostate == WindowVisualState.Minimized ? WindowVisualState.Normal : foostate);
                return;

            }
            //Fallback for interface was coming from anyhwere where  there was no windowhandle available
            Condition searchcondition = new AndCondition(
              new PropertyCondition(AutomationElement.NameProperty, window_in.Title),
              new PropertyCondition(AutomationElement.IsWindowPatternAvailableProperty, true),
              new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            var FoundWindow = AutomationElement.RootElement.FindFirst(TreeScope.Children, searchcondition);
            var bar = (FoundWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern);
            var barstate = bar.Current.WindowVisualState;
            bar.SetWindowVisualState(barstate == WindowVisualState.Minimized ? WindowVisualState.Normal : barstate);

        }        /// <summary>
                 /// Only Searches under the current Desktop
                 /// </summary>
                 /// <returns></returns>
        public IEnumerable<IWindow> GetWindows()
        {

            var AllWindows = new List<IWindow>();

            //todo check if AutomationElement.RootElement is the destop1???? and if the name Property is what we need :D
            var desktopname = AutomationElement.RootElement.Current.Name;
            Console.WriteLine($"{WindowSwitcherLogDomain} DesktopName {desktopname}");
            var SearchWindowsCondition = new AndCondition(
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window),
                new PropertyCondition(AutomationElement.IsWindowPatternAvailableProperty, true));

            var FoundWindows = AutomationElement.RootElement.FindAll(TreeScope.Children, SearchWindowsCondition);
            //todo check if the Childrens are the desktops or the windows.
            foreach (AutomationElement aEm in FoundWindows)
            {
                Console.WriteLine($"{WindowSwitcherLogDomain} FoundWindow {aEm}");
                var current = aEm.Current;
                var process = Process.GetProcessById(current.ProcessId);
                var window = new Window
                {
                    WindowHandle = current.NativeWindowHandle,
                    ProcessName = process.ProcessName,
                    Title = current.Name
                };
                AllWindows.Add(window);
            }

            return AllWindows;
        }
    }
}
