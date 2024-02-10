using System;
using SmartModSwitch.UI;
using Dalamud.Interface.Windowing;
using System.IO;
using SmartModSwitch.Data;

namespace SmartModSwitch.UI;


public sealed class UIManager : IDisposable {
	public WindowSystem WindowSystem = new(SMSW.Name);
	public Config.WindowMain ConfigWindow { get; init; }
	public MainWindow MainWindow { get; init; }
	public OverlayWindow OverlayWindow { get; init; }
	public NewAssignmentWindow NewAssignmentWindow { get; init; }
	public UIManager() {
		// you might normally want to embed resources and load them from the manifest stream
		var imagePath = Path.Combine(SMSW.PluginInterface.AssemblyLocation.Directory?.FullName!, "Assets", "goat.png");
		var goatImage = SMSW.PluginInterface.UiBuilder.LoadImage(imagePath);

		ConfigWindow = new Config.WindowMain();
		MainWindow = new MainWindow(goatImage);
		OverlayWindow = new OverlayWindow();
		NewAssignmentWindow = new NewAssignmentWindow();

		WindowSystem.AddWindow(ConfigWindow);
		WindowSystem.AddWindow(MainWindow);
		WindowSystem.AddWindow(OverlayWindow);
		WindowSystem.AddWindow(NewAssignmentWindow);

		SMSW.PluginInterface.UiBuilder.Draw += DrawUI;
		SMSW.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

		//debug
		MainWindow.IsOpen = true;

	}

	private void DrawUI() {
		this.WindowSystem.Draw();
	}

	public void DrawConfigUI() {
		ConfigWindow.IsOpen = true;
	}
	public void Dispose() {
		WindowSystem.RemoveAllWindows();
		ConfigWindow.Dispose();
		MainWindow.Dispose();
		OverlayWindow.Dispose();
		NewAssignmentWindow.Dispose();
	}

}