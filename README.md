# Playthrough Logging For Needy Streamer Overload

A simple mod that tracks all actions and events during a single playthrough and saves them in a CSV file, one per save.
You can find these files in the Logs folder, located in the same place as the main mod folder.

### Note: You must have BepInEx pre-configured and enabled with the game to use this mod.
[You can read how to configure BepInEx with the game here.](https://gist.github.com/amazeedaizee/ae0dd70cc0d842d6a83cd80451e3752e)

## Overview

Logs will only save completed days or days that have lead to an ending.

Logs will automatically export to the Logs folder after the game is closed. 

To force export logs, use the **Scroll Lock** Key. In this case, actions and events will be saved to a new log separate from the previous export.

## Change Log Language

This mod supports multiple languages. You can change what language the logs will save as through the **settings.json** file.

Here are the numbers to use when editing the **settings.json**:

- 0 : Japanese
- 1 : English
- 2 : Chinese (simplified)
- 3 : Korean
- 4 : Chinese (traditional)
- 5 : Vietnamese
- 6 : French
- 7 : Italian
- 8 : German
- 9 : Spanish

Any other value will set the language to the default (based on the current language in the game).

-----

Mod uses the [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) library, to export the .ame file (use is TBD!).


This mod is fan-made and is not associated with xemono and WSS Playground. All properties belong to their respective owners.

Haven't downloaded Needy Streamer Overload yet? Get it here: https://store.steampowered.com/app/1451940/NEEDY_STREAMER_OVERLOAD/
