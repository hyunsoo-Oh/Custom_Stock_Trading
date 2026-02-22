using AutoTradingMonitor.Presentation.ViewModels.TopBar.Metrics;
using AutoTradingMonitor.Presentation.Views.TopBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace AutoTradingMonitor.Presentation.ViewModels.TopBar
{
    // TopBarWidget을 포함하는 부모 View에서:
    // <views:TopBarWidget DataContext = "{Binding TopBar}" />
    // 부모 ViewModel에서:
    // public TopBarViewModel TopBar { get; } = new();
    public sealed class TopBarViewModel
    {
        public ObservableCollection<IMetricVm> Metrics { get; } = new();

        public TopBarViewModel()
        {
            // 숫자 카드 (지수/자금/손익)
            Metrics.Add(new NumberMetricVm("KOSPI")
            {
                ValueText = "0.00",
                Unit = "",
                SubText = "▲ 0.00 (0.00%)"
            });

            Metrics.Add(new NumberMetricVm("KOSDAQ")
            {
                ValueText = "0.00",
                Unit = "",
                SubText = "▼ 0.00 (0.00%)"
            });

            Metrics.Add(new NumberMetricVm("자금")
            {
                ValueText = "₩ 0",
                Unit = "",
                SubText = "Available"
            });

            Metrics.Add(new NumberMetricVm("손익")
            {
                ValueText = "₩ 0",
                Unit = "",
                SubText = "Today"
            });

            //// 스파크라인 카드 (Network / CPU-MEM)
            //var net = new SparklineMetricVm("Network") { ValueText = "0 KB/s" };
            //SeedSeries(net, 24);
            //Metrics.Add(net);

            //var cpu = new SparklineMetricVm("CPU / MEM") { ValueText = "0% / 0%" };
            //SeedSeries(cpu, 24);
            //Metrics.Add(cpu);

            // 게이지 카드 (TR)
            Metrics.Add(new GaugeMetricVm("TR")
            {
                Percent = 30,
                ValueText = "30%"
            });
        }

        private static void SeedSeries(SparklineMetricVm vm, int count)
        {
            vm.Series.Clear();
            for (int i = 0; i < count; i++)
                vm.Series.Add(0); // 더미
        }
    }
}
