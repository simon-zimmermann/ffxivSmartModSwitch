using System.Collections.Generic;
using ImGuiNET;
using System.Numerics;
using System;
using Dalamud.Interface;

namespace SmartModSwitch.UI.ImGuiExt;

public class ModifyListWidget<T> where T : class {
	private readonly List<T> items;
	/// Called when an item is clicked
	/// Parameters: clicked item, index of clicked item
	public Action<T, int> OnItemClick { private get; set; } = (item, index) => { };
	//callback to convert an item to a string
	public Func<T, string> ItemToString { private get; set; } = (val) => val.ToString() ?? "NULL";
	/// Called on click on the add button. Use it to draw contents of a poput, setaup is already done
	/// If the user has accepted, return a new item, which will be added to the list
	public Func<bool, T?> DrawCallAddItem { private get; set; } = (firstOpen) => { return null; };
	public T? SelectedItem {
		get {
			if (selectedIdx < 0 || selectedIdx >= items.Count)
				return null;
			else
				return items[selectedIdx];
		}
	}


	public readonly string title;
	private int selectedIdx = 0;
	public ModifyListWidget(List<T> items, string title) {
		this.items = items;
		this.title = title;
	}

	public void Draw() {
		bool triggerPopupOpen = false;
		ImGui.BeginGroup();

		//heading
		ImGui.Text(title);

		//buttons to the right of heading
		var buttonwidth = ImGuiUtil.IconButtonSize(FontAwesomeIcon.Plus).X + ImGuiUtil.DeleteButtonSize().X;
		ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonwidth);
		ImGui.CalcItemWidth();

		if (ImGuiUtil.IconButton(FontAwesomeIcon.Plus)) {
			ImGui.OpenPopup("input_popup");
			triggerPopupOpen = true;
		}
		ImGui.SameLine();
		if (ImGuiUtil.IconButtonDelete()) {
			items.RemoveAt(selectedIdx);
			selectedIdx -= 1;
			OnItemClick(items[selectedIdx], selectedIdx);
		}

		//main list
		ImGui.BeginListBox("", new Vector2(-1, 500));
		for (int n = 0; n < items.Count; n++) {
			string itemStr = ItemToString(items[n]);
			bool isSelected = selectedIdx == n;
			if (ImGui.Selectable($"{itemStr}##{n}", isSelected)) {
				selectedIdx = n;
				OnItemClick(items[n], n);
			}
		}
		ImGui.EndListBox();
		ImGui.EndGroup();


		//add item popup
		T? createdItem = null;
		if (ImGui.BeginPopup("input_popup", ImGuiUtil.defaultPopupFlags)) {
			createdItem = DrawCallAddItem(triggerPopupOpen);
			if (createdItem != null)
				items.Add(createdItem);
			ImGui.EndPopup();
		}
	}

}