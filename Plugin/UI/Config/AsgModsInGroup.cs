

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
            var command = selectedItem.Command;
            if(ImGui.InputText("Command", ref command, 100))
                selectedItem.Command = command;
                
            if (ImGui.Button("Test")) {
                SMSW.Logger.Info("Test button pressed");
                var executor = new AsgModExecutor(group, selectedItem);
                executor.Execute();
            }
        }

    }
}