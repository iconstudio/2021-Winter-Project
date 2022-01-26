using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using PN = PhotonNetwork;

public class IntroSystem : PunBehaviour
{
	public float Period = 3f;

	IEnumerator GotoMain()
	{
		yield return new WaitForSecondsRealtime(Period);

		SceneManager.LoadScene("SceneSignIn");
	}

	void Start()
	{
		GameManager.Connect();
	}

	public override void OnConnectedToPhoton()
	{
		StartCoroutine(GotoMain());
	}
}
