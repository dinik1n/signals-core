using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OnlineFilterTests
    {
        private static float[] RandomSignal()
        {
            var random = new Random(42);
            var data = new float[1000];
            for (int i = 0; i < data.Length; i++)
                data[i] = (float)random.NextDouble() * 10 - 5;
            return data;
        }

        private static float[] FilterInChunks(float[] data, int windowSize, int chunk)
        {
            var filter = new OnlineFilter(windowSize);
            var result = new float[data.Length];
            for (int start = 0; start < data.Length; start += chunk)
            {
                int len = Math.Min(chunk, data.Length - start);
                var part = new float[len];
                Array.Copy(data, start, part, 0, len);
                Array.Copy(filter.Filter(part), 0, result, start, len);
            }
            return result;
        }

        [Fact] //------------------------------------------------------------Тест 10

        public void Filter_Ramp_Window3_HandCalculated()
            => AssertSignal.Equal(new[] { 1f, 4f / 3, 2f, 3f, 4f },
                                  new OnlineFilter(3).Filter(new float[] { 1, 2, 3, 4, 5 }));

        [Fact] //------------------------------------------------------------Тест 11
        public void Filter_TwoCalls_EqualsOneCall()
        {
            var filter = new OnlineFilter(3);
            float[] a = filter.Filter(new float[] { 1, 2 });
            float[] b = filter.Filter(new float[] { 3, 4, 5 });
            AssertSignal.Equal(new[] { 1f, 4f / 3, 2f, 3f, 4f }, a.Concat(b).ToArray());
        }

        [Theory] //------------------------------------------------------------Тест 12

        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void Filter_Chunked_EqualsWhole(int chunk)
        {
            float[] data = RandomSignal();
            AssertSignal.Equal(new OnlineFilter(7).Filter(data), FilterInChunks(data, 7, chunk));
        }

        [Fact] //------------------------------------------------------------Тест 13

        public void Filter_Online_EqualsOfflineShiftedByM()
        {
            float[] data = RandomSignal();
            float[] online = new OnlineFilter(7).Filter(data);
            float[] offline = OfflineFilter.Filter(data, 7);
            for (int i = 3; i < data.Length; i++)
                Assert.Equal(offline[i - 3], online[i], 1e-5f);
        }

        [Theory] //------------------------------------------------------------Тест 14

        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidWindowSize_Throws(int windowSize)
            => Assert.Throws<ArgumentOutOfRangeException>(() => new OnlineFilter(windowSize));

        [Fact] //------------------------------------------------------------Тест 31
        public void Reset_ThenNewSignal_EqualsNewFilter()
        {
            var filter = new OnlineFilter(3);
            filter.Filter(new float[] { 9, 8, 7, 6 });
            filter.Reset();
            float[] signal = { 1, 2, 3, 4, 5 };
            AssertSignal.Equal(new OnlineFilter(3).Filter(signal), filter.Filter(signal));
        }

        [Fact] //------------------------------------------------------------Тест 32
        public void Filter_Window1_EqualsInput()
        {
            float[] data = { 3, -1, 4, 1, 5 };
            AssertSignal.Equal(data, new OnlineFilter(1).Filter(data));
        }

        [Fact] //------------------------------------------------------------Тест 33
        public void Filter_LongSignal_MatchesDoubleReference()
        {
            const int w = 7;
            var random = new Random(42);
            var data = new float[200000];
            for (int i = 0; i < data.Length; i++)
                data[i] = 900f + (float)random.NextDouble() * 200f;

            float[] actual = new OnlineFilter(w).Filter(data);

            for (int i = 0; i < data.Length; i++)
            {
                double sum = 0;
                for (int k = i - w + 1; k <= i; k++)
                    sum += data[Math.Max(k, 0)];
                Assert.True(Math.Abs(sum / w - actual[i]) <= 1e-3, "Индекс " + i);
            }
        }
    }
}
