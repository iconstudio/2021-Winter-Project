using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;
using TMPro;
using ExitGames.Client.Photon.StructWrapping;

public class MainManager : PunBehaviour
{
	public enum PHASE
	{
		SIGNIN, LOBBY, IN_ROOM
	}
	public PHASE Phase
	{
		get => default;
		set
		{
			switch (value)
			{
				case PHASE.SIGNIN:
				{
					UI_signin.SetActive(true);
					UI_lobby.SetActive(false);
					UI_room.SetActive(false);
					UI_rooms_view.SetActive(false);
					UI_msg_leave_room.SetActive(false);
					Button_connections.gameObject.SetActive(true);
				}
				break;

				case PHASE.LOBBY:
				{
					UI_signin.SetActive(false);
					UI_lobby.SetActive(true);
					UI_room.SetActive(false);
					UI_rooms_view.SetActive(true);
					UI_msg_leave_room.SetActive(false);
					Button_connections.gameObject.SetActive(true);
				}
				break;

				case PHASE.IN_ROOM:
				{
					UI_signin.SetActive(false);
					UI_lobby.SetActive(false);
					UI_room.SetActive(true);
					UI_rooms_view.SetActive(false);
					UI_msg_leave_room.SetActive(false);
					Button_connections.gameObject.SetActive(false);
				}
				break;
			}
		}
	}

	private const string MsgSignIn = "Please enter your ID.";

	private GameObject UI_signin, UI_lobby, UI_room, UI_rooms_view, UI_msg_leave_room;
	public TMP_InputField Inputfield_nickname;
	public Button Button_connections;
	public Text Text_button_connections, Text_server;
	public TextMeshProUGUI Text_notification, Text_network;

	public void Awake()
	{
		UI_signin = GameObject.Find("SignInUI");
		UI_lobby = GameObject.Find("LobbyUI");
		UI_room = GameObject.Find("RoomInUI");
		UI_rooms_view = GameObject.Find("RoomsListViewUI");
		UI_msg_leave_room = GameObject.Find("LeaveQueryMsgUI");

		var obj = GameObject.Find("NickNameInputField");
		Inputfield_nickname = obj.GetComponent<TMP_InputField>();

		obj = GameObject.Find("NickNameNotification");
		Text_notification = obj.GetComponent<TextMeshProUGUI>();
		Text_notification.text = MsgSignIn;

		obj = GameObject.Find("NetworkNotification");
		Text_network = obj.GetComponent<TextMeshProUGUI>();
		Text_network.text = "Server is connected.";

		obj = GameObject.Find("ServerStatusText");
		Text_server = obj.GetComponent<Text>();

		obj = GameObject.Find("MainConnectButton");
		Button_connections = obj.GetComponent<Button>();
		Text_button_connections = Button_connections.GetComponentInChildren<Text>();

		Phase = PHASE.SIGNIN;
	}
	public void Start()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}

		if (PN.connected)
		{
			Text_button_connections.text = "Disconnect";
		}
	}
	public void Update()
	{
		if (PN.connected)
		{
			if (Input.GetButtonDown("Submit"))
			{
				LoginAndJoinLobbyProceed();
			}

			switch (Phase)
			{
				case PHASE.SIGNIN:
				{

				}
				break;

				case PHASE.LOBBY:
				{
					UpdateServerStatus();
				}
				break;

				case PHASE.IN_ROOM:
				{
					UpdateServerStatus();

					if (!PN.inRoom)
					{
						Phase = PHASE.SIGNIN;
					}
				}
				break;
			}
		}
	}

	public void LoginAndJoinLobbyProceed()
	{
		if (PN.connectedAndReady
			&& !PN.insideLobby
			&& Inputfield_nickname is not null)
		{
			var My_nickname = Inputfield_nickname.text.Trim();
			if (0 < My_nickname.Length)
			{
				var player_check = GameManager.PlayerExists(My_nickname);
				if (!player_check)
				{
					PlayerPrefs.SetString("NickName", My_nickname);
					PN.playerName = My_nickname;

					print("Player's nickname is " + My_nickname + ".");

					PN.JoinLobby(GameManager.Lobby_options);
				}
				else
				{
					StartCoroutine(MsgOnSignInFailed());
				}
			}
		}
	}
	public IEnumerator MsgOnSignInFailed()
	{
		Text_notification.text = "Error Appeared: Cannot sign in!";
		yield return new WaitForSeconds(3f);

		Text_notification.text = MsgSignIn;
		yield return null;
	}
	public IEnumerator MsgOnJoinFailed()
	{
		Text_notification.text = "Error Appeared: Cannot join the lobby!";
		yield return new WaitForSeconds(3f);

		Text_notification.text = MsgSignIn;
		yield return null;
	}
	public void UpdateServerStatus()
	{
		print(Text_server);
		if (PN.connectedAndReady && Text_server is not null && Text_server.IsActive())
		{
			Text_server.text = "Version: " + PN.gameVersion
			+ "\nTotal players: " + PN.countOfPlayers
			+ "\nPlayers in lobby: " + PN.countOfPlayersOnMaster
			+ "\nPlayers in room: " + PN.countOfPlayersInRooms
			+ "\nName of lobby: " + PN.lobby.Name
			+ "\nName of room: " + PN.room.Name
			;
		}
	}
	public void OnClickMainButton()
	{
		if (PN.connected)
		{
			if (PN.insideLobby)
				PN.LeaveLobby();
			if (PN.inRoom)
				PN.LeaveRoom();

			PN.Disconnect();
		}
		else
		{
			GameManager.Connect();
		}
	}
	public void OnClickCreateRoomButton()
	{
		if (!PN.inRoom)
		{
			var room_name = PN.playerName + "Room";
			var result = PN.CreateRoom(room_name
				, GameManager.Room_options
				, null);
		}
	}
	public void OnClickSearchButton()
	{
		if (PN.connectedAndReady)
		{
			var roomsCount = PN.countOfRooms;
			if (0 < roomsCount)
			{
				var rooms = PN.GetRoomList();
				List<RoomInfo> room_pool = new();

				var key = GameManager.META_PLAYER.SCORE_RANKING;
				var player_score = (long)(PN.player.CustomProperties[key]);

				foreach (var diff in GameManager.SEARCE_SCORE_DIFFS)
				{
					foreach (var room in rooms)
					{
						long target_score;
						if (room.PlayerCount < room.MaxPlayers
							&& room.CustomProperties.TryGetValue(key
							, out target_score))
						{
							var score_begin = Math.Max(target_score - diff, 0);
							var score_end = target_score + diff;
							if (score_begin <= player_score && player_score < score_end)
							{
								room_pool.Add(room);
							}
						}
					}

					if (0 < room_pool.Count)
					{
						break;
					}
				}

				room_pool.Sort((RoomInfo lhs, RoomInfo rhs) =>
				{
					long lhs_score;
					lhs.CustomProperties.TryGetValue(key, out lhs_score);

					long rhs_score;
					lhs.CustomProperties.TryGetValue(key, out rhs_score);

					return (int)(rhs_score - lhs_score);
				});

				var room_target = room_pool[0];
				if (PN.JoinRoom(room_target.Name))
				{
					// 
				}
			}

			if (!PN.inRoom)
			{
				var room_name = PN.playerName + "Room";
				var result = PN.CreateRoom(room_name
					, GameManager.Room_options
					, null);

				if (result)
				{
					var my_room = PN.room;
				}
			}
		}
	}
	public void OnClickLeaveRoomButton()
	{
		if (PN.inRoom)
		{
			UI_msg_leave_room.SetActive(true);
		}
	}
	public void OnClickLeaveRoomToOKButton()
	{
		if (PN.inRoom)
		{
			if (PN.isMasterClient)
			{
				if (1 < PN.room.PlayerCount)
				{
					var playerList = PN.otherPlayers;
					var NewMaster = playerList[0];
					if (NewMaster is not null)
					{
						PN.SetMasterClient(NewMaster);
					}
				}
			}

			PN.LeaveRoom();
		}
	}
	public void OnClickGameStartButton()
	{
		if (PN.inRoom)
		{
			var room = PN.room;
			var players_count = PN.room.PlayerCount;
			if (PN.offlineMode || 0 < players_count)
			{
				GameManager.LoadScene("SceneGame");
			}
		}
	}

	public override void OnConnectedToPhoton()
	{
		Text_network.text = "Server is connected.";
		Text_button_connections.text = "Disconnect";

		Phase = PHASE.SIGNIN;
	}
	public override void OnDisconnectedFromPhoton()
	{
		Text_network.text = "Server is disconnected.";
		Text_button_connections.text = "Connect";

		Phase = PHASE.SIGNIN;
	}
	public override void OnCreatedRoom()
	{
		if (PN.inRoom)
		{
			print("Created a room: " + PN.room.Name);
		}
	}
	public override void OnJoinedLobby()
	{
		Phase = PHASE.LOBBY;
	}
	public override void OnJoinedRoom()
	{
		Phase = PHASE.IN_ROOM;
		GameManager.My_room = PN.room.Name;
		print("Joined to a room: " + PN.room.ToString());
	}
	public override void OnReceivedRoomListUpdate()
	{

	}
	public override void OnLeftLobby()
	{
		var status = PN.connectionStateDetailed;
		if (PN.inRoom || status == ClientState.Joining)
		{
			Phase = PHASE.IN_ROOM;
		}
		else if (status == ClientState.Leaving || !PN.connectedAndReady)
		{
			Phase = PHASE.SIGNIN;
		}
	}
	public override void OnLeftRoom()
	{
		Phase = PHASE.LOBBY;
	}
	public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{

	}
	public override void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		print(cause);
	}
}
