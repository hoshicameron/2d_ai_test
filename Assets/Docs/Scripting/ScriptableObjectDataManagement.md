# ScriptableObject Data Management System

This document provides an overview of the ScriptableObject-based data management system used in the "Petals of Hope" project. This system is designed to provide a flexible and efficient way to manage game data, including entity stats, abilities, and level settings.

## Table of Contents

1. [Overview](#overview)
2. [Core Components](#core-components)
   - [Entity Stats](#entity-stats)
   - [Abilities](#abilities)
   - [Level Settings](#level-settings)
3. [Editor Integration](#editor-integration)
4. [Best Practices](#best-practices)
5. [Example Usage](#example-usage)
6. [Troubleshooting](#troubleshooting)

## Overview

The ScriptableObject Data Management system leverages Unity's ScriptableObjects to create data assets that can be edited in the Unity Editor and referenced by game objects at runtime. This approach provides several benefits:

- **Non-destructive editing**: Changes to data assets don't require code changes
- **Version control friendly**: Data is stored as assets that can be easily versioned
- **Runtime efficiency**: Data is loaded into memory once and shared across objects
- **Designer-friendly**: Non-programmers can tweak game balance without touching code

## Core Components

### Entity Stats

Entity stats are used to define the attributes of game entities (players, enemies, bosses, etc.). The system is built around a base `EntityStatsSO` class with derived classes for specific entity types.

#### Key Classes:

- **`EntityStatsSO`**: Base class for all entity stats
  - Common properties: Health, Movement Speed, Damage, Defense
  - Validation methods
  - Events for stat changes

- **`PlayerStatsSO`**: Player-specific stats
  - Jump force, air jumps, dash settings
  - Experience and level progression

- **`EnemyStatsSO`**: Enemy-specific stats
  - Detection range, attack patterns
  - Score value, loot drops

- **`BossStatsSO`**: Boss-specific stats (inherits from EnemyStatsSO)
  - Phase-based behavior
  - Special abilities

### Abilities

Abilities define special actions that entities can perform, such as double jumping or dashing. The system is built around a base `AbilitySO` class with derived classes for specific abilities.

#### Key Classes:

- **`AbilitySO`**: Base class for all abilities
  - Cooldown management
  - Activation and deactivation logic
  - Common properties: Name, Description, Icon

- **`DoubleJumpSO`**: Double jump ability
  - Configurable jump force and gravity
  - Visual and audio feedback

- **`DashSO`**: Dash/movement ability
  - Configurable distance and duration
  - Invincibility frames
  - Trail effects

### Level Settings

Level settings define the configuration for individual game levels, including visual style, gameplay parameters, and progression.

#### Key Classes:

- **`LevelSettingsSO`**: Configuration for a single level
  - Scene reference
  - Visual style (colors, lighting)
  - Gameplay parameters (time limit, gravity)
  - Progression requirements

## Editor Integration

The system includes custom editor scripts to enhance the Unity Editor workflow:

- **Stats Data Editor**: Custom inspectors for all stat types
  - Validation buttons
  - Default value presets
  - Debug information

- **Ability Editor**: Custom inspectors for abilities
  - Visual feedback for ability states
  - Test buttons for in-editor testing
  - Default value presets

- **Level Settings Editor**: Tools for managing level data
  - Scene assignment
  - Visual style previews
  - Progression setup

## Best Practices

1. **Use Inheritance Wisely**: Create appropriate base classes and derive specific types from them
2. **Validate Data**: Implement validation methods to catch invalid data early
3. **Use ScriptableObject.Instantiate**: Create runtime copies of ScriptableObjects when you need to modify them
4. **Organize Assets**: Keep data assets organized in a logical folder structure
5. **Use Addressables**: For larger projects, consider using Addressables to manage ScriptableObject loading and unloading

## Example Usage

### Creating a New Enemy Type

1. Right-click in the Project window
2. Select Create > Petals of Hope > Stats > Enemy Stats
3. Name the asset (e.g., "WolfEnemyStats")
4. Configure the stats in the Inspector
5. Assign to an enemy prefab or spawner

### Creating a New Ability

1. Right-click in the Project window
2. Select Create > Petals of Hope > Abilities > [Ability Type]
3. Name the ability (e.g., "FireballAbility")
4. Configure the ability parameters
5. Assign to a player or enemy prefab

### Setting Up a New Level

1. Create a new scene
2. Right-click in the Project window
3. Select Create > Petals of Hope > Levels > Level Settings
4. Configure the level settings
5. Assign the scene to the Level Settings asset
6. Set up progression links to other levels

## Troubleshooting

### Common Issues

1. **Changes not saving**: Make sure to save the scene and the ScriptableObject asset
2. **Null reference errors**: Ensure all required references are assigned in the Inspector
3. **Performance issues**: Avoid modifying ScriptableObjects at runtime; create runtime copies instead
4. **Version control conflicts**: Use Unity's Smart Merge for .asset files

### Debugging Tips

1. Enable debug logging in the ScriptableObject's Inspector
2. Use the validation buttons to check for configuration issues
3. Check the Console window for any error messages
4. Use the test buttons in the Editor to verify ability behavior

## Extending the System

To add new stat types or abilities:

1. Create a new class that inherits from the appropriate base class
2. Add any additional properties or methods
3. Create a custom editor if needed
4. Add menu items for easy asset creation

## Conclusion

The ScriptableObject Data Management system provides a powerful and flexible way to manage game data in "Petals of Hope". By following the patterns and best practices outlined in this document, you can efficiently create and maintain a wide variety of game content.
