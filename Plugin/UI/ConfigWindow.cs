using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;
using SmartModSwitch.UI.ImGuiExt;

namespace SmartModSwitch.UI;

public class ConfigWindow : Window, IDisposable
{
	private readonly SmartModSwitch smsw;
	private SelectListComponent<AssignmentGroup> assignmentGroups;

	public ConfigWindow(SmartModSwitch smsw) : base(
		"ConfigWindow", ImGuiWindowFlags.NoCollapse)
	{
		this.smsw = smsw;
		//this.Size = new Vector2(232, 75);
		//this.SizeCondition = ImGuiCond.Always;
		assignmentGroups = new SelectListComponent<AssignmentGroup>(smsw.Config.AssignmentGroups, () => new AssignmentGroup("asd"));
	}

	public void Dispose() { }

	public override void Draw()
	{
		assignmentGroups.Draw();
		// can't ref a property, so use a local copy
		var configValue = smsw.Config.SomePropertyToBeSavedAndWithADefault;
		if (ImGui.Checkbox("Random Config Bool", ref configValue))
		{
			smsw.Config.SomePropertyToBeSavedAndWithADefault = configValue;
			// can save immediately on change, if you don't want to provide a "Save and Close" button
			smsw.Config.Save();
		}
	}
}
