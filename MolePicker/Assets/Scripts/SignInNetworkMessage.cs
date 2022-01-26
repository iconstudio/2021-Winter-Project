using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon;
using PN = PhotonNetwork;

public class SignInNetworkMessage : UnityEngine.MonoBehaviour
{
	public Text Text;
	void Update()
	{
		if (PN.connectedAndReady)
		{
			Text.text = "Connected";
		}
		else
		{
			Text.text = "Disconnected";
		}
	}
}
