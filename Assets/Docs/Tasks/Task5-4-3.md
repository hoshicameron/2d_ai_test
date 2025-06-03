# Task ID: 5.4.3
# Parent Task ID: 5.4
# Title: Optimize CPU Usage (GC Allocations, Hot Paths)
# Status: pending
# Dependencies: 5.4.1 (Profiling results)
# Priority: high
# Estimated Effort: L (iterative)
# Assignee: Unassigned

# Description:
Analyze CPU profiling data to identify scripts and methods causing high CPU load or frequent Garbage Collector (GC) allocations. Optimize these "hot paths" by caching references, minimizing work in `Update`/`FixedUpdate`, and avoiding memory allocations in frequently called code.

# Details:
1.  **Review Profiler Data (CPU Usage & GC.Alloc):**
    *   Focus on methods with high "Self" CPU time.
    *   Look for methods that frequently show up with `GC.Alloc` markers next to them in the Profiler's Hierarchy view.
2.  **Common Optimizations:**
    *   **Caching Component References:**
        *   Instead of `GetComponent<T>()` in `Update()` or other frequent methods, call it once in `Awake()` or `Start()` and store the result in a private field. (Many existing tasks already specify this).
    *   **Avoiding `new` in Loops/Update:**
        *   If creating temporary objects (Vectors, Quaternions, arrays, lists, strings) in `Update` or loops, try to reuse existing instances or use structs where appropriate.
        *   Example: Instead of `Vector3 newPos = new Vector3(...)` every frame, if it's always the same offset from a changing value, calculate components and assign to `transform.position` directly or reuse a member `Vector3` variable.
    *   **String Operations:**
        *   Avoid string concatenation (`+`) in `Update()` or loops. Use `StringBuilder` if complex string manipulation is needed, or pre-cache strings.
        *   Avoid `Debug.Log()` calls in production builds or frequent Update methods (they cause allocations). Use conditional compilation (`#if DEBUG_MODE`) or a custom logger.
    *   **LINQ:**
        *   LINQ queries often allocate memory (e.g., for enumerators, intermediate collections). In performance-critical code, consider replacing LINQ with manual loops (e.g., `foreach`, `for`).
    *   **Physics Queries (Non-Allocating Versions):**
        *   Use non-allocating versions of physics raycasts/overlap checks where available (e.g., `Physics2D.RaycastNonAlloc()`, `Physics2D.OverlapCircleNonAlloc()`). These require pre-allocating an array for results.
    *   **Coroutines:**
        *   Avoid `yield return new WaitForSeconds(...)` inside loops if the wait time is constant; cache `WaitForSeconds` object: `private WaitForSeconds _myWait = new WaitForSeconds(1f); ... yield return _myWait;`.
    *   **Event System:**
        *   If using UnityEvents in Inspector with non-primitive arguments, they might cause allocations. For performance-critical events, consider C# events or ScriptableObject events with value type payloads or pre-allocated reference type payloads.
    *   **Optimizing Loops:**
        *   Cache `Count` or `Length` for lists/arrays if accessed multiple times in a loop condition.
        *   Consider if a loop can be broken early.
3.  **Iterate and Re-Profile:**
    *   Apply optimizations to one identified bottleneck at a time.
    *   Re-run the Profiler to confirm the optimization had the intended effect and didn't introduce new issues.

# Acceptance Criteria:
- Key CPU hot spots identified in profiling (Task 5.4.1) are addressed.
- Measurable reduction in `GC.Alloc` calls per frame in problematic areas.
- Overall CPU frame time is reduced or stabilized, especially during intense gameplay moments.
- No new functional bugs are introduced by optimizations.

# Test Strategy:
- **Targeted Profiling:** After optimizing a specific script or system, profile that scenario again to measure the improvement directly.
- **Regression Testing:** Ensure core gameplay functionality remains unaffected by code changes made for optimization.
- Compare "before" and "after" Profiler captures.

# Notes/Questions:
- "Premature optimization is the root of all evil." Focus on bottlenecks identified by the Profiler, not on optimizing code that isn't performance-critical.
- Some GC allocations are unavoidable, but the goal is to minimize frequent, large, or unnecessary ones in hot paths.
- This is highly iterative. Many small changes can add up.