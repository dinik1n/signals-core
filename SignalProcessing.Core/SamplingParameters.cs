using System;

namespace SignalProcessing.Core
{
    public sealed class SamplingParameters
    {
        public float SamplingRate { get; }

        public float Duration { get; }

        public int SampleCount { get; }

        public SamplingParameters(float samplingRate, float duration)
        {
            if (samplingRate <= 0 ||
                float.IsNaN(samplingRate) ||
                float.IsInfinity(samplingRate))
            {
                throw new ArgumentOutOfRangeException(nameof(samplingRate));
            }

            if (duration <= 0 ||
                float.IsNaN(duration) ||
                float.IsInfinity(duration))
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            SamplingRate = samplingRate;
            Duration = duration;

            double count = Math.Floor(
                (double)samplingRate * duration + 0.5d);

            if (count > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            SampleCount = (int)count;
        }

        public float TimeAt(int index)
        {
            if (index < 0 || index >= SampleCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return index / SamplingRate;
        }
    }
}