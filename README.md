# Mayrit

## Sinopsis

Mayrit es un videojuego que sumerge al jugador en la época del Madrid musulmán, permitiéndole experimentar la vida de diferentes ciudadanos y personajes históricos, y presenciar el desarrollo del asentamiento fortificado. El juego abarca desde la fundación de la ciudad hasta la conquista cristiana, mostrando la evolución de Mayrit y su sociedad a través de diversas perspectivas. El objetivo principal es dar a conocer el rico patrimonio histórico y cultural musulmán, tanto material como inmaterial, y su influencia en la ciudad incluso en la actualidad.

## Mecánicas Principales

- **Control de diferentes personajes**: Cada uno con jugabilidad y narrativa específicas. La narrativa se desarrollará a medida que el jugador completa los objetivos propios del personaje/etapa, proporcionando contexto histórico y justificando su impacto en la ciudad.

- **Cambio de Perspectiva**: El jugador cambiará entre tercera persona (para las actividades de los personajes) y una vista global de la ciudad (para la observación de la ciudad).

- **Exploración**: El jugador podrá explorar un entorno 3D que representa la ciudad de Mayrit en diferentes etapas de su desarrollo.

- **Progresión histórica**: La construcción de edificios y la llegada de nuevos personajes seguirán una línea temporal basada en la historia de Madrid.

## Períodos Históricos

El juego incluye 8 hitos históricos principales que marcan la evolución de Mayrit:

1. **Visión** - Los inicios y la concepción de Mayrit
2. **Fundación** - El establecimiento formal de la ciudad
3. **Albacar** - Desarrollo del recinto amurallado exterior
4. **Almudayna** - Construcción de la alcazaba o ciudadela
5. **Ataque de Ramiro II** - Conflictos con los reinos cristianos
6. **Almanzor** - Época del poderoso caudillo musulmán
7. **Escuela** - Desarrollo educativo y cultural
8. **Conquista** - La conquista cristiana que marca el final del período musulmán

## Requisitos Técnicos

### Requisitos de Sistema
- **Motor**: Unity 6000.1.11f1
- **Plataforma**: PC (Windows)
- **Resolución**: 1024x768 (mínima)

### Requisitos de Desarrollo
- Unity 6000.1.11f1 o superior
- Visual Studio o Visual Studio Code
- .NET Framework compatible con Unity

## Estructura del Proyecto

```
Mayrit/
├── Assets/
│   ├── Scripts/
│   │   ├── Camera system/     # Sistema de cámaras (tercera persona, espectador, orbital)
│   │   ├── Characters/        # Sistema de personajes jugables
│   │   ├── Game/             # Lógica principal del juego
│   │   ├── Information/      # Sistema de información contextual
│   │   ├── Objects/          # Objetos interactivos
│   │   ├── Progress/         # Gestión de progreso e hitos históricos
│   │   ├── UI/               # Interfaz de usuario
│   │   └── Utils/            # Utilidades generales
│   ├── Scenes/
│   │   ├── MainMenuScene.unity    # Menú principal
│   │   ├── GameScene.unity        # Escena principal del juego
│   │   └── PacoScene.unity        # Escena de desarrollo
│   ├── Prefabs/              # Prefabricados de Unity
│   ├── Terrain/              # Terrenos y entornos 3D
│   └── UI/                   # Elementos de interfaz
├── ProjectSettings/          # Configuración del proyecto Unity
└── README.md                # Este archivo
```

## Instalación y Configuración

1. **Clonar el repositorio**:
   ```bash
   git clone https://github.com/fkavaro/Mayrit.git
   cd Mayrit
   ```

2. **Abrir en Unity**:
   - Abrir Unity Hub
   - Hacer clic en "Add project from disk"
   - Seleccionar la carpeta `Mayrit/` del proyecto
   - Asegurarse de tener Unity 6000.1.11f1 instalado

3. **Configuración inicial**:
   - El proyecto debería cargar automáticamente todas las dependencias
   - La escena principal se encuentra en `Assets/Scenes/GameScene.unity`

## Desarrollo

### Arquitectura

El proyecto utiliza varios patrones de diseño:

- **Singleton**: Para managers principales (GameManager, CameraManager, ProgressManager, UIManager)
- **State Machine**: Para la gestión de estados de cámara y progreso
- **ScriptableObjects**: Para almacenar información de personajes, edificios e hitos históricos
- **Observer Pattern**: Para eventos de cambio de hito y actualización de tiempo

### Sistemas Principales

- **Sistema de Cámaras**: Gestiona múltiples tipos de cámara (tercera persona, espectador, orbital)
- **Sistema de Personajes**: Controla el movimiento, animaciones y comportamiento de personajes
- **Sistema de Progreso**: Maneja la progresión temporal e histórica del juego
- **Sistema de Información**: Proporciona contenido educativo contextual

## Créditos

**Desarrollado por**: Universidad Rey Juan Carlos

## Licencia

[Información de licencia por determinar]
