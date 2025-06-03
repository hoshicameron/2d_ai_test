# Task ID: 1.3.4
# Parent Task ID: 1.3
# Title: Create Editor Tools for Data Management/Validation (Optional)
# Status: pending
# Dependencies: 1.3.1, 1.3.2, 1.3.3 # The SOs to manage/validate
# Priority: low
# Estimated Effort: M
# Assignee: Unassigned

# Description:
(Optional) Implement basic editor tools or custom editors for ScriptableObject data to aid in management or perform validation on the data (e.g., ensure health is always positive).

# Details:
This task is marked as optional in the Implementation Plan. If undertaken:
1.  **Identify Validation Needs:**
    *   Review the created `EntityStatsSO`, `AbilitySO`, `LevelSettingsSO` and their derivatives.
    *   Identify fields that have implicit constraints (e.g., `maxHealth > 0`, `cooldown >= 0`).
    *   Many basic validations can be done with attributes like `[Min(0)]`. This task would cover more complex or cross-field validations if needed.

2.  **Implement Custom Editors or `OnValidate`:**
    *   **Using `OnValidate()`:**
        *   For simple, per-SO validation, add an `OnValidate()` method to the ScriptableObject scripts themselves.
        ```csharp
        // Example in EntityStatsSO.cs
        protected virtual void OnValidate()
        {
            if (maxHealth <= 0)
            {
                maxHealth = 1;
                Debug.LogWarning($"maxHealth for {name} was invalid and reset to 1. Please set a positive value.", this);
            }
        }
        ```
        *   Call `base.OnValidate()` in derived classes if they also implement it.
    *   **Custom Editors:**
        *   If more complex UI or validation logic is needed in the Inspector, create custom editors (similar to Task 1.2.6 for events).
        *   Example: A custom editor for `PlayerStatsSO` could show derived stats (e.g., DPS if relevant) or provide buttons for common actions.
    *   **Editor Window for Overview/Bulk Editing:**
        *   A more advanced tool could be an Editor Window that lists all `PlayerStatsSO` (or other types) in the project, allowing for comparison or bulk modification. This is likely out of scope for "basic" tools.

3.  **Consider a "Validate All Project Data" Tool:**
    *   A utility that can be run from a menu item to scan all relevant SO assets in the project and report any validation errors in the console.
    *   This would involve using `AssetDatabase.FindAssets` to locate all SOs of specific types and then running validation logic on each.

# Acceptance Criteria:
- (If `OnValidate` is used) Key ScriptableObject data classes (Stats, Abilities, Levels) implement `OnValidate()` to enforce critical data constraints (e.g., non-negative values for health, speed, cooldowns).
- Invalid data entered in the Inspector is automatically corrected or a warning is logged.
- (If custom editors are made) Custom editors provide improved UI or validation feedback for specific SO types.
- (If a validation tool is made) A menu item exists to trigger project-wide data validation, and it reports issues.

# Test Strategy:
- Manual Verification:
    - Edit SO assets in the Inspector and try to input invalid data (e.g., negative health).
    - Verify that `OnValidate()` corrects the data or logs a warning.
    - If custom tools are built, test their functionality thoroughly.

# Notes/Questions:
- The plan marks this as "optional". The `[Min()]` attribute already handles some basic validation. `OnValidate()` is a good lightweight way to add further self-correction or warnings.
- For this initial pass, implementing `OnValidate()` for critical fields in the SOs (like ensuring `maxHealth > 0`) is a good first step if this task is pursued.
- Focus on simple, high-value validations first.