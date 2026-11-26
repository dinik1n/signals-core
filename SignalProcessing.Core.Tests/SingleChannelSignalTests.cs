using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
        public class SingleChannelSignalTests
        {

            [Fact]//------------------------------------------------------------Тест 1
            public void Constructor_NullData_Throws()
                => Assert.Throws<ArgumentNullException>(() => new SingleChannelSignal(100f, null));

            [Theory]
            [InlineData(0f)]
            [InlineData(-1f)]

            public void Constructor_NonPositiveFrequency_Throws(float frequency)//------------------------------------------------------------Тест 2
                => Assert.Throws<ArgumentOutOfRangeException>(() => new SingleChannelSignal(frequency, new float[0]));
        }
}

