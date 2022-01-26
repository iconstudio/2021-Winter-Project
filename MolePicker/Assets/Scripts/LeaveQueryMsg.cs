using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LeaveQueryMsg : MonoBehaviour
{
	public Image FadeBackground;
	public RectTransform PanelTransform;

	public enum PHASE
	{
		OPENING, IDLE, CLOSING
	}
	public PHASE Phase = PHASE.OPENING;
	public float Lifetime = 0f;
	public float Duration = 0.3f;
	public float Alpha;

	public void OnClickYesButton()
	{
		Phase = PHASE.CLOSING;
	}
	public void OnClickNoButton()
	{
		Phase = PHASE.CLOSING;
	}

	void Awake()
	{
		Alpha = FadeBackground.color.a;
	}
	void Start()
	{
		PanelTransform.localScale = new Vector3(0f, 0f, 0f);
	}
	void OnEnable()
	{
		Phase = PHASE.OPENING;
	}
	void OnDisable()
	{
		Lifetime = 0f;
	}
	void Update()
	{
		var ratio = Lifetime / Duration;
		FadeBackground.color = new Color(0f, 0f, 0f, Alpha * ratio);
		var scale = ratio * ratio;
		PanelTransform.localScale = new Vector3(scale, scale, scale);

		switch (Phase)
		{
			case PHASE.OPENING:
			{
				Lifetime = Mathf.Clamp(Lifetime + Time.deltaTime, 0f, Duration);

				if (Duration == Lifetime)
				{
					Phase = PHASE.IDLE;

					gameObject.SetActive(true);
				}
			}
			break;

			case PHASE.IDLE:
			{

			}
			break;

			case PHASE.CLOSING:
			{
				Lifetime = Mathf.Clamp(Lifetime - Time.deltaTime, 0f, Duration);

				if (Lifetime == 0f)
				{
					Phase = PHASE.IDLE;

					gameObject.SetActive(false);
				}
			}
			break;
		}
	}
}
