# Playthrough Logging For Needy Streamer Overload

A simple mod that tracks all actions and events during a single playthrough and saves them in a CSV file, one per save.
You can find these files in the Logs folder, located in the same place as the main mod folder.

## Overview

Logs will only save completed days or days that have lead to an ending.

Logs will automatically export to the Logs folder after the game is closed. 

To force export logs, use the **Scroll Lock** Key. When saving logs this way, a brand new log will be exported after the game closes or when the **Scroll Lock** key is pressed again.

## Change Log Language

This mod supports multiple languages. The language the mod uses is automatically set based on the current language of the game, however it is possible to change what language the logs will save as through the **settings.json** file.

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
