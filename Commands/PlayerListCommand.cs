using System;
using System.Collections.Generic;
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
            // Verificar si se solicita formato JSON
            bool useJsonFormat = false;
            int startIndex = 1;
            
            if (command.ArgCount > 1 && command.ArgByIndex(1).ToLower() == "json")
            {
                useJsonFormat = true;
                startIndex = 2;
            }

            // Obtener lista de jugadores con o sin estadísticas según el formato
            var players = _playerService.GetPlayerList(useJsonFormat);

            // Aplicar filtros si se proporcionan (después del parámetro json si existe)
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
                // Devolver como JSON
                var response = new PlayerListResponse
                {
                    ServerInfo = _serverService.GetServerInfo(),
                    PlayerCount = players.Count,
                    Players = players
                };

                var json = JsonSerializer.Serialize(response, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                command.ReplyToCommand(json);
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
}