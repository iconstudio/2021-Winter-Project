using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class IntroTextWave : MonoBehaviour
{
	public TextMesh mtext;
	private Vector3 mPosition, mTargetPosition;

	public void Awake()
	{
		mtext = GetComponent<TextMesh>();
		mPosition = transform.position;
		mTargetPosition = transform.position;
		mTargetPosition.y -= 30f;
	}
	// Start is called before the first frame update
	void Start()
	{
		
	}
	// Update is called once per frame
	void Update()
	{
		var deltas = Time.deltaTime;
		
		transform.position = mPosition * Mathf.PingPong(Time.time, 1f);
	}
}
