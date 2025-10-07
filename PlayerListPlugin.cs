using System;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Core.Attributes.Registration;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using PlayerListPlugin.Commands;
using PlayerListPlugin.Configuration;
using PlayerListPlugin.Services;
using PlayerListPlugin.Helpers;

namespace PlayerListPlugin;

public class PlayerListPlugin : BasePlugin, IPluginConfig<PlayerListConfig>
{
    public override string ModuleName => "PlayerList";
    public override string ModuleVersion => "1.3.0";
    public override string ModuleAuthor => "Prahzera";
    
    public PlayerListConfig Config { get; set; } = new();
    
    private readonly PlayerConnectionTracker _connectionTracker = new();
    private DateTime _pluginStartTime;
    
    // Services
    private PlayerService? _playerService;
    private ServerService? _serverService;
    
    // Helpers
    private Formatter? _formatter;
    
    // Commands
    private PlayerListCommand? _playerListCommand;
    private ServerInfoCommand? _serverInfoCommand;
    private PlayerInfoCommand? _playerInfoCommand;

    public void OnConfigParsed(PlayerListConfig config)
    {
        Config = config;
    }

    public override void Load(bool hotReload)
    {
        // Initialize services
        _pluginStartTime = DateTime.Now;
        _serverService = new ServerService(_pluginStartTime);
        _playerService = new PlayerService(_connectionTracker, Config);
        
        // Initialize helpers
        _formatter = new Formatter(Config);
        
        // Initialize commands
        _playerListCommand = new PlayerListCommand(_playerService, _serverService, _formatter, Config);
        _serverInfoCommand = new ServerInfoCommand(_serverService, _formatter);
        _playerInfoCommand = new PlayerInfoCommand(_playerService, _formatter, Config);

        RegisterListener<Listeners.OnClientConnected>(OnClientConnected);
        RegisterListener<Listeners.OnClientDisconnect>(OnClientDisconnect);
    }

    private void OnClientConnected(int playerSlot)
    {
        if (Config.TrackConnectionTime)
        {
            var player = Utilities.GetPlayerFromSlot(playerSlot);
            if (player != null && player.IsValid)
            {
                _connectionTracker.OnPlayerConnect(player);
            }
        }
    }

    private void OnClientDisconnect(int playerSlot)
    {
        var player = Utilities.GetPlayerFromSlot(playerSlot);
        if (player != null && player.IsValid)
        {
            _connectionTracker.OnPlayerDisconnect(player);
        }
    }

    [ConsoleCommand("playerlist", "Muestra una lista de todos los jugadores conectados. Uso: playerlist [json] [filtros]")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnPlayerListCommand(CCSPlayerController? player, CommandInfo command)
    {
        _playerListCommand?.HandleCommand(player, command);
    }

    [ConsoleCommand("serverinfo", "Muestra información del servidor. Uso: serverinfo [json]")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnServerInfoCommand(CCSPlayerController? player, CommandInfo command)
    {
        _serverInfoCommand?.HandleCommand(player, command);
    }

    [ConsoleCommand("playerinfo", "Muestra información detallada de un jugador específico. Uso: playerinfo <steamid>")]
    [CommandHelper(whoCanExecute: CommandUsage.SERVER_ONLY)]
    public void OnPlayerInfoCommand(CCSPlayerController? player, CommandInfo command)
    {
        _playerInfoCommand?.HandleCommand(player, command);
    }

    public override void Unload(bool hotReload)
    {
        _connectionTracker.Clear();
    }
}