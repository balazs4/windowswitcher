using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace windowswitcher
{

    public class Window : IWindow
    {

        internal Window(string title) : this(title, string.Empty) { }

        internal Window(string windowTitle, string desktopTitle):this(windowTitle, desktopTitle,0) { }

        internal Window(string windowTitle, string desktopTitle, int windowHandle)
        {
            myWindowTitle = windowTitle;
            myDesktopTitle = desktopTitle;
            myWindowHandle = windowHandle;
        }

        private string myWindowTitle = String.Empty;
        private string myDesktopTitle = String.Empty;
        private int myWindowHandle = 0;

        public string Title => myWindowTitle;

        public int WindowHandle => myWindowHandle; 
    }

    public class WindowSwitcher : IWindowSwitcher
    {
        private const string WindowSwitcherLogDomain= "WindowSwitcher c Logging:";
        public void ActivateWindow(IWindow window_in)
        {
            if (window_in is Window && ((Window)window_in).WindowHandle != 0)
            {
                var element=AutomationElement.RootElement.FindFirst(TreeScope.Children, 
                    new PropertyCondition(AutomationElement.NativeWindowHandleProperty, ((Window)window_in).WindowHandle));
                (element.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern).SetWindowVisualState(WindowVisualState.Normal);
                return;

            }
            //Fallback for interface was coming from anyhwere where  there was no windowhandle available
            Condition searchcondition = new AndCondition(
              new PropertyCondition(AutomationElement.NameProperty, window_in.Title),
              new PropertyCondition(AutomationElement.IsWindowPatternAvailableProperty, true),
              new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Window));
            var FoundWindow= AutomationElement.RootElement.FindFirst(TreeScope.Children, searchcondition);
            (FoundWindow.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern)
                .SetWindowVisualState(WindowVisualState.Normal);

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

                AllWindows.Add(new Window(aEm.Current.Name, String.Empty, aEm.Current.NativeWindowHandle));
            }

            return AllWindows;
        }
    }
}
