using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public class SingleChannelSignal
    {
        public SingleChannelSignal(float frequency, float[] data)
        {
            Frequency = frequency;
            Data = data;
        }
        public float Frequency { get; }
        public float[] Data { get; }
        public float[] TimeTick
        {
            get
            {
                float[] timeTick = new float[Data.Length];
                for (int i = 0; i < Data.Length; i++)
                {
                    timeTick[i] = i / Frequency;
                }
                return timeTick;
            }
        }
    }
}
