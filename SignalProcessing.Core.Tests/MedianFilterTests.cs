using System;
using System.Linq;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class MedianFilterTests
    {
        private static float[] RandomSignal()
        {
            var random = new Random(42);
            var data = new float[1000];
            for (int i = 0; i < data.Length; i++)
                data[i] = (float)random.NextDouble() * 10 - 5;
            return data;
        }

        [Fact] //------------------------------------------------------------Тест 41
        public void Offline_SingleOutlier_IsRemoved()
            => AssertSignal.Equal(new float[] { 1, 1, 1, 1, 1, 1, 1 },
                OfflineMedianFilter.Filter(new float[] { 1, 1, 1, 9, 1, 1, 1 }, 3));

        [Fact] //------------------------------------------------------------Тест 42
        public void Offline_Step_IsPreserved()
        {
            float[] step = { 0, 0, 0, 1, 1, 1 };
            AssertSignal.Equal(step, OfflineMedianFilter.Filter(step, 3));
        }

        [Fact] //------------------------------------------------------------Тест 43
        public void MovingAverage_Step_IsBlurred()
        {
            float[] step = { 0, 0, 0, 1, 1, 1 };
            float[] result = OfflineFilter.Filter(step, 3);
            Assert.True(Enumerable.Range(0, step.Length).Any(i => Math.Abs(result[i] - step[i]) > 1e-5f));
        }

        [Fact] //------------------------------------------------------------Тест 44
        public void Online_EqualsOfflineShiftedByM()
        {
            float[] data = RandomSignal();
            float[] online = new OnlineMedianFilter(7).Filter(data);
            float[] offline = OfflineMedianFilter.Filter(data, 7);
            for (int i = 3; i < data.Length; i++)
                Assert.Equal(offline[i - 3], online[i], 1e-5f);
        }

        [Theory] //------------------------------------------------------------Тест 45
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void Online_Chunked_EqualsWhole(int chunk)
        {
            float[] data = RandomSignal();
            var filter = new OnlineMedianFilter(7);
            var result = new float[data.Length];
            for (int start = 0; start < data.Length; start += chunk)
            {
                int len = Math.Min(chunk, data.Length - start);
                var part = new float[len];
                Array.Copy(data, start, part, 0, len);
                Array.Copy(filter.Filter(part), 0, result, start, len);
            }
            AssertSignal.Equal(new OnlineMedianFilter(7).Filter(data), result);
        }

        [Theory] //------------------------------------------------------------Тест 46
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void InvalidWindowSize_Throws(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => OfflineMedianFilter.Filter(new float[] { 1, 2, 3 }, windowSize));
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => new OnlineMedianFilter(windowSize));
        }
    }
}