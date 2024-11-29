
# Unity Word Matcher Game 🎮

A unique twist on the classic brick breaker game where players match word pairs while playing! Control a paddle, aim the ball, and hit bricks to reveal word parts. Match the correct word combinations to score points and beat the timer.


## 📺 Gameplay Demo
<!-- Add your gameplay video here -->
<details>
<summary>Click to see gameplay demo!</summary>

[![Gameplay Video](path_to_video_thumbnail.png)](https://github.com/Jahnu36/computer-graphics/blob/main/Gaming%20-%20Level1%20-%20PC%2C%20Mac%20%26%20Linux%20Standalone%20-%20Unity%202020.3.41f1%20Personal_%20_DX11_%202024-11-30%2002-46-13.mp4)
</details>

## 🎯 Game Features

- Classic paddle and ball mechanics with modern word-matching gameplay
- Intuitive aiming system with visual trajectory line
- Dynamic ball speed that increases based on:
  - Time remaining
  - Player score
- Floating bricks with word segments
- Combo scoring system with multipliers
- Word validation system using Groq API
- Time-based challenges
- Visual feedback for matched words
- Game over condition after 3 consecutive incorrect matches

## 🎮 Game Screenshots
![image](https://github.com/user-attachments/assets/b64fe1a6-6ad4-4e5e-865d-ee9745701ed0)

![image](https://github.com/user-attachments/assets/43299d87-b2bc-47d2-96fb-2f2580b94d1d)

![image](https://github.com/user-attachments/assets/2d11b914-bed0-43cd-babc-077f1d7d610e)


## 🏗️ Architecture

### System Architecture
```mermaid
graph TD
    A[Game Manager] --> B[Word Validation Manager]
    A --> C[Timer System]
    A --> D[Score Manager]
    B --> E[Groq API Integration]
    A --> F[Input Handler]
    F --> G[Paddle Controller]
    F --> H[Ball Controller]
    A --> I[Brick Manager]
    I --> J[Word Display System]
```

### Data Flow
```mermaid
sequenceDiagram
    participant Player
    participant Paddle
    participant Ball
    participant Brick
    participant GameManager
    participant WordValidator

    Player->>Paddle: Move/Aim
    Paddle->>Ball: Launch
    Ball->>Brick: Collide
    Brick->>GameManager: Reveal Word Part
    GameManager->>WordValidator: Validate Combination
    WordValidator->>GameManager: Return Result
    GameManager->>Player: Update Score/Status
```

## 🎮 How to Play

1. Control the paddle using arrow keys (← →)
2. Aim the ball using your mouse
3. Click to launch the ball
4. Hit bricks to reveal word parts
5. Match correct word combinations (e.g., UNI-TY, GA-ME)
6. Score points and achieve combos for higher multipliers
7. Complete as many words as possible before time runs out!

## 🏆 Scoring System

- Base points for matching words: 100 points
- Bonus points for consecutive matches: 20 points
- Score multiplier increases with consecutive correct matches
- Time-based bonus points at the end of the game
- Maximum multiplier: 5x

## 💻 Technical Architecture

### Component Structure
```mermaid
classDiagram
    class GameManager {
        +ValidateWord()
        +UpdateScore()
        +ManageGameState()
    }
    class Ball {
        +Launch()
        +Reset()
        +UpdateSpeed()
    }
    class Paddle {
        +Move()
        +ClampPosition()
    }
    class Brick {
        +ShowWord()
        +Float()
        +HandleCollision()
    }
    GameManager --> Ball
    GameManager --> Paddle
    GameManager --> Brick
```

## 🔧 Core Components

Paddle: Handles player movement and ball attachment
Ball: Manages ball physics, aiming, and speed adjustments
Brick: Controls word display, floating animation, and collision detection
GameManager: Handles scoring, word validation, and game state
Timer: Manages game time and time-based mechanics

## 🛠️ Technical Features

- Smooth paddle movement with screen boundary detection
- Dynamic ball physics with speed adjustment
- Word validation using Groq API integration
- Floating animation for bricks
- Timer system
- Combo tracking system
- Score multiplier mechanics
- TextMeshPro integration for clear text display

## 🔄 Game States
```mermaid
stateDiagram-v2
    [*] --> MainMenu
    MainMenu --> Playing: Start Game
    Playing --> Paused: ESC
    Paused --> Playing: Resume
    Playing --> GameOver: Time Up/3 Mistakes
    GameOver --> MainMenu: Restart
    GameOver --> [*]: Quit
```

## 📝 Prerequisites

- Unity 2020.3 or later
- TextMeshPro package
- Newtonsoft.Json package

## 🔑 API Configuration

Before running the game, make sure to:
1. Set up your Groq API key
2. Configure the validation endpoint in GameManager.cs
3. Test the word validation system

## 🎮 Controls

- **Left Arrow/Right Arrow**: Move paddle
- **Mouse**: Aim ball
- **Left Click**: Launch ball

## 🎯 Development Roadmap

### Phase 1 (Current)
- ✅ Basic gameplay mechanics
- ✅ Word matching system
- ✅ Score tracking
- ✅ Timer implementation

### Phase 2 (Planned)
- 🔲 Power-up system
- 🔲 Level progression
- 🔲 Sound effects
- 🔲 Background music

### Phase 3 (Future)
- 🔲 Online leaderboard
- 🔲 Multiple game modes
- 🔲 Achievement system
- 🔲 Social features

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.


## 📞 Contact

For any queries or suggestions, please reach out to:
- Email: reddyjahnavi36@gmail.com
