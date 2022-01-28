using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

using TMPro;
using Photon;
using PN = PhotonNetwork;
using Unity.VisualScripting;
using UnityEditor;

public class GameSystem : PunBehaviour
{
	[Header("Players")]
	public PhotonPlayer Opponent;
	public GameObject Player1, Player1_camera;
	public GameObject Player2, Player2_camera;
	public int ScoreStart = 0;
	[Header("Props")]
	public Camera Camera_main;
	public GameObject Player_prefab;
	public GameObject UI_victory;
	public GameObject Prefab_beek;
	public TextMeshProUGUI Text_description;
	[Header("Intro")]
	public Camera Camera_intro;
	public bool Intro_done = false;
	public Vector3 Intro_cam_begin = new(0f, 200f, -50f);
	public Vector3 Intro_cam_target = new(0f, 40f, -15f);
	public float Intro_durations = 1.8f;
	public float Intro_time = 0f;
	[Header("Game")]
	public float Game_durations = 60f;
	public float Game_time = 0f;
	[Header("Spaces")]
	public GameObject[] Player_spawn_points;
	public MoleHole[] Enemy_spawn_points;


	[PunRPC]
	public void GameComplete()
	{
		print("The game is ended now.");
		UI_victory.SetActive(true);

		var score = ScoreStart;
		var new_score = PN.player.GetScore();

		Text_description.text = "Score is " + (score).ToString();
		if (0 < new_score)
			Text_description.text += " + " + new_score;
	}
	[PunRPC]
	public void UpdateGameTimes(object time)
	{
		Game_time = (float)time;
	}
	[PunRPC]
	public void GameStart()
	{
		Intro_done = true;

		if (PN.isMasterClient)
		{
			Player1_camera.gameObject.SetActive(true);
			Player2_camera.gameObject.SetActive(false);
			Camera_main = Player1_camera.GetComponent<Camera>();

			var view = Player1.GetComponent<PhotonView>();
			view.TransferOwnership(PN.player);
			StartCoroutine(MolesAppearScript());
		}
		else
		{
			Player1_camera.gameObject.SetActive(false);
			Player2_camera.gameObject.SetActive(true);
			Camera_main = Player2_camera.GetComponent<Camera>();

			var view = Player2.GetComponent<PhotonView>();
			view.TransferOwnership(PN.player);
		}

		Camera_intro.gameObject.SetActive(false);
	}
	[PunRPC]
	public void CreateMole(Vector3 position, MoleHole home)
	{
		var new_mole = PN.Instantiate("Mole", position, Quaternion.identity, photonView.group);

		new_mole.GetComponent<Mole>().Home = home;
		home.available = false;
	}
	public IEnumerator IntroPresentation()
	{
		if (photonView.isMine)
		{
			while (true)
			{
				var ratio = Mathf.Min(1f, Intro_time / Intro_durations);
				Camera_intro.transform.position = Vector3.Slerp(Intro_cam_begin, Intro_cam_target, ratio);

				if (Intro_durations <= Intro_time)
				{
					photonView.RPC("GameStart", PhotonTargets.AllViaServer);
					break;
				}
				Intro_time += Time.deltaTime;

				yield return new WaitForEndOfFrame();
			}
		}
	}
	public IEnumerator MolesAppearScript()
	{
		while (true) // on master
		{
			if (Game_durations <= Game_time)
			{
				if (!PN.isMasterClient)
				{
					break;
				}

				while (true)
				{
					if (!photonView.isMine)
					{
						yield return new WaitForEndOfFrame();
					}
					else
					{

						yield break;
					}
				}
			}

			var ratio = Game_time / Game_durations;

			Vector3 position;
			MoleHole home = null;
			GameObject new_mole;
			if (photonView.isMine)
			{
				var repeat = 1;
				if (0.4f < ratio)
				{
					repeat = 2 + (int)(ratio / 0.4) * (int)Random.Range(0f, 2f);
				}

				for (var j = 0; j < repeat; j++)
				{
					for (var i = 0; i < 2; i++)
					{
						var choice = (int)Random.Range(0, Enemy_spawn_points.Length);

						home = Enemy_spawn_points[choice];
						if (home.available)
							break;
					}
					if (home is not null && !home.available)
						yield return new WaitForSeconds(0.01f);

					home.available = false;

					position = home.transform.position;
					new_mole = PN.Instantiate("Mole", position, Quaternion.identity, photonView.group);
					new_mole.GetComponent<Mole>().Home = home;
					print("Creating a mole.");
				}
			};

			var tick = 1f + (1f - ratio) * Random.Range(0.1f, 0.6f) + (1f - ratio) * 0.5f;
			if (0.5 <= ratio)
			{
				tick = Mathf.Max(0.2f, tick * 0.75f);
			}
			yield return new WaitForSeconds(tick);
		}
	}
	public void OnClickLobbyButton()
	{
		PN.LeaveRoom();
		PN.LoadLevel("SceneLobby");
	}
	public float GetGameElapsedTime()
	{
		return Game_time;
	}
	public float GetGameDuration()
	{
		return Game_durations;
	}

	void Start()
	{
		UI_victory.SetActive(false);

		if (!PN.connected)
		{
			PN.Reconnect();
		}

		if (photonView.isMine)
		{
			ScoreStart = PN.player.GetScore();
			PN.player.SetScore(0);

			// Game System Management
			if (PN.isMasterClient)
			{
				StartCoroutine(IntroPresentation());
			}
		}
	}
	void Update()
	{
		if (Intro_done)
		{
			Game_time += Time.deltaTime;

			if (photonView.isMine && Game_durations <= Game_time)
			{
				Game_time = Game_durations;
				
				photonView.RPC("GameComplete", PhotonTargets.All);
			}

			RaycastHit hit;

			var ray = Camera_main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider is not null)
				{
					var hit_obj = hit.collider.gameObject;
					if (hit_obj is not null)
					{
						print("Hit to " + hit_obj.name);

						if (Input.GetMouseButtonDown(0))
						{
							var hit_name = hit_obj.name;
							// Attack
							if (hit_name.Equals("Mole") || hit_name.Equals("Mole(Clone)"))
							{
								PN.player.AddScore(10);
								print("Attack!");
								PN.Destroy(hit_obj);
							}

							// Create a particle effect
							PN.Instantiate(Prefab_beek.name, hit.point, Quaternion.identity, 0);
						}
					}
				}
			}
		}
	}
	void OnCollisionEnter(Collision collision)
	{

	}
	void OnTriggerEnter(Collider other)
	{

	}

	public override void OnLeftRoom()
	{
		if (0f == Game_time)
		{
			PN.player.SetScore(ScoreStart);
		}
		else
		{
			PN.player.AddScore(ScoreStart);
		}
	}
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if (photonView.isMine)
		{
			photonView.RPC("GameComplete", PhotonTargets.All);
		}
	}
	public override void OnDisconnectedFromPhoton()
	{
		SceneManager.LoadScene("SceneSignIn");
	}
}
