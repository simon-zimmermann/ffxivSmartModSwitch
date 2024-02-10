

using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI.Config;

public class AsgGroupList {
    private readonly SearchGroupedListWidget<Emote, EmoteCategory> emoteSelection;
    public AsgGroupList() {
        emoteSelection = new SearchGroupedListWidget<Emote, EmoteCategory>("Select Emote", SMSW.GameData.emotes, SMSW.GameData.emoteCategories) {
            ItemToString = (entry) => { return entry.Name; },
            CategoryToString = (entry) => { return entry.Name; },
            IsItemInCategory = (item, category) => {
                EmoteCategory? itemcat = item.EmoteCategory.Value;
                if (itemcat != null)
                    return itemcat == category;
                else return false;
            }
        };
    }
    public void Draw(Asg currentGroup) {
        ImGui.Text("Assignment Group Settings");
        bool enabled = currentGroup.Enabled;
        if (ImGui.Checkbox("Enabled", ref enabled))
            currentGroup.Enabled = enabled;
        int selectedType = (int)currentGroup.Type;
        if (ImGuiUtil.Combo("Type", ref selectedType, Strings.UIAsgTypeStr)) {
            currentGroup.Type = (AsgType)selectedType;
        }
        switch (currentGroup.Type) {
            case AsgType.EMOTE:
                ImGui.Indent();
                int idx = currentGroup.EmoteIdx;
                if (emoteSelection.Draw(ref idx)) {
                    currentGroup.EmoteIdx = idx;
                }
                ImGui.Unindent();
                break;
            case AsgType.COMMAND:
                ImGui.Indent();
                //string enteredCommand = currentGroup.Command;
                //if (ImGui.InputText("Command", ref enteredCommand, 100)) {
                //	currentGroup.Command = enteredCommand;
                //}
                ImGui.Unindent();
                break;
        }
        int selectedReset = (int)currentGroup.Reset;
        if (ImGuiUtil.Combo("Reset Condition", ref selectedReset, Strings.UIAsgResetStr)) {
            currentGroup.Reset = (AsgReset)selectedReset;
        }
        if (currentGroup.Reset == AsgReset.TIMER) {
            ImGui.Indent();
            float enteredResetTime = currentGroup.ResetTime;
            if (ImGui.InputFloat("Reset Time [s]", ref enteredResetTime)) {
                currentGroup.ResetTime = enteredResetTime;
            }
            ImGui.Unindent();
        }
    }
}