# 🎮 Tetris Inventory System V2

A Tetris-style inventory system prototype built in Unity as part of my Games Engine Programming module. This system supports dynamic item placement, rotation, and interaction in a grid-based UI, inspired by inventories in games like *Resident Evil 4* or *Escape from Tarkov*.

> 🧠 Designed for rapid prototyping and experimentation with Unity UI, input handling, and custom data structures.

---

## 📦 Features

- 🔲 **Grid-based inventory layout**
- 🔄 **Item rotation**
- 🧪 **Item types**:
  - Weapons (e.g. sword, gun)
  - Potions (health or buff effects)
  - Food (e.g. apples, rations)
- ♻️ **Drag-and-drop mechanics**
- 🛠️ **Item scriptable objects**
- 📚 **Inspector customization**
- 🧩 **Modular system architecture**
- ☑️ **Singleton management** (replaces early observable prototype)

---

## 🧪 Technical Summary

- Written in **C#** for Unity
- Input handled via **Event Triggers** and **custom drag logic**
- Grid stored as a **2D array**
- Includes **debug display tools** for tracking items

---

## 🚀 Getting Started

1. Clone the repo:
   ```bash
   git clone https://github.com/jacobvillard/InventorySystemV2.git
    
2. Open the project in **Unity 2022.3 LTS** (or newer).  
3. Open the `main` scene.  
4. Hit ▶ **Play** to test the system.

---

## 🎥 Demo

Watch the system in action on YouTube:  
[📺 Inventory System Showcase](https://youtu.be/7PDsUQXIJmA?si=hUMwMj52JTiQgWwM)

---

## 📁 Project Structure

InventorySystemV2/
├── Scripts/ # Core inventory and item logic
├── ScriptableObject/ # Item definitions
└── Scenes/ main.unity # Main test scene

---

## 🧪 Technical Notes

- Originally developed using an observable pattern, later switched to a **singleton** for performance and simplicity.
- Modular design allows for quick iteration on new item types or grid behavior.
- Designed with extension in mind for use in full survival, RPG, or looter-style games.

---

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

## 🤝 Contact

**Email:** jacobvillard@gmail.com  
**Website:** [jacobvillard.github.io](https://jacobvillard.github.io)  
**GitHub:** [github.com/jacobvillard](https://github.com/jacobvillard)
