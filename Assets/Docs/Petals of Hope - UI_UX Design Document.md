# **Petals of Hope \- UI/UX Design Document**

## **1\. Introduction**

This document outlines the User Interface (UI) and User Experience (UX) design for *Petals of Hope*. The goal is to create an intuitive, accessible, and immersive interface that complements the game's pixel art style and platforming gameplay, ensuring a positive experience for the target audience (casual gamers, 10+).

## **2\. UX Principles**

* **Clarity:** All UI elements must be easily understandable at a glance. Information should be presented clearly without cluttering the screen.  
* **Responsiveness:** Player inputs must have immediate and clear feedback (visual and auditory). Menus and transitions should feel snappy.  
* **Consistency:** UI elements, terminology, and interaction patterns should be consistent throughout the game.  
* **Accessibility:** Consider options for control remapping, volume adjustments, and potentially visual aids (e.g., high-contrast mode, though pixel art might limit this).  
* **Immersion:** The UI style should match the game's 16-bit pixel art aesthetic, enhancing rather than detracting from the game world.

## **3\. UI Elements Breakdown**

### **3.1. Main Menu**

* **Layout:** Centered options list over a thematic background (e.g., pixel art of Elias looking towards the mountains, with the magical flower subtly visible).  
* **Options:**  
  * Start Game: Begins a new game or loads the last checkpoint/save. If save data exists, might change to Continue.  
  * Options: Leads to the Options sub-menu.  
  * Exit: Closes the game application.  
* **Visual Style:** Retro-style pixelated font (e.g., a clean sans-serif pixel font), bordered buttons/options, clear selection highlight (e.g., color change, bouncing arrow).  
* **Interaction:** Navigate with Arrow Keys/D-Pad, select with Enter/Action Button (e.g., Spacebar or designated controller button). Mouse support on PC (point and click).

### **3.2. Options Menu**

* **Layout:** Simple list or tabbed interface.  
* **Options:**  
  * Volume: Sliders for Master, Music, SFX volume (0-100%). Visual feedback on the slider.  
  * Controls: Display current control mapping (Keyboard, potentially Gamepad). Option to view/remap controls if feasible (especially for PC).  
  * Back: Return to Main Menu.  
* **Visual Style:** Consistent with Main Menu (pixel font, borders, highlights). Sliders are visually clear pixel bars.  
* **Interaction:** Navigate and adjust settings using Keyboard/Gamepad/Mouse. Changes are saved automatically or via a Confirm button.

### **3.3. In-Game HUD (Heads-Up Display)**

* **Layout:** Positioned minimally, likely top-left or top-right corner, to avoid obstructing gameplay view. Semi-transparent background optional for better visibility.  
* **Elements:**  
  * **Health Bar:** Represented by pixel hearts (e.g., 3-5 hearts). Damage shown by emptying hearts (or half-hearts). Consistent with retro style.  
  * **Coin Counter:** Pixelated coin icon followed by a numerical count (e.g., "COIN x 042").  
  * **Ability Icons (Optional but Recommended):** Small, distinct pixel icons appear (e.g., near health/coins or corner) when Double Jump, Wall Jump, Dash are unlocked. Provides a visual reminder of available skills. Could subtly glow when the ability is ready/usable if there were cooldowns (not applicable here, but good practice).  
* **Visual Style:** Crisp pixel art icons and numbers matching the Art Style Guide. Designed to be legible against varied backgrounds.  
* **Feedback:** Health bar updates instantly on damage/healing. Coin counter increments with clear visual/audio feedback upon collection.

### **3.4. Pause Menu**

* **Layout:** Appears as a semi-transparent overlay covering the center of the screen, pausing the game action.  
* **Options:**  
  * Resume: Unpauses the game.  
  * Restart Level: Restarts the current level from the beginning (confirmation prompt recommended: "Restart Level? All progress in this level will be lost.").  
  * Options: Access a simplified version of the Options menu (Volume adjustments primarily).  
  * Quit to Main Menu: Exits the current level and returns to the Main Menu (confirmation prompt recommended: "Quit to Main Menu? Unsaved progress in this level will be lost.").  
* **Visual Style:** Consistent pixel art style, semi-transparent background (e.g., dark pixelated overlay), clear text, selection highlight.  
* **Interaction:** Accessible via Esc key/Start button. Navigate options with Keyboard/Gamepad, select with Enter/Action Button.

### **3.5. Other UI Elements**

* **Checkpoints:** Visual indicator when a checkpoint is reached (e.g., a flag raising, a glowing symbol activating) accompanied by a sound effect.  
* **Dialogue/Narrative Text:** If any (e.g., brief intro/outro text per world), presented in a clear pixelated text box at the bottom or top of the screen.  
* **Boss Health Bar:** Appears at the top or bottom center during boss fights. Larger, more prominent than the player HUD. Pixel art style matching the boss theme.  
* **Loading Screens:** Simple screen with game title/logo, potentially a spinning coin or relevant pixel art animation, and a loading indicator (pixel bar or text). Keep loading times minimal as per Technical Design Doc.  
* **Level Start/End:** Brief text overlay (e.g., "Forest \- Level 1", "Level Complete\!").

## **4\. User Experience Considerations**

* **Controls:**  
  * **Responsiveness:** Ensure tight, responsive controls crucial for platforming. Minimal input lag.  
  * **Mapping:** Default mapping (Arrows \+ Space) is simple. Provide clear visual mapping in Options. Consider Gamepad support with standard layouts.  
  * **Mobile Adaptation:** If adapted for mobile, use on-screen virtual buttons (D-pad, Jump, Action). Ensure buttons are large enough for touch input and placed ergonomically. Allow customization of button placement/size if possible.  
* **Feedback:**  
  * **Visual:** Player flashes briefly on taking damage, screen shake for heavy impacts (optional), particles on coin collect/enemy defeat, jump dust effect.  
  * **Audio:** Distinct sounds for jump, land, damage, heal, coin collect, enemy hit/defeat, ability use, menu navigation (as per Sound Design Doc). Audio feedback reinforces actions.  
* **Onboarding:** Early levels act as tutorials, introducing mechanics gradually with safe spaces to practice. Minimal text, rely on level design to teach.  
* **Error States/Frustration:** Fair checkpoint placement mitigates frustration from death. Clear hazard indicators (e.g., sharp spikes look dangerous). Avoid unavoidable cheap shots. If the player gets stuck, Restart Level is available.

## **5\. UI Flow Example (Starting Game)**

1. **Launch Game** \-\> Shows Title Screen/Main Menu.  
2. **Select Start Game** \-\> Brief Loading Screen.  
3. **Load Level 1** \-\> "Forest \- Level 1" text overlay appears briefly.  
4. **Gameplay Starts** \-\> HUD is visible. Player controls Elias.  
5. **Player presses Esc/Start** \-\> Pause Menu overlay appears. Game pauses.  
6. **Select Resume** \-\> Overlay disappears. Game resumes instantly.  
7. **Player completes level** \-\> "Level Complete\!" overlay, transition to next level loading screen or map (if applicable).