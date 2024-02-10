using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI.Config;

public class AsgAddGroupPopup {
    private Asg newGroup = new();
    private readonly SearchGroupedListWidget<Emote, EmoteCategory> emoteSelectNewGroup;
    public AsgAddGroupPopup() {
        emoteSelectNewGroup = new SearchGroupedListWidget<Emote, EmoteCategory>("Select Emote", SMSW.GameData.emotes, SMSW.GameData.emoteCategories) {
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
    public Asg? Draw(bool firstOpen) {
        if (firstOpen) {
            ImGui.SetKeyboardFocusHere(0);
            emoteSelectNewGroup.Reset();
            newGroup = new Asg();
        }

        int selectedType = (int)newGroup.Type;
        if (ImGuiUtil.Combo("Type", ref selectedType, Strings.UIAsgTypeStr))
            newGroup.Type = (AsgType)selectedType;
        switch (selectedType) {
            case (int)AsgType.NOTHING:
                break;
            case (int)AsgType.EMOTE:
                int emoteIdx = newGroup.EmoteIdx;
                if (emoteSelectNewGroup.Draw(ref emoteIdx))
                    newGroup.EmoteIdx = emoteIdx;
                break;
            case (int)AsgType.COMMAND:
                ImGui.Text("Command: (TODO)");
                break;
        }

        if (ImGui.Button("OK")) {
            ImGui.CloseCurrentPopup();
            return newGroup;
        }
        return null;
    }
}