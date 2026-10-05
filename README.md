# UTHMGO — Location-Based AR Treasure Hunt

UTHMGO is a mobile location-based augmented reality treasure hunt developed with **Unity and C#**. The experience combines real-world GPS positioning with AR interactions and location-based riddles, encouraging players to explore selected locations around the UTHM campus to discover and collect virtual treasure chests.

> **Portfolio repository:** This repository currently contains the core source scripts and Unity package/project-version configuration prepared from the project files. Scenes, prefabs, media, and other redistributable project assets can be added separately after licensing and file-size review.

## Gameplay Flow

`Riddle` → `Navigate to Location` → `GPS Proximity Check` → `Treasure Chest Appears` → `Open Chest` → `Take Treasure` → `Progress +1` → `Next Riddle` → `Victory`

## Key Features

- GPS-based treasure location detection across five UTHM campus locations
- Geographic proximity calculation using the Haversine formula
- Runtime treasure chest spawning near the player's camera
- Interactive chest opening using Unity Animator
- Location-based riddle progression
- Treasure collection counter and completion tracking
- Android camera and fine-location permission handling
- Victory scene after all treasures are collected
- Background music and menu/settings UI scripts

## Technologies

- **Unity:** 6000.0.43f1
- **Language:** C#
- **AR:** AR Foundation 6.0.6, Google ARCore XR Plugin 6.0.6
- **Rendering:** Universal Render Pipeline 17.0.4
- **UI:** Unity UI / TextMeshPro
- **Platform:** Android-oriented mobile project
- **Location:** Unity Location Service (`Input.location`)

## Core Technical Implementation

### GPS Proximity Detection

The application reads the player's latitude and longitude through Unity Location Services. Each treasure has a predefined geographic coordinate. The distance between the player and each treasure is calculated using the **Haversine formula**, providing a practical distance estimate for proximity-based spawning.

### Treasure Spawning

When the player enters the configured proximity radius, the corresponding chest prefab is instantiated in front of the main camera. A single `currentChest` reference prevents multiple active treasure instances from being spawned at the same time.

### Chest Interaction

The spawned chest receives references to the scene's **Open** and **Take Treasure** buttons at runtime. Opening the chest triggers the Animator's `Open` parameter. Taking the treasure invokes a callback to the location manager, marks the treasure as collected, updates progress, removes the active chest, and advances the experience.

### Progression

The project tracks collected treasures and updates the riddle UI for the next location. Once all five treasures are collected, the game loads the victory scene.

## Main Source Files

| Script | Responsibility |
| --- | --- |
| `LocationBased.cs` | GPS initialization, distance calculation, treasure spawning, collection tracking, riddles, and victory progression |
| `INTERACT_CHEST.cs` | Runtime UI binding, chest animation, and treasure collection callback |
| `Chestspawn.cs` | Earlier/auxiliary chest spawning implementation retained from the original project |
| `MUSIC.cs` | Music persistence and volume control |
| `PLAY_BUTTON.cs` | Main play-button scene navigation |
| `SETTING_BUTTON*.cs` | Settings UI navigation |
| `TUTORIAL.cs` | Tutorial navigation |
| `ABOUT_BUTTON.cs` / `HOW_BUTTON.cs` | Supporting menu navigation |

## Development Challenge

One key challenge was inconsistent proximity detection during real-world GPS testing. The distance calculation was refined using the Haversine formula. The spawning flow was also controlled through a single active-chest reference to reduce duplicate treasure instances.

## Repository Structure

```text
UTHMGO-location-based-ar-treasure-hunt/
├── Assets/
│   └── Scripts/
├── Documentation/
│   └── Screenshots/
├── Packages/
├── ProjectSettings/
├── .gitignore
└── README.md
```

## Screenshots / Demo

Add selected screenshots to `Documentation/Screenshots/`, for example:

- Main menu / UTHMGO landing screen
- Riddle interface
- GPS/location gameplay
- AR treasure chest before opening
- Opened treasure chest
- Treasure counter / progression
- Victory screen

A short 30–60 second gameplay demo is also recommended for the portfolio version of this repository.

## Running the Project

The current repository package is a **source-code showcase**, not yet a complete redistributable Unity project. To make it fully runnable, add the original project scenes, prefabs, UI assets, AR setup, and other required assets that you have the right to redistribute. Open the completed project using **Unity 6000.0.43f1** and allow Unity Package Manager to restore the dependencies in `Packages/manifest.json`.

## Notes

- GPS coordinates in the original source correspond to treasure locations used for the UTHM campus experience.
- Do not commit Android signing keys, passwords, API credentials, generated Unity folders, or third-party assets without redistribution permission.
- The original MonoBehaviour class/file names are retained to avoid breaking Unity scene and prefab references.

## Author

Developed as a Unity mobile AR project for an academic/portfolio context.
