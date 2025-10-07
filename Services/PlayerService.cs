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

    public List<PlayerInfo> GetPlayerList()
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
                IsBot = p.IsBot
            })
            .ToList();
    }

    public PlayerInfo GetPlayerInfo(CCSPlayerController player)
    {
        return new PlayerInfo
        {
            Name = player.PlayerName,
            SteamID = player.SteamID.ToString(),
            Team = GetTeamName(player.Team),
            ConnectedTime = GetConnectedTime(player),
            SessionTime = GetSessionTime(player),
            IsBot = player.IsBot
        };
    }

    public string GetTeamName(CsTeam team)
    {
        return team switch
        {
            CsTeam.None => "Ninguno",
            CsTeam.Spectator => "Espectador",
            CsTeam.Terrorist => "Terrorista",
            CsTeam.CounterTerrorist => "Anti-Terrorista",
            _ => "Desconocido"
        };
    }

    private long GetConnectedTime(CCSPlayerController player)
    {
        // Devuelve el tiempo de conexión en segundos desde Unix epoch
        return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }

    private TimeSpan? GetSessionTime(CCSPlayerController player)
    {
        if (_config.TrackConnectionTime && player.SteamID > 0)
        {
            return _connectionTracker.GetPlayerSessionTime(player.SteamID);
        }
        return null;
    }
}