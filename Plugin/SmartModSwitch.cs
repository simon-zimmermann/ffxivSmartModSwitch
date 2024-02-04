using Dalamud.Game.Config;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using SmartModSwitch.UI;
using SmartModSwitch.Data;
using SmartModSwitch.Interop;

namespace SmartModSwitch;

public sealed class SmartModSwitch : IDalamudPlugin
{
	public readonly static string Name = "SmartModSwitch";

	// injected services
	public DalamudPluginInterface PluginInterface { get; init; }
	public ICommandManager CommandManager { get; init; }
	public IPluginLog Logger { get; init; }
	public IDataManager DataManager { get; init; }
	public IGameInteropProvider GameInteropProvider { get; init; }

	// plugin function modules
	public Config Config { get; init; }
	public CommandHandler CommandHandler { get; init; }
	public UIManager UIManager { get; init; }
	public PenumbraIPC PenumbraIPC { get; init; }
	public ChatHelper ChatHelper { get; init; }

	public SmartModSwitch(
		[RequiredVersion("1.0")] DalamudPluginInterface _pluginInterface,
		[RequiredVersion("1.0")] ICommandManager _commandManager,
		[RequiredVersion("1.0")] IPluginLog _pluginLog,
		[RequiredVersion("1.0")] IDataManager _dataManager,
		[RequiredVersion("1.0")] IGameInteropProvider _gameInteropProvider)
	{
		// init injected services
		PluginInterface = _pluginInterface;
		CommandManager = _commandManager;
		Logger = _pluginLog;
		DataManager = _dataManager;
		GameInteropProvider = _gameInteropProvider;

		Logger.Info("Initializing SmartModSwitch");

		// plugin function modules
		Config = PluginInterface.GetPluginConfig() as Config ?? new Config();
		Config.Initialize(this);

		CommandHandler = new CommandHandler(this);
		UIManager = new UIManager(this);
		PenumbraIPC = new PenumbraIPC(this);
		ChatHelper = new ChatHelper(this);
		
		
		ChatHelper.SendSanitizedChatMessage("/echo wat");

	}

	public void Dispose()
	{
		Config.Save();
		UIManager.Dispose();
		CommandHandler.Dispose();
		PenumbraIPC.Dispose();
	}

}
