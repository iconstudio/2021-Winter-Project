using System;
using System.Collections;

using UnityEngine;

using Photon;

public class IntroManager : PunBehaviour
{
	public float Period = 3f;

	IEnumerator GotoMain()
	{
		yield return new WaitForSecondsRealtime(Period);

		MoleHunter.LoadScene("SceneMain");
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
		StartCoroutine(GotoMain());
	}
}
