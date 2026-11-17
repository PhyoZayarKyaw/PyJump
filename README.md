# PyJump

A 2D Mario-style platformer built in Unity (C#) that teaches foundational **Python programming concepts** through gameplay. Made as the practical part of my MSc dissertation at the University of Strathclyde.

<img width="667" height="536" alt="Screenshot 2026-08-02 221847" src="https://github.com/user-attachments/assets/29be0719-02a0-4178-b5ef-8d84fb98e8d0" />

---

## About the Project

Many beginners find programming hard to start with. This game tests whether a purpose-built **serious game** can help STEM students with little or no programming experience learn the basics.

Instead of reading about a concept, the player uses it to solve puzzles and get through each level.

**Concepts covered**
- Data types (int, float, string, bool)
- Conditionals
- Loops
- Lists

## Research Question

Can a purpose-built 2D platformer serious game teach foundational Python programming concepts (data types, conditionals, loops) to STEM students with little or no prior programming experience?

## Key Features

- **Teach-then-test stages:** each concept has a teaching stage followed by a testing stage.
- **Puzzle mechanics tied to concepts:**
  - Pick up value boxes and place them in typed sockets (data types)
  - Collect items in the correct order (lists)
  - Stomp enemies in a set order (nested loops)
  - A boss fight with dynamic hit requirements
- **NPC question and answer interactions** with a hint system
- **Instruction recall** so players can review what they were taught
- **Three-star achievement system** for each level
- **Two difficulty settings**
- **Platformer features:** slopes, trampolines, power-ups, enemies, flagpole level completion

## Built With

- [Unity](https://unity.com/) `[add your Unity version, e.g. 2022.3 LTS]`
- C#
- TextMeshPro

## How to Run

### Option 1: Play the build
1. Go to the **Releases** page of this repository.
2. Download the build for your system.
3. Unzip it and run the game.

### Option 2: Open in Unity
1. Install **Unity Hub** and the Unity version listed above.
2. Clone this repository:
   ```
   git clone https://github.com/yourusername/your-repo-name.git
   ```
3. In Unity Hub, click **Add**, then select the cloned folder.
4. Open the project and load the first scene from `Assets/Scenes`.
5. Press **Play**.

### Option 3: Open in Browser
1. Play directly via browser using this link: https://zolarcopus.itch.io/pyjump

## Controls

| Action | Key |
|---|---|
| Move | A / D or Arrow keys |
| Jump | Space |
| Interact / Pick up | Tab |

## Evaluation

The game was tested with **5 participants** using a within-subject **pre-test / post-test** design. Participants also completed a feedback questionnaire about engagement, perceived learning, and usability.

**Main findings**
- A learning gain was present, but it was modest.
- The result was limited by a ceiling effect, because participants had more programming experience than intended.
- Player movement was the clearest area for improvement.

## Future Work

- Improve player movement and controls
- Test with a larger group of true beginners
- Add more concepts and levels

## Acknowledgements

- The game architecture was adapted from an existing open-source Unity Mario implementation https://github.com/zigurous/unity-super-mario-tutorial, then extended with concept-specific puzzle, NPC, and progression systems.
- Thanks to my dissertation supervisor and the participants who tested the game.

## Author

**Phyo Zayar Kyaw**
MSc Advanced Computer Science with AI, University of Strathclyde

- GitHub: PhyoZayarKyaw(https://github.com/PhyoZayarKyaw)
- LinkedIn: https://www.linkedin.com/in/phyo-zayar-kyaw-047b5332a/
- Email: phyozayarkyaw2018@gmail.com
