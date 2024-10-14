# RedBackBots GameSight - AR RoboCup SPL Visualiser

This is the official 2024 RedbackBots release of GameSight. The 2024 release is published under the coderelease2024 tag on this repository. This software is provided 'as is', and the team does not provide public support of this software. You can find the RedbackBots main code release on [public code release repository](1).

RedbackBots is a undergraduate and postgraduate student team supported by the AI Innovation Lab in the School of Computing Technologies at RMIT University.
The goals of RedbackBots are:

1. A research project of the AI Innovation Lab, and
2. To provide education for RMIT students in applications of AI to autonomous robotics systems. 

You may contact the RedbackBots team at: <stem.redbackbots@rmit.edu.au>

## RedbackBots Team Report

Our 2024 RedbackBots team report is available on our [public code release repository](1).

## License

Before cloning this repository, you must read our [license terms](License.md).

# Overview

Welcome to GameSight, the AR RoboCup SPL Visualiser!

This project is designed for Meta Quest 2/3 devices and provides an augmented reality experience to visualize RoboCup SPL soccer games in real-time.

This application can be installed and run directly on your Meta Quest headset or compiled from the source code for custom modifications. It uses the Unity engine and integrates with RoboCup SPL [GameController](https://github.com/RoboCup-SPL/GameController3).

## Features

- **Robot Localisation**: View overlays of assumed players' positions according to what they are sending back to the GameController.
- **Robot-Ball Visualisations**: See representations of where the players are seeing balls.
- **Game State information**: View true game state information received from the GameController.
- **Robot Return Data**: View full player information sent back in the RoboCupGameControlReturnData packets.

## Requirements

- **Device**: Meta Quest 2/3 (with Touch Controllers).
- **Software**: Unity 2021 or later (for compilation).
- **Android SDK**: Required for building the .apk.

## Installation

### 1. Download from App Lab (Recommended)

While we are waiting on approval from Meta to publish GameSight in the App Lab publicly, email us at stem.redbackbots@rmit.edu.au and we will add you as a "beta tester":

1. Click **Download** and install the application on your Meta Quest headset
2. Once installed, find the application in your Quest library under "Apps"

### 2. Install from Pre-built APK (Release Version)

If you prefer to manually install the app:

1. Download the latest `.apk` file from our [Release Page](https://github.com/rmit-computing-technologies/redbackbots-gamesight-coderelease/releases/tag/coderelease2024).
2. Connect your Meta Quest headset to your computer using a USB-C cable
3. Enable Developer Mode on your Quest device:
   - Open the Meta Quest app on your phone
   - Go to **Settings** > **Developer Mode** and toggle it on
4. Use **SideQuest** or **adb** to install the `.apk`:
   - With SideQuest: Click **Install APK** and select the downloaded `.apk` file
   - With adb: Open a terminal and run:

     ``` bash
     adb install path/to/gamesight.apk
     ```

### 3. Compile and Build from Source (For Developers)

To modify the app or compile it yourself, follow these steps.

#### Prerequisites

- **Unity 2021 or later** installed on your machine
- **Android SDK**: Ensure that the Android SDK is set up in your Unity preferences
- **Oculus Integration SDK**: This is required for Quest-specific features

#### Steps to Compile

1. Clone the repository:

   ```bash
   git clone https://github.com/rmit-computing-technologies/redbackbots-gamesight-coderelease.git
   ```

2. Open the project in Unity:
From Unity Hub, select "Open Project" and navigate to the cloned folder

3. Set the platform to Android:
Go to File > Build Settings
Select Android and click Switch Platform

4. Configure for Quest:
Open XR Plugin Management from Project Settings
Enable Oculus under Android settings

5. Build the APK:
In Build Settings, click Build
Select a location to save the .apk file
After the build completes, you can install it on your device using the steps in the pre-built APK section above

## Running the Application

Once installed, follow these steps to use the application:

1. Complete room scanning ensuring that the field is scanned (at the very least the centre circle and centre line in additon to the area you will be standing)
2. Launch the app from your Quest library
3. Grant Permissions: The first time you run the app, it will request camera and motion tracking permissions and access to spatial data.
4. You can check if GameSight has successfully connected to a GameController by viewing the floating message
5. Place the field by first pointing your left controller (and corresponding laser) at the centre of the centre circle, press the left trigger once to start placing the field
6. Point the controller at the intersection of the centre and edge field lines (making sure the team logos are on the correct sides of the field) and press the left trigger again to finish placing the field
7. Start receiving true GameController data by pushing the y button on the left controller

### Further Controls

- The A and X buttons will cycle between different information card attached to each controller
- You can replace the field at any time following the same instructions as above
- To stop receving true GameController data you can push the Y button again

### Troubleshooting

- The most common error is when GameSight does not correctly recieve Spatial Data access. When this happens the laser pointers from the controllers will not stop when they hit the ground. To fix this, close the app, remove spatial data permission and relaunch
- Another common error is when no lasers are visibible coming out of the controller, often this can be fixed by leaving and re-entering the guardian boundary

## Support

For any issues or feature requests, feel free to contact us through our GitHub issues page or through the official RoboCup SPL discord or email the RedbackBots team at <stem.redbackbots@rmit.edu.au>.

----

Copyright (c) 2024.

Artificial Intelligence Innovation Lab, Centre for Industrial AI and Research Innovation (CIAIRI), School of Computing Technologies, STEM College, RMIT University.

[1]: https://github.com/rmit-computing-technologies/redbackbots-coderelease