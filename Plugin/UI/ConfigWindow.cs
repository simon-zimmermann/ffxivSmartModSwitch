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

namespace SmartModSwitch.UI;

public class ConfigWindow : Window, IDisposable {
	private readonly SmartModSwitch smsw;
	private readonly SelectListComponent<Asg> assignmentGroups;
	private readonly SelectListComponent<string> activeMods;
	private readonly EmoteSearchBox emoteSelection;
	private readonly List<string> tempActiveModsList = ["temp mod 1", "temp mod 2"];
	//private string tmp_input = "";

	public ConfigWindow(SmartModSwitch smsw) : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		this.smsw = smsw;
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		// UI
		string popup_value = "";
		assignmentGroups = new SelectListComponent<Asg>(smsw.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				smsw.Logger.Info($"Selected Assignment group: {entry}");
			},
			DrawCallPopup = () => {
				ImGui.InputText("Assignment group name", ref popup_value, 100);
				if (ImGui.Button("OK")) {
					ImGui.CloseCurrentPopup();
					var obj = new Asg(popup_value);
					popup_value = "";
					return obj;
				}
				return null;
			},
			DrawCallListItem = (entry, index, isSelected) => {
				return ImGui.Selectable($"{entry.Name}##{index}", isSelected);
			}
		};
		activeMods = new SelectListComponent<string>(tempActiveModsList, "Active Mods") {
			OnItemClick = (entry, index) => {
				smsw.Logger.Info($"Selected Mod entry: {entry}");
			},
			DrawCallPopup = () => {
				ImGui.InputText("Mod name", ref popup_value, 100);
				if (ImGui.Button("OK")) {
					ImGui.CloseCurrentPopup();
					var obj = popup_value;
					popup_value = "";
					return obj;
				}
				return null;
			},
			DrawCallListItem = (entry, index, isSelected) => {
				return ImGui.Selectable($"{entry}##{index}", isSelected);
			}
		};
		emoteSelection = new EmoteSearchBox(smsw.GameData);
	}

	public void Dispose() { }

	private void DrawAssgSettings(Asg currentGroup) {
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
				//string input = "";//smsw.GameData.emoteNames[idx];
				int category = 0;
				//TODO copy from https://github.com/BanditTech/Triggered/blob/4a19bd56fece77f30cd8b2be35965ad12091fb76/DropdownBoxUtility.cs
				//if (ImGuiUtil.DrawDropdownBox(ref tmp_input, ref category, ref idx, smsw.GameData.emoteNames, smsw.GameData.emoteCategoryNames)) {
				//	currentGroup.EmoteIdx = idx;
				//	tmp_input = smsw.GameData.emoteNames[idx];
				//	smsw.Logger.Info($"Selected Emote: {tmp_input}");
				//	smsw.Logger.Info($"Selected Emote Category: {smsw.GameData.emoteCategoryNames[category]}");
				//	smsw.Logger.Info($"Selected Emote Index: {idx}");
//
				//}
				if (emoteSelection.Draw(ref idx)) {
					currentGroup.EmoteIdx = idx;
					//tmp_input = smsw.GameData.emoteNames[idx];
					smsw.Logger.Info($"Selected Emote: {smsw.GameData.emoteNames[idx]}");
					smsw.Logger.Info($"Selected Emote Category: {smsw.GameData.emoteCategoryNames[category]}");
					smsw.Logger.Info($"Selected Emote Index: {idx}");
				}
				//ImGui.BeginListBox("Select Emote", new Vector2(-1, -1));
				//for (int n = 0; n < availableEmotes.Count; n++) {
				//	bool isSelected = currentGroup.EmoteIdx == n;
				//	if (ImGui.Selectable(availableEmotes[n].Name, isSelected)) {
				//		currentGroup.EmoteIdx = n;
				//	}
				//}
				//ImGui.EndListBox();
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
			activeMods.Draw();
			ImGui.EndChild();
		}

	}
}
