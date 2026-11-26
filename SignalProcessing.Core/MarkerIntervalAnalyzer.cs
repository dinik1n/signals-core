
namespace SignalProcessing.Core
{
    public sealed class MarkerIntervalAnalyzer
    {
        private float? _previousTime;

        public float? MinimumInterval { get; private set; }

        public float? MaximumInterval { get; private set; }

        public void OnMarkerDetected(
            object sender,
            MarkerDetectedEventArgs e)
        {
            if (e == null)
                return;

            if (_previousTime.HasValue)
            {
                float interval = e.Time - _previousTime.Value;

                if (!MinimumInterval.HasValue ||
                    interval < MinimumInterval.Value)
                {
                    MinimumInterval = interval;
                }

                if (!MaximumInterval.HasValue ||
                    interval > MaximumInterval.Value)
                {
                    MaximumInterval = interval;
                }
            }

            _previousTime = e.Time;
        }

        public void Reset()
        {
            _previousTime = null;
            MinimumInterval = null;
            MaximumInterval = null;
        }
    }
}
