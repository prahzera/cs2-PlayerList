using System;
using System.Linq;
using CounterStrikeSharp.API;
using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Cvars;
using CounterStrikeSharp.API.Modules.Entities;
using PlayerListPlugin.Models;

namespace PlayerListPlugin.Services;

public class ServerService
{
    private readonly DateTime _pluginStartTime;

    public ServerService(DateTime pluginStartTime)
    {
        _pluginStartTime = pluginStartTime;
    }

    public ServerInfo GetServerInfo()
    {
        var serverInfo = new ServerInfo
        {
            HostName = ConVar.Find("hostname")?.StringValue ?? "Nombre del Servidor",
            MapName = NativeAPI.GetMapName(),
            MaxPlayers = Server.MaxPlayers,
            CurrentPlayers = Utilities.GetPlayers().Count(p => p.IsValid && p.SteamID > 0),
            Uptime = DateTime.Now - _pluginStartTime
        };

        return serverInfo;
    }
}