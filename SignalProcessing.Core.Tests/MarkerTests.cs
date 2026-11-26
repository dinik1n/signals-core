
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class MarkerTests
    {
        [Fact] //------------------------------------------------------------Тест 75
        public void MarkerCounter_CountsUpAndDownCrossings()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f, -1f, 1f, -1f });

            var analyzer = new ThresholdAnalyzer();
            var counter = new MarkerCounter();

            analyzer.MarkerDetected += counter.OnMarkerDetected;

            analyzer.Analyze(signal, 0f);

            Assert.Equal(2, counter.UpCount);
            Assert.Equal(2, counter.DownCount);
        }

        [Fact] //------------------------------------------------------------Тест 76
        public void MarkerCounter_Reset_ClearsCounters()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f, -1f });

            var analyzer = new ThresholdAnalyzer();
            var counter = new MarkerCounter();

            analyzer.MarkerDetected += counter.OnMarkerDetected;

            analyzer.Analyze(signal, 0f);
            counter.Reset();

            Assert.Equal(0, counter.UpCount);
            Assert.Equal(0, counter.DownCount);
        }

        [Fact] //------------------------------------------------------------Тест 77
        public void MarkerIntervalAnalyzer_CalculatesMinimumAndMaximum()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f, 1f, -1f, -1f, 1f, 1f, 1f, -1f });

            var analyzer = new ThresholdAnalyzer();
            var intervalAnalyzer = new MarkerIntervalAnalyzer();

            analyzer.MarkerDetected +=
                intervalAnalyzer.OnMarkerDetected;

            analyzer.Analyze(signal, 0f);

            Assert.True(intervalAnalyzer.MinimumInterval.HasValue);
            Assert.True(intervalAnalyzer.MaximumInterval.HasValue);

            Assert.Equal(
                0.2f,
                intervalAnalyzer.MinimumInterval.Value,
                6);

            Assert.Equal(
                0.3f,
                intervalAnalyzer.MaximumInterval.Value,
                6);
        }

        [Fact] //------------------------------------------------------------Тест 78
        public void MarkerIntervalAnalyzer_Reset_ClearsIntervals()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f, -1f });

            var analyzer = new ThresholdAnalyzer();
            var intervalAnalyzer = new MarkerIntervalAnalyzer();

            analyzer.MarkerDetected +=
                intervalAnalyzer.OnMarkerDetected;

            analyzer.Analyze(signal, 0f);

            Assert.True(intervalAnalyzer.MinimumInterval.HasValue);
            Assert.True(intervalAnalyzer.MaximumInterval.HasValue);

            intervalAnalyzer.Reset();

            Assert.False(intervalAnalyzer.MinimumInterval.HasValue);
            Assert.False(intervalAnalyzer.MaximumInterval.HasValue);
        }
    }
}
