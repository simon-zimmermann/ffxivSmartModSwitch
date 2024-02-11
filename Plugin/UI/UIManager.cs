using System;
using Dalamud.Interface.Windowing;
using System.IO;

namespace SmartModSwitch.UI;


public sealed class UIManager : IDisposable {
	public WindowSystem WindowSystem = new(SMSW.Name);
	public Config.WindowMain ConfigWindow { get; init; }
	public MainWindow MainWindow { get; init; }
	public OverlayWindow OverlayWindow { get; init; }
	public UIManager() {
		// you might normally want to embed resources and load them from the manifest stream
		var imagePath = Path.Combine(SMSW.PluginInterface.AssemblyLocation.Directory?.FullName!, "Assets", "goat.png");
		var goatImage = SMSW.PluginInterface.UiBuilder.LoadImage(imagePath);

		ConfigWindow = new Config.WindowMain();
		MainWindow = new MainWindow(goatImage);
		OverlayWindow = new OverlayWindow();

		WindowSystem.AddWindow(ConfigWindow);
		WindowSystem.AddWindow(MainWindow);
		WindowSystem.AddWindow(OverlayWindow);

		SMSW.PluginInterface.UiBuilder.Draw += DrawUI;
		SMSW.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;

		//debug
		//ConfigWindow.IsOpen = true;

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
	}

}