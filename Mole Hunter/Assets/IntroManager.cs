using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon;
using ExPhoton = ExitGames.Client.Photon;
using System;

public class IntroManager : PunBehaviour
{
	public float Period = 3f;

	IEnumerator GotoNextRoom()
	{
		yield return new WaitForSecondsRealtime(Period);

		SceneManager.LoadScene("SceneMain");
	}

	public void Start()
	{
		if (!MoleHunter.Is_connected)
		{
			var global = FindObjectOfType<MoleHunter>();
			if (global != null)
			{
				global.Connect();
			}
			else
			{
				throw new MissingReferenceException("There is no object of 'MoleHunter'.");
			}
		}
	}
	public override void OnConnectedToPhoton()
	{
		StartCoroutine(GotoNextRoom());
	}
}
