using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;
using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class Player : PunBehaviour
{
	public TextMeshPro Text_mesh_pro;

	void Awake()
	{
		Text_mesh_pro = GetComponentInChildren<TextMeshPro>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
