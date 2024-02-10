using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;
using Dalamud.Interface;
using System.Linq;
using Lumina.Excel.GeneratedSheets;
using SmartModSwitch.Interop;

namespace SmartModSwitch.UI.ConfigWindow;

public class ConfigWindowMain : Window, IDisposable {
	private readonly SmartModSwitch smsw;
	private readonly ModifyListWidget<Asg> assignmentGroups;
	private Asg? selectedGroup;
	private ModifyListWidget<AsgModsEntry>? activeModsList;
	private readonly SearchGroupedListWidget<Emote, EmoteCategory> emoteSelection;
	private readonly List<PenumbraMod> availableMods;

	public ConfigWindowMain(SmartModSwitch smsw) : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		this.smsw = smsw;
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};
		availableMods = smsw.PenumbraIPC.GetModList();

		// UI
		string popup_value = "";
		assignmentGroups = new ModifyListWidget<Asg>(smsw.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				AssignmentGroupSelected(entry);
			},
			DrawCallAddItem = (firstOpen) => {
				if (firstOpen)
					ImGui.SetKeyboardFocusHere(0);
				ImGui.InputText("Assignment group name", ref popup_value, 100);
				if (ImGui.Button("OK")) {
					ImGui.CloseCurrentPopup();
					var obj = new Asg(popup_value);
					popup_value = "";
					return obj;
				}
				return null;
			}
		};
		emoteSelection = new SearchGroupedListWidget<Emote, EmoteCategory>("Select Emote", smsw.GameData.emotes, smsw.GameData.emoteCategories) {
			ItemToString = (entry) => { return entry.Name; },
			CategoryToString = (entry) => { return entry.Name; },
			IsItemInCategory = (item, category) => {
				EmoteCategory? itemcat = item.EmoteCategory.Value;
				if (itemcat != null)
					return itemcat == category;
				else return false;
			}
		};
		AssignmentGroupSelected(assignmentGroups.SelectedItem);
	}
	private void AssignmentGroupSelected(Asg? group) {
		if (group == null)
			return;
		smsw.Logger.Info($"Selected Assignment group: {group.Name}");
		selectedGroup = group;

		//re-create mod select window
		int selectedNewModIdx = -1;
		var modSelection = new SearchGroupedListWidget<PenumbraMod, string>("Select Mod", availableMods, []);
		activeModsList = new ModifyListWidget<AsgModsEntry>(selectedGroup.Mods, "Active Mods") {
			OnItemClick = (entry, index) => {
				smsw.Logger.Info($"Selected Mod entry: {entry}");
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
	public void Dispose() { }

	private void DrawAssgSettings(Asg? currentGroup) {
		if (currentGroup == null)
			return;
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

	public override void Draw() {
		{
			Vector2 buttonSize = ImGuiUtil.IconButtonSize(FontAwesomeIcon.Save, "Save");
			ImGui.BeginChild("top", new Vector2(-1, buttonSize.Y));
			ImGui.Text("Create and select assignment groups on the left, assign mods on the right.");
			ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonSize.X);
			if (ImGuiUtil.IconButton(FontAwesomeIcon.Save, "Save")) {
				smsw.Config.Save();
			}
			ImGui.EndChild();
		}
		{
			ImGui.BeginChild("left", new Vector2(ImGui.GetContentRegionAvail().X * 0.5f, -1));
			assignmentGroups.Draw();
			ImGui.Separator();
			DrawAssgSettings(assignmentGroups.SelectedItem);
			ImGui.EndChild();
		}
		ImGui.SameLine();
		{
			ImGui.BeginChild("right", new Vector2(ImGui.GetContentRegionAvail().X, -1));
			if (selectedGroup != null && activeModsList != null) {
				var selectedItem = activeModsList.SelectedItem;
				activeModsList.Draw();
				if (selectedItem != null) {
					if (ImGui.Button("Test")) {
						smsw.Logger.Info("Test button pressed");
						var notSelected = selectedGroup.Mods.Where(x => x != selectedItem).ToList();
						smsw.Logger.Info("Activating: {0}", selectedItem.Mod.ModName);
						foreach (var mod in notSelected) {
							smsw.Logger.Info("Deactivating: {0}", mod.Mod.ModName);
						}
						var emote = smsw.GameData.emotes[selectedGroup.EmoteIdx];
						smsw.Logger.Info("Executing emote: {0}", emote.TextCommand.Value?.Command ?? "NO");
					}
				}
			}

			ImGui.EndChild();
		}

	}
}
