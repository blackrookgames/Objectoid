using Executioner;
using System.Reflection;

namespace test
{
    internal class Program
    {
        static int Main(string[] args)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string prefix = Path.GetFileNameWithoutExtension(assembly.Location);

            CommandExecutor commandExecutor = new CommandExecutor(assembly);
            CommandExecutorResult result = commandExecutor.Run(args, syntaxPrefix: prefix);
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