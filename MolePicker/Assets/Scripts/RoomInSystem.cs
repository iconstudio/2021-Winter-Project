using System;
using System.Collections;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class RoomInSystem : PunBehaviour
{
	public PhotonPlayer Opponent;
	public PlayerInfoPanel Panel_view_master, Panel_view_others;
	public GameObject UI_msg_leave;
	public Text Text_room_title;
	public GameObject Button_start;
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
		if (PN.connectedAndReady && PN.inRoom && !Game_started)
		{
			var players_count = PN.room.PlayerCount;
			if (PN.offlineMode || 0 < players_count)
			{
				print("Game is started.");
				Game_started = true;
				PN.room.IsVisible = false;
				PN.room.IsOpen = false;
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
				if (1 < PN.room.PlayerCount)
				{
					PN.SetMasterClient(Opponent);
				}
			}

			PN.LeaveRoom();
			UpdatePanels();
		}
	}
	private void UpdatePanels()
	{
		var master = PN.masterClient;
		var player = PN.player;
		if (master is null)
		{
			PN.SetMasterClient(PN.player);
			Panel_view_master.Owner = player;
		}
		else if (master == player)
		{
			Panel_view_master.Owner = player;
			if (Opponent is null)
			{
				Panel_view_others.Owner = null;
			}
			else
			{
				Panel_view_others.Owner = Opponent;
			}
		}
		else
		{
			Panel_view_master.Owner = master;
			Panel_view_others.Owner = player;
		}
	}

	void Awake()
	{
		UI_msg_leave.SetActive(false);
		Game_started = false;
	}
	void OnEnable()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}

		Text_room_title.text = PN.room.Name;

		if (!PN.isMasterClient)
		{
			Opponent = PN.masterClient;
			Button_start.SetActive(false);
		}
		else
		{
			Button_start?.SetActive(true);
		}

		UpdatePanels();
	}

	public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
	{
		if (newMasterClient == PN.player)
		{
			Button_start?.SetActive(true);
		}
		else
		{
			Button_start?.SetActive(false);
		}
		UpdatePanels();
	}
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Opponent = newPlayer;
		UpdatePanels();
	}
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		Opponent = null;
		UpdatePanels();
		Game_started = false;
	}
	public override void OnLeftRoom()
	{
		if (PN.isMasterClient)
		{
			OnClickYesToLeaveButton();
		}

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