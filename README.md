# VortexEngine
.NET Core 3+ Game Engine (WIP)

**Current Features:**

+ Currently tested on OSX 10.15.2
+ Platform abstraction (windowing and input) with SDL2
+ Simple Game and Scene concept with time scalable variable frame rate game loop;
+ Assets Manager and PNG loading with STBSharp
+ Abstracted Rendering system, currently implemented are simple batch renderers:
+ SDL2 Renderer with SDL2 (SDL2-C#)
+ Metal Renderer with Sokol (SokolSharp)
+ GameToolkit classes: Behavior System; GameObject, Sprite and Animated Sprite abstractions; Tween movement;

**To Do:**

+ Fix Windows Support
+ Implement Sokol Opengl Renderer
+ Add Gamepad Input support
+ Implement Sine Movement Behavior
+ Implement Text GameObject
+ Refactor GameToolkit classes as necessary
+ Add Sound Support
+ Add Linux Support

**Long Term:**

+ Add more Behaviors
+ Implement Game Editor








