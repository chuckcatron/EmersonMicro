using System;

namespace centralProcessing.Helpers
{
    public interface IScreenHelper
    {
        void Clear();
        void Print(string msg);
        string GetResponse();
    }

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