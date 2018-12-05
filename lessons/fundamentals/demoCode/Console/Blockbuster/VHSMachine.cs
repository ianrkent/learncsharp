using System;

namespace ConsoleApp.Blockbuster
{
    public class VHSMachine
    {
        public VHSTape CurrentVhsTape { get; }

        public Image CurrentImage { get; }

        public bool Rewind(TimeSpan distance) { return true; }

        public bool FastForward(TimeSpan distacSpan) { return true; }

        public void Play() { }

        public void Stop() { }
    }
}