# ValleyTapping Unity prototype

This folder contains the initial C# prototype source for the Unity rebuild. It is not yet a complete Unity project: create a project in Unity Hub using a currently supported Unity LTS release, then copy `Assets/` into that project.

## Setup
1. In Unity Hub, create a 2D project with a supported LTS version.
2. Copy this folder's `Assets/` directory into the new project's root.
3. Open or create a scene named `Main`.
4. Create an empty GameObject named `GameSession` and attach `Assets/Scripts/Core/GameSession.cs`.
5. Add UI Text fields and Buttons using the built-in Unity UI system, then assign them in the Inspector.
6. Connect the tap button to `GameSession.Tap` and the upgrade button to `GameSession.BuyUpgrade`.
7. Test save/load in the Editor before configuring Android and iOS builds.

The script uses `UnityEngine.UI.Text` to avoid requiring TextMeshPro for the first prototype. Replace it with TMP later if desired.

## Current scope
Tap for coins, buy a tap-power upgrade, and persist coins/upgrades in a versioned local save file. Art, pets, farm, audio, offline earnings, and online services are not implemented yet.
