using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class GameManager : PunBehaviour
{
	public float Game_begin_time = 3.5f;
	public float Game_time = 0f, Game_duration = (float)System.TimeSpan.FromMinutes(3).TotalSeconds;
	public enum PHASE
	{
		READY, GAME, COMPLETE, DONE
	}
	private PHASE __Phase = PHASE.READY;
	public PHASE Phase
	{
		get
		{
			return __Phase;
		}
		set
		{
			if (__Phase != value)
			{
				print("The state of game is changed to " + value + " from " + __Phase);
				__Phase = value;
			}
		}
	}

	public GameObject Camera;
	public float Camera_speed = 3f;

	public void Awake()
	{
		Camera = GameObject.Find("GameCamera");
		print("Camera is " + Camera.ToString());
	}
	public void Start()
	{
		Game_time = 0f;

		StartCoroutine(GameProcess());
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
			break;

			case PHASE.GAME:
			{
				var deltas = Time.deltaTime;

				var camvelocity = new Vector3(0f, Camera_speed, 0f);
				this.Camera.transform.TransformVector(camvelocity);
				this.Camera.transform.Translate(camvelocity * deltas);
			}
			break;

			case PHASE.DONE:
			{

			}
			break;
		}
	}
	public void OnGUI()
	{
		
	}

	private IEnumerator GameProcess()
	{
		yield return new WaitForSeconds(Game_begin_time);

		Phase = PHASE.GAME;

		while (Game_time < Game_duration)
		{
			Game_time += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		Phase = PHASE.COMPLETE;
	}
}
