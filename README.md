# ICS4U FinalProject
Main Block Comment

Dreamwielder is a top down, roguelite game inspired by Nuclear Throne. The game is melee focused with numerous upgrades to keep the game fresh and interesting. 
This project was made in about three weeks worth of work using Unity version 2022.3.9f1 for the ICS4U Final project.

Story:
Dreamwielder's story revolves around the concept of a dream/nightmare that occurs when someone sleeps. Within the game, everything is madeup within someone's unconsious state. 
This is why when you win, you wake up from the game and when you lose, you fail to escape. It also explains all the bizzare enemies, upgrades, weapons along with the colourful setting.

NOTE: There were various custom assets also made for this project, this includes
- All enemy sprites and animations.
- The drops from enemies are custom made.
- The sword the player carries and its animations.
- The ui elements found throughout the game (except for the cards).

Entity:

- Different status conditions such as stun, root, endlag, slow, speed boost, etc.
- Abstract class.
- Uses couroutines to manage speed boosts and to avoid issues when stacking speed buffs at the same time.
- Has all damage and heal methods.
- Uses Physics2D to generate knockback.

Enemy:
- Uses the AStar pathfinding algorithm to move enemies to the player.
- Each enemy has unique custom made animations and code.
- All have unique behaviors.
- Enemie stats scale with the levels.
- Enemies have trackers to determine their position.
- Shark:
	- Fleeing type enemy, this enemy kites the player, fleeing once they get too close.
	- They fire projectiles in a circle around them, making them difficult to deal with.
- Star: 
	- Standard melee enemy.
- Starwand:
	- Upon getting within range, fires projectiles at the player.
- Shadow:
	- Inspired by the thumper from Lethal Company. 
	- Charges up towards the player, increasing speed and gaining hyperarmour. Upon contact, deals damage and knocks player back.
- Cloud
	- Sniper type enemy with very long range.
	- Locks onto player and fires high speed projectile.
- BigStar:
	- large tank type enemy.
	- has high knockback resistance and cannot be stunned.



Player Stats + Player Camera + Player Controller:
- Camera moves using vector math in the direction one's cursor is. (This is also used in the menu)
- Contains many base mechanics.
	- Main attack (M1)
	- Secondary Attack (m2)
	- Tempo Burst which requires tempo (F) also deals max health % damage
	- Rush/Dash attack that requires tempo (Space)
- Successfully loads upgrades that each have unique interactions which are all compatable with on another.
- Uses vector math to position and animate the weapon within unity.
- Contains a tempo system. This is shown using the blue bar on the left. Tempo decreases like an exponential function by multiplying the current value by a factor of 0.997.
- Tempo increases player movement speed and melee damage.
- Some moves such as tempo burst and rush has both a tempo requirement and a tempo cost, meaning that you get above a certain amount of tempo then spend a lesser amount each cast.
- Most attacks interact and destroy enemy projectiles

Weapons + Melee Weapons:
- Has a 3-4 hit combo system that allows certain abilities to be chained together.
- Sample combo: (m1 -> m1 -> side gun -> rush).
- Melee weapon all applies on hit and on attack upgrade effects.

Upgrades:
- 19 Upgrades in total, each has specific abilities or stat buffs.
- Designed to function together. For example, Owl Claw, Owl Slice, and Bloodmoon all intend of functioning together as they all interact with one another.
- Secondary upgrades swap the secondary weapon of the player.
- Each upgrade is seperated into types which need to be seperately accomdated for.
- Sometimes unique upgrades warrented their own implementation.
Unique Secondaries:
- FireColumn/Darkbolt: Summons a lightning bolt that does two ticks of damage.
- Phantom Step: Iframes and speed for tempo.
- Sidegun: Knockback projectile.
- Windwall: projectile blocking wall.



Player Manager:
- Manages most stat buffs and modifications (attack buffs, health buffs, range buffs).
- Manages whenever an upgrade is added, or player data needs to be saved when reloading the map.
- Uses singleton to provide references to the player for all scripts.
- Manages spawning and despawning the player while making sure all stats such as health, upgrades, movespeed, etc are all saved and brought over.

Projectile Manager:
- Extremely modular script that manages all projectile generation within the game.
- uses vector math and Unity's 2D physics to calculate range and rotation.
- Easily able to add more projectile types using serialized fields.

Sound Manager:
- Extremely modular script that allows for sound to be easily added and played without any tedious additions.
- Normally in unity, a new sound source is given to entities that play sound. With sound manager, it can be done with one method without using any additional scripts
- Plays all music and sounds.

Enemy Manager:
- Handles all enemy spawning
- Graphs were created using desmos to map out enemy spawnrates. Lower tier enemies spawn less as higher tier ones spawn more.
- Despawning and clearing enemies.
- Allows enemies to scale off of level.

Map Manager: 
- Responsible for produral generation of maps for each level in the game, so no map will be the same.
- Using a 2D array, it uses a modified version of the drunkard walk generation algorithm.
- Can specify size and other values.

Upgrade Manager:
- Responsible for giving upgrades upon level completion.
- Ensures same upgrades do not show up twice.
- Gives additional rewarded upgrades for coloured challenge rooms.

Grid manager:
- Responsible for creating a 2D array for the level selection between procedurally generated maps.
- Tiles/Rooms have also procedurally generated variations to them.
- Each variation slightly changes the map that you enter into, giving the player freedom of choice.

Data Manager:
- Saves and loads data between instances of the game.
- saves stats such as game time, money, menu upgrades, etc.
- Used especially in the main menu where you can see your stats and buy permanent upgrades within the game's shop.

Menu manager: 
- Responsible for allowing the player to interact with menu components.
- Generates the actual UI for screens like the tutorial or shop.

Scene Loader:
- Loads different scenes such as menu, death screen, game screen etc and prepares it for use.



>>Credits:

SFX:
Main Menu: https://www.youtube.com/watch?v=fxBuSlPhFI8
Game: https://www.youtube.com/watch?v=X_S9DsRLUHE
weapon hit: https://www.youtube.com/watch?v=bA2_SiKGKfs
Phantom Step: https://www.youtube.com/watch?v=IGbvqYbJ-ow
Windwall: https://soundcloud.com/allcastcouk/wind-howling-strong-wind-hq-sound-recording
OwlSlice: https://www.youtube.com/watch?v=2zaf_V9g8PQ
Death: https://www.youtube.com/watch?v=hI7yTX6Yjl4
Card Pick: https://www.youtube.com/watch?v=3A3pAwglm_U
DarkBolt: https://www.youtube.com/watch?v=dYuxVKD6vng
Tempo Burst: https://www.youtube.com/watch?v=vbIb-Z5uO7c&pp=ygUbZGVlcHdva2VuIGdyYW5kIGphdmVsaW4gc2Z4
https://www.youtube.com/watch?v=1yxN0FXE0ME
slash: https://www.youtube.com/watch?v=AIS8KbfVIcE
Rush: https://www.youtube.com/watch?v=wURp8_kO2m8&pp=ygUYc3dvcmQgc2xpY2Ugc291bmQgZWZmZWN0
Ending: https://www.youtube.com/watch?v=f5Ogh8ip3qs

Sprites:
https://xyezawr.itch.io/gif-free-pixel-effects-pack-15-magick-arrows Creator: xyezawr
https://bdragon1727.itch.io/retro-impact-effect-pack-2 (Spells) Creator: bdragon1727
https://snoblin.itch.io/pixel-rpg-free-npc (Character) Creator: snobli
https://xyezawr.itch.io/gif-free-pixel-effects-pack-5-blood-effects Creator: xyezawr
https://ansimuz.itch.io/gothicvania-magic-pack-9 Creator:ansimuz
https://craftpix.net/freebies/free-sky-with-clouds-background-pixel-art-set/ (cloud background)
https://imgur.com/a/ybP5qH2 (Card sprites)
https://imgur.com/a/ybP5qH2 (Card sprites)
https://openart.ai/discovery/sd-1007466442633007244 End Screen  

Packages:
A* Pathfinding Project: https://arongranberg.com/astar/

Known Bugs:
- Sometimes a Null value exception will appear in the console when running the game. I believe this is a Unity bug that doesn't affect the game at all. It has been there since we started and hasn't done anything 