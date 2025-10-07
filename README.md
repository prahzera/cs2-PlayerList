# PlayerList Plugin for CS2

[![English](https://img.shields.io/badge/README-English-blue)](README.md)
[![Español](https://img.shields.io/badge/README-Español-red)](README_es.md)

This plugin for Counter-Strike 2 adds RCON commands that allow you to get information about players connected to the server and server statistics. The output format depends on the parameters provided:

- Without parameters: Displays formatted table
- With `json` parameter: Returns JSON

**Important note**: Commands can only be executed from the server console or RCON. In-game players cannot execute these commands.

## Features

- `playerlist` command with conditional output (table or JSON) and server information
- `serverinfo` command to get detailed server information
- `playerinfo` command to get detailed information about a specific player
- `exportplayerlist` command to export the player list to a JSON file
- Filters for the [`playerlist`](https://github.com/prahzera/cs2-PlayerList/blob/main/Commands/PlayerListCommand.cs) command
- Player session time tracking
- Customizable configuration using CounterStrikeSharp's configuration system
- **Security restriction**: Can only be executed by server administrators (console or RCON)

## Available Commands

### playerlist
Displays a list of all connected players with server information.

**Usage:**
```
playerlist [json] [filters]
```

**Output formats:**
- `playerlist` - Displays a formatted table with server information
- `playerlist json` - Returns JSON with server and player information

**Available filters:**
- `--team:t` or `--terrorist`: Show only terrorist team players
- `--team:ct` or `--counterterrorist`: Show only counter-terrorist team players
- `--team:spectator`: Show only spectators
- `--no-bots` or `--exclude-bots`: Exclude bots from the list
- `--players-only`: Show only human players

**Examples:**
```
playerlist
playerlist json
playerlist json --no-bots
playerlist --team:t --no-bots
```

### serverinfo
Displays detailed server information.

**Usage:**
```
serverinfo [json]
```

**Output formats:**
- `serverinfo` - Displays a formatted table with server information
- `serverinfo json` - Returns JSON with server information

**Examples:**
```
serverinfo
serverinfo json
```

### playerinfo
Displays detailed information about a specific player.

**Usage:**
```
playerinfo <steamid>
```

**Example:**
```
playerinfo 76561198012345678
```

### exportplayerlist
Exports the player list to a JSON file in the server's configuration directory.

**Usage:**
```
exportplayerlist
```

## Installation

1. Compile the plugin using `dotnet build`
2. Copy the generated `PlayerListPlugin.dll` file to your CS2 server's `plugins` folder
3. Restart the server

The first time the plugin is run, CounterStrikeSharp will automatically create a configuration file at [`configs/plugins/PlayerListPlugin.json`](https://github.com/usuario/cs2-PlayerList/blob/main/configs/plugins/PlayerListPlugin/PlayerListPlugin.json) with the default settings.

## Usage

### Display player table with server information

Run the `playerlist` command from the server console:

```
playerlist
```

This will display a formatted table like:

```
========================================================================
SERVER: Sample Server | MAP: de_dust2
PLAYERS: 2/16
========================================================================
Name                 SteamID           Team            Bot   Session Time  
------------------------------------------------------------------------
John Doe             76561198012345678 Terrorist       No    15m 30s        
Bot1                 0                 Counter-Terrorist Yes   12m 45s        
------------------------------------------------------------------------
Total players: 2 | Uptime: 25m 10s
========================================================================
```

### Display JSON of players with server information

Run the `playerlist json` command from the server console or RCON:

```
playerlist json
```

This will return JSON like:

```json
{
  "ServerInfo": {
    "HostName": "Sample Server",
    "MapName": "de_dust2",
    "MaxPlayers": 16,
    "CurrentPlayers": 2,
    "Uptime": "00:25:10"
  },
  "PlayerCount": 2,
  "Players": [
    {
      "Name": "John Doe",
      "SteamID": "76561198012345678",
      "Team": "Terrorist",
      "ConnectedTime": 1697049600,
      "SessionTime": "00:15:30",
      "IsBot": false
    },
    {
      "Name": "Bot1",
      "SteamID": "0",
      "Team": "Counter-Terrorist",
      "ConnectedTime": 1697049600,
      "SessionTime": "00:12:45",
      "IsBot": true
    }
  ]
}
```

### Display server information

Run the `serverinfo` command from the server console:

```
serverinfo
```

This will display a formatted table like:

```
========================================================================
SERVER INFORMATION
========================================================================
Server name:         Sample Server
Current map:         de_dust2
Players:             2/16
Plugin uptime:       25m 10s
========================================================================
```

## Configuration

The plugin uses CounterStrikeSharp's built-in configuration system. The first time it runs, a configuration file is automatically created at:

```
configs/plugins/PlayerListPlugin/PlayerListPlugin.json
```

Default content:
```json
{
  "EnablePlayerInfoCommand": true,
  "EnableFilters": true,
  "TrackConnectionTime": true,
  "EnableExport": true,
  "TableFormat": {
    "ShowBotColumn": true,
    "ShowTeamColumn": true,
    "ShowConnectedTimeColumn": true
  },
  "ConfigVersion": 1
}
```

## Security

All commands are restricted for use only by server administrators. In-game players cannot execute these commands, which prevents unauthorized access to player information.

## Requirements

- Counter-Strike Sharp
- .NET 8.0 SDK