# Task ID: 4.1.1
# Parent Task ID: 4.1
# Title: Setup Tilemap System with Rule Tiles
# Status: completed
# Dependencies: 1.1.1 # Unity Project Setup (2D URP)
# Priority: high
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Utilize Unity's Tilemap system to create Tile Palettes populated with Rule Tiles for efficient 2D level construction.

# Details:
1.  **Ensure 2D Tilemap Editor Package:**
    *   Verify that the `2D Tilemap Editor` package is installed (usually included with 2D templates). If not, install via Package Manager.
2.  **Prepare Tileset Sprites:**
    *   Obtain or create tileset sprite sheets (e.g., ground, walls, platforms).
    *   Import them into Unity (`Assets/_Project/Art/Tilemaps/Sprites/`).
    *   Set Texture Type to `Sprite (2D and UI)`, Sprite Mode to `Multiple`.
    *   Use the Sprite Editor to slice the sheet into individual tiles. Apply appropriate pivot points and pixels per unit.
3.  **Create Rule Tiles:**
    *   For each logical tile type (e.g., Ground, Platform Edge, Inner Wall), create a Rule Tile asset.
    *   Right-click in Project window -> `Create > 2D > Tiles > Rule Tile`.
    *   Name them descriptively (e.g., `GroundRuleTile`, `PlatformTopRuleTile`).
    *   Store these in `Assets/_Project/Art/Tilemaps/RuleTiles/`.
    *   **Configure Rules:**
        *   Open the Rule Tile asset.
        *   Assign the default sprite.
        *   Add rules based on neighboring tiles (This Tile, Not This Tile, Any Tile). Assign specific sprites for different configurations (e.g., corners, edges, standalone tiles).
        *   This allows for auto-tiling behavior.
        *   Example for Ground: A tile surrounded by other ground tiles uses an inner ground sprite. A tile with no ground tile above it uses a top-edge ground sprite.
4.  **Create Tile Palette(s):**
    *   Open `Window > 2D > Tile Palette`.
    *   Click "Create New Palette". Name it (e.g., `EnvironmentPalette`, `HazardsPalette`).
    *   Save the palette asset in `Assets/_Project/Art/Tilemaps/Palettes/`.
    *   Drag your created Rule Tiles (and any regular Tiles) from the Project window into the Tile Palette window.
5.  **Setup Grid and Tilemap GameObjects in a Test Scene:**
    *   Create a `Grid` GameObject (`GameObject > 2D Object > Tilemap > Rectangular` or `Hexagonal` etc. Use Rectangular for platformers).
    *   This will create a Grid with a child Tilemap GameObject.
    *   Rename Tilemap GameObjects for clarity (e.g., `GroundTilemap`, `PlatformsTilemap`, `HazardsTilemap`).
    *   Add `TilemapCollider2D` component to Tilemaps that need collision (e.g., `GroundTilemap`).
        *   Check "Used by Composite" if you plan to use a `CompositeCollider2D` on the parent Grid or a separate collider GameObject for optimized collision.
    *   (Optional) Add `CompositeCollider2D` to the Grid (or a child GameObject) and `Rigidbody2D` (static) for optimized colliders. TilemapColliders then point their `Used By Composite` to this.
6.  **Basic Level Blockout Test:**
    *   Use the Tile Palette window to paint tiles onto the Tilemap(s) in the test scene.
    *   Verify Rule Tiles behave as expected, automatically selecting appropriate sprites based on neighbors.

# Acceptance Criteria:
- At least one Tile Palette is created (e.g., `EnvironmentPalette`).
- Palette is populated with several Rule Tiles (e.g., for ground, platforms) that demonstrate auto-tiling behavior.
- Rule Tiles use sprites sliced from a tileset.
- A test scene demonstrates painting with these Rule Tiles on a Tilemap, and collisions work correctly.
- Level designers can use this setup to efficiently create level geometry.

# Test Strategy:
- Manual Verification:
    - Create Rule Tiles and configure their rules with different sprites.
    - Create a Tile Palette and add these Rule Tiles.
    - In a test scene, paint on a Tilemap using the palette. Observe if Rule Tiles select the correct sprites based on their neighbors.
    - Add a Player character and test collision with the painted Tilemaps (ensure `TilemapCollider2D` is added and configured).

# Notes/Questions:
- Creating good Rule Tiles can be time-consuming but greatly speeds up level design.
- Consider using multiple Tilemaps for different layers (e.g., background, foreground, solid collision, one-way platforms).
- Ensure `Pixels Per Unit` on tile sprites and `Cell Size` on the Grid component are consistent for correct scaling.