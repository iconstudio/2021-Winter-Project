using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Photon;

class Shovel : PunBehaviour
{
	public Renderer Model_renderer;
	public GameObject Player;

	private void Awake()
	{
		var high_view = Player.GetPhotonView();
		photonView.TransferOwnership(high_view.owner);
	}
	private void Update()
	{
		if (photonView.isMine)
		{
			Model_renderer.transform.position = Vector3.zero;
		}
	}
}
