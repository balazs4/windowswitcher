using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using windowswitcher;

namespace windowswitcher.app
{
    public abstract class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AppViewModel : ViewModel
    {
        private readonly IDispatcherService dispatcher;
        private readonly IWindowSwitcher switcher;

        public AppViewModel(IDispatcherService _dispatcher, IWindowSwitcher _switcher)
        {
            dispatcher = _dispatcher;
            switcher = _switcher;
            Title = "windowswitcher";
            windows = new ObservableCollection<IWindow>(switcher.QueryWindows()); //TODO: make it async if the swithcer is not performant
            Activate = new LambdaCommand(window => switcher.ActivateWindow((IWindow)window), window => window is IWindow);
        }

        public string Title { get; private set; }

        private string searchText;
        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                RaisePropertyChangedEvent("SearchText");
            }
        }

        private ObservableCollection<IWindow> windows;
        public ObservableCollection<IWindow> Windows
        {
            get { return windows; }
            set
            {
                windows = value;
                RaisePropertyChangedEvent("Candidates");
            }
        }

        public ICommand Activate { get; private set; }
    }
}

