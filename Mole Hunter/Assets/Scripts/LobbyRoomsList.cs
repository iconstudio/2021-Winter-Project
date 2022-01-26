using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class LobbyRoomsList : UnityEngine.MonoBehaviour
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

		}
	}

	void Awake()
	{
		Description?.SetActive(false);
	}
	void Start()
	{
		TurnOffAllRoomSlots();

		if (PN.connectedAndReady && PN.insideLobby)
		{
			StartCoroutine(UpdateRooms());
		}
	}
	void Update()
	{

	}
	private IEnumerator UpdateRooms()
	{
		if (PN.connectedAndReady && PN.insideLobby)
		{
			TakeRoomsList();
			TurnOffAllRoomSlots();

			var rooms_number = Rooms.Length;
			if (0 < rooms_number)
			{
				Description?.SetActive(false);

				if (4 < rooms_number)
				{
					var first_index = Room_page % 4 + Room_page * 4;
					var last_index = Mathf.Min(first_index + 4, rooms_number);

					for (int i = first_index; i < last_index; i++)
					{
						var Item_room = Rooms[i];

						var j = i % 4;
						var Slot = Items[j];

						if (Item_room is not null && Slot is not null)
						{
							Slot.SetActive(true);
							var caption = Slot.GetComponentInChildren<Text>();

							caption.text = "<" + Item_room.Name + ">\nMembers: " + Item_room.PlayerCount + " / " + Item_room.MaxPlayers;
						}
						else
						{
							break;
						}
					}
				}
			}
			else
			{
				Description?.SetActive(true);
			}

			yield return new WaitForSeconds(5f);
		}
		else
		{
			yield return null;
		}
	}
}
