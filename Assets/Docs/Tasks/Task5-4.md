# Task ID: 5.4
# Parent Task ID: 5
# Title: Performance Optimization & Profiling
# Status: pending
# Dependencies: All previous systems. Game should be feature-complete or near-complete.
# Priority: critical (for shippable product)
# Estimated Effort: XL (can be ongoing)
# Assignee: Unassigned

# Description:
Systematically profile the game on target platforms/specs to identify performance bottlenecks (CPU, GPU, Memory). Implement optimizations such as object pooling, reducing GC allocations, and optimizing rendering.

# Details:
This is an iterative process of profiling, identifying issues, implementing fixes, and re-profiling.
- **Profiling:** Regularly use Unity Profiler (`Window > Analysis > Profiler`). Attach to builds on target hardware if possible.
    - CPU Usage: Identify expensive scripts, physics, rendering calls.
    - GPU Usage: Identify draw calls, fill rate, shader complexity issues.
    - Memory: Track allocations, GC spikes, total memory usage.
- **Object Pooling:** Implement for frequently instantiated/destroyed objects like projectiles, particles, SFX AudioSources, and potentially enemies. (VFXManager and AudioManager already started this).
- **CPU Optimization:**
    - Minimize work in `Update()`/`FixedUpdate()`. Cache component references.
    - Reduce GC Allocations: Avoid `new` in loops, string operations in hot paths, unnecessary LINQ. Use non-allocating versions of Physics APIs where possible.
- **GPU Optimization:**
    - Draw Calls: Use Sprite Atlases, Static/Dynamic Batching, GPU Instancing for similar meshes/materials.
    - Overdraw: Optimize UI layout, use opaque sprites where possible, manage sprite sorting layers.
    - Shaders: Simplify complex shaders, use Shader LOD if applicable.
- **Memory Optimization:**
    - Texture Management: Use appropriate compression (ASTC, ETC2), reduce resolutions for non-critical textures, enable mipmaps.
    - Audio Management: Use compressed audio formats (Vorbis), stream BGM from disk instead of loading fully into memory.
- **Event System Optimization:** Ensure no excessive event raising or listener overhead.
- **Physics Optimization:** Check for unnecessary Rigidbody/Collider components, optimize physics layers and collision matrix (`Edit > Project Settings > Physics 2D`).

Refer to subtasks for specific areas of optimization, though many of these are continuous improvement efforts.

# Acceptance Criteria:
- Game is profiled on target specifications, and key bottlenecks are identified.
- Object pooling is implemented for projectiles, VFX, and SFX AudioSources.
- Measurable improvements in CPU usage (e.g., reduced spikes, lower average frame time).
- Reduction in GC allocations and GC-related performance hitches.
- Optimized GPU performance (e.g., reduced draw calls, acceptable fill rate).
- Optimized memory usage (textures, audio).
- Game maintains a stable target framerate (e.g., 30/60 FPS) on target platforms.

# Test Strategy:
- **Baseline Profiling:** Profile the game before optimizations to establish a baseline.
- **Iterative Profiling:** After each significant optimization, re-profile to measure impact.
- **Target Platform Testing:** Deploy and profile on actual target hardware (e.g., specific mobile devices, consoles, min-spec PC).
- **Stress Testing:** Test scenes with many enemies, VFX, and physics objects to identify worst-case performance.

# Notes/Questions:
- Performance optimization is often an ongoing task throughout development and especially towards the end.
- The specific target platforms and performance goals (e.g., target FPS) must be defined by the project leads/designers.
- Some optimizations (like Sprite Atlasing) might require changes to art assets or import settings.