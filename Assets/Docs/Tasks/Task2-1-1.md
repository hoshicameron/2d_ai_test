# Task ID: 2.1.1
# Parent Task ID: 2.1
# Title: Create PlayerInputActions Asset and Define Actions
# Status: completed
# Dependencies: 1.1.3 # Input System Package installed
# Priority: critical
# Estimated Effort: M
# Assignee: Unassigned

# Description:
Create the `PlayerInputActions.inputactions` asset and define the necessary Action Maps (`Gameplay`, `UI`) and their respective Actions.

# Details:
1.  **Create Asset:**
    *   Navigate to `Assets/_Project/Settings/Input/`.
    *   Right-click -> `Create > Input Actions`. Name it `PlayerInputActions`.
2.  **Open Asset Editor:** Double-click the `PlayerInputActions.inputactions` asset to open the editor.
3.  **Define Action Maps:**
    *   Create an Action Map named `Gameplay`.
    *   Create an Action Map named `UI`.
4.  **Define Actions for `Gameplay` Action Map:**
    *   **`Move`**:
        *   Action Type: `Value`
        *   Control Type: `Vector2`
        *   Bindings:
            *   WASD (Composite: 2D Vector): W (Up), S (Down), A (Left), D (Right)
            *   Left Stick [Gamepad] (Usage: Primary2DMotion)
            *   D-Pad [Gamepad]
    *   **`Jump`**:
        *   Action Type: `Button`
        *   Bindings:
            *   Space [Keyboard]
            *   Button South [Gamepad] (e.g., A on Xbox, X on PlayStation)
    *   **`Dash`**:
        *   Action Type: `Button`
        *   Bindings:
            *   Left Shift [Keyboard]
            *   Button East [Gamepad] (e.g., B on Xbox, Circle on PlayStation) or Button West [Gamepad] (X on Xbox, Square on PS) - decide on one. Plan doesn't specify. Let's assume Button West for now.
    *   **`Interact`**:
        *   Action Type: `Button`
        *   Bindings:
            *   E [Keyboard]
            *   Button North [Gamepad] (e.g., Y on Xbox, Triangle on PlayStation)
5.  **Define Actions for `UI` Action Map:**
    *   **`Navigate`**:
        *   Action Type: `Value`
        *   Control Type: `Vector2`
        *   Bindings:
            *   WASD (Composite: 2D Vector)
            *   Arrow Keys (Composite: 2D Vector)
            *   Left Stick [Gamepad]
            *   D-Pad [Gamepad]
    *   **`Submit`**:
        *   Action Type: `Button`
        *   Bindings:
            *   Enter [Keyboard]
            *   Space [Keyboard]
            *   Button South [Gamepad]
    *   **`Cancel`**:
        *   Action Type: `Button`
        *   Bindings:
            *   Escape [Keyboard]
            *   Button East [Gamepad]
6.  **Generate C# Class:**
    *   In the Inspector for the `PlayerInputActions.inputactions` asset:
        *   Check "Generate C# Class".
        *   Class Name: `PlayerInputActions` (or keep default if it's already that).
        *   Namespace: `PetalsOfHope.Core.Input` (or leave blank if you want it in global, but namespace is good).
        *   File Path: `Assets/_Project/Scripts/Core/Input/PlayerInputActions.cs` (ensure it's generated here and not alongside the asset if you specify path). Let Unity manage the path if it generates it next to the .inputactions file, then move it if necessary. The plan asks for InputReader in Scripts/Core/Input, so the generated class would ideally also be there or nearby. For now, let's assume Unity generates it and it's fine.
    *   Click "Apply" to save changes and generate/update the C# class.

# Acceptance Criteria:
- `PlayerInputActions.inputactions` asset is created in `Assets/_Project/Settings/Input/`.
- Action Maps `Gameplay` and `UI` are defined.
- Actions `Move`, `Jump`, `Dash`, `Interact` are defined under `Gameplay` with appropriate types, controls, and bindings for keyboard and gamepad.
- Actions `Navigate`, `Submit`, `Cancel` are defined under `UI` with appropriate types, controls, and bindings for keyboard and gamepad.
- A C# class for the input actions is generated successfully.

# Test Strategy:
- Manual Verification:
    - Open the `PlayerInputActions.inputactions` asset and inspect all action maps, actions, and their bindings.
    - Verify the C# class is generated and compiles without errors.
    - (Later) Use this asset in `InputReader` to test actual input processing.

# Notes/Questions:
- The specific gamepad button for `Dash` (East or West) should be confirmed if there's a preference. For now, Button West (X/Square) is chosen as a common action button.
- The generated C# class location can be controlled. If Unity places it by default next to the `.inputactions` asset in `Settings/Input`, it's fine. If it needs to be in `Scripts/Core/Input`, ensure the "File Path" setting on the asset inspector is used or move the file manually (and update any references if Unity doesn't handle it). The plan specifies `InputReader.cs` in `_Project/Scripts/Core/Input/`, so the generated C# class related to input actions should ideally be co-located or managed clearly. Let's assume we place the generated class in `Assets/_Project/Scripts/Core/Input/Generated/` to keep it separate but organized.
- Ensure control schemes are set up if different devices need different binding sets (e.g., "Keyboard&Mouse", "Gamepad"). The above bindings can exist in a single default scheme.