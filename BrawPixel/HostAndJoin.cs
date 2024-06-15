using Godot;
using System;

public partial class HostAndJoin : Control
{
    // Exported variables for the port and address to allow easy configuration from the editor
    [Export]
    private int port = 8910;

    [Export]
    private string address = "127.0.0.1";

    // Variable to hold the ENetMultiplayerPeer object for managing multiplayer connections
    private ENetMultiplayerPeer peer;

    // Called when the node enters the scene tree for the first time
    public override void _Ready()
    {
        // Connect multiplayer signals to corresponding methods
        Multiplayer.PeerConnected += PeerConnected;
        Multiplayer.PeerDisconnected += PeerDisconnected;
        Multiplayer.ConnectedToServer += ConnectedToServer;
        Multiplayer.ConnectionFailed += ConnectionFailed;
        
    }

    // Method called when connection to server fails
    private void ConnectionFailed()
    {
        GD.Print("CONNECTION FAILED");
    }

    // Method called when successfully connected to the server
    private void ConnectedToServer()
    {
        GD.Print("CONNECTION SUCCESSFUL");
        // Send player information to the server
        RpcId(1, "sendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
    }

    // Method called when a peer disconnects
    private void PeerDisconnected(long id)
    {
        GD.Print("PLAYER DISCONNECTED " + id.ToString());
    }

    // Method called when a peer connects
    private void PeerConnected(long id)
    {
        GD.Print("PLAYER CONNECTED " + id.ToString());
    }

    // Called every frame, 'delta' is the elapsed time since the previous frame
    public override void _Process(double delta)
    {
    }

    // Method to host a game
    private void hostGame()
    {
        peer = new ENetMultiplayerPeer();
        var error = peer.CreateServer(port, 2); // Create server on specified port with 2 maximum peers
        if (error != Error.Ok)
        {
            GD.Print("Error: Cannot host! :" + error.ToString());
            return;
        }
        // Enable data compression for the host
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

        // Set the multiplayer peer to the created server
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("Waiting For Players!");
    }

    // Method called when the host button is pressed
    public void _on_host_button_down()
    {
        hostGame();
        // Send player information for the host (ID 1 is for the server itself)
        sendPlayerInformation(GetNode<LineEdit>("LineEdit").Text, 1);
    }

    // Method called when the join button is pressed
    public void _on_join_button_down()
    {
        peer = new ENetMultiplayerPeer();
        peer.CreateClient(address, port); // Create client to connect to the specified address and port
        // Enable data compression for the client
        peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
        Multiplayer.MultiplayerPeer = peer;
        GD.Print("JOINING GAME");
    }

    // Method called when the start game button is pressed
    public void _on_start_game_button_down()
    {
        Rpc("startGame");
    }

    // Remote Procedure Call (RPC) method to start the game
    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void startGame()
    {
        // Load and instantiate the multiplayer game scene
        var scene = ResourceLoader.Load<PackedScene>("res://scenes/Multi.tscn").Instantiate<Node2D>();
        // Add the instantiated scene to the scene tree
        GetTree().Root.AddChild(scene);
        // Hide the current control (lobby) to transition to the game scene
        this.Hide();
    }

    // RPC method to send player information to all peers
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    private void sendPlayerInformation(string name, int id)
    {
        // Create a new PlayerInfo object with the given name and ID
        PlayerInfo playerInfo = new PlayerInfo()
        {
            Name = name,
            Id = id
        };

        // Add the player information to the game manager's player list if not already present
        if (!GameManager.Players.Contains(playerInfo))
        {
            GameManager.Players.Add(playerInfo);
        }

        // If this instance is the server, broadcast player information to all connected peers
        if (Multiplayer.IsServer())
        {
            foreach (var item in GameManager.Players)
            {
                Rpc("sendPlayerInformation", item.Name, item.Id);
            }
        }
    }
}
