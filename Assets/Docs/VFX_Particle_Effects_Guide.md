# VFX Particle Effects Creation Guide for Petals of Hope

This guide provides detailed instructions for creating particle effects using Unity's Shuriken Particle System. Each effect is designed for a 2D pixel art game, focusing on performance, visual clarity, and integration with the VFX pooling system.

## General Setup for All Effects

1. **Create a Particle System**:
   - In Unity Editor: `GameObject → Effects → Particle System`
   - Rename the GameObject to the effect name (e.g., "JumpDust_VFX")

2. **Base Configuration (apply to all effects)**:
   - **Duration**: 0.5 - 2 seconds (depending on effect)
   - **Start Delay**: 0
   - **Start Lifetime**: 0.5 - 2 seconds
   - **Start Speed**: 1 - 2 (low for subtle effects)
   - **Start Size**: 0.1 - 0.5 (small for pixel art scale)
   - **Start Rotation**: 0 (or random for variety)
   - **Gravity Modifier**: 0 (disable gravity unless falling effect)
   - **Max Particles**: 10 - 50 (keep low for performance)
   - **Play on Awake**: *Uncheck* (effects will be triggered by script)
   - **Stop Action**: Disable (for pooling compatibility)
   - **Simulation Space**: World (to avoid camera issues)
   - **Renderer**: Material: Create new Sprite material, assign pixel art texture if available

3. **Pixel Art Tips**:
   - Use low particle count and small sizes
   - Set Filter Mode to "Point" for crisp pixels
   - Use Sorting Layer: "Effects" (create if needed)
   - Disable shape textures or use simple sprites

4. **Create Prefab**:
   - Drag the GameObject from Hierarchy to `PetalsOfHope/Prefabs/VFX/` folder
   - Delete the instance from the scene

5. **VFX System Integration**:
   - Add prefab reference to VFXSystem component's `initialVfxData` list
   - Set VFXType accordingly (need to define enums in script)

## Specific Effect Configurations

### 1. Player Jump (Dust Puff)

**Description**: Small dust cloud rising from feet when jumping.

1. **Emission Module**:
   - Rate over Time: 20
   - Burst: 10 particles at 0.1s

2. **Shape Module**:
   - Shape: Circle
   - Radius: 0.1
   - Emit from: Base

3. **Velocity over Lifetime**:
   - X/Y: Random 0.5, Z: 1 (upward drift)

4. **Color over Lifetime**:
   - Gradient: Brown → Transparent (dust effect)

5. **Size over Lifetime**:
   - Curve: Grow to max then shrink

6. **Duration**: 0.5 seconds
7. **Max Particles**: 20

### 2. Player Land (Impact Dust)

**Description**: Brief dust explosion on ground contact after jump.

1. **Emission Module**:
   - Rate over Time: 30
   - Burst: 15 particles at 0s

2. **Shape Module**:
   - Shape: Cone
   - Angle: 30°
   - Emit from: Base

3. **Velocity over Lifetime**:
   - X: Random ±0.5, Y: 1 (upward burst)

4. **Color over Lifetime**:
   - Gradient: Tan → Transparent (ground dust)

5. **Size over Lifetime**:
   - Curve: Start small, grow quickly

6. **Duration**: 0.3 seconds
7. **Max Particles**: 30

### 3. Talisman Collect (Sparkle/Glow)

**Description**: Bright sparkles when collecting items.

1. **Emission Module**:
   - Rate over Time: 15
   - Burst: 5 particles at 0.1s

2. **Shape Module**:
   - Shape: Sphere
   - Radius: 0.2
   - Emit from: Volume

3. **Velocity over Lifetime**:
   - Random: 0.5 in all axes

4. **Color over Lifetime**:
   - Gradient: Yellow/Gold → White → Transparent

5. **Size over Lifetime**:
   - Curve: Pulsing/sparkle effect

6. **Duration**: 0.8 seconds
7. **Max Particles**: 15

### 4. Player/Enemy Hit (Sparks, Flash)

**Description**: Impact sparks with flash effect.

1. **Emission Module**:
   - Rate over Time: 25
   - Burst: 20 particles at 0s

2. **Shape Module**:
   - Shape: Hemispherical
   - Radius: 0.1

3. **Velocity over Lifetime**:
   - X/Y: Random ±2, Z: 0.5 (outward burst)

4. **Color over Lifetime**:
   - Gradient: Red/Orange → White → Transparent

5. **Size over Lifetime**:
   - Curve: Flicker/explode

6. **Duration**: 0.4 seconds
7. **Max Particles**: 40

### 5. Enemy Death (Smoke/Puff/Explosion)

**Description**: Smoke cloud or small puff on enemy death.

1. **Emission Module**:
   - Rate over Time: 20
   - Burst: 10 particles at 0s

2. **Shape Module**:
   - Shape: Sphere
   - Radius: 0.15
   - Emit from: Volume

3. **Velocity over Lifetime**:
   - Random: 0.8 in all axes

4. **Color over Lifetime**:
   - Gradient: Gray → Black → Transparent

5. **Size over Lifetime**:
   - Curve: Expand over time

6. **Duration**: 1.0 seconds
7. **Max Particles**: 25

### 6. Player Dash (Trail/Burst)

**Description**: Speed trail or start/end burst for dash.

1. **Emission Module**:
   - Rate over Time: 30
   - Burst: 10 particles at 0.05s

2. **Shape Module**:
   - Shape: Box
   - Box X/Y: 0.1, Z: 0.5
   - Rotate for trail direction

3. **Velocity over Lifetime**:
   - Linear: Backward direction relative to movement

4. **Color over Lifetime**:
   - Gradient: Blue/White → Transparent

5. **Size over Lifetime**:
   - Curve: Thin trail effect

6. **Duration**: 0.6 seconds
7. **Max Particles**: 35

### 7. Wall Slide (Dust/Sparks)

**Description**: Scrape dust or sparks when sliding against walls.

1. **Emission Module**:
   - Rate over Time: Continuous, 15

2. **Shape Module**:
   - Shape: Box
   - Box X: 0.05, Y/Z: 0.1
   - Rotate to face wall direction

3. **Velocity over Lifetime**:
   - Linear: Sideways motion

4. **Color over Lifetime**:
   - Gradient: Brown for dust, Orange for sparks

5. **Size over Lifetime**:
   - Curve: Small and brief

6. **Duration**: 1.2 seconds (continuous while sliding)
7. **Max Particles**: 20

### 8. Weapon Swing/Impact (If Applicable)

**Description**: Brief slash effect or impact flash.

1. **Emission Module**:
   - Rate over Time: 25
   - Burst: 15 particles at 0s

2. **Shape Module**:
   - Shape: Cone
   - Rotate for swing direction

3. **Velocity over Lifetime**:
   - Radial: Outward burst

4. **Color over Lifetime**:
   - Gradient: Silver/White → Transparent

5. **Size over Lifetime**:
   - Curve: Quick expand and fade

6. **Duration**: 0.3 seconds
7. **Max Particles**: 30

## Testing and Optimization

### Testing Each Effect
1. Drag prefab into scene
2. Select Particle System component
3. Click Play in Inspector to preview
4. Adjust settings as needed

### Performance Optimization
- Keep Max Particles under 100 total
- Use Rate over Time instead of Emissions Scale
- Disable unnecessary modules
- Profile with Unity Profiler

### Pixel Art Considerations
- Use sprite textures instead of procedural shapes
- Match game resolution (e.g., if game is 256px high, scale accordingly)
- Test on target platform for crispness

## Integration with VFX System

Once prefabs are created:
1. Open VFXSystem in scene
2. Add each prefab to `initialVfxData`
3. Define/assign appropriate VFXType enum value
4. Trigger via VFXRequestEvent

This guide ensures consistent, performant VFX that integrate seamlessly with the project's pixel art style and VFX pooling system.
