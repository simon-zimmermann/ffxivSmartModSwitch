using System.Linq;
using System.Threading;
using Penumbra.Api;
using SmartModSwitch.Data;
using SmartModSwitch.Interop;

namespace SmartModSwitch;

public class AsgModExecutor {
    private readonly Asg group;
    private readonly AsgModsEntry entry;
    public AsgModExecutor(Asg group, AsgModsEntry mod) {
        this.group = group;
        this.entry = mod;
    }
    public void Execute() {
        SMSW.PenumbraIPC.SetModState(entry.PenumbraMod, true);

        var notSelected = group.Mods.Where(x => x != entry).ToList();
        foreach (var mod in notSelected) {
            SMSW.PenumbraIPC.SetModState(mod.PenumbraMod, false);
        }

        var emote = SMSW.GameData.emotes[group.EmoteIdx];
        SMSW.Logger.Info("Executing emote: {0}", emote.TextCommand.Value?.Command ?? "EMOTE COMMAND NOT FOUND");
        SMSW.ChatHelper.SendSanitizedChatMessage(emote.TextCommand.Value?.Command.ToString() ?? "");
    }
}