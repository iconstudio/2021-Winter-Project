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

	public delegate bool PlyerPredicate(PhotonPlayer player);
	public RoomOptions Room_options;
	public static Color Player_color_1 = new(246f, 255f, 255f);
	public static Color Player_color_2 = new(2548f, 54f, 30f);

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

		Room_options = new();
		Room_options.IsOpen = true;
		Room_options.MaxPlayers = 2;

		PN.automaticallySyncScene = true;
		PN.autoCleanUpPlayerObjects = true;
		PN.autoJoinLobby = false;
	}
	void Update()
	{

	}
}
