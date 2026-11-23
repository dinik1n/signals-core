using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public static class SignalOperations
    {
        // поэлементная сумма сигналов
        public static SingleChannelSignal Sum(IReadOnlyList<SingleChannelSignal> signals)
        {
            if (signals == null|| signals.Count == 0 )
            {
                throw new ArgumentNullException("signals = 0 или содержит 0", nameof(signals));
            }
            foreach (SingleChannelSignal s in signals)
                if (s == null)
                    throw new ArgumentException("Список содержит null.",nameof(signals));


            float freq = signals.First().Frequency;

            int max_len = signals.Select(el => el.Data.Length).Max();
            float[] summ_signals = new float[max_len];
            double tolerance = 1e-5;
            for (int i = 0; i < signals.Count; i++)
            {
                if (Math.Abs(signals[i].Frequency - freq) > tolerance)
                {
                    throw new ArgumentException("Разница частот больше 1е-5", nameof(signals));
                }
            }
            for (int i = 0; i < signals.Count; i++)
            {
                {
                    for (int j = 0; j < signals[i].Data.Length; j++)
                    {
                        summ_signals[j] += signals[i].Data[j];
                    }
                }
            }

            return new SingleChannelSignal(freq, summ_signals);
        }
        // последовательная склейка сигналов в порядке следования в списке
        public static SingleChannelSignal Concat(IReadOnlyList<SingleChannelSignal> signals)
        {
            if (signals == null || signals.Count == 0)
            {
                throw new ArgumentNullException("signals = 0 или содержит 0", nameof(signals));

            }
            foreach (SingleChannelSignal s in signals)
                if (s == null)
                    throw new ArgumentException("Список содержит null.", nameof(signals));

            float freq = signals.First().Frequency;

            IEnumerable<float> concat_signals = new float[0];
            double tolerance = 1e-5;
            for (int i = 0; i < signals.Count; i++)
            {
                if (Math.Abs(signals[i].Frequency - freq) > tolerance)
                {
                    throw new ArgumentException("Разница частот больше 1е-5", nameof(signals));
                }
            }
            for (int i = 0; i < signals.Count; i++)
            {
                concat_signals = concat_signals.Concat(signals[i].Data);
            }

            return new SingleChannelSignal(freq, concat_signals.ToArray());
        }
    }

}
