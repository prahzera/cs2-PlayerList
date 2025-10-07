using System.Collections.Generic;

namespace PlayerListPlugin.Models;

public class PlayerListResponse
{
    public ServerInfo? ServerInfo { get; set; }
    public int PlayerCount { get; set; }
    public List<PlayerInfo> Players { get; set; } = new List<PlayerInfo>();
}