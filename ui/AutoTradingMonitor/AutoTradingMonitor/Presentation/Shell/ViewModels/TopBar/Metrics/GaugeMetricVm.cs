using AutoTradingMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTradingMonitor.Presentation.ViewModels.TopBar.Metrics
{
    public sealed class GaugeMetricVm : MetricVmBase
    {
        public Trend Trend { get; set; } = Trend.Neutral;
        // Title은 생성할 때만 설정
        public GaugeMetricVm(string title)
        {
            Title = title ?? "";
        }

        private double _percent;
        public double Percent
        {
            get => _percent;
            set
            {
                if (_percent.Equals(value)) return;
                _percent = value;
                OnPropertyChanged();
            }
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
    }
}
