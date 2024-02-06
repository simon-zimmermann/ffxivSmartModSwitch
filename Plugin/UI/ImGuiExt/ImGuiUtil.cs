using Dalamud.Interface;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SmartModSwitch.UI.ImGuiExt;
public class ImGuiUtil {
    private static int uniqueIdCounter = 1000;
    public static string GetUniqueIdSuffix() {
        uniqueIdCounter++;
        return "##" + uniqueIdCounter;
    }
    public static Vector2 CalcButtonSize(string text) {
        return ImGui.CalcTextSize(text) + ImGui.GetStyle().ItemInnerSpacing * 2 + ImGui.GetStyle().FramePadding * 2;
    }
    public static bool IconButton(FontAwesomeIcon icon, string label = "") {
        var icoLabel = icon.ToIconString();
        if (label != "") {
            ImGui.AlignTextToFramePadding();
            ImGui.Text(label);
            ImGui.SameLine();

        }
        ImGui.PushFont(UiBuilder.IconFont);
        bool ret = ImGui.Button(icoLabel);
        ImGui.PopFont();
        return ret;
    }
    public static Vector2 IconButtonSize(FontAwesomeIcon icon, string label = "") {
        var icoLabel = icon.ToIconString();
        ImGui.PushFont(UiBuilder.IconFont);
        Vector2 icoSize = CalcButtonSize(icoLabel);
        ImGui.PopFont();
        Vector2 ret = icoSize;
        if (label != "") {
            ret.X += ImGui.CalcTextSize(label).X + ImGui.GetStyle().FramePadding.X * 2;
        }
        return ret;
    }
    public static bool IconButtonDelete() {
        bool ret = false;
        bool isEnabled = ImGui.IsKeyDown(ImGuiKey.ModCtrl);
        string suffix = GetUniqueIdSuffix();

        ImGui.BeginDisabled(!isEnabled);
        if (isEnabled) {
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 0, 0, 1));
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.6f, 0, 0, 1));
        }
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button(FontAwesomeIcon.Trash.ToIconString() + suffix))
            ret = true;
        ImGui.PopFont();

        if (isEnabled)
            ImGui.PopStyleColor(2);
        ImGui.EndDisabled();

        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
            ImGui.SetTooltip("Hold Ctrl while pressing to delete item");

        return ret;
    }
    public static Vector2 DeleteButtonSize() {
        return CalcButtonSize("del");
    }
    public static bool Combo(string label, ref int current, string[] items) {
        return ImGui.Combo(label, ref current, items, items.Length);
    }
    public static void DrawDropdownBox(ref string input, ref int selectedListIndex) {
        List<string> filteredItems = new List<string>();
        string[] listNames = {
            "TYPES OF EMOTES!!!",
            "Explicit",
            "Implicit",
            "Fractured",
            "Enchant",
            "Scourge",
            "Crafted",
            "Crucible",
            "Veiled",
            "Monster",
            "Delve",
            "Ultimatum",
        };
        string[] leftItems = {
            "ADD SOME TEST ITEMS1",
            "ADD SOME TEST ITEMS2",
            "ADD SOME TEST ITEMS3",
            "ADD SOME TEST ITEMS4",
            "ADD SOME TEST ITEMS5",
            "ADD SOME TEST ITEMS6",
        };


        bool isInputTextEnterPressed = ImGui.InputText("##input", ref input, 32, ImGuiInputTextFlags.EnterReturnsTrue);
        var min = ImGui.GetItemRectMin();
        var size = ImGui.GetItemRectSize();
        bool isInputTextActivated = ImGui.IsItemActivated();

        if (isInputTextActivated) {
            ImGui.SetNextWindowPos(new Vector2(min.X, min.Y));
            ImGui.OpenPopup("##popup");
        }

        if (ImGui.BeginPopup("##popup", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings)) {
            if (isInputTextActivated)
                ImGui.SetKeyboardFocusHere(0);
            ImGui.InputText("##input_popup", ref input, 32);
            ImGui.SameLine();
            ImGui.Combo("##listCombo", ref selectedListIndex, listNames, listNames.Length);
            filteredItems.Clear();
            // Select items based on the selected list index
            string[] selectedItems = leftItems;//GetSelectedListItems(selectedListIndex);

            if (string.IsNullOrEmpty(input))
                foreach (string item in selectedItems)
                    filteredItems.Add(item);
            else {
                var parts = input.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (string str in selectedItems) {
                    bool allPartsMatch = true;
                    foreach (string part in parts) {
                        if (!str.Contains(part, StringComparison.OrdinalIgnoreCase)) {
                            allPartsMatch = false;
                            break;
                        }
                    }
                    if (allPartsMatch)
                        filteredItems.Add(str);
                }
            }
            ImGui.BeginChild("scrolling_region", new Vector2(size.X * 2, size.Y * 10), false, ImGuiWindowFlags.HorizontalScrollbar);
            foreach (string item in filteredItems) {
                if (ImGui.Selectable(item)) {
                    //App.Log("Selected popup");
                    input = item;
                    ImGui.CloseCurrentPopup();
                    break;
                }
            }
            ImGui.EndChild();

            //if (isInputTextEnterPressed || ImGui.IsKeyPressed(ImGui.GetKeyIndex(ImGuiKey.Escape))) {
            //    // App.Log("Closing popup");
            //    ImGui.CloseCurrentPopup();
            //}

            ImGui.EndPopup();
        }
    }
}