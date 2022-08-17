using BetterConsole.ViewModel;
using Newtonsoft.Json;
using System.IO.Pipes;
using static BetterConsole.Common.PipeContract;

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

    private static readonly JsonSerializer Serializer = new();

    private NamedPipeServerStream Stream;
    private LogWindowModel ViewModel;
    private Thread Thread;
    private bool Enabled = true;

    public void Initialize(LogWindowModel viewModel)
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
        ViewModel.LogLocal("Waiting for client connection.");
        Stream = new(PipeName);
        Stream.WaitForConnection();
        ViewModel.LogLocal("Client connected.");

        ReadStream();
      }
    }

    private void ReadStream()
    {
      using (var reader = new BinaryReader(Stream))
      {
        LogMessage message = new();
        while (Enabled && Stream.IsConnected)
        {
          try
          {
            message = JsonConvert.DeserializeObject<LogMessage>(reader.ReadString());
            // Ignore control messages
            if (!message.Control)
            {
              ViewModel.LogRemote(message);
            }
          }
          catch (Exception e)
          {
            ViewModel.LogLocal($"Exception reading remote logs: {e}");
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
