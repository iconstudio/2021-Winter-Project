using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class IntroTextWave : MonoBehaviour
{
	public TextMesh mtext;
	private Vector3 mPosition, mTargetPosition;

	private class Phase
	{
		public float mTime;
		public float mPeriod;

		public Phase(float pPeroid)
		{
			mPeriod = pPeroid;
		}
	}

	private Phase JUMP_IN, JUMPING, STAY, JUMP_OUT;

	public void Awake()
	{
		mtext = GetComponent<TextMesh>();
		mPosition = transform.position;
		mTargetPosition = transform.position;
		mTargetPosition.y -= 30f;

		JUMP_IN = new(0.3f);
		JUMPING = new(1f);
		STAY = new(1.5f);
		JUMP_OUT = new(0.5f);
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
