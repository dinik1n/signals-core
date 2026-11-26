using System;
using System.IO;
using SignalProcessing.Core;
using SignalProcessing.Core.Tests;
using Xunit;



namespace SignalProcessing.Core.Tests
{
    public class SignalFileTests : IDisposable
    {
        private readonly string _filePath;
        public SignalFileTests() { _filePath = Path.GetTempFileName(); }
        public void Dispose() { File.Delete(_filePath); }

        [Fact] //------------------------------------------------------------Тест 15

        public void WriteThenRead_ReturnsSameSignal()
        {
            var signal = new SingleChannelSignal(250f, new float[] { 0.5f, -1.25f, 3f, 0.1f });
            SingleChannelSignalWriter.Write(signal, _filePath);
            SingleChannelSignal read = SingleChannelSignalReader.Read(_filePath);
            Assert.Equal(250f, read.Frequency, 1e-5f);
            AssertSignal.Equal(signal.Data, read.Data);
        }

        [Fact] //------------------------------------------------------------Тест 16

        public void Read_OnlyFrequencyLine_ReturnsEmptySignal()
        {
            File.WriteAllText(_filePath, "100");
            SingleChannelSignal read = SingleChannelSignalReader.Read(_filePath);
            Assert.Empty(read.Data);
            Assert.Equal(100f, read.Frequency, 1e-5f);
        }

        [Theory] //------------------------------------------------------------Тест 17

        [InlineData("")]
        [InlineData("100\n1\nabc\n3")]
        [InlineData("0\n1\n2")]
        public void Read_InvalidContent_Throws(string content)
        {
            File.WriteAllText(_filePath, content);
            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(_filePath));
        }
    }
}