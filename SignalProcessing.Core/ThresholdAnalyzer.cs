
using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    public sealed class ThresholdAnalyzer
    {
        public event EventHandler<MarkerDetectedEventArgs> MarkerDetected;
        public SignalAnalysisResult Analyze(
            SingleChannelSignal signal,
            float threshold)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));

            if (float.IsNaN(threshold) ||
                float.IsInfinity(threshold))
            {
                throw new ArgumentOutOfRangeException(nameof(threshold));
            }

            for (int i = 0; i < signal.Data.Length; i++)
            {
                if (float.IsNaN(signal.Data[i]) ||
                    float.IsInfinity(signal.Data[i]))
                {
                    throw new ArgumentException(
                        "Сигнал содержит некорректные значения.",
                        nameof(signal));
                }
            }

            List<SignalMarker> markers = new List<SignalMarker>();

            for (int i = 1; i < signal.Data.Length; i++)
            {
                CrossingDirection? direction = null;

                if (signal.Data[i - 1] < threshold &&
                    signal.Data[i] >= threshold)
                {
                    direction = CrossingDirection.Up;
                }
                else if (signal.Data[i - 1] >= threshold &&
                         signal.Data[i] < threshold)
                {
                    direction = CrossingDirection.Down;
                }

                if (direction.HasValue)
                {
                    SignalMarker marker = new SignalMarker(
                        i,
                        direction.Value);

                    markers.Add(marker);

                    MarkerDetected?.Invoke(
                        this,
                        new MarkerDetectedEventArgs(signal, marker));
                }
            }

            return new SignalAnalysisResult(signal, markers);
        }
    }
}
