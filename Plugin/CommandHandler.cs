using System;
using Dalamud.Game.Command;
using Penumbra.Api;
using Lumina.Excel.GeneratedSheets;
using System.Linq;
using Newtonsoft.Json;
namespace SmartModSwitch;

public sealed class CommandHandler : IDisposable {
    private const string CmdStringSMSW = "/smsw";
    private const string CmdStringDebug = "/smsw_debug";

    public CommandHandler() {
        SMSW.CommandManager.AddHandler(CmdStringSMSW, new CommandInfo(this.CommandHandlerSMSW) {
            HelpMessage = "A useful message to display in /xlhelp"
        });
        SMSW.CommandManager.AddHandler(CmdStringDebug, new CommandInfo(this.CommandHandlerDebug) {
            HelpMessage = "A useful message to display in /xlhelp"
        });
    }

    public void Dispose() {
        SMSW.CommandManager.RemoveHandler(CmdStringSMSW);
        SMSW.CommandManager.RemoveHandler(CmdStringDebug);
    }

    private void CommandHandlerSMSW(string command, string args) {
        SMSW.Logger.Info("CommandHandlerSMSW called");
        SMSW.UIManager.MainWindow.IsOpen = !SMSW.UIManager.MainWindow.IsOpen;
    }
    private void CommandHandlerDebug(string command, string args) {
        //helpful: https://github.com/Ottermandias/Penumbra.Api/blob/main/IPenumbraApi.cs
        SMSW.Logger.Info("CommandHandlerDebug called");
        var pGetColl = Ipc.GetCurrentCollectionName.Subscriber(SMSW.PluginInterface);
        var currentCollection = pGetColl!.Invoke();
        SMSW.Logger.Info("Current collection: {0}", currentCollection);

        var modlist = SMSW.PenumbraIPC.GetModList();
        SMSW.Logger.Info("Modlist:");
        foreach (var mod in modlist) {
            SMSW.Logger.Info("Mod: {0} {1}", mod.ModName, mod.PenumbraPath);
        }

        // var penumbraConfig = Ipc.GetConfiguration.Subscriber(smsw.PluginInterface).Invoke();
        // smsw.Logger.Info("Penumbra config: {0}", penumbraConfig);


        // var success = Ipc.TrySetMod.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base", "dances/memes/[OCN] Drop the bass","[OCN] Drop the bass",false);
        // smsw.Logger.Info("TrySetMod success: {0}", success);


        // var pGetChangedItems = Penumbra.Api.Ipc.GetChangedItems.Subscriber(smsw.PluginInterface);
        // var changedItems = pGetChangedItems!.Invoke(currentCollection);
        // foreach (var changedItem in changedItems)
        // {
        //     smsw.Logger.Info("Changed:  {0}  {1}", changedItem.Key, changedItem.Value ?? "");
        // }
        // var res = Ipc.GetChangedItems.Subscriber(smsw.PluginInterface).Invoke("Zfox Serious Base");
        // smsw.Logger.Info("Return value");
        // smsw.Logger.Info("res type: {0}", res.GetType());
        // foreach (var r in res)
        // {
        //     var name = r.Key;
        //     var obj = r.Value;
        //     if (obj == null)
        //     {
        //         continue;
        //     }
        //     if (obj is Emote)
        //     {
        //         var emote = (Emote)obj;
        //         smsw.Logger.Info("Changed emote: {0}", emote.Name);
        //         //var json = JsonConvert.SerializeObject(obj, Formatting.Indented, 
        //         //            new JsonSerializerSettings { 
        //         //                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore 
        //         //            });
        //         ////smsw.Logger.Info("Emote JSON: {0}", json);
        //         break;
        //     }
        // }

        // var res2 = Ipc.GetPlayerResourceTrees.Subscriber(smsw.PluginInterface).Invoke(true);
        // smsw.Logger.Info("Return value");
        // smsw.Logger.Info("res2 type: {0}", res2.GetType());
        // Ipc.ResourceTree tree = res2.Values.First();
        // smsw.Logger.Info("Resource tree: {0}", tree);
        // if (tree != null)
        // {
        //     foreach (var r in tree.Nodes)
        //     {
        //         var name = r.Name ?? "";
        //         var obj = r.ObjectAddress;
        //         smsw.Logger.Info("Resource: {0} {1}", name, obj);
        //     }
        // }
    }
}
