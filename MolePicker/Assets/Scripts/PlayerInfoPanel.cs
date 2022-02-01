using System;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class PlayerInfoPanel : PunBehaviour
{
	public Text Text_name, Text_score;
	public bool Show = false;

	public PhotonPlayer Owner
	{
		set
		{
			if (value is not null)
			{
				Show = true;
				Text_name.gameObject.SetActive(true);
				Text_score.gameObject.SetActive(true);

				photonView.TransferOwnership(value);
			}
			else
			{
				Show = false;
				Text_name.gameObject.SetActive(false);
				Text_score.gameObject.SetActive(false);
			}
		}
		get => photonView.owner;
	}

	void Update()
	{
		var owner = photonView.owner;
		if (Show)
		{
			Text_name.text = owner.NickName + "(" + owner.ID + ")";
			Text_score.text = "Score: " + owner.GetScore();
		}
	}
}
