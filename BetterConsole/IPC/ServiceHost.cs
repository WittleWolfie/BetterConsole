using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BetterConsole.IPC
{
  internal class ServiceHost : ServiceBase
  {
    private static Thread serviceThread;
    private static bool stopping;
    private static MainPage Content;
    private static NamedPipesServer PipeServer;

    public ServiceHost(MainPage content)
    {
      Content = content;
      ServiceName = "Named Pipes Sample Service";
    }

    protected override void OnStart(string[] args)
    {
      Run(Content);
    }

    protected override void OnStop()
    {
      Abort();
    }

    protected override void OnShutdown()
    {
      Abort();
    }

    public static void Run(MainPage content)
    {
      Content = content;
      serviceThread = new Thread(InitializeServiceThread)
      {
        Name = "Named Pipes Sample Service Thread",
        IsBackground = true
      };
      serviceThread.Start();
    }

    public static void Abort()
    {
      stopping = true;
    }

    private static void InitializeServiceThread()
    {
      PipeServer = new(Content);
      PipeServer.InitializeAsync().GetAwaiter().GetResult();

      while (!stopping)
      {
        Task.Delay(100).GetAwaiter().GetResult();
      }
    }
  }
}
