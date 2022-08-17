﻿using Newtonsoft.Json;
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
        Stream = new(PipeName);
        Stream.WaitForConnection();
        ViewModel.Status = "Client connected.";

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
            if (message.Control)
            {
              // Ignore control messages
              continue;
            }

            if (message.Message is not null && message.Message.Any())
            {
              ViewModel.Message = message.Message.First();
            }
          }
          catch (Exception)
          {
            // TODO: Create some kind of console logging
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
