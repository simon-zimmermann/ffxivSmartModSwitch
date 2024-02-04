using System;
using System.Collections.Generic;
using System.Linq;
using Penumbra.Api;
using Penumbra.Api.Enums;

namespace SmartModSwitch.Interop;


public record PenumbraMod(string ModName, string FileSystemPath, string PenumbraPath);
public sealed class PenumbraIPC : IDisposable
{
    private readonly SmartModSwitch smsw;

    public PenumbraIPC(SmartModSwitch smsw)
    {
        this.smsw = smsw;
    }

    /// <summary>
    /// Builds a list of all available mods, and their penumbra directories
    /// </summary>
    /// <returns></returns>
    public List<PenumbraMod> GetModList()
    {
        var ret = new List<PenumbraMod>();
        var modNameList = Ipc.GetMods.Subscriber(smsw.PluginInterface).Invoke();
        foreach (var mod in modNameList)
        {
            var pathResult = Ipc.GetModPath.Subscriber(smsw.PluginInterface).Invoke(mod.Item1, mod.Item2);
            if (pathResult.Item1 == PenumbraApiEc.Success)
            {
                ret.Add(new PenumbraMod(mod.Item2, mod.Item1, pathResult.Item2));
            }
        }
        return ret;
    }
    public void Dispose()
    {
    }
}