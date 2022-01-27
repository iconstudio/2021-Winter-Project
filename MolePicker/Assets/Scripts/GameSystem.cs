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

public class GameSystem : PunBehaviour
{
	[Header("Player (Mine)")]
	public GameObject Player; // My Playing Entity
	public int Score_rank; // Backup
	[Header("Props")]
	public Camera Camera_main;
	public GameObject Player_prefab;
	public TextMeshProUGUI Text_countdown;
	public GameObject UI_victory;
	public GameObject Prefab_beek;
	[Header("Intro")]
	public Camera Camera_intro;
	public bool Intro_done = false;
	public Vector3 Intro_cam_begin = new(0f, 400f, -10f);
	public Vector3 Intro_cam_target = new(0f, 40f, -10f);
	public float Intro_durations = 1.8f;
	public float Intro_time = 0f;
	[Header("Spaces")]
	public GameObject[] Player_spawn_points;
	public MoleHole[] Enemy_spawn_points;

	[PunRPC]
	public void GameStart()
	{
		if (photonView.isMine)
		{
			Camera_intro.gameObject.SetActive(false);

			var my_spawner = Player_spawn_points[0];
			var my_look = Quaternion.identity;
				//Quaternion.LookRotation(new Vector3(0f, 1f, 0f));

			Player = CreatePlayerObject(my_spawner.transform.position, my_look);
			//Camera_main = 
		}
	}
	public GameObject CreatePlayerObject(Vector3 position, Quaternion angle)
	{
		var player = PN.Instantiate(Player_prefab.name, position, angle, photonView.group);

		return player;
	}
	public IEnumerator IntroPresentation()
	{
		while (true)
		{
			var ratio = Mathf.Min(1f, Intro_time / Intro_durations);
			Camera_intro.transform.position = Vector3.Slerp(Intro_cam_begin, Intro_cam_target, ratio);

			if (Intro_durations <= Intro_time)
			{
				Intro_done = true;
				photonView.RPC("GameStart", PhotonTargets.AllViaServer);
				break;
			}
			Intro_time += Time.deltaTime;

			yield return new WaitForEndOfFrame();
		}
	}
	public IEnumerator MolesAppearScript()
	{
		while (true)
		{
			var choice = (int)Random.Range(0, Enemy_spawn_points.Length);
			var selection = Enemy_spawn_points[choice];

			//selection.SendMessage("");

			yield return new WaitForSeconds(0.1f);
		}
	}
	private void ForceVictory()
	{

	}
	public void TryAttack()
	{

	}

	void Start()
	{
		UI_victory.SetActive(false);

		if (!PN.connected)
		{
			PN.Reconnect();
		}

		Camera_intro.gameObject.SetActive(true);

		if (photonView.isMine)
		{
			Score_rank = PN.player.GetScore();
			PN.player.SetScore(0);
		}

		// Game System Management
		if (PN.isMasterClient)
		{
			StartCoroutine(IntroPresentation());
			// Setup the board

			// Create players (2 players)

			// Change the phase

			// Start timer

			// 
		}
		else
		{
		}
	}
	void Update()
	{
		if (Intro_done)
		{
			RaycastHit hit;

			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider is not null)
				{
					if (Input.GetMouseButtonDown(0))
					{
						if (photonView.isMine)
						{
							// Attack
							PN.player.AddScore(10);
						}

						// Create a particle effect
						PN.Instantiate(Prefab_beek.name, hit.point, Quaternion.identity, photonView.group);
					}

					var obj = hit.collider.gameObject;
					if (obj is not null)
					{
						var plc = obj.GetComponent<Player>();
						if (obj.GetType() == typeof(Player))
						{

						}
					}

					if (hit.rigidbody != null)
					{
						//hit.rigidbody.AddForceAtPosition(ray.direction * 3f, hit.point);
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

	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{

	}
	public override void OnDisconnectedFromPhoton()
	{
		PN.player.SetScore(Score_rank);
		SceneManager.LoadScene("SceneSignIn");
	}
}
