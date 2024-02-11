using System.Collections.Generic;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.Interop;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI.Config;

public class AsgGroupDetails {
    private Asg group;
    private ModifyListWidget<AsgModsEntry> activeModsList;
    private readonly List<PenumbraMod> availableMods;
    public AsgGroupDetails(Asg group) {
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
            },
            SaveCallback = SMSW.Config.Save
        };
    }

    public void DrawModList() {
        //Top: mod list
        activeModsList.Draw();
    }
    public void DrawModSettings() {
        //Bottom: mod settings
        var selectedItem = activeModsList.SelectedItem;
        if (selectedItem != null) {
            var command = selectedItem.Command;
            if (ImGui.InputText("Command", ref command, 100)) {
                selectedItem.Command = command;
                SMSW.Config.Save();
            }

            if (ImGui.Button("Test")) {
                SMSW.Logger.Info("Test button pressed");
                var executor = new AsgModExecutor(group, selectedItem);
                executor.Execute();
            }
        }
    }
    
    public void DrawGroupSettings() {
        ImGui.Text("Assignment Group Settings");
        bool enabled = group.Enabled;
        if (ImGui.Checkbox("Enabled", ref enabled)) {
            group.Enabled = enabled;
            SMSW.Config.Save();
        }

        int selectedReset = (int)group.Reset;
        if (ImGuiUtil.Combo("Reset Condition", ref selectedReset, Strings.UIAsgResetStr)) {
            group.Reset = (AsgReset)selectedReset;
            SMSW.Config.Save();
        }
        if (group.Reset == AsgReset.TIMER) {
            ImGui.Indent();
            float enteredResetTime = group.ResetTime;
            if (ImGui.InputFloat("Reset Time [s]", ref enteredResetTime)) {
                group.ResetTime = enteredResetTime;
                SMSW.Config.Save();
            }
            ImGui.Unindent();
        }
    }
}