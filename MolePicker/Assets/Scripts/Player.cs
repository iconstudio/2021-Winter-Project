using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class Player : PunBehaviour
{
	[Header(("Props"))]
	public Animator My_animator;
	public Camera My_camera;
	public Rigidbody My_weight;

	[Header(("Attributes"))]
	public float Move_speed;

	void Start()
	{
		photonView.ownershipTransfer = OwnershipOption.Takeover;
	}
	void OnEnable()
	{
		if (!photonView.isMine)
		{
			My_camera.gameObject.SetActive(false);
			My_weight.useGravity = false;
		}
		else
		{

		}
	}
	void OnDisable()
	{
		
	}
	void FixedUpdate()
	{
		if (!photonView.isMine)
			return;

		print("Player updating");
		var deltas = Time.deltaTime;

		var input_x_raw = Input.GetAxisRaw("Horizontal");
		var input_z_raw = Input.GetAxisRaw("Vertical");

		var direction = new Vector3(input_x_raw, 0f, input_z_raw);
		transform.TransformDirection(direction);

		var force = Move_speed * direction * 100f;
		print("Force: " + force);
		My_weight.AddForce(force);
	}

	public override void OnDisconnectedFromPhoton()
	{
		if (photonView.isMine)
		{
		}
	}
}
