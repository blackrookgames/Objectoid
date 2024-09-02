using Executioner;
using System.Reflection;

namespace test
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            CommandExecutor commandExecutor = new CommandExecutor(assembly);
            commandExecutor.SyntaxPrefix = Path.GetFileNameWithoutExtension(assembly.Location);
            CommandExecutorResult result = commandExecutor.Run(args);
            switch (result.Code)
            {
                case CommandExecutorResultCode.Info:
                    Console.WriteLine(result.Message);
                    return 0;
                case CommandExecutorResultCode.Fail:
                    Console.WriteLine($"ERROR:\r\n{result.Message}");
                    return 1;
                default:
                    return 0;
            }
        }
    }
}