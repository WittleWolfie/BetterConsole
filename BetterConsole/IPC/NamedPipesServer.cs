using H.Pipes.Args;
using H.Pipes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetterConsole.Common;
using H.Formatters;

namespace BetterConsole.IPC
{
  internal class NamedPipesServer
  {
    const string PIPE_NAME = "samplepipe";

    private PipeServer<PipeMessage> server;
    private MainPage Content;

    public NamedPipesServer(MainPage content)
    {
      Content = content;
    }

    public async Task InitializeAsync()
    {
      server = new PipeServer<PipeMessage>(PIPE_NAME, formatter: new NewtonsoftJsonFormatter());

      server.ClientConnected += async (o, args) => await OnClientConnectedAsync(args);
      server.ClientDisconnected += (o, args) => OnClientDisconnected(args);
      server.MessageReceived += (sender, args) => OnMessageReceived(args.Message);
      server.ExceptionOccurred += (o, args) => OnExceptionOccurred(args.Exception);

      await server.StartAsync();
    }

    private async Task OnClientConnectedAsync(ConnectionEventArgs<PipeMessage> args)
    {
      Content.ViewModel.Status = $"Client {args.Connection.PipeName} is now connected!";

      await args.Connection.WriteAsync(new PipeMessage
      {
        Action = ActionType.SendText,
        Text = "Hi from server"
      });
    }

    private void OnClientDisconnected(ConnectionEventArgs<PipeMessage> args)
    {
      Content.ViewModel.Status = $"Client {args.Connection.PipeName} disconnected";
    }

    private void OnMessageReceived(PipeMessage message)
    {
      switch (message.Action)
      {
        case ActionType.SendText:
          Content.ViewModel.Message = $"Text from client: {message.Text}";
          break;

        default:
          Content.ViewModel.Message = $"Unknown Action Type: {message.Action}";
          break;
      }
    }

    private void OnExceptionOccurred(Exception ex)
    {
      Content.ViewModel.Message = $"Exception occured in pipe: {ex}";
    }

    public void Dispose()
    {
      DisposeAsync().GetAwaiter().GetResult();
    }

    public async Task DisposeAsync()
    {
      if (server != null)
        await server.DisposeAsync();
    }
  }
}
