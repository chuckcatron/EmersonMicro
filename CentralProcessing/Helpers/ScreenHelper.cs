using System;
using centralProcessing.Interfaces;

namespace centralProcessing.Helpers
{
    public class ScreenHelper : IScreenHelper
    {
        public void Clear()
        {
            Console.Clear();
        }

        public void Print(string msg)
        {
            Console.WriteLine(msg);
        }

        public string GetResponse()
        {
            return Console.ReadLine();
        }
    }
}