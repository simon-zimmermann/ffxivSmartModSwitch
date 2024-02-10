using System;
using System.Collections.Generic;
using System.Linq;
using Penumbra.Api;
using Penumbra.Api.Enums;

namespace SmartModSwitch.Interop;


public record PenumbraMod(string ModName, string FileSystemPath, string PenumbraPath);
public sealed class PenumbraIPC : IDisposable {

    /// <summary>
    /// Builds a list of all available mods, and their penumbra directories
    /// </summary>
    /// <returns></returns>
    public List<PenumbraMod> GetModList() {
        var ret = new List<PenumbraMod>();
        var modNameList = Ipc.GetMods.Subscriber(SMSW.PluginInterface).Invoke();
        foreach (var mod in modNameList) {
            var pathResult = Ipc.GetModPath.Subscriber(SMSW.PluginInterface).Invoke(mod.Item1, mod.Item2);
            if (pathResult.Item1 == PenumbraApiEc.Success) {
                ret.Add(new PenumbraMod(mod.Item2, mod.Item1, pathResult.Item2));
            }
        }
        return ret;
    }
    public void SetModState(PenumbraMod mod, bool state) {
        var collection = Ipc.GetCurrentCollectionName.Subscriber(SMSW.PluginInterface).Invoke();
        var success = Ipc.TrySetMod.Subscriber(SMSW.PluginInterface).Invoke(collection, mod.PenumbraPath, "", state);
        SMSW.Logger.Info($"Mod {mod.PenumbraPath} set to state {state} in collection {collection}; Success: {success}");
    }
    public void Dispose() {
    }
}