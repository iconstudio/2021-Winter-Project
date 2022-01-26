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

		GameManager.LoadScene("SceneMain");
	}

	public void Start()
	{
		if (!PhotonNetwork.connected)
		{
			var global = FindObjectOfType<GameManager>();
			if (global != null)
			{
				GameManager.Connect();
			}
			else
			{
				throw new MissingReferenceException("There is no GameManager.");
			}
		}
	}
	public override void OnConnectedToPhoton()
	{
		StartCoroutine(GotoMain());
	}
}
