using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class Player : PunBehaviour
{
	[Header(("Attributes"))]
	public float Move_speed;

	[Header(("Props"))]
	public Animator My_animator;
	public Camera My_camera;
	public Rigidbody My_weight;

	void Awake()
	{
		if (!photonView.isMine)
		{
			// Camera
			My_weight.useGravity = false;
		}
	}
	void Start()
	{
		if (photonView.isMine)
		{

		}
	}
	void FixedUpdate()
	{
		if (!photonView.isMine)
			return;

		var deltas = Time.deltaTime;

		var input_x_raw = Input.GetAxisRaw("Horizontal");
		var input_z_raw = Input.GetAxisRaw("Vertical");

		var direction = new Vector3(input_x_raw, 0f, input_z_raw);
		transform.TransformDirection(direction);

		transform.Translate(Move_speed * deltas * direction);
	}

	public override void OnDisconnectedFromPhoton()
	{

	}
}
