using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace SignalProcessing.Core.Tests
{
    internal static class AssertSignal
    {
        public static void Equal(float[] expected, float[] actual,
        float tolerance = 1e-5f)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], tolerance);
        }
    }

    public class OfflineFilterTests
    {
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            // Arrange
            float[] data = { 5, 5, 5, 5, 5 };
            // Act
            float[] result = OfflineFilter.Filter(data, 3);
            // Assert
            Assert.Equal(data.Length, result.Length);
            foreach (float value in result)
                Assert.Equal(5f, value, 1e-5f);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void Filter_InvalidWindowSize_Throws(int windowSize)
        {
            float[] data = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
            () => OfflineFilter.Filter(data, windowSize));
        }
    }
}

