using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;

public class LobbyPlayersList : PunBehaviour
{
	public RectTransform Players_list_view;
	
	public void Awake()
	{
		Players_list_view = GameObject.Find("PlayersListContent").GetComponent<RectTransform>();

	}

	// Update is called once per frame
	void Update()
	{

	}
}
