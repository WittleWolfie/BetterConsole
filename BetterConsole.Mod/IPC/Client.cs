using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;

namespace BetterConsole.Mod.IPC
{
  public class Client : IDisposable
  {
    // Shared name between Client & Server
    private const string PipeName = "BetterConsole.Pipe";

    private static Client _instance;
    public static Client Instance => _instance ??= new();

    private bool Enabled = true;
    private NamedPipeClientStream Stream;
    private Thread Thread;
    private readonly ConcurrentQueue<string> LogQueue = new();

    public void Initialize()
    {
      if (Stream is not null) { return; }


      Stream = new(PipeName);
      Thread = new Thread(new ThreadStart(InitializeAsync));
      Thread.Start();
    }

    private void InitializeAsync()
    {
      Main.Logger.Log("Initiating mod connection.");
      while (Enabled)
      {

        try
        {
          Stream.Connect();

          Main.Logger.Log("Mod connection established.");
          ReportLog("Mod connection established.");

          WriteStream();
        }
        catch (Win32Exception)
        {
          // No server available
          Thread.Sleep(5000);
        }
        catch (Exception e)
        {
          Main.Logger.LogException("Error while connecting to BetterConsole.", e);
          break;
        }
      }
    }

    private void WriteStream()
    {
      using (var writer = new StreamWriter(Stream))
      {
        string line = null;
        while (Enabled && Stream.IsConnected)
        {
          if (LogQueue.Any() && LogQueue.TryDequeue(out line))
          {
            writer.WriteLine(line);
            writer.Flush();
            Main.Logger.Log($"Wrote line: {line}");
          }
          else
          {
            Main.Logger.Log("Waitin'");
            // Wait for more messages
            Thread.Sleep(1000);
          }
        }
      }
    }

    public void ReportLog(string text)
    {
      LogQueue.Enqueue(text);
    }

    public void Dispose()
    {
      Enabled = false;
      Stream?.Dispose();
      if ((bool)(Thread?.IsAlive))
      {
        Thread.Abort();
      }
    }
  }
}
