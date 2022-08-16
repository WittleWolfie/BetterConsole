using BetterConsole.Mod.IPC;
using HarmonyLib;
using System;
using static UnityModManagerNet.UnityModManager;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace BetterConsole.Mod
{
  public static class Main
  {
    public static ModLogger Logger;

    private static Harmony Harmony;

    public static bool Load(ModEntry modEntry)
    {
      try
      {
        Logger = modEntry.Logger;
        modEntry.OnUnload = OnUnload;

        Harmony = new Harmony(modEntry.Info.Id);
        Harmony.PatchAll();

        Client.Instance.Initialize();
        Owlcat.Runtime.Core.Logging.Logger.Instance.AddLogger(new ConsoleLogSink());

        Logger.Log("Finished patching.");
      }
      catch (Exception e)
      {
        Logger.LogException("Failed to patch", e);
      }
      return true;
    }

    private static bool OnUnload(ModEntry modEntry)
    {
      Client.Instance.Dispose();
      Harmony.UnpatchAll();
      return true;
    }
  }
}
