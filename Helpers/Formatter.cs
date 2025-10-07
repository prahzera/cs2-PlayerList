using System;
using System.Collections.Generic;
using System.Linq;
using CounterStrikeSharp.API.Modules.Commands;
using PlayerListPlugin.Models;
using PlayerListPlugin.Configuration;

namespace PlayerListPlugin.Helpers;

public class Formatter
{
    private readonly PlayerListConfig _config;

    public Formatter(PlayerListConfig config)
    {
        _config = config;
    }

    public string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    public string FormatSessionTime(TimeSpan? sessionTime)
    {
        if (sessionTime == null) return "Desconocido";
        
        if (sessionTime.Value.TotalHours >= 1)
            return $"{sessionTime.Value.Hours}h {sessionTime.Value.Minutes}m";
        else if (sessionTime.Value.TotalMinutes >= 1)
            return $"{sessionTime.Value.Minutes}m {sessionTime.Value.Seconds}s";
        else
            return $"{sessionTime.Value.Seconds}s";
    }

    public string FormatUptime(TimeSpan uptime)
    {
        if (uptime.TotalHours >= 1)
            return $"{(int)uptime.TotalHours}h {uptime.Minutes}m {uptime.Seconds}s";
        else if (uptime.TotalMinutes >= 1)
            return $"{uptime.Minutes}m {uptime.Seconds}s";
        else
            return $"{uptime.Seconds}s";
    }

    public void DisplayServerInfoTable(CommandInfo command, ServerInfo serverInfo)
    {
        command.ReplyToCommand("========================================================================");
        command.ReplyToCommand("INFORMACIÓN DEL SERVIDOR");
        command.ReplyToCommand("========================================================================");
        command.ReplyToCommand($"Nombre del servidor:     {serverInfo.HostName}");
        command.ReplyToCommand($"Mapa actual:             {serverInfo.MapName}");
        command.ReplyToCommand($"Jugadores:               {serverInfo.CurrentPlayers}/{serverInfo.MaxPlayers}");
        command.ReplyToCommand($"Tiempo activo plugin:    {FormatUptime(serverInfo.Uptime)}");
        command.ReplyToCommand("========================================================================");
    }

    public void DisplayPlayerTable(CommandInfo command, List<PlayerInfo> players, ServerInfo serverInfo)
    {
        // Encabezado de la tabla
        command.ReplyToCommand("========================================================================");
        command.ReplyToCommand($"SERVIDOR: {serverInfo.HostName} | MAPA: {serverInfo.MapName}");
        command.ReplyToCommand($"JUGADORES: {serverInfo.CurrentPlayers}/{serverInfo.MaxPlayers}");
        command.ReplyToCommand("========================================================================");
        
        var header = $"{"Nombre",-20} {"SteamID",-17}";
        if (_config.TableFormat.ShowTeamColumn)
            header += $" {"Equipo",-15}";
        if (_config.TableFormat.ShowBotColumn)
            header += $" {"Bot",-5}";
        if (_config.TableFormat.ShowConnectedTimeColumn)
            header += $" {"Tiempo Sesión",-15}";
        
        command.ReplyToCommand(header);
        command.ReplyToCommand("------------------------------------------------------------------------");
        
        // Filas de la tabla
        foreach (var p in players)
        {
            var row = $"{Truncate(p.Name, 20),-20} {p.SteamID,-17}";
            if (_config.TableFormat.ShowTeamColumn)
                row += $" {Truncate(p.Team, 15),-15}";
            if (_config.TableFormat.ShowBotColumn)
                row += $" {(p.IsBot ? "Si" : "No"),-5}";
            if (_config.TableFormat.ShowConnectedTimeColumn)
                row += $" {FormatSessionTime(p.SessionTime),-15}";
            
            command.ReplyToCommand(row);
        }
        
        command.ReplyToCommand("------------------------------------------------------------------------");
        command.ReplyToCommand($"Total de jugadores: {players.Count} | Tiempo activo: {FormatUptime(serverInfo.Uptime)}");
        command.ReplyToCommand("========================================================================");
    }

    public void DisplayPlayerInfoTable(CommandInfo command, PlayerInfo playerInfo)
    {
        command.ReplyToCommand("========================================================================");
        command.ReplyToCommand($"INFORMACIÓN DETALLADA DEL JUGADOR: {playerInfo.Name}");
        command.ReplyToCommand("========================================================================");
        command.ReplyToCommand($"Nombre:          {playerInfo.Name}");
        command.ReplyToCommand($"SteamID:         {playerInfo.SteamID}");
        command.ReplyToCommand($"Equipo:          {playerInfo.Team}");
        command.ReplyToCommand($"Es bot:          {(playerInfo.IsBot ? "Si" : "No")}");
        command.ReplyToCommand($"Tiempo conectado: {FormatSessionTime(playerInfo.SessionTime)}");
        command.ReplyToCommand("========================================================================");
    }
}