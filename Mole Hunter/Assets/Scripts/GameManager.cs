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
	public float Camera_speed = 10f;
	
	public void Awake()
	{
		
	}
	public void Start()
	{
		
	}
}
