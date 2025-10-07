using System.Collections.Generic;
using PlayerListPlugin.Models;

namespace PlayerListPlugin.Helpers;

public class FilterUtility
{
    public static void ApplyFilters(List<PlayerInfo> players, string[] args, int startIndex)
    {
        for (int i = startIndex; i < args.Length; i++)
        {
            var arg = args[i].ToLower();
            
            switch (arg)
            {
                case "--team:t":
                case "--terrorist":
                    players.RemoveAll(p => p.Team != "Terrorista");
                    break;
                case "--team:ct":
                case "--counterterrorist":
                    players.RemoveAll(p => p.Team != "Anti-Terrorista");
                    break;
                case "--team:spectator":
                    players.RemoveAll(p => p.Team != "Espectador");
                    break;
                case "--bots":
                case "--include-bots":
                    // No hacer nada, los bots ya están incluidos
                    break;
                case "--no-bots":
                case "--exclude-bots":
                    players.RemoveAll(p => p.IsBot);
                    break;
                case "--players-only":
                    players.RemoveAll(p => p.IsBot);
                    break;
            }
        }
    }
}