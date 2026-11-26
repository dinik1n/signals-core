using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SamplingParametersTests
    {
        [Fact] //------------------------------------------------------------Тест 47
        public void Constructor_ValidParameters_ReturnsCorrectSampleCount()
        {
            var sampling = new SamplingParameters(100, 2);

            Assert.Equal(200, sampling.SampleCount);
        }

        [Theory] //------------------------------------------------------------Тест 48
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_NonPositiveSamplingRate_Throws(float samplingRate)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SamplingParameters(samplingRate, 2));
        }
    }
}