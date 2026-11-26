using SignalProcessing.Core;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class LineGeneratorTests
    {
        [Fact] //------------------------------------------------------------Тест 54
        public void Generate_Line_ReturnsCorrectValues()
        {
            var generator = new LineGenerator(2f, 1f);
            var sampling = new SamplingParameters(4f, 1f);

            var signal = generator.Generate(sampling);

            AssertSignal.Equal(
                new float[] { 1f, 1.5f, 2f, 2.5f },
                signal.Data,
                1e-6f);
        }
    }
}