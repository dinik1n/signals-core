using System;

namespace SignalProcessing.Core
{

    public class OnlineFilter
    {
        private readonly int _windowSize;
        private readonly CircularBuffer<float> _window;
        private double _sum;
        private bool _started;

        public OnlineFilter(int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize));
            _windowSize = windowSize;
            _window = new CircularBuffer<float>(windowSize);
        }

        public float[] Filter(float[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                float x = data[i];
                if (!_started)
                {
                    for (int k = 0; k < _windowSize - 1; k++)
                        _window.Add(x);
                    _sum = (double)(_windowSize - 1) * x;
                    _started = true;
                }
                double leaving = _window.IsFull ? _window[0] : 0;
                _window.Add(x);
                _sum += x - leaving;
                result[i] = (float)(_sum / _windowSize);
            }
            return result;
        }
        public void Reset()
        {
            _window.Clear();
            _sum = 0;
            _started = false;
        }
    }
}