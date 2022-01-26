using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using PN = PhotonNetwork;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameManager : PunBehaviour
{
	private static GameManager _instance;
	public static GameManager Instance
	{
		private set => _instance = value;
		get => _instance;
	}

	public enum META_PLAYER : uint
	{
		NUM_OF_GAMES = 20,
		WIN, LOSE, DRAW,
		SCORE_RANKING = 30
	}
	public delegate bool PlyerPredicate(PhotonPlayer player);
	public Hashtable Player_attributes;
	public Hashtable Room_attributes;
	public RoomOptions Room_options;
	public static readonly long[] SEARCE_SCORE_DIFFS = new long[] { 100, 250, 400 };

	public static void Connect()
	{
		print("Connecting to the server...");
		PN.ConnectUsingSettings(PN.gameVersion);
	}

	void Awake()
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
	void Start()
	{
		PN.gameVersion = "1.0.0";

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

		PN.automaticallySyncScene = true;
		PN.autoCleanUpPlayerObjects = true;
		PN.autoJoinLobby = false;
		PN.SetPlayerCustomProperties(Player_attributes);
	}
	void Update()
	{

	}
}
