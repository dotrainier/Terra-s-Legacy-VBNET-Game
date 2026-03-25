# Terra's Legacy - Game Code Documentation

## Quick Start

Terra's Legacy is a WPF-based 2D action game where the player fights enemies in real-time combat.

---

## Keyboard Controls

| Key | Action | Function |
|-----|--------|----------|
| **A** or **←** | Move Left | `isLeft = True` in `windowKeyDown()` |
| **D** or **→** | Move Right | `isRight = True` in `windowKeyDown()` |
| **Space** | Jump | `Character.isJumping = True` in `windowKeyDown()` |
| **U** | Attack | `Character.isAttacking = True` in `windowKeyDown()` |

**Location:** [Game.xaml.vb](Game.xaml.vb#L781-L800)

To modify controls, edit the key checks in `windowKeyDown()` and `windowKeyUp()` methods.

---

## ProjectLocation - Purpose & Importance

### What is ProjectLocation?

```vb
ProjectLocation = AppDomain.CurrentDomain.BaseDirectory
ProjectLocation = ProjectLocation.Replace("\bin\Debug\net8.0-windows\", "\")
```

### Why This Matters

1. **Gets Base Directory** - `AppDomain.CurrentDomain.BaseDirectory` returns the compiled executable's directory: `\bin\Debug\net8.0-windows\`

2. **Stores Project Root** - The `.Replace()` strips the build path to get back to the **project root** directory where assets are located

3. **Asset Loading** - Assets (images, sounds, sprites) are stored in the `Assets/` folder at the **project root**, not in the build output

### Example Usage
```vb
Dim soundPath = ProjectLocation & "Assets\Sound\Jumping.wav"
Dim imagePath = ProjectLocation & "Assets\ui\buttons\startBtn.png"
```

**Without the Replace:** The app would look for assets in `\bin\Debug\net8.0-windows\Assets\` (doesn't exist) ❌  
**With the Replace:** The app correctly loads from `\Assets\` at project root ✓

---

## Core Classes & Key Functions

### Game.xaml.vb (Main Game Loop)

| Function | Purpose |
|----------|---------|
| `windowKeyDown()` | Detects pressed keys (movement, jump, attack) |
| `MovementTimer_Tick()` | Updates player position based on key input |
| `CharacterJumpingTimer_Tick()` | Handles jump physics and gravity |
| `PlayerAttackEnemy_Tick()` | Detects collision between attack range and enemies |
| `EnemyMovement_Tick()` | Updates enemy positions (AI behavior) |
| `SpawnEnemy_Tick()` | Spawns random enemies at intervals |

**Modify:** Change `Gravity`, `JumpSpeed`, `Character.CharacterSpeed` constants to adjust game feel.

### Character.vb (Player)

| Property | Default | Use |
|----------|---------|-----|
| `Health` | 300 | Player HP (regenerates on enemy kill) |
| `CharacterSpeed` | 8 | Movement speed (pixels per frame) |
| `CurrentState` | PlayerState.Idle | Enum: Idle, Running, Attacking, Dead, Jump |
| `isJumping` | False | Jump state flag |
| `isAttacking` | False | Attack state flag |

**Modify:** Edit properties in `Character.vb` constructor to adjust player stats.

### Enemy.vb (AI Enemies)

| Property | Goblin | Warhog |
|----------|--------|--------|
| `Health` | 100 | 100 |
| `Damage` | Variable | Variable |
| `Speed` | AI-driven (chases player) | Random walks |

**Modify:** Edit `TypeCharacteristics()` method to change enemy AI behavior and stats.

### Animation System

Sprites are stored in `Assets/characterActions/` by action type:
- `IdleLeft/IdleRight` - 4 frames
- `RunLeft/RunRight` - 8 frames  
- `AttackLeft/AttackRight` - 8 frames
- `DeadLeft/DeadRight` - 8 frames
- `JumpLeft/JumpRight` - Jump animation

**To modify animations:** Update frame counts in `Character.vb` constructor and replace sprite images in asset folders.

---

## Key Timers (Game Loop)

| Timer | Interval | Purpose |
|-------|----------|---------|
| `MovementTimer` | 30ms | Player movement & state changes |
| `CharacterJumpingTimer` | 30ms | Jump physics (gravity simulation) |
| `PlayerAttackEnemy` | 50ms | Player attack collision detection |
| `EnemyMovement` | 50ms | Enemy AI movement |
| `EnemyAttackPlayer` | 100ms | Enemy attack collision & damage |
| `SpawnEnemy` | Variable | Enemy spawning (2.2s, 500ms, 5s intervals) |

---

## BodyBoxes.vb (Collision System)

Defines hitbox dimensions for accurate combat collision:

```vb
FootWidth = 25        ' Character visual width
HitBoxWidth = 10      ' Damage collision area
AttackBoxWidth = 40   ' Attack range
```

**Modify:** Adjust these values to change combat feel and hit detection accuracy.

---

## Common Modifications

### Change Player Speed
```vb
' In Character.vb constructor
Public Property CharacterSpeed As Integer = 8  ' Increase for faster movement
```

### Adjust Jump Height
```vb
' In Game.xaml.vb
Dim Force As Integer = 20  ' Higher = stronger jump
```

### Change Enemy Spawn Rate
```vb
' In Game.xaml.vb, SpawnEnemy_Tick()
Dim numbers As New List(Of Integer)() From {2200, 500, 500, 5000, 100, 4000}
' Modify these millisecond values
```

### Add New Keyboard Control
```vb
' In Game.xaml.vb, windowKeyDown()
ElseIf e.Key = Key.YOUR_KEY Then
    ' Your action here
End If
```

---

## Project Structure

```
TerrasLegacy/
├── Game.xaml / Game.xaml.vb          (Main game scene & logic)
├── Home.xaml / Home.xaml.vb          (Menu scene)
├── Character.vb                      (Player class)
├── Enemy.vb                          (Enemy AI)
├── Sound.vb                          (Audio manager)
├── BodyBoxes.vb                      (Collision hitbox base class)
└── Assets/
    ├── Sound/                        (WAV files)
    ├── characterActions/             (Player sprites)
    ├── Mob/                          (Enemy sprites)
    ├── Tiles/                        (Background tiles)
    └── ui/                           (Menu buttons & icons)
```

---

## Sound Effects

Sounds are loaded dynamically via [Sound.vb](Sound.vb) using ProjectLocation:

```vb
Dim soundPath = ProjectLocation & "Assets\Sound\Jumping.wav"
Sound.PlaySound(soundPath)
```

Add new sounds by placing .wav files in `Assets/Sound/` and calling `PlaySound()` with the asset path.

---

## Troubleshooting

| Issue | Cause | Fix |
|-------|-------|-----|
| Assets not loading | ProjectLocation not set | Ensure `.Replace()` removes build path correctly |
| Game runs very slow | Timer intervals too short | Increase `Interval` in DispatcherTimer definitions |
| Movement feels sluggish | CharacterSpeed too low | Increase `CharacterSpeed` constant |
| Jump doesn't work | `Gravity` constant too high | Decrease `Gravity` value in Game.xaml.vb |

---

**For questions on specific behavior, check the commented code sections and timer callbacks.**
