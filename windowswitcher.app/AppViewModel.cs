using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Linq;
using windowswitcher;
using System;
using System.Reflection;

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

        private string GetVersion()
        {
            try
            {
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Revision}";
            }
            catch (Exception)
            {
                return "0.0.0";
            }
        }

        public AppViewModel(IWindowSwitcher _switcher)
        {

            switcher = _switcher;
            Title = $"windowswitcher v{GetVersion()}";
            windows = new ObservableCollection<IWindow>(switcher.GetWindows());
            Activate = new LambdaCommand(_ =>
            {
                switcher.ActivateWindow(SelectedWindow);
                App.Current.Shutdown();
            },_ => SelectedWindow != null);

            OpenLink = new LambdaCommand(_ =>
            {
                System.Diagnostics.Process.Start("https://github.com/balazs4/windowswitcher/");
            });

            SelectPrev = new LambdaCommand(_ =>
            {
                if(!Windows.MoveCurrentToPrevious())
                {
                    //hacky tracky workaround works :D
                    Windows.MoveCurrentToNext();
                }
                RaisePropertyChangedEvent("Windows");
            });

            SelectNext = new LambdaCommand(_ =>
             {
                 if (!Windows.MoveCurrentToNext())
                 {
                     //hacky tracky workaround works :D
                     Windows.MoveCurrentToPrevious();
                 }
                 RaisePropertyChangedEvent("Windows");
             }
            );
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
                    var terms = SearchText.Split(' ').Select(x => x.ToLower());
                    var title = window.Title.ToLower();
                    var processname = window.ProcessName.ToLower();
                    return terms.All(x => title.Contains(x) || processname.Contains(x));
                };
                SelectedWindow = filtering.CurrentItem as IWindow;
                return filtering;
            }
        }


        public ICommand SelectNext { get; private set; }

        public ICommand SelectPrev { get; private set; }

        public ICommand Activate { get; private set; }
        public ICommand OpenLink { get; private set; }
    }
}

