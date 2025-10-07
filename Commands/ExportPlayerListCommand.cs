using System;
using System.IO;
using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using PlayerListPlugin.Models;
using PlayerListPlugin.Services;
using PlayerListPlugin.Configuration;

namespace PlayerListPlugin.Commands;

public class ExportPlayerListCommand
{
    private readonly PlayerService _playerService;
    private readonly ServerService _serverService;
    private readonly PlayerListConfig _config;

    public ExportPlayerListCommand(PlayerService playerService, ServerService serverService, PlayerListConfig config)
    {
        _playerService = playerService;
        _serverService = serverService;
        _config = config;
    }

    public void HandleCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (!_config.EnableExport)
        {
            command.ReplyToCommand("La exportación de lista de jugadores está deshabilitada en la configuración.");
            return;
        }

        try
        {
            var players = _playerService.GetPlayerList();
            var serverInfo = _serverService.GetServerInfo();

            var response = new PlayerListResponse
            {
                ServerInfo = serverInfo,
                PlayerCount = players.Count,
                Players = players
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            var fileName = $"playerlist_export_{DateTime.Now:yyyyMMdd_HHmmss}.json";
            var filePath = Path.Combine(Server.GameDirectory, "csgo", "cfg", fileName);
            File.WriteAllText(filePath, json);

            command.ReplyToCommand($"Lista de jugadores exportada a: {filePath}");
        }
        catch (Exception ex)
        {
            command.ReplyToCommand($"Error al exportar la lista de jugadores: {ex.Message}");
        }
    }
}