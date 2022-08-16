using System.IO.Pipes;

namespace BetterConsole.IPC
{
  public class Server : IDisposable
  {
    // Shared name between Client & Server
    private const string PipeName = "BetterConsole.Pipe";

    private static Server _instance;
    public static Server Instance => _instance ??= new();

    private NamedPipeServerStream Stream;
    private ContentViewModel ViewModel;
    private Thread Thread;
    private bool Enabled = true;

    public void Initialize(ContentViewModel viewModel)
    {
      ViewModel = viewModel;
      if (Stream is not null) { return; }

      Stream = new(PipeName);
      Thread = new Thread(new ThreadStart(InitializeAsync));
      Thread.Start();
    }

    private void InitializeAsync()
    {
      ViewModel.Status = "Waiting for client connection.";
      while (Enabled)
      {
        if (!Stream.IsConnected)
        {
          Stream.Dispose();
          Stream = new(PipeName);
          Stream.WaitForConnection();
          ViewModel.Status = "Client connected.";
        }

        ReadStream();
      }
    }

    private void ReadStream()
    {
      using (var reader = new StreamReader(Stream))
      {
        string line = null;
        while (Enabled && Stream.IsConnected)
        {
          line = reader.ReadLine();
          if (string.IsNullOrEmpty(line))
          {
            // Wait for more messages
            Thread.Sleep(1000);
          }
          else
          {
            ViewModel.Message = line;
          }
        }
      }
    }

    public void Dispose()
    {
      Enabled = false;
      Stream?.Dispose();
    }
  }
}
