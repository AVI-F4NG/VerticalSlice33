# GDIM33 Vertical Slice
## Milestone 1 Devlog
### Question 1
The CanUnlockDoor state graph inside the player’s Visual Scripting State Machine:

This graph runs only when the player is already in the state where they are standing in front of the first locked door with the key-card sequence completed. The graph starts with an Update node, which checks every frame whether the player is pressing the interaction key. That feeds into Get Key Down (E), which outputs true only on the frame when E is pressed, so the door is not unlocked continuously. Its result goes into a Branch node, which decides whether the rest of the graph should run. If the branch is true, the graph uses Get Variable (door1ClosedObj) to fetch the closed door GameObject stored in the player’s object variables, then sends that into Set Active (False), which disables the visible blocking door in the scene. After that, the graph uses Get Variable (door1TriggerObj) and another Set Active (False) to disable the interaction trigger as well, so the player cannot keep re-triggering the same unlock event. Finally, the graph uses Self plus Trigger Custom Event ("DoorUnlocked") to send out a completion signal, which is what the transition graph listens for in order to move into the next state. In short, this graph converts one input press into three concrete results: remove the closed door, remove its trigger, and notify the state machine that the unlock step is finished.

### Question 2
<img width="1853" height="1192" alt="gdim33 pitch breakdown (4)" src="https://github.com/user-attachments/assets/b20fed45-2fe1-40ad-a6df-1c7057bb4e8b" />
Changes to the breakdown: added the top graph for the state machine. The state machine has 5 states: SearchForKeycard, CanPickUpKeycard, SearchForDoor, CanUnlockDoor, and SequenceComplete. I also added the logic for transitions between states. 

The state machine controls the key card pickup and first door unlock sequence on the player side. It organizes that interaction into a small set of progression states instead of relying on one long graph that is always checking every possible condition. The machine begins in a state where the player is effectively searching for the key card. When the player enters the key card’s interaction range, the state machine transitions into a state where the key card can be picked up. In that state, pressing E deactivates the key card object and sends a custom event that advances the machine to the next stage, where the player is now progressing toward the door. When the player reaches the door’s interaction range, the machine enters the unlock state, and pressing E deactivates the closed door object and its trigger. A final custom event then moves the machine into its completed state. This structure is useful because each interaction is only available at the correct moment: the player cannot unlock the door before the key card step has happened. This state machine is related to other systems in the game because it acts as the progression control layer for this built section. It depends on the player movement system to physically move the player into the key card and door interaction ranges. It also interacts directly with the scene object system, since the state graphs activate or deactivate specific GameObjects such as the key card, the closed door, and the door trigger. In addition, it uses the input system, because the transitions inside the pickup and unlock states are triggered by pressing E. So even though the state machine is the main logic organizer for this sequence, it does not work alone: movement gets the player into range, trigger colliders detect proximity, the graph reads the input, and the affected room objects change state in response.

## Milestone 2 Devlog

### Question 1
This complicating gameplay feature combines the Room 2 progression logic with the cross-room monitor flash puzzle. In Room 2, the player reveals the hidden USB behind the wall picture, picks it up, and receives a progress update through the HUD. That item then changes the behavior of the computer back in Room 1: instead of showing its normal screen, it opens the virus prompt, and once the player continues, it triggers the repeating monitor flash sequence that provides the clue for the Room 2 password door. This makes the feature more complex than a normal pickup or door interaction because it connects multiple rooms, multiple UI systems, progression flags, and a puzzle clue that must be observed in one place and used in another.

#### 1. Build the Room 2 item-reveal and USB pickup flow

1. Create the wall picture interaction so pressing E near it shifts the picture and reveals the USB.
2. Add the USB pickup interaction so pressing E near the USB collects it and disables the USB object.
3. Store the pickup result in a progression flag so the game remembers the USB has been found.
4. Show a HUD update after pickup by displaying the USB icon and a short message.
5. Make Room 2 re-check that progression flag when the scene loads so the USB stays gone after it has already been collected.

#### 2. Connect the USB to the Room 1 computer and monitor sequence

1. Make the computer interaction open its normal screen before the USB is found.
2. Change the computer’s behavior so that after the USB is found, opening it shows the virus prompt instead.
3. Add the two virus buttons so Eject Disk first changes into "CONTINUE", while either button eventually starts the same sequence.
4. Trigger the wild-color flash effect, then automatically close the computer screen.
5. Start the repeating monitor flash sequence after the screen closes, and keep it running as the clue source for the password puzzle.

#### 3. Use the monitor clue to drive the Room 2 password progression

1. Build the Door A keypad screen with digit buttons and an entry text display.
2. Make the keypad automatically evaluate once 4 digits are entered.
3. Show "SUCCESS" on a correct code and "FAILURE" on an incorrect one, then clear the entry after a short delay if wrong.
4. On success, unlock Door A and update its blocked/open state.
5. Let the player interact with Door A again to proceed into Room 3, completing the Room 2 + monitor-flash progression chain.

### Question 2

Yes, the task breakdowns were helpful because they turned a complicated feature into smaller parts that could be built and tested one at a time. For this milestone, the Room 2 plus monitor-flash system involved several connected pieces -- revealing the USB, updating the HUD, changing the computer’s behavior, triggering the monitor sequence, and then using that clue for the password door -- so writing the work in steps made it easier to focus on one part at a time instead of trying to finish the whole chain at once. It also helped identify where a problem was coming from, because each step had a more specific expected result.

At the same time, I would improve my breakdowns by making them even more concrete about dependencies between systems and how scene changes affect references and state. Some of the hardest issues came from interactions between UI, object references, and scene loading, not just from the feature logic itself. If I were to do the breakdown again, I would be more explicit about which parts should stay scene-local, which values should persist only as data, and which test should happen immediately after each setup step. That would make the breakdown more useful not just for building the feature, but also for avoiding architecture problems earlier.

### Question 3

I did not use a C# method to fire a custom event into a Graph, or a Graph to call back into C# for the Room 2 and monitor flash sequence. Instead, I kept using the existing Visual Scripting state machine for the earlier Room 1 key card and door flow, and used C# scripts for the newer multi-room systems. The main C# scripts involved are ProximityInteractUI2D, ComputerMonitorUI, USBPickup, LocalHUD, and GameSession. Those scripts communicate through shared scene objects and progression flags rather than custom-event calls, so C# controls the room-specific UI, item pickup, keypad logic, and scene transitions, while the older Visual Scripting graph still controls the state-based interaction sequence that was already built.

This serves an architectural purpose: it keeps the state-machine logic in Visual Scripting where it was already working, but moves the more complicated cross-room systems into C#, where it is easier to store flags like USBFound, doorAUnlocked, and hasKeyCard2, and re-apply them when scenes reload. In other words, the bridge is not an event bridge so much as a responsibility split: Visual Scripting handles the contained player-state flow, while C# handles reusable gameplay systems, UI behavior, and progression data.

### Question 4

The Unity system I used is the tilemap system. It is used to display the environment of the game (floor, walls), to make the game aesthetically appealing and also more manageable because it is more convenient to paint the tiles than manually place everything in the scenes. A second layer of tilemap is added to represent items in the room that have colliders and the player cannot pass through these objects (the monitor tables in room 1, the walls around the rooms to prevent the player from going out of the map, etc.)

## Milestone 3 Devlog

### Question 1

The Shader Graph creates an animated CRT glitch by using the UV node as the sprite texture coordinates, sending it into a Split node, using Split G / UV.y to generate horizontal bands, multiplying that value by _GlitchBands, passing it through Floor to make discrete band IDs, combining that with animated shader time from the Time node, multiplying Time by _GlitchSpeed, adding it to the band ID, passing the result through Sine, and multiplying by _GlitchStrength to produce a horizontal glitch offset; that offset is packed with Combine as (X = glitch offset, Y = 0) and added to the original UV with an Add node to create GlitchedUV, which is then used by three Sample Texture 2D nodes because Sample Texture 2D can sample a texture using supplied UV coordinates. The graph creates RGB splitting by sampling the same _MainTex three times with GlitchedUV + RGBOffset for red, GlitchedUV for green, and GlitchedUV - RGBOffset for blue, then uses three Split nodes and one Combine node to rebuild the final color from RedSample.R, GreenSample.G, and BlueSample.B. It adds scanlines by taking UV.y, multiplying by _ScanlineCount, passing through Sine, normalizing with Add and Multiply, then using One Minus and Lerp with _ScanlineIntensity to create a brightness multiplier that is multiplied into the RGB-split color. Finally, it adds flicker with another Time - Multiply - Sine - Add - Multiply chain, uses One Minus, Add, and Lerp with _FlickerIntensity to create a whole-screen brightness multiplier, multiplies that into the scanned color, then connect the result to Base Color so the effect changes the fragment color.

*Entire Graph*

<img width="3814" height="1541" alt="Screenshot 2026-05-24 182814" src="https://github.com/user-attachments/assets/f90eee65-69e2-4d31-9021-bec9a8726bbd" />

*First Half*

<img width="3394" height="1508" alt="Screenshot 2026-05-24 183052" src="https://github.com/user-attachments/assets/d8294418-39e2-4a23-b35f-cf8097a8ebf0" />

*Second Half*

<img width="2797" height="1809" alt="Screenshot 2026-05-24 183116" src="https://github.com/user-attachments/assets/288458f1-591c-49be-b2f5-63a32be7f6b7" />

*Blackboard Variables*

<img width="456" height="929" alt="Screenshot 2026-05-24 185120" src="https://github.com/user-attachments/assets/de07a554-b3dc-42d7-8c30-4648d4a79ecf" />

### Question 2
I have addressed these issues: 
- **The player is moving too slow:** adjusted movement speed factor from 3 to 5
- **The different door controls are confusing:** made all doors enter-able upon collision instead of requiring the E key
- **The password entry screen should disappear automatically after a successful entry:** implemented auto-disable after success
- **The player teleports to outside the map if the player is on a location in room3 that correponds to a point that is outside the wall in room2, causing the player to be permanently stuck and can only get unstuck by restarting the game:** added fixed-location teleporters so the player could only go back to room2 by using the teleporter that is placed on the correct spot, preventing them to teleport to spaces outside the wall
- **The character glitches when touching the walls:** fixed; added detection for 2+ keys pressed and collision, and added a cooldown between animation clip changes if these states are detected
- **Cannot delete digit if entered the wrong digit halfway:** Added "delete" and "quit" buttons to the password entry screen
- **"Strange painting" is ambiguous:** Added a prompt to the "strange picture" whenever the player is close to it, so the player will know that it's interactable.

### Question 3
Finished the maze at the final room (room3) and moved the final key card to the end of the room, and created teleporters for the ease of travelling back to the origin point (and back to room2), and deleted any placeholder "esc to quit" logic. I tried the idea in the original pitch but it's difficult to create colliders detecting if the player stepped on a directional path without blocking their way; also it's hard for the player to tell if they've stepped on a directional path or not, so I changed over to a maze with nodes and paths.

## Final Devlog
### Question 1
The core gameplay loop is built around exploring rooms, investigating suspicious objects, collecting key items, and using those discoveries to unlock new spaces. The player moves through connected rooms, presses E to interact with clue objects and systems, picks up important items like key cards and the USB, and uses environmental information to solve progression puzzles. The main content in the current game includes the office room, lounge, and maintenance room, along with the computer clue chain, flower vase key card discovery, hidden USB reveal behind the picture, virus-triggered monitor flash sequence, keypad password door, maze, and the final stairway escape.

This matches the original Vertical Slice plan because it gives the player a small but complete version of the intended full-game experience. Instead of trying to include every possible feature, the current build demonstrates the main identity of the game: a creepy futuristic facility where rooms are interconnected, clues carry across spaces, and progression comes from observation, system interaction, and cross-room puzzle chaining rather than isolated mini-games. In that sense, the implemented gameplay shows what the full game would feel like on a larger scale: exploring unsettling industrial rooms, uncovering hidden logic in the environment, and escaping by understanding how the facility’s systems connect to each other.

### Question 2
The rendering effect is activated directly from the computer’s gameplay state in ComputerMonitorUI.cs. When the player clicks Continue on the virus prompt, OnContinuePressed() starts the ContinueRoutine() coroutine; that routine sets progression flags (GameSession.computerLockedAfterUSBUse and GameSession.monitorSequenceStarted), runs the temporary full-screen flash effect through WildFlashRoutine(), closes the UI, and then calls ApplyPostVirusScreenVisual(). That method is the point where the world-space monitor rendering actually changes: it targets a SpriteRenderer, swaps its material to changedScreenMaterial (the custom Shader Graph-driven material), swaps its sprite to changedScreenSprite, and computes a corrected localScale from the original and replacement sprite bounds so the screen keeps the same apparent size after the sprite change. The script also caches the original sprite, material, and scale in Awake() through CacheOriginalVisualState(), and in Start() it checks GameSession.monitorSequenceStarted so the post-virus material/sprite state is reapplied automatically if the player returns later, rather than only appearing during the initial trigger. 

### Question 3
My process is to start by identifying the core player loop. I first define what the player is doing most often -- moving, interacting, picking up items -- and treat those as the game’s core mechanics. Then I break the project into major systems, such as player control, interaction, UI, puzzle logic, progression tracking, scene flow, and audio/visual feedback. After that, I break each system into smaller implementable parts: specific mechanics, then specific scripts/classes, then the methods and data each class is responsible for, plus how those classes communicate. In practice, this means moving from a high-level design idea to concrete pieces like controllers, managers, triggers, UI handlers, and helper methods.

One important part of this process is that it helps reveal the real scope of the project. At the top-level design stage, a mechanic can sound simple, but once it is broken into systems and then into classes and methods, extra supporting functions usually appear -- things like state checks, helper methods, transitions, references, feedback systems, and edge-case handling. That is useful because it makes the hidden workload visible early. My current plan, then, is to keep using the breakdown-bubble map as both a design tool and a scope-checking tool: define the core mechanics, separate them into systems, reduce those systems into concrete technical tasks, and then continuously playtest on the intended platform instead of relying only on the game engine view, since that is the real player experience.

#### Question 3.1
I think bubble break-downs are more helpful, because they allow me to see the class/method/variable relationships more clearly at an early stage -- it helps a lot with understanding the overall requirements for the intended game mechanic part.

#### Question 3.2
It usually makes the project feel larger, because once I break a big feature into smaller steps, I start seeing the hidden support work -- extra systems, helper methods, edge cases, and dependencies -- that were not obvious at the top level. That gives me a more realistic understanding of the actual scope.

#### Question 3.3
It relates closely to how I built the Vertical Slice, because the project worked best when I treated it as a set of smaller connected systems instead of one large “escape room game” idea. Breaking the game down into player movement, interaction, UI, item progression, computer behavior, and room transitions made it easier to build and test each part separately. At the same time, the process also showed me where my planning was weaker: some features sounded simple at first, but once I implemented them, I found extra dependencies between scenes, UI, and progression logic that made the scope larger than expected. That is something I would improve in the future by doing an even more detailed technical breakdown earlier, especially for systems that connect multiple rooms or multiple mechanics.


## Open-source assets
### Environment
- Decorations / computer screens: [computer gibber](https://opengameart.org/content/computer-gibber-inspection-panels-stairs-transition-elements)
- Environment: [futuristic industrial tileset](https://opengameart.org/content/futuristic-industrial-technical-tileset)
- Environment / decorations: [industrial tiles](https://opengameart.org/content/industrial-tiles)

### Sound Effects
- [Office Sound Effect](https://pixabay.com/sound-effects/film-special-effects-generator-large-01-31429/) by <a href="https://pixabay.com/users/freesound_community-46691455/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=31429">freesound_community</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=31429">Pixabay</a>
- [Lounge Sound Effect](https://pixabay.com/sound-effects/film-special-effects-ventilation-ambience-sound-362606/) by <a href="https://pixabay.com/users/alex_jauk-16800354/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=362606">Alexander Jauk</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=362606">Pixabay</a>
- [Maintenance Room Sound Effect](https://pixabay.com/sound-effects/technology-industrial-machine-cycle-73890/) by <a href="https://pixabay.com/users/freesound_community-46691455/?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=73890">freesound_community</a> from <a href="https://pixabay.com/sound-effects//?utm_source=link-attribution&utm_medium=referral&utm_campaign=music&utm_content=73890">Pixabay</a>
- [Computer booting SFX](https://pixabay.com/sound-effects/household-switch-click-and-beep-001a-11602/)
- [Interaction click](https://pixabay.com/sound-effects/film-special-effects-click-345983/)
- [Glitch SFX](https://pixabay.com/sound-effects/film-special-effects-glitchy-sound-374839/)
- [Footsteps](https://pixabay.com/sound-effects/film-special-effects-footsteps-male-362053/)
- [Error SFX](https://pixabay.com/sound-effects/film-special-effects-ui-error-pop-515668/)
- [Success SFX](https://pixabay.com/sound-effects/film-special-effects-success2-57646/)
