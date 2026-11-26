using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SawGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 59
        public void Generate_Sawtooth_ReturnsCorrectValues()
        {
            var generator = new SawGenerator(1f, 0f, 1f);
            var sampling = new SamplingParameters(4f, 1f);

            var signal = generator.Generate(sampling);

            AssertSignal.Equal(
                new float[] { -1f, -0.5f, 0f, 0.5f },
                signal.Data,
                1e-6f);
        }

        [Fact] //------------------------------------------------------------Тест 60
        public void Constructor_ZeroPeriod_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new SawGenerator(0f, 0f, 1f));
        }
    }
}