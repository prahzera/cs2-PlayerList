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
    
    // Estadísticas adicionales (solo para JSON)
    public int Kills { get; set; }
    public int Deaths { get; set; }
    public int Assists { get; set; }
}