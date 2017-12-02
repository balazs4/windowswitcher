using System;
using System.Collections.Generic;
using System.Windows.Automation;

namespace windowswitcher
{
    public interface IWindow
    {
        string Title { get; }
    }


    public interface IWindowSwitcher
    {
        IEnumerable<IWindow> GetWindows();

        void ActivateWindow(IWindow window_in);

    }

}
