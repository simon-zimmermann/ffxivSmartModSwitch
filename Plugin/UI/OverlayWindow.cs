using System;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SmartModSwitch.UI;

public class OverlayWindow : Window, IDisposable {

    public OverlayWindow() : base(
        "OverlayWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoTitleBar) {
        this.SizeConstraints = new WindowSizeConstraints {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        this.BgAlpha = 0.5f;
        this.Flags |= ImGuiWindowFlags.NoMove;
    }

    public void Dispose() {
    }

    public override void Draw() {
        //ImGui.Text($"The random config bool is {SMSW.Config.SomePropertyToBeSavedAndWithADefault}");

        if (ImGui.Button("Show Settings")) {
            SMSW.UIManager.DrawConfigUI();
        }

        ImGui.Spacing();

        ImGui.Text("Have a goat:");
    }
}
