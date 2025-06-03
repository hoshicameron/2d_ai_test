# Technical Design Document for *Petals of Hope*

## Overview

This document outlines the technical requirements and architecture for *Petals of Hope*, a pixel art platformer with 40 levels. It ensures the development team has a clear understanding of the tools, technologies, and performance targets needed to bring the game to life.

## Game Engine

- **Engine**: Unity
- **Version**: 6
- **Reason**: Unity supports 2D pixel art workflows, has robust platform support, and offers a large community for troubleshooting.

## Programming Language

- **Language**: C#
- **Reason**: Native to Unity, widely used, and suitable for game logic and mechanics.

## Target Platforms

- **Primary**: PC (Windows, macOS)
- **Secondary**: Mobile (iOS, Android)
- **Resolution**: 1920x1080 (scalable for mobile)

## Performance Targets

- **Frame Rate**: 60 FPS
- **Loading Times**: Under 5 seconds per level
- **Optimization**: Pixel-perfect rendering for crisp pixel art visuals

## Special Technical Features

- **Pixel Art Rendering**: Use Unity’s Pixel Perfect Camera component to maintain sharp, unblurred visuals.
- **Physics**: 2D physics system for platforming mechanics (e.g., jumping, collisions).
- **Save System**: Local save files for progress across 40 levels.

## Tools and Software

- **IDE**: Visual Studio 2022
- **Version Control**: Git (via GitHub or similar)
- **Asset Creation**: Aseprite (for pixel art), Audacity (for audio)

## Technical Challenges

- Ensuring smooth performance on mobile devices with 40 unique levels.
- Maintaining consistent pixel art scaling across different screen resolutions.

## Next Steps

- Confirm engine and platform choices with the team.
- Set up the initial Unity project with pixel-perfect settings.