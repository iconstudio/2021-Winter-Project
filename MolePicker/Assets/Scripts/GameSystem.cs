using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using PN = PhotonNetwork;

public class GameSystem : PunBehaviour
{
	[Header("Props")]
	public GameObject VictoryUI;
	[Header("Gaming")]
	public MoleHole[] Enemy_spawn_points;

	private void ForceVictory()
	{

	}

	private void Awake()
	{
		VictoryUI.SetActive(false);
	}

	void Start()
	{
		if (!PN.connected)
		{
			PN.Reconnect();
		}

		Enemy_spawn_points = FindObjectsOfType<MoleHole>();
	}

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		 
	}
	public override void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("SceneSignIn");
	}
}
