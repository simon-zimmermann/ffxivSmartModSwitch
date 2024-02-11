using Dalamud.Game.Config;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using SmartModSwitch.UI;
using SmartModSwitch.Data;
using SmartModSwitch.Interop;

namespace SmartModSwitch;

public sealed class SMSW : IDalamudPlugin {
	public readonly static string Name = "SmartModSwitch";

#pragma warning disable CS8618 // static fields are initialized in the constructor -> if they are null, something went very wrong
	// injected services
	public static DalamudPluginInterface PluginInterface { get; private set; }
	public static ICommandManager CommandManager { get; private set; }
	public static IPluginLog Logger { get; private set; }
	public static IDataManager DataManager { get; private set; }
	public static IGameInteropProvider GameInteropProvider { get; private set; }

	// static plugin modules
	public static GameData GameData { get; private set; }
	public static PenumbraIPC PenumbraIPC { get; private set; }
	public static ChatHelper ChatHelper { get; private set; }
	public static CommandHandler CommandHandler { get; private set; }
	public static UIManager UIManager { get; private set; }
	public static ModConfig Config { get; private set; }
#pragma warning restore CS8618

	// plugin function modules

	public SMSW(
		[RequiredVersion("1.0")] DalamudPluginInterface _pluginInterface,
		[RequiredVersion("1.0")] ICommandManager _commandManager,
		[RequiredVersion("1.0")] IPluginLog _pluginLog,
		[RequiredVersion("1.0")] IDataManager _dataManager,
		[RequiredVersion("1.0")] IGameInteropProvider _gameInteropProvider) {
		// init injected services
		PluginInterface = _pluginInterface;
		CommandManager = _commandManager;
		Logger = _pluginLog;
		DataManager = _dataManager;
		GameInteropProvider = _gameInteropProvider;
		//init static plugin modules. They may not access "this" and any non-static members
		GameData = new GameData();
		PenumbraIPC = new PenumbraIPC();
		ChatHelper = new ChatHelper();
		CommandHandler = new CommandHandler();

		Logger.Info("Initializing SmartModSwitch");

		// plugin function modules
		Config = PluginInterface.GetPluginConfig() as ModConfig ?? new ModConfig();
		Config.Initialize();

		//init UI last, as it may access all other modules
		UIManager = new UIManager();


		//ChatHelper.SendSanitizedChatMessage("/echo wat");

	}

	public void Dispose() {
		UIManager.Dispose();
		CommandHandler.Dispose();
		PenumbraIPC.Dispose();
		ChatHelper.Dispose();
		GameData.Dispose();
	}

}
