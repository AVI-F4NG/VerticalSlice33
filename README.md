# GDIM33 Vertical Slice
## Milestone 1 Devlog
### Question 1
The CanUnlockDoor state graph inside the player’s Visual Scripting State Machine:

This graph runs only when the player is already in the state where they are standing in front of the first locked door with the key-card sequence completed. The graph starts with an Update node, which checks every frame whether the player is pressing the interaction key. That feeds into Get Key Down (E), which outputs true only on the frame when E is pressed, so the door is not unlocked continuously. Its result goes into a Branch node, which decides whether the rest of the graph should run. If the branch is true, the graph uses Get Variable (door1ClosedObj) to fetch the closed door GameObject stored in the player’s object variables, then sends that into Set Active (False), which disables the visible blocking door in the scene. After that, the graph uses Get Variable (door1TriggerObj) and another Set Active (False) to disable the interaction trigger as well, so the player cannot keep re-triggering the same unlock event. Finally, the graph uses Self plus Trigger Custom Event ("DoorUnlocked") to send out a completion signal, which is what the transition graph listens for in order to move into the next state. In short, this graph converts one input press into three concrete results: remove the closed door, remove its trigger, and notify the state machine that the unlock step is finished.

### Question 2
<img width="1853" height="1192" alt="gdim33 pitch breakdown (4)" src="https://github.com/user-attachments/assets/b20fed45-2fe1-40ad-a6df-1c7057bb4e8b" />
Changes to the breakdown: added the top graph for the state machine. The state machine has 5 states: SearchForKeycard, CanPickUpKeycard, SearchForDoor, CanUnlockDoor, and SequenceComplete. I also added the logic for transitions between states.


The state machine controls the key card pickup and first door unlock sequence on the player side. It organizes that interaction into a small set of progression states instead of relying on one long graph that is always checking every possible condition. The machine begins in a state where the player is effectively searching for the key card. When the player enters the key card’s interaction range, the state machine transitions into a state where the key card can be picked up. In that state, pressing E deactivates the key card object and sends a custom event that advances the machine to the next stage, where the player is now progressing toward the door. When the player reaches the door’s interaction range, the machine enters the unlock state, and pressing E deactivates the closed door object and its trigger. A final custom event then moves the machine into its completed state. This structure is useful because each interaction is only available at the correct moment: the player cannot unlock the door before the key card step has happened.


This state machine is related to other systems in the game because it acts as the progression control layer for this built section. It depends on the player movement system to physically move the player into the key card and door interaction ranges. It also interacts directly with the scene object system, since the state graphs activate or deactivate specific GameObjects such as the key card, the closed door, and the door trigger. In addition, it uses the input system, because the transitions inside the pickup and unlock states are triggered by pressing E. So even though the state machine is the main logic organizer for this sequence, it does not work alone: movement gets the player into range, trigger colliders detect proximity, the graph reads the input, and the affected room objects change state in response.

## Milestone 2 Devlog
Milestone 2 Devlog goes here.
## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!
