using BetterConsole.Common;
using H.Formatters;
using H.Pipes;
using System;
using System.Threading.Tasks;

namespace BetterConsole.TestApp
{
  public class NamedPipesClient : IDisposable
  {
    const string pipeName = "samplepipe";

    private static NamedPipesClient instance;
    private PipeClient<PipeMessage> client;

    public static NamedPipesClient Instance
    {
      get
      {
        return instance ?? new NamedPipesClient();
      }
    }

    private NamedPipesClient()
    {
      instance = this;
    }
    public async Task InitializeAsync()
    {
      if (client != null && client.IsConnected)
        return;

      client = new PipeClient<PipeMessage>(pipeName, formatter: new NewtonsoftJsonFormatter());
      client.MessageReceived += (sender, args) => OnMessageReceived(args.Message);
      client.Disconnected += (o, args) => Console.WriteLine("Disconnected from server");
      client.Connected += (o, args) => Console.WriteLine("Connected to server");
      client.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

      await client.ConnectAsync();

      await client.WriteAsync(new PipeMessage
      {
        Action = ActionType.SendText,
        Text = "Hello from client",
      });
    }

    public async void SendMessage(string text)
    {
      await client.WriteAsync(new PipeMessage
      {
        Action = ActionType.SendText,
        Text = text,
      });
    }

    private void OnMessageReceived(PipeMessage message)
    {
      switch (message.Action)
      {
        case ActionType.SendText:
          Console.WriteLine(message.Text);
          break;
        default:
          Console.WriteLine($"Method {message.Action} not implemented");
          break;
      }
    }

    private void OnExceptionOccurred(Exception exception)
    {
      Console.WriteLine($"An exception occured: {exception}");
    }

    public void Dispose()
    {
      if (client != null)
        client.DisposeAsync().GetAwaiter().GetResult();
    }
  }
}
