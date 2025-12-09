# 📦 Modular Clean Inventory System (Unity 6 + MVP + VContainer)

A production-ready, scalable Inventory System demonstrating **Clean Architecture**, **Dependency Injection**, and **Test-Driven Development (TDD)** in Unity.

![Unity Version](https://img.shields.io/badge/Unity-6000.3.0f1-black)
![Architecture](https://img.shields.io/badge/Architecture-MVP-blue)
![Testing](https://img.shields.io/badge/Testing-NUnit-green)

## 🎯 The Goal
To engineer a system that solves the common "Spaghetti Code" problems in game development. This project moves away from tight coupling (Singletons/Monobehaviours) to a modular architecture where **Logic**, **Data**, and **UI** are completely separated.

## ✨ Key Features
*   **📐 Model-View-Presenter (MVP):** Decouples business logic from the Unity UI. The Model is pure C# and has no dependencies on `UnityEngine.UI`.
*   **💉 Dependency Injection (VContainer):** Implements a stateless architecture. Services and Models are injected, making the system modular and easy to expand (e.g., sharing the Save System with Chests or Shops).
*   **💾 Persistence Layer:** A robust, stateless JSON Save/Load service handling Data Transfer Objects (DTOs) and `ScriptableObject` registry lookups.
*   **🖱️ Complex Interaction:** Full Drag-and-Drop system utilizing Unity's `IBeginDragHandler` and `IDropHandler` interfaces, connected by the Presenter.
*   **🧪 Unit Testing:** 
![Tests Passing](Assets/Documentation/Images/tests_passing.png)
Core logic (Stacking, Swapping, Capacity limits) verified with NUnit EditMode tests.
*   **🤖 Modular Character Controller:** A Finite State Machine (FSM) implementation where behaviors (Idle, Move, Jump) are isolated in pure C# classes.
*   **🏗️ State Pattern:** States are decoupled via C# Events (`OnMove`, `OnJump`, etc...), allowing the Controller to act as a logic switchboard without spaghetti code.
*   **⚙️ Data-Driven Tuning:** Movement physics (Speed, Jump Force) are configured via `ScriptableObjects`, enabling designers to tweak feel without code changes.
*   **🎮 Input Bridging:** An `InputReader` layer that converts inputs into Domain-specific data structs, keeping logic independent of the Input System.

### 🎮 Gameplay Demos
![Gameplay Inventory Demo](Assets/Documentation/Images/demo.gif)
*(Showing Drag & Drop, Stacking, and Save/Load)*

![Gameplay Finite State Machine Demo](Assets/Documentation/Images/demo_FiniteStateMachine.gif)
*(Character moving, jumping, and air moving)*

## 🏗️ Architecture Overview

![Architecture Diagram](Assets/Documentation/Images/arch_diagram.png)

### 1. The Domain (Model) 🧠
*   **Responsibility:** Holds the state (`List<InventorySlot>`) and executes logic.
*   **Key Trait:** Reactive. Fires C# Events (`OnSlotUpdated`) when state changes.
*   **Tech:** Pure C#. No Monobehaviours.

### 2. The View (UI) 🖼️
*   **Responsibility:** Renders the grid and captures Input (Clicks, Drags).
*   **Key Trait:** Passive. It does not decide *what* happens, only *that* an interaction occurred.
*   **Tech:** Unity UI, Event Interfaces.

### 3. The Presenter (Controller) 🔌
*   **Responsibility:** The "Glue." Listens to the View events and calls Model commands.
*   **Key Trait:** Testable. Can be unit tested by mocking the View and Model.
*   **Tech:** VContainer EntryPoint (`IStartable`, `IDisposable`).

### 4. The Character (Finite State Machine) 🏃
*   **The Brain (StateMachine):** A pure C# class that manages transitions (`Enter`, `Exit`, `Update`, `FixedUpdate`) and dependencies.
*   **The States:**
    *   **Idle:** Listens for input to transition to move or jump.
    *   **Move:** Handles ground movement and rotation and transition to idle or jump
    *   **Jump:** Handles character jump and air-rotation and movement and transition to idle.
*   **The Wiring:** States communicate via **Events**. The `IdleState` doesn't know the `MoveState` exists; it just fires `OnMove`, and the `PlayerController` handles the transition. Same applies for all states.

## ⚔️ "Legacy" vs "Modern" Comparison

This project includes a [Legacy Reference Script](Assets/Documentation/Legacy_Reference/LegacyInventoryManager.cs) to demonstrate the improvement over standard implementation.

| Feature | Legacy Approach (Junior) | Modern Approach (This Project) |
| :--- | :--- | :--- |
| **Data Access** | `InventoryManager.Instance` (Singleton) | `[Inject]` via VContainer |
| **Save System** | `PlayerPrefs` inside UI code | `JsonInventoryService` (Stateless) |
| **Coupling** | Logic references UI Text/Images | Logic fires Events, UI listens |
| **Testing** | Impossible without playing Scene | Logic tested in < 0.1s via Test Runner |

## 🚀 How to Run
1.  Open project in **Unity 6**.
2.  Open **Test Runner** (Window > General > Test Runner) and run EditMode tests to verify logic.
3.  Open `Prototype_Scene`.
4.  **Controls:**
4.  **Controls:**
    *   **WASD / Arrows**: Move Character
    *   **Space**: Jump (with Air Control)
    *   **A**: Add Sword
    *   **S**: Add 5 Potions (Stacking Test)
    *   **R**: Remove Item
    *   **K**: Save Inventory to JSON
    *   **Mouse**: Drag and Drop items to swap slots.

## 📂 Project Structure
*   `_Project/Scripts/Core`: Shared Interfaces and Signals.
*   `_Project/Scripts/Features/Inventory`: The core module.
    *   `/Domain`: Logic & Data (Model, ScriptableObjects).
    *   `/View`: UI Scripts (MonoBehaviours).
    *   `/Presentation`: Presenters (Pure C#).
    *   `/Services`: JSON Persistence.
*   `_Project/Tests`: NUnit Test Assembly.