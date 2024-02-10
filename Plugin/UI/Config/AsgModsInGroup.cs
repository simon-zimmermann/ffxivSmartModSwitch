

using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.Interop;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI.Config;

public class AsgModsInGroup {
    private Asg group;
    private ModifyListWidget<AsgModsEntry> activeModsList;
    private readonly List<PenumbraMod> availableMods;
    public AsgModsInGroup(Asg group) {
        this.group = group;
        availableMods = SMSW.PenumbraIPC.GetModList();

        //re-create mod select window
        int selectedNewModIdx = -1;
        var modSelection = new SearchGroupedListWidget<PenumbraMod, string>("Select Mod", availableMods, []);
        activeModsList = new ModifyListWidget<AsgModsEntry>(group.Mods, "Active Mods") {
            OnItemClick = (entry, index) => {
                SMSW.Logger.Info($"Selected Mod entry: {entry}");
            },
            DrawCallAddItem = (firstOpen) => {
                modSelection.Draw(ref selectedNewModIdx);
                if (ImGui.Button("OK")) {
                    ImGui.CloseCurrentPopup();
                    var obj = new AsgModsEntry(availableMods[selectedNewModIdx]);
                    selectedNewModIdx = -1;
                    return obj;
                }

                return null;
            }
        };
    }

    public void Draw() {
        //Top: mod list
        activeModsList.Draw();

        //Bottom: mod settings
        var selectedItem = activeModsList.SelectedItem;
        if (selectedItem != null) {
            if (ImGui.Button("Test")) {
                SMSW.Logger.Info("Test button pressed");
                var notSelected = group.Mods.Where(x => x != selectedItem).ToList();
                SMSW.Logger.Info("Activating: {0}", selectedItem.Mod.ModName);
                foreach (var mod in notSelected) {
                    SMSW.Logger.Info("Deactivating: {0}", mod.Mod.ModName);
                }
                var emote = SMSW.GameData.emotes[group.EmoteIdx];
                SMSW.Logger.Info("Executing emote: {0}", emote.TextCommand.Value?.Command ?? "NO");
            }
        }

    }
}