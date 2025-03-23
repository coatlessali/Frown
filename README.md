# FROWN
A SmileOS 2.0 compatibility layer for UNIX-like operating systems.

### What is this?
This is a tool for installing dummy Unity Players for other operating systems into a Windows build of ULTRAKILL. This allows it to be played on other operating systems without the need for actual compatibility layers or emulation.

### Who is this for?
- Hobbyists
- People without access to X11/Xwayland
- MacOS users without access to CrossOver/Whisky
- Other Niche Linux-oriented setups

### What is the point of this?
This can dramatically lower the amount of dependencies required to run ULTRAKILL on any given Linux computer, as well as CPU overhead.

|                  | Native           | WINE/Proton                                |
------------------------------------------------------------------------------------
| 32-bit libraries | No               | Depends on build settings (usually Yes)    |
| Vulkan           | Optional 1.0     | Optional 1.0 / 1.3 for DXVK                |
| OpenGL           | 3.2              | 3.2 (Native) / Later for wined3d           |
| CPU Extensions   | SSE2             | SSE2                                       |
| X11              | Optional         | Yes                                        |
