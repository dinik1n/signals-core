using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class NoiseGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 55
        public void Generate_FixedSeed_ReturnsValuesInRangeAndIsRepeatable()
        {
            var firstGenerator = new NoiseGenerator(
                2f, () => new Random(42));

            var secondGenerator = new NoiseGenerator(
                2f, () => new Random(42));

            var sampling = new SamplingParameters(100f, 2f);

            var firstSignal = firstGenerator.Generate(sampling);
            var secondSignal = secondGenerator.Generate(sampling);

            foreach (float value in firstSignal.Data)
            {
                Assert.InRange(value, -2f, 2f);
            }

            AssertSignal.Equal(
                firstSignal.Data,
                secondSignal.Data,
                1e-6f);
        }
    }
}