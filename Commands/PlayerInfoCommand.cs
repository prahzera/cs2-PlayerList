using System;
using System.Linq;
using System.Text.Json;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using CounterStrikeSharp.API.Modules.Entities;
using PlayerListPlugin.Models;
using PlayerListPlugin.Services;
using PlayerListPlugin.Helpers;
using PlayerListPlugin.Configuration;

namespace PlayerListPlugin.Commands;

public class PlayerInfoCommand
{
    private readonly PlayerService _playerService;
    private readonly Formatter _formatter;
    private readonly PlayerListConfig _config;

    public PlayerInfoCommand(PlayerService playerService, Formatter formatter, PlayerListConfig config)
    {
        _playerService = playerService;
        _formatter = formatter;
        _config = config;
    }

    public void HandleCommand(CCSPlayerController? player, CommandInfo command)
    {
        if (!_config.EnablePlayerInfoCommand)
        {
            command.ReplyToCommand("El comando playerinfo está deshabilitado en la configuración.");
            return;
        }

        if (command.ArgCount < 2)
        {
            command.ReplyToCommand("Uso: playerinfo <steamid>");
            return;
        }

        var steamIdArg = command.ArgByIndex(1);
        if (!ulong.TryParse(steamIdArg, out var steamId))
        {
            command.ReplyToCommand("SteamID inválido. Debe ser un número.");
            return;
        }

        var targetPlayer = Utilities.GetPlayers()
            .FirstOrDefault(p => p.IsValid && p.SteamID == steamId);

        if (targetPlayer == null)
        {
            command.ReplyToCommand($"No se encontró ningún jugador con SteamID {steamId}.");
            return;
        }

        var playerInfo = _playerService.GetPlayerInfo(targetPlayer);

        if (player == null)
        {
            // Ejecutado desde la consola del servidor - mostrar información detallada en formato legible
            _formatter.DisplayPlayerInfoTable(command, playerInfo);
        }
        else
        {
            // Ejecutado desde RCON - devolver JSON
            var json = JsonSerializer.Serialize(playerInfo, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            command.ReplyToCommand(json);
        }
    }
}