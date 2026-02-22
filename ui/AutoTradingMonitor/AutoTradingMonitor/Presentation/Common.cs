//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace AutoTradingMonitor.Presentation
//{
//    public sealed class AsyncCommand : IAsyncCommand
//    {
//        private readonly Func<object?, Task> _execute;
//        private readonly Func<object?, bool>? _canExecute;

//        public bool IsRunning { get; private set; }

//        public event EventHandler? CanExecuteChanged;

//        private AsyncCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute)
//        {
//            _execute = execute;
//            _canExecute = canExecute;
//        }

//        /*
//            Create
//            - parameter 없는 커맨드 생성용 편의 메서드
//        */
//        public static IAsyncCommand Create(Func<Task> execute, Func<bool>? canExecute = null)
//        {
//            return new AsyncCommand(
//                _ => execute(),
//                canExecute is null ? null : (_ => canExecute()));
//        }

//        /*
//            Create<T>
//            - parameter 있는 커맨드 생성용(필요할 때 사용)
//        */
//        public static IAsyncCommand Create(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
//        {
//            return new AsyncCommand(execute, canExecute);
//        }

//        public bool CanExecute(object? parameter)
//        {
//            if (IsRunning) return false;
//            return _canExecute?.Invoke(parameter) ?? true;
//        }

//        public async void Execute(object? parameter)
//        {
//            // WPF ICommand 경유 실행
//            await ExecuteAsync(parameter);
//        }

//        public async Task ExecuteAsync(object? parameter = null)
//        {
//            if (!CanExecute(parameter)) return;

//            try
//            {
//                IsRunning = true;
//                RaiseCanExecuteChanged();

//                await _execute(parameter);
//            }
//            finally
//            {
//                IsRunning = false;
//                RaiseCanExecuteChanged();
//            }
//        }

//        /*
//            RaiseCanExecuteChanged
//            - UI가 버튼 활성/비활성 갱신하도록 이벤트 호출
//        */
//        public void RaiseCanExecuteChanged()
//        {
//            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
//        }
//    }
//}
