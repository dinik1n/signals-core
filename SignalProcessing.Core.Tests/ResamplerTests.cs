using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ResamplerTests
    {
        private static SingleChannelSignal S(float f, params float[] d) => new SingleChannelSignal(f, d);

        [Fact] //------------------------------------------------------------Тест 34
        public void Resample_SameFrequency_ReturnsSameSamples()
            => AssertSignal.Equal(new float[] { 1, 5, 2 }, Resampler.Resample(S(10, 1, 5, 2), 10).Data);

        [Fact] //------------------------------------------------------------Тест 35
        public void Resample_Upsample_x2()
        {
            var r = Resampler.Resample(S(1, 0, 2, 4, 8), 2);
            AssertSignal.Equal(new float[] { 0, 1, 2, 3, 4, 6, 8 }, r.Data);
            Assert.Equal(2f, r.Frequency, 1e-5f);
        }

        [Fact] //------------------------------------------------------------Тест 36
        public void Resample_Downsample_Half()
            => AssertSignal.Equal(new float[] { 0, 2, 4 }, Resampler.Resample(S(2, 0, 1, 2, 3, 4), 1).Data);

        [Fact] //------------------------------------------------------------Тест 37
        public void Resample_NonIntegerRatio()
            => AssertSignal.Equal(new[] { 0f, 4.5f, 9f }, Resampler.Resample(S(3, 0, 3, 6, 9), 2).Data);

        [Fact] //------------------------------------------------------------Тест 38
        public void Resample_Line_StaysTheSameLine()
        {
            var data = new float[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = (float)(2.0 * i / 100 + 1);
            var r = Resampler.Resample(new SingleChannelSignal(100f,data), 37f);
            for (int j = 0; j < r.Data.Length; j++)
                Assert.Equal((float)(2.0 * j / 37 + 1), r.Data[j], 1e-4f);
        }

        [Fact] //------------------------------------------------------------Тест 39
        public void Resample_SingleSample_ReturnsSameSample()
            => AssertSignal.Equal(new float[] { 7 }, Resampler.Resample(S(10, 7), 50).Data);

        [Fact] //------------------------------------------------------------Тест 39
        public void Resample_EmptySignal_ReturnsEmpty()
            => Assert.Empty(Resampler.Resample(S(10), 50).Data);

        [Theory] //------------------------------------------------------------Тест 40
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Resample_NonPositiveFrequency_Throws(float f)
            => Assert.Throws<ArgumentOutOfRangeException>(() => Resampler.Resample(S(10, 1, 2), f));

        [Fact] //------------------------------------------------------------Тест 40
        public void Resample_Null_Throws()
            => Assert.Throws<ArgumentNullException>(() => Resampler.Resample(null, 10f));
    }
}
