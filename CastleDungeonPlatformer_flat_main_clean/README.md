# Castle / Dungeon Platformer (Godot 4.4 + C#)

**Controls**: A/D move, Space jump (double), S crouch, Shift dash, F fire  
**Scenes**: Run `scenes/Main.tscn`

## Features
- Player: movement, double jump, crouch, wall slide, dash, projectile
- Enemy: NavigationAgent2D chases player (NavigationRegion2D included)
- UI: Coin counter (HUD)
- Particles: jump burst & coin pickup, torch flame + light
- TileMap: castle tileset placeholder (32x32). NavigationRegion2D provides nav out-of-the-box; replace with TileMap nav when ready.
- Clean C# only scripts; GameManager autoload

## Setup
1. Open project in Godot 4.4 (C#). First run will build the .NET solution.
2. Confirm `GameManager.cs` is autoloaded (already set in `project.godot`).
3. Press ▶️ to play `Main.tscn`.

## Notes
- Replace placeholder PNGs in `assets/` with your pixel art.
- To use TileMap-based pathfinding, paint Navigation in the TileSet and delete/disable `Navigation` (NavigationRegion2D) in Main.
