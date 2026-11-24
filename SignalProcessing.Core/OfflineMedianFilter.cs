using System;

namespace SignalProcessing.Core
{
    public static class OfflineMedianFilter
    {
        public static float[] Filter(float[] data, int windowSize)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (windowSize <= 0 || windowSize % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize));

            float[] result = new float[data.Length];
            if (data.Length == 0)
                return result;

            int m = (windowSize - 1) / 2;
            int last = data.Length - 1;
            float[] window = new float[windowSize];
            for (int i = 0; i < data.Length; i++)
            {
                for (int k = 0; k < windowSize; k++)
                    window[k] = data[Math.Min(Math.Max(i - m + k, 0), last)];
                Array.Sort(window);
                result[i] = window[m];
            }
            return result;
        }
    }
}