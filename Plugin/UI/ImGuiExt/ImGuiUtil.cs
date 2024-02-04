using Dalamud.Interface;
using ImGuiNET;
using System.Numerics;

namespace SmartModSwitch.UI.ImGuiExt;
public class ImGuiUtil
{
    public static float CalcButtonWidth(string text)
    {
        return ImGui.CalcTextSize(text).X + ImGui.GetStyle().ItemInnerSpacing.X * 2 + ImGui.GetStyle().FramePadding.X * 2;
    }
    public static bool AddButton()
    {
        var label = FontAwesomeIcon.Plus.ToIconString();
        ImGui.PushFont(UiBuilder.IconFont);
        bool ret = ImGui.Button(label);
        ImGui.PopFont();
        return ret;
    }
    public static float AddButtonSize()
    {
        var label = FontAwesomeIcon.Plus.ToIconString();
        ImGui.PushFont(UiBuilder.IconFont);
        float size =  CalcButtonWidth(label);
        ImGui.PopFont();
        return size;
    }
    public static bool DeleteButton()
    {
        bool ret = false;
        bool isEnabled = ImGui.IsKeyDown(ImGuiKey.ModCtrl);

        ImGui.BeginDisabled(!isEnabled);
        if (isEnabled)
        {
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(1, 0, 0, 1));
            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.6f, 0, 0, 1));
        }
        ImGui.PushFont(UiBuilder.IconFont);
        if (ImGui.Button(FontAwesomeIcon.Trash.ToIconString()))
            ret = true;
        ImGui.PopFont();

        if (isEnabled)
            ImGui.PopStyleColor(2);
        ImGui.EndDisabled();

        if (ImGui.IsItemHovered(ImGuiHoveredFlags.AllowWhenDisabled))
            ImGui.SetTooltip("Hold Ctrl while pressing to delete item");

        return ret;
    }
    public static float DeleteButtonSize()
    {
        return CalcButtonWidth("del");
    }
}