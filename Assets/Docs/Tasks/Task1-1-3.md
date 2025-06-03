# Task ID: 1.1.3
# Parent Task ID: 1.1
# Title: Import Essential Unity Packages
# Status: pending
# Dependencies: 1.1.1
# Priority: critical
# Estimated Effort: S
# Assignee: Unassigned

# Description:
Import essential Unity packages required for the project, such as Input System and Cinemachine.

# Details:
1.  Open the Unity Project.
2.  Go to `Window > Package Manager`.
3.  Ensure the "Packages" filter is set to "Unity Registry".
4.  **Input System:**
    *   Search for "Input System".
    *   Select it and click "Install".
    *   If prompted to enable the backends and restart the editor, click "Yes". This will likely disable the old Input Manager.
5.  **Cinemachine:**
    *   Search for "Cinemachine".
    *   Select it and click "Install".
    *   The 2D URP Core template might already include Cinemachine. Verify if it's present. If not, install it.
6.  Verify that URP (Universal Render Pipeline) is installed and configured, as per the "2D URP Core" template selection. If not (e.g., if started from 2D Core and URP was added manually), ensure it's correctly set up in `Project Settings > Graphics` and a URP Asset is assigned.
7.  Commit changes to Git: `git add Packages/manifest.json Packages/packages-lock.json` then `git commit -m "Imported essential Unity packages: Input System, Cinemachine"`.

# Acceptance Criteria:
- The "Input System" package is installed and enabled in the project.
- The "Cinemachine" package is installed in the project.
- URP is confirmed to be active and configured for the project.
- Package changes are committed to Git.

# Test Strategy:
- Manual Verification:
    - Check `Window > Package Manager > In Project` to see "Input System" and "Cinemachine" listed as installed.
    - Verify `Project Settings > Player > Other Settings > Active Input Handling` is set to "Input System Package (New)" or "Both".
    - (Optional) Briefly test adding a Cinemachine Virtual Camera to an empty scene to ensure it's functional.
    - Check Git history for the commit.

# Notes/Questions:
- Confirm if any other packages are deemed "essential" at this stage based on the architectural design document if available.