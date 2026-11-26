using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class TriangleGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 57
        public void Generate_Triangle_ReturnsCorrectValues()
        {
            var generator = new TriangleGenerator(1f, 0f, 1f);
            var sampling = new SamplingParameters(4f, 1f);

            var signal = generator.Generate(sampling);

            AssertSignal.Equal(
                new float[] { -1f, 0f, 1f, 0f },
                signal.Data,
                1e-6f);
        }

        [Fact] //------------------------------------------------------------Тест 58
        public void Constructor_ZeroPeriod_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new TriangleGenerator(0f, 0f, 1f));
        }
    }
}