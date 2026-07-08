using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using PlayerListPlugin.Models;
using PlayerListPlugin.Services;
using PlayerListPlugin.Helpers;
using PlayerListPlugin.Configuration;

namespace PlayerListPlugin.Commands;

public class PlayerListCommand
{
    private readonly PlayerService _playerService;
    private readonly ServerService _serverService;
    private readonly Formatter _formatter;
    private readonly PlayerListConfig _config;

    public PlayerListCommand(PlayerService playerService, ServerService serverService, Formatter formatter, PlayerListConfig config)
    {
        _playerService = playerService;
        _serverService = serverService;
        _formatter = formatter;
        _config = config;
    }

    public void HandleCommand(CCSPlayerController? player, CommandInfo command)
    {
        try
        {
            bool useJsonFormat = false;
            bool useCompactFormat = false;
            int startIndex = 1;

            if (command.ArgCount > 1)
            {
                var arg = command.ArgByIndex(1).ToLower();
                if (arg == "json")
                {
                    useJsonFormat = true;
                    startIndex = 2;
                }
                else if (arg == "compact")
                {
                    useCompactFormat = true;
                    startIndex = 2;
                }
            }

            // Estadísticas solo en JSON o compact
            var players = _playerService.GetPlayerList(useJsonFormat || useCompactFormat);

            // Aplicar filtros si se proporcionan
            if (_config.EnableFilters && command.ArgCount > startIndex)
            {
                var args = new List<string>();
                for (int i = 0; i < command.ArgCount; i++)
                {
                    args.Add(command.ArgByIndex(i));
                }
                FilterUtility.ApplyFilters(players, args.ToArray(), startIndex);
            }

            if (useJsonFormat)
            {
                var response = new PlayerListResponse
                {
                    ServerInfo = _serverService.GetServerInfo(),
                    PlayerCount = players.Count,
                    Players = players
                };

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
                {
                    WriteIndented = false
                });

                command.ReplyToCommand(json);
            }
            else if (useCompactFormat)
            {
                var serverInfo = _serverService.GetServerInfo();
                var sb = new StringBuilder();

                // Línea 1: ServerInfo
                sb.Append(serverInfo.HostName);
                sb.Append('|');
                sb.Append(serverInfo.MapName);
                sb.Append('|');
                sb.Append(serverInfo.MaxPlayers);
                sb.Append('|');
                sb.Append(serverInfo.CurrentPlayers);
                sb.Append('|');
                sb.Append(serverInfo.Uptime.ToString(@"hh\:mm\:ss\.fffffff"));
                sb.Append('\n');

                // Línea 2: PlayerCount
                sb.Append(players.Count);
                sb.Append('\n');

                // Líneas 3+: Players
                foreach (var p in players)
                {
                    sb.Append(p.Name.Replace("|", "┃"));
                    sb.Append('|');
                    sb.Append(p.SteamID);
                    sb.Append('|');
                    sb.Append(TeamToShort(p.Team));
                    sb.Append('|');
                    sb.Append(p.ConnectedTime);
                    sb.Append('|');
                    sb.Append(p.Kills);
                    sb.Append('|');
                    sb.Append(p.Deaths);
                    sb.Append('|');
                    sb.Append(p.Assists);
                    sb.Append('\n');
                }

                command.ReplyToCommand(sb.ToString().TrimEnd('\n'));
            }
            else
            {
                // Mostrar como tabla (sin estadísticas)
                var serverInfo = _serverService.GetServerInfo();
                _formatter.DisplayPlayerTable(command, players, serverInfo);
            }
        }
        catch (Exception ex)
        {
            command.ReplyToCommand($"Error al obtener la lista de jugadores: {ex.Message}");
        }
    }

    private static string TeamToShort(string team)
    {
        return team switch
        {
            "Counter-Terrorists" => "CT",
            "Terrorists" => "T",
            "Spectator" => "SPEC",
            "None" => "NONE",
            _ => team
        };
    }
}
