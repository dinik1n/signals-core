using System;

namespace SignalProcessing.Core
{
    public class OnlineMedianFilter
    {
        private readonly int _windowSize;
        private readonly CircularBuffer<float> _window;
        private bool _started;

     public OnlineMedianFilter(int windowSize)
        {
            if (windowSize <= 0 || windowSize % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize));
            _windowSize = windowSize;
            _window = new CircularBuffer<float>(windowSize);
        }

     public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (!_started)
                {
                    for (int k = 0; k < _windowSize - 1; k++)
                        _window.Add(data[i]);
                    _started = true;
                }
                _window.Add(data[i]);
                float[] sorted = _window.ToArray();
                Array.Sort(sorted);
                result[i] = sorted[_windowSize / 2];
            }
            return result;
        }

        public void Reset()
        {
            _window.Clear();
            _started = false;
        }
    }
}