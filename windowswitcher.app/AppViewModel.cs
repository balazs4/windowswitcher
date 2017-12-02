using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
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
        private readonly IWindowSwitcher switcher;

        public AppViewModel(IWindowSwitcher _switcher)
        {
            switcher = _switcher;
            Title = "windowswitcher";
            Activate = new LambdaCommand(_ =>
            {
                var window = SelectedWindow == null ? windows[0] : SelectedWindow;
                switcher.ActivateWindow(window);
                App.Current.Shutdown(0);
            }, _ => windows.Count > 0 || SelectedWindow != null);
            windows = new ObservableCollection<IWindow>(switcher.GetWindows()); //TODO: make it async if the swithcer is not performant
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
                RaisePropertyChangedEvent("Windows");
            }
        }

        private IWindow selectedWindow;
        public IWindow SelectedWindow
        {
            get { return selectedWindow; }
            set
            {
                selectedWindow = value;
                RaisePropertyChangedEvent("SelectedWindow");
            }
        }


        private ObservableCollection<IWindow> windows;
        public ICollectionView Windows
        {
            get
            {
                var filtering = CollectionViewSource.GetDefaultView(windows);
                filtering.Filter = item =>
                {
                    var window = item as IWindow;
                    if (window == null) return true;
                    if (string.IsNullOrWhiteSpace(SearchText)) return true;
                    if (string.IsNullOrWhiteSpace(window.Title)) return true;
                    return window.Title.ToLower().Contains(SearchText.ToLower());
                };
                return filtering;
            }
        }

        public ICommand Activate { get; private set; }
    }
}

