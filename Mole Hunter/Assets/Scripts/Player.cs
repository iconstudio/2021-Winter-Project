using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;
using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class Player : PunBehaviour
{
	public PhotonPlayer Owner;
	public TextMeshPro Text_mesh_pro;

	void Awake()
	{
		Text_mesh_pro = GetComponentInChildren<TextMeshPro>();
	}

	// Start is called before the first frame update
	void Start()
	{
		Owner = PhotonNetwork.player;
	}

	// Update is called once per frame
	void Update()
	{
		if (MoleHunter.Is_connected && photonView.owner is not null)
		{
			if (photonView.isMine)
			{

			}
			else
			{

			}
		}
		else
		{
			Destroy(gameObject);

			if (photonView.owner is not null)
			{
				print(photonView.owner + " is disconnected.");
			}
		}
	}

	public override void OnPhotonInstantiate(PhotonMessageInfo info)
	{
		
	}
}
