using Godot;
using System;

public partial class HostAndJoin : Control
{
	[Export]
	private int port =8910;

	[Export]
	private string address = "127.0.0.1";

	private ENetMultiplayerPeer peer;
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
	}

	private void ConnectionFailed()
	{
		GD.Print("CONNECTION FAILED");
	}

	private void ConnectedToServer()
	{
		GD.Print("CONNECTION SUCCESFUL");
		RpcId(1, "sendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
	}

	private void PeerDisconnected(long id)
	{
	   GD.Print("PLAYER DISCONNECTED" + id.ToString());
	}

	private void PeerConnected(long id){
		GD.Print("PLAYER CONNECTED" + id.ToString());
	}
	
	
	public override void _Process(double delta)
	{
	}
	private void hostGame(){
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(port, 2);
		if(error != Error.Ok){
			GD.Print("error cannot host! :" + error.ToString());
			return;
		}
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);

		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Waiting For Players!");
	}
	public void _on_host_button_down()
	{
		hostGame();
		sendPlayerInformation(GetNode<LineEdit>("LineEdit").Text, 1);
	}
	public void _on_join_button_down(){
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(address, port);
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("JOINING GAME");
	}
	public void _on_start_game_button_down(){
		Rpc("startGame");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void startGame(){
		var scene = ResourceLoader.Load<PackedScene>("res://scenes/Multi.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void sendPlayerInformation(string name, int id){
		PlayerInfo playerInfo = new PlayerInfo(){
			Name = name,
			Id = id
		};
		
		if(!GameManager.Players.Contains(playerInfo)){
			
			GameManager.Players.Add(playerInfo);
			
		}

		if(Multiplayer.IsServer()){
			foreach (var item in GameManager.Players)
			{
				Rpc("sendPlayerInformation", item.Name, item.Id);
			}
		}
	}
}
