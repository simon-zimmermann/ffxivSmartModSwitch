using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI;

public class ConfigWindow : Window, IDisposable {
	private readonly SmartModSwitch smsw;
	private readonly SelectListComponent<AssignmentGroup> assignmentGroups;

	public ConfigWindow(SmartModSwitch smsw) : base(
		"ConfigWindow", ImGuiWindowFlags.NoCollapse) {
		this.smsw = smsw;

		string popup_value = "";
		assignmentGroups = new SelectListComponent<AssignmentGroup>(smsw.Config.AssignmentGroups, "Assignment Groups") {
			OnItemClick = (entry, index) => {
				smsw.Logger.Info($"Selected: {entry}");
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
	}

	public void Dispose() { }

	public override void Draw() {
		assignmentGroups.Draw();



		// can't ref a property, so use a local copy
		var configValue = smsw.Config.SomePropertyToBeSavedAndWithADefault;
		if (ImGui.Checkbox("Random Config Bool", ref configValue)) {
			smsw.Config.SomePropertyToBeSavedAndWithADefault = configValue;
			// can save immediately on change, if you don't want to provide a "Save and Close" button
			smsw.Config.Save();
		}
	}
}
