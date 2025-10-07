# PlayerList Plugin para CS2

[![English](https://img.shields.io/badge/README-English-blue)](README.md)
[![Español](https://img.shields.io/badge/README-Español-red)](README_es.md)

Este plugin para Counter-Strike 2 agrega comandos RCON que permiten obtener información de los jugadores conectados al servidor y estadísticas del servidor. El formato de salida depende de los parámetros proporcionados:

- Sin parámetros: Muestra tabla formateada
- Con parámetro `json`: Devuelve JSON con estadísticas adicionales de los jugadores (kills, deaths, assists, etc.)

**Nota importante**: Los comandos solo pueden ser ejecutados desde la consola del servidor o RCON. Los jugadores en el juego no pueden ejecutar estos comandos.

## Características

- Comando `playerlist` con salida condicional (tabla o JSON) e información del servidor
- Comando `serverinfo` para obtener información detallada del servidor
- Comando `playerinfo` para obtener información detallada de un jugador específico
- Filtros para el comando [`playerlist`](https://github.com/prahzera/cs2-PlayerList/blob/main/Commands/PlayerListCommand.cs)
- Seguimiento de tiempo de sesión de los jugadores (tiempo desde que el jugador se conectó al servidor)
- Configuración personalizable usando el sistema de configuración de CounterStrikeSharp
- **Salida JSON mejorada**: Estadísticas de jugadores (kills, deaths, assists) disponibles solo en formato JSON
- **Restricción de seguridad**: Solo puede ser ejecutado por administradores del servidor (consola o RCON)

## Comandos disponibles

### playerlist
Muestra una lista de todos los jugadores conectados con información del servidor.

**Uso:**
```
playerlist [json] [filtros]
```

**Formatos de salida:**
- `playerlist` - Muestra una tabla formateada con información del servidor (sin estadísticas de jugadores)
- `playerlist json` - Devuelve JSON con información del servidor y estadísticas de jugadores (kills, deaths, assists)

**Filtros disponibles:**
- `--team:t` o `--terrorist`: Mostrar solo jugadores del equipo terrorista
- `--team:ct` o `--counterterrorist`: Mostrar solo jugadores del equipo contra-terrorista
- `--team:spectator`: Mostrar solo espectadores
- `--no-bots` o `--exclude-bots`: Excluir bots de la lista
- `--players-only`: Mostrar solo jugadores humanos

**Ejemplos:**
```
playerlist
playerlist json
playerlist json --no-bots
playerlist --team:t --no-bots
```

### serverinfo
Muestra información detallada del servidor.

**Uso:**
```
serverinfo [json]
```

**Formatos de salida:**
- `serverinfo` - Muestra una tabla formateada con información del servidor
- `serverinfo json` - Devuelve JSON con información del servidor

**Ejemplos:**
```
serverinfo
serverinfo json
```

### playerinfo
Muestra información detallada de un jugador específico.

**Uso:**
```
playerinfo <steamid>
```

**Ejemplo:**
```
playerinfo 76561198012345678
```

## Instalación

1. Compila el plugin usando `dotnet build`
2. Copia el archivo `PlayerListPlugin.dll` generado en la carpeta `plugins` de tu servidor CS2
3. Reinicia el servidor

La primera vez que se ejecute el plugin, CounterStrikeSharp creará automáticamente un archivo de configuración en [`configs/plugins/PlayerListPlugin/PlayerListPlugin.json`](https://github.com/prahzera/cs2-PlayerList/blob/main/configs/plugins/PlayerListPlugin/PlayerListPlugin.json) con la configuración predeterminada.

## Uso

### Mostrar tabla de jugadores con información del servidor

Ejecuta el comando `playerlist` desde la consola del servidor:

```
playerlist
```

Esto mostrará una tabla formateada como:

```
========================================================================
SERVIDOR: Servidor de Ejemplo | MAPA: de_dust2
JUGADORES: 2/16
========================================================================
Nombre               SteamID           Equipo          Bot   Tiempo Sesión  
------------------------------------------------------------------------
Juan Perez           76561198012345678 Terrorists      No    15m 30s        
Bot1                 0                 Counter-Terrorists Si    12m 45s        
------------------------------------------------------------------------
Total de jugadores: 2 | Tiempo activo: 25m 10s
========================================================================
```

### Mostrar JSON de jugadores con información del servidor y estadísticas

Ejecuta el comando `playerlist json` desde la consola del servidor o RCON:

```
playerlist json
```

Esto devolverá un JSON como:

```json
{
  "ServerInfo": {
    "HostName": "Servidor de Ejemplo",
    "MapName": "de_dust2",
    "MaxPlayers": 16,
    "CurrentPlayers": 2,
    "Uptime": "00:25:10"
  },
  "PlayerCount": 2,
  "Players": [
    {
      "Name": "Juan Perez",
      "SteamID": "76561198012345678",
      "Team": "Terrorists",
      "ConnectedTime": 1697049600,
      "SessionTime": "00:15:30",
      "IsBot": false,
      "Kills": 5,
      "Deaths": 2,
      "Assists": 3
    },
    {
      "Name": "Bot1",
      "SteamID": "0",
      "Team": "Counter-Terrorists",
      "ConnectedTime": 1697049600,
      "SessionTime": "00:12:45",
      "IsBot": true,
      "Kills": 0,
      "Deaths": 0,
      "Assists": 0
    }
  ]
}
```

Nota: Las estadísticas de jugadores (kills, deaths, assists) solo están disponibles en formato JSON, no en la tabla formateada.

El "Tiempo Sesión" muestra cuánto tiempo lleva el jugador conectado al servidor. Esta función se puede habilitar o deshabilitar en el archivo de configuración.

### Mostrar información del servidor

Ejecuta el comando `serverinfo` desde la consola del servidor:

```
serverinfo
```

Esto mostrará una tabla formateada como:

```
========================================================================
INFORMACIÓN DEL SERVIDOR
========================================================================
Nombre del servidor:     Servidor de Ejemplo
Mapa actual:             de_dust2
Jugadores:               2/16
Tiempo activo plugin:    25m 10s
========================================================================
```

## Configuración

El plugin utiliza el sistema de configuración integrado de CounterStrikeSharp. La primera vez que se ejecuta, se crea automáticamente un archivo de configuración en:

```
configs/plugins/PlayerListPlugin/PlayerListPlugin.json
```

Contenido predeterminado:
```json
{
  "EnablePlayerInfoCommand": true,
  "EnableFilters": true,
  "TrackConnectionTime": true,
  "TableFormat": {
    "ShowBotColumn": true,
    "ShowTeamColumn": true,
    "ShowConnectedTimeColumn": true
  },
  "ConfigVersion": 1
}
```

Opciones de configuración:
- `EnablePlayerInfoCommand`: Habilita/deshabilita el comando `playerinfo`
- `EnableFilters`: Habilita/deshabilita filtros para el comando `playerlist`
- `TrackConnectionTime`: Habilita/deshabilita el seguimiento de tiempo de sesión de los jugadores
- `TableFormat`: Controla qué columnas se muestran en la tabla formateada
  - `ShowBotColumn`: Muestra/oculta la columna "Bot"
  - `ShowTeamColumn`: Muestra/oculta la columna "Equipo"
  - `ShowConnectedTimeColumn`: Muestra/oculta la columna "Tiempo Sesión"

## Seguridad

Todos los comandos están restringidos para ser usados únicamente por administradores del servidor. Los jugadores en el juego no pueden ejecutar estos comandos, lo que previene el acceso no autorizado a la información de los jugadores.

## Requisitos

- Counter-Strike Sharp
- .NET 8.0 SDK