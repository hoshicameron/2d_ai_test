# Task ID: 5.4.4
# Parent Task ID: 5.4
# Title: Optimize GPU Usage (Draw Calls, Shaders, Overdraw)
# Status: pending
# Dependencies: 5.4.1 (Profiling results)
# Priority: medium
# Estimated Effort: L (iterative, can involve art pipeline changes)
# Assignee: Unassigned

# Description:
Analyze GPU profiling data to identify rendering bottlenecks. Implement optimizations such as using Sprite Atlases, enabling/fixing batching, simplifying shaders, and reducing overdraw.

# Details:
1.  **Review Profiler Data (GPU Usage, Rendering Module, Stats Window):**
    *   Check total draw calls (`Batches` in Stats window or Profiler). High numbers are often a target for optimization.
    *   Look for reasons why batching might be failing (different materials, shaders, textures).
    *   Use Frame Debugger (`Window > Analysis > Frame Debugger`) to step through rendering of a frame and see individual draw calls and their states.
2.  **Sprite Atlasing:**
    *   If using many individual sprites, combine them into Sprite Atlases using Unity's Sprite Packer (`Edit > Project Settings > Editor > Sprite Packer Mode`).
    *   Ensure sprites intended to be batched together are on the same atlas.
    *   This reduces draw calls by allowing sprites using the same atlas (material) to be drawn in a single batch.
3.  **Static and Dynamic Batching:**
    *   **Static Batching:** For non-moving environment sprites/meshes, mark them as `Static` in the Inspector. Unity can combine their geometry if they share materials.
    *   **Dynamic Batching:** Unity automatically tries to batch small dynamic sprites/meshes if they share materials. Ensure this is enabled (`Project Settings > Player > Other Settings > Dynamic Batching`). Check Profiler to see if it's working.
4.  **GPU Instancing:**
    *   For rendering many identical meshes (e.g., particles, foliage, repeated props) with the same material, enable GPU Instancing on the material. The shader must support instancing.
5.  **Shader Optimization:**
    *   If custom shaders are used, simplify them if they are too complex (many texture lookups, complex calculations).
    *   For built-in shaders, choose the simplest one that meets visual requirements (e.g., Unlit vs. Lit if lighting isn't needed).
    *   Consider Shader LOD (Level of Detail) for different quality settings.
6.  **Overdraw Reduction:**
    *   Overdraw is when the same pixel is rendered multiple times in a frame (e.g., transparent UI panels overlapping, many layers of particles).
    *   Use the Overdraw view mode in the Scene view (`Scene view tab > Draw Mode > Overdraw`) to visualize it.
    *   **UI:** Minimize overlapping transparent UI elements. Use opaque backgrounds where possible.
    *   **Sprites:** Use opaque sprites instead of transparent ones if transparency isn't needed. Be mindful of sprite sorting order.
    *   **Particles:** Reduce particle counts, sizes, or use less transparent particles if overdraw is high.
7.  **Texture Optimization (Impacts GPU memory and bandwidth):**
    *   (Also part of Memory Optimization - Task 5.4.5) Use compressed texture formats. Reduce texture dimensions if they are larger than needed for their on-screen size.
8.  **Optimize Sprite Sorting:**
    *   Use Sorting Groups on complex GameObjects with multiple SpriteRenderers to manage their rendering order efficiently.
    *   Minimize changes to Z-position for sorting if using perspective camera for 2D, or rely on Sorting Layers/Order in Layer for orthographic.

# Acceptance Criteria:
- Draw call count (`Batches`) is reduced in key scenes, particularly due to Sprite Atlasing and batching.
- Frame Debugger analysis shows improved batching.
- (If applicable) GPU Instancing is used for suitable repeated elements.
- Overdraw is minimized in UI and gameplay, as verified by Overdraw scene view.
- (If custom shaders were an issue) Complex shaders are simplified or optimized.
- Overall GPU frame time is reduced or game runs smoother on GPU-bound scenarios.

# Test Strategy:
- **Targeted Profiling & Frame Debugger:** After each optimization (e.g., enabling Sprite Packer, marking objects static), use Profiler and Frame Debugger to verify the change had the intended effect on draw calls and batching.
- **Visual Verification:** Ensure optimizations do not negatively impact the game's visual quality significantly unless it's a deliberate trade-off.
- **Platform Testing:** GPU performance can vary greatly between devices. Test on target platforms.

# Notes/Questions:
- This often requires close collaboration with artists (for atlases, texture settings) and potentially tech artists (for shaders).
- Some of these, like Sprite Atlasing, can have a big impact with relatively low effort if sprites are organized well.
- URP (Universal Render Pipeline) has its own batcher (SRP Batcher). Ensure materials and shaders are compatible with it for optimal performance.