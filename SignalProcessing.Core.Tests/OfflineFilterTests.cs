using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OfflineFilterTests
    {
        

        [Fact] //------------------------------------------------------------Тест 3
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
        public void Filter_InvalidwindowSize_Throws(int windowSize)
        {
            float[] data = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>("windowSize",
            () => OfflineFilter.Filter(data, windowSize));
        }
        [Fact] //------------------------------------------------------------Тест 4

        public void Filter_Ramp_Window3_HandCalculated()
    => AssertSignal.Equal(new[] { 4f / 3, 2f, 3f, 4f, 14f / 3 },
                          OfflineFilter.Filter(new float[] { 1, 2, 3, 4, 5 }, 3));

        [Fact] //------------------------------------------------------------Тест 5

        public void Filter_Impulse_Window3_SpreadsOverThree()
            => AssertSignal.Equal(new[] { 0f, 0f, 1f / 3, 1f / 3, 1f / 3, 0f, 0f },
                                  OfflineFilter.Filter(new float[] { 0, 0, 0, 1, 0, 0, 0 }, 3));

        [Fact] //------------------------------------------------------------Тест 6

        public void Filter_Window1_EqualsInputButNewArray()
        {
            float[] data = { 1, 2, 3 };
            float[] result = OfflineFilter.Filter(data, 1);
            AssertSignal.Equal(data, result);
            Assert.NotSame(data, result);
        }

        [Theory] //------------------------------------------------------------Тест 7

        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void Filter_InvalidWindowSize_Throws(int windowSize)
            => Assert.Throws<ArgumentOutOfRangeException>("windowSize",
                () => OfflineFilter.Filter(new float[] { 1, 2, 3 }, windowSize));

        [Fact] //------------------------------------------------------------Тест 8

        public void Filter_Null_Throws()
            => Assert.Throws<ArgumentNullException>(() => OfflineFilter.Filter(null, 3));

        [Fact] //------------------------------------------------------------Тест 8

        public void Filter_EmptyArray_ReturnsEmpty()
            => Assert.Empty(OfflineFilter.Filter(new float[0], 3));

        [Fact] //------------------------------------------------------------Тест 9

        public void Filter_DoesNotModifyInput()
        {
            float[] data = { 1, 2, 3, 4, 5 };
            OfflineFilter.Filter(data, 3);
            AssertSignal.Equal(new float[] { 1, 2, 3, 4, 5 }, data);
        }
    }
}
