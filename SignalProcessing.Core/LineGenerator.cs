namespace SignalProcessing.Core
{
    public sealed class LineGenerator : ISignalGenerator
    {
        private readonly float _b;
        private readonly float _k;
        public LineGenerator(float k, float b)
        {
            _k = k;
            _b = b;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new System.ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];

            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                data[i] = _k * t + _b;
            }

            return new SingleChannelSignal(sampling.SamplingRate, data);
        }
    }
}