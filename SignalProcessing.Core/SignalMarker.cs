
using System;

namespace SignalProcessing.Core
{
    public sealed class SignalMarker
    {
        public int SampleIndex { get; }
        public CrossingDirection Direction { get; }

        public SignalMarker(
            int sampleIndex,
            CrossingDirection direction)
        {
            if (sampleIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(sampleIndex));

            if (!Enum.IsDefined(typeof(CrossingDirection), direction))
                throw new ArgumentOutOfRangeException(nameof(direction));

            SampleIndex = sampleIndex;
            Direction = direction;
        }
    }
}
