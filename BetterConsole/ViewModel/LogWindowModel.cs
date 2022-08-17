using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using static BetterConsole.Common.PipeContract;

namespace BetterConsole.ViewModel
{
  /// <summary>
  /// View model for the LogWindow. Contains a list of log messages and applied filters.
  /// </summary>
  public class LogWindowModel : BindableObject
  {
    /// <summary>
    /// Maximum displayed messages after which they are removed.
    /// </summary>
    private const int MaxMessages = 5000;

    /// <summary>
    /// Currently displayed messages.
    /// </summary>
    public ObservableCollection<LogMessageModel> Messages { get; } = new();
    private readonly SynchronizationContext Context = SynchronizationContext.Current;

    public void LogLocal(string message)
    {
      Dispatcher.Dispatch(() => AddMessage(new LogMessageModel(message)));
    }

    public void LogRemote(LogMessage message)
    {
      Dispatcher.Dispatch(() => AddMessage(new LogMessageModel(message)));
    }

    private void AddMessage(LogMessageModel message)
    {
      Messages.Add(message);
      if (Messages.Count > MaxMessages)
      {
        Messages.RemoveAt(0);
      }
    }
  }
}
