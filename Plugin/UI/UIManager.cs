using System;
using SmartModSwitch.Windows;
using Dalamud.Interface.Windowing;
using System.IO;

namespace SmartModSwitch;

public sealed class UIManager : IDisposable
{
	private readonly SmartModSwitch smsw;
	public WindowSystem WindowSystem = new(SmartModSwitch.Name);
	public ConfigWindow ConfigWindow { get; init; }
	public MainWindow MainWindow { get; init; }
	public OverlayWindow OverlayWindow { get; init; }
	public UIManager(SmartModSwitch smsw)
	{
		this.smsw = smsw;

		// you might normally want to embed resources and load them from the manifest stream
		var imagePath = Path.Combine(smsw.PluginInterface.AssemblyLocation.Directory?.FullName!, "Assets", "goat.png");
		var goatImage = smsw.PluginInterface.UiBuilder.LoadImage(imagePath);

		ConfigWindow = new ConfigWindow(smsw);
		MainWindow = new MainWindow(smsw, goatImage);
		OverlayWindow = new OverlayWindow(smsw);

		WindowSystem.AddWindow(ConfigWindow);
		WindowSystem.AddWindow(MainWindow);
		WindowSystem.AddWindow(OverlayWindow);

		smsw.PluginInterface.UiBuilder.Draw += DrawUI;
		smsw.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
	}

	private void DrawUI()
	{
		this.WindowSystem.Draw();
	}

	public void DrawConfigUI()
	{
		ConfigWindow.IsOpen = true;
	}
	public void Dispose()
	{
		WindowSystem.RemoveAllWindows();
		ConfigWindow.Dispose();
		MainWindow.Dispose();
		OverlayWindow.Dispose();
	}

}