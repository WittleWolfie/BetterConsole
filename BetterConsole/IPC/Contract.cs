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
    /// Prefix for control commands which should not be logged to the console.
    /// </summary>
    public const string ControlPrefix = "CONTROL";
  }
}
