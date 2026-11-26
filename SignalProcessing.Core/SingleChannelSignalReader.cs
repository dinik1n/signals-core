using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public class SingleChannelSignalReader
    {
        static public SingleChannelSignal Read(string filePath)
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string freqLine = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(freqLine))
                    {
                        throw new InvalidDataException("Файл пуст или не содержит частоту.");
                    }

                    float freq = float.Parse(freqLine, CultureInfo.InvariantCulture);

                    if (freq <= 0)
                        throw new InvalidDataException("Строка 1: частота должна быть положительной.");
                    List<float> value = new List<float>();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        value.Add(float.Parse(line, CultureInfo.InvariantCulture));
                    }
                    float[] arrSignal = value.ToArray();
                    SingleChannelSignal data = new SingleChannelSignal(freq, arrSignal);
                    return data;
                }
            }
            catch (FormatException ex)
            {
                throw new InvalidDataException("Файл содержит данные в неверном формате.", ex);
            }
        }
    }
}