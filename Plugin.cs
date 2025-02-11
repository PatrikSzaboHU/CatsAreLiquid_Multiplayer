// Cats are Liquid Multiplayer mod
// Made by Patrik Szabó

using BepInEx;
using BepInEx.Logging;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Reflection;
using System;
using UnityEngine;
using System.Globalization;

namespace Cal_Multiplayer
{
    [BepInPlugin("Cal_Multiplayer", "Cats are Liquid Multiplayer", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        // Variables for networking
        private bool isNetworkMultiplayerEnabled = false;
        private bool isInFirstSyncPhase = true; // Toggles automatic position syncs. Starts off true, switches to false once everything is initialized.
        private bool isHosting = false; // Is this player the host
        private TcpListener server;
        private TcpClient client;
        private TcpClient incomingClient;
        private CancellationTokenSource networkCancellationSource; // Token that cancels network refreshes
        private string hostIP = "";

        // Variables for form and UI
        private bool showForm = false; // Corresponds to network form
        private bool showSyncSettingsForm = false;

        // Variables for position syncing
        private float positionSyncTimer = 0f; // Current time
        private float positionSyncInterval = 1.5f; // How often the position syncs (in seconds)

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo("Multiplayer plugin loaded!");
            
            // Make sure all values are in the correct cultural format
            // no matter what language or region the host machine has set
            // (required so the program can correctly split the values)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;
        }

        private void Update()
        {
            // Shows the main network form/UI
            if (Input.GetKeyDown(KeyCode.F3))
            {
                showForm = !showForm;
            }

            // Only executes if a client is connected or connected to a host
            if ((incomingClient != null && incomingClient.Connected) || (client != null && client.Connected))
            {
                // Get the correct network stream
                NetworkStream stream;
                if (isHosting)
                {
                    stream = incomingClient.GetStream();
                } else
                {
                    stream = client.GetStream();
                }

                // Handlers for releasing keys
                
                // Horizontal movement (keys A+D)
                if ((Input.GetKeyUp(KeyCode.D) && !OtherKeysDown(KeyCode.D)) || (Input.GetKeyUp(KeyCode.A) && !OtherKeysDown(KeyCode.A)))
                {
                    SendCommand(stream, "MoveAction:S;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }
                
                // Jumping
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    SendCommand(stream, "MoveAction:C;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }
                
                // Liquid
                if (Input.GetKeyUp(KeyCode.S))
                {
                    SendCommand(stream, "MoveAction:N;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }

                // Handlers for pressing keys
                
                // Right
                if (Input.GetKeyDown(KeyCode.D))
                {
                    SendCommand(stream, "MoveAction:R;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }
                
                // Left
                if (Input.GetKeyDown(KeyCode.A))
                {
                    SendCommand(stream, "MoveAction:L;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SendCommand(stream, "MoveAction:J;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }
                
                // Liquid
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SendCommand(stream, "MoveAction:W;");
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                    }
                    else
                    {
                        Logger.LogError("Cannot send pos: cat or path not found");
                    }
                }

                // Automatic position syncing
                // If the player is syncing the position for the first time,
                // this waits until that is finished to avoid sending the wrong positions
                if (!isInFirstSyncPhase)
                {
                    positionSyncTimer += Time.deltaTime;
                    if (positionSyncTimer > positionSyncInterval)
                    {
                        GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                        if (catPart != null)
                        {
                            SendCommand(stream, "PositionSync:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                            Logger.LogInfo("Sending sync command for pos: " + catPart.transform.position.x + "," + catPart.transform.position.y);
                            positionSyncTimer = 0f;
                        }
                        else
                        {
                            Logger.LogError("Cannot sync pos: cat or path not found");
                        }
                    }
                }
            }
        }

        // Check for movement cancellation for the opposite direction on
        // the horizontal axis
        private bool OtherKeysDown(KeyCode releasedKey)
        {
            if (releasedKey != KeyCode.D && Input.GetKey(KeyCode.D)) return true;
            if (releasedKey != KeyCode.A && Input.GetKey(KeyCode.A)) return true;

            return false;
        }

        // Sets and interpolates the lumi's position to a new position 
        private void SyncCatPosition(string catObjectName, float newPosX, float newPosY)
        {
            // Set all 12 cat parts to the new position
            for (int i = 0; i < 12; i++)
            {
                if (GameObject.Find(catObjectName + "/Liquid Features(Clone)") == null && i < 0)
                {
                    Logger.LogInfo("Cat is liquid, won't sync other parts");
                    continue;
                }
                GameObject catPart = GameObject.Find(catObjectName + "/Cat Part " + i.ToString());
                if (catPart != null)
                {
                    Vector3 newPosition = catPart.transform.position;
                    newPosition.x = newPosX;
                    newPosition.y = newPosY;
                    catPart.transform.position = Vector3.Lerp(catPart.transform.position, newPosition, Time.deltaTime * 10); // Smooths out movement instead of hard overwrite
                }
                else
                {
                    Logger.LogError("Cannot do function pos sync: cat or path not found");
                }
            }

            // Set metaball plane's position
            GameObject metaballPlane = GameObject.Find(catObjectName + "/Metaball Plane");
            if (metaballPlane != null)
            {
                Vector3 newPosition = metaballPlane.transform.position;
                newPosition.x = newPosX;
                newPosition.y = newPosY;
                metaballPlane.transform.position = Vector3.Lerp(metaballPlane.transform.position, newPosition, Time.deltaTime * 10);
            }
            else
            {
                Logger.LogError("Cannot sync metaball plane: cat or path not found");
            }

            // Set metaball camera's position
            GameObject metaballCamera = GameObject.Find(catObjectName + "/Metaball Camera");
            if (metaballCamera != null)
            {
                Vector3 newPosition = metaballCamera.transform.position;
                newPosition.x = newPosX;
                newPosition.y = newPosY;
                metaballCamera.transform.position = Vector3.Lerp(metaballCamera.transform.position, newPosition, Time.deltaTime * 10);
            }
            else
            {
                Logger.LogError("Cannot sync metaball camera: cat or path not found");
            }
        }

        // Sets lumi's position to a new position WITHOUT interpolation
        private void SetCatPosition(string catObjectName, float newPosX, float newPosY) // Sets pos without interpolation
        {
            for (int i = 0; i < 12; i++)
            {
                if (GameObject.Find(catObjectName + "/Liquid Features(Clone)") == null && i < 0)
                {
                    Logger.LogInfo("Cat is liquid, won't sync other parts");
                    continue;
                }
                GameObject catPart = GameObject.Find(catObjectName + "/Cat Part " + i.ToString());
                if (catPart != null)
                {
                    Vector3 newPosition = catPart.transform.position;
                    newPosition.x = newPosX;
                    newPosition.y = newPosY;
                    catPart.transform.position = newPosition;
                }
                else
                {
                    Logger.LogError("Cannot do function pos sync: cat or path not found");
                }
            }

            GameObject metaballPlane = GameObject.Find(catObjectName + "/Metaball Plane");
            if (metaballPlane != null)
            {
                Vector3 newPosition = metaballPlane.transform.position;
                newPosition.x = newPosX;
                newPosition.y = newPosY;
                metaballPlane.transform.position = newPosition;
            }
            else
            {
                Logger.LogError("Cannot sync metaball plane: cat or path not found");
            }

            GameObject metaballCamera = GameObject.Find(catObjectName + "/Metaball Camera");
            if (metaballCamera != null)
            {
                Vector3 newPosition = metaballCamera.transform.position;
                newPosition.x = newPosX;
                newPosition.y = newPosY;
                metaballCamera.transform.position = newPosition;
            }
            else
            {
                Logger.LogError("Cannot sync metaball camera: cat or path not found");
            }
        }

        // Sends data over the tcp tunnel
        private void SendCommand(NetworkStream stream, string command)
        {
            Logger.LogInfo("Sending " + command);
            byte[] data = Encoding.UTF8.GetBytes(command);
            stream.Write(data, 0, data.Length);
        }

        // Invokes an NPC action (MoveLeft, MoveRight, ...) for animation sync and smoothness 
        private void InvokeAction(Component npcPathfinder, MethodInfo performActionMethod, Type catActionType, string actionName, float actionDelay)
        {
            Logger.LogInfo("InvokeAction well... invoked lmao");
            try
            {
                object catAction = Enum.Parse(catActionType, actionName);
                performActionMethod.Invoke(npcPathfinder, [catAction, actionDelay]);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error invoking action {actionName}: {ex.Message}");
            }
        }

        // Starts hosting the server (peer-to-peer)
        private async Task StartServer(CancellationToken cancellationToken)
        {
            try
            {
                server = new TcpListener(IPAddress.Any, 2287);
                server.Start();
                Logger.LogInfo("Server started on port 2287.");

                while (!cancellationToken.IsCancellationRequested)
                {
                    if (server.Pending())
                    {
                        incomingClient = await server.AcceptTcpClientAsync();
                        Logger.LogInfo("Player 2 connected.");
                        _ = HandleClient(incomingClient, cancellationToken);
                    }

                    await Task.Delay(10); // Avoid high CPU usage
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Logger.LogError($"Server error: {ex.Message}");
            }
            finally
            {
                server?.Stop();
                Logger.LogInfo("Server stopped.");
            }
        }

        // Connects to a specified host
        private async Task StartClient(CancellationToken cancellationToken)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(IPAddress.Parse(hostIP), 2287);
                Logger.LogInfo("Connected to server.");

                await HandleClient(client, cancellationToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Logger.LogError($"Client error: {ex.Message}");
            }
        }

        // Handles a connected tcp client. Gets available data and passes it on to execute.
        private async Task HandleClient(TcpClient tcpClient, CancellationToken cancellationToken)
        {
            try
            {
                using NetworkStream stream = tcpClient.GetStream();
                byte[] buffer = new byte[1024];

                // Initial position syncing
                // Get correct cat based on if player is host or client
                if (isHosting)
                {
                    GameObject npcCatPart = GameObject.Find("NPC Cat(Clone)/Cat Part 0");
                    if (npcCatPart != null)
                    {
                        SendCommand(stream, "StartPos:" + npcCatPart.transform.position.x + "," + npcCatPart.transform.position.y + ";");
                        positionSyncTimer = 0f;
                    }
                    else
                    {
                        Logger.LogError("Cannot do initial pos sync (host): npc or path not found");
                    }
                }
                else
                {
                    GameObject catPart = GameObject.Find("Cat/Cat Part 0");
                    if (catPart != null)
                    {
                        SendCommand(stream, "StartPos:" + catPart.transform.position.x + "," + catPart.transform.position.y + ";");
                        positionSyncTimer = 0f;
                    }
                    else
                    {
                        Logger.LogError("Cannot do initial pos sync: cat or path not found");
                    }
                }

                // This loop executes while the networking isn't stopped using the token
                while (!cancellationToken.IsCancellationRequested)
                {
                    if (stream.DataAvailable)
                    {
                        // Read the incoming data
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();
                        Logger.LogInfo($"RX: {message}");

                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            // Split strings based on ";" because multiple commands can be on the same line
                            foreach (var command in message.Split(';', (char)StringSplitOptions.RemoveEmptyEntries))
                            {
                                // Pass on the command to execute
                                Logger.LogInfo($"Executing {command}");
                                HandleNetworkCommand(command.Trim());
                            }
                        }
                        else
                        {
                            Logger.LogInfo("No commands to execute. (might be null, empty or whitespace");
                        }
                    }

                    // Prevent tight loop and high CPU usage
                    await Task.Delay(100, cancellationToken);
                }
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                Logger.LogError($"Client handling error: {ex.Message}");
            }
            finally
            {
                // Disconnect gracefully
                tcpClient.Close();
                Logger.LogInfo("Client disconnected.");
            }
        }

        // Handles and executes a string command 
        private void HandleNetworkCommand(string command)
        {
            try
            {
                // Find the NPC to execute on
                GameObject npc = GameObject.Find("NPC Cat(Clone)");
                if (npc != null)
                {
                    // Get pathfinder. This will be modified for most actions
                    var npcPathfinder = npc.GetComponent("NPCPathfinder");
                    if (npcPathfinder != null)
                    {
                        MethodInfo performActionMethod = npcPathfinder.GetType().GetMethod("PerformAction", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                        if (performActionMethod != null)
                        {
                            Type catActionType = performActionMethod.GetParameters()[0].ParameterType;

                            NetworkStream stream;
                            if (isHosting)
                            {
                                 stream = incomingClient.GetStream();
                            } else
                            {
                                stream = client.GetStream();
                            }

                            // Commands look like this
                            // CommandName:Value1,Value2;
                            // Split and get command
                            switch (command.Split(':')[0])
                            {
                                // Tells the NPC to move left, right, etc...
                                // Just like the prophets tell the NPC to move left or right
                                case "MoveAction":
                                    // Command meanings:
                                    // R: Move right
                                    // L: Move left
                                    // J: Climb/Jump
                                    // W: Turn into liquid
                                    // S: Stop moving
                                    // C: Stop climbing
                                    // N: Turn back to cat
                                    switch (command.Split(':')[1])
                                    {
                                        case "R":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "MoveRight", 0.0f);
                                            break;
                                        case "L":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "MoveLeft", 0.0f);
                                            break;
                                        case "J":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "Climb", 0.0f);
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "Jump", 0.0f);
                                            break;
                                        case "W":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "SwitchToLiquidState", 0.0f);
                                            break;
                                        case "S":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "StopMoving", 0.0f);
                                            break;
                                        case "C":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "StopClimb", 0.0f);
                                            break;
                                        case "N":
                                            InvokeAction(npcPathfinder, performActionMethod, catActionType, "SwitchToDefaultState", 0.0f);
                                            break;
                                        default:
                                            Logger.LogWarning($"Unknown MoveAction argument: {command}");
                                            break;
                                    }
                                    break;
                                
                                // Gracefully sets NPC position to new position
                                case "PositionSync":
                                    var posX = float.Parse(command.Split(':')[1].Split(',')[0], CultureInfo.InvariantCulture);
                                    var posY = float.Parse(command.Split(':')[1].Split(',')[1], CultureInfo.InvariantCulture);
                                    Logger.LogInfo("Syncing clone");
                                    SyncCatPosition("NPC Cat(Clone)", posX, posY);
                                    break;
                                
                                // Instantly (without interpolation/smoothing) sets the NPC cat to a new starting position
                                case "StartPos":
                                    var startPosX = float.Parse(command.Split(':')[1].Split(',')[0], CultureInfo.InvariantCulture);
                                    var startPosY = float.Parse(command.Split(':')[1].Split(',')[1], CultureInfo.InvariantCulture);
                                    Logger.LogInfo("Syncing cat");
                                    SetCatPosition("Cat", startPosX, startPosY);
                                    isInFirstSyncPhase = false; // Enables auto pos syncs
                                    break;
                                
                                default:
                                    Logger.LogWarning($"Unknown command type: {command}");
                                    break;
                            }
                        }
                        else
                        {
                            Logger.LogError("PerformAction method not found on NPCPathfinder");
                        }
                    }
                    else
                    {
                        Logger.LogError("NPCPathfinder component not found on the GameObject");
                    }
                }
                else
                {
                    Logger.LogError("GameObject not found");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error processing network command: {ex.Message}");
            }
        }

        // Starts and stops the networking
        private void ToggleNetworking()
        {
            if (isNetworkMultiplayerEnabled)
            {
                StopNetworking();
            }
            else
            {
                StartNetworking();
            }
        }

        // Starts networking
        private void StartNetworking()
        {
            isNetworkMultiplayerEnabled = true;
            networkCancellationSource = new CancellationTokenSource();

            // If no IP is provided start hosting, else connect
            if (string.IsNullOrEmpty(hostIP))
            {
                Logger.LogInfo("Starting server...");
                _ = StartServer(networkCancellationSource.Token);
                isHosting = true;
            }
            else
            {
                Logger.LogInfo("Connecting to server...");
                _ = StartClient(networkCancellationSource.Token);
                isHosting = false;
            }
        }

        // Stops networking and disconnects
        private void StopNetworking()
        {
            isNetworkMultiplayerEnabled = false;
            networkCancellationSource?.Cancel();
            networkCancellationSource?.Dispose();
            networkCancellationSource = null;

            server?.Stop();
            client?.Close();
            Logger.LogInfo("Networking stopped.");
        }

        // Forms/UI
        private void OnGUI()
        {
            // Main networking form
            if (showForm)
            {
                // Create window in the center
                GUILayout.BeginArea(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 80, 320, 180), GUI.skin.box);
                GUILayout.BeginVertical();
                
                GUILayout.Label("You might need to pause the game to see your cursor.");

                // IP entry field
                GUILayout.Label("Enter IP to connect (leave empty to host):");
                hostIP = GUILayout.TextField(hostIP, 15);

                if (GUILayout.Button("Start/Stop Networking"))
                {
                    ToggleNetworking();
                    showForm = false;
                }

                if (GUILayout.Button("Sync/Player Settings"))
                {
                    showForm = false;
                    showSyncSettingsForm = true;
                }

                if (GUILayout.Button("Close"))
                {
                    showForm = false;
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }

            // Sync settings form
            if (showSyncSettingsForm)
            {
                GUILayout.BeginArea(new Rect(Screen.width / 2 - 160, Screen.height / 2 - 80, 320, 200), GUI.skin.box);
                GUILayout.BeginVertical();

                GUILayout.Label("Prioritize smoothness: Position syncing is less frequent, ensuring smoothness at the cost of position accuracy for movement happening between a second (ex.: Jumps might not be accurate).");
                GUILayout.Label("Prioritize accuracy: Position syncing is more frequent, ensuring accuracy and less latency at the cost of smoothness (character's sides might blink).");

                // Sets sync interval values to default
                if (GUILayout.Button("Prioritize smoothness (default)"))
                {
                    positionSyncInterval = 1.5f;
                    showSyncSettingsForm = false;
                    showForm = true;
                }

                // Makes syncs more frequent for accuracy
                if (GUILayout.Button("Prioritize accuracy"))
                {
                    positionSyncInterval = 0.01f;
                    showSyncSettingsForm = false;
                    showForm = true;
                }

                if (GUILayout.Button("Back"))
                {
                    showSyncSettingsForm = false;
                    showForm = true;
                }

                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
        }

        private void OnDestroy()
        {
            StopNetworking();
            Logger.LogInfo("Plugin destroyed. Networking stopped.");
        }
    }
}