using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Threading;

namespace windowswitcher.test
{
    [TestFixture]
    public class WindowSwitcherTests
    {
        [TestCase]
        public void TestGetWindowsUnderCurrentDesktop_GoodCase()
        {
            var Switcher = new WindowSwitcher();
            var collectiontoCheck = new List<IWindow>();
            Switcher.GetWindows(collectiontoCheck.Add);
            foreach (var item in collectiontoCheck)
            {
                Switcher.ActivateWindow(item);
                Thread.Sleep(50);
            }
        }
            


    }
}
