using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class RoomInSystem : PunBehaviour
{
	public GameObject UI_msg_leave;
	public Text Text_my_name, Text_oppo_name;
	public int Players_number = 0;
	public bool Game_started = false;

	public void OnClickLeaveButton()
	{
		if (!UI_msg_leave.GetActive())
		{
			UI_msg_leave.SetActive(true);
		}
	}
	public void OnClickGameStartButton()
	{
		if (PN.inRoom)
		{
			var players_count = PN.room.PlayerCount;
			if (PN.offlineMode || 0 < players_count)
			{
				print("Game is started,");
				PN.LoadLevel("SceneGame");
			}
		}
	}
	public void OnClickYesToLeaveButton()
	{
		if (PN.inRoom)
		{
			if (PN.isMasterClient)
			{
				if (1 < Players_number)
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

	void Awake()
	{
		UI_msg_leave.SetActive(false);

		Text_my_name.text = PN.playerName;
		Text_oppo_name.text = "";
	}
	void Start()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}
		Game_started = false;
	}

	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Players_number = PN.room.PlayerCount;
		Text_oppo_name.text = newPlayer.NickName;
	}
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if (otherPlayer == PN.player)
		{
			//
		}
		else
		{
			Text_oppo_name.text = "";
		}
		Game_started = false;
	}
	public override void OnLeftRoom()
	{
		if (PN.connectedAndReady)
		{
			SceneManager.LoadScene("SceneLobby");
		}
		else
		{
			SceneManager.LoadScene("SceneSignIn");
		}
	}
}