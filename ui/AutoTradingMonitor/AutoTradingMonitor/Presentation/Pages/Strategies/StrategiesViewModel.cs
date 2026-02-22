//using System;
//using System.Collections.Generic;
//using System.Text;

///*
//    목표:
//    - UI에서 직접 실행 로직을 갖지 않고, UseCase를 호출하는 얇은 어댑터 역할
//    - Start/Stop/Apply 같은 동작은 비동기로 처리하고 중복 실행을 막는다(IsBusy)
//*/

//namespace AutoTradingMonitor.Presentation.ViewModels.Pages
//{
//    public sealed class StrategiesPageViewModel
//    {
//        // ===== 바인딩용 상태 =====
//        public string SearchText { get; set; } = "";
//        public bool FilterRunning { get; set; }
//        public bool FilterStopped { get; set; }
//        public bool FilterError { get; set; }

//        public IReadOnlyList<StrategyRowVm> Strategies { get; }
//        public StrategyDetailVm? SelectedStrategy { get; set; }

//        public IReadOnlyList<string> LogLevels { get; } = new[] { "All", "Info", "Warn", "Error" };
//        public string SelectedLogLevel { get; set; } = "All";
//        public IReadOnlyList<string> LogLines { get; }

//        // ===== 명령(테스트에서는 Command.Execute()로 검증) =====
//        public IAsyncCommand AddStrategyCommand { get; }
//        public IAsyncCommand StartStrategyCommand { get; }
//        public IAsyncCommand StopStrategyCommand { get; }
//        public IAsyncCommand RestartStrategyCommand { get; }
//        public IAsyncCommand ApplyParamsCommand { get; }
//        public IAsyncCommand ClearLogCommand { get; }

//        // ===== 외부 의존성은 UseCase로만 주입(브로커/DB 직접 X) =====
//        private readonly IStartStrategyUseCase _start;
//        private readonly IStopStrategyUseCase _stop;
//        private readonly IApplyParametersUseCase _apply;

//        public StrategiesPageViewModel(
//            IStartStrategyUseCase start,
//            IStopStrategyUseCase stop,
//            IApplyParametersUseCase apply)
//        {
//            _start = start;
//            _stop = stop;
//            _apply = apply;

//            StartStrategyCommand = AsyncCommand.Create(async () =>
//            {
//                if (SelectedStrategy is null) return;
//                await _start.ExecuteAsync(SelectedStrategy.Id);
//            });

//            StopStrategyCommand = AsyncCommand.Create(async () =>
//            {
//                if (SelectedStrategy is null) return;
//                await _stop.ExecuteAsync(SelectedStrategy.Id);
//            });

//            ApplyParamsCommand = AsyncCommand.Create(async () =>
//            {
//                if (SelectedStrategy is null) return;
//                await _apply.ExecuteAsync(SelectedStrategy.Id, SelectedStrategy.ToParameterSet());
//            });

//            // 나머지도 동일 패턴으로 구성
//        }
//    }