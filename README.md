# Lego Pieces

![https://i.hizliresim.com/own3bkc.gif](https://i.hizliresim.com/own3bkc.gif)

## Piece Creation Algorithm

I have not used any generation system for this project. This was the main function of the project. Since the first obstacle is that we are not able to fully randomly generate the pieces and connect them after generation, I have decided to take a different approach. I have been inspired by the growth of the plant roots and followed the same approach. After deciding the board size, and the piece count depending on the difficulty of the level, the 1x1 pieces are randomly spread across the board. Like the roots of the plants, the blocks choose a random suitable, and empty direction, and duplicate damsels towards them until the board is full.

This allows us to create a super performant system and eliminate the first obstacle. And also satisfy our rules of block count and grid size.

###

## Goal

The goal is to create a puzzle game where the player has to complete the grid with the procedurally generated pieces. The piece count and the complexity of the level are determined by the difficulty of the level. One of the main priorities of the project is performance and reliability.

## Rules

- The game should be developed using Unity.
- Graphical representation can be minimal with programmer art.
- We do not expect to see a game flow (menus, level choice screen, etc.), the game can open
up directly from a random level.
- All levels should be procedurally generated with 3 distinct level difficulties (easy,
medium, and hard). You can choose how to define level difficulty (piece count, the board size, etc).
- Levels can have minimum of 5, maximum of 12 pieces. The grid board size can be a minimum
of 4, and maximum of 6.
- Describe your level output as a portable text format. (like JSON, YAML, etc.) (levels should
be easily downloadable from a server)
- Please describe your procedural generation algorithm in written form. Explain high level
constructs, and other algorithms you based your implementation on if any.

## Game Difficulties

### Easy

Grid Size 4x4, min Pieces 5 max Pieces 7

### Medium

Grid Size 5x5, min Pieces 7 max Pieces 9

### Hard

Grid Size 6x6, min Pieces 9 max Pieces 12

## Obstacles

One of the first obstacles is creating a procedural system. Generating the scene fully randomly and then connecting the pieces would not satisfy the rules. The other problem is that the randomness of the generation could exceed the maximum call stack size and fail to generate.

