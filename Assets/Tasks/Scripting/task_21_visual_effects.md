# Task ID: 21
# Parent Task ID: None
# Title: Visual Effects
# Status: completed
# Dependencies: 1
# Priority: medium
# Estimated Effort: S
# Assignee: AI

# Description:
Implement Visual Effects.

# Details:
## Implementation Complete
- Created `VFXManager` for efficient pooling and management of visual effects
- Implemented `VFXUtils` for runtime creation of common effects (dust, hits, heals)
- Created `PlayerVFX` component to handle player-specific visual effects
- Added `Dissolve` shader with configurable edge effects
- Implemented `DissolveEffect` component for dissolve/screen transition effects
- Added comprehensive unit tests for all VFX systems

## Key Features
- Object pooling for optimal performance
- Support for both pre-made and runtime-generated effects
- Easy-to-use API for playing and managing effects
- Configurable parameters for all visual effects
- Support for screen-space and world-space effects
- Integration with existing game systems

## Files Created/Modified
- `Assets/_Project/Scripts/VFX/VFXManager.cs` - Manages VFX pooling and playback
- `Assets/_Project/Scripts/VFX/VFXUtils.cs` - Utility functions for common VFX
- `Assets/_Project/Scripts/VFX/PlayerVFX.cs` - Player-specific VFX handling
- `Assets/_Project/Scripts/VFX/DissolveEffect.cs` - Controls dissolve/screen transition effects
- `Assets/_Project/Shaders/Dissolve.shader` - Custom shader for dissolve effects
- `Assets/_Project/Scripts/Tests/VFXTests.cs` - Unit tests for VFX systems

## Usage Examples
```csharp
// Play a VFX using the VFXManager
VFXManager.Instance.PlayVFX("Explosion", position, rotation);

// Create a dust effect at runtime
VFXUtils.CreateDustEffect(transform.position, Color.white, 1f);

// Start a dissolve effect on an object
var dissolve = GetComponent<DissolveEffect>();
dissolve.StartDissolve();
```

## Notes
- All effects are optimized for performance using object pooling
- The system is designed to be easily extended with new effects
- Supports both 2D and 3D effects
- Includes editor utilities for testing and debugging
