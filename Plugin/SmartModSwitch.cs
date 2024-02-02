using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace SmartModSwitch;

public sealed class SmartModSwitch : IDalamudPlugin
{
	public readonly static string Name = "SmartModSwitch";

	// injected services
	public DalamudPluginInterface PluginInterface { get; init; }
	public ICommandManager CommandManager { get; init; }
	public IPluginLog Logger { get; init; }

	// plugin function modules
	public Configuration Configuration { get; init; }
	public CommandHandler CommandHandler { get; init; }
	public UIManager UIManager { get; init; }

	public SmartModSwitch(
		[RequiredVersion("1.0")] DalamudPluginInterface _pluginInterface,
		[RequiredVersion("1.0")] ICommandManager _commandManager,
		[RequiredVersion("1.0")] IPluginLog _pluginLog)
	{
		// init injected services
		PluginInterface = _pluginInterface;
		CommandManager = _commandManager;
		Logger = _pluginLog;

		Logger.Info("Initializing SmartModSwitch");

		// plugin function modules
		Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
		Configuration.Initialize(PluginInterface);

		CommandHandler = new CommandHandler(this);

		UIManager = new UIManager(this);
	}

	public void Dispose()
	{
		Configuration.Save();
		UIManager.Dispose();
		CommandHandler.Dispose();
	}

}
