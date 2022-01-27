using System;
using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;
using ExitGames.Client.Photon.StructWrapping;

public class LobbySystem : PunBehaviour
{
	public GameObject UI_msg_leave;
	public Text Inputfield_title;

	public void CreateOwnRoom(string title)
	{
		if (!PN.inRoom)
		{
			PN.CreateRoom(title, GameManager.Instance.Room_options, PN.lobby);
		}
	}

	public void OnClickCreateButton()
	{
		var Title = Inputfield_title.text.Trim();
		if (0 < Title.Length)
		{
			CreateOwnRoom(Title);
		}
	}
	public void OnClickSearchButton()
	{
		if (PN.connectedAndReady)
		{
			var roomsCount = PN.countOfRooms;
			if (0 < roomsCount)
			{
				PN.JoinRandomRoom();
			}
			else
			{
				OnClickCreateButton();
			}
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
	void OnEnable()
	{
		if (!PN.connected)
		{
			GameManager.Connect();
		}
	}

	public override void OnCreatedRoom()
	{
		PN.SetMasterClient(PN.player);
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
	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		// if can't find any proper room
		//OnClickCreateButton();
	}
	public override void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("SceneSignIn");
	}
}
