AIzaSyA782Xv7h7S9ygXuEEaFKoWySwCHxq5eD8
# Petals of Hope - Progress Tracker

This document tracks the implementation status of major features across different development phases.

Mark tasks as:
- `[ ]` To Do
- `[/]` In Progress
- `[x]` Done

---

## Phase 1: Foundation Systems

- [ ] **Project Setup:** Create and configure the Unity project, set up version control, and establish a standard folder structure.
- [x] **Event Bus System:** Implement a decoupled communication system using ScriptableObjects.
- [x] **ScriptableObject Data Management:** Implement a system for managing game data using ScriptableObjects.
- [x] **Save/Load System:** Implement a modular system for saving and loading game state.

---

## Phase 2: Core Gameplay Systems

- [x] **Input System:** Implement a robust input system using Unity's Input System package.
- [x] **Animation Controller (Reusable):** Create a reusable component to manage Animator parameters.
- [x] **State Machine System:** Implement a reusable finite state machine (FSM) system.
- [x] **Player Controller:** Implement the core `PlayerController` and basic movement states.
- [x] **Camera System:** Implement camera follow behavior (Cinemachine recommended).

---

## Phase 3: Enemy Systems

- [x] **Enemy Base Classes:** Create a reusable base class for all enemies.
- [x] **AI System:** Design and implement a modular AI system (e.g., State Machine).
- [x] **Basic Enemy Types:** Create initial concrete enemy implementations.

---

## Phase 4: Level Design & Game Progression

- [x] **Level Design Tools & Workflow:** Establish efficient tools and workflows for creating levels (Tilemaps, Prefabs).
- [x] **Talisman Award System:** Implement the system for awarding Talismans programmatically.
- [x] **Game Progression System:** Implement logic to control game flow based on collected Talismans.

---

## Phase 5: Polish & Refinement

- [x] **UI Systems:** Implement user interface elements (Menus, HUD).
- [x] **Audio Systems:** Implement a system for managing SFX and BGM.
- [x] **Visual Effects (VFX):** Integrate particle effects and other visual enhancements.
- [x] **Performance Optimization:** Profile and optimize the game.
