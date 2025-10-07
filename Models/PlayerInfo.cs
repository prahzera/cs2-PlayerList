using System;

namespace PlayerListPlugin.Models;

public class PlayerInfo
{
    public string Name { get; set; } = string.Empty;
    public string SteamID { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public long ConnectedTime { get; set; }
    public TimeSpan? SessionTime { get; set; }
    public bool IsBot { get; set; }
}