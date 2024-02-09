using System;
using System.Collections.Generic;
using System.Numerics;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;
using SmartModSwitch.Interop;

namespace SmartModSwitch.UI.ImGuiExt;

public class EmoteSearchBox {
    private string searchText = "";
    private int selectedCategoryIdx = 0;
    private readonly GameData gameData;
    private readonly List<Emote> listItems;
    private readonly List<string> listCategories;

    public EmoteSearchBox(GameData gameData) {
        this.gameData = gameData;
        listItems = gameData.emotes;
        listCategories = gameData.emoteCategoryNames;
        listCategories.Insert(0, "All");
    }
    public bool Draw(ref int selectedListIdx) {
        List<Emote> filteredItems = [];
        bool ret = false;

        string displayText = listItems[selectedListIdx].Name;
        bool isInputTextEnterPressed = ImGui.InputText("##input", ref displayText, 32, ImGuiInputTextFlags.EnterReturnsTrue);
        var min = ImGui.GetItemRectMin();
        var size = ImGui.GetItemRectSize();
        bool isInputTextActivated = ImGui.IsItemActivated();

        if (isInputTextActivated) {
            ImGui.SetNextWindowPos(new Vector2(min.X, min.Y));
            ImGui.OpenPopup("##popup");
        }
        if (ImGui.BeginPopup("##popup", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings)) {
            if (isInputTextActivated) {
                searchText = "";
                ImGui.SetKeyboardFocusHere(0);
            }
            ImGui.InputTextWithHint("##search", "Search Emotes", ref searchText, 32);
            ImGui.SameLine();
            ImGui.Combo("##listCombo", ref selectedCategoryIdx, listCategories.ToArray(), listCategories.Count);
            filteredItems.Clear();
            // Select items based on the selected list index
            List<Emote> selectedItems = [];//listItems;//GetSelectedListItems(selectedListIndex);
            if(selectedCategoryIdx == 0)
                selectedItems = listItems;
            else
                foreach (Emote emote in gameData.emotes) {
                    EmoteCategory? category = emote.EmoteCategory.Value;
                    if (category != null)
                        if (category.Name == listCategories[selectedCategoryIdx])
                            selectedItems.Add(emote);
                }

            if (string.IsNullOrEmpty(searchText))
                foreach (Emote item in selectedItems)
                    filteredItems.Add(item);
            else {
                var parts = searchText.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (Emote em in selectedItems) {
                    string str = em.Name;
                    bool allPartsMatch = true;
                    foreach (string part in parts) {
                        if (!str.Contains(part, StringComparison.OrdinalIgnoreCase)) {
                            allPartsMatch = false;
                            break;
                        }
                    }
                    if (allPartsMatch)
                        filteredItems.Add(em);
                }
            }
            ImGui.BeginChild("scrolling_region", new Vector2(size.X * 2, size.Y * 10), false, ImGuiWindowFlags.HorizontalScrollbar);
            foreach (Emote item in filteredItems) {
                if (ImGui.Selectable(item.Name)) {
                    searchText = item.Name;
                    selectedListIdx = listItems.IndexOf(item);
                    ImGui.CloseCurrentPopup();
                    ret = true;
                }
            }
            ImGui.EndChild();
            if (isInputTextEnterPressed || ImGui.IsKeyPressed(ImGuiKey.Escape)) {
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }
        return ret;
    }

}