using System;
using System.Collections.Concurrent;
using CounterStrikeSharp.API.Core;

namespace PlayerListPlugin.Services;

public class PlayerConnectionTracker
{
    private readonly ConcurrentDictionary<ulong, DateTime> _playerConnectTimes = new();

    public void OnPlayerConnect(CCSPlayerController player)
    {
        if (player.IsValid && player.SteamID > 0)
        {
            _playerConnectTimes[player.SteamID] = DateTime.Now;
        }
    }

    public void OnPlayerDisconnect(CCSPlayerController player)
    {
        if (player.SteamID > 0)
        {
            _playerConnectTimes.TryRemove(player.SteamID, out _);
        }
    }

    public DateTime? GetPlayerConnectTime(ulong steamId)
    {
        return _playerConnectTimes.TryGetValue(steamId, out var connectTime) ? connectTime : null;
    }

    public TimeSpan? GetPlayerSessionTime(ulong steamId)
    {
        if (_playerConnectTimes.TryGetValue(steamId, out var connectTime))
        {
            return DateTime.Now - connectTime;
        }
        return null;
    }

    public void Clear()
    {
        _playerConnectTimes.Clear();
    }
}