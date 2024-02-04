using System.Collections.Generic;
using ImGuiNET;
using System.Numerics;
using System;
using Dalamud.Interface;

namespace SmartModSwitch.UI.ImGuiExt;

public class SelectListComponent<T> : IUiWidget where T : class
{
    private List<T> items;
    private int selectedIdx = 0;
    private Func<T> onClick;
    private Func<T>? drawOverride;
    public SelectListComponent(List<T> items, Func<T> onClick, Func<T>? drawOverride = null)
    {
        this.items = items;
        this.onClick = onClick;
        this.drawOverride = drawOverride;
    }

    public void Draw()
    {
        ImGui.BeginGroup();
        ImGui.Text("Assignment Groups");
        var buttonwidth = ImGuiUtil.AddButtonSize() + ImGuiUtil.DeleteButtonSize();
        ImGui.SameLine(ImGui.GetContentRegionAvail().X - buttonwidth);
        ImGui.CalcItemWidth();
        
        if (ImGuiUtil.AddButton())
        {
        }
        ImGui.SameLine();
        if (ImGuiUtil.DeleteButton())
        {
        }
        ImGui.BeginListBox("", new Vector2(-1, 500));
        for (int n = 0; n < items.Count; n++)
        {
            bool isSelected = selectedIdx == n;
            if (ImGui.Selectable($"{n}: {items[n]}", isSelected))
            {
                selectedIdx = n;
                //smsw.Logger.Info($"Selected {n}: {items[n]}");
            }
        }
        ImGui.EndListBox();
        ImGui.EndGroup();

    }

    public void RemoveSelected()
    {
        items.RemoveAt(selectedIdx);
    }
}