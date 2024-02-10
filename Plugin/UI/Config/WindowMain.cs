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

	private AsgGroupList asgSettings;
	private AsgModsInGroup? asgActiveModList;
	private Asg newGroup = new Asg();

	public WindowMain() : base(
		"SmartModSwitch Config", ImGuiWindowFlags.NoCollapse) {
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(800, 600),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		// UI
		string popup_value = "";
		assignmentGroups = new ModifyListWidget<Asg>(SMSW.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				AssignmentGroupSelected(entry);
			},
			DrawCallAddItem = DrawAddItem
		};
		asgSettings = new AsgGroupList();

		AssignmentGroupSelected(assignmentGroups.SelectedItem);
	}
	private void AssignmentGroupSelected(Asg? group) {
		if (group == null)
			return;
		SMSW.Logger.Info($"Selected Assignment group: {group.ToString()}");
		asgActiveModList = new AsgModsInGroup(group);
	}
	public void Dispose() { }

	private Asg? DrawAddItem(bool firstOpen) {
		if (firstOpen)
			ImGui.SetKeyboardFocusHere(0);
		//ImGui.InputText("Assignment group name", ref popup_value, 100);

		int selectedType = (int)newGroup.Type;
		if (ImGuiUtil.Combo("Type", ref selectedType, Strings.UIAsgTypeStr)) {
			newGroup.Type = (AsgType)selectedType;
		}
		if (ImGui.Button("OK")) {
			ImGui.CloseCurrentPopup();
			//var obj = new Asg(popup_value);
			//popup_value = "";
			return newGroup;
		}
		return null;
	}

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
