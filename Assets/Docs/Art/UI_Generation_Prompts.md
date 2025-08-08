# UI/UX Design Generation Prompts for "Petals of Hope" (JSON Format)

This document contains prompts in a JSON-like structure for an AI image generator to create UI/UX designs for the 2D pixel art platformer, "Petals of Hope."

## General Style Guidelines

- **Theme**: Fantasy, Hopeful, Adventure
- **Art Style**: Vibrant 16-bit pixel art, reminiscent of classic SNES games.
- **Color Palette**: Use distinct palettes for each environment (Forest: greens/browns; Desert: oranges/yellows; Mountain: blues/whites; Cave: purples/neons).
- **Font**: Use a clean, readable, blocky pixel art font.
- **Consistency**: All UI elements should share a consistent design language (e.g., borders, button styles, icons).

---

```json
{
  "ui_prompts": [
    {
      "screen": "SplashScreen",
      "prompt": "Generate a splash screen for a 2D pixel art platformer called 'Petals of Hope.' The screen should feature the game's logo prominently in the center. Below the logo, include a simple, horizontal loading bar that is partially filled. The background should be a dark, atmospheric pixel art image hinting at a magical forest at night. The overall mood is mysterious but hopeful."
    },
    {
      "screen": "MainScreen",
      "prompt": "Generate a main screen for a 2D pixel art platformer called 'Petals of Hope.' The screen should feature the game's title logo at the top. In the center, display a vertical menu with the following options: 'New Game,' 'Load Game,' 'Options,' and 'Quit.' The 'Load Game' button should appear slightly grayed out to indicate it's disabled. The background should be a beautiful, vibrant pixel art illustration of a hero looking out over a lush forest valley towards a distant, glowing mountain."
    },
    {
      "screen": "HUD",
      "prompt": "Generate a UI design for the in-game HUD of a 2D pixel art platformer. The design should be clean and minimalistic, placed in the corners of the screen to not obstruct the gameplay view. In the top-left corner, display the player's health as a row of three pixel art hearts. In the top-right corner, display a small pixel art talisman icon next to a text counter that reads 'Talismans: 0'. The UI elements should have a subtle, dark border to make them readable against various backgrounds."
    },
    {
      "screen": "PauseScreen",
      "prompt": "Generate a pause screen for a 2D pixel art platformer. The screen should be a semi-transparent dark overlay that sits on top of the paused gameplay. In the center, display a clean, bordered panel with the title 'Paused.' Below the title, list the following menu options vertically: 'Resume,' 'Options,' and 'Return to Main Menu.' The design should be simple, clear, and not overly cluttered."
    },
    {
      "screen": "OptionsScreen",
      "prompt": "Generate an options screen for a 2D pixel art platformer. The screen should have a title that reads 'Options.' Below the title, include the following elements: a 'Master Volume' section with a text label and a horizontal pixel art slider; a 'Music Volume' section with a text label and a horizontal pixel art slider; a 'SFX Volume' section with a text label and a horizontal pixel art slider. At the bottom of the screen, include a 'Back' button. The entire screen should be enclosed in a decorative, 16-bit style border."
    }
  ]
}
