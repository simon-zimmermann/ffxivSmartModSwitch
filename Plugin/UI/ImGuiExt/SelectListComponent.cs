using System.Collections.Generic;
using ImGuiNET;
using System.Numerics;
using System;
using Dalamud.Interface;

namespace SmartModSwitch.UI.ImGuiExt;

public class SelectListComponent<T> : IUiWidget where T : class {
	private List<T> items;
	private int selectedIdx = 0;
	private Action<T, int> onClick;
	private Func<T, int, bool, bool>? drawOverride;
	private Func<T> createEntry;
	public SelectListComponent(List<T> items, Action<T, int> onClick, Func<T> createEntry, Func<T, int, bool, bool>? drawOverride = null) {
		this.items = items;
		this.onClick = onClick;
		this.drawOverride = drawOverride;
		this.createEntry = createEntry;
	}

	public void Draw() {
		ImGui.BeginGroup();
		ImGui.Text("Assignment Groups");
		var buttonwidth = ImGuiUtil.AddButtonSize() + ImGuiUtil.DeleteButtonSize();
		ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonwidth);
		ImGui.CalcItemWidth();

		if (ImGuiUtil.AddButton()) {
			items.Add(createEntry());
		}
		ImGui.SameLine();
		if (ImGuiUtil.DeleteButton()) {
			items.RemoveAt(selectedIdx);
		}
		ImGui.BeginListBox("", new Vector2(-1, 500));
		for (int n = 0; n < items.Count; n++) {
			bool isSelected = selectedIdx == n;
			bool entrySelected = false;
			if (drawOverride != null)
				entrySelected = drawOverride(items[n], n, isSelected);
			else
				entrySelected = ImGui.Selectable($"{n}: {items[n]}", isSelected);


			if (entrySelected) {
				selectedIdx = n;
				onClick(items[n], n);
			}
		}
		ImGui.EndListBox();
		ImGui.EndGroup();
	}
}