# 🏫 EARIST Cavite 3D Campus Map

<div align="center">

![Unity](https://img.shields.io/badge/Unity-2022.3.32f1-black?logo=unity)
![Platform](https://img.shields.io/badge/Platform-Android-green?logo=android)
![C#](https://img.shields.io/badge/Language-C%23-purple?logo=csharp)
![License](https://img.shields.io/badge/License-MIT-blue)

**An interactive 3D campus navigation application for EARIST Cavite Campus with voice-controlled pathfinding, speech recognition, and text-to-speech capabilities.**

[Features](#-features) • [Technologies](#-technologies) • [Installation](#-installation) • [Usage](#-usage) • [Architecture](#-architecture)

</div>

---

## 📖 Overview

EARIST Cavite 3D Campus Map is a Unity-based mobile application that provides an immersive 3D virtual tour and navigation system for the Eulogio "Amang" Rodriguez Institute of Science and Technology (EARIST) Cavite Campus. The application features voice-controlled navigation, real-time pathfinding, emergency evacuation guidance, and an admin mode for campus building management.

---

## ✨ Features

### 🗣️ Voice-Controlled Navigation
- **Speech-to-Text Recognition**: Speak destination names to navigate hands-free
- **Text-to-Speech Feedback**: Audio confirmation of navigation commands and directions
- **Natural Language Processing (NLP)**: Intelligent phrase detection supporting 150+ navigation phrases including:
  - "Take me to [location]"
  - "Where is [location]"
  - "Navigate to [location]"
  - "Find the shortest path to [location]"
  - Walking-specific queries with distance and time estimates

### 🗺️ Pathfinding System
- **A* Pathfinding Algorithm**: Efficient route calculation using the A* Pathfinding Project
- **Real-time Path Updates**: Dynamic path recalculation as the player moves
- **Visual Path Rendering**: Glowing white line renderer showing the navigation path
- **Path Information Display**: Shows computation time, searched nodes, and distance in meters

### 🚨 Emergency Mode
- **One-Touch Activation**: Quick toggle for emergency evacuation mode
- **Nearest Exit Detection**: Automatically finds the closest emergency exit
- **Dynamic Path Updates**: Continuously updates the escape route as you move
- **Voice Alerts**: Audio notifications for emergency mode activation/deactivation

### 🗺️ Minimap System
- **Real-time Overhead View**: Renders a top-down view of the campus
- **Expandable Interface**: Toggle between compact and expanded minimap views
- **Camera-based Rendering**: Uses dedicated camera with render texture

### 👤 Role-Based Access
- **User Mode**: Standard navigation and exploration features
- **Admin Mode**: Protected access with login credentials for campus management
- **Role Switching**: Seamless switching between user and admin interfaces

### 🏗️ Admin Building Management (Admin Mode Only)
- **Prefab Placement System**: Place various building components:
  - Floors, Walls, Roofs
  - Stairs and Staircases
  - Balconies, Doors
  - Ground floors
- **Grid-Snapping**: Precise placement with configurable grid size
- **Object Manipulation**: Move, rotate, and delete placed objects
- **Undo System**: Stack-based undo for recent placements
- **Touch-based Controls**: Drag and position objects on mobile devices

### 🎮 Player Controls
- **First-Person View**: Immersive campus exploration
- **Joystick Controls**: Mobile-friendly movement using virtual joystick
- **Touch Camera Controls**: Look around using touch input
- **Character Controller**: Physics-based movement with gravity and ground detection

---

## 📍 Campus Locations

The application includes navigation to **60+ campus locations** including:

| Category | Locations |
|----------|-----------|
| **Academic Buildings** | Main Building, Criminology Building, Annex Building |
| **Classrooms** | Room 101-107, 203-211, 302, Annex 101-303 |
| **Laboratories** | Computer Lab, Physics Lab, Psychology Lab, Chemistry Lab, Speech Lab |
| **Administrative** | Administration Office, Registrar's Office, Graduate Office, Faculty Room |
| **Libraries** | Library Undergrad Program, Library Graduate Program |
| **Facilities** | Canteen, Covered Court, Basketball Court, Stage, Conference Room |
| **Special Areas** | MPH Room 1-3, Internet Room, School Clinic, Dragon Fruit Farm |
| **Emergency** | Fire Exit 1-5, Evacuation Spot A & B |
| **Other** | Pent House, Head Quarters, Comfort Room, Entrance |

---

## 🛠️ Technologies

### Core Framework
| Technology | Version | Purpose |
|------------|---------|---------|
| **Unity Engine** | 2022.3.32f1 (LTS) | Game engine and development platform |
| **Universal Render Pipeline** | 14.0.11 | Modern rendering pipeline |
| **C#** | .NET Standard | Primary programming language |

### Unity Packages
| Package | Version | Purpose |
|---------|---------|---------|
| Cinemachine | 2.10.0 | Camera management |
| Input System | 1.7.0 | Modern input handling |
| ProBuilder | 5.2.3 | 3D modeling and level design |
| ProGrids | 3.0.3-preview.6 | Grid-based editing |
| TextMeshPro | 3.0.6 | Advanced text rendering |
| Newtonsoft JSON | 3.2.1 | JSON serialization |
| Timeline | 1.7.6 | Animation sequencing |

### Third-Party Assets
| Asset | Purpose |
|-------|---------|
| **A* Pathfinding Project** | Advanced pathfinding and navigation |
| **Joystick Pack** | Mobile virtual joystick controls |
| **Starter Assets** | Character controller foundation |

### External APIs
| API | Provider | Purpose |
|-----|----------|---------|
| **Speech-to-Text API** | Google Cloud | Voice recognition for navigation commands |
| **Text-to-Speech API** | Google Cloud | Audio feedback and announcements |
| **Natural Language API** | Google Cloud | Entity extraction and NLP processing |

### API Configuration
```
⚠️ IMPORTANT: API keys are required for voice features to function.
Configure your Google Cloud API key in the following scripts:
- SpeechToText.cs
- VoiceAssistantOutput.cs
- NLP.cs
```

---

## 🏗️ Architecture

### Project Structure
```
Assets/
├── Scripts/
│   ├── Core Navigation
│   │   ├── LocationManager.cs      # Location registration and lookup
│   │   ├── PathRenderer.cs         # A* path calculation and visualization
│   │   ├── PathVisualizer.cs       # Line renderer path display
│   │   └── CampusGuideController.cs # Main navigation controller
│   │
│   ├── Voice System
│   │   ├── SpeechToText.cs         # Google Speech-to-Text integration
│   │   ├── VoiceAssistantOutput.cs # Google Text-to-Speech integration
│   │   ├── NLP.cs                  # Natural language processing
│   │   ├── MicrophoneButton.cs     # Voice recording control
│   │   ├── WavUtility.cs           # Audio format conversion
│   │   └── AudioClipUtility.cs     # Audio processing utilities
│   │
│   ├── Player Controls
│   │   ├── PlayerMove.cs           # Character movement with joystick
│   │   ├── CameraLook.cs           # First-person camera control
│   │   ├── FixedTouchField.cs      # Touch input handling
│   │   ├── FixedButton.cs          # UI button interactions
│   │   └── FixedJoystick.cs        # Virtual joystick (Joystick Pack)
│   │
│   ├── Admin System
│   │   ├── AdminLoginManager.cs    # Admin authentication
│   │   ├── BuildingManager.cs      # Building prefab placement
│   │   ├── RoleSwitcher.cs         # User/Admin mode switching
│   │   └── RoleSelectionManager.cs # Role selection navigation
│   │
│   ├── UI Components
│   │   ├── MiniMapSetup.cs         # Minimap camera configuration
│   │   ├── MinimapExpander.cs      # Minimap size toggle
│   │   └── GridManager.cs          # 3D grid management
│   │
│   ├── Emergency System
│   │   └── EmergencyMode.cs        # Emergency evacuation mode
│   │
│   └── Utilities
│       ├── EmailSender.cs          # OTP email functionality
│       └── OTPManager.cs           # One-time password management
│
├── Scenes/
│   ├── AdminLoginScene.unity       # Admin login interface
│   └── UserInterfaceScene.unity    # Main 3D campus scene
│
├── AstarPathfindingProject/        # A* Pathfinding library
├── Joystick Pack/                  # Mobile joystick controls
├── StarterAssets/                  # Character controller assets
└── Materials/                      # Building and environment materials
```

### System Flow
```
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐
│   Voice Input   │────▶│  Speech-to-Text  │────▶│   NLP Manager   │
│ (Microphone)    │     │  (Google Cloud)  │     │ (Entity Extract)│
└─────────────────┘     └──────────────────┘     └────────┬────────┘
                                                          │
                                                          ▼
┌─────────────────┐     ┌──────────────────┐     ┌─────────────────┐
│  Path Renderer  │◀────│ Location Manager │◀────│ Command Parser  │
│   (A* Path)     │     │  (Position Map)  │     │ (Room Matching) │
└────────┬────────┘     └──────────────────┘     └─────────────────┘
         │
         ▼
┌─────────────────┐     ┌──────────────────┐
│  Line Renderer  │────▶│  Text-to-Speech  │
│ (Visual Path)   │     │  (Voice Output)  │
└─────────────────┘     └──────────────────┘
```

---

## 📱 Installation

### Prerequisites
- Unity Hub with Unity 2022.3.32f1 LTS installed
- Android Build Support module
- Google Cloud account with enabled APIs:
  - Cloud Speech-to-Text API
  - Cloud Text-to-Speech API
  - Cloud Natural Language API

### Setup Steps

1. **Clone the Repository**
   ```bash
   git clone https://github.com/yourusername/Earist-Cavite-3D-Map.git
   ```

2. **Open in Unity**
   - Launch Unity Hub
   - Click "Open" and select the project folder
   - Wait for Unity to import all assets

3. **Configure API Keys**
   - Open `Assets/Scripts/SpeechToText.cs`
   - Replace the `apiKey` variable with your Google Cloud API key
   - Repeat for `VoiceAssistantOutput.cs` and `NLP.cs`

4. **Build for Android**
   - Go to `File > Build Settings`
   - Select Android platform and click "Switch Platform"
   - Configure Player Settings (package name, minimum API level, etc.)
   - Click "Build" or "Build and Run"

### Build Requirements
- Minimum Android API Level: 24 (Android 7.0)
- Target Android API Level: 33 (Android 13)
- Scripting Backend: IL2CPP (recommended for release)

---

## 🎮 Usage

### Basic Navigation
1. **Manual Path Finding**
   - Use the "From" and "To" input fields
   - Enter location names and tap "Find Path"
   - Follow the glowing white line to your destination

2. **Voice Navigation**
   - Tap the microphone button
   - Speak your destination (e.g., "Take me to the library")
   - Wait for voice confirmation
   - Follow the generated path

3. **Emergency Mode**
   - Tap the emergency button
   - The app will automatically find the nearest exit
   - Follow the path to safety
   - Tap again to deactivate

### Admin Features
1. Login with admin credentials
2. Toggle Build Mode to access building tools
3. Select prefab types (Floor, Wall, Stairs, etc.)
4. Tap on the ground to place objects
5. Use arrow buttons to move selected objects
6. Use rotate button for 90° rotation increments
7. Use undo/delete for corrections

---

## ⚙️ Configuration

### Location Registration
Locations are automatically registered at runtime by finding all GameObjects with the "Location" tag. To add new locations:
1. Create or select a GameObject at the desired position
2. Set its tag to "Location"
3. Name the GameObject with the location name (e.g., "Room 101")

### Emergency Exits
Emergency exits are identified by the "Emergency" tag. Add this tag to all fire exits and evacuation spots.

### Path Settings
Configure in `PathRenderer.cs`:
- Line thickness: Adjust `startWidth` and `endWidth`
- Path update frequency: Modify the coroutine wait time

---

## 🔐 Security Notes

> ⚠️ **Important**: Before deploying to production:
> 1. Move API keys to secure environment variables or a backend service
> 2. Change default admin credentials in `RoleSwitcher.cs` and `AdminLoginManager.cs`
> 3. Implement proper authentication for admin features
> 4. Enable API key restrictions in Google Cloud Console

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **EARIST Cavite Development Team**

---

## 🙏 Acknowledgments

- [A* Pathfinding Project](https://arongranberg.com/astar/) - Pathfinding solution
- [Google Cloud](https://cloud.google.com/) - Speech and Language APIs
- [Unity Technologies](https://unity.com/) - Game engine
- EARIST Cavite Campus - For the campus data and support

---

<div align="center">

**Made with ❤️ for EARIST Cavite Campus**

</div>
