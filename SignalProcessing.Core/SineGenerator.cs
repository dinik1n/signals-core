using System;

namespace SignalProcessing.Core
{
    public sealed class SineGenerator : ISignalGenerator
    {
        private readonly float _amplitude;
        private readonly float _period;
        private readonly float _phase;

        public SineGenerator(float period, float phase, float amplitude)
        {
            if (period <= 0 ||
                float.IsNaN(period) ||
                float.IsInfinity(period))
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            _period = period;
            _phase = phase;
            _amplitude = amplitude;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];

            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);

                data[i] = _amplitude * (float)Math.Sin(
                    2.0 * Math.PI / _period * (t - _phase));
            }

            return new SingleChannelSignal(sampling.SamplingRate, data);
        }
    }
}