using System;
using System.Collections.Generic;

namespace windowswitcher
{
    public interface IWindow
    {
        string Title { get; }

        string Desktop { get; }
    }


    public interface IWindowSwitcher
    {
        IEnumerable<IWindow> GetWindowsUnderCurrentDesktop();

        IEnumerable<IWindow> QueryWindows();

        void ActivateWindow(IWindow window_in);

    }

}
