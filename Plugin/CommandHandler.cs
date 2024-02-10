using System;
using Dalamud.Game.Command;
using Penumbra.Api;
using Lumina.Excel.GeneratedSheets;
using System.Linq;
using Newtonsoft.Json;
namespace SmartModSwitch;

public sealed class CommandHandler : IDisposable {
    private const string CmdStringSMSW = "/smsw";

    public CommandHandler() {
        SMSW.CommandManager.AddHandler(CmdStringSMSW, new CommandInfo(this.CommandHandlerSMSW) {
            HelpMessage = "A useful message to display in /xlhelp"
        }); ;
    }

    public void Dispose() {
        SMSW.CommandManager.RemoveHandler(CmdStringSMSW);
    }

    private void CommandHandlerSMSW(string command, string args) {
        SMSW.Logger.Info("CommandHandlerSMSW called");
        //no args -> open config window
        if (args == "") {
            SMSW.UIManager.ConfigWindow.IsOpen = !SMSW.UIManager.ConfigWindow.IsOpen;
            return;
        }

        //find matchin AsgModEntry
        foreach (var asg in SMSW.Config.AssignmentGroups) {
            foreach (var mod in asg.Mods) {
                if (mod.Command == args) {
                    var executor = new AsgModExecutor(asg, mod);
                    executor.Execute();
                    return;
                }
            }
        }

        //if no match found, open config window
        SMSW.UIManager.ConfigWindow.IsOpen = true;

    }
}

