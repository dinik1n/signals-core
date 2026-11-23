using System;
using System.Linq;

namespace SignalProcessing.Core
{
    public static class OfflineFilter
    {
        static public float[] Filter(float[] data, int windowSize)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length == 0)
                return new float[0];
            if (windowSize <= 0 || windowSize % 2 == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(windowSize),
                    "Размер окна должен быть положительным и нечётным.");
            }
            int M = (windowSize - 1) / 2;
            float[] extended = Enumerable.Repeat(data[0], M).Concat(data).Concat(Enumerable.Repeat(data[data.Length - 1], M)).ToArray();
            return Enumerable.Range(0, data.Length).Select(i => (float)extended.Skip(i).Take(windowSize).Average()).ToArray();
        }
    }
}