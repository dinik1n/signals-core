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
                float freq = (float.Parse(reader.ReadLine()));
                List<float> value = new List<float>();
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    value.Add(float.Parse(line));
                }
                float[] arrSignal = value.ToArray();
                SingleChannelSignal data = new SingleChannelSignal(freq, arrSignal);
                return data;
            }

        }

    }
}
