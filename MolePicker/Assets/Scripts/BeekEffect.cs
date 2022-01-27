using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BeekEffect : Photon.MonoBehaviour
{
	public float Duration = 0.25f;
	public float Life = 0.25f;
	public Light Lantern;

	void Update()
	{
		var color = Lantern.color;
		color.a = (Life / Duration);

		if (Life <= 0)
		{
			Destroy(gameObject);
		}
		else
		{
			Life -= Time.deltaTime;
		}
	}
}
