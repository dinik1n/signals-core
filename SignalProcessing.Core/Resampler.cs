using System;

namespace SignalProcessing.Core
{
    public static class Resampler
    {
        public static SingleChannelSignal Resample(SingleChannelSignal signal, float newFrequency)
        {
            if (signal == null) throw new ArgumentNullException(nameof(signal));
            if (!(newFrequency > 0)) throw new ArgumentOutOfRangeException(nameof(newFrequency));

            float[] x = signal.Data;
            int n = x.Length;
            if (n == 0)
                return new SingleChannelSignal(newFrequency,new float[0]);

            double f = signal.Frequency, fNew = newFrequency;
            int count = (int)Math.Floor((n - 1) * fNew / f + 1e-6) + 1;
            float[] y = new float[count];
            for (int j = 0; j < count; j++)
            {
                double p = j * f / fNew;
                int k = (int)Math.Floor(p);
                if (k >= n - 1)
                    y[j] = x[n - 1];
                else
                    y[j] = (float)(x[k] + (p - k) * (x[k + 1] - x[k]));
            }
            return new SingleChannelSignal(newFrequency,y);
        }
    }
}