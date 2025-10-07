using System.Text.Json.Serialization;
using CounterStrikeSharp.API.Core;

namespace PlayerListPlugin.Configuration;

public class PlayerListConfig : BasePluginConfig
{
    [JsonPropertyName("EnablePlayerInfoCommand")]
    public bool EnablePlayerInfoCommand { get; set; } = true;

    [JsonPropertyName("EnableFilters")]
    public bool EnableFilters { get; set; } = true;

    [JsonPropertyName("TrackConnectionTime")]
    public bool TrackConnectionTime { get; set; } = true;

    [JsonPropertyName("EnableExport")]
    public bool EnableExport { get; set; } = true;

    [JsonPropertyName("TableFormat")]
    public TableFormatConfig TableFormat { get; set; } = new TableFormatConfig();
}