namespace MauiBase.BackgroundServices
{
    internal class CounterBackgroundService : IPeriodicTask
    {
        public TimeSpan Interval => TimeSpan.FromSeconds(1);

        public Task<bool> StartJob()
        {
            if (Interval >= TimeSpan.FromSeconds(60))
                return Task.FromResult(false);

            App.IntervalCounter += 1;

            return Task.FromResult(true);
        }
    }
}
