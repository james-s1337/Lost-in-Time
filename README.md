## Lost in Time

Lost in Time is a 2D roguelike dungeon crawler that is inspired by Dead Cells. You are a secret agent that must go back to different times in the past through a time machine to prevent a future apocalypse from occuring. However, the time machine is unstable, 
so you don't know where you are going to end up at. 

## Overview

You start with a simple gun and sword, but as you progress through the procedurally generated levels, your gear will slowly be upgraded through finding random loot and weapon upgrades. Even though each level/time period is procedurally generated, there will be a general
ruleset for each level/time period so that they feel distinct and structured (ex. a level will always have its starting point near the bottom, and then branch upwards in a y shape, and certain rooms can only generate beside other certain rooms in a limited quantity).
This way of generating is very similar to the approach used in the 2D roguelike game Dead Cells.

You can also get random abilities, which will drastically affect how you would approach your current run. At the end of certain levels, there will also be a boss fight, and you will unlock new features like for example a weapon modifier reroller after
defeating certain bosses.

The end goal of this game is to beat all the levels/time periods (which their order is randomized and some time periods can only be accessed through certain other time periods) without dying and using what you get through your run.

## Lessons Learned

This project has taught me:

- How to make and use a player finite state machine (modular, OOP) to control animation states and to easily add new states such as wall climb and skills to the player
- Use a combination of Binary Space Partitioning and conditional graphs to generate random but structured dungeons (planned)
- Use of Context-Steering for smarter enemy AI that avoids obstacles and has a "line-of-sight"
- General game design and level design
- How to add 2D shaders (planned)

## Credits

Created by James Song
