# INFR3110 - F2020 - Final Exam

Project Template for your Final Test for INFR3110 @ Ontario Tech University
Edited for the Final Exam for Game Engines.

Exam parts are as follows:
Part 1 - Input Manager
Part 2 - Bullet Factory Method
Part 3 - Saving/Loading DLL
Bonus: AI Programmer (Level Generation)

- CONTROLS (Main) -
-- Player Actions (Keyboard/Mouse) --
W/Up Arrow - Move Forward
S/Down Arrow - Move Backward
A/Left Arrow - Move Left
D/Right Arrow - Move Right
Left Mouse Click/Hold - Fire Projectile

-- Other (Keyboard) --
I - Import to File
O - Export from File
G - Randomize Stage
R - Refresh Stage (make all blocks visible)
C - Clear Stage (delete all blocks)
B - Return to Start Menu

-- Component Coontrols (Inspector Only) --
--- Game State Loader ---
Save Data On Destroy: saves the level data upon component's destruction, such as when leaving the scene.
Delete Data on Destroy: deletes the data upon component's destruction, such as when leaving the scene.
File: file to save to or load from.

Blocks: blocks that have been loaded or will be saved.
Load As Children: load blocks up as children of the parent.
Find Blocks: search for blocks to add to the list when saving the stage.
	- It only adds objects with CubeBehaviours specifically.
	- It does NOT add the player to the list, since there should only ever be one.
Add Block Children: adds children from the Game State Loader component's GameObject.
	- if 'FindBlocks' is true, then this variable doesn't matter.

--- Level Generator ---
Block Parent: the parent for the generated and moved blocks.
Default Parent: uses the component's game object as the parent for the moved and generated blocks.
Add Parents: adds parents to spawned and moved objects.
Preserve Parents: if true, objects that already have parents keep them.

Block Options: the blocks that can be generated for the map. 
	- There are two options (BlocKA and BlockB), both of which are the same aside from their colour.
	- Each block has a 50:50 chance of being used.
Area Position: position of the block area
Area Size: the size of the area for creating the stage.
	- The maximum amount of blocks possible is areaSize.x * areaSize.y * areaSize.z.

Direc Change Chance: chance of changing direction when "craving" out map (0 - 1 range)
Spread Chance: chance of carving out adjacent blocks when craving along a path (0 - 1 range)
Cycles: the amount of cycles on iteration of the carving goes through at max.
Col Manager: the collision manager, which is used for clearing out deleted blocks and adding back new ones.

- NOTES -
* Loading and saving to a file only works in the Inspector. In other words, this functionality is not available in the EXE.
* The program is buggy with generating new stage layouts and reading in file information. So it will likely throw errors if run through the Unity Engine.
* There's a glitch where going back from the menu to the main game will cause the bullets not to spawn properly. I don't know what triggers it, but it's something I encountered.
	* I don't have time to fix the glitch, but I figured I should mention it.