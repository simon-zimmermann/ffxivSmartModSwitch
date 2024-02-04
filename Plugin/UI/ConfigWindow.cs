using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;
using Dalamud.Interface;

namespace SmartModSwitch.UI;

public class ConfigWindow : Window, IDisposable {
	private readonly SmartModSwitch smsw;
	private readonly SelectListComponent<AssignmentGroup> assignmentGroups;
	private readonly SelectListComponent<string> activeMods;
	private readonly List<string> tempActiveModsList = ["temp mod 1", "temp mod 2"];

	public ConfigWindow(SmartModSwitch smsw) : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		this.smsw = smsw;
		this.SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		string popup_value = "";
		assignmentGroups = new SelectListComponent<AssignmentGroup>(smsw.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				smsw.Logger.Info($"Selected Assignment group: {entry}");
			},
			DrawCallPopup = () => {
				ImGui.InputText("Assignment group name", ref popup_value, 100);
				if (ImGui.Button("OK")) {
					ImGui.CloseCurrentPopup();
					var obj = new AssignmentGroup(popup_value);
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

	private void DrawAssignmentGoupSettings(AssignmentGroup currentGroup) {
		ImGui.Text("Assignment Group Settings");
		bool enabled = currentGroup.Enabled;
		if (ImGui.Checkbox("Enabled", ref enabled))
			currentGroup.Enabled = enabled;

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
			DrawAssignmentGoupSettings(assignmentGroups.SelectedItem);
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
