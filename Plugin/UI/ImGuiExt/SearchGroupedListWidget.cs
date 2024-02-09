using System;
using System.Collections.Generic;
using System.Numerics;
using Dalamud.Game.Text.SeStringHandling;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using SmartModSwitch.Interop;

namespace SmartModSwitch.UI.ImGuiExt;

/// <summary>
/// A widget that displays a list of items and allows the user to search and filter the list.
/// T1: The type of the items in the main list.
/// T2: The type of the categories, which the user can select in a dropdown.
/// </summary>
public class SearchGroupedListWidget<T1, T2> where T1 : class where T2 : class {
    /// <summary>
    /// The text visible in the search input field.
    /// Will be reset on re-open
    /// </summary>
    private string searchText = "";
    private int selectedCategoryIdx = 0;
    private readonly List<T1> listItems;
    List<T1> filteredItems = [];
    List<T1> selectedItems = [];
    private readonly List<T2> listCategories;
    public string[] listCategoryStrings = [];
    private readonly string label;
    public Func<T1, string> ItemToString { private get; set; } = (val) => "CALLBACK NOT SET: ItemToString";
    private Func<T2, string> _categoryToString = (val) => "CALLBACK NOT SET: CategoryToString";
    public Func<T2, string> CategoryToString {
        private get { return _categoryToString; }
        set {
            _categoryToString = value;
            //when converter function is set, update the list of category strings
            //also prepend an "All" category
            listCategoryStrings = new string[listCategories.Count + 1];
            listCategoryStrings[0] = "All";
            for (int i = 0; i < listCategories.Count; i++)
                listCategoryStrings[i + 1] = value(listCategories[i]);
        }
    }
    public Func<T1, T2, bool> IsItemInCategory { private get; set; } = (item, category) => false;

    public SearchGroupedListWidget(string label, List<T1> listItems, List<T2> listCategories) {
        this.listItems = listItems;
        this.listCategories = listCategories;
        this.label = label;
    }
    /// <summary>
    /// Select list items based on the selected category,
    /// and then filter this selection based on the search text.
    /// </summary>
    private void SelectFilterItems() {
        //select
        selectedItems = [];
        if (selectedCategoryIdx == 0)
            selectedItems = listItems;
        else
            foreach (T1 emote in listItems) {
                //-1 to account for the "All" category
                if (IsItemInCategory(emote, listCategories[selectedCategoryIdx - 1]))
                    selectedItems.Add(emote);
            }
        //filter
        filteredItems = [];
        if (string.IsNullOrEmpty(searchText))
            filteredItems = selectedItems;
        else {
            var parts = searchText.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (T1 item in selectedItems) {
                string str = ItemToString(item);
                bool allPartsMatch = true;
                foreach (string part in parts) {
                    if (!str.Contains(part, StringComparison.OrdinalIgnoreCase)) {
                        allPartsMatch = false;
                        break;
                    }
                }
                if (allPartsMatch)
                    filteredItems.Add(item);
            }
        }
    }
    public bool Draw(ref int selectedListIdx) {
        bool ret = false;

        //draw display input to show the selected item
        string displayText = "";
        if (selectedListIdx >= 0 && selectedListIdx < listItems.Count)
            displayText = ItemToString(listItems[selectedListIdx]);
        ImGui.InputText(label + "##input", ref displayText, 32, ImGuiInputTextFlags.EnterReturnsTrue);
        bool triggerPopupOpen = ImGui.IsItemActivated();

        //open popup on click in input field
        if (triggerPopupOpen) {
            ImGui.SetNextWindowPos(ImGui.GetItemRectMin());
            ImGui.OpenPopup("##popup");
        }

        if (ImGui.BeginPopup("##popup", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings)) {
            //reset & set focus when popup is drawn the first time after opening
            if (triggerPopupOpen) {
                searchText = "";
                SelectFilterItems();
                ImGui.SetKeyboardFocusHere(0);
            }
            //draw header. Only update filtered items on actual filter change
            if (ImGui.InputTextWithHint("##search", "Filter", ref searchText, 32)) {
                SelectFilterItems();
            }
            ImGui.SameLine();
            if (ImGui.Combo("##listCombo", ref selectedCategoryIdx, listCategoryStrings, listCategoryStrings.Length)) {
                SelectFilterItems();
            }

            //draw list
            ImGui.BeginChild("scrolling_region", new Vector2(-1, ImGui.GetTextLineHeightWithSpacing() * 10), false, ImGuiWindowFlags.HorizontalScrollbar);
            foreach (T1 item in filteredItems) {
                string itemStr = ItemToString(item);
                if (ImGui.Selectable(itemStr)) {
                    searchText = itemStr;
                    selectedListIdx = listItems.IndexOf(item);
                    ImGui.CloseCurrentPopup();
                    ret = true;
                }
            }
            ImGui.EndChild();

            ImGui.EndPopup();
        }
        return ret;
    }

}