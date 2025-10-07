using System.Text.Json.Serialization;

namespace PlayerListPlugin.Configuration;

public class TableFormatConfig
{
    [JsonPropertyName("ShowBotColumn")]
    public bool ShowBotColumn { get; set; } = true;

    [JsonPropertyName("ShowTeamColumn")]
    public bool ShowTeamColumn { get; set; } = true;

    [JsonPropertyName("ShowConnectedTimeColumn")]
    public bool ShowConnectedTimeColumn { get; set; } = true;
}