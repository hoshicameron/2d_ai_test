# Petals of Hope

A 2D pixel art game developed with Unity.

## Project Structure

```
Assets/
├── _Project/
│   └── Scripts/              # All C# scripts
│       └── PetalsOfHope.Runtime.asmdef  # Main assembly definition
```

## Code Style & Conventions

### Namespace Structure

All C# scripts should use the `PetalsOfHope` root namespace, followed by the appropriate sub-namespace based on the feature area. For example:

```csharp
namespace PetalsOfHope.Core.Events
{
    public class MyEventClass { /* ... */ }
}

namespace PetalsOfHope.Gameplay.Player
{
    public class PlayerController { /* ... */ }
}
```

### Key Directories
- `_Project/Scripts/` - Contains all game scripts organized by feature area
  - `Core/` - Core systems and utilities
  - `Gameplay/` - Gameplay-related code
  - `UI/` - User interface code
  - `Data/` - Data structures and persistence

### Naming Conventions
- **Namespaces**: `PascalCase`
- **Classes**: `PascalCase`
- **Interfaces**: `IPascalCase`
- **Methods**: `PascalCase()`
- **Public Fields & Properties**: `PascalCase`
- **Private Fields**: `_camelCase`

### Assembly Definitions
- `PetalsOfHope.Runtime` - Main game assembly with root namespace `PetalsOfHope`
- Additional assemblies may be created for editor scripts and modules as needed
