using System;

namespace PlayerListPlugin.Models;

public class ServerInfo
{
    public string HostName { get; set; } = string.Empty;
    public string MapName { get; set; } = string.Empty;
    public int MaxPlayers { get; set; }
    public int CurrentPlayers { get; set; }
    public TimeSpan Uptime { get; set; }
}