using AutoTradingMonitor.Presentation.Models;
using AutoTradingMonitor.Presentation.ViewModels.Shell;
using AutoTradingMonitor.Presentation.ViewModels.TopBar;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;

namespace AutoTradingMonitor.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public NavViewModel Nav { get; }
        public StatusViewModel Status { get; }
        public TopBarViewModel TopBar { get; } = new();
        public MainViewModel()
        {
            Nav = new NavViewModel();
            Status = new StatusViewModel();

            Status.PropertyChanged += (_, e) =>
            {
                if (!string.IsNullOrWhiteSpace(e.PropertyName))
                {
                    OnPropertyChanged(e.PropertyName);
                }
            };
        }

        // Navigation 바인딩 호환용 프록시
        public ObservableCollection<NavItem> NavItems => Nav.NavItems;
        public NavItem? SelectedNavItem
        {
            get => Nav.SelectedNavItem;
            set
            {
                if (Nav.SelectedNavItem == value) return;
                Nav.SelectedNavItem = value;
                OnPropertyChanged();
            }
        }

        // Status 바인딩 호환용 프록시
        public string KRMarketText => Status.KRMarketText;
        public string USMarketText => Status.USMarketText;
        public Brush KRMarketBrush => Status.KRMarketBrush;
        public Brush USMarketBrush => Status.USMarketBrush;
        public string LocalTime => Status.LocalTime;
        public int Latency => Status.Latency;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
