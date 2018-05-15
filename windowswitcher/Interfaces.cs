using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Automation;

namespace windowswitcher
{
    public interface IWindow
    {
        string Title { get; }

        string ProcessName { get; }
    }


    public interface IWindowSwitcher
    {
        void GetWindows(Action<IWindow> callback);

        void ActivateWindow(IWindow window_in);
    }

}
