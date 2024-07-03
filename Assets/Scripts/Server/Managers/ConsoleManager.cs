using Shared;
using System;
using System.Reflection;

namespace Server 
{
    public static class ConsoleManager
    {
        public static void ConsoleListenForInput()
        {
            while (true)
            {
                string input = System.Console.ReadLine();
                if (input != "")
                {
                    char c = input[0];
                    c = Char.ToUpper(c);
                    input = c + input.Substring(1);
                    string[] inputArray = input.Split(" ");
                    ConsoleHandleCommand(inputArray);
                }
            }
        }
        public static void ConsoleHandleCommand(string[] command) 
        {
            Type toUse = typeof(ConsoleManager);
            MethodInfo methodInfo = toUse.GetMethod("ConsoleCommand"+command[0]);
            methodInfo.Invoke(methodInfo.Name, new object[] {command});
        }
        public static void ConsoleCommandTest(string[] command) 
        {
            Printer.Log("Test");
        }
    }
}