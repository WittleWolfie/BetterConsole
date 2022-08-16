using System.IO.Pipes;

namespace BetterConsole.IPC
{
  /// <summary>
  /// Server for BetterConsole. BetterConsole.Mod acts as a client sending log events.
  /// </summary>
  /// 
  /// <remarks>
  /// Originally I tried to use H.Pipes which provides a wrapper for easier use. Unfortunately it is built entirely
  /// on async pipes which is not supported in Unity. Now it's using RawPipes though converting to JSON would be easy
  /// if needed.
  /// </remarks>
  public class Server : IDisposable
  {
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

      Thread = new Thread(new ThreadStart(InitializeAsync));
      Thread.Start();
    }

    /// <summary>
    /// Since async pipes aren't available just loop waiting for input in a thread. This is the outer loop which
    /// connects and reconnects, ReadStream is the inner loop which reads input.
    /// </summary>
    private void InitializeAsync()
    {
      while (Enabled)
      {
        Stream?.Dispose();
        ViewModel.Status = "Waiting for client connection.";
        Stream = new(Contract.PipeName);
        Stream.WaitForConnection();
        ViewModel.Status = "Client connected.";

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
          else if (!line.StartsWith(Contract.ControlPrefix))
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
