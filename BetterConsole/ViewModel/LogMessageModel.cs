using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BetterConsole.Common.PipeContract;

namespace BetterConsole.ViewModel
{
  /// <summary>
  /// Represents an individual Log Message view in LogWindow.
  /// </summary>
  public class LogMessageModel
  {
    public string Timestamp { get; }
    public string Severity { get; }
    public string Channel { get; }
    public string Message { get; }

    public LogMessageModel(LogMessage logMessage)
    {
      Timestamp = logMessage.Timestamp;
      Severity = logMessage.Severity;
      Channel = logMessage.ChannelName;
      Message = string.Join("\n", logMessage.Message);
    }

    /// <summary>
    /// For local messages.
    /// </summary>
    public LogMessageModel(string localMessage)
    {
      Timestamp = string.Empty;
      Severity = string.Empty;
      Channel = "Local";
      Message = localMessage;
    }
  }
}
