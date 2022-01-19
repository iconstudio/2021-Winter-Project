using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class GameManager : PunBehaviour
{
	public enum PHASE
	{
		READY, GAME, FADEOUT, DONE
	}
	public PHASE Phase = PHASE.READY;
	public Camera Camera;
	public float Camera_speed = 10f;

	public void Awake()
	{
		Camera = FindObjectOfType<Camera>();
	}
	public void Start()
	{

	}
	public void Update()
	{
		if (Camera is null)
		{
			return;
		}

		switch (Phase)
		{
			case PHASE.READY:
				{

				}
				Camera = Camera.main;
				break;

			case PHASE.GAME:
				break;

			case PHASE.DONE:
				break;
		}
	}
}
