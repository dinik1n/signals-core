
namespace SignalProcessing.Core
{
    public sealed class MarkerCounter
    {
        public int UpCount { get; private set; }
        public int DownCount { get; private set; }

        public void OnMarkerDetected(
            object sender,
            MarkerDetectedEventArgs e)
        {
            if (e == null)
                return;

            if (e.Marker.Direction == CrossingDirection.Up)
                UpCount++;
            else
                DownCount++;
        }

        public void Reset()
        {
            UpCount = 0;
            DownCount = 0;
        }
    }
}
