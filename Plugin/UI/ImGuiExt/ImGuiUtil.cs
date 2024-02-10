using Dalamud.Interface;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace SmartModSwitch.UI.ImGuiExt;
public class ImGuiUtil {
    public static readonly ImGuiWindowFlags defaultPopupFlags = ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings;
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
        //bool ret = false;
        bool isEnabled = ImGui.IsKeyDown(ImGuiKey.ModCtrl);

        ImGui.BeginDisabled(!isEnabled);
        if (isEnabled) {
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 0, 0, 1));
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.6f, 0, 0, 1));
        }
        ImGui.PushFont(UiBuilder.IconFont);
        bool ret = IconButton(FontAwesomeIcon.Trash);
        ImGui.PopFont();

        if (isEnabled)
           ImGui.PopStyleColor(2);
        ImGui.EndDisabled();

        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
            ImGui.SetTooltip("Hold Ctrl while pressing to delete item");

        return ret;
    }
    public static Vector2 DeleteButtonSize() {
        return IconButtonSize(FontAwesomeIcon.Trash);
    }
    public static bool Combo(string label, ref int current, string[] items) {
        return ImGui.Combo(label, ref current, items, items.Length);
    }

}