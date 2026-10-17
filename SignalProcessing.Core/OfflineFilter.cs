using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public class OfflineFilter
    {
        static public float[] Filter(float[] data, int windowSize)
        {
            //int M = (windowSize - 1) / 2;
            //IEnumerable<float> x = Enumerable.Range(0, M).Select(el => data[0]);
            //IEnumerable<float> z = Enumerable.Range(0, M).Select(el => data.Last());
            //float[] y = x.Concat(data).Concat(z).ToArray();

            //float[] newSignal = new float[data.Length];

            //for (int i = 0; i < y.Length; i++)
            //{
            //    y[i] += (data[i - M]) / windowSize;
            //}

            //return newSignal;
            // Расширяем сигнал в начале и конце дублированием первого и последнего значений
            // Проверка входных параметров
            int M = (windowSize - 1) / 2;

            // Создаём расширенный массив с дублированием крайних значений
            float[] extended = Enumerable.Repeat(data[0], M).Concat(data).Concat(Enumerable.Repeat(data[data.Length - 1], M)).ToArray();
            return Enumerable.Range(0, data.Length).Select(i => (float)extended.Skip(i).Take(windowSize).Average()).ToArray();
            

        }
    }
}
