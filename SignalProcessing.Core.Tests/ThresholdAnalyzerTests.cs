
using System;
using System.Collections.Generic;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ThresholdAnalyzerTests
    {
        [Fact] //------------------------------------------------------------Тест 64
        public void SignalMarker_ValidArguments_CreatesMarker()
        {
            var marker = new SignalMarker(3, CrossingDirection.Up);

            Assert.Equal(3, marker.SampleIndex);
            Assert.Equal(CrossingDirection.Up, marker.Direction);
        }

        [Fact] //------------------------------------------------------------Тест 65
        public void SignalMarker_InvalidArguments_Throws()
        {
            var indexException = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SignalMarker(-1, CrossingDirection.Up));

            Assert.Equal("sampleIndex", indexException.ParamName);

            var directionException = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SignalMarker(1, (CrossingDirection)999));

            Assert.Equal("direction", directionException.ParamName);
        }

        [Fact] //------------------------------------------------------------Тест 66
        public void SignalAnalysisResult_InvalidArguments_Throws()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { 1f, 2f });

            Assert.Throws<ArgumentNullException>(
                () => new SignalAnalysisResult(
                    null,
                    new List<SignalMarker>()));

            Assert.Throws<ArgumentNullException>(
                () => new SignalAnalysisResult(
                    signal,
                    null));

            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => new SignalAnalysisResult(
                    signal,
                    new[]
                    {
                        new SignalMarker(2, CrossingDirection.Up)
                    }));

            Assert.Equal("markers", exception.ParamName);
        }

        [Fact] //------------------------------------------------------------Тест 67
        public void SignalAnalysisResult_MarkersAreReadOnlySnapshot()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { 1f, 2f, 3f });

            var markers = new List<SignalMarker>
            {
                new SignalMarker(1, CrossingDirection.Up)
            };

            var result = new SignalAnalysisResult(signal, markers);

            markers.Add(new SignalMarker(2, CrossingDirection.Down));

            Assert.Single(result.Markers);
            Assert.IsNotType<List<SignalMarker>>(result.Markers);
        }

        [Fact] //------------------------------------------------------------Тест 68
        public void MarkerDetectedEventArgs_CalculatesTimeAndValue()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { 0f, 2f, -3f });

            var marker = new SignalMarker(
                2,
                CrossingDirection.Down);

            var eventArgs = new MarkerDetectedEventArgs(signal, marker);

            Assert.Same(signal, eventArgs.Signal);
            Assert.Same(marker, eventArgs.Marker);
            Assert.Equal(0.2f, eventArgs.Time, 6);
            Assert.Equal(-3f, eventArgs.Value, 6);
        }

        [Fact] //------------------------------------------------------------Тест 69
        public void Analyze_WithoutSubscribers_ReturnsMarkersAndPreservesSignal()
        {
            float[] data = { -1f, 1f, 2f, -1f, -2f, 1f };
            float[] original = (float[])data.Clone();

            var signal = new SingleChannelSignal(10f, data);
            var analyzer = new ThresholdAnalyzer();

            var result = analyzer.Analyze(signal, 0f);

            Assert.Same(signal, result.Signal);
            Assert.Equal(3, result.Markers.Count);

            Assert.Equal(1, result.Markers[0].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[0].Direction);

            Assert.Equal(3, result.Markers[1].SampleIndex);
            Assert.Equal(CrossingDirection.Down, result.Markers[1].Direction);

            Assert.Equal(5, result.Markers[2].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[2].Direction);

            AssertSignal.Equal(original, data, 1e-6f);
        }

        [Fact] //------------------------------------------------------------Тест 70
        public void Analyze_ThresholdPlateau_DetectsOnlyTwoCrossings()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 0f, 0f, -1f });

            var analyzer = new ThresholdAnalyzer();

            var result = analyzer.Analyze(signal, 0f);

            Assert.Equal(2, result.Markers.Count);

            Assert.Equal(1, result.Markers[0].SampleIndex);
            Assert.Equal(CrossingDirection.Up, result.Markers[0].Direction);

            Assert.Equal(3, result.Markers[1].SampleIndex);
            Assert.Equal(CrossingDirection.Down, result.Markers[1].Direction);
        }

        [Fact] //------------------------------------------------------------Тест 71
        public void Analyze_NoCrossings_ReturnsEmptyResultAndDoesNotRaiseEvents()
        {
            var analyzer = new ThresholdAnalyzer();

            int eventCount = 0;

            analyzer.MarkerDetected += (sender, e) =>
            {
                eventCount++;
            };

            var signals = new[]
            {
                new SingleChannelSignal(10f, new float[0]),
                new SingleChannelSignal(10f, new float[] { 1f }),
                new SingleChannelSignal(10f, new float[] { 1f, 2f, 3f })
            };

            foreach (var signal in signals)
            {
                var result = analyzer.Analyze(signal, 0f);

                Assert.Empty(result.Markers);
            }

            Assert.Equal(0, eventCount);
        }

        [Fact] //------------------------------------------------------------Тест 72
        public void Analyze_ManagesMultipleEventSubscribersCorrectly()
        {
            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f });

            var analyzer = new ThresholdAnalyzer();

            var firstEvents = new List<MarkerDetectedEventArgs>();
            var secondEvents = new List<MarkerDetectedEventArgs>();

            object receivedSender = null;

            EventHandler<MarkerDetectedEventArgs> firstHandler =
                (sender, e) =>
                {
                    receivedSender = sender;
                    firstEvents.Add(e);
                };

            EventHandler<MarkerDetectedEventArgs> secondHandler =
                (sender, e) =>
                {
                    secondEvents.Add(e);
                };

            analyzer.MarkerDetected += firstHandler;
            analyzer.MarkerDetected += secondHandler;

            var firstResult = analyzer.Analyze(signal, 0f);

            analyzer.MarkerDetected -= secondHandler;

            var secondResult = analyzer.Analyze(signal, 0f);

            Assert.Single(firstResult.Markers);
            Assert.Single(secondResult.Markers);

            Assert.Equal(2, firstEvents.Count);
            Assert.Single(secondEvents);

            Assert.Same(analyzer, receivedSender);
            Assert.Same(signal, firstEvents[0].Signal);
            Assert.Same(firstResult.Markers[0], firstEvents[0].Marker);
            Assert.Same(firstEvents[0].Marker, secondEvents[0].Marker);
        }

        [Fact] //------------------------------------------------------------Тест 73
        public void Analyze_InvalidArguments_ThrowsWithoutEvents()
        {
            var analyzer = new ThresholdAnalyzer();

            int eventCount = 0;

            analyzer.MarkerDetected += (sender, e) =>
            {
                eventCount++;
            };

            Assert.Throws<ArgumentNullException>(
                () => analyzer.Analyze(null, 0f));

            var signal = new SingleChannelSignal(
                10f,
                new float[] { -1f, 1f });

            float[] invalidThresholds =
            {
                float.NaN,
                float.PositiveInfinity,
                float.NegativeInfinity
            };

            foreach (float threshold in invalidThresholds)
            {
                var exception = Assert.Throws<ArgumentOutOfRangeException>(
                    () => analyzer.Analyze(signal, threshold));

                Assert.Equal("threshold", exception.ParamName);
            }

            Assert.Equal(0, eventCount);
        }

        [Fact] //------------------------------------------------------------Тест 74
        public void Analyze_InvalidSignalValues_ThrowsWithoutEvents()
        {
            var analyzer = new ThresholdAnalyzer();

            int eventCount = 0;

            analyzer.MarkerDetected += (sender, e) =>
            {
                eventCount++;
            };

            float[] invalidValues =
            {
                float.NaN,
                float.PositiveInfinity,
                float.NegativeInfinity
            };

            foreach (float invalidValue in invalidValues)
            {
                var signal = new SingleChannelSignal(
                    10f,
                    new float[] { -1f, 1f, invalidValue });

                var exception = Assert.Throws<ArgumentException>(
                    () => analyzer.Analyze(signal, 0f));

                Assert.Equal("signal", exception.ParamName);
            }

            Assert.Equal(0, eventCount);
        }
    }
}
