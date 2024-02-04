using System.Collections.Generic;
using ImGuiNET;
using System.Numerics;
using System;
using Dalamud.Interface;

namespace SmartModSwitch.UI.ImGuiExt;

public class SelectListComponent<T> : IUiWidget where T : class {
	private readonly List<T> items;
	/// <summary>
	/// Called when an item is clicked
	/// Parameters: clicked item, index of clicked item
	/// </summary>
	public Action<T, int> OnItemClick { private get; set; }
	/// <summary>
	/// Called to draw the item, optional
	/// Parameters: item to draw, index of item to draw, whether the item is selected
	/// Return true if the user has selected the item, false otherwise
	/// </summary>
	public Func<T, int, bool, bool> DrawCallListItem { private get; set; }
	/// <summary>
	/// Called on click on the add button. Use it to draw contents of a poput, setaup is already done
	/// If the user has accepted, return a new item, which will be added to the list
	/// </summary>
	public Func<T?> DrawCallPopup { private get; set; }
	public T SelectedItem { get { return items[selectedIdx]; } }


	public readonly string title;
	private int selectedIdx = 0;
	public SelectListComponent(List<T> items, string title) {
		this.items = items;
		this.title = title;

		OnItemClick = (item, index) => { };
		DrawCallListItem = (item, index, isSelected) => {
			return ImGui.Selectable($"{item}##{index}", isSelected);
		};
		DrawCallPopup = () => {
			return null;
		};
	}

	public void Draw() {
		//add item popup
		T? createdItem = null;
		if (ImGui.BeginPopup("input_popup")) {
			createdItem = DrawCallPopup();
			if (createdItem != null)
				items.Add(createdItem);
			ImGui.EndPopup();
		}

		ImGui.BeginGroup();

		//heading
		ImGui.Text(title);

		//buttons to the right of heading
		var buttonwidth = ImGuiUtil.IconButtonSize(FontAwesomeIcon.Plus).X + ImGuiUtil.DeleteButtonSize().X;
		ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonwidth);
		ImGui.CalcItemWidth();

		if (ImGuiUtil.IconButton(FontAwesomeIcon.Plus)) {
			ImGui.OpenPopup("input_popup");
		}
		ImGui.SameLine();
		if (ImGuiUtil.IconButtonDelete()) {
			items.RemoveAt(selectedIdx);
		}

		//main list
		ImGui.BeginListBox("", new Vector2(-1, 500));
		for (int n = 0; n < items.Count; n++) {
			bool isSelected = selectedIdx == n;
			bool entrySelected = false;
			entrySelected = DrawCallListItem(items[n], n, isSelected);


			if (entrySelected) {
				selectedIdx = n;
				OnItemClick(items[n], n);
			}
		}
		ImGui.EndListBox();
		ImGui.EndGroup();
	}

}