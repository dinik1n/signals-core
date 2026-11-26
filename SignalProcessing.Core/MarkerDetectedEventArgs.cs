
using System;

namespace SignalProcessing.Core
{
    public sealed class MarkerDetectedEventArgs : EventArgs
    {

        public SingleChannelSignal Signal { get; }

        public SignalMarker Marker { get; }
        public float Time { get; }

        public float Value { get; }

        public MarkerDetectedEventArgs(
            SingleChannelSignal signal,
            SignalMarker marker)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));

            if (marker == null)
                throw new ArgumentNullException(nameof(marker));

            if (marker.SampleIndex >= signal.Data.Length)
                throw new ArgumentOutOfRangeException(nameof(marker));

            Signal = signal;
            Marker = marker;

            Time = (float)marker.SampleIndex / signal.Frequency;
            Value = signal.Data[marker.SampleIndex];
        }
    }
}
