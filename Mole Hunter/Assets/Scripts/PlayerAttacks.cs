using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor.Scripting;

using Photon;

/// <summary>
/// 플레이어의 공격을 담당하는 클래스
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerAttacks : PunBehaviour
{
	[Header("States")]
	public bool Attacking = false;

	[Header("Timings")]
	public const float Attack_period = 2f;
	public float Attack_time = 0f;

	public void Start()
	{
		if (MoleHunter.Is_connected)
		{

		}
		else
		{
			throw new Exception("Disconnectded");
		}
	}
	public void Update()
	{
		if (MoleHunter.Is_connected && photonView.owner is not null)
		{
			if (photonView.isMine)
			{

			}
			else
			{

			}
		}
	}
}
