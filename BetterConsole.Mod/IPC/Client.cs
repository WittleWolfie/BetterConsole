using BetterConsole.Common;
using H.Formatters;
using H.Pipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole.Mod.IPC
{
  public class Client : IDisposable
  {
    private static Client _instance;
    public static Client Instance => _instance ??= new Client();

    private PipeClient<PipeMessage> BackingClient;

    public async Task InitializeAsync()
    {
      if (BackingClient is not null && BackingClient.IsConnected) { return; }

      Main.Logger.Log("Initiating mod connection.");

      BackingClient = new(PipeMessage.PipeName, formatter: new NewtonsoftJsonFormatter());
      BackingClient.Disconnected += (o, args) => Main.Logger.Log("Disconnected from server.");
      BackingClient.Connected += (o, args) => Main.Logger.Log("Connected to server.");
      BackingClient.ExceptionOccurred += (o, args) => OnException(args.Exception);

      await BackingClient.ConnectAsync();

      Main.Logger.Log("Mod connection established.");
      SendText("Mod connection established.");
    }

    public async void SendText(string text)
    {
      await BackingClient.WriteAsync(new()
      {
        Action = ActionType.SendText,
        Text = text
      });
    }

    private void OnException(Exception e)
    {
      Main.Logger.LogException("PipeClient triggered an exception.", e);
    }

    public void Dispose()
    {
      if (BackingClient is not null)
      {
        BackingClient.DisposeAsync().GetAwaiter().GetResult();
      }
    }
  }
}
