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

namespace SmartModSwitch.UI.Config;

public class WindowMain : Window, IDisposable {
	private readonly ModifyListWidget<Asg> assignmentGroups;

	private AsgSettings asgSettings;
	private AsgModsInGroup? asgActiveModList;
	private AsgAddGroupPopup asgAddGroupPopup;

	public WindowMain() : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		// UI
		asgAddGroupPopup = new AsgAddGroupPopup();
		assignmentGroups = new ModifyListWidget<Asg>(SMSW.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				AssignmentGroupSelected(entry);
			},
			DrawCallAddItem = asgAddGroupPopup.Draw
		};
		asgSettings = new AsgSettings();


		AssignmentGroupSelected(assignmentGroups.SelectedItem);
	}
	private void AssignmentGroupSelected(Asg? group) {
		if (group == null)
			return;
		SMSW.Logger.Info($"Selected Assignment group: {group.ToString()}");
		asgActiveModList = new AsgModsInGroup(group);
	}
	public void Dispose() { }


	public override void Draw() {
		{
			Vector2 buttonSize = ImGuiUtil.IconButtonSize(FontAwesomeIcon.Save, "Save");
			ImGui.BeginChild("top", new Vector2(-1, buttonSize.Y));
			ImGui.Text("Create and select assignment groups on the left, assign mods on the right.");
			ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonSize.X);
			if (ImGuiUtil.IconButton(FontAwesomeIcon.Save, "Save")) {
				SMSW.Config.Save();
			}
			ImGui.EndChild();
		}
		{
			ImGui.BeginChild("left", new Vector2(ImGui.GetContentRegionAvail().X * 0.5f, -1));
			assignmentGroups.Draw();
			ImGui.Separator();
			if (assignmentGroups.SelectedItem != null)
				asgSettings.Draw(assignmentGroups.SelectedItem);
			ImGui.EndChild();
		}
		ImGui.SameLine();
		{
			ImGui.BeginChild("right", new Vector2(ImGui.GetContentRegionAvail().X, -1));
			asgActiveModList?.Draw();
			ImGui.EndChild();
		}

	}
}
