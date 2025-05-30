# Lagniappe Gumbo

*Lagniappe Gumbo* is a narrative-driven game about memory, food, and quiet acts of generosity—created for the COREBLAZER Game Jam 2025.

You play as Nina, a girl visiting her grandmother’s old neighborhood in New Orleans for the first time. When she finds a faded gumbo recipe scribbled on a napkin, she decides to recreate it—hoping to surprise her grandmother with a taste of the past. As she searches for ingredients, she begins to uncover stories from those who knew her grandmother long ago.

The game builds toward a communal moment—neighbors gathering, jazz rising, and something long-lost returning.

## Themes

- Generosity, framed as giving beyond expectation
- Food as cultural memory
- Neighborhood as a living archive
- Quiet, layered storytelling inspired by magical realism

## Gameplay Summary

- One-screen exploration with light player movement
- Interactive NPC dialogue with branching memory-sharing moments
- Quiz-based storytelling: players earn ingredients by recalling details from NPC memories
- Dynamic gumbo pot system that tracks ingredients and reacts poetically to what’s added
- A final sequence that brings the entire neighborhood together—if the right gumbo is made

## Development Highlights

- Built in Unity 2022.3 LTS (2D URP)
- Modular NPC system using ScriptableObjects
- Custom interaction system (`IInteractable`) for pickups and dialogue triggers
- Dialogue and quiz UI built with UGUI and TextMeshPro
- GameManager singleton tracks ingredient state and unlocks flow milestones
- SpawnManager dynamically introduces NPCs based on player progress
- Jazz soundtrack builds layer-by-layer with each new ingredient added

## Current Progress (as of May 29)

- All five core NPCs are implemented, each with custom dialogue, memory text, and quiz flow
- Tutorial ingredient pickups (roux, rice, stock) are working and animated
- Gumbo pot system fully implemented with flavor feedback for each item
- Game flow logic: waves of NPCs appear based on ingredient progress
- Intro dialogue and ending scene designed and partially implemented

## Project Structure

Assets/
├── Prefabs/
│ ├── NPCs/
│ ├── Ingredients/
│ └── UI/
├── Scripts/
│ ├── GameManager.cs
│ ├── DialogueUIController.cs
│ ├── IngredientPickup.cs
│ ├── SpawnManager.cs
│ └── InteractionSystem/
├── Sprites/
└── UI/

csharp
Copy
Edit

## Team

- Bergen Carloss – Design, narrative, programming, UI, systems
- Music and illustrations are a mix of hand-curated assets and original concepts built during the jam

## Goals for Final Submission

- Finish visual pass on the background “quarter” (via Figma or tileset)
- Add ambient NPC movement to bring the space to life
- Implement layered jazz soundtrack and audio logic
- Polish ending sequence and dialogue with Grandma Bev

## Why the Title?

“Lagniappe” means a little something extra—especially something unexpected, offered freely, as a gesture of goodwill. That’s what this gumbo is.

## License

This project was created as part of a non-commercial game jam and is shared for learning and inspiration.
