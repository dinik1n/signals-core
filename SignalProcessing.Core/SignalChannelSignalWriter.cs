using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SignalProcessing.Core
{
    static class SignalChannelSignalWriter
    {
        static public void Write(SignalChannelSignal obj, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(obj.Freq);
                foreach(double element in obj.Value)
                {
                    writer.WriteLine(element);
                }
                
            }

        }
    }
}
