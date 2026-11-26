using System;

namespace SignalProcessing.Core
{
    public sealed class SawGenerator : ISignalGenerator
    {
        private readonly float period;
        private readonly float phase;
        private readonly float amplitude;

        public SawGenerator(float period, float phase, float amplitude)
        {
            if (period <= 0 ||
                float.IsNaN(period) ||
                float.IsInfinity(period))
                throw new ArgumentOutOfRangeException(nameof(period));

            if (float.IsNaN(phase) || float.IsInfinity(phase))
                throw new ArgumentOutOfRangeException(nameof(phase));

            if (amplitude < 0 ||
                float.IsNaN(amplitude) ||
                float.IsInfinity(amplitude))
                throw new ArgumentOutOfRangeException(nameof(amplitude));

            this.period = period;
            this.phase = phase;
            this.amplitude = amplitude;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];

            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                double position = (t - phase) / period;
                double fraction = position - Math.Floor(position);

                data[i] = amplitude * (float)(2.0 * fraction - 1.0);
            }

            return new SingleChannelSignal(sampling.SamplingRate, data);
        }
    }
}