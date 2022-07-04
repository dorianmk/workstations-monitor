using System;
using Common.Interfaces;

namespace WorkstationService.Entry.Components
{
    internal class Logger : IAction<string>
    {
        public Logger()
        {
        }

        public void Do(string text)
        {
            if (Environment.UserInteractive)
                Console.WriteLine(text);
        }
    }
}
