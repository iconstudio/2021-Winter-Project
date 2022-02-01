using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Photon;
using PN = PhotonNetwork;

public class Player : PunBehaviour
{
	[Header("View")]
	public int Player_index = 0;
	public Color Player_color = Color.white;

	[Header(("Props"))]
	public GameSystem System;
	public Camera My_camera;
	public TMP_Text Text_score, Text_time;
	public GameObject My_shovel;
	public Rigidbody My_body;

	[Header(("Attributes"))]
	public float Move_speed = 50f;
	public float Mouse_x, Mouse_y;
	public float Lifetime = 0f, Durations = 0f;
	public int Score = 0;

	[PunRPC]
	public void UpdateScore(float time, float dur)
	{
		Lifetime = time;
		Durations = dur;
		Score = PN.player.GetScore();
	}

	void OnEnable()
	{
		System = GameObject.Find("GameSystem").GetComponent<GameSystem>();
	}
	void Update()
	{
		var life = System.GetGameElapsedTime();
		var dur = System.GetGameDuration();
		object[] varis = { life, dur };

		if (photonView.isMine)
		{


			photonView.RPC("UpdateInfos", PhotonTargets.All, varis);
		}
		
		Text_score.text = "Score: " + PN.player.GetScore();
		Text_time.text = "Time: " + Mathf.FloorToInt(life) + " / " + dur;
	}
    private void FixedUpdate()
    {
		var input_x = Input.GetAxisRaw("Horizontal");
		var input_y = Input.GetAxisRaw("Vertical");

		var force = new Vector3(input_x, 0f, input_y);
		transform.TransformDirection(force);

		if (My_body.velocity.magnitude <= Move_speed)
        {
			My_body.;
        }
		My_body.AddRelativeForce(force);
    }
}
