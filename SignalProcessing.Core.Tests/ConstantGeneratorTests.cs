using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ConstantGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 61
        public void Generate_ConstantSignal_ReturnsSameValues()
        {
            var generator = new ConstantGenerator(5f);
            var sampling = new SamplingParameters(4f, 1f);

            var signal = generator.Generate(sampling);

            AssertSignal.Equal(
                new float[] { 5f, 5f, 5f, 5f },
                signal.Data,
                1e-6f);
        }

        [Fact] //------------------------------------------------------------Тест 62
        public void Constructor_InfinityValue_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => new ConstantGenerator(float.PositiveInfinity));
        }

        [Fact] //------------------------------------------------------------Тест 63
        public void Factory_ConstantParameters_ReturnsConstantGenerator()
        {
            var parameters = new ConstantParameters
            {
                Value = 3f
            };

            var generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<ConstantGenerator>(generator);
        }
    }
}