using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;
using UnityEngine.UIElements;

public class GameSystem : PunBehaviour
{
	[Header("Player (Mine)")]
	public GameObject Player; // My Playing Entity
	[Header("Props")]
	public GameObject Player_prefab;
	public Text Text_countdown;
	public GameObject UI_victory;
	[Header("Spaces")]
	public GameObject[] Player_spawn_points;
	public MoleHole[] Enemy_spawn_points;

	public GameObject CreatePlayerObject(Vector3 position, Quaternion angle)
	{
		var player = PN.Instantiate(Player_prefab.name, position, angle, photonView.group);

		return player;
	}
	private void ForceVictory()
	{

	}
	public void BeekOnGround(Vector3 position)
	{
		// Create a particle effect
		RaycastHit hit;

		var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider is not null) {
				photonView.RPC("BeekOnGround", PhotonTargets.All, hit.point);

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
					hit.rigidbody.AddForceAtPosition(ray.direction * 3f, hit.point);
				}
			}
		}
	}
	public void BeekOnGround(float x, float y, float z) => BeekOnGround(new Vector3(x, y, z));
	public void TryAttack()
	{
		
	}

	void Awake()
	{
		UI_victory.SetActive(false);
	}
	void Start()
	{
		if (!PN.connected)
		{
			PN.Reconnect();
		}

		Enemy_spawn_points = FindObjectsOfType<MoleHole>();

		photonView.TransferOwnership(PN.player);

		// Game System Management
		if (PN.isMasterClient)
		{
			// Setup the board
			
			// Create players (2 players)

			// Change the phase

			// Start timer

			// 
		}
	}
	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (photonView.isMine)
			{
				// Attack
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
		SceneManager.LoadScene("SceneSignIn");
	}
}
