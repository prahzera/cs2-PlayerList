using System;
using System.Text.Json;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Commands;
using PlayerListPlugin.Models;
using PlayerListPlugin.Services;
using PlayerListPlugin.Helpers;

namespace PlayerListPlugin.Commands;

public class ServerInfoCommand
{
    private readonly ServerService _serverService;
    private readonly Formatter _formatter;

    public ServerInfoCommand(ServerService serverService, Formatter formatter)
    {
        _serverService = serverService;
        _formatter = formatter;
    }

    public void HandleCommand(CCSPlayerController? player, CommandInfo command)
    {
        try
        {
            var serverInfo = _serverService.GetServerInfo();
            
            // Verificar si se solicita formato JSON
            bool useJsonFormat = command.ArgCount > 1 && command.ArgByIndex(1).ToLower() == "json";

            if (useJsonFormat)
            {
                var json = JsonSerializer.Serialize(serverInfo, new JsonSerializerOptions 
                { 
                    WriteIndented = true 
                });

                command.ReplyToCommand(json);
            }
            else
            {
                _formatter.DisplayServerInfoTable(command, serverInfo);
            }
        }
        catch (Exception ex)
        {
            command.ReplyToCommand($"Error al obtener la información del servidor: {ex.Message}");
        }
    }
}