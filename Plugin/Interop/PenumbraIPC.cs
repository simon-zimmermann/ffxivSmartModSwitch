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
        (var valid, var individual, var collection) = Ipc.GetCollectionForObject.Subscriber(SMSW.PluginInterface).Invoke(0); //0 => current character
        if (!valid) {
            SMSW.Logger.Error("Failed to get collection for current character");
            return;
        }
        var success = Ipc.TrySetMod.Subscriber(SMSW.PluginInterface).Invoke(collection, mod.PenumbraPath, mod.ModName, state);
        SMSW.Logger.Info($"Mod {mod.PenumbraPath} set to state {state} in collection {collection} (individual: {individual}); Success: {success}");
    }
    public bool GetPenumbraEnabled() {
        return Ipc.GetEnabledState.Subscriber(SMSW.PluginInterface).Invoke();
    }
    public void Dispose() {
    }
}