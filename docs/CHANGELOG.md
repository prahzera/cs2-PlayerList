# Changelog

## [1.3.0] - 2025-10-06

### Modificado
- Reorganización completa del proyecto en una estructura modular
- Eliminación del campo TickRate ya que en CS2 se usa subticks
- Uso de cvar para obtener el HostName del servidor
- Creación de directorios separados para Commands, Services, Models, Configuration y Helpers
- Mejora de la escalabilidad y mantenibilidad del código

### Directorios
- **Commands**: Contiene todos los comandos del plugin
- **Services**: Lógica de negocio principal
- **Models**: Modelos de datos
- **Configuration**: Configuración del plugin
- **Helpers**: Funciones auxiliares y utilitarias

### Comandos
- `playerlist`: Muestra una lista de todos los jugadores conectados
- `serverinfo`: Muestra información detallada del servidor
- `playerinfo`: Muestra información detallada de un jugador específico
- `exportplayerlist`: Exporta la lista de jugadores a un archivo JSON

## [1.2.0] - 2025-10-5

### Añadido
- Comando `exportplayerlist` para exportar la lista de jugadores a un archivo JSON
- Seguimiento de tiempo de sesión de los jugadores
- Filtros para el comando playerlist

## [1.1.0] - 2025-10-3

### Añadido
- Comando `playerinfo` para obtener información detallada de un jugador específico
- Comando `serverinfo` para obtener información detallada del servidor
- Soporte para salida en formato JSON

## [1.0.0] - 2025-10-02

### Añadido
- Versión inicial del plugin
- Comando `playerlist` con salida en formato tabla