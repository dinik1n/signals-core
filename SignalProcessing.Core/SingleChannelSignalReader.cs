using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public class SignalChannelSignalReader
    {
        static public SingleChannelSignal Read(string filePath)
        {

            using (StreamReader reader = new StreamReader(filePath))
            {
                string first = reader.ReadLine();
                if (first == null) throw new InvalidDataException("Файл пуст.");
                float freq = (float.Parse(reader.ReadLine()));
                if (freq <= 0)
                    throw new InvalidDataException("Строка 1: частота должна быть положительной.");
                List<float> value = new List<float>();
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    value.Add(float.Parse(line));
                }
                float[] arrSignal = value.ToArray();
                SingleChannelSignal data = new SingleChannelSignal(freq, arrSignal);
                return data;
            }

        }

    }
}
