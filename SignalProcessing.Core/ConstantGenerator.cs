using System;

namespace SignalProcessing.Core
{
    public sealed class ConstantGenerator : ISignalGenerator
    {
        private readonly float value;

        public ConstantGenerator(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
                throw new ArgumentOutOfRangeException(nameof(value));

            this.value = value;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = value;
            }

            return new SingleChannelSignal(sampling.SamplingRate, data);
        }
    }
}