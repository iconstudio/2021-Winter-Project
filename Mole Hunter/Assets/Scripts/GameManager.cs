using System.Collections;

using UnityEngine;

using Photon;
using ExPhoton = ExitGames.Client.Photon;

public class GameManager : PunBehaviour
{
	public float Game_begin_time = 3.5f;
	public float Game_time = 0f, Game_duration = (float)System.TimeSpan.FromMinutes(3).TotalSeconds;
	public enum PHASE
	{
		READY, GAME, COMPLETE, DONE,
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

	public VictoryUI Complete_info;
	public PhotonView PhotonView;
	public GameObject Camera;
	public float Camera_speed = 3f;

	public MoleHole[] Holes;
	public int Holes_number => Holes.Length;

	public void Awake()
	{
		var obj = GameObject.Find("VictoryUI");
		Complete_info = obj?.GetComponent<VictoryUI>();

		Camera = GameObject.Find("GameCamera");
		print("Camera is " + Camera.ToString());

		Holes = FindObjectsOfType<MoleHole>();
		foreach (var hole in Holes)
		{
			hole.SendMessage("Ready");
		}
	}
	public void Start()
	{
		Game_time = 0f;

		StartCoroutine(GameProcess());
	}
	public void Update()
	{
		if (Camera is null || photonView.owner is null)
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

				if (Input.GetMouseButtonDown(0))
				{
					// attack
					Ray mouse_clicker = new();
					mouse_clicker.direction = this.Camera.transform.forward; //Vector3.forward;
				}
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
		// 게임 준비

		yield return new WaitForSeconds(Game_begin_time)

		// 게임 시작
		Phase = PHASE.GAME;

		while (Game_time < Game_duration)
		{
			Game_time += Time.deltaTime;

			// FixedUpdate와 같은 시간대
			yield return new WaitForEndOfFrame();
		}

		// 게임 끝
		Phase = PHASE.COMPLETE;


	}
}
