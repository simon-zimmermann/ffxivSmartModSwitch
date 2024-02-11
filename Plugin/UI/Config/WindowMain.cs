using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;
using Dalamud.Interface;

namespace SmartModSwitch.UI.Config;

public sealed class WindowMain : Window, IDisposable {
	private readonly ModifyListWidget<Asg> assignmentGroups;

	private AsgGroupDetails? asgGroupDetails;
	private readonly AsgAddGroupPopup asgAddGroupPopup;

	public WindowMain() : base("SmartModSwitch Config") {
		SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(300, 300),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		// UI
		asgAddGroupPopup = new AsgAddGroupPopup();
		assignmentGroups = new ModifyListWidget<Asg>(SMSW.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				AssignmentGroupSelected(entry);
			},
			DrawCallAddItem = asgAddGroupPopup.Draw,
			SaveCallback = SMSW.Config.Save
		};


		AssignmentGroupSelected(assignmentGroups.SelectedItem);
	}
	private void AssignmentGroupSelected(Asg? group) {
		if (group == null)
			return;
		asgGroupDetails = new AsgGroupDetails(group);
	}
	public void Dispose() { }


	public override void Draw() {
		{
			Vector2 buttonSize = ImGuiUtil.IconButtonSize(FontAwesomeIcon.Save, "Save");
			ImGui.BeginChild("top", new Vector2(-1, buttonSize.Y));
			ImGui.Text("Create and select assignment groups on the left, assign mods on the right.");
			ImGui.EndChild();
		}
		{
			ImGui.BeginChild("left", new Vector2(ImGui.GetContentRegionAvail().X * 0.5f, -1));
			assignmentGroups.Draw();
			ImGui.Separator();
			if (assignmentGroups.SelectedItem != null)
				asgGroupDetails?.DrawGroupSettings();
			ImGui.EndChild();
		}
		ImGui.SameLine();
		{
			ImGui.BeginChild("right", new Vector2(ImGui.GetContentRegionAvail().X, -1));
			asgGroupDetails?.DrawModList();
			asgGroupDetails?.DrawModSettings();
			ImGui.EndChild();
		}

	}
}
