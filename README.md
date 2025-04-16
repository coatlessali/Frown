# FROWN
A SmileOS 2.0 compatibility layer for UNIX-like operating systems.

### What is this?
This is a tool for installing dummy Unity Players for other operating systems into a Windows build of ULTRAKILL. This allows it to be played on other operating systems without the need for actual compatibility layers or emulation.

### How does it work?
On Linux:
- Overwrites your netstandard2.1 DLLs
- Drops a dummy UnityPlayer.so and executable into your game folder
- Provides plugins for Discord and Steam
On macOS:
- Creates a dummy ULTRAKILL.app
- Copies and symlinks files into it

### Who is this for?
- Hobbyists
- Linux users without access to X11/Xwayland
- MacOS users without access to CrossOver/Whisky
- Other Niche Linux/UNIX-oriented setups
- Have you ever watched a Bringus Studios video? Yeah, that genre of shenanigans

### What is the point of this?
This can dramatically lower the amount of dependencies required to run ULTRAKILL on any given Linux computer, as well as CPU overhead. It also eliminates the need to set up WINE/Proton/CrossOver.

### Will this work forever?
In theory, it could as long as no Windows specific dependencies are added to the game.

### What's currently broken?
- The chess minigame for one. You can compile your own stockfish executable for your platform (make sure to compile without any extensions) and rename it to the same name as the original stockfish exe, then replace the original if you want it to work.
- There's a texture that doesn't display properly in 6-1 under Vulkan. It's barely noticeable in actual gameplay.
- The game has a tendency to crash on first level load under Wayland. Once you're in, crashes are very rare.

### Is this actually a compatibility layer like WINE?
No. That's just a silly lore bit. I like the aesthetic of SmileOS 2.0.

### Is this endorsed by the ULTRAKILL development team?
No, but it also hasn't been denounced. Yet. If they ask me to take this down, I will. If any of them are reading this and want me to take it down, they can contact me via Discord or [Email](mailto:coatlessali@protonmail.com).

### Can I speedrun with this?
No. Speedrun mods have stated that any addition to the game files is not legal for runs. Use WINE/Proton/CrossOver instead.

### Is this compatible with mods?
In theory. You need to use the UNIX version of BepInEx, and if your mod uses Windows specific functionality, or only has shaders compiled for D3D11, you're out of luck.

### Can you make this work with XYZ?
Here's a table.

| Platform   | Can I? | Will I? | Why?                                                                      |
| ---------- | ------ | ------- | ------------------------------------------------------------------------- |
| Win32-i686 | ?      | Maybe   | Would be funny. Could push some very old hw to the limit.                 |
| Android    | ?      | No      | Even if possible, I do not want the curse of infinite Android kids.       |
| Linux-i686 | ?      | Maybe   | Funny embedded devices.                                                   |
| ChromeOS   | ?      | No      | Don't have test hardware.                                                 |
| iOS/iPadOS | No     | No      | Don't have test hardware + needs Metal support.                           |
| macOS-ARM  | No     | No      | Game technically launches, but needs Metal support to function.           |
| tvOS       | No     | No      | Don't have test hardware + needs Metal support.                           |
| Switch     | ?      | Maybe   | Would depend on me either having a license or a game of the same version. |
| Xbox One   | ?      | Maybe   | Would need hardware to test with and an Xbox developer license.           |
| Xbox X|S   | ?      | Maybe   | See above.                                                                |
| FreeBSD    | ?      | Maybe   | Might work with the Linux compatibility stuff it has.                     |
| Other      | ?      | No      | Lack of interest.                                                         |

### What are the system requirements?
macOS: Mojave 10.15+ w/ OpenGL 3.2 or greater. Reportedly runs best on Intel.

Linux: glibc 2.2.5 or greater, OpenGL 3.2/Vulkan 1.0 or greater, X11/Wayland, x86 environment (todo: test emulation on arm/riscv)
