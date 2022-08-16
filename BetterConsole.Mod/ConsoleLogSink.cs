using BetterConsole.Mod.IPC;
using Owlcat.Runtime.Core.Logging;

namespace BetterConsole.Mod
{
  /// <summary>
  /// Forwards log events to BetterConsole using <see cref="IPC.Client"/>.
  /// </summary>
  public class ConsoleLogSink : ILogSink
  {
    public void Log(LogInfo logInfo)
    {
      if (!string.IsNullOrEmpty(logInfo?.Message))
      {
        Client.Instance.ReportLog(logInfo.Message);
      }
    }

    public void Destroy()
    {
      Main.Logger.Log("Destroying log sink.");
      Client.Instance.Dispose();
    }
  }
}
