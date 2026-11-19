namespace CosinefulPalettes.Vendor.ExportForge.Utils
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class Debouncer
    {
        private int _version;

        /// <summary>
        /// Runs <paramref name="action"/> after <paramref name="delayMilliseconds"/>,
        /// unless another call to <see cref="Debounce"/> or <see cref="Cancel"/> happens first.
        /// </summary>
        /// <param name="action">Action to run.</param>
        /// <param name="delayMilliseconds">Delay before running the action.</param>
        public async Task Debounce(Action action, int delayMilliseconds)
        {
            var version = Interlocked.Increment(ref _version);

            await Task.Delay(delayMilliseconds);

            // A newer call superseded this one.
            if (version != Volatile.Read(ref _version))
            {
                return;
            }

            action();
        }

        /// <summary>
        /// Cancels the pending action, if any.
        /// </summary>
        public void Cancel()
        {
            Interlocked.Increment(ref _version);
        }
    }
}
