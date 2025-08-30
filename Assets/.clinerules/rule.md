# Unity Project Rules - Petals of Hope

This document outlines the standard rules and best practices for developing the "Petals of Hope" Unity 2D project. Adhering to these guidelines ensures consistency, maintainability, and quality across the project. These rules are based on Unity's recommendations, C# best practices, and project-specific considerations (e.g., pixel art style, 2D gameplay).

## 1. Project Structure
- **Organize Assets Logically**: Use folders under `Assets/PetalsOfHope/` to categorize content. Avoid cluttering the root Assets folder.
  - `Scenes/`: All Unity scenes (e.g., Level01, Menu).
  - `Scripts/`: C# scripts, organized by feature (e.g., Player/, Enemies/).
  - `Art/`: Sprites, textures, animations (focus on pixel art; see Art Style Guide in Docs/).
  - `Audio/`: Sound effects, music.
  - `Prefabs/`: Reusable GameObjects.
  - `ScriptableObjects/`: Data containers for levels, items, etc.
  - `Tests/`: Unit tests (use Unity Test Runner).
- **Avoid Unnecessary Folders**: Keep the hierarchy shallow; no deep nesting unless required.
- **Generic Unity Folders**: Use `Resources/`, `Plugins/`, `Shaders/`, etc., only for global assets. Limit to essential cross-scene items.

## 2. Naming Conventions
- **GameObjects**: Use PascalCase (e.g., "PlayerSprite", "EnemySpawner"). Prefix with feature (e.g., "UI_", "FX_").
- **Scripts**: Match script name to class name using PascalCase (e.g., "PlayerController.cs"). Keep descriptive but concise.
- **Variables and Methods**:
  - Serialized Private: camelCase with [SerializeField] (e.g., `[SerializeField] private int count;`).
  - Non-serialized Private: camelCase with underscore prefix (e.g., `private int _count;`).
  - Public/Properties: PascalCase (e.g., `PlayerHealth`).
  - Constants: UPPER_SNAKE_CASE (e.g., `MAX_SCORE`).
- **Assets**: Descriptive names (e.g., "Player_Idle_SpriteSheet.png").
- **Scenes and Prefabs**: Use underscores for readability (e.g., "Level_01.scene", "Player.prefab").
- **Tags and Layers**: Define in `Project Settings` and use consistently.

## 3. Coding Standards
- **C# Style**:
  - Use tabs for indentation (4 spaces) to match Unity's default.
  - Maximum line length: 120 characters.
  - Use regions (`#region`/`#endregion`) to organize code in large scripts (e.g., Constants, Events).
- **Best Practices**:
  - Avoid `MonoBehaviour` overuse; use Scriptable Objects for data-heavy logic.
  - Use `SerializeField` for private fields editable in Inspector.
  - Implement the Singleton pattern sparingly (e.g., for GameManager); prefer dependency injection.
  - Handle null checks and error logging (e.g., `Debug.LogWarning`).
  - Use Unity's `EventSystem` for inter-object communication instead of direct references.
- **Performance**:
  - Minimize `Update()` calls; use `FixedUpdate()` for physics.
  - Pool objects (e.g., bullets) using Object Pooling.
  - Use Layers and Collision Masks effectively for 2D collisions.
  - Profile with Unity Profiler; aim for target frame rate (e.g., 60 FPS).
- **Unity-Specific**:
  - Use `Awake()` for initialization, `Start()` for references.
  - Attach scripts intelligently; avoid attaching to every object.
  - Leverage Unity's built-in tools like DOTween for animations (as per Plugins/).

## 4. Asset Management
- **Texture Settings**: For pixel art, set Filter Mode to "Point", Compression to "None", and snap to pixels.
- **Audio**: Use 16-bit WAV for sounds; compress longer tracks.
- **Shaders**: Keep custom shaders simple; reference existing ones (e.g., Dissolve.shader).
- **Prefabs Variants**: Use for slight modifications; keep base prefabs clean.

## 5. Version Control (Git)
- **Git Workflow**: Use feature branches (e.g., `feature/enemy-ai`); merge via pull requests.
- **Ignore Patterns**: Use `.gitignore` for build files, temp assets, and large binaries.
- **Commits**: Write clear messages (e.g., "Fix player jump physics"). Commit frequently but logically.
- **Collaboration**: Pull regularly; resolve conflicts carefully (especially on scenes/scenes).

## 6. Documentation and Testing
- **Code Comments**: Use XML comments for public methods (e.g., `/// <summary>Description</summary>`).
- **Project Docs**: Maintain in `Assets/Docs/` (e.g., reference existing GDD, Technical Design Docs).
- **Testing**: Write unit tests for-scripts in `Tests/` folder using NUnit.
- **Changelogs**: Update in Docs for major changes.

## 7. Project-Specific Guidelines (Petals of Hope)
- **2D Focus**: All art in pixel art style; animations via Sprite Sheets or DOTween.
- **Gameplay**: Implement features from Docs (e.g., enemy AI, hazards); prioritize data-driven design with ScriptableObjects.
- **UI/UX**: Follow UI/UX Design Doc; use TextMesh Pro for text.
- **Builds**: Target platforms from Project Plan; test on device regularly.
- **Art Style**: Adhere to "Petals of Hope - Updated Art Style Guide (Pixel Art).md" in Docs/.

## 8. General Best Practices
- **Collaboration**: Use Unity Collab or Git for team sync.
- **Quality Assurance**: Playtest frequently; use Unity Analytics for metrics if applicable.
- **Updates**: Keep Unity version updated but stable; avoid experimental features.
- **Deprecation**: Remove unused assets/scripts periodically.

Violations of these rules should be discussed in team reviews. For questions, refer to Unity documentation or project Docs.
