using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole.IPC
{
  /// <summary>
  /// Should be identical to Contract.cs in BetterConsole.Mod. Holds common constants.
  /// </summary>
  public static class Contract
  {
    public const string PipeName = "BetterConsole.Pipe";

    /// <summary>
    /// Struct with log message details. Used to serialize/deserialize JSON.
    /// </summary>
    public struct LogMessage
    {
      public bool Control;
      public string ChannelName;
      public string Severity;
      public List<string> Message;
    }
  }
}
