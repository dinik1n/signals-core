using System;

namespace SignalProcessing.Core
{
    public sealed class NoiseGenerator : ISignalGenerator
    {
        private readonly Random _random;
        private readonly float _range;
        public NoiseGenerator(float range, Func<Random> randomFactory = null)
        {
            if (range < 0 ||
                float.IsNaN(range) ||
                float.IsInfinity(range))
            {
                throw new ArgumentOutOfRangeException(nameof(range));
            }

            _range = range;
            _random = randomFactory == null
                ? new Random()
                : randomFactory();

            if (_random == null)
                throw new InvalidOperationException(
                    "Источник случайных чисел не должен быть null.");
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)(
                    (_random.NextDouble() * 2.0 - 1.0) * _range);
            }

            return new SingleChannelSignal(sampling.SamplingRate, data);
        }
    }
}