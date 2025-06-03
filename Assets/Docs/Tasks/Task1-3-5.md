# Task ID: 1.3.5
# Parent Task ID: 1.3
# Title: Create Initial Data Assets
# Status: completed
# Dependencies: 1.3.1, 1.3.2, 1.3.3 # The SO definitions
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Create initial ScriptableObject data assets for player stats, enemy stats, abilities, and level settings, placing them in appropriate subfolders under `_Project/ScriptableObjects/`.

# Details:
1.  **Navigate to `Assets/_Project/ScriptableObjects/` and its subfolders.** (e.g., `Data/Player/`, `Data/Enemies/`, `Data/Abilities/`, `Data/Levels/`)

2.  **Create Player Stats Assets:**
    *   Folder: `Assets/_Project/ScriptableObjects/Data/Player/`
    *   Create `PlayerStatsSO` asset:
        *   Name: `DefaultPlayerStatsSO`
        *   Configure values (example): `maxHealth = 100`, `movementSpeed = 7`, `jumpForce = 15`.

3.  **Create Enemy Stats Assets:**
    *   Folder: `Assets/_Project/ScriptableObjects/Data/Enemies/`
    *   Create `EnemyStatsSO` asset for a "Wolf" type enemy:
        *   Name: `WolfEnemyStatsSO`
        *   Configure values (example): `maxHealth = 50`, `patrolSpeed = 2`, `chaseSpeed = 5`, `detectionRange = 10`, `damage = 10`.
    *   Create `EnemyStatsSO` asset for a "Spider" type enemy (if different stats):
        *   Name: `SpiderEnemyStatsSO`
        *   Configure values.
    *   (Optional) Create a `BossStatsSO` asset if a placeholder boss is considered early:
        *   Folder: `Assets/_Project/ScriptableObjects/Data/Enemies/Bosses/`
        *   Name: `PlaceholderBossStatsSO`
        *   Configure values.

4.  **Create Ability Data Assets:**
    *   Folder: `Assets/_Project/ScriptableObjects/Data/Abilities/Types/`
    *   Create `DoubleJumpSO` asset:
        *   Name: `DefaultDoubleJumpAbilitySO`
        *   Configure values (example): `doubleJumpForceMultiplier = 0.8`. Ensure `abilityName`, `description`, `icon` (placeholder if no art yet) are set.
    *   Create `DashSO` asset:
        *   Name: `DefaultDashAbilitySO`
        *   Configure values (example): `dashSpeed = 25`, `dashDuration = 0.15`, `cooldown = 1.5`. Ensure `abilityName`, `description`, `icon` are set.

5.  **Create Level Settings Assets:**
    *   Folder: `Assets/_Project/ScriptableObjects/Data/Levels/`
    *   Create `LevelSettingsSO` asset for a tutorial or first level:
        *   Name: `TutorialLevelSettingsSO`
        *   Configure values (example): `gravityScale = 1.0`, `backgroundMusic = null` (or assign placeholder clip if available). Set `defaultPlayerSpawnPosition`.
    *   Create one for a generic test level:
        *   Name: `TestLevelSettingsSO`
        *   Configure values.

6.  **Organization:**
    *   Ensure assets are placed in the correct subfolders within `_Project/ScriptableObjects/` as per the structure defined in Task 1.1.2.

# Acceptance Criteria:
- At least one `PlayerStatsSO` asset (`DefaultPlayerStatsSO`) is created and configured with initial values.
- At least one `EnemyStatsSO` asset (e.g., `WolfEnemyStatsSO`) is created and configured.
- At least one of each placeholder ability SO (`DefaultDoubleJumpAbilitySO`, `DefaultDashAbilitySO`) is created and configured.
- At least one `LevelSettingsSO` asset (e.g., `TutorialLevelSettingsSO`) is created and configured.
- All created assets are saved in their designated subfolders under `Assets/_Project/ScriptableObjects/`.

# Test Strategy:
- Manual Verification:
    - Navigate to the specified folders in the Unity Project window.
    - Verify that the assets exist.
    - Select each asset and inspect its values in the Inspector to confirm they are set as expected.

# Notes/Questions:
- Created initial placeholder values that can be tuned during development and playtesting.
- All asset creation directories have been set up at `Assets/_Project/ScriptableObjects/Data/`.
- Default assets include:
  - Player: `DefaultPlayerStatsSO`
  - Enemies: `WolfEnemyStatsSO`, `SpiderEnemyStatsSO`
  - Boss: `PlaceholderBossStatsSO`
  - Abilities: `DefaultDoubleJumpAbilitySO`, `DefaultDashAbilitySO`
  - Levels: `TutorialLevelSettingsSO`, `TestLevelSettingsSO`
- Implementation completed on 2025-06-03. All initial assets are ready for use in the game.