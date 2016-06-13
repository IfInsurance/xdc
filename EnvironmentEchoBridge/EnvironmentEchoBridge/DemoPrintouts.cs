using System;

namespace EnvironmentEchoBridge
{
    public static class DemoPrintouts
    {
        private static void PrepareOutput()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        public static void Begin(string str, params string[] args)
        {
            PrepareOutput();
            Console.Write("¤ ");
            Console.Write(String.Format(str, args));
            Console.ResetColor();
        }

        public static void End(string str, params string[] args)
        {
            PrepareOutput();
            Console.WriteLine(String.Format(str, args));
            Console.ResetColor();
        }
    }
}
