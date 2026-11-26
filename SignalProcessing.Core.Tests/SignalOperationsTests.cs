using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalOperationsTests
    {
        private static SingleChannelSignal S(float freq, params float[] data)
            => new SingleChannelSignal(freq,data);

        [Fact] //------------------------------------------------------------Тест 18

        public void Sum_DifferentLengths_ShortPaddedWithZeros()
        {
            var r = SignalOperations.Sum(new[] { S(100, 1, 2, 3), S(100, 10, 20) });
            AssertSignal.Equal(new float[] { 11, 22, 3 }, r.Data);
            Assert.Equal(100f, r.Frequency, 1e-5f);
        }

        [Fact] //------------------------------------------------------------Тест 19

        public void Concat_TwoSignals_Joined()
            => AssertSignal.Equal(new float[] { 1, 2, 3 },
                                  SignalOperations.Concat(new[] { S(100, 1, 2), S(100, 3) }).Data);

        [Fact] //------------------------------------------------------------Тест 20

        public void Sum_DifferentFrequencies_Throws()
            => Assert.Throws<ArgumentException>(() => SignalOperations.Sum(new[] { S(100, 1), S(200, 1) }));

        [Fact] //------------------------------------------------------------Тест 20
        public void Concat_DifferentFrequencies_Throws()
            => Assert.Throws<ArgumentException>(() => SignalOperations.Concat(new[] { S(100, 1), S(200, 1) }));

        [Fact] //------------------------------------------------------------Тест 21
        public void Sum_EmptyList_Throws()
            => Assert.Throws<ArgumentException>(() => SignalOperations.Sum(new List<SingleChannelSignal>()));

        [Fact] //------------------------------------------------------------Тест 21
        public void Sum_NullList_Throws()
            => Assert.Throws<ArgumentNullException>(() => SignalOperations.Sum(null));

        [Fact] //------------------------------------------------------------Тест 21
        public void Sum_ListWithNull_Throws()
            => Assert.Throws<ArgumentNullException>(() => SignalOperations.Sum(new SingleChannelSignal[] { S(100, 1), null }));

        [Fact] //------------------------------------------------------------Тест 22
        public void Sum_DoesNotModifySources()
        {
            var a = S(100, 1, 2, 3);
            var b = S(100, 10, 20);
            SignalOperations.Sum(new[] { a, b });
            AssertSignal.Equal(new float[] { 1, 2, 3 }, a.Data);
            AssertSignal.Equal(new float[] { 10, 20 }, b.Data);
        }
    }
}
