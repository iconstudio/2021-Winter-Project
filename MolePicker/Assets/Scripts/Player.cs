using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Photon;
using PN = PhotonNetwork;

public class Player : PunBehaviour
{
	[Header(("Props"))]
	public GameSystem System;
	public Camera My_camera;
	public TMP_Text Text_score, Text_time;

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
}
