using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Zoo
{
    public class EarthLiveTicker : ILiveTicker
    {
        private static readonly EarthLiveTicker LiveTiker = new EarthLiveTicker();

        private readonly object _syncObj = new object();
        private readonly IList<ITickListener> _listeners = new List<ITickListener>();
        public const int Interval = 10;

        private EarthLiveTicker()
        {
            var tickTimer = new Timer(Interval);
            tickTimer.Elapsed += (o, e) => RunClock();
            tickTimer.AutoReset = true;
            tickTimer.Start();
        }

        public static ILiveTicker LiveTicker => LiveTiker;

        public void Subscribe(ITickListener tickListener)
        {
            lock (_syncObj)
            {
                if (tickListener != null)
                    _listeners.Add(tickListener);
            }
        }

        public void Unsubscribe(ITickListener tickListener)
        {
            lock (_syncObj)
            {
                _listeners.Remove(tickListener);
            }
        }

        private void RunClock()
        {
            Task.Run(() =>
            {
                lock (_syncObj)
                {
                    var snapshot = _listeners.ToList();
                    foreach (var tickListener in snapshot)
                    {
                        tickListener.OnTick();
                    }
                }
            });
        }
    }
}