# Task ID: 5.4.5
# Parent Task ID: 5.4
# Title: Optimize Memory Usage (Textures, Audio)
# Status: pending
# Dependencies: 5.4.1 (Profiling results)
# Priority: medium
# Estimated Effort: L (can involve art/audio asset changes)
# Assignee: Unassigned

# Description:
Analyze memory profiling data to identify large memory consumers, particularly textures and audio clips. Implement optimizations such as texture compression, resolution reduction, and audio compression/streaming.

# Details:
1.  **Review Profiler Data (Memory Module):**
    *   Use the "Detailed" view of the Memory Profiler to see what types of assets are consuming the most memory (Textures, AudioClips, Meshes, Animations, etc.).
    *   Identify specific large assets.
2.  **Texture Optimization:**
    *   **Compression:**
        *   Select texture assets in Project window. In Inspector, set Texture Type to `Sprite (2D and UI)` or `Default` as appropriate.
        *   Choose an appropriate Compression format based on target platform and quality needs (e.g., ASTC for mobile, DXT/BCn for PC). `Crunch Compression` can also be good.
        *   Adjust compression quality if needed.
    *   **Resolution:**
        *   Reduce `Max Size` for textures that are displayed small on screen or are less critical (e.g., UI elements, distant backgrounds). Avoid using 4K textures if 1K or 2K suffices.
        *   Use "POT" (Power of Two) dimensions for textures (e.g., 128x128, 256x512) for better compatibility and some compression benefits, though modern hardware is more flexible.
    *   **Mipmaps:**
        *   Enable mipmaps (`Generate Mip Maps`) for textures viewed at varying distances (mostly 3D, but can help with aliasing on scaled 2D sprites if trilinear filtering is used). For pixel art, usually off unless scaling artifacts are an issue.
    *   **Read/Write Enabled:** Disable `Read/Write Enabled` flag for textures unless you specifically need to access their pixel data from scripts at runtime (it doubles memory usage).
    *   **Sprite Atlases:** (Also a GPU optimization) Using atlases reduces memory by packing multiple sprites into fewer, larger textures, but also helps by reducing the number of texture objects in memory.
3.  **Audio Optimization:**
    *   **Compression:**
        *   Select AudioClip assets. In Inspector, set Compression Format.
        *   `Vorbis` is good for most BGM and SFX (good quality/size ratio).
        *   `ADPCM` is very small but lower quality, suitable for short, simple SFX if memory is extremely tight.
        *   Adjust `Quality` slider for Vorbis.
    *   **Load Type:**
        *   For BGM (long audio files): Set `Load Type` to `Streaming`. This streams audio from disk instead of loading the entire clip into memory.
        *   For SFX (short clips): `Decompress On Load` loads uncompressed audio into memory for fast playback (uses more RAM). `Compressed In Memory` keeps it compressed in RAM and decompresses on the fly (uses less RAM, more CPU). Choose based on SFX length, frequency, and memory/CPU budget.
    *   **Preload Audio Data:**
        *   For critical sounds that must play instantly, check `Preload Audio Data`. For less critical or streamed sounds, uncheck it.
    *   **Force To Mono:** If stereo isn't needed for certain sounds, check `Force To Mono` to reduce size.
4.  **Other Memory Considerations:**
    *   **Meshes/Animations:** Optimize complexity if they are major memory consumers (usually more relevant for 3D games).
    *   **ScriptableObjects/Code:** Large data structures or collections in scripts can consume memory. Review and optimize if identified by profiler.
    *   **Asset Unloading:** Ensure unused assets are being unloaded correctly by Unity (e.g., `Resources.UnloadUnusedAssets()`, though use with caution as it can cause hitches. Better to manage asset loading/unloading explicitly with Addressables if memory is tight).

# Acceptance Criteria:
- Key large textures are identified and optimized (compression, resolution).
- BGM audio clips are set to `Streaming` load type and compressed.
- SFX audio clips are appropriately compressed and configured for their load type.
- Measurable reduction in total memory usage, particularly for textures and audio.
- No significant loss of visual or audio quality unless it's an accepted trade-off.

# Test Strategy:
- **Memory Profiling:** Use Unity Profiler's Memory module (Detailed view) before and after optimizations to track changes in memory usage for Textures, AudioClips, and total memory.
- **Visual/Audio Review:** After optimizing assets, check them in-game to ensure quality is still acceptable.
- **Platform Testing:** Memory limits are often stricter on mobile/console. Test on target devices.

# Notes/Questions:
- This often requires re-importing assets after changing import settings.
- Balancing quality and memory footprint is key.
- For very large projects or many assets, the Addressable Asset System is recommended for finer-grained memory management and content delivery (beyond scope of this plan's detail).