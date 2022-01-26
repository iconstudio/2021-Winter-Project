using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon;

/// <summary>
/// 플레이어의 공격을 담당하는 클래스
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerAttacks : PunBehaviour
{
	public bool Attacking = false;
	public const float Attack_period = 2f;
	public float Attack_time = 0f;

	public void Start()
	{

	}
	public void Update()
	{
		if (PhotonNetwork.connected && photonView.owner is not null)
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
