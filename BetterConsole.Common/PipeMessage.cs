using System;

namespace BetterConsole.Common
{
  public enum ActionType
  {
    Unknown,
    SendText
  }

  [Serializable]
  public class PipeMessage
  {
    public Guid Id { get; set; }
    public ActionType Action { get; set; }
    public string Text { get; set; }

    public PipeMessage()
    {
      Id = Guid.NewGuid();
    }
  }
}
