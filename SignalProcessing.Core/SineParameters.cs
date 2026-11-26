namespace SignalProcessing.Core
{
    public sealed class SineParameters : GeneratorParameters
    {
        public float Period { get; set; }

        public float Phase { get; set; }

        public float Amplitude { get; set; }
    }
}