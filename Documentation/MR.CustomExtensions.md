# MRExtensionMethods

This repository contains a number of extension methods I frequently use for Unity.
Some could be useful to others, some are oddly specific and probably shouldn't be here...

The extensions are currently organized into the following categories:

+ GameObjects
+ IEnumerators
+ Lists
+ Vectors
+ Miscellaneous

See the various .cs files for what is available.

In addition, **MRhierarchy.cs** allows us to colourize and customize the look of GameObjects in the Hierarchy.
This may be useful for organizing various parts of the hierarchy more clearly.
For example, we can add a background colour, border colour (and size), text colour (and size).

To activate this use **//** (double slash; same as creating a comment in your C# script) at the beginning of the
GameObject's name. Then you may name it, or leave the name blank. The following options are available:


+ bg:   => background colour
+ b:    => border colour
+ bs:   => border size
+ t:    => text colour
+ ts:   => text size
+ gr:   => gradient with two colours separated by **-** (e.g. red-blue)
+ icon: icn: ic:    => draw MRicon

using **b:=** results in the border being the same colour as the background (i.e. no border)
All UnityEngine.Color predefined colours are allowed to be used:

* yellow
* clear
* grey
* gray
* magenta
* cyan
* red
* black
* white
* blue
* green

and example use could be:

```
// EnemyTypes bg:red b:green bs:4 t:black ts: 16
// Terrain gr:yellow-green
// ----- Fish ----- gr:magenta-cyan b:black bs:3
```