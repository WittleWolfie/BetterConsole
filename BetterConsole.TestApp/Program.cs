using System;

namespace BetterConsole.TestApp
{
  internal class Program
  {
    static void Main(string[] args)
    {
      NamedPipesClient.Instance.InitializeAsync();

      while (true)
      {
        Console.WriteLine("Enter text to send.");
        var text = Console.ReadLine();
        NamedPipesClient.Instance.SendMessage(text);

        Console.WriteLine("Hit ESC to quit or any key to continue: ");
        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
        {
          break;
        }
      }

      NamedPipesClient.Instance.Dispose();
      Console.WriteLine("Goodbye!");
    }
  }
}
