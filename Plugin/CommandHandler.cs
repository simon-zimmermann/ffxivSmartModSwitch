using System;
using Dalamud.Game.Command;
namespace SmartModSwitch;

public sealed class CommandHandler : IDisposable
{
    private readonly SmartModSwitch smsw;

    private const string CmdStringSMSW = "/smsw";

    public CommandHandler(SmartModSwitch smsw)
    {
        this.smsw = smsw;

        smsw.CommandManager.AddHandler(CmdStringSMSW, new CommandInfo(this.CommandHandlerSMSW)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });
    }

    public void Dispose()
    {
        smsw.CommandManager.RemoveHandler(CmdStringSMSW);
    }

    private void CommandHandlerSMSW(string command, string args)
    {
        smsw.Logger.Info("CommandHandlerSMSW called");
        smsw.UIManager.MainWindow.IsOpen = !smsw.UIManager.MainWindow.IsOpen;
    }
}
