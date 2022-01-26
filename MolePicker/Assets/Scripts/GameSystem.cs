using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using PN = PhotonNetwork;

public class GameSystem : PunBehaviour
{
	void Start()
	{
		if (!PN.connected)
		{
			PN.Reconnect();
		}
	}
	public override void OnDisconnectedFromPhoton()
	{

	}
}
