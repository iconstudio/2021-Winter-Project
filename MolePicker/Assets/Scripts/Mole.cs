using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;
using PN = PhotonNetwork;

public class Mole : PunBehaviour
{
	public MoleHole Home;
	public Rigidbody Rigid_body;
	public float Standing_time = 0.8f;
	public float Stand_time = 3f;
	public float Stand_duration = 3f;
	public float Sit_time = 0.5f;
	public Vector3 Begin;
	public Vector3 Movement = new Vector3(0f, 4f, 0f);

	void Start()
	{
		if (photonView.isMine)
		{
			Stand_duration = Random.Range(1f, 4f);
			Stand_time = Stand_duration;

			Begin = transform.position;
		}
	}
	void Update()
	{
		if (photonView.isMine)
		{
			float ratio;

			if (0 < Standing_time)
			{
				ratio = Mathf.Min(1f, Standing_time / 0.8f);
				Standing_time -= Time.deltaTime;
				transform.position = Begin + Movement * (1 - ratio);
			}
			else if (0 < Stand_time)
			{
				ratio = Mathf.Min(1f, Stand_time / Stand_duration);
				Stand_time -= Time.deltaTime;
				transform.position = Begin + Movement;
			}
			else if (0 < Sit_time)
			{
				ratio = Mathf.Min(1f, Sit_time / 0.5f);
				Sit_time -= Time.deltaTime;
				transform.position = Begin + Movement * ratio;
			}
			else
			{
				PN.Destroy(gameObject);
			}
		}
	}
	void OnDestroy()
	{
		if (Home is not null)
			Home.available = true;
	}
}
