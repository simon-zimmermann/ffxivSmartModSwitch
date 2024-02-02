using System;
using System.Numerics;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace SmartModSwitch.Windows;

public class MainWindow : Window, IDisposable {
	private IDalamudTextureWrap GoatImage;
	private Plugin Plugin;
	private Configuration Configuration;


	public MainWindow(Plugin plugin, IDalamudTextureWrap goatImage) : base(
		"My Amazing Window", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse) {
		this.SizeConstraints = new WindowSizeConstraints {
			MinimumSize = new Vector2(375, 330),
			MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
		};

		this.GoatImage = goatImage;
		this.Plugin = plugin;
		this.Configuration = plugin.Configuration;

	}

	public void Dispose() {
		this.GoatImage.Dispose();
	}

	public override void Draw() {
		ImGui.Text($"The random config bool is {this.Plugin.Configuration.SomePropertyToBeSavedAndWithADefault}");

		if (ImGui.Button("Show Settings")) {
			this.Plugin.DrawConfigUI();
		}

		ImGui.Spacing();

		ImGui.Text("Have a goat:");
		ImGui.Indent(55);
		ImGui.Image(this.GoatImage.ImGuiHandle, new Vector2(this.GoatImage.Width, this.GoatImage.Height));
		ImGui.Unindent(55);

		var val = this.Configuration.OverlayActive;
		if (ImGui.Checkbox("Random Config Bool", ref val)) {
			this.Configuration.OverlayActive = val;
			// can save immediately on change, if you don't want to provide a "Save and Close" button
			this.Configuration.Save();

			Plugin.OverlayWindow.IsOpen = val;
		}
	}
}
