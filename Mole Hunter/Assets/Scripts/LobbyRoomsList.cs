using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class LobbyRoomsList : PunBehaviour
{
	public GameObject Description;
	public RoomInfo[] Rooms;
	public int Room_page = 0;
	public GameObject ItemPanel;
	public GameObject[] Items;

	private void TurnOffAllRoomSlots()
	{
		for (int i = 0; i < Items.Length; i++)
		{
			var item = Items[i];
			item.SetActive(false);
		}
	}
	private void TakeRoomsList()
	{
		Rooms = PN.GetRoomList();
	}
	public void OnClickLTButton()
	{
		if (PN.connectedAndReady && PN.insideLobby)
		{
			if (0 < Room_page)
				Room_page--;
		}
	}
	public void OnClickRTButton()
	{
		if (PN.connectedAndReady && PN.insideLobby)
		{
			var rooms_number = Rooms.Length;
			var last_index = rooms_number % 4;
			if (Room_page < last_index - 1)
				Room_page++;
		}
	}

	void Awake()
	{
		Description?.SetActive(false);
	}
	void Start()
	{
		TurnOffAllRoomSlots();
	}
	public override void OnReceivedRoomListUpdate()
	{
		if (PN.connectedAndReady && PN.insideLobby)
		{
			TakeRoomsList();
			TurnOffAllRoomSlots();

			var rooms_number = Rooms.Length;
			if (0 < rooms_number)
			{
				Description.SetActive(false);
				ItemPanel.SetActive(true);

				if (0 < rooms_number)
				{
					for (int i = 0; i < 4; i++)
					{
						var j = Room_page % 4 + i;
						if (rooms_number <= j) break;

						var Slot = Items[i];
						Slot.SetActive(true);
						var caption = Slot.GetComponentInChildren<Text>();

						var Item_room = Rooms[j];
						caption.text = "<" + Item_room.Name + ">\nMembers: " + Item_room.PlayerCount + " / " + Item_room.MaxPlayers;
					}
				}
			}
			else
			{
				Description.SetActive(true);
				ItemPanel.SetActive(false);
			}
		}
	}
}
