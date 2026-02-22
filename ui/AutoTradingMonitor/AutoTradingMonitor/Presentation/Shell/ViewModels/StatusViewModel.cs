using AutoTradingMonitor.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading; // Brush, Brushes

namespace AutoTradingMonitor.Presentation.ViewModels.Shell
{
    public class StatusViewModel : INotifyPropertyChanged
    {
        // ===== 시장 Open/Close 상태 =====
        private MarketState _krMarketState;
        public MarketState KRMarketState
        {
            get => _krMarketState;
            set
            {
                if (_krMarketState == value) return;
                _krMarketState = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(KRMarketBrush));
                OnPropertyChanged(nameof(KRMarketText));
            }
        }
        private MarketState _usMarketState;
        public MarketState USMarketState
        {
            get => _usMarketState;
            set
            {
                if (_usMarketState == value) return;
                _usMarketState = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(USMarketBrush));
                OnPropertyChanged(nameof(USMarketText));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        // ===== 시간 =====
        private string _localTime = "";
        public string LocalTime
        {
            get => _localTime;
            set
            {
                if (_localTime == value) return;
                _localTime = value;
                OnPropertyChanged();
            }
        }

        // ===== Latency =====
        private int _latency;
        public int Latency
        {
            get => _latency;
            set
            {
                if (_latency == value) return;
                _latency = value;
                OnPropertyChanged();
            }
        }
        private readonly DispatcherTimer _timer;
        public StatusViewModel()
        {
            KRMarketState = MarketState.Unknown;
            USMarketState = MarketState.Unknown;

            // 초기값
            LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Latency = 21;

            // 1초마다 시간 갱신
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (_, __) =>
            {
                LocalTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            };
            _timer.Start();
        }

        public string KRMarketText =>
            KRMarketState switch
            {
                MarketState.Open => "Open",
                MarketState.Closed => "Closed",
                _ => "—"
            };
        public string USMarketText =>
            USMarketState switch
            {
                MarketState.Open => "Open",
                MarketState.Closed => "Closed",
                _ => "—"
            };
        public Brush KRMarketBrush =>
                KRMarketState switch
                {
                    MarketState.Open => Brushes.LimeGreen,
                    MarketState.Closed => Brushes.IndianRed,
                    _ => Brushes.Gray
                };
        public Brush USMarketBrush =>
                USMarketState switch
                {
                    MarketState.Open => Brushes.LimeGreen,
                    MarketState.Closed => Brushes.IndianRed,
                    _ => Brushes.Gray
                };
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
}
