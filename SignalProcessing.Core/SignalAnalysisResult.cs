
using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    public sealed class SignalAnalysisResult
    {
        private readonly IReadOnlyList<SignalMarker> _markers;

        public SingleChannelSignal Signal { get; }

        public IReadOnlyList<SignalMarker> Markers => _markers;

        public SignalAnalysisResult(
            SingleChannelSignal signal,
            IEnumerable<SignalMarker> markers)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));

            if (markers == null)
                throw new ArgumentNullException(nameof(markers));

            List<SignalMarker> markerList = new List<SignalMarker>();

            foreach (SignalMarker marker in markers)
            {
                if (marker == null)
                    throw new ArgumentNullException(nameof(markers));

                if (marker.SampleIndex >= signal.Data.Length)
                    throw new ArgumentOutOfRangeException(nameof(markers));

                markerList.Add(marker);
            }

            Signal = signal;
            _markers = markerList.AsReadOnly();
        }
    }
}
