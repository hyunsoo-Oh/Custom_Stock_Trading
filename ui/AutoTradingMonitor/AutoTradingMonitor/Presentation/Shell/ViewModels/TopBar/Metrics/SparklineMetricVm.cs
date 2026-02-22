using AutoTradingMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoTradingMonitor.Presentation.ViewModels.TopBar.Metrics
{
    public sealed class SparklineMetricVm : MetricVmBase
    {
        public Trend Trend { get; set; } = Trend.Neutral;
        // Title은 생성할 때만 설정
        public SparklineMetricVm(string title)
        {
            Title = title ?? "";
        }

        private string _valueText = "";
        public string ValueText
        {
            get => _valueText;
            set
            {
                if (_valueText == value) return;
                _valueText = value;
                OnPropertyChanged();
            }
        }

        // 나중에 Polyline으로 그릴 데이터
        public ObservableCollection<double> Series { get; } = new();
    }
}
