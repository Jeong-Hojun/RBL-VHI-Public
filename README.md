# Virtual Hand Illusion (VHI)-based Motor Imagery Guidance System

> **Sungkyunkwan University, Robotics & Biomechanics Lab (SKKU_RBL)**

A Unity-based clinical rehabilitation application for stroke patients, integrating Virtual Hand Illusion (VHI) therapy with Leap Motion hand tracking, Arduino-controlled haptic stimulation, and EEG/GSR biofeedback.

![MI Guidance System Screenshot](docs/screenshot.png)

---

## Features

### Therapy Modes
| Mode | Description |
|------|-------------|
| **Motor Imagery (MI)** | Visual hand animation with haptic feedback during imagined hand movements |
| **Action Observation (AO)** | Virtual hand illusion to induce sensorimotor illusion |
| **Tactile Stimulation** | Individual finger vibration via Arduino-controlled haptic device (fingers 2–6) |
| **Reaching Task** | 3D target-reaching rehabilitation exercise using virtual hand |
| **Neurofeedback (NFB)** | Real-time EEG/GSR signal visualization as a score bar for biofeedback |

---

## Scenes

| Scene | Description |
|-------|-------------|
| `VHI_CHAMC_GSR_0809` | GSR (galvanic skin response) biofeedback variant |
| `VHI_CHAMC_EEG_0816` | EEG (brainwave) biofeedback variant |
| `VHI_for_NF_250311` | Latest neurofeedback version |
| `VHI_SKKU_1031_SHAM` | Sham (placebo) control condition for clinical trials |
| `Main_ver2_CHAMC` | Main therapy scene |
| `PreScreen_ver.1.4_1030` | Pre-screening and calibration |

---

## Requirements

### Software
- **Unity 2018.4.26f1**
- Windows 10 or later

### Third-Party Assets (must be installed separately)

This repository contains only the source scripts. The following assets must be purchased/downloaded and installed manually into the project before it will compile:

| Asset | Source | Purpose |
|-------|--------|---------|
| **Leap Motion Core Assets** | [Ultraleap Developer Site](https://developer.leapmotion.com/) (free) | Hand tracking SDK |
| **LM Realistic Hands** | Unity Asset Store | High-fidelity hand models and textures |
| **ARDUnity** | Unity Asset Store | Arduino ↔ Unity serial communication |
| **Steel Dagger** | Unity Asset Store | Threat stimulus prop (GSR scene) |
| **BomJ MeatHammer** | Unity Asset Store | Threat stimulus prop (GSR scene) |
| **Elven Long Bow + Sci-Fi Crossbow** | Unity Asset Store | Threat stimulus props (GSR scene) |
| **Simple Low Poly Nature / Ground Materials FD** | Unity Asset Store | Environment background |
| **SimplePoly Stadium Kit** | Unity Asset Store | Environment background |

> The core therapy functionality (hand tracking, haptics, biofeedback) requires Leap Motion Core Assets, LM Realistic Hands, and ARDUnity. The AssetStore props are only used in the GSR threat scene and can be substituted freely.

### Hardware
- **Leap Motion Controller** — hand position and gesture tracking
- **Arduino-based haptic device** — finger vibration stimulator (default: COM8, 115200 baud)
- **EEG or GSR sensor** (Shimmer device recommended) — required for biofeedback scenes

---

## Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Jeong-Hojun/RBL-VHI.git
   cd RBL-VHI
   git lfs pull
   ```

2. **Open in Unity Hub**
   Open the project with Unity **2018.4.26f1**

3. **Install Leap Motion Driver**
   Install the [Leap Motion Orion SDK](https://developer.leapmotion.com/) and connect the controller

4. **Connect Arduino device**
   Verify the COM port in `ArduinoControl.cs` (default: COM8)

5. **Select and run a scene**
   Open the appropriate scene from `Assets/Scenes/` based on the experiment protocol

---

## Usage Guide

> **Recommended:** Have the patient wear a palm-supporter before starting any session.

### Before Starting

1. **Select the affected hand** using the **L** / **R** buttons on the main screen.
   - This swaps the active hand model and adjusts the collision layer so the correct hand interacts with targets.
2. **Select a therapy mode** from the three buttons.
3. Press **START** to begin.

---

### Mode 1 — Action Observation (AO)

Only the virtual hand is displayed. No coin motor or targets.

| Session | Key | What Happens |
|---------|-----|--------------|
| **Illusion session** | `R` (unlock) → `S` | The virtual hand moves. Press `R` again to stop. |
| **Imagination session** | `R` (unlock) → `A` | A beep sounds and the virtual hand performs open/close motion. Press `R` again to stop. |

---

### Mode 2 — Visuo-Tactile VHI

A virtual coin motor is displayed on each finger of the hand model.

| Session | Key | What Happens |
|---------|-----|--------------|
| **Illusion session** | `T` | Starts tactile stimulation sequence. The active finger's coin motor turns **red** and the hand shakes visually in sync with the vibration. Press `T` again to stop. |
| **Imagination session** | `R` (unlock) → `A` | A beep sounds and the virtual hand performs open/close motion. |

The stimulation sequence (finger order, duration, repetitions) is configured in `TactileTaskParent.cs` via the Inspector.

---

### Mode 3 — Visuo-Motor VHI

Virtual reaching targets (cylinders) appear in 3D space.

| Session | Key | What Happens |
|---------|-----|--------------|
| **Illusion session** | `V` | Starts the reaching task. The active target turns **yellow**. When the patient's hand reaches it, it flashes **blue** then resets to white, and the next target activates. Press `V` again to stop. |
| **Imagination session** | `R` (unlock) → `A` | A beep sounds and the virtual hand performs open/close motion. |

---

### GSR Scene — Additional Setup

In the GSR scene (`VHI_CHAMC_GSR_0809`), physiological data is recorded over TCP:

1. Enter the **IP address** of the GSR acquisition computer in the input field.
2. Press **START** — the system connects to port `6175` and begins receiving GSR data.
3. GSR data (timestamp + GSR value) is streamed continuously and saved as a `.csv` file when the application closes.
   > ⚠️ The save path is currently hardcoded to `D:\RESEARCH\EEG_NFTsystem\Data\GSR_AOMIvsVHI\`. Change this in `TCPManager1.cs` if needed.

**Threat stimulus (K key):**
Press `K` to drop a knife (or hammer/axe) toward the virtual hand — this triggers a fear/threat response. The K key state (`0` or `1`) is logged alongside the GSR data in the CSV, so you can align physiological responses to the stimulus onset.

---

### Keyboard Reference

| Key | Function |
|-----|----------|
| `R` | Toggle ready/lock state (required before imagination session) |
| `A` | Start imagination session (hand open/close + beep) |
| `S` | Start illusion session — Action Observation mode |
| `T` | Toggle tactile stimulation — Visuo-Tactile mode |
| `V` | Toggle virtual reaching task — Visuo-Motor mode |
| `K` | Trigger threat stimulus (knife/hammer drop) — GSR scene |

---

## Project Structure

```
Assets/
├── Scenes/           # Therapy scenario scenes
├── Scripts/          # Core C# scripts
├── ARDUnity/         # Arduino communication library
├── LeapMotion/       # Leap Motion SDK
├── LMRealisticHands/ # High-quality hand models and textures
├── Prefabs/          # Arm/hand models, UI, backgrounds
├── Animations/       # Hand and arm animation clips
└── Sounds/           # Audio cues (beep, ding)
```

---

## Key Scripts

| Script | Description |
|--------|-------------|
| `FingerAnimation.cs` / `FingerAnimationAO.cs` | Motor imagery / action observation animation control |
| `TactileTaskParent.cs` / `TactileTask.cs` | Tactile stimulation sequence generation and management |
| `ReachingTaskParent1.cs` / `ReachingTask.cs` | Reaching task target control |
| `ArduinoControl.cs` | Arduino serial communication and stimulation code dispatch |
| `ScoreManager.cs` | Biofeedback score UI display (receives data via TCP) |
| `TCPManager.cs` | TCP receiver for EEG/GSR sensor data |
| `CsvFileWriter.cs` | Experimental data export to CSV |

---

## Publications

This system was developed in conjunction with the following research:

1. **Jeong, H., & Kim, J. (2021).** Development of a Guidance System for Motor Imagery Enhancement Using the Virtual Hand Illusion. *Sensors*, 21(6), 2197. https://doi.org/10.3390/s21062197

2. **Jeong, H., & Kim, J. (2022).** Motor Performance Index for Evaluation of Distributed Pattern in Multi-channel EEG. *2022 9th IEEE RAS/EMBS International Conference for Biomedical Robotics and Biomechatronics (BioRob)*, pp. 1–5. https://doi.org/10.1109/BioRob52689.2022.9925535

3. **Jeong, H., Jung, H., Kim, M., & Kim, J. (2023).** Virtual Hand Illusion-Based Motor Imagery Guidance System for Stroke Patients: A Pilot Study. *2023 IEEE International Conference on Metrology for eXtended Reality, Artificial Intelligence and Neural Engineering (MetroXRAINE)*, pp. 1051–1056. https://doi.org/10.1109/MetroXRAINE58569.2023.10405674

---

## License

This software was developed for clinical research purposes at the Rehab-Biomechatronics Lab (RBL) , Sungkyunkwan University. Unauthorized use or distribution is prohibited.

---

*SKKU Rehab-Biomechatronics Lab*
