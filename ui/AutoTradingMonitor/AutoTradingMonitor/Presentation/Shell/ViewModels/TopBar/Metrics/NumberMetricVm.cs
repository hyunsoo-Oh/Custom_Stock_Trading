using AutoTradingMonitor.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoTradingMonitor.Presentation.ViewModels.TopBar.Metrics
{
    public sealed class NumberMetricVm : MetricVmBase
    {
        public Trend Trend { get; set; } = Trend.Neutral;
        // Title은 생성할 때만 설정
        public NumberMetricVm(string title)
        {
            Title = title ?? "";
        }

        private string _valueText = "0";
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

        private string _unit = "";
        public string Unit
        {
            get => _unit;
            set
            {
                if (_unit == value) return;
                _unit = value;
                OnPropertyChanged();
            }
        }

        private string _subText = "";
        public string SubText
        {
            get => _subText;
            set
            {
                if (_subText == value) return;
                _subText = value;
                OnPropertyChanged();
            }
        }
    }
}
