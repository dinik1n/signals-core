using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SineGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 53
        public void Generate_StandardSine_ReturnsCorrectValues()
        {
            var generator = new SineGenerator(1f, 0f, 1f);
            var sampling = new SamplingParameters(4f, 1f);

            var signal = generator.Generate(sampling);

            Assert.Equal(4, signal.Data.Length);

            Assert.Equal(0f, signal.Data[0], 1e-6f);
            Assert.Equal(1f, signal.Data[1], 1e-6f);
            Assert.Equal(0f, signal.Data[2], 1e-6f);
        }

        [Fact] //------------------------------------------------------------Тест 56
        public void Constructor_ZeroPeriod_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SineGenerator(0f, 0f, 1f));
        }
    }
}