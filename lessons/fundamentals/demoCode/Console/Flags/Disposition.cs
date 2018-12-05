using System;

namespace ConsoleApp.Flags
{
    [Flags]
    public enum Disposition
    {
        Sunny,
        Positive,
        Miserable,
        Whiney
    }

    public class FlagsDemo
    {
        public void Go()
        {
            // Demo use of Flags enum
            
        }
    }

}
