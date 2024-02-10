using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SmartModSwitch.Data;

namespace SmartModSwitch.UI;

public class MainWindow : Window, IDisposable {
	private IDalamudTextureWrap GoatImage;


	public MainWindow(IDalamudTextureWrap goatImage) : base(
		"My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse) {
		this.SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(375, 330),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		this.GoatImage = goatImage;

	}

	public void Dispose() {
		this.GoatImage.Dispose();
	}

	public override void Draw() {
		ImGui.Text($"The random config bool is {SMSW.Config.SomePropertyToBeSavedAndWithADefault}");

		if (ImGui.Button("Show Settings")) {
			SMSW.UIManager.DrawConfigUI();
		}

		ImGui.Spacing();

		ImGui.Text("Have a goat:");
		ImGui.Indent(55);
		ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
		ImGui.Unindent(55);

		var val = SMSW.Config.OverlayActive;
		if (ImGui.Checkbox("Random Config Bool", ref val)) {
			SMSW.Config.OverlayActive = val;
			// can save immediately on change, if you don't want to provide a "Save and Close" button
			SMSW.Config.Save();

			SMSW.UIManager.OverlayWindow.IsOpen = val;
		}

	}
}
