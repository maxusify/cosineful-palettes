namespace CosinefulPalettes.Utils.ExportForge
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Debouncer : IDisposable
    {
        public int DelayMilliseconds { get; set; }

        private CancellationTokenSource? _cancelTokenSource;

        public async Task Debounce(Action action)
        {
            if (_cancelTokenSource is not null)
            {
               await _cancelTokenSource.CancelAsync();
               _cancelTokenSource.Dispose();
            }

            _cancelTokenSource = new CancellationTokenSource();

            try
            {
                await Task.Delay(DelayMilliseconds, _cancelTokenSource.Token);
                action();
            }
            catch (OperationCanceledException)
            {
                // Cancellation is expected. There is nothing to do.
            }
        }

        public void Dispose()
        {
            _cancelTokenSource?.Dispose();
            _cancelTokenSource = null;
        }
    }
}
