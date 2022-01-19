using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class DestroyByTimeExtend : MonoBehaviour
{
	public float lifeTime;
	// Use this for initialization
	void Start()
	{
		Destroy(gameObject, lifeTime);
		GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.25f);
	}
}
