using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using PN = PhotonNetwork;
using ExitGames.Client.Photon.StructWrapping;
using System.Collections.Generic;

public class LobbySystem : PunBehaviour
{
	public GameObject UI_msg_leave;

	public void CreateOwnRoom()
	{
		if (!PN.inRoom)
		{
			var room_name = PN.playerName + "Room";

			var metaphor = GameManager.META_PLAYER.SCORE_RANKING;
			long my_score;
			if (!PN.player.CustomProperties.TryGetValue<long>(metaphor, out my_score))
			{
				my_score = 0L;
			}

			var my_option = GameManager.Instance.Room_options;
			my_option.CustomRoomProperties[metaphor] = my_score;

			PN.CreateRoom(room_name
				, my_option
				, null);
		}
	}

	public void OnClickCreateButton()
	{
		CreateOwnRoom();
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
				long player_score;
				if (!PN.player.CustomProperties.TryGetValue<long>(key, out player_score))
				{
					player_score = 0;
				}

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
					return; 
				}
			}

			// if can't find any proper room.
			CreateOwnRoom();
		}
	}
	public void OnClickDisconnectButton()
	{
		if (!UI_msg_leave.GetActive())
		{
			UI_msg_leave.SetActive(true);
		}
	}
	public void OnClickYesToLeaveButton()
	{
		if (PN.connected || PN.connecting)
		{
			if (PN.insideLobby)
				PN.LeaveLobby();
			if (PN.inRoom)
				PN.LeaveRoom();

			PN.Disconnect();
		}

		SceneManager.LoadScene("SceneSignIn");
	}

	void Awake()
	{
		UI_msg_leave.SetActive(false);
	}
	void Start()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}
	}

	public override void OnJoinedRoom()
	{
		SceneManager.LoadScene("SceneRoomIn");
	}
	public override void OnLeftLobby()
	{
		UI_msg_leave.SetActive(false);
	}
	public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
	{
		print(codeAndMsg);
	}
	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		//print((string)codeAndMsg[0] + (string)codeAndMsg[1]);
		print(codeAndMsg);
	}
	public override void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("SceneSignIn");
	}
}
