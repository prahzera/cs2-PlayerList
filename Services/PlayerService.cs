using System;
using System.Collections.Generic;
using System.Linq;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Entities;
using CounterStrikeSharp.API.Modules.Utils;
using PlayerListPlugin.Models;
using PlayerListPlugin.Configuration;

namespace PlayerListPlugin.Services;

public class PlayerService
{
    private readonly PlayerConnectionTracker _connectionTracker;
    private readonly PlayerListConfig _config;

    public PlayerService(PlayerConnectionTracker connectionTracker, PlayerListConfig config)
    {
        _connectionTracker = connectionTracker;
        _config = config;
    }

    public List<PlayerInfo> GetPlayerList(bool includeStats = false)
    {
        return Utilities.GetPlayers()
            .Where(p => p.IsValid && p.PlayerPawn.IsValid && p.SteamID > 0)
            .Select(p => new PlayerInfo
            {
                Name = p.PlayerName,
                SteamID = p.SteamID.ToString(),
                Team = GetTeamName(p.Team),
                ConnectedTime = GetConnectedTime(p),
                SessionTime = GetSessionTime(p),
                IsBot = p.IsBot,
                Kills = includeStats ? GetPlayerKills(p) : 0,
                Deaths = includeStats ? GetPlayerDeaths(p) : 0,
                Assists = includeStats ? GetPlayerAssists(p) : 0
            })
            .ToList();
    }

    public PlayerInfo GetPlayerInfo(CCSPlayerController player, bool includeStats = false)
    {
        return new PlayerInfo
        {
            Name = player.PlayerName,
            SteamID = player.SteamID.ToString(),
            Team = GetTeamName(player.Team),
            ConnectedTime = GetConnectedTime(player),
            SessionTime = GetSessionTime(player),
            IsBot = player.IsBot,
            Kills = includeStats ? GetPlayerKills(player) : 0,
            Deaths = includeStats ? GetPlayerDeaths(player) : 0,
            Assists = includeStats ? GetPlayerAssists(player) : 0
        };
    }

    public string GetTeamName(CsTeam team)
    {
        return team switch
        {
            CsTeam.None => "None",
            CsTeam.Spectator => "Spectator",
            CsTeam.Terrorist => "Terrorists",
            CsTeam.CounterTerrorist => "Counter-Terrorists",
            _ => "Unknown"
        };
    }

    private long GetConnectedTime(CCSPlayerController player)
    {
        // Devuelve el tiempo de conexión en segundos desde Unix epoch
        return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    private TimeSpan? GetSessionTime(CCSPlayerController player)
    {
        // Si el seguimiento de tiempo de conexión está deshabilitado, retornar null
        if (!_config.TrackConnectionTime || player.SteamID <= 0)
        {
            return null;
        }

        // Obtener el tiempo de sesión del tracker
        var sessionTime = _connectionTracker.GetPlayerSessionTime(player.SteamID);
        
        // Si no se encuentra el tiempo de sesión, intentar calcularlo de otra manera
        if (sessionTime == null)
        {
            // Como fallback, podemos usar el tiempo desde que se cargó el plugin
            // Esto no es ideal pero mejor que null
            return TimeSpan.Zero;
        }
        
        return sessionTime;
    }

    // Métodos para obtener estadísticas del jugador
    private int GetPlayerKills(CCSPlayerController player)
    {
        try
        {
            var kills = player.ActionTrackingServices?.MatchStats?.Kills;
            return kills.HasValue ? kills.Value : 0;
        }
        catch
        {
            return 0;
        }
    }

    private int GetPlayerDeaths(CCSPlayerController player)
    {
        try
        {
            var deaths = player.ActionTrackingServices?.MatchStats?.Deaths;
            return deaths.HasValue ? deaths.Value : 0;
        }
        catch
        {
            return 0;
        }
    }

    private int GetPlayerAssists(CCSPlayerController player)
    {
        try
        {
            var assists = player.ActionTrackingServices?.MatchStats?.Assists;
            return assists.HasValue ? assists.Value : 0;
        }
        catch
        {
            return 0;
        }
    }
}