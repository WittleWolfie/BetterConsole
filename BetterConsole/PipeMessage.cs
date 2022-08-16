using System;

namespace BetterConsole.Common
{
  /// <summary>
  /// Enum shared between BetterConsole and BetterConsole.Mod for networking API.
  /// </summary>
  public enum ActionType
  {
    Unknown,
    SendText
  }

  /// <summary>
  /// Class shared between BetterConsole and BetterConsole.Mod for networking API.
  /// </summary>
  [Serializable]
  public class PipeMessage
  {
    public const string PipeName = "BetterConsole.Pipe";

    public ActionType Action { get; set; }
    public string Text { get; set; }
  }
}
