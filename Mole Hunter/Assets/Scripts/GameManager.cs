using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class GameManager : PunBehaviour
{
	private static GameManager instance;
	public static GameManager Instance
	{
		private set => instance = value;
		get
		{
			return instance;
		}
	}
	public static string My_room;

	public const string GAME_VERSION = "0.0.1";
	public ExPhoton.Hashtable Player_attributes;
	public ExPhoton.Hashtable Room_attributes;
	public RoomOptions Room_options;

	public static readonly long[] SEARCE_SCORE_DIFFS = new long[] { 200, 300, 400 };

	public enum META_PLAYER : uint
	{
		NUM_OF_GAMES = 20,
		WIN, LOSE, DRAW,
		SCORE_RANKING = 30
	}
	public delegate bool PlyerPredicate(PhotonPlayer player);

	public void Awake()
	{
		if (Instance is null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	public void Start()
	{
		Player_attributes = new();
		Player_attributes[META_PLAYER.NUM_OF_GAMES] = 0L;
		Player_attributes[META_PLAYER.WIN] = 0L;
		Player_attributes[META_PLAYER.LOSE] = 0L;
		Player_attributes[META_PLAYER.DRAW] = 0L;
		Player_attributes[META_PLAYER.SCORE_RANKING] = 0L;

		Room_attributes = new();
		Room_attributes[META_PLAYER.SCORE_RANKING] = 0L;

		Room_options = new();
		Room_options.IsOpen = true;
		Room_options.MaxPlayers = 2;
		Room_options.CustomRoomProperties = Room_attributes;

		PhotonNetwork.autoCleanUpPlayerObjects = true;
		PhotonNetwork.autoJoinLobby = false;
		PhotonNetwork.SetPlayerCustomProperties(Player_attributes);
		//PhotonHandler.StopFallbackSendAckThread();
	}
	public void Update()
	{
	}

	public override void OnConnectedToPhoton()
	{
		print("Connection Succeed.");
	}
	public override void OnDisconnectedFromPhoton()
	{
		print("Disconnected.");
	}
	public override void OnJoinedLobby()
	{
		print("Joined to the lobby.");
	}
	public override void OnLeftLobby()
	{
		print("Left from the lobby.");
	}
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		print(cause);
	}
	public override void OnConnectionFail(DisconnectCause cause)
	{
		print(cause);
	}

	public static void Connect()
	{
		PhotonNetwork.ConnectUsingSettings(GAME_VERSION);
	}

	public static void LoadScene(string scene_name)
	{
		PhotonNetwork.LoadLevel(scene_name);
		print("Going to " + scene_name + ".");
	}

	public static PhotonPlayer SearchPlayer(PlyerPredicate predicate)
	{
		if (PhotonNetwork.connected)
		{
			var plist = PhotonNetwork.otherPlayers;
			foreach (var player in plist)
			{
				if (predicate(player))
				{
					return player;
				}
			}
		}

		return null;
	}
	public static bool AnyOfPlayer(PlyerPredicate predicate)
	{
		return (SearchPlayer(predicate) is not null);
	}
	public static bool PlayerExists(int id)
	{
		var check = PhotonPlayer.Find(id);
		return check is not null;
	}
	public static bool PlayerExists(string name)
	{
		return AnyOfPlayer((PhotonPlayer player) => (player.NickName == name));
	}
}
