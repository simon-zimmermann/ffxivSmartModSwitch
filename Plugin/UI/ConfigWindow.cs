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
	private readonly List<string> tempActiveModsList = ["temp mod 1", "temp mod 2"];
	private readonly List<Emote> availableEmotes = [];

	public ConfigWindow(SmartModSwitch smsw) : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		this.smsw = smsw;
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		var emoteSheet = smsw.DataManager.GetExcelSheet<Emote>();
		if (emoteSheet != null)
			foreach (var item in emoteSheet)
				if (item.Name != null && item.Name != "")
					availableEmotes.Add(item);



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
				string input = "";
				int idx=0;
				ImGuiUtil.DrawDropdownBox(ref input, ref idx);
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
