AI Behavior Tree Documentation
Welcome to the AI Behavior Tree system! This guide will walk you through the architecture, concepts, and steps required to create a wide variety of AI opponents for the game.

1. Core Concepts
   Our system is built on a few key components that work together. Understanding their roles is key to using the system effectively.

Behavior Tree (ScriptableObject): This is the master "blueprint" for an AI's brain. It's an asset that holds a reference to the top-level (root) node of a decision-making tree. You can have one of these for each AI archetype (e.g., Goblin_BehaviorTree, Archer_BehaviorTree).

Node (ScriptableObject): Nodes are the individual building blocks of the tree. Every logical step, from making a decision to performing an action, is a Node asset. This allows you to build complex logic by simply connecting these small, reusable pieces.

Data SOs (ScriptableObject): To keep our nodes clean and reusable, all configuration values (like detection ranges, cooldowns, or timings) are stored in separate data assets (e.g., PatrolDataSO, AttackDataSO). This allows you to tweak an AI's "personality" by changing its data, without ever touching its core logic tree.

BehaviorTreeRunner (Component): This is the engine. It's a MonoBehaviour that you attach to an enemy GameObject. Its job is to:

Hold references to the BehaviorTree and Data assets.

Create a unique, private copy of the tree at runtime.

"Tick" the tree every frame, causing the AI to think and act.

2. Types of Nodes
   There are three main categories of nodes you will work with.

Composites (The Brain's Logic)
These nodes control the flow of execution. They have one or more children.

Selector (?): The "OR" gate. It tries each of its children in order, from top to bottom. The first child that returns Success or Running wins, and the Selector immediately stops. It only fails if all its children fail. Use this to create a priority list.

Sequence (=>): The "AND" gate. It runs each of its children in order. If any child fails, the entire Sequence fails immediately. It only succeeds if all of its children succeed. Use this to create a required series of steps.

Decorators (The Modifiers)
These nodes have only one child and modify its behavior.

Inverter: Flips the result of its child. Success becomes Failure, and Failure becomes Success. This is how you create "if-not" logic.

Leaf Nodes (The Senses and Muscles)
These are the nodes at the end of the branches that do the actual work. They have no children.

Condition Nodes: The AI's senses. They check something about the game world and immediately return Success or Failure. (e.g., IsPlayerVisible?, IsAtLedge?).

Action Nodes: The AI's muscles. They perform an action. They can return Success if the action is instant, or Running if it takes time. (e.g., Patrol, Chase, Attack).

3. How to Create a New AI
   Follow these steps to configure a new AI from scratch.

Step 1: Configure the Enemy Prefab
Select your enemy GameObject or prefab.

Ensure it has the CharacterControllerBase and an AIInputSource component.

Add the BehaviorTreeRunner component.

Step 2: Create and Assign Data Assets
In your project folder (e.g., Assets/AI/Data), right-click and go to Create > AI > Data.

Create one of each required data asset: AttackDataSO, ChaseDataSO, IdleDataSO, and PatrolDataSO. Give them descriptive names (e.g., Goblin_AttackData).

Select each data asset and configure its values in the Inspector (e.g., set attack range, detection radius, etc.).

Drag these four data assets into their corresponding slots on the enemy's BehaviorTreeRunner component.

Step 3: Create and Assemble the Behavior Tree
This is where you build the AI's brain.

Create a folder for your new tree (e.g., Assets/AI/Trees/Goblin).

Inside, create all the required Node assets using the Create > AI > Behavior Tree > Nodes menu. You will need to create Selector, Sequence, Inverter, and all the Action and Condition nodes for your desired logic.

Connect the nodes by selecting a parent node (like a Sequence) and dragging its child nodes into the "Children" list in the Inspector. Work from the small branches up to the main root.

Create a final BehaviorTree asset (Create > AI > Behavior Tree > Behavior Tree). Name it (e.g., Goblin_BehaviorTree).

Select this BehaviorTree asset and drag your root node (usually your master Selector) into its "Root Node" slot.

Finally, drag this BehaviorTree asset into the "Tree Asset" slot on the enemy's BehaviorTreeRunner component.

4. Examples
   By re-arranging the same node building blocks, you can create vastly different AI personalities.

Example 1: The Standard Melee Guard (Our Current AI)
This AI prioritizes attacking, then chasing, and falls back to a complex patrol/idle routine. This is a great template for a standard "goon" enemy.

graph TD
A("? Master Selector") --> B["=> Attack Logic"];
A --> C["=> Chase Logic"];
A --> D("? Patrol & Idle Logic");

    subgraph Patrol & Idle Logic
        direction TB
        D --> E["=> Initial Spawn"];
        D --> F["=> Handle Obstacle"];
        D --> G["! Patrol"];
    end

Example 2: The "Timid Ranged" AI
This AI has a different priority: its goal is to maintain a safe distance. It uses the exact same nodes, just arranged differently.

Logic:

If the player is too close, its highest priority is to flee.

If the player is at a perfect range, it will attack.

If the player is visible but too far away, it will chase.

If the player is not visible, it will patrol.

Tree Structure:

graph TD
A("? Ranged AI Brain") --> B["=> Flee Sequence"];
A --> C["=> Attack Sequence"];
A --> D["=> Chase Sequence"];
A --> E("? Patrol Logic");

    subgraph Flee Sequence
        B --> B1{"Is Player Too Close?"};
        B --> B2["! Flee (Move Away)"];
    end

To build this, you would need to create two new nodes: a condition IsPlayerTooClose? and an action Flee. This demonstrates the power of the system: create a couple of new leaf nodes, and you can build entirely new behaviors.

Example 3: The Stationary Turret
This is the simplest AI. It doesn't move at all. Its tree only has one branch.

Logic:

If the player is visible and in range, attack.

Otherwise, do nothing.

Tree Structure:

graph TD
A("? Turret Brain") --> B["=> Attack Sequence"];
A --> C["! Idle"];

    subgraph Attack Sequence
        B --> B1{"Is Player Visible?"};
        B --> B2{"Is Player In Attack Range?"};
        B --> B3["! Attack"];
    end

This shows that you only need to build the logic you need. A simple AI will have a simple tree.