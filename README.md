# Cats are Liquid: Multiplayer
A multiplayer mod/plugin written in C# for *Cats are Liquid: A Better Place*.

> [!IMPORTANT]  
> This mod is still in development. More features and fixes will be added over time.

> [!CAUTION]  
> *Currently*, the mod only works if you control the cat using WASD! Please set your keybinds to WASD in the game settings.

## Features
- ✅ Peer-to-peer multiplayer – connect using IP addresses
- ✅ Adjustable settings to prioritize smoothness or movement accuracy

## Coming Soon / In Development
- ⭕ Local multiplayer (2 players on the same keyboard using WASD and arrow keys)
- ⭕ Room/code-based multiplayer to eliminate the need for IP addresses
- ⭕ Room synchronization

## Current Limitations
These issues will be addressed in future updates (except for Windows exclusivity):
- ❌ Movement syncs correctly *only* if WASD is used.
- ❌ Requires an NPC (red, green, or blue) in the room.
- ❌ Both players must have the same worldpack and be in the same room.
- ❌ Windows/PC-only support.

---

# Installation (Recommended Method)
1. Visit the [Releases page](https://github.com/PatrikSzaboHU/CatsAreLiquid_Multiplayer/releases/) and download the `complete.zip` file from the latest release. This package includes both [BepInEx](https://github.com/BepInEx/BepInEx) (the tool that injects the mod into the game) and the mod itself.
2. Locate the game’s installation directory:
   - Open Steam and go to **Library** → *Cats are Liquid - A Better Place* → Click the **gear icon** on the right → **Manage** → **Browse local files**.
   - This will open the game’s installation folder. Keep this window open.
3. Extract the downloaded archive and open the extracted folder. Copy all its contents into the game's installation directory.
4. Start the game. If the installation was successful, pressing `F3` should open the multiplayer settings.

---

# Connecting Over a Local Network
If both computers are on the same network, follow these steps:

### Host (Server)
1. Open a Command Prompt on the host PC and run:  
   ```sh
   ipconfig | findstr "IPv4"
   ```
   This will display your local IP address.
2. Start the game and enter a world with an NPC cat (red, green, or blue).
3. Press `F3`, leave all settings as default, and click **Start/Stop Networking**.

### Client (Player 2)
1. Join the same room/worldpack as the host.
2. Press `F3` and enter the host's IP address in the provided textbox.
3. Click **Start/Stop Networking**.

After connecting, the client will take control of the NPC. Move around to verify the connection.

---

# Connecting Over a Public Network
If you and the other player are not on the same local network, follow these steps:

### Host (Server)
1. Open [whatismyip.com](https://whatismyip.com) and note down your **IPv4 address**.
2. Log in to your router's admin panel (usually accessible via `192.168.1.1` or `192.168.0.1` in a web browser).
3. Locate **Port Forwarding** settings.
4. Add a new port forwarding rule:
   - **Protocol**: TCP
   - **Port**: `2287`
   - **Local IP Address**: Your computer's local IP (from `ipconfig | findstr "IPv4"` command)
   - **External Port**: Same as internal (e.g., `2287`)
5. Save the changes and restart your router if needed.
6. Start the game, enter a world with an NPC cat, press `F3`, and click **Start/Stop Networking**.

> [!NOTE]  
> Keep in mind, that port forwarding settings are different on every router. I recommend that you look up a guide for your specific router.

### Client (Player 2)
1. Join the same room/worldpack as the host.
2. Press `F3` and enter the host’s **public IP address** (from Google) in the textbox.
3. Click **Start/Stop Networking**.

After this, the connection should be established, and the client should take control of the NPC.

---

# Need Help?
If you need assistance with setup, feel free to DM me on Discord (`cablesalty`) or message me on [Telegram](https://telegram.me/cablesalty).

---

# Found a Bug?
If you encounter any issues, please DM me on Discord or [open an issue](https://github.com/PatrikSzaboHU/CatsAreLiquid_Multiplayer/issues) on GitHub.

---

