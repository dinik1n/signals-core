using System;
using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalGeneratorFactoryTests
    {
        [Fact] //------------------------------------------------------------Тест 49
        public void Create_SineParameters_ReturnsSineGenerator()
        {
            var parameters = new SineParameters
            {
                Period = 1f,
                Phase = 0f,
                Amplitude = 1f
            };

            var generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<SineGenerator>(generator);
        }

        [Fact] //------------------------------------------------------------Тест 50
        public void Create_LineAndNoiseParameters_ReturnCorrespondingGenerators()
        {
            var lineParameters = new LineParameters
            {
                K = 2f,
                B = 1f
            };

            var noiseParameters = new NoiseParameters
            {
                Range = 2f
            };

            var lineGenerator = SignalGeneratorFactory.Create(lineParameters);
            var noiseGenerator = SignalGeneratorFactory.Create(noiseParameters);

            Assert.IsType<LineGenerator>(lineGenerator);
            Assert.IsType<NoiseGenerator>(noiseGenerator);
        }

        [Fact] //------------------------------------------------------------Тест 51
        public void Create_NullParameters_Throws()
        {
            Assert.Throws<ArgumentNullException>(
                () => SignalGeneratorFactory.Create(null));
        }

        [Fact] //------------------------------------------------------------Тест 52
        public void Create_UnsupportedParameters_Throws()
        {
            var exception = Assert.Throws<NotSupportedException>(
                () => SignalGeneratorFactory.Create(new FakeParameters()));

            Assert.Contains(nameof(FakeParameters), exception.Message);
        }

        private sealed class FakeParameters : GeneratorParameters
        {
        }
    }
}